using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Iteration : Interfaces.IIteration
    {
        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int RegionID
        {
            get { return m_RegionID; }
            set { m_RegionID = value; }
        }

        public int ProductID
        {
            get { return m_ProductID; }
            set { m_ProductID = value; }
        }

        public int ConfigurationID
        {
            get { return m_ConfigurationID; }
            set { m_ConfigurationID = value; }
        }

        public int SKUID
        {
            get { return m_SKUID; }
            set { m_SKUID = value; }
        }


        public int MeasureID
        {
            get { return m_MeasureID; }
            set { m_MeasureID = value; }
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
            if (obj is Iteration)
            {
                Iteration iteration = (Iteration)obj;
                if (
                    ID != iteration.ID ||
                    RegionID != iteration.RegionID ||
                    ProductID != iteration.ProductID ||
                    ConfigurationID != iteration.ConfigurationID ||
                    SKUID != iteration.SKUID ||
                    MeasureID != iteration.MeasureID
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
        int m_RegionID;
        int m_ProductID;
        int m_ConfigurationID;
        int m_SKUID;
        int m_MeasureID;
        #endregion
    }//end class
}// end namespace
