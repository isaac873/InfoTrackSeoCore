using System.Collections.Generic;

namespace InfoTrackSEOCore.Services
{
    public interface IScrapeParser
    {
		/// <summary>
		/// Parses a block of html from a Google search result and identifies the results that match the configured url.
		/// </summary>
		/// <param name="data">A block of text representing the html of a Google search result.</param>
		/// <returns>A list of the index positions where the configured url was found.</returns>
        List<int> ParseScrapedData(string data);
    }
}
