using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Phase : Interfaces.IPhase
    {
        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int ProductID
        {
            get { return m_ProductID; }
            set { m_ProductID = value; }
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
            if (obj is Phase)
            {
                Phase phase = (Phase)obj;
                if (
                    ID != phase.ID ||
                    Name != phase.Name ||
                    ProductID != phase.ProductID
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
        string m_Name;
        int m_ProductID;
        #endregion
    }//end class
}// end namespace
