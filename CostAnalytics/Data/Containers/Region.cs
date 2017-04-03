using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Region : Interfaces.IRegion
    {
        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string RegionName
        {
            get { return m_RegionName; }
            set { m_RegionName = value; }
        }

        public string Country
        {
            get { return m_Country; }
            set { m_Country = value; }
        }

        public string CountryCode
        {
            get { return m_CountryCode; }
            set { m_CountryCode = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Custom Equality comparer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Region)
            {
                Region region = (Region)obj;
                if (
                    ID != region.ID ||
                    RegionName != region.RegionName ||
                    Country != region.Country ||
                    CountryCode != region.CountryCode
                    )
                    return false;
                else
                    return true;
            }//endif
            else
            {
                return false;
            }//end else
        }//end method

        /// <summary>
        /// Custom Get Hash Code method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }//end method
        #endregion

        #region Members
        int m_ID;
        string m_RegionName;
        string m_Country;
        string m_CountryCode;
        #endregion
    }//end class
}// end namespace
