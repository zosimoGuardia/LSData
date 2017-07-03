using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = Dell.CostAnalytics.DataFactory;
using System.Collections.Generic;
using Dell.CostAnalytics.Global;

namespace Dell.CostAnalytics.UnitTest.DataFactoryTest
{
    [TestClass]
    public class WarrantyDataParserTest
    {
        [TestMethod]
        public void TestParseData()
        {
            System.Diagnostics.Debug.Write("Unit test TestParseData has started...\n");
            string warrantyInfoFilePath = @"..\..\Files\WarrantyInfo\201706.xlsx";

            Factory.Parsers.WarrantyInfoParser warrantyInfoParser = new Factory.Parsers.WarrantyInfoParser(warrantyInfoFilePath);

            warrantyInfoParser.Parse();

            System.Diagnostics.Debug.Write("Unit test TestParseData has ended...\n");
        }
    }
}
