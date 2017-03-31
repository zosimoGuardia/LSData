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
        public Iteration() { }; //End default constructor

        /** <summary> Constructor that initializes class with pre-specified members. </summary>
          * <param name="_id"> The Iteration ID </param>
          * <param name="_region"> Object that contains region information. </param>
          * <param name="_product"> Object that contains product information. </param>
          * <param name="_config"> Object that contains configuration information. </param>
          * <param name="_sku"> Object that contains SKU information. </param>
          * <param name="_measure"> Object that contain Cost Adder information. </param>
          **/
        public Iteration(int _id, Region _region, Product _product, Configuration _config,
                         SKU _sku, Measure _measure)
        {
            m_ID = _id;
            m_Region = _region;
            m_Product = _product;
            m_Configuration = _config;
            m_SKU = _sku;
            m_Measure = _measure;
        } //End constructor (int, Region, Product, Configuration, SKU, Measure)
        #endregion

        #region Members
        private int m_ID;
        private Region m_Region;
        private Product m_Product;
        private Configuration m_Configuration;
        private SKU m_SKU;
        private Measure m_Measure;
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
                { return new Region(); }
                else
                { return m_Region; }
            }
            set { m_Region = value; }
        } //End property Region

        /// <summary> Property for Product </summary>
        public Product Product
        {
            get
            {
                if (m_Product == null)
                { return new Product(); }
                else
                { return m_Product; }
            }
            set { m_Product = value; }
        } //End property Product

        /// <summary> Property for Configuration </summary>
        public Configuration Configuration
        {
            get
            {
                if (m_Configuration == null)
                { return new Configuration(); }
                else
                { return m_Configuration; }
            }
            set { m_Configuration = value; }
        } //End property Configuration

        /// <summary> Property for SKU </summary>
        public SKU SKU
        {
            get
            {
                if (m_SKU == null)
                { return new SKU(); }
                else
                { return m_SKU; }
            }
            set { m_SKU = value; }
        } //End property SKU

        /// <summary> Property for Measure </summary>
        public Measure Measure
        {
            get
            {
                if (m_Measure == null)
                { return new Measure(); }
                else
                { return m_Measure; }
            }
            set { m_Measure = value; }
        } //End property Measure
        #endregion

    } //End class Iteration
}
