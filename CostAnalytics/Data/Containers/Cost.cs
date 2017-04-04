using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Cost : Interfaces.ICost
    {
        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int IterationID
        {
            get { return m_IterationID; }
            set { m_IterationID = value; }
        }

        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        public double CurrentCost
        {
            get { return m_CurrentCost; }
            set { m_CurrentCost = value; }
        }

        public double CostNext1
        {
            get { return m_CostNext1; }
            set { m_CostNext1 = value; }
        }
        
        public double CostNext2
        {
            get { return m_CostNext2; }
            set { m_CostNext2 = value; }
        }
        public double CostNext3
        {
            get { return m_CostNext3; }
            set { m_CostNext3 = value; }
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
            if (obj is Cost)
            {
                Cost cost = (Cost)obj;
                if (
                    ID != cost.ID ||
                    IterationID != cost.IterationID ||
                    Date != cost.Date ||
                    CurrentCost != cost.CurrentCost ||
                    CostNext1 != cost.CostNext1 ||
                    CostNext2 != cost.CostNext2 ||
                    CostNext3 != cost.CostNext3
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
        int m_IterationID;
        DateTime m_Date;
        double m_CurrentCost;
        double m_CostNext1;
        double m_CostNext2;
        double m_CostNext3;
        #endregion
    }//end class
}// end namespace
