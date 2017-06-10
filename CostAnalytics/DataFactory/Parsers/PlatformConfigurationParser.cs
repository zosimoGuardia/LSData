using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.DataFactory.Parsers
{
    public class PlatformConfigurationParser :IDisposable
    {
        #region Constructors
        /// <summary> This constructor takes the configuration file name and sets it as an attribute. </summary>
        /// <param name="consolidatedFileName"> File that contains the config-to-product mapping. </param>
        public PlatformConfigurationParser(string platformConfigurationFilePath)
        {
            this.m_platformConfigurationFilePath = platformConfigurationFilePath;
        }//end constructor
        #endregion

        #region Methods
        /// <summary> Parse the configuration file. </summary>
        /// <param name="fileName"> The configuration file. </param>
        /// <returns> List of Platform Config objects. </returns>
        public List<PlatformConfiguration> Parse()
        {
            List<PlatformConfiguration> toReturn = new List<PlatformConfiguration>();
            using (FileStream fs = File.OpenRead(m_platformConfigurationFilePath))
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
                                string[] raw = line.Split(':');
                                toReturn.Add(new PlatformConfiguration() {
                                    Configuration = raw[0],
                                    Platform = raw[1],
                                    Model = raw[2],
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
        /// <summary> Sub-class Platform Configuration </summary>
        public class PlatformConfiguration
        {
            public string Configuration { get; set; }
            public string Platform { get; set; }
            public string Model { get; set; }
            public string Variant { get; set; }
        }//end subclass
        #endregion

        #region Members
        private string m_platformConfigurationFilePath;
        #endregion
    }//end class
}//end namespace
