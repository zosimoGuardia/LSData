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

        /// <summary> Instantiates a Product Object with specified properties </summary>
        /// <param name="ID"> Product ID </param>
        /// <param name="Name"> Marketing Product Name (internal) </param>
        /// <param name="LOB"> Product Family the platform belongs to. </param>
        /// <param name="Model"> Client Product Name (external) </param>
        /// <param name="Variant"> The product's form factor </param>
        /// <param name="Phase"> Current stage of the product lifecycle. </param>
        public Product(int ID, string Name, string LOB, string Model, string Variant)
        {
            m_ID = ID;
            m_Name = Name;
            m_LOB = LOB;
            m_Model = Model;
            m_Variant = Variant;
            m_Phase = Phase;
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
        public string Model {
            get { return m_Model; }
            set { m_Model = value; }
        } //End property Model

        //<summary> Property for Variant </summary>
        public string Variant {
            get { return m_Variant; }
            set { m_Variant = value; }
        } //End property Variant

        //<summary> Property for Phase </summary>
        public string Phase {
            get { return m_Phase; }
            set { m_Phase = value; }
        } //End property Phase
        #endregion

        #region Methods
        /// <summary> Compares two Biz.Container.Product objects for equality </summary>
        /// <param name="obj">The Product object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Product)
            {
                Product Product = (Product)obj;
                if (
                    Name != Product.Name ||
                    LOB != Product.LOB ||
                    Model != Product.Model ||
                    Variant != Product.Variant ||
                    Phase != Product.Phase
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

        /// <summary> Custom Get Hash Code method for Product </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        //<summary> Members of Product class </summary>
        private int m_ID;
        private string m_Name;
        private string m_LOB;
        private string m_Model;
        private string m_Variant;
        private string m_Phase;
        #endregion
    }//end class
}//end namespace
