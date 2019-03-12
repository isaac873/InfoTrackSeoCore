using InfoTrackSEOCore.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfoTrackSEOCore.Services
{
    public class ScrapeParser : IScrapeParser
    {
        private InfoTrackSeoCoreConfiguration _config;

        public ScrapeParser(InfoTrackSeoCoreConfiguration config)
        {
            _config = config;
        }

		/// <summary>
		/// Parses a block of html from a Google search result and identifies the results that match the configured url.
		/// </summary>
		/// <param name="data">A block of text representing the html of a Google search result.</param>
		/// <returns>A list of the index positions where the configured url was found.</returns>
		public List<int> ParseScrapedData(string data)
        {
            var indexes = new List<int>();

            if (string.IsNullOrEmpty(data) == false)
            {
                // Google wraps each search result item in a div with a class of "g" so I've created a regex to find each one of these divs
                var regex = new Regex(_config.SearchRegex);
                var matches = regex.Matches(data).Cast<Match>().ToList();

                // Now we iterate through each search result (should always be 100 as our search query is for 100 results.) and select all the ones that match the configured string.
                indexes = matches.Select((contents, index) => new { index, contents })
                    .Where(content => content.ToString().Contains(_config.InfoTrackUrl))
                    .Select(x => x.index + 1)
                    .ToList();
            }
            
            return indexes;
        }
    }
}
