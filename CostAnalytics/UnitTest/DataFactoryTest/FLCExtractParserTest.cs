using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = Dell.CostAnalytics.DataFactory;
using System.Collections.Generic;
using Dell.CostAnalytics.Global;

namespace Dell.CostAnalytics.UnitTest.DataFactoryTest
{
    [TestClass]
    public class FLCExtractParserTest
    {
        [TestMethod]
        public void TestParseData()
        {
            System.Diagnostics.Debug.Write("Unit test TestParseData has started...\n");
            string flcExtractFilePath = AppSettings.FLCExtractPath;
            string platformConfigurationFilePath = AppSettings.ConsolidatedFilePath;

            Factory.Parsers.PlatformConfigurationParser platformConfigurationParser = new  Factory.Parsers.PlatformConfigurationParser(platformConfigurationFilePath);
            Factory.Parsers.FLCExtractParser flcExtractparser = new Factory.Parsers.FLCExtractParser(flcExtractFilePath);

            var platformConfigurations = platformConfigurationParser.Parse();
            flcExtractparser.Parse(platformConfigurations);

            System.Diagnostics.Debug.Write("Unit test TestParseData has ended...\n");
        }
    }
}
