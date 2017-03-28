using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = Dell.CostAnalytics.DataFactory;
using System.Collections.Generic;

namespace DataFactoryTest
{
    [TestClass]
    public class FLCExtractParserTest
    {
        [TestMethod]
        public void TestParseData()
        {
            System.Diagnostics.Debug.Write("Unit test TestParseData has started...\n");
            const string flcExtractFileName = "C:\\Users\\Dominic_Bett\\Desktop\\Desktop Items\\PROJECTS\\Cost Analysis\\FLC_EXTRACT_EUC_PC_20161207051244.csv";
            const string consolidatedFileName = "C:\\Users\\Dominic_Bett\\Desktop\\Desktop Items\\PROJECTS\\Cost Analysis\\consolidated.txt";

            Factory.Parsers.ConsolidatedParser consolidatedParser = new  Factory.Parsers.ConsolidatedParser(consolidatedFileName);
            Factory.Parsers.FLCExtractParser flcExtractparser = new Factory.Parsers.FLCExtractParser(flcExtractFileName);

            var consolidatedFilter = consolidatedParser.ParseFilters();
            flcExtractparser.ParseData(consolidatedFilter);

            System.Diagnostics.Debug.Write("Unit test TestParseData has ended...\n");
        }
    }
}
