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
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Product"></param>
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
        /// Property for Product Phase
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

        #region Members
        int m_ID;
        string m_Name;
        Product m_Product;
        #endregion
    }//end class
}//end namespace
