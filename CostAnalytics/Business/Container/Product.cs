using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Container
{
    public class Product
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Product()
        {
        } //end constructor

        /// <summary>
        /// Costructor - Initializes all properties
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="LOB"></param>
        /// <param name="Model"></param>
        /// <param name="Variant"></param>
        public Product(int ID, string Name, string LOB, int Model, string Variant)
        {
            m_ID = ID;
            m_Name = Name;
            m_LOB = LOB;
            m_Model = Model;
            m_Variant = Variant;
        }//end constructor
        #endregion

        #region Properties
        /// <summary>
        /// Property for Product ID
        /// </summary>
        public int ID {
            get { return m_ID; }
            set { m_ID = value; }
        }//end property

        /// <summary>
        /// Property for Product Name
        /// </summary>
        public string Name {
            get { return m_Name; }
            set { m_Name = value; }
        }//end property

        public string LOB {
            get { return m_LOB; }
            set { m_LOB = value; }
        }
        public int Model {
            get { return m_Model; }
            set { m_Model = value; }
        }
        public string Variant {
            get { return m_Variant; }
            set { m_Variant = value; }
        }
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        private string m_LOB;
        private int m_Model;
        private string m_Variant;
        #endregion
    }//end class
}//end namespace
