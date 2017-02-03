using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web.Services.Protocols;
using Edge10.Birst.BirstWebService;
using System.Linq;

namespace Edge10.Birst
{
	/// <summary>
	/// A wrapper around the Birst service.
	/// </summary>
	public class BirstServiceWrapper : IBirstServiceWrapper
	{
		private Lazy<CommandWebService> _service;

		/// <summary>
		/// Initializes a new instance of the <see cref="BirstServiceWrapper"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public BirstServiceWrapper(IBirstConfiguration configuration)
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));
			
			
			_service = new Lazy<CommandWebService>(() =>
			{
				if (configuration.Uri == null)
					throw new BirstException("Birst has not been configured for this environment");

				return new CommandWebService
				{
					Url             = new Uri(configuration.Uri, "CommandWebservice.asmx").ToString(),
					CookieContainer = new CookieContainer()
				};
			});
		}

		/// <summary>
		/// Calls the login method on the service.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public string Login(string username, string password)
		{
			return _service.Value.Login(username, password);
		}

		/// <summary>
		/// Calls the getDirectoryContents on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="spaceID">The space identifier.</param>
		/// <param name="dir">The dir.</param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public FileNode GetDirectoryContents(string token, string spaceID, string dir)
		{
			return _service.Value.getDirectoryContents(token, spaceID, dir);
		}

		/// <summary>
		/// Calls the addUser on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="additionalParameters">The additional parameters.</param>
		[ExcludeFromCodeCoverage]
		public void CreateUser(string token, string username, string additionalParameters)
		{
			_service.Value.addUser(token, username, additionalParameters);
		}

		/// <summary>
		/// Calls the addUserToSpace on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="spaceId">The space ID.</param>
		/// <param name="hasAdmin">The hasAdmin flag.</param>
		[ExcludeFromCodeCoverage]
		public void AddUserToSpace(string token, string username, string spaceId, bool hasAdmin)
		{
			_service.Value.addUserToSpace(token, username, spaceId, hasAdmin);
		}

		/// <summary>
		/// Calls the addUserToGroupInSpace on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="spaceId">The space ID.</param>
		/// <param name="groupName">The group name.</param>
		[ExcludeFromCodeCoverage]
		public void AddUserToGroupInSpace(string token, string username, string groupName, string spaceId)
		{
			_service.Value.addUserToGroupInSpace(token, username, groupName, spaceId);
		}

		/// <summary>
		/// Calls the addProductToUser on the service.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="username">The username.</param>
		/// <param name="productId">The product ID.</param>
		[ExcludeFromCodeCoverage]
		public void AddProductToUser(string token, string username, int productId)
		{
			_service.Value.addProductToUser(token, username, productId);
		}

		/// <summary>
		/// Calls the login and returns null in case of known login exceptions.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		[ExcludeFromCodeCoverage]
		public string GetLoginToken(string login, string password)
		{
			using (new SecurityProtocolContext())
			{
				try
				{
					return Login(login, password);
				}
				catch (SoapException e) when (IsLoginFailureException(e))
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Calls the setUserPassword on the service.
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		[ExcludeFromCodeCoverage]
		public void SetUserPassword(string token, string username, string password)
		{
			_service.Value.setUserPassword(token, username, password);
		}

		/// <summary>
		/// Starts extraction of the specified connections on the Birst service.  Does
		/// not process the extracted data (i.e. only stages the data).
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="connections">The connections to be extracted.</param>
		/// <returns>
		/// A token that can be polled for job status
		/// </returns>
		[ExcludeFromCodeCoverage]
		public string ExtractCloudConnectorData(string token, string spaceId, IEnumerable<CloudConnection> connections)
		{
			return _service.Value.extractCloudConnectorData(token, spaceId, connections.ToArray());
		}

		/// <summary>
		/// Starts processing of the specified processing groups through the Birst web service.
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="spaceId">The space identifier.</param>
		/// <param name="processingGroups">The processing groups.</param>
		/// <returns>
		/// A token that can be polled for job status
		/// </returns>
		[ExcludeFromCodeCoverage]
		public string PublishData(string token, string spaceId, IEnumerable<string> processingGroups)
		{
			//note: according to Birst recommendation, always use UTC now for date.  This sets the
			//processing date so never really want to backdate it
			return _service.Value.publishData(token, spaceId, processingGroups.ToArray(), DateTime.UtcNow);
		}

		/// <summary>
		/// Checks whether or not a job is complete on the Birst web service
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		///   <c>true</c> if the job is complete; otherwise, <c>false</c>.
		/// </returns>
		[ExcludeFromCodeCoverage]
		public bool IsJobComplete(string token, string jobToken)
		{
			return _service.Value.isJobComplete(token, jobToken);
		}

		/// <summary>
		/// Returns the list of groups in birst space.
		/// </summary>
		/// <param name="token"></param>
		/// <param name="spaceId"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public IEnumerable<string> ListGroupsInSpace(string token, string spaceId)
		{
			return _service.Value.listGroupsInSpace(token, spaceId);
		}

		/// <summary>
		/// Gets the status of a job from the Birst web service
		/// </summary>
		/// <param name="token">The auth token.</param>
		/// <param name="jobToken">The job token.</param>
		/// <returns>
		/// The status of the job identified by the specified token.
		/// </returns>
		[ExcludeFromCodeCoverage]
		public StatusResult GetJobStatus(string token, string jobToken)
		{
			return _service.Value.getJobStatus(token, jobToken);
		}

		private bool IsLoginFailureException(SoapException e)
		{
			return
				e.Message == "Validation failure" ||                                         //username or password less than the required length (6 at the time of writing)
				e.Message.StartsWith("Login: Failed web service login request for user") ||  //unknown user
				e.Message.StartsWith("Login: Failed web services login attempt for user");   //known user with incorrect password
		}
	}
}
