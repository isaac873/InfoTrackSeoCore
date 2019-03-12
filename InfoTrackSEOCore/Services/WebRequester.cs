using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackSEOCore.Services
{
    public class WebRequester : IWebRequester
    {
		private IHttpClientFactory _clientFactory;

		public WebRequester(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		/// <summary>
		/// Makes a web request to the provided url.
		/// </summary>
		/// <param name="url">The url to make a request to.</param>
		/// <returns>A stream representing the web page retrieved.</returns>
		public async Task<Stream> MakeWebRequest(string url)
        {
			var request = new HttpRequestMessage(HttpMethod.Get, url);

			var client = _clientFactory.CreateClient();

			// Make the web request to Google
			var response = await client.SendAsync(request);
			
			if (response != null)
			{
				return await response.Content.ReadAsStreamAsync();
			}

			return null;
        }
    }
}
