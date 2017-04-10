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
        /// Returns all instances for ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Biz.Containers.Iteration Get(int ID)
        {
            Data.Sql.IterationSql iterationSql = new Data.Sql.IterationSql();
            Data.Containers.Iteration data = iterationSql.GetByID(ID);

            Biz.Containers.Iteration toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns></returns>
        public static Biz.Containers.Iteration[] GetAll()
        {
            Data.Sql.IterationSql iterationSql = new Data.Sql.IterationSql();
            Data.Containers.Iteration[] data = iterationSql.GetAll();

            List<Biz.Containers.Iteration> toReturn = new List<Biz.Containers.Iteration>();
            foreach(var info in data)
            {
                Biz.Containers.Iteration iteration = ConvertFromDataContainer(info);
                toReturn.Add(iteration);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Returns the ID of the item added to the database
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int Add(Biz.Containers.Iteration data)
        {
            var info = ConvertToDataContainer(data);
            Data.Sql.IterationSql iterationSql = new Data.Sql.IterationSql();
            int toReturn = iterationSql.Add(info);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="data"></param>
        public static void Update(Biz.Containers.Iteration data)
        {
            var info = ConvertToDataContainer(data);
            Data.Sql.IterationSql iterationSql = new Data.Sql.IterationSql();
            iterationSql.Update(info);
        }//end method

        //Deletes an instance
        public static void Delete(int ID)
        {
            Data.Sql.IterationSql iterationSql = new Data.Sql.IterationSql();
            iterationSql.Delete(ID);
        }//end method
        #endregion
        
        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"></param>
        internal static Biz.Containers.Iteration ConvertFromDataContainer(Data.Containers.Iteration data)
        {
            return new Biz.Containers.Iteration()
            {
                ID = data.ID,
                Configuration = null, //TODO
                Measure =  Measure.ConvertFromDataContainer(Data.Sql.MeasureSql.CachedValues.Value.FirstOrDefault( x=>x.ID == data.MeasureID)),
                Product = null, //TODO
                Region = null, // TODO
                SKU = null //TODO
                //TODO: Complete conversion fo all other properties
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal static Data.Containers.Iteration ConvertToDataContainer(Biz.Containers.Iteration info)
        {
            return new Data.Containers.Iteration()
            {
                ID = info.ID,
                ConfigurationID = info.Configuration.ID,
                MeasureID = info.Measure.ID,
                ProductID = info.Product.ID,
                RegionID = info.Region.ID,
                SKUID = info.SKU.ID
            };
        }//end method
        #endregion

    }
}
