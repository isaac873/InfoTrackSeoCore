using System;
using System.Threading.Tasks;
using InfoTrackSEOCore.Models;
using InfoTrackSEOCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackSEOCore.Controllers
{
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class SEOController : ControllerBase
	{
		private readonly ISearchScraper _searchScraper;
		private readonly IScrapeParser _scrapeParser;
		private readonly ILogger _logger;

		public SEOController(ISearchScraper searchScraper, IScrapeParser scrapeParser, ILogger logger)
		{
			_searchScraper = searchScraper;
			_scrapeParser = scrapeParser;
			_logger = logger;
		}

		[HttpGet]
		[Route("get-results")]
		public async Task<InfoTrackSEOModel> GetResults()
		{
			var model = new InfoTrackSEOModel();

			try
			{
				// Get data to be scraped.
				var text = await _searchScraper.GetScrapeData();

				if (string.IsNullOrEmpty(text))
				{
					// Nothing was returned from the web request. Exit early.
					model.Success = false;
					model.Message = "An error occurred trying to fetch results from Google, please try again later.";
				}
				else
				{
					// Parse the data and obtain our indexes.
					var indexes = _scrapeParser.ParseScrapedData(text);

					if (indexes.Count > 0)
					{
						// We have results, yay~! set the model properties.
						model.Success = true;
						model.Message = "Your results appeared in the following index positions:";
						model.IndexPositions = indexes;
					}
					else
					{
						// No results, boo... Set an appropriate status message.
						model.Success = false;
						model.Message = "Unfortunately no results were found.";
					}
				}
			}
			catch (Exception e)
			{
				// Something has gone wrong - likely with the response from Google. Inform the user.
				model.Success = false;
				model.Message = "An unexpected error has occurred, please try again later.";

				_logger.Log(e.Message);
			}

			return model;
		}
	}
}
