using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Cost
    {
        #region Methods
        /// <summary>
        /// Returns the Cost ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Cost object we want retrieved. </param>
        /// <returns> A Cost business container with Cost ID record information. </returns>
        public static Biz.Containers.Cost Get(int ID)
        {
            Data.Sql.CostSql CostSql = new Data.Sql.CostSql();
            Data.Containers.Cost data = CostSql.GetByID(ID);

            Biz.Containers.Cost toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Cost items
        /// </summary>
        /// <returns> An array of Cost Business containers. </returns>
        public static Biz.Containers.Cost[] GetAll()
        {
            Data.Sql.CostSql CostSql = new Data.Sql.CostSql();
            Data.Containers.Cost[] data = CostSql.GetAll();

            List<Biz.Containers.Cost> toReturn = new List<Biz.Containers.Cost>();
            foreach (var info in data)
            {
                Biz.Containers.Cost Cost = ConvertFromDataContainer(info);
                toReturn.Add(Cost);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a CostSql container.
        /// </summary>
        /// <param name="info"> The Cost business container that needs to be added. </param>
        /// <returns> The Cost ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Cost info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.CostSql CostSql = new Data.Sql.CostSql();
            int toReturn = CostSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the CostSql container.
        /// </summary>
        /// <param name="info"> The Cost container object we want to update. </param>
        public static void Update(Biz.Containers.Cost info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.CostSql CostSql = new Data.Sql.CostSql();
            CostSql.Update(data);
        }//end method

        /// <summary> Deletes an Cost record via CostSql container. </summary>
        /// <param name="ID"> The ID of the Cost record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.CostSql CostSql = new Data.Sql.CostSql();
            CostSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Cost object. </param>
        /// <returns> The business container Cost object. </returns>
        internal static Biz.Containers.Cost ConvertFromDataContainer(Data.Containers.Cost data)
        {
            return new Biz.Containers.Cost()
            {
                ID = data.ID,
                Iteration = Iteration.ConvertFromDataContainer(Data.Sql.IterationSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.IterationID)),
                Date = data.Date,
                CurrentCost = data.CurrentCost,
                CostNext1 = data.CostNext1,
                CostNext2 = data.CostNext2,
                CostNext3 = data.CostNext3
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Cost object. </param>
        /// <returns> The data container Cost object. </returns>
        internal static Data.Containers.Cost ConvertToDataContainer(Biz.Containers.Cost info)
        {
            return new Data.Containers.Cost()
            {
                ID = info.ID,
                IterationID = info.Iteration.ID,
                Date = info.Date,
                CurrentCost = info.CurrentCost,
                CostNext1 = info.CostNext1,
                CostNext2 = info.CostNext2,
                CostNext3 = info.CostNext3
            };
        }//end method
        #endregion

    } //End class Cost
} //End namespace