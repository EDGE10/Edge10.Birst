using System;
using NUnit.Framework;
using Moq;
using Edge10.Birst.BirstWebService;
using MsTest = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edge10.Birst.Tests
{
	/// <summary>
	/// Tests the functionality of the <see cref="BirstServiceWrapper"/> class.
	/// </summary>	
	[TestFixture]
	public class TestBirstServiceWrapper
	{
		private Mock<IBirstConfiguration> _configuration;

		[SetUp]
		public void SetUp()
		{
			_configuration = new Mock<IBirstConfiguration>();
		}

		/// <summary>
		/// Ensures that appropriate <see cref="ArgumentNullExceptions"/> are thrown when
		/// null parameters are passed to the constructor.
		/// </summary>
		[Test]
		public void Constructor_Throws_Exception_On_Null_Parameters()
		{
			Assert.Throws<ArgumentNullException>(() => new BirstServiceWrapper(null));
		}

		[Test]
		public void EnsureConfigured_Is_Called_Then_Url_And_CookieContainer_Are_Set_On_Service()
		{
			_configuration.Setup(c => c.Uri).Returns(new Uri("http://birst"));

			//testing this using a private object reading the internal service field as nothing
			//public can run this without making a call to the live service
			var service = new MsTest.PrivateObject(new BirstServiceWrapper(_configuration.Object)).GetField("_service") as Lazy<CommandWebService>;

			Assert.AreEqual("http://birst/CommandWebservice.asmx", service.Value.Url.ToString());
			Assert.IsNotNull(service.Value.CookieContainer);
		}

		[Test]
		public void Constructor_Throws_If_Url_Is_Not_Set()
		{
			_configuration.Setup(c => c.Uri).Returns<Uri>(null);

			var service = new MsTest.PrivateObject(new BirstServiceWrapper(_configuration.Object)).GetField("_service") as Lazy<CommandWebService>;

			Assert.Throws<BirstException>(() => service.Value.Login("", ""));
		}
	}
}
