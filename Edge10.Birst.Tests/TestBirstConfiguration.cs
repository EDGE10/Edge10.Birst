using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

namespace Edge10.Birst.Tests
{
	/// <summary>
	/// Tests the functionality of the <see cref="BirstConfiguration"/> class.
	/// </summary>	
	[TestFixture]
	public class TestBirstConfiguration
	{
		[Test]
		public void EnsureConfigured_Throws_Exception_For_Missing_Settings()
		{
			var configuration = new BirstConfiguration();
			
			//should initially throw before configuring anything
			Assert.Throws<BirstException>(() => configuration.EnsureConfigured());

			configuration.Uri = new Uri("http://birst");
			Assert.Throws<BirstException>(() => configuration.EnsureConfigured());

			//should throw when url and space are configured
			configuration.SpaceId = "space";
			Assert.Throws<BirstException>(() => configuration.EnsureConfigured());

			//should not throw when everything is configured
			configuration.SSOPassword = "password";
			configuration.EnsureConfigured();
		}
	}
}
