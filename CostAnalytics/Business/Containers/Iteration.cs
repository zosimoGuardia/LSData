using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Iteration :IContainer.IIteration
    {
        #region Constructors
        /// <summary> Default constructor for class Iteration </summary>
        public Iteration() { } //End default constructor

        /// <summary>
        /// Constructor that initializes class with pre-specified members.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Region"></param>
        /// <param name="Product"></param>
        /// <param name="Configuration"></param>
        /// <param name="SKU"></param>
        /// <param name="Measure"></param>
        public Iteration(int ID, Region Region, Product Product, Configuration Configuration,
                         SKU SKU, Measure Measure)
        {
            m_ID = ID;
            m_Region = Region;
            m_Product = Product;
            m_Configuration = Configuration;
            m_SKU = SKU;
            m_Measure = Measure;
        } //End constructor (int, Region, Product, Configuration, SKU, Measure)
        #endregion

        #region Properties
        /// <summary> Property for Iteration ID </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Region </summary>
        public Region Region
        {
            get
            {
                if (m_Region == null)
                    m_Region = new Region();
                return m_Region;
            }
            set { m_Region = value; }
        } //End property Region

        /// <summary> Property for Product </summary>
        public Product Product
        {
            get
            {
                if (m_Product == null)
                    m_Product = new Product();
                return m_Product;
            }
            set { m_Product = value; }
        } //End property Product

        /// <summary> Property for Configuration </summary>
        public Configuration Configuration
        {
            get
            {
                if (m_Configuration == null)
                    m_Configuration = new Configuration();
                return m_Configuration;
            }
            set { m_Configuration = value; }
        } //End property Configuration

        /// <summary> Property for SKU </summary>
        public SKU SKU
        {
            get
            {
                if (m_SKU == null)
                    m_SKU = new SKU();
                return m_SKU;
            }
            set { m_SKU = value; }
        } //End property SKU

        /// <summary> Property for Measure </summary>
        public Measure Measure
        {
            get
            {
                if (m_Measure == null)
                    m_Measure = new Measure();
                return m_Measure;
            }
            set { m_Measure = value; }
        } //End property Measure
        #endregion

        #region Methods
        /// <summary> Compares two Biz.Container.Iteration objects for equality </summary>
        /// <param name="obj">The Iteration object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Iteration)
            {
                Iteration iteration = (Iteration)obj;
                if (
                    Region != iteration.Region ||
                    Product != iteration.Product ||
                    Configuration != iteration.Configuration ||
                    SKU != iteration.SKU ||
                    Measure != iteration.Measure
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

        /// <summary> Custom Get Hash Code method for Iteration </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        private int m_ID;
        private Region m_Region;
        private Product m_Product;
        private Configuration m_Configuration;
        private SKU m_SKU;
        private Measure m_Measure;
        #endregion


    } //End class Iteration
}//end namespace
