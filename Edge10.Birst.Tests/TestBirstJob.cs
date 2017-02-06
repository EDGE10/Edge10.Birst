using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge10.Birst.Tests
{
	[TestFixture]
	public class TestBirstJob
	{
		private Mock<IBirstAdminService> _adminService;

		[SetUp]
		public void SetUp()
		{
			_adminService = new Mock<IBirstAdminService>();
		}

		[Test]
		public void Constructor_Throws_On_Null_Parameters()
		{
			Assert.Throws<ArgumentNullException>(() => new BirstJob(null, _adminService.Object));
			Assert.Throws<ArgumentNullException>(() => new BirstJob(svc => "token", null));
		}

		[Test]
		public void Initial_Status()
		{
			var job = new BirstJob(svc => 
			{
				Assert.Fail("Should not be called");
				return null;
			}, _adminService.Object);

			_adminService.Verify(a => a.IsJobComplete(It.IsAny<string>()), Times.Never());
			_adminService.Verify(a => a.GetJobStatus(It.IsAny<string>()), Times.Never());
		}

		[Test]
		public void RunToCompletion_Starts_Then_Polls_For_Update()
		{
			var job = new BirstJob(CheckServiceAndReturnToken("token"), _adminService.Object);
			job.PollingInterval = TimeSpan.FromMilliseconds(1);

			_adminService.SetupSequence(a => a.IsJobComplete("token"))
				.Returns(false)
				.Returns(false)
				.Returns(true);

			_adminService.Setup(a => a.GetJobStatus("token")).Returns(new BirstWebService.StatusResult
			{
				statusCode = "success"
			});

			job.RunToCompletion();
			
			_adminService.Verify(a => a.IsJobComplete(It.IsAny<string>()), Times.Exactly(3));
			_adminService.Verify(a => a.GetJobStatus(It.IsAny<string>()), Times.Once());
		}

		[Test]
		public void RunToCompletion_Throws_On_Failed_Job()
		{
			var job = new BirstJob(CheckServiceAndReturnToken("token"), _adminService.Object);
			job.PollingInterval = TimeSpan.FromMilliseconds(1);

			_adminService.SetupSequence(a => a.IsJobComplete("token"))
				.Returns(false)
				.Returns(false)
				.Returns(true);

			var status = new BirstWebService.StatusResult
			{
				statusCode = "failed"
			};
			_adminService.Setup(a => a.GetJobStatus("token")).Returns(status);

			var error = Assert.Throws<BirstException>(() => job.RunToCompletion());

			Assert.That(error.Status, Is.SameAs(status));

			_adminService.Verify(a => a.IsJobComplete(It.IsAny<string>()), Times.Exactly(3));
			_adminService.Verify(a => a.GetJobStatus(It.IsAny<string>()), Times.Once());
		}

		private Func<IBirstAdminService, string> CheckServiceAndReturnToken(string token)
		{
			return svc =>
			{
				Assert.That(svc, Is.SameAs(_adminService.Object));
				return token;
			};
		}
	}
}
