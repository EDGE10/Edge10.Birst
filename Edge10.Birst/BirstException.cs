using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Edge10.Birst
{
	[Serializable]
	[ExcludeFromCodeCoverage]
	public class BirstException : Exception
	{
		public BirstException()
		{
		}

		public BirstException(string message) : base(message)
		{
		}

		public BirstException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected BirstException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}