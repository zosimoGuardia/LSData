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
        /// <summary> This constructor takes the configuration file name and sets it as an attribute. </summary>
        /// <param name="consolidatedFileName"> File that contains the config-to-product mapping. </param>
        public ConsolidatedParser(string consolidatedFileName)
        {
            this.consolidatedFileName = consolidatedFileName;
        }//end constructor
        #endregion

        #region Methods
        /// <summary> Parse the configuration file. </summary>
        /// <param name="fileName"> The configuration file. </param>
        /// <returns> List of ConsolidatedFilter objects. </returns>
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
                        while ((line = sr.ReadLine())!= null)
                        {
                            try
                            {
                                var raw = line.Split(':');
                                toReturn.Add(new ConsolidatedFilter() {
                                    Platform = raw[1],
                                    Model = raw[2],
                                    Configuration = raw[0],
                                    Variant = raw[3]
                                });
                            } catch (Exception exc)
                            {   throw exc;  }//end catch
                        }//end while
                    }//end Stream Reader
                }//end Buffered Stream
            }//end File Stream
            return toReturn;
        }//end method
        
        /// <summary> Implement IDisposable </summary>
        public void Dispose()
        {
            this.Dispose();
        }//end method
        #endregion
        #region Subclasses
        /// <summary> Sub-class Filter </summary>
        public class ConsolidatedFilter
        {
            public string PlatformName { get; set; }
            public string Platform { get; set; }
            public string Model { get; set; }
            public string Configuration { get; set; }
            public string Variant { get; set; }
        }//end subclass
        #endregion

        #region Members
        private string consolidatedFileName;
        #endregion
    }//end class
}//end namespace
