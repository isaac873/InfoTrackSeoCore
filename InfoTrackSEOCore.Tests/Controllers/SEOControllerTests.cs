using InfoTrackSEOCore.Controllers;
using InfoTrackSEOCore.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoTrackSEOCore.Tests.Controllers
{
	[TestClass]
	public class SEOControllerTests
	{
		[TestMethod]
		public void GetResults_ReturnsError_WhenScrapeFailed()
		{
			var searchScraperMock = new Mock<ISearchScraper>();
			searchScraperMock.Setup(p => p.GetScrapeData())
				.Returns(Task.FromResult(""));

			var scrapeParserMock = new Mock<IScrapeParser>();

			var loggerMock = new Mock<ILogger>();

			var target = new Mock<SEOController>(searchScraperMock.Object, scrapeParserMock.Object, loggerMock.Object);

			var result = target.Object.GetResults().GetAwaiter().GetResult();

			Assert.IsFalse(result.Success);
			Assert.AreEqual("An error occurred trying to fetch results from Google, please try again later.", result.Message);
		}

		[TestMethod]
		public void GetResults_ReturnsError_WhenNoIndexesFound()
		{
			var searchScraperMock = new Mock<ISearchScraper>();
			searchScraperMock.Setup(p => p.GetScrapeData())
				.Returns(Task.FromResult("some content here"));

			var scrapeParserMock = new Mock<IScrapeParser>();
			scrapeParserMock.Setup(p => p.ParseScrapedData(It.IsAny<string>()))
				.Returns(new List<int>());

			var loggerMock = new Mock<ILogger>();

			var target = new Mock<SEOController>(searchScraperMock.Object, scrapeParserMock.Object, loggerMock.Object);

			var result = target.Object.GetResults().GetAwaiter().GetResult();

			Assert.IsFalse(result.Success);
			Assert.AreEqual("Unfortunately no results were found.", result.Message);
		}

		[TestMethod]
		public void GetResults_ReturnsError_WhenExceptionThrown()
		{
			var exceptionText = "An exception occured";
			var usedExceptionText = string.Empty;

			var searchScraperMock = new Mock<ISearchScraper>();
			searchScraperMock.Setup(p => p.GetScrapeData())
				.Returns(() => { throw new Exception(exceptionText); }); //throw an exception here to test the exception conditions.

			var scrapeParserMock = new Mock<IScrapeParser>();
			scrapeParserMock.Setup(p => p.ParseScrapedData(It.IsAny<string>()))
				.Returns(new List<int>());

			var loggerMock = new Mock<ILogger>();
			loggerMock.Setup(p => p.Log(It.IsAny<string>()))
				.Callback((string msg) =>
				{
					usedExceptionText = msg;

					return;
				});

			var target = new Mock<SEOController>(searchScraperMock.Object, scrapeParserMock.Object, loggerMock.Object);

			var result = target.Object.GetResults().GetAwaiter().GetResult();

			Assert.IsFalse(result.Success);
			Assert.AreEqual("An unexpected error has occurred, please try again later.", result.Message);
			Assert.AreEqual(exceptionText, usedExceptionText);
		}

		[TestMethod]
		public void GetResults_ReturnsIndexResult_WhenScrapeSuccessful()
		{
			var searchResults = "some content was returned here";
			var indexPositions = new List<int> { 1, 3, 5 };
			var usedData = string.Empty;

			var searchScraperMock = new Mock<ISearchScraper>();
			searchScraperMock.Setup(p => p.GetScrapeData())
				.Returns(Task.FromResult(searchResults));

			var scrapeParserMock = new Mock<IScrapeParser>();
			scrapeParserMock.Setup(p => p.ParseScrapedData(It.IsAny<string>()))
				.Returns((string data) => 
				{
					usedData = data;
					return indexPositions;
				});

			var loggerMock = new Mock<ILogger>();

			var target = new Mock<SEOController>(searchScraperMock.Object, scrapeParserMock.Object, loggerMock.Object);

			var result = target.Object.GetResults().GetAwaiter().GetResult();

			Assert.IsTrue(result.Success);
			Assert.AreEqual("Your results appeared in the following index positions:", result.Message);
			Assert.AreEqual(searchResults, usedData);
			Assert.AreEqual(3, result.IndexPositions.Count);
			Assert.AreEqual(1, result.IndexPositions[0]);
			Assert.AreEqual(3, result.IndexPositions[1]);
			Assert.AreEqual(5, result.IndexPositions[2]);
		}
	}
}
