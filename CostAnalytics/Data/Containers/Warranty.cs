using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Warranty : Interfaces.IWarranty
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

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region Methods
        /// <summary> Custom Equality comparer </summary>
        /// <param name="obj"> Warranty object we're comparing for equality. </param>
        /// <returns> True if object are of the same type and attributes are equal; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Warranty)
            {
                Warranty warranty = (Warranty)obj;
                if (
                    ID != warranty.ID ||
                    Name != warranty.Name ||
                    Description != warranty.Description
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
        /// <returns> The hashcode for this object. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }//end method
        #endregion

        #region Members
        int m_ID;
        string m_Name;
        string m_Description;
        #endregion
    }//end class

}// end namespace
