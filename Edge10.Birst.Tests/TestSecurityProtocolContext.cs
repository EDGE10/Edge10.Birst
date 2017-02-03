using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Edge10.Birst.Tests
{
	/// <summary>
	/// Tests the functionality of the <see cref="SecurityProtocolContext"/> class.
	/// </summary>
	[TestFixture]
	public class TestSecurityProtocolContext
	{
		[Test]
		public void Constructor_Changes_Protocol_And_Disposing_Restores()
		{
			//set this to something that doesn't contain the flag which will be set
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

			var testSubject = new SecurityProtocolContext();
			Assert.AreEqual(SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12, ServicePointManager.SecurityProtocol);

			testSubject.Dispose();
			Assert.AreEqual(SecurityProtocolType.Ssl3, ServicePointManager.SecurityProtocol);
		}
	}
}
