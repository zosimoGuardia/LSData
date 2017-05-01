using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Phase  : IContainer.IPhase
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Phase() { }//End default constructor

        /// <summary>
        /// Constructor initializes class with specified params
        /// </summary>
        /// <param name="ID">The ID of this Phase object.</param>
        /// <param name="Name">The name of the phase.</param>
        /// <param name="Product">The product object this phase references.</param>
        public Phase(int ID, string Name, Product Product)
        {
            m_ID = ID;
            m_Name = Name;
            m_Product = Product;
        }//end constructor
        #endregion

        #region Properties
        /// <summary>
        /// Property for Phase ID
        /// </summary>
        public int ID {
            get { return m_ID; }
            set { m_ID = value; }
        }//end property

        /// <summary>
        /// Property for Phase Name
        /// </summary>
        public string Name {
            get { return m_Name; }
            set { m_Name = value; }
        }//end property

        /// <summary>
        /// Property for Product
        /// </summary>
        public Product Product {
            get
            {
                if (m_Product == null)
                    m_Product = new Product();
                return m_Product;
            }
            set { m_Product = value; }
        }//end property
        #endregion

        #region Methods
        /// <summary> Compares two Biz.Container.Phase objects for equality </summary>
        /// <param name="obj">The Phase object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Phase)
            {
                Phase Phase = (Phase)obj;
                if (
                    Name != Phase.Name ||
                    Product != Phase.Product
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

        /// <summary> Custom Get Hash Code method for Phase </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        int m_ID;
        string m_Name;
        Product m_Product;
        #endregion
    }//end class
}//end namespace
