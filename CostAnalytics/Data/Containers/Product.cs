using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers
{
    [Serializable()]
    public sealed class Product : Interfaces.IProduct
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

        public string LOB
        {
            get { return m_LOB; }
            set { m_LOB = value; }
        }

        public int Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        public string Variant
        {
            get { return m_Variant; }
            set { m_Variant = value; }
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
            if (obj is Product)
            {
                Product product = (Product)obj;
                if (
                    ID != product.ID ||
                    Name != product.Name ||
                    LOB != product.LOB ||
                    Model != product.Model ||
                    Variant != product.Variant
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
        string m_LOB;
        int m_Model;
        string m_Variant;
        #endregion
    }//end class
}// end namespace
