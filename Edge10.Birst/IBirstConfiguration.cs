using System;

namespace Edge10.Birst
{
	/// <summary>
	/// An interface for classes which manages Birst related configuration properties.
	/// </summary>
	public interface IBirstConfiguration
	{
		/// <summary>
		/// Gets the base URI.
		/// </summary>
		Uri Uri { get; }

		/// <summary>
		/// Gets the space ID for the current tenant.
		/// </summary>
		string SpaceId { get; }

		/// <summary>
		/// The group for users in the space for the current tenant.
		/// </summary>
		string GroupInSpace { get; }

		/// <summary>
		/// Gets the SSO password for the current tenant.
		/// </summary>
		string SSOPassword { get; }

		/// <summary>
		/// Gets the current users email address.
		/// </summary>
		string Email { get; }

		/// <summary>
		/// Gets the current users Birst password.
		/// </summary>
		string UserPassword { get; }

		/// <summary>
		/// Gets the base email address for created users.
		/// </summary>
		string BaseEmailAddress { get; }

		/// <summary>
		/// Ensures that Birst is configured for the current tenant, and throws a <see cref="ServiceUnavailableException" /> if not.
		/// </summary>
		void EnsureConfigured();
	}
}