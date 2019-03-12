using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackSEOCore.Services
{
    public interface IWebRequester
    {
		/// <summary>
		/// Makes a web request to the provided url.
		/// </summary>
		/// <param name="url">The url to make a request to.</param>
		/// <returns>A stream representing the web page retrieved.</returns>
        Task<Stream> MakeWebRequest(string url);
    }
}
