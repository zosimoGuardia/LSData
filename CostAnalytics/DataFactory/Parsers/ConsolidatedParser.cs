using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.DataFactory.Parsers
{
    public class ConsolidatedParser :IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsolidatedParser(string consolidatedFileName)
        {
            this.consolidatedFileName = consolidatedFileName;
        }//end constructor
        #endregion

        #region Methods
        /// <summary>
        /// Parse the "consolidated" file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<ConsolidatedFilter> ParseFilters()
        {
            List<ConsolidatedFilter> toReturn = new List<ConsolidatedFilter>();
            using (FileStream fs = File.OpenRead(consolidatedFileName))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line = string.Empty;
                        //LAT, JAYTON 5175,Venue 11 Pro:Jayton,5175:scp_jayton_min_us|scp_loveland 14 bdw_min_us
                        //PlatformNames..:Platform,Series:Configurations..
                        while ((line = sr.ReadLine())!= null)
                        {
                            try
                            {
                                var raw = line.Split(':');
                                toReturn.Add(new ConsolidatedFilter() {
                                    PlatformName = raw[0],
                                    Platform = raw[1].Split(',').First(),
                                    Model = raw[1].Split(',').Last(),
                                    Configuration = raw[2].Split('|')
                            });
                            } catch (Exception exc)
                            {
                                throw exc;
                            }//end catch
                        }//end while
                    }//end Stream Reader
                }//end Buffered Stream
            }//end File Stream
            return toReturn;
        }//end method
        
        /// <summary>
        /// Implement IDisposable
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
        }//end method
        #endregion
        #region Subclasses
        /// <summary>
        /// Sub-class Filter
        /// </summary>
        public class ConsolidatedFilter
        {
            public string PlatformName { get; set; }
            public string Platform { get; set; }
            public string Model { get; set; }
            public string[] Configuration { get; set; }
        }//end subclass
        #endregion

        #region Members
        private string consolidatedFileName;
        #endregion
    }//end class
}//end namespace
