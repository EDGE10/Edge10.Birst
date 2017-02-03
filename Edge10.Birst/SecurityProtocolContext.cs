using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// Updates the security protocol to include the type required by Birst calls
	/// then restores it when disposed.
	/// </summary>
	public class SecurityProtocolContext : IDisposable
	{
		SecurityProtocolType _original;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityProtocolContext"/> class.
		/// </summary>
		public SecurityProtocolContext()
		{
			_original = ServicePointManager.SecurityProtocol;

			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				ServicePointManager.SecurityProtocol = _original;
		}
	}
}
