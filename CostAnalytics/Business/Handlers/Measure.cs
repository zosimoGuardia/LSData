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
        /// Returns all instances for ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Biz.Containers.Measure Get(int ID)
        {
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            Data.Containers.Measure data = measureSql.GetByID(ID);

            Biz.Containers.Measure toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns></returns>
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
        /// Returns the ID of the item added to the database
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int Add(Biz.Containers.Measure data)
        {
            var info = ConvertToDataContainer(data);
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            int toReturn = measureSql.Add(info);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="data"></param>
        public static void Update(Biz.Containers.Measure data)
        {
            var info = ConvertToDataContainer(data);
            Data.Sql.MeasureSql measureSql = new Data.Sql.MeasureSql();
            measureSql.Update(info);
        }//end method

        //Deletes an instance
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
        /// <param name="data"></param>
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
        /// <param name="info"></param>
        /// <returns></returns>
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
