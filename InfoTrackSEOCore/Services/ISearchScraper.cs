using System.Threading.Tasks;

namespace InfoTrackSEOCore.Services
{
    public interface ISearchScraper
    {
		/// <summary>
		/// Gets a string representing the html of a given Google search result page.
		/// </summary>
        Task<string> GetScrapeData();
    }
}
