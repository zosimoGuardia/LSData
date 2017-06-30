using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Warranty
    {
        #region Methods
        /// <summary> Returns the Warranty ID instance that matches this ID </summary>
        /// <param name="ID">The ID of a Warranty object we want retrieved. </param>
        /// <returns> A Warranty business container with Warranty ID record information. </returns>
        public static Biz.Containers.Warranty Get(int ID)
        {
            Data.Sql.WarrantySql WarrantySql = new Data.Sql.WarrantySql();
            Data.Containers.Warranty data = WarrantySql.GetByID(ID);

            Biz.Containers.Warranty toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary> Returns all Warranty items. </summary>
        /// <returns> An array of Warranty Business containers. </returns>
        public static Biz.Containers.Warranty[] GetAll()
        {
            Data.Sql.WarrantySql WarrantySql = new Data.Sql.WarrantySql();
            Data.Containers.Warranty[] data = WarrantySql.GetAll();

            List<Biz.Containers.Warranty> toReturn = new List<Biz.Containers.Warranty>();
            foreach (var info in data)
            {
                Biz.Containers.Warranty Warranty = ConvertFromDataContainer(info);
                toReturn.Add(Warranty);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary> Adds a record to the database via a WarrantySql container. </summary>
        /// <param name="info"> The Warranty business container that needs to be added. </param>
        /// <returns> The Warranty ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Warranty info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.WarrantySql WarrantySql = new Data.Sql.WarrantySql();
            int toReturn = WarrantySql.Add(data);
            return toReturn;
        }//end method

        /// <summary> Updates a record via the WarrantySql container. </summary>
        /// <param name="info"> The Warranty container object we want to update. </param>
        public static void Update(Biz.Containers.Warranty info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.WarrantySql WarrantySql = new Data.Sql.WarrantySql();
            WarrantySql.Update(data);
        }//end method

        /// <summary> Deletes an Warranty record via WarrantySql container. </summary>
        /// <param name="ID"> The ID of the Warranty record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.WarrantySql WarrantySql = new Data.Sql.WarrantySql();
            WarrantySql.Delete(ID);
        } //End method
        #endregion

        #region Conversion Methods
        /// <summary> Converts Data Container object to Business Container object. </summary>
        /// <param name="data"> The data container Warranty object. </param>
        /// <returns> The business container Warranty object. </returns>
        internal static Biz.Containers.Warranty ConvertFromDataContainer(Data.Containers.Warranty data)
        {
            return new Biz.Containers.Warranty()
            {
                ID = data.ID,
                Name = data.Name,
                Description = data.Description
            };
        }//end Method

        /// <summary> Converts Business Container object to Data Container object. </summary>
        /// <param name="info">The business container Warranty object. </param>
        /// <returns> The data container Warranty object. </returns>
        internal static Data.Containers.Warranty ConvertToDataContainer(Biz.Containers.Warranty info)
        {
            return new Data.Containers.Warranty()
            {
                ID = info.ID,
                Name = info.Name,
                Description = info.Description
            };
        }//end method
        #endregion

    } //End class Warranty

} //End namespace