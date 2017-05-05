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
            if (Phases == null)         { Phases = Handler.Phase.GetAll().ToList(); }
            if (Products == null)       { Products = Handler.Product.GetAll().ToList(); }
            if (Regions == null)        { Regions = Handler.Region.GetAll().ToList(); }
            if (SKUs == null)           { SKUs = Handler.SKU.GetAll().ToList(); }
        }
        #endregion

        #region Methods
        /// <summary> This method returns the Configuration object that matches the one passed into the function. </summary>
        /// <param name="Configuration"> A Container Configuration object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.Configuration GetConfiguration(Cont.Configuration Configuration)
        {
            Cont.Configuration toReturn = null;

            toReturn = Configurations.FirstOrDefault(x => x.Equals(Configuration));
            if (toReturn == null)
            {
                Configuration.ID = Handler.Configuration.Add(Configuration);
                Configurations.Add(Configuration);
                toReturn = Configuration;
            }
            return toReturn;
        } //End method GetConfiguration


        /// <summary> This method returns the Measure object that matches the one passed into the function. </summary>
        /// <param name="measure"> A Container Measure object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.Measure GetMeasure(Cont.Measure measure)
        {
            Cont.Measure toReturn = null;

            toReturn = Measures.FirstOrDefault(x => x.Equals(measure));
            if (toReturn == null)
            {
                measure.ID = Handler.Measure.Add(measure);
                Measures.Add(measure);
                toReturn = measure;
            }
            return toReturn;
        } //End method
        

        /// <summary> This method returns the Phase object that matches the one passed into the function. </summary>
        /// <param name="Phase"> A Container Phase object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.Phase GetPhase(Cont.Phase Phase)
        {
            Cont.Phase toReturn = null;

            toReturn = Phases.FirstOrDefault(x => x.Equals(Phase));
            if (toReturn == null)
            {
                Phase.ID = Handler.Phase.Add(Phase);
                Phases.Add(Phase);
                toReturn = Phase;
            }
            return toReturn;
        } //End method


        /// <summary> This method returns the Product object that matches the one passed into the function. </summary>
        /// <param name="Product"> A Container Product object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.Product GetProduct(Cont.Product Product)
        {
            Cont.Product toReturn = null;

            toReturn = Products.FirstOrDefault(x => x.Equals(Product));
            if (toReturn == null)
            {
                Product.ID = Handler.Product.Add(Product);
                Products.Add(Product);
                toReturn = Product;
            }
            return toReturn;
        } //End method


        /// <summary> This method returns the Region object that matches the one passed into the function. </summary>
        /// <param name="Region"> A Container Region object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.Region GetRegion(Cont.Region Region)
        {
            Cont.Region toReturn = null;

            toReturn = Regions.FirstOrDefault(x => x.Equals(Region));
            if (toReturn == null)
            {
                Region.ID = Handler.Region.Add(Region);
                Regions.Add(Region);
                toReturn = Region;
            }
            return toReturn;
        } //End method


        /// <summary> This method returns the SKU object that matches the one passed into the function. </summary>
        /// <param name="SKU"> A Container SKU object. </param>
        /// <returns> The matched object if found, otherwise adds it to the DB and return the object. </returns>
        public Cont.SKU GetSKU(Cont.SKU SKU)
        {
            Cont.SKU toReturn = null;

            toReturn = SKUs.FirstOrDefault(x => x.Equals(SKU));
            if (toReturn == null)
            {
                SKU.ID = Handler.SKU.Add(SKU);
                SKUs.Add(SKU);
                toReturn = SKU;
            }
            return toReturn;
        } //End method
        #endregion

        #region Members
        static List<Cont.Configuration> Configurations = null;
        static List<Cont.Measure> Measures = null;
        static List<Cont.Phase> Phases = null;
        static List<Cont.Product> Products = null;
        static List<Cont.Region> Regions = null;
        static List<Cont.SKU> SKUs = null;
        #endregion
    }//end class
}//end namespace