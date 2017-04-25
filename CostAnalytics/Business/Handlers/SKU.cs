using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class SKU
    {
        #region Methods
        /// <summary>
        /// Returns the SKU ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a SKU object we want retrieved. </param>
        /// <returns> A SKU business container with SKU ID record information. </returns>
        public static Biz.Containers.SKU Get(int ID)
        {
            Data.Sql.SKUSql SKUSql = new Data.Sql.SKUSql();
            Data.Containers.SKU data = SKUSql.GetByID(ID);

            Biz.Containers.SKU toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all SKU items
        /// </summary>
        /// <returns> An array of SKU Business containers. </returns>
        public static Biz.Containers.SKU[] GetAll()
        {
            Data.Sql.SKUSql skuSql = new Data.Sql.SKUSql();
            Data.Containers.SKU[] data = skuSql.GetAll();

            List<Biz.Containers.SKU> toReturn = new List<Biz.Containers.SKU>();
            foreach (var info in data)
            {
                Biz.Containers.SKU sku = ConvertFromDataContainer(info);
                toReturn.Add(sku);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a SKUSql container.
        /// </summary>
        /// <param name="info"> The SKU business container that needs to be added. </param>
        /// <returns> The SKU ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.SKU info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.SKUSql skuSql = new Data.Sql.SKUSql();
            int toReturn = skuSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the SKUSql container.
        /// </summary>
        /// <param name="info"> The SKU container object we want to update. </param>
        public static void Update(Biz.Containers.SKU info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.SKUSql skuSql = new Data.Sql.SKUSql();
            skuSql.Update(data);
        }//end method

        /// <summary> Deletes an SKU record via SKUSql container. </summary>
        /// <param name="ID"> The ID of the SKU record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.SKUSql skuSql = new Data.Sql.SKUSql();
            skuSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container SKU object. </param>
        /// <returns> The business container SKU object. </returns>
        internal static Biz.Containers.SKU ConvertFromDataContainer(Data.Containers.SKU data)
        {
            return new Biz.Containers.SKU()
            {
                ID = data.ID,
                Name = data.Name,
                Description = data.Description,
                Commodity = data.Commodity
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container SKU object. </param>
        /// <returns> The data container SKU object. </returns>
        internal static Data.Containers.SKU ConvertToDataContainer(Biz.Containers.SKU info)
        {
            return new Data.Containers.SKU()
            {
                ID = info.ID,
                Name = info.Name,
                Description = info.Description,
                Commodity = info.Commodity
            };
        }//end method
        #endregion

    } //End class SKU
} //End namespace