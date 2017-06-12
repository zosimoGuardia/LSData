using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.DataFactory.Parsers
{
    class WarrantyInfoParser : IDisposable
    {
        #region Constructors
        /// <summary> This constructor takes the warranty file path and sets it as an attribute. </summary>
        /// <param name="warrantyFileName"> File that contains the warranty information. </param>
        public WarrantyInfoParser(string warrantyFilePath)
        {
            this.m_warrantyInfoFilePath = warrantyFilePath;
        } //End constructor (string)
        #endregion

        #region Methods
        /// <summary> Disposes object </summary>
        public void Dispose()
        {
            this.Dispose();
        }
        #endregion

        #region Subclasses
        public class WarrantyInfo
        {
            public string Name { get; set; }
            public string P
        }
        #endregion

        #region Properties
        private string m_warrantyInfoFilePath;
        #endregion
    }
}
