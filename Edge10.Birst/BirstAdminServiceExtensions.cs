using Edge10.Birst.BirstWebService;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge10.Birst
{
	/// <summary>
	/// Extensions for <see cref="IBirstAdminService"/>
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class BirstAdminServiceExtensions
	{
		public static void RunExtractCloudConnectionDataJob(this IBirstAdminService adminService, string spaceId, IEnumerable<CloudConnection> connections)
		{ 
			var job = new BirstJob(svc => 
				svc.ExtractCloudConnectorData(spaceId, connections), 
				adminService);

			job.RunToCompletion();
		}

		public static void RunPublishDataJob(this IBirstAdminService adminService, string spaceId, IEnumerable<string> processingGroups)
		{
			var job = new BirstJob(svc =>
				svc.PublishData(spaceId, processingGroups),
				adminService);

			job.RunToCompletion();
		}
	}
}
