using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Product : IContainer.IProduct
    {
        #region Constructors
        /// <summary> Default Constructor for the Product Class </summary>
        public Product()
        {
        } //end constructor

        /** <summary> Instantiates a Product Object with specified properties </summary>
          * <param name="ID"> The Product ID </param>
          * <param name="Name"> The internal name of the product. </param>
          * <param name="LOB"> The Line of Business the product belongs to. </param>
          * <param name="Model"> The external name of the product (Model number). </param>
          * <param name="Variant"> The product's form factor (if more than one is available). </param>
          **/
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
        /// <summary> Property for Product ID </summary>
        public int ID {
            get { return m_ID; }
            set { m_ID = value; }
        }//end property

        //<summary> Property for Product Name </summary>
        public string Name {
            get { return m_Name; }
            set { m_Name = value; }
        }//end property

        //<summary> Property for LOB </summary>
        public string LOB {
            get { return m_LOB; }
            set { m_LOB = value; }
        } //End property LOB

        //<summary> Property for Model </summary>
        public int Model {
            get { return m_Model; }
            set { m_Model = value; }
        } //End property Model

        //<summary> Property for Variant </summary>
        public string Variant {
            get { return m_Variant; }
            set { m_Variant = value; }
        } //End property Variant
        #endregion


        #region Members
        //<summary> Members of Product class </summary>
        private int m_ID;
        private string m_Name;
        private string m_LOB;
        private int m_Model;
        private string m_Variant;
        #endregion
    }//end class
}//end namespace
