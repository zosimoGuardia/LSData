using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz = Dell.CostAnalytics.Business.Handlers;

namespace Dell.CostAnalytics.UnitTest.BusinessTest
{
    [TestClass]
    public class BusinessTest
    {
        [TestMethod]
        public void GetObject()
        {
            System.Diagnostics.Debug.WriteLine("Unit test for object has started...");

            var configurations = Biz.Configuration.GetAll();

            if (configurations == null)
                System.Diagnostics.Debug.WriteLine("ERROR: configuration is null");
            else if (configurations.Length == 0)
                System.Diagnostics.Debug.WriteLine("ERROR: configuration is empty");
            else
            {
                System.Diagnostics.Debug.WriteLine("First Configuration Name: " + configurations[0].Name);
                System.Diagnostics.Debug.WriteLine("First Configuration Type: " + configurations[0].Type);
                System.Diagnostics.Debug.WriteLine("Total Configurations: " +  configurations.Length);
            }

            System.Diagnostics.Debug.WriteLine("Unit test for object has ended...");
        }
    }
}
