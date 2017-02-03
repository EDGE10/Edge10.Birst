using Edge10.Birst.BirstWebService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// An interface for classes that work with Birst administration API.
	/// </summary>
	public interface IBirstAdminService
	{
		/// <summary>
		/// Creates a Birst user with specified credentials
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		void CreateUser(string login, string password);

		/// <summary>
		/// Updates Birst user's password.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		void SetUserPassword(string login, string password);

		/// <summary>
		/// Starts extraction of the specified connections on the Birst service.  Does
		/// not process the extracted data (i.e. only stages the data).
		/// </summary>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="connections">The connections to be extracted.</param>
		/// <returns>A token that can be polled for job status</returns>
		string ExtractCloudConnectorData(string spaceId, IEnumerable<CloudConnection> connections);

		/// <summary>
		/// Starts processing of the specified processing groups through the Birst web service.
		/// </summary>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="processingGroups">The processing groups.</param>
		/// <returns>A token that can be polled for job status</returns>
		string PublishData(string spaceId, IEnumerable<string> processingGroups);

		/// <summary>
		/// Checks whether or not a job is complete on the Birst web service
		/// </summary>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		///   <c>true</c> if the job is complete; otherwise, <c>false</c>.
		/// </returns>
		bool IsJobComplete(string jobToken);

		/// <summary>
		/// Gets the status of a job from the Birst web service
		/// </summary>
		/// <param name="jobToken">The job token.</param>
		/// <returns>The status of the job identified by the specified token.</returns>
		StatusResult GetJobStatus(string jobToken);

		/// <summary>
		/// Gets the SSO token using credentials from config.
		/// </summary>
		/// <returns></returns>
		Task<string> GetSSOToken();

		/// <summary>
		/// Gets the SSO token using provided credentials.
		/// </summary>
		/// <returns></returns>
		Task<string> GetSSOToken(string spaceId, string ssoPassword, string username);
	}
}