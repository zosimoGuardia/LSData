using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BizHandler = Dell.CostAnalytics.Business.Handlers;
using BizContainer = Dell.CostAnalytics.Business.Containers;

namespace Dell.CostAnalytics.UnitTest.BusinessTest
{
    [TestClass]
    public class BusinessTest
    {
        [TestMethod]
        public void AddObject()
        {
            System.Diagnostics.Debug.WriteLine("Unit test for object has started...");
            
            BizContainer.Measure measure = new BizContainer.Measure() { Name = "MeasureTest2" };

            try
            {
                int ID = BizHandler.Measure.Add(measure);
                measure.ID = ID;
                System.Diagnostics.Debug.WriteLine(String.Format("Measure ID: {0} Name: {1}", measure.ID, measure.Name));
            } catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("ERROR: {0} \n {1}", exc.Message, exc.InnerException));
                throw exc;
            }

            System.Diagnostics.Debug.WriteLine("Unit test for object has ended...");
        }

        [TestMethod]
        public void GetObject()
        {
            System.Diagnostics.Debug.WriteLine("Unit test for object has started...");

            var configurations = BizHandler.Configuration.GetAll();

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
