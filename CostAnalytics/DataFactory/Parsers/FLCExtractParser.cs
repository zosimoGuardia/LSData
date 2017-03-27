using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Cont = Dell.CostAnalytics.Business.Container;

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
            this.flcExtractFileName = flcExtractFileName;
            this.InitializeDataModel();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to Parse the CSV from the file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseData(List<ConsolidatedParser.ConsolidatedFilter> consolidatedFilter)
        {
            DataTable dt = DataTable.New.ReadCsv(flcExtractFileName);

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






            foreach (var row in filteredRows)
            {
                Cont.Iteration iteration = new Cont.Iteration();
                var platform = consolidatedFilter.First(x => x.PlatformName == row["Platform"]);
                Cont.Product product = new Cont.Product(0, platform.Platform, row["LOB"], Convert.ToInt32(platform.Model), "");

                
                iteration.ID = 0;
                iteration.Product = product;
            }

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

            foreach(var config in configurationData)
            {
                Console.WriteLine(config.Configuration);
            }

            var prodLines = from row in filteredRows select row["Product Line"].ToString();

            foreach (var prodLine in prodLines)
            {
                Console.WriteLine(prodLine);
            }
        }//end method
        
        /// <summary>
        /// Get existing data from the database and populate the business model
        /// </summary>
        private void InitializeDataModel()
        {
          this.configurations = null;
            this.costs = null;
            this.measures = null;
            this.phases = null;
            this.products = null;
            this.regions = null;
            this.skus = null;
        }//end method
        #endregion

        #region Members
        private string flcExtractFileName;
        
        //Data from database
        private  List<Cont.Configuration> configurations;
        private  List<Cont.Cost> costs;
        private  List<Cont.Measure> measures;
        private  List<Cont.Phase> phases;
        private  List<Cont.Product> products;
        private  List<Cont.Region> regions;
        private  List<Cont.SKU> skus;
        #endregion
    }//end class
}//end namespace
