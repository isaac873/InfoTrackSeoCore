using InfoTrackSEOCore.Configuration;
using InfoTrackSEOCore.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InfoTrackSEOCore.Tests.Controllers
{
	[TestClass]
	public class ScrapeParserTests
	{
		[DataTestMethod]
		[DataRow("")]
		[DataRow("This is a string that will not have any matches.")]
		public void ParseScrapeData_ReturnsEmptyList_WhenNoMatchedData(string data)
		{
			var target = new Mock<ScrapeParser>(new InfoTrackSeoCoreConfiguration
			{
				SearchRegex = "<div>"
			});

			var result = target.Object.ParseScrapedData(data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void ParseScrapeData_ReturnsIndexesOfMatches_WhenFound()
		{
			var data = "<div class=\"g\">some asd content here</div><div class=\"g\">some nothing here</div><div class=\"g\">this one also contains asd nothing</div>";

			var target = new Mock<ScrapeParser>(new InfoTrackSeoCoreConfiguration
			{
				SearchRegex = "<div class=\"g\"(.*?)</div>",
				InfoTrackUrl = "asd"
			});

			var result = target.Object.ParseScrapedData(data);

			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(3, result[1]);
		}
	}
}
