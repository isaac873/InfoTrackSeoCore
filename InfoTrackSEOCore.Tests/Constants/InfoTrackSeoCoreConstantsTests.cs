using InfoTrackSEOCore.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTrackSEOCore.Tests.Constants
{
	[TestClass]
	public class InfoTrackSeoCoreConstantsTests
	{
		[TestMethod]
		public void SearchTermConstant_IsExpectedValue()
		{
			Assert.AreEqual("online title search", InfoTrackSeoCoreConstants.SearchTerm);
		}
	}
}
