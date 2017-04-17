using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Iteration
    {
        #region Methods
        /// <summary>
        /// Returns the Iteration ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Iteration object we want retrieved. </param>
        /// <returns> A Iteration business container with Iteration ID record information. </returns>
        public static Biz.Containers.Iteration Get(int ID)
        {
            Data.Sql.IterationSql IterationSql = new Data.Sql.IterationSql();
            Data.Containers.Iteration data = IterationSql.GetByID(ID);

            Biz.Containers.Iteration toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Iteration items
        /// </summary>
        /// <returns> An array of Iteration Business containers. </returns>
        public static Biz.Containers.Iteration[] GetAll()
        {
            Data.Sql.IterationSql IterationSql = new Data.Sql.IterationSql();
            Data.Containers.Iteration[] data = IterationSql.GetAll();

            List<Biz.Containers.Iteration> toReturn = new List<Biz.Containers.Iteration>();
            foreach (var info in data)
            {
                Biz.Containers.Iteration Iteration = ConvertFromDataContainer(info);
                toReturn.Add(Iteration);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a IterationSql container.
        /// </summary>
        /// <param name="info"> The Iteration business container that needs to be added. </param>
        /// <returns> The Iteration ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Iteration info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.IterationSql IterationSql = new Data.Sql.IterationSql();
            int toReturn = IterationSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the IterationSql container.
        /// </summary>
        /// <param name="info"> The Iteration container object we want to update. </param>
        public static void Update(Biz.Containers.Iteration info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.IterationSql IterationSql = new Data.Sql.IterationSql();
            IterationSql.Update(data);
        }//end method

        /// <summary> Deletes an Iteration record via IterationSql container. </summary>
        /// <param name="ID"> The ID of the Iteration record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.IterationSql IterationSql = new Data.Sql.IterationSql();
            IterationSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Iteration object. </param>
        /// <returns> The business container Iteration object. </returns>
        internal static Biz.Containers.Iteration ConvertFromDataContainer(Data.Containers.Iteration data)
        {
            return new Biz.Containers.Iteration()
            {
                ID = data.ID,
                Region = Region.ConvertFromDataContainer(Data.Sql.RegionSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.RegionID)),
                Product = Product.ConvertFromDataContainer(Data.Sql.ProductSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.ProductID)),
                Configuration = Configuration.ConvertFromDataContainer(Data.Sql.ConfigurationSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.ConfigurationID)),
                SKU = SKU.ConvertFromDataContainer(Data.Sql.SKUSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.SKUID)),
                Measure = Measure.ConvertFromDataContainer(Data.Sql.MeasureSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.MeasureID))
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Iteration object. </param>
        /// <returns> The data container Iteration object. </returns>
        internal static Data.Containers.Iteration ConvertToDataContainer(Biz.Containers.Iteration info)
        {
            return new Data.Containers.Iteration()
            {
                ID = info.ID,
                RegionID = info.Region.ID,
                ProductID = info.Product.ID,
                ConfigurationID = info.Configuration.ID,
                SKUID = info.SKU.ID,
                MeasureID = info.Measure.ID
            };
        }//end method
        #endregion

    } //End class Iteration
} //End namespace