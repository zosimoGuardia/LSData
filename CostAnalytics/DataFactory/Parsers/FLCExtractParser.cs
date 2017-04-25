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
            }
        }//end method

        #region SubClasses
        /// <summary>
        /// Has Getter Methods that returns Database Objects
        /// equivalent to the one passed in
        /// </summary>
        class DatabaseObject
        {
            #region Constructors
            /// <summary>
            /// Comstructor: Initializes the database objects
            /// </summary>
            public DatabaseObject()
            {
                //TODO: Get data from DB
                this.m_Configurations = null;
                this.m_Measures = null;
                this.m_Phases = null;
                this.m_Products = null;
                this.m_Regions = null;
                this.m_SKUs = null;
            }//end constructor
            #endregion

            #region Methods
            /// <summary>
            /// Get Configuration
            /// </summary>
            /// <param name="configuration"></param>
            /// <returns></returns>
            public Cont.Configuration getConfiguration(Cont.Configuration configuration)
            {
                Cont.Configuration toReturn = null;

                toReturn = this.m_Configurations.FirstOrDefault(x => x.Name == configuration.Name && x.Type == configuration.Type);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to configuration Object
                    this.m_Configurations.Add(configuration);
                    toReturn = configuration;
                }//end if
                return toReturn;
            }// End Getter

            /// <summary>
            /// Get Measure
            /// </summary>
            /// <param name="measure"></param>
            /// <returns></returns>
            public Cont.Measure getMeasure(Cont.Measure measure)
            {
                Cont.Measure toReturn = null;

                toReturn = this.m_Measures.FirstOrDefault(x => x.Name == measure.Name);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to measure Object
                    this.m_Measures.Add(measure);
                    toReturn = measure;
                }//end if
                return toReturn;
            }// End Getter

            /// <summary>
            /// Get Phase
            /// </summary>
            /// <param name="phase"></param>
            /// <returns></returns>
            public Cont.Phase getPhase(Cont.Phase phase)
            {
                Cont.Phase toReturn = null;

                toReturn = this.m_Phases.FirstOrDefault(x => x.Name == phase.Name && x.Product.ID == phase.Product.ID);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to phase Object
                    this.m_Phases.Add(phase);
                    toReturn = phase;
                }//end if
                return toReturn;
            }// End Getter

            /// <summary>
            /// Get Product
            /// </summary>
            /// <param name="product"></param>
            /// <returns></returns>
            public Cont.Product getProduct(Cont.Product product)
            {
                Cont.Product toReturn = null;

                toReturn = this.m_Products.FirstOrDefault(x => x.Name == product.Name && x.LOB == product.LOB && x.Model == product.Model && x.Variant == product.Variant);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to product Object
                    this.m_Products.Add(product);
                    toReturn = product;
                }//end if
                return toReturn;
            }// End Getter

            /// <summary>
            /// Get Region
            /// </summary>
            /// <param name="region"></param>
            /// <returns></returns>
            public Cont.Region getRegion(Cont.Region region)
            {
                Cont.Region toReturn = null;

                toReturn = this.m_Regions.FirstOrDefault(x => x.RegionName == region.RegionName && x.Country == region.Country && x.CountryCode == region.CountryCode);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to region Object
                    this.m_Regions.Add(region);
                    toReturn = region;
                }//end if
                return toReturn;
            }// End Getter

            /// <summary>
            /// GetSKU
            /// </summary>
            /// <param name="sku"></param>
            /// <returns></returns>
            public Cont.SKU getSKU(Cont.SKU sku)
            {
                Cont.SKU toReturn = null;

                toReturn = this.m_SKUs.FirstOrDefault(x => x.Name == sku.Name && x.Description == sku.Description && x.Commodity == sku.Commodity);
                if (toReturn == null)
                {
                    //TODO: Add to DB
                    //  -Returns ID
                    //  -Add ID to sku Object
                    this.m_SKUs.Add(sku);
                    toReturn = sku;
                }//end if
                return toReturn;
            }// End Getter
            #endregion

            #region Members
            //Data from database
            List<Cont.Configuration> m_Configurations;
            List<Cont.Measure> m_Measures;
            List<Cont.Phase> m_Phases;
            List<Cont.Product> m_Products;
            List<Cont.Region> m_Regions;
            List<Cont.SKU> m_SKUs;
            #endregion
        }//end class
        #endregion
        #endregion

        #region Members
        string m_FLCExtractFileName = String.Empty;
        #endregion
    }//end class
}//end namespace
