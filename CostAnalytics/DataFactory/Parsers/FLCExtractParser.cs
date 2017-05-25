using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Cont = Dell.CostAnalytics.Business.Containers;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Dell.CostAnalytics.DataFactory.Parsers
{
    /// <summary> Class to extract data from FLC_EXTRACT_EUC_PC_* file </summary>
    public class FLCExtractParser
    {
        #region Constructors
        /// <summary> This constructor takes in the path to the FLC_Extract file and sets the property </summary>
        public FLCExtractParser(string flcExtractFileName)
        {
            this.m_FLCExtractFileName = flcExtractFileName;
        }//end constructor
        #endregion

        #region Methods
        /// <summary> Creates a data table from the CSV file and calls the ReadData. </summary>
        /// <param name="fileName"> The list of ConsolidatedFilter objects extracted from the configuration file. </param>
        public void ParseData(List<ConsolidatedParser.ConsolidatedFilter> consolidatedFilter)
        {
            DataTable dt = DataTable.New.ReadCsv(m_FLCExtractFileName);

            var validConfigurations = from filter in consolidatedFilter
                                      select filter.Configuration;

            //Headers
            //"Region"; "Country"; "CCN"; "LOB"; "Product Line"; "Platform"; "SBU"; "Configuration Type"; "Config Name"; "Config Channel"; 
            //"Retail Customer"; "SKU"; "SKU Description"; "SKU Qty"; "MOD"; "MOD Description"; "MOD Qty"; "Total Measure Name"; "Measure Name"; 
            //"Part"; "Part Description"; "Part Qty"; "Measure Detail"; "Commodity"; "Cost As of Date"; "Dec FY17 EXT  Mth 1"; 
            //"Jan FY17 EXT  Mth 2"; "Feb FY18 EXT  Mth 3"; "Mar FY18 EXT  Mth 4"; "Apr FY18 EXT  Mth 5"; "May FY18 EXT  Mth 6"; 
            //"Jun FY18 EXT  Mth 7"; "Jul FY18 EXT  Mth 8"; "Aug FY18 EXT  Mth 9"; "Sep FY18 EXT  Mth 10"; "Oct FY18 EXT  Mth 11"; 
            //"Nov FY18 EXT  Mth 12"; "Warranty"; "Exception Reason Code"; "Latest Rollover Date"; "Cost Type"; "Duration"; "Product Type Desc"; "";


            var filteredRows = from row in dt.Rows
                               where validConfigurations.Contains(row["Config Name"])
                               select row;

            var totalCosts = filteredRows.Where(x => (x["Measure Detail"]).Equals("Total Cost") && String.IsNullOrEmpty(x["MOD"]));



            foreach (var row in filteredRows)
            {
                Cont.Iteration iteration = new Cont.Iteration();
                var platform = consolidatedFilter.First(x => x.PlatformName == row["Platform"]);
                Cont.Product product = new Cont.Product(0, platform.Platform, row["LOB"], Convert.ToInt32(platform.Model), platform.Variant);

                iteration.ID = 0;
                iteration.Product = product;
            }

            this.ReadData(filteredRows.ToArray());
        }//end method


        /// <summary> Reads the filtered rows from the data table and instantiates the respective container objects. </summary>
        /// <param name="filteredRows"> A datatable containing the rows we care about per the specified filter criteria. </param>
        private void ReadData(Row[] filteredRows)
        {
            Handlers.DatabaseObject dbo = new Handlers.DatabaseObject();

            foreach (var row in filteredRows)
            {
                /* Region */
                Cont.Region regionInfo = new Cont.Region()
                {
                    RegionName = row["Region"].Trim(),
                    Country = row["Country"].Trim(),
                    CountryCode = row["CCN"].Trim()
                };
                dbo.GetRegion(regionInfo);


                /* Product */
                Cont.Product productInfo = new Cont.Product()
                {
                    LOB = row["LOB"].Trim(),
                };
                dbo.GetProduct(productInfo);


                /* Configuration */
                String confType = "";
                if (Regex.IsMatch(row["Config Name"], "_min_"))
                { confType = "Min"; }
                else if (Regex.IsMatch(row["Config Name"], "_avg_"))
                { confType = "Avg"; }
                Cont.Configuration configurationInfo = new Cont.Configuration()
                {
                    Name = row["Config Name"].Trim(),
                    Type = confType                                                     // We also need to talk about naming scheme going forward.
                };
                dbo.GetConfiguration(configurationInfo);


                /* Measure */
                Cont.Measure measureInfo = new Business.Containers.Measure()
                {
                    Name = row["Measure Name"].Trim()
                };
                dbo.GetMeasure(measureInfo);


                /* SKU */
                Cont.SKU skuInfo = new Business.Containers.SKU()
                {
                    Name = row["SKU"].Trim(),
                    Description = row["SKU Description"].Trim(),
                    Commodity = row["Commodity"].Trim()
                };
                dbo.GetSKU(skuInfo);


                /* Iteration */
                Cont.Iteration iterationInfo = new Cont.Iteration()
                {
                    Region = regionInfo,
                    Measure = measureInfo,
                    Configuration = configurationInfo,
                    Product = productInfo,
                    SKU = skuInfo
                };
                Business.Handlers.Iteration.Add(iterationInfo);


                /* Cost */
                string formatString = "yyyyMMddHHmmss";
                var date = DateTime.ParseExact(m_FLCExtractFileName.Trim().Split('_').Last(), formatString, CultureInfo.InvariantCulture);
                var currentCost = Convert.ToDouble(row[row.ColumnNames.First(x => x.Contains("Mth 1"))]);
                var Pred1Cost = Convert.ToDouble(row[row.ColumnNames.First(x => x.Contains("Mth 2"))]);
                var Pred2Cost = Convert.ToDouble(row[row.ColumnNames.First(x => x.Contains("Mth 3"))]);
                var Pred3Cost = Convert.ToDouble(row[row.ColumnNames.First(x => x.Contains("Mth 4"))]);
                Cont.Cost costInfo = new Business.Containers.Cost()
                {
                    Iteration = iterationInfo,
                    Date = date,
                    CurrentCost = currentCost,
                    CostNext1 = Pred1Cost,
                    CostNext2 = Pred2Cost,
                    CostNext3 = Pred3Cost
                };
                Business.Handlers.Cost.Add(costInfo);
            }//end for
        }//end method

        #endregion

        #region Members
        string m_FLCExtractFileName = String.Empty;
        #endregion
    }//end class
}//end namespace
