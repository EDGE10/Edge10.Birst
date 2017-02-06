using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// Wraps starting a birst job then polling for status.
	/// </summary>
	/// <seealso cref="Edge10.Birst.IBirstJob" />
	public class BirstJob : IBirstJob
	{
		private IBirstAdminService _adminService;
		private Func<IBirstAdminService, string> _startJob;

		/// <summary>
		/// Initializes a new instance of the <see cref="BirstJob"/> class.
		/// </summary>
		/// <param name="startJob">The start job.</param>
		/// <param name="adminService">The admin service.</param>
		/// <exception cref="ArgumentNullException">
		/// startJob
		/// or
		/// adminService
		/// </exception>
		public BirstJob(Func<IBirstAdminService, string> startJob, IBirstAdminService adminService)
		{
			if (startJob     == null) throw new ArgumentNullException(nameof(startJob));
			if (adminService == null) throw new ArgumentNullException(nameof(adminService));
			
			_startJob     = startJob;
			_adminService = adminService;
		}

		/// <summary>
		/// Gets or sets the polling interval at which to check for updates.
		/// </summary>
		public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(10);

		/// <summary>
		/// Starts the birst job and polls for updates until it has completed.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="BirstException"></exception>
		public void RunToCompletion()
		{
			var jobToken = _startJob(_adminService);
			var complete  = _adminService.IsJobComplete(jobToken);

			while (!complete)
			{
				Thread.Sleep(this.PollingInterval);
				complete = _adminService.IsJobComplete(jobToken);
			}

			var status = _adminService.GetJobStatus(jobToken);
			if (status.statusCode == "failed")
				throw new BirstException(status);
		}
	}
}
