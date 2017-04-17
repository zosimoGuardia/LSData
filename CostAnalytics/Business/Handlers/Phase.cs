using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Phase
    {
        #region Methods
        /// <summary>
        /// Returns the Phase ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Phase object we want retrieved. </param>
        /// <returns> A Phase business container with Phase ID record information. </returns>
        public static Biz.Containers.Phase Get(int ID)
        {
            Data.Sql.PhaseSql PhaseSql = new Data.Sql.PhaseSql();
            Data.Containers.Phase data = PhaseSql.GetByID(ID);

            Biz.Containers.Phase toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Phase items
        /// </summary>
        /// <returns> An array of Phase Business containers. </returns>
        public static Biz.Containers.Phase[] GetAll()
        {
            Data.Sql.PhaseSql PhaseSql = new Data.Sql.PhaseSql();
            Data.Containers.Phase[] data = PhaseSql.GetAll();

            List<Biz.Containers.Phase> toReturn = new List<Biz.Containers.Phase>();
            foreach (var info in data)
            {
                Biz.Containers.Phase Phase = ConvertFromDataContainer(info);
                toReturn.Add(Phase);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a PhaseSql container.
        /// </summary>
        /// <param name="info"> The Phase business container that needs to be added. </param>
        /// <returns> The Phase ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Phase info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.PhaseSql PhaseSql = new Data.Sql.PhaseSql();
            int toReturn = PhaseSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the PhaseSql container.
        /// </summary>
        /// <param name="info"> The Phase container object we want to update. </param>
        public static void Update(Biz.Containers.Phase info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.PhaseSql PhaseSql = new Data.Sql.PhaseSql();
            PhaseSql.Update(data);
        }//end method

        /// <summary> Deletes an Phase record via PhaseSql container. </summary>
        /// <param name="ID"> The ID of the Phase record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.PhaseSql PhaseSql = new Data.Sql.PhaseSql();
            PhaseSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Phase object. </param>
        /// <returns> The business container Phase object. </returns>
        internal static Biz.Containers.Phase ConvertFromDataContainer(Data.Containers.Phase data)
        {
            return new Biz.Containers.Phase()
            {
                ID = data.ID,
                Name = data.Name,
                Product = Product.ConvertFromDataContainer(Data.Sql.ProductSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.ProductID)),
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Phase object. </param>
        /// <returns> The data container Phase object. </returns>
        internal static Data.Containers.Phase ConvertToDataContainer(Biz.Containers.Phase info)
        {
            return new Data.Containers.Phase()
            {
                ID = info.ID,
                Name = info.Name,
                ProductID = info.Product.ID
            };
        }//end method
        #endregion

    } //End class Phase
} //End namespace