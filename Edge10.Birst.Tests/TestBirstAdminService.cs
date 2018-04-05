using System;
using NUnit.Framework;
using Moq;
using Edge10.Birst.BirstWebService;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml;
using Edge10.Birst.Utils;

namespace Edge10.Birst.Tests
{
	[TestFixture]
	public class TestBirstAdminService
	{
		private Mock<IBirstConfiguration> _birstConfiguration;
		private Mock<IBirstServiceWrapper> _birstServiceWrapper;
		private Mock<IHttpClientFacade> _httpClient;

		public BirstAdminService TestSubject { get; private set; }

		[SetUp]
		public void SetUp()
		{
			_birstConfiguration  = new Mock<IBirstConfiguration>();
			_birstServiceWrapper = new Mock<IBirstServiceWrapper>();
			_httpClient          = new Mock<IHttpClientFacade>();

			this.TestSubject = new BirstAdminService(_birstConfiguration.Object, _birstServiceWrapper.Object, _httpClient.Object);
		}

		[Test]
		public void Constructor_Throws_On_Null_Arguments()
		{
			Assert.Throws<ArgumentNullException>(() => new BirstAdminService(null, _birstServiceWrapper.Object, _httpClient.Object));
			Assert.Throws<ArgumentNullException>(() => new BirstAdminService(_birstConfiguration.Object, null, _httpClient.Object));
			Assert.Throws<ArgumentNullException>(() => new BirstAdminService(_birstConfiguration.Object, _birstServiceWrapper.Object, null));
		}

		[Test]
		public void CreateUser_Adds_To_Space_Group_And_Product()
		{
			const string newUserLogin    = "newUser";
			const string newUserPassword = "newPassword";

			const string spaceId = "my-space";
			const string group   = "DefaultGroup";

			_birstConfiguration.Setup(x => x.SpaceId).Returns(spaceId);
			_birstConfiguration.Setup(x => x.GroupInSpace).Returns(group);

			var token = SetupLogin();

			TestSubject.CreateUser(newUserLogin, newUserPassword);

			_birstServiceWrapper.Verify(x => x.CreateUser(token, newUserLogin, $"password={newUserPassword}"));
			_birstServiceWrapper.Verify(x => x.AddUserToSpace(token, newUserLogin, spaceId, true));
			_birstServiceWrapper.Verify(x => x.AddUserToGroupInSpace(token, newUserLogin, group, spaceId));
			_birstServiceWrapper.Verify(x => x.AddProductToUser(token, newUserLogin, BirstAdminService.BirstServiceProductId));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("\t")]
		public void CreateUser_Doesnt_Add_To_Group_When_Group_Null(string group)
		{
			var token = SetupLogin();

			const string newUserLogin = "newUser";
			const string newUserPassword = "newPassword";
			const string spaceId = "my-space";

			_birstConfiguration.Setup(x => x.SpaceId).Returns(spaceId);
			_birstConfiguration.Setup(x => x.GroupInSpace).Returns(group);

			this.TestSubject.CreateUser(newUserLogin, newUserPassword);

			_birstServiceWrapper.Verify(x => x.CreateUser(token, newUserLogin, $"password={newUserPassword}"), Times.Once());
			_birstServiceWrapper.Verify(x => x.AddUserToSpace(token, newUserLogin, spaceId, true), Times.Once());
			_birstServiceWrapper.Verify(x => x.AddUserToGroupInSpace(token, newUserLogin, group, spaceId), Times.Never(), "Shouldn't attempt to add to a null group");
			_birstServiceWrapper.Verify(x => x.AddProductToUser(token, newUserLogin, BirstAdminService.BirstServiceProductId), Times.Once());
		}

		[Test]
		public void CreateUser_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.CreateUser("username", "password"));
		}

		[Test]
		public void SetUserPassword_Sets_Password()
		{
			const string username     = "username";
			const string userPassword = "newPassword";
			var token                 = SetupLogin();

			TestSubject.SetUserPassword(username, userPassword);

			_birstServiceWrapper.Verify(x => x.SetUserPassword(token, username, userPassword));
		}

		[Test]
		public void SetUserPassword_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.SetUserPassword("username", "password"));
		}

		[Test]
		public void ExtractCloudConnectorData_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.ExtractCloudConnectorData("space", new CloudConnection[0]));
		}

		[Test]
		public void ExtractCloudConnectorData_Returns_Token_From_Service()
		{
			var authToken   = SetupLogin();
			var connections = new[] { new CloudConnection() };

			_birstServiceWrapper.Setup(b => b.ExtractCloudConnectorData(authToken, "space", connections))
				.Returns("job token");

			var result = this.TestSubject.ExtractCloudConnectorData("space", connections);
			Assert.AreEqual("job token", result);
		}

		[Test]
		public void PublishData_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.PublishData("space", new string[0]));
		}

		[Test]
		public void PublishData_Returns_Token_From_Service()
		{
			var authToken        = SetupLogin();
			var processingGroups = new[] { "processing group" };

			_birstServiceWrapper.Setup(b => b.PublishData(authToken, "space", processingGroups))
				.Returns("job token");

			var result = this.TestSubject.PublishData("space", processingGroups);
			Assert.AreEqual("job token", result);
		}

		[Test]
		public void IsJobComplete_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.IsJobComplete("token"));
		}

		[Test]
		public void IsJobComplete_Returns_Result_From_Service()
		{
			var authToken = SetupLogin();
			
			_birstServiceWrapper.Setup(b => b.IsJobComplete(authToken, "job token"))
				.Returns(true);

			var result = this.TestSubject.IsJobComplete("job token");
			Assert.IsTrue(result);
		}

		[Test]
		public void GetJobStatus_Throws_When_Cant_Authenticate()
		{
			CheckFailedAuthentication(() => TestSubject.GetJobStatus("token"));
		}

		[Test]
		public void GetJobStatus_Returns_Token_From_Service()
		{
			var authToken = SetupLogin();

			var serviceStatus = new StatusResult();
			_birstServiceWrapper.Setup(b => b.GetJobStatus(authToken, "job token"))
				.Returns(serviceStatus);

			var result = this.TestSubject.GetJobStatus("job token");
			Assert.AreEqual(serviceStatus, result);
		}
		
		[Test]
		public void GetJobStatus_Returns_Failed_On_Soap_Exception()
		{
			var authToken = SetupLogin();

			_birstServiceWrapper.Setup(b => b.GetJobStatus(authToken, "job token"))
				.Throws(new SoapException("Oops", XmlQualifiedName.Empty));

			var result = TestSubject.GetJobStatus("job token");
			
			Assert.AreEqual("Failed", result.statusCode);
			Assert.AreEqual("Oops", result.message);
		}

		[Test]
		public async Task GetSSOToken_Returns_Null_If_Token_Request_Fails()
		{
			SetupSettings();

			//use a callback to check that during the post request we have set the required security protocol
			_httpClient.Setup(h => h.PostAsync(new Uri("http://birst/tokengenerator.aspx"), It.Is<FormUrlEncodedContent>(c => c.ReadAsStringAsync().Result == "birst.spaceId=Space_ID&birst.ssopassword=SSO_Password&birst.username=email%40somewhere.com")))
				.Callback(() => Assert.IsTrue(ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12)))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

			Assert.IsNull(await TestSubject.GetSSOToken());
		}

		[Test]
		public async Task GetSSOToken_Returns_Token()
		{
			SetupSettings();

			_httpClient.Setup(h => h.PostAsync(new Uri("http://birst/tokengenerator.aspx"), It.Is<FormUrlEncodedContent>(c => c.ReadAsStringAsync().Result == "birst.spaceId=Space_ID&birst.ssopassword=SSO_Password&birst.username=email%40somewhere.com")))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("token_response") });

			var token = await TestSubject.GetSSOToken();

			Assert.AreEqual("token_response", token);
		}

		[Test]
		public async Task GetSSOToken_Returns_Token_With_Parameters()
		{
			_birstConfiguration.Setup(c => c.Uri).Returns(new Uri("http://birst"));

			_httpClient.Setup(h => h.PostAsync(new Uri("http://birst/tokengenerator.aspx"), It.Is<FormUrlEncodedContent>(c => c.ReadAsStringAsync().Result == "birst.spaceId=Space_ID_1&birst.ssopassword=SSO_Password_1&birst.username=email%40somewhere.com_1")))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("token_response") });

			var token = await TestSubject.GetSSOToken("Space_ID_1", "SSO_Password_1", "email@somewhere.com_1");

			Assert.AreEqual("token_response", token);
		}

		[Test]
		public void GetSSOToken_Throws_On_Null_Parameters()
		{
			Assert.ThrowsAsync<ArgumentNullException>(() => TestSubject.GetSSOToken(null, "sso-password", "username"));
			Assert.ThrowsAsync<ArgumentNullException>(() => TestSubject.GetSSOToken("space-id", null, "username"));
			Assert.ThrowsAsync<ArgumentNullException>(() => TestSubject.GetSSOToken("space-id", "sso-password", null));
		}

		private void SetupSettings()
		{
			_birstConfiguration.Setup(c => c.Uri).Returns(new Uri("http://birst"));
			_birstConfiguration.Setup(c => c.Email).Returns("email@somewhere.com");
			_birstConfiguration.Setup(c => c.SpaceId).Returns("Space_ID");
			_birstConfiguration.Setup(c => c.SSOPassword).Returns("SSO_Password");
		}

		private void CheckFailedAuthentication(TestDelegate run)
		{
			const string currentUserLogin    = "currentUser";
			const string currentUserPassword = "currentPassword";
			
			_birstConfiguration.Setup(x => x.Email).Returns(currentUserLogin);
			_birstConfiguration.Setup(x => x.UserPassword).Returns(currentUserPassword);

			_birstServiceWrapper.Setup(x => x.GetLoginToken(currentUserLogin, currentUserPassword)).Returns((string)null);

			Assert.Throws<InvalidOperationException>(run);
		}

		private string SetupLogin()
		{
			const string currentUserLogin    = "currentUser";
			const string currentUserPassword = "currentPassword";
			const string token               = "token";

			_birstConfiguration.Setup(x => x.Email).Returns(currentUserLogin);
			_birstConfiguration.Setup(x => x.UserPassword).Returns(currentUserPassword);
			_birstServiceWrapper.Setup(x => x.GetLoginToken(currentUserLogin, currentUserPassword)).Returns(token);

			return token;
		}
	}
}