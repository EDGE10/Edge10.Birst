using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// Manages Birst related configuration properties.
	/// </summary>
	public class BirstConfiguration : IBirstConfiguration
	{
		/// <summary>
		/// Gets the base URI.
		/// </summary>
		public Uri Uri { get; set; }
		
		/// <summary>
		/// Gets the space ID for the current tenant.
		/// </summary>
		public string SpaceId { get; set; }


		/// <summary>
		/// The group for users in the space for the current tenant.
		/// </summary>
		public string GroupInSpace { get; set; }

		/// <summary>
		/// Gets the SSO password for the current tenant.
		/// </summary>
		public string SSOPassword { get; set; }

		/// <summary>
		/// Gets the current users email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets the current users Birst password.
		/// </summary>
		public string UserPassword { get; set; }

		/// <summary>
		/// Gets the base email address for created users.
		/// </summary>
		public string BaseEmailAddress { get; set; }

		/// <summary>
		/// Ensures that Birst is configured for the current tenant, and throws a <see cref="BirstException" /> if not.
		/// </summary>
		/// <exception cref="BirstException">Birst has not been configured for this environment</exception>
		public void EnsureConfigured()
		{
			if (this.Uri == null ||
				string.IsNullOrWhiteSpace(this.SpaceId) ||
				string.IsNullOrWhiteSpace(this.SSOPassword))
				throw new BirstException("Birst has not been configured for this environment");
		}
	}
}
