using System;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// Wraps starting a birst job then polling for status.
	/// </summary>
	public interface IBirstJob
	{
		/// <summary>
		/// Gets or sets the polling interval at which to check for updates.
		/// </summary>
		TimeSpan PollingInterval { get; set; }

		/// <summary>
		/// Starts the birst job and polls for updates until it has completed.
		/// </summary>
		/// <returns></returns>
		void RunToCompletion();
	}
}