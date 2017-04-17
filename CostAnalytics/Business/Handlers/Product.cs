using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Product
    {
        #region Methods
        /// <summary>
        /// Returns the Product ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Product object we want retrieved. </param>
        /// <returns> A Product business container with Product ID record information. </returns>
        public static Biz.Containers.Product Get(int ID)
        {
            Data.Sql.ProductSql ProductSql = new Data.Sql.ProductSql();
            Data.Containers.Product data = ProductSql.GetByID(ID);

            Biz.Containers.Product toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Product items
        /// </summary>
        /// <returns> An array of Product Business containers. </returns>
        public static Biz.Containers.Product[] GetAll()
        {
            Data.Sql.ProductSql ProductSql = new Data.Sql.ProductSql();
            Data.Containers.Product[] data = ProductSql.GetAll();

            List<Biz.Containers.Product> toReturn = new List<Biz.Containers.Product>();
            foreach (var info in data)
            {
                Biz.Containers.Product Product = ConvertFromDataContainer(info);
                toReturn.Add(Product);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a ProductSql container.
        /// </summary>
        /// <param name="info"> The Product business container that needs to be added. </param>
        /// <returns> The Product ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Product info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.ProductSql ProductSql = new Data.Sql.ProductSql();
            int toReturn = ProductSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the ProductSql container.
        /// </summary>
        /// <param name="info"> The Product container object we want to update. </param>
        public static void Update(Biz.Containers.Product info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.ProductSql ProductSql = new Data.Sql.ProductSql();
            ProductSql.Update(data);
        }//end method

        /// <summary> Deletes an Product record via ProductSql container. </summary>
        /// <param name="ID"> The ID of the Product record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.ProductSql ProductSql = new Data.Sql.ProductSql();
            ProductSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Product object. </param>
        /// <returns> The business container Product object. </returns>
        internal static Biz.Containers.Product ConvertFromDataContainer(Data.Containers.Product data)
        {
            return new Biz.Containers.Product()
            {
                ID = data.ID,
                Name = data.Name,
                LOB = data.LOB,
                Model = data.Model,
                Variant = data.Variant
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Product object. </param>
        /// <returns> The data container Product object. </returns>
        internal static Data.Containers.Product ConvertToDataContainer(Biz.Containers.Product info)
        {
            return new Data.Containers.Product()
            {
                ID = info.ID,
                Name = info.Name,
                LOB = info.LOB,
                Model = info.Model,
                Variant = info.Variant
            };
        }//end method
        #endregion

    } //End class Product
} //End namespace