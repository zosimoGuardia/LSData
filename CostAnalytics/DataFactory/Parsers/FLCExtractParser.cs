using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public FLCExtractParser()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to Parse the CSV from the file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseData(string fileName)
        {
            using (CachedCsvReader csv = new CachedCsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    for(int i = 0; i < fieldCount; i++)
                    {
                        Console.WriteLine(string.Format("{0} = {1};",
                              headers[i], csv[i]));
                        Console.Read();
                    }//end for
                }//end while
            }//end using
        }//end method
        #endregion
    }//end class
}//end namespace
