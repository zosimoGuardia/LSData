using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class SKU : Interfaces.ISKU
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

        public string Commodity
        {
            get { return m_commodity; }
            set { m_commodity = value; }
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
            if (obj is SKU)
            {
                SKU sku = (SKU)obj;
                if (
                    ID != sku.ID ||
                    Name != sku.Name ||
                    Description != sku.Description ||
                    Commodity != sku.Commodity
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
        string m_Description;
        string m_commodity;
        #endregion
    }//end class
}// end namespace
