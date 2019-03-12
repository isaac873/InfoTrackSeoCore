using InfoTrackSEOCore.Configuration;
using InfoTrackSEOCore.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;

namespace InfoTrackSEOCore.Tests.Controllers
{
	[TestClass]
	public class SearchScraperTests
	{
		[TestMethod]
		public void GetScrapeData_ReturnsText_AfterSuccessfulWebRequest()
		{
			var streamContent = "Top movies of 2019";
			var usedUrl = string.Empty;

			var webRequesterMock = new Mock<IWebRequester>();
			webRequesterMock.Setup(p => p.MakeWebRequest(It.IsAny<string>()))
				.Returns((string url) =>
				{
					usedUrl = url;

					var stream = new MemoryStream();
					var writer = new StreamWriter(stream);
					writer.Write(streamContent);
					writer.Flush();
					stream.Position = 0;
					return Task.FromResult((Stream)stream);
				});

			var target = new Mock<SearchScraper>(new InfoTrackSeoCoreConfiguration
			{
				GoogleSearchUrl = "http://w.com/{0}"
			}, webRequesterMock.Object);

			var result = target.Object.GetScrapeData().GetAwaiter().GetResult();

			Assert.AreEqual(streamContent, result);
			Assert.AreEqual("http://w.com/online+title+search", usedUrl);
		}

		[TestMethod]
		public void GetScrapeData_ReturnsEmptyString_AfterFailedWebRequest()
		{
			var usedUrl = string.Empty;

			var webRequesterMock = new Mock<IWebRequester>();
			webRequesterMock.Setup(p => p.MakeWebRequest(It.IsAny<string>()))
				.Returns((string url) =>
				{
					usedUrl = url;

					return null;
				});

			var target = new Mock<SearchScraper>(new InfoTrackSeoCoreConfiguration
			{
				GoogleSearchUrl = "http://w.com/{0}"
			}, webRequesterMock.Object);

			var result = target.Object.GetScrapeData();

			Assert.AreEqual("http://w.com/online+title+search", usedUrl);
		}
	}
}
