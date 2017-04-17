using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Region
    {
        #region Methods
        /// <summary>
        /// Returns the Region ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Region object we want retrieved. </param>
        /// <returns> A Region business container with Region ID record information. </returns>
        public static Biz.Containers.Region Get(int ID)
        {
            Data.Sql.RegionSql RegionSql = new Data.Sql.RegionSql();
            Data.Containers.Region data = RegionSql.GetByID(ID);

            Biz.Containers.Region toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Region items
        /// </summary>
        /// <returns> An array of Region Business containers. </returns>
        public static Biz.Containers.Region[] GetAll()
        {
            Data.Sql.RegionSql RegionSql = new Data.Sql.RegionSql();
            Data.Containers.Region[] data = RegionSql.GetAll();

            List<Biz.Containers.Region> toReturn = new List<Biz.Containers.Region>();
            foreach (var info in data)
            {
                Biz.Containers.Region Region = ConvertFromDataContainer(info);
                toReturn.Add(Region);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a RegionSql container.
        /// </summary>
        /// <param name="info"> The Region business container that needs to be added. </param>
        /// <returns> The Region ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Region info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.RegionSql RegionSql = new Data.Sql.RegionSql();
            int toReturn = RegionSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the RegionSql container.
        /// </summary>
        /// <param name="info"> The Region container object we want to update. </param>
        public static void Update(Biz.Containers.Region info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.RegionSql RegionSql = new Data.Sql.RegionSql();
            RegionSql.Update(data);
        }//end method

        /// <summary> Deletes an Region record via RegionSql container. </summary>
        /// <param name="ID"> The ID of the Region record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.RegionSql RegionSql = new Data.Sql.RegionSql();
            RegionSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Region object. </param>
        /// <returns> The business container Region object. </returns>
        internal static Biz.Containers.Region ConvertFromDataContainer(Data.Containers.Region data)
        {
            return new Biz.Containers.Region()
            {
                ID = data.ID,
                RegionName = data.RegionName,
                Country = data.Country,
                CountryCode = data.CountryCode
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Region object. </param>
        /// <returns> The data container Region object. </returns>
        internal static Data.Containers.Region ConvertToDataContainer(Biz.Containers.Region info)
        {
            return new Data.Containers.Region()
            {
                ID = info.ID,
                RegionName = info.RegionName,
                Country = info.Country,
                CountryCode = info.CountryCode
            };
        }//end method
        #endregion

    } //End class Region
} //End namespace