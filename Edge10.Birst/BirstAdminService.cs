using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using Edge10.Birst.BirstWebService;
using Edge10.Birst.Utils;

namespace Edge10.Birst
{
	/// <summary>
	/// An inplementation of <see cref="IBirstAdminService"/>
	/// </summary>
	public class BirstAdminService : IBirstAdminService
	{
		/// <summary>
		/// The ID of the Birst web service product.
		/// </summary>
		public const int BirstServiceProductId = 11;

		private readonly IBirstConfiguration _birstConfiguration;
		private readonly IBirstServiceWrapper _birstServiceWrapper;
		private readonly IHttpClientFacade _httpClient;

		/// <summary>
		/// Creates a new instance of <see cref="BirstAdminService"/>
		/// </summary>
		/// <param name="birstConfiguration">The Birst configuration.</param>
		/// <param name="birstServiceWrapper">The Birst service wrapper.</param>
		/// <param name="httpClient">The http client.</param>
		public BirstAdminService(IBirstConfiguration birstConfiguration, IBirstServiceWrapper birstServiceWrapper, IHttpClientFacade httpClient)
		{
			if (birstConfiguration == null) throw new ArgumentNullException(nameof(birstConfiguration));
			if (birstServiceWrapper == null) throw new ArgumentNullException(nameof(birstServiceWrapper));
			if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

			_birstConfiguration  = birstConfiguration;
			_birstServiceWrapper = birstServiceWrapper;
			_httpClient          = httpClient;
		}

		/// <summary>
		/// Creates a Birst user with specified credentials
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		public void CreateUser(string login, string password)
		{
			var authToken = Login();

			_birstServiceWrapper.CreateUser(authToken, login, $"password={password}");

			_birstServiceWrapper.AddUserToSpace(authToken, login, _birstConfiguration.SpaceId, hasAdmin: true);  //admin access is required to enumerate dashboards

			if (!string.IsNullOrWhiteSpace(_birstConfiguration.GroupInSpace))
				_birstServiceWrapper.AddUserToGroupInSpace(authToken, login, _birstConfiguration.GroupInSpace, _birstConfiguration.SpaceId);

			_birstServiceWrapper.AddProductToUser(authToken, login, BirstServiceProductId);
		}

		/// <summary>
		/// Updates Birst user's password.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		public void SetUserPassword(string login, string password)
		{
			var authToken = Login();
			_birstServiceWrapper.SetUserPassword(authToken, login, password);
		}

		/// <summary>
		/// Starts extraction of the specified connections on the Birst service.  Does
		/// not process the extracted data (i.e. only stages the data).
		/// </summary>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="connections">The connections to be extracted.</param>
		/// <returns>
		/// A token that can be polled for job status
		/// </returns>
		public string ExtractCloudConnectorData(string spaceId, IEnumerable<CloudConnection> connections)
		{
			var authToken = Login();
			return _birstServiceWrapper.ExtractCloudConnectorData(authToken, spaceId, connections);
		}

		/// <summary>
		/// Starts processing of the specified processing groups through the Birst web service.
		/// </summary>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="processingGroups">The processing groups.</param>
		/// <returns>
		/// A token that can be polled for job status
		/// </returns>
		public string PublishData(string spaceId, IEnumerable<string> processingGroups)
		{
			var authToken = Login();
			return _birstServiceWrapper.PublishData(authToken, spaceId, processingGroups);
		}

		/// <summary>
		/// Checks whether or not a job is complete on the Birst web service
		/// </summary>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		///   <c>true</c> if the job is complete; otherwise, <c>false</c>.
		/// </returns>
		public bool IsJobComplete(string jobToken)
		{
			var authToken = Login();
			return _birstServiceWrapper.IsJobComplete(authToken, jobToken);
		}

		/// <summary>
		/// Gets the status of a job from the Birst web service
		/// </summary>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		/// The status of the job identified by the specified token.
		/// </returns>
		public StatusResult GetJobStatus(string jobToken)
		{
			var authToken = Login();
			try
			{
				return _birstServiceWrapper.GetJobStatus(authToken, jobToken);
			}
			catch (SoapException e)
			{
				return new StatusResult
				{
					statusCode = "Failed",
					message = e.Message
				};
			}

		}

		/// <summary>
		/// Gets the SSO token using credentials from config.
		/// </summary>
		/// <returns></returns>
		public Task<string> GetSSOToken()
		{
			return GetSSOToken(_birstConfiguration.Uri, _birstConfiguration.SpaceId, _birstConfiguration.SSOPassword, _birstConfiguration.Email);
		}

		/// <summary>
		/// Gets the SSO token using provided credentials.
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetSSOToken(Uri uri, string spaceId, string ssoPassword, string username)
		{
			if (uri         == null) throw new ArgumentNullException(nameof(uri));
			if (spaceId     == null) throw new ArgumentNullException(nameof(spaceId));
			if (ssoPassword == null) throw new ArgumentNullException(nameof(ssoPassword));
			if (username    == null) throw new ArgumentNullException(nameof(username));

			var tokenGeneratorContent = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "birst.spaceId", spaceId },
				{ "birst.ssopassword", ssoPassword },
				{ "birst.username", username }
			});

			using (new SecurityProtocolContext())
			{
				var response =
					await _httpClient.PostAsync(new Uri(uri, "tokengenerator.aspx"), tokenGeneratorContent);

				if (!response.IsSuccessStatusCode)
					return null;

				return await response.Content.ReadAsStringAsync();
			}
		}

		private string Login()
		{
			var token = _birstServiceWrapper.GetLoginToken(_birstConfiguration.Email, _birstConfiguration.UserPassword);

			if (string.IsNullOrWhiteSpace(token))
				throw new InvalidOperationException("Can't authenticate to birst. Either current user doesn't have Birst account configured or there is no access to Birst web service.");

			return token;
		}
	}
}