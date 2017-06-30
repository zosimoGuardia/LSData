using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class WarrantyCost : Interfaces.IWarrantyCost
    {
        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        } //End property Date

        public double Cost
        {
            get { return m_Cost; }
            set { m_Cost = value; }
        } //End property Cost

        public int WarrantyID
        {
            get { return m_WarrantyID; }
            set { m_WarrantyID = value; }
        } //End property WarrantyID

        public int ConfigurationID
        {
            get { return m_ConfigurationID; }
            set { m_ConfigurationID = value; }
        } //End property ConfigurationID
        #endregion

        #region Methods
        /// <summary> Custom Equality comparer </summary>
        /// <param name="obj"> The object you want to compare for. </param>
        /// <returns> True if objects are of the same type and contain the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is WarrantyCost)
            {
                WarrantyCost warrantyCost = (WarrantyCost)obj;
                if (
                    ID != warrantyCost.ID ||
                    Date != warrantyCost.Date ||
                    Cost != warrantyCost.Cost ||
                    WarrantyID != warrantyCost.WarrantyID ||
                    ConfigurationID != warrantyCost.ConfigurationID
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

        /// <summary> Custom Get Hash Code method </summary>
        /// <returns> The object's hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }//end method
        #endregion

        #region Members
        int m_ID;
        DateTime m_Date;
        double m_Cost;
        int m_WarrantyID;
        int m_ConfigurationID;
        #endregion
    }//end class
}// end namespace
