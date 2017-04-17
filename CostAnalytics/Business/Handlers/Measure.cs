using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Measure
    {
        #region Methods
        /// <summary>
        /// Returns the Measure ID instance that matches this ID
        /// </summary>
        /// <param name="ID"> The ID of the Measure object we want retrieved. </param>
        /// <returns> A Measure business container with SKU ID record information. </returns>
        public static Biz.Containers.Measure Get(int ID)
        {
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            Data.Containers.Measure data = measureSql.GetByID(ID);

            Biz.Containers.Measure toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Measure items.
        /// </summary>
        /// <returns> An array of Measure Business containers. </returns>
        public static Biz.Containers.Measure[] GetAll()
        {
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            Data.Containers.Measure[] data = measureSql.GetAll();

            List<Biz.Containers.Measure> toReturn = new List<Biz.Containers.Measure>();
            foreach(var info in data)
            {
                Biz.Containers.Measure measure = ConvertFromDataContainer(info);
                toReturn.Add(measure);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a MeasureSql container.
        /// </summary>
        /// <param name="info">The Measure business container that needs to be added.</param>
        /// <returns> The Measure ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Measure info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            int toReturn = measureSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the MeasureSql container.
        /// </summary>
        /// <param name="info"> The Measure container object we want to update. </param>
        public static void Update(Biz.Containers.Measure info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            measureSql.Update(data);
        }//end method

        /// <summary> Deletes an Measure record via MeasureSql container. </summary>
        /// <param name="ID"> The ID of the Measure record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            measureSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Measure object. </param>
        /// <returns> The business container Measure object. </returns>
        internal static Biz.Containers.Measure ConvertFromDataContainer(Data.Containers.Measure data)
        {
            return new Biz.Containers.Measure()
            {
                ID = data.ID,
                Name = data.Name
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Measure object. </param>
        /// <returns> The data container Measure object. </returns>
        internal static Data.Containers.Measure ConvertToDataContainer(Biz.Containers.Measure info)
        {
            return new Data.Containers.Measure()
            {
                ID = info.ID,
                Name = info.Name
            };
        }//end method
        #endregion

    }
}
