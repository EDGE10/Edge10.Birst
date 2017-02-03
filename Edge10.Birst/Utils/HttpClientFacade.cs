using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Edge10.Birst.Utils
{
	/// <summary>
	/// Facade implementation of the <see cref="IHttpClientFacade"/> interface.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class HttpClientFacade : IHttpClientFacade
	{
		public static IHttpClientFacade CreateDefault()
		{
			return new HttpClientFacade(new HttpClientHandler());
		}

		private HttpClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpClientFacade" /> class.
		/// </summary>
		/// <param name="clientHandler">The single client handler instance.</param>
		public HttpClientFacade(HttpClientHandler clientHandler)
		{
			_client = new HttpClient(clientHandler, false);
		}

		public void SetAuthorizationHeader(AuthenticationHeaderValue header)
		{
			DefaultRequestHeaders.Authorization = header;
		}

		public Uri BaseAddress
		{
			get { return _client.BaseAddress; }
			set { _client.BaseAddress = value; }
		}

		public HttpRequestHeaders DefaultRequestHeaders => _client.DefaultRequestHeaders;

		public void CancelPendingRequests()
		{
			_client.CancelPendingRequests();
		}

		public long MaxResponseContentBufferSize
		{
			get { return _client.MaxResponseContentBufferSize; }
			set { _client.MaxResponseContentBufferSize = value; }
		}

		public Task<HttpResponseMessage> DeleteAsync(string requestUri)
		{
			return _client.DeleteAsync(requestUri);
		}

		public TimeSpan Timeout
		{
			get { return _client.Timeout; }
			set { _client.Timeout = value; }
		}

		public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
		{
			return _client.DeleteAsync(requestUri);
		}

		public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
		{
			return _client.DeleteAsync(requestUri, cancellationToken);
		}

		public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
		{
			return _client.DeleteAsync(requestUri, cancellationToken);
		}

		public Task<HttpResponseMessage> GetAsync(string requestUri)
		{
			return _client.GetAsync(requestUri);
		}

		public Task<HttpResponseMessage> GetAsync(Uri requestUri)
		{
			return _client.GetAsync(requestUri);
		}

		public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
		{
			return _client.GetAsync(requestUri, cancellationToken);
		}

		public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
		{
			return _client.GetAsync(requestUri, completionOption);
		}

		public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
		{
			return _client.GetAsync(requestUri, cancellationToken);
		}

		public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
		{
			return _client.GetAsync(requestUri, completionOption);
		}

		public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
		{
			return _client.GetAsync(requestUri, completionOption, cancellationToken);
		}

		public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
		{
			return _client.GetAsync(requestUri, completionOption, cancellationToken);
		}

		public Task<byte[]> GetByteArrayAsync(string requestUri)
		{
			return _client.GetByteArrayAsync(requestUri);
		}

		public Task<byte[]> GetByteArrayAsync(Uri requestUri)
		{
			return _client.GetByteArrayAsync(requestUri);
		}

		public Task<Stream> GetStreamAsync(string requestUri)
		{
			return _client.GetStreamAsync(requestUri);
		}

		public Task<Stream> GetStreamAsync(Uri requestUri)
		{
			return _client.GetStreamAsync(requestUri);
		}

		public Task<string> GetStringAsync(string requestUri)
		{
			return _client.GetStringAsync(requestUri);
		}

		public Task<string> GetStringAsync(Uri requestUri)
		{
			return _client.GetStringAsync(requestUri);
		}

		public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
		{
			return _client.PostAsync(requestUri, content);
		}

		public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
		{
			return _client.PostAsync(requestUri, content);
		}

		public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return _client.PostAsync(requestUri, content, cancellationToken);
		}

		public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return _client.PostAsync(requestUri, content, cancellationToken);
		}

		public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
		{
			return _client.PutAsync(requestUri, content);
		}

		public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
		{
			return _client.PutAsync(requestUri, content);
		}

		public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return _client.PutAsync(requestUri, content, cancellationToken);
		}

		public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return _client.PutAsync(requestUri, content, cancellationToken);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
		{
			return _client.SendAsync(request);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return _client.SendAsync(request, cancellationToken);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
		{
			return _client.SendAsync(request, completionOption);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
		{
			return _client.SendAsync(request, completionOption, cancellationToken);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_client?.Dispose();

			_client = null;
		}
	}
}