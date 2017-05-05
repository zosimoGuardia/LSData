using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Cont = Dell.CostAnalytics.Business.Containers;

namespace Dell.CostAnalytics.DataFactory.Parsers
{
    /// <summary>
    /// Class to extracts data from FLC_EXTRACT_EUC_PC_* File
    /// </summary>
    public class FLCExtractParser
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public FLCExtractParser(string flcExtractFileName)
        {
            this.m_FLCExtractFileName = flcExtractFileName;
        }//end constructor
        #endregion

        #region Methods
        /// <summary>
        /// Method to Parse the CSV from the file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseData(List<ConsolidatedParser.ConsolidatedFilter> consolidatedFilter)
        {
            DataTable dt = DataTable.New.ReadCsv(m_FLCExtractFileName);

            var validPlatforms = from filter in consolidatedFilter
                                 select filter.PlatformName;
            //consolidatedFilter.Select(x => x.PlatformName).ToList();
            var validConfigurations = consolidatedFilter.SelectMany(x => x.Configuration).ToList();

            //Headers
            //"Region"; "Country"; "CCN"; "LOB"; "Product Line"; "Platform"; "SBU"; "Configuration Type"; "Config Name"; "Config Channel"; 
            //"Retail Customer"; "SKU"; "SKU Description"; "SKU Qty"; "MOD"; "MOD Description"; "MOD Qty"; "Total Measure Name"; "Measure Name"; 
            //"Part"; "Part Description"; "Part Qty"; "Measure Detail"; "Commodity"; "Cost As of Date"; "Dec FY17 EXT  Mth 1"; 
            //"Jan FY17 EXT  Mth 2"; "Feb FY18 EXT  Mth 3"; "Mar FY18 EXT  Mth 4"; "Apr FY18 EXT  Mth 5"; "May FY18 EXT  Mth 6"; 
            //"Jun FY18 EXT  Mth 7"; "Jul FY18 EXT  Mth 8"; "Aug FY18 EXT  Mth 9"; "Sep FY18 EXT  Mth 10"; "Oct FY18 EXT  Mth 11"; 
            //"Nov FY18 EXT  Mth 12"; "Warranty"; "Exception Reason Code"; "Latest Rollover Date"; "Cost Type"; "Duration"; "Product Type Desc"; "";


            var filteredRows = from row in dt.Rows
                               where validPlatforms.Contains(row["Platform"])
                               where validConfigurations.Contains(row["Config Name"])
                               select row;

            var totalCosts = filteredRows.Where(x => (x["Measure Detail"]).Equals("Total Cost") && String.IsNullOrEmpty(x["MOD"]));





            foreach (var row in filteredRows)
            {
                Cont.Iteration iteration = new Cont.Iteration();
                var platform = consolidatedFilter.First(x => x.PlatformName == row["Platform"]);
                Cont.Product product = new Cont.Product(0, platform.Platform, row["LOB"], Convert.ToInt32(platform.Model), "");


                iteration.ID = 0;
                iteration.Product = product;
            }

            this.ReadData(filteredRows.ToArray());


            /*
            var configurationData = from row in filteredRows
                                    group row by new
                                    {
                                        Configuration = row["Config Name"]
                                    } into topSlice
                                    select new
                                    {
                                        Configuration = topSlice.Select(x => x["Config Name"]),
                                        Region = topSlice.Select(x => x["Region"]),
                                        Country = topSlice.Select(x => x["Country"])
                                    };

            foreach (var config in configurationData)
            {
                Console.WriteLine(config.Configuration);
            }

            var prodLines = from row in filteredRows select row["Product Line"].ToString();

            foreach (var prodLine in prodLines)
            {
                Console.WriteLine(prodLine);
            }*/
        }//end method

        private void ReadData(Row[] filteredRows)
        {
            Handlers.DatabaseObject dbo = new Handlers.DatabaseObject();

            foreach (var row in filteredRows)
            {
                // Region
                Cont.Region regionInfo = new Cont.Region()
                {
                    RegionName = row["Region"].Trim(),
                    Country = row["Country"].Trim(),
                    CountryCode = row["CCN"].Trim()
                };
                Cont.Region region = dbo.GetRegion(regionInfo);

                //TODO: Implement similar to above for prouct, configuration, measure, sku

                // Product
                Cont.Product productInfo = null;

                // Configuration
                Cont.Configuration configurationInfo = null;

                // Measure
                Cont.Measure measureInfo = null;

                // SKU
                Cont.SKU skuInfo = null;

                // Iteration
                Cont.Iteration iterationInfo = new Cont.Iteration()
                {
                    Region = region,
                    //TODO: Add all other properties
                };
                Business.Handlers.Iteration.Add(iterationInfo);

                // Cost
                Cont.Cost costInfo = null;
                //TODO: Implement similar to above
            }//end for
        }//end method

        #endregion

        #region Members
        string m_FLCExtractFileName = String.Empty;
        #endregion
    }//end class
}//end namespace
