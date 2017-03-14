using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = Dell.CostAnalytics.DataFactory;

namespace DataFactoryTest
{
    [TestClass]
    public class FLCExtractParserTest
    {
        [TestMethod]
        public void TestParseData()
        {
            System.Diagnostics.Debug.Write("Unit test TestParseData has started...\n");
            string fileName = "C:\\Users\\Dominic_Bett\\Desktop\\Desktop Items\\PROJECTS\\Cost Analysis\\FLC_EXTRACT_EUC_PC_20161207051244.csv";
            Factory.Parsers.FLCExtractParser parser = new Factory.Parsers.FLCExtractParser();
            parser.ParseData(fileName);

            System.Diagnostics.Debug.Write("Unit test TestParseData has ended...\n");
        }
    }
}
