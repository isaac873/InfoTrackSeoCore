using InfoTrackSEOCore.Configuration;
using InfoTrackSEOCore.Constants;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace InfoTrackSEOCore.Services
{
    public class SearchScraper : ISearchScraper
    {
        private InfoTrackSeoCoreConfiguration _config;
        private IWebRequester _webRequester;

        public SearchScraper(InfoTrackSeoCoreConfiguration config, IWebRequester webRequester)
        {
            _config = config;
            _webRequester = webRequester;
        }

		/// <summary>
		/// Gets a string representing the html of a given Google search result page.
		/// </summary>
		public async Task<string> GetScrapeData()
        {
            var text = "";

            // Get our url format from config and plug in the specific query.
            var url = string.Format(_config.GoogleSearchUrl, HttpUtility.UrlEncode(InfoTrackSeoCoreConstants.SearchTerm));

            var responsestream = await _webRequester.MakeWebRequest(url);

            if (responsestream != null)
            {
                using (var responsereader = new StreamReader(responsestream))
                {
                    // Get the response and read it out as a string
                    text = responsereader.ReadToEnd();
                }
            }

            return text;
        }
    }
}
