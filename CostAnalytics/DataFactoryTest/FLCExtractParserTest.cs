using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = Dell.CostAnalytics.DataFactory;
using System.Collections.Generic;
using Dell.CostAnalytics.Global;

namespace DataFactoryTest
{
    [TestClass]
    public class FLCExtractParserTest
    {
        [TestMethod]
        public void TestParseData()
        {
            System.Diagnostics.Debug.Write("Unit test TestParseData has started...\n");
            string flcExtractFileName = AppSettings.FLCExtractPath;
            string consolidatedFileName = AppSettings.ConsolidatedFilePath;

            Factory.Parsers.ConsolidatedParser consolidatedParser = new  Factory.Parsers.ConsolidatedParser(consolidatedFileName);
            Factory.Parsers.FLCExtractParser flcExtractparser = new Factory.Parsers.FLCExtractParser(flcExtractFileName);

            var consolidatedFilter = consolidatedParser.ParseFilters();
            flcExtractparser.ParseData(consolidatedFilter);

            System.Diagnostics.Debug.Write("Unit test TestParseData has ended...\n");
        }
    }
}
