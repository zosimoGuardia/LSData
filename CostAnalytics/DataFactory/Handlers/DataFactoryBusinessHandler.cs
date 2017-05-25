using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Business.Containers;
using Handler = Dell.CostAnalytics.Business.Handlers;

namespace Dell.CostAnalytics.DataFactory.Handlers
{
    public class DatabaseObject
    {
        #region Constructors
        /// <summary> Default constructor for the class. </summary>
        public DatabaseObject()
        {
            // Initialize the members
            if (Configurations == null) { Configurations = Handler.Configuration.GetAll().ToList(); }
            if (Measures == null)       { Measures =  Handler.Measure.GetAll().ToList(); }
            if (Products == null)       { Products = Handler.Product.GetAll().ToList(); }
            if (Regions == null)        { Regions = Handler.Region.GetAll().ToList(); }
            if (SKUs == null)           { SKUs = Handler.SKU.GetAll().ToList(); }
        }
        #endregion

        #region Methods
        /// <summary> This method returns the Configuration object that matches the one passed into the function. </summary>
        /// <param name="Configuration"> A Container Configuration object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public void GetConfiguration(Cont.Configuration Configuration)
        {
            var info = Configurations.FirstOrDefault(x => x.Equals(Configuration));
            if (info == null)
            {
                Configuration.ID = Handler.Configuration.Add(Configuration);
                Configurations.Add(Configuration);
            }
            else
            {
                Configuration.ID = info.ID;
            }
        } //End method GetConfiguration


        /// <summary> This method returns the Measure object that matches the one passed into the function. </summary>
        /// <param name="measure"> A Container Measure object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public void GetMeasure(Cont.Measure measure)
        {
            var info = Measures.FirstOrDefault(x => x.Equals(measure));
            if (info == null)
            {
                measure.ID = Handler.Measure.Add(measure);
                Measures.Add(measure);
            }
            else
            {
                measure.ID = info.ID;
            }
        } //End method
        

        /// <summary> This method returns the Product object that matches the one passed into the function. </summary>
        /// <param name="Product"> A Container Product object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public void GetProduct(Cont.Product Product)
        {
            var info = Products.FirstOrDefault(x => x.Equals(Product));
            if (info == null)
            {
                Product.ID = Handler.Product.Add(Product);
                Products.Add(Product);
            }
            else
            {
                Product.ID = info.ID;
            }
        } //End method


        /// <summary> This method returns the Region object that matches the one passed into the function. </summary>
        /// <param name="Region"> A Container Region object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public void GetRegion(Cont.Region Region)
        {
            var info = Regions.FirstOrDefault(x => x.Equals(Region));
            if (info == null)
            {
                Region.ID = Handler.Region.Add(Region);
                Regions.Add(Region);
            } else
            {
                Region.ID = info.ID;
            }
        } //End method


        /// <summary> This method returns the SKU object that matches the one passed into the function. </summary>
        /// <param name="SKU"> A Container SKU object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public void GetSKU(Cont.SKU SKU)
        {
            var info = SKUs.FirstOrDefault(x => x.Equals(SKU));
            if (info == null)
            {
                SKU.ID = Handler.SKU.Add(SKU);
                SKUs.Add(SKU);
            }
            else
            {
                SKU.ID = info.ID;
            }
        } //End method
        #endregion

        #region Members
        static List<Cont.Configuration> Configurations = null;
        static List<Cont.Measure> Measures = null;
        static List<Cont.Product> Products = null;
        static List<Cont.Region> Regions = null;
        static List<Cont.SKU> SKUs = null;
        #endregion
    }//end class
}//end namespace