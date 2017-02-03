using Edge10.Birst.BirstWebService;
using System;
using System.Collections.Generic;

namespace Edge10.Birst
{
	/// <summary>
	/// An interface for classes that wrap the Birst service.
	/// </summary>
	public interface IBirstServiceWrapper
	{
		/// <summary>
		/// Calls the login method on the service.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		string Login(string username, string password);

		/// <summary>
		/// Calls the getDirectoryContents on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="spaceID">The space identifier.</param>
		/// <param name="dir">The dir.</param>
		/// <returns></returns>
		FileNode GetDirectoryContents(string token, string spaceID, string dir);

		/// <summary>
		/// Calls the addUser on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="additionalParameters">The additional parameters.</param>
		void CreateUser(string token, string username, string additionalParameters);

		/// <summary>
		/// Calls the addUserToSpace on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="spaceId">The space ID.</param>
		/// <param name="hasAdmin">The hasAdmin flag.</param>
		void AddUserToSpace(string token, string username, string spaceId, bool hasAdmin);

		/// <summary>
		/// Calls the addUserToGroupInSpace on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="spaceId">The space ID.</param>
		/// <param name="groupName">The group name.</param>
		void AddUserToGroupInSpace(string token, string username, string groupName, string spaceId);

		/// <summary>
		/// Calls the addProductToUser on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="productId">The product ID.</param>
		void AddProductToUser(string token, string username, int productId);

		/// <summary>
		/// Calls the login and returns null in case of known login exceptions
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		string GetLoginToken(string login, string password);

		/// <summary>
		/// Calls the setUserPassword on the service.
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		void SetUserPassword(string token, string username, string password);

		/// <summary>
		/// Starts extraction of the specified connections on the Birst service.  Does
		/// not process the extracted data (i.e. only stages the data).
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="connections">The connections to be extracted.</param>
		/// <returns>A token that can be polled for job status</returns>
		string ExtractCloudConnectorData(string token, string spaceId, IEnumerable<CloudConnection> connections);

		/// <summary>
		/// Starts processing of the specified processing groups through the Birst web service.
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="processingGroups">The processing groups.</param>
		/// <returns>A token that can be polled for job status</returns>
		string PublishData(string token, string spaceId, IEnumerable<string> processingGroups);

		/// <summary>
		/// Checks whether or not a job is complete on the Birst web service
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		///   <c>true</c> if the job is complete; otherwise, <c>false</c>.
		/// </returns>
		bool IsJobComplete(string token, string jobToken);

		/// <summary>
		/// Gets the status of a job from the Birst web service
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="jobToken">The job token.</param>
		/// <returns>The status of the job identified by the specified token.</returns>
		StatusResult GetJobStatus(string token, string jobToken);

		/// <summary>
		/// Returns the list of groups in birst space.
		/// </summary>
		/// <param name="token"></param>
		/// <param name="spaceId"></param>
		/// <returns></returns>
		IEnumerable<string> ListGroupsInSpace(string token, string spaceId);
	}
}