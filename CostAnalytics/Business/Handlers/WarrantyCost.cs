using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class WarrantyCost
    {
        #region Methods
        /// <summary> Returns the WarrantyCost ID instance that matches this ID. </summary>
        /// <param name="ID">The ID of a WarrantyCost object we want retrieved. </param>
        /// <returns> A WarrantyCost business container with WarrantyCost ID record information. </returns>
        public static Biz.Containers.WarrantyCost Get(int id)
        {
            Data.Sql.WarrantyCostSql WarrantyCostSql = new Data.Sql.WarrantyCostSql();
            Data.Containers.WarrantyCost data = WarrantyCostSql.GetByID(id);

            Biz.Containers.WarrantyCost toReturn = ConvertFromDataContainer(data);

            return toReturn;

        } //End GetByID method

        /// <summary> Returns all WarrantyCost items </summary>
        /// <returns> An array of WarrantyCost Business containers. </returns>
        public static Biz.Containers.WarrantyCost[] GetAll()
        {
            Data.Sql.WarrantyCostSql WarrantyCostSql = new Data.Sql.WarrantyCostSql();
            Data.Containers.WarrantyCost[] data = WarrantyCostSql.GetAll();

            List<Biz.Containers.WarrantyCost> toReturn = new List<Biz.Containers.WarrantyCost>();

            foreach (var info in data)
            {
                Biz.Containers.WarrantyCost WarrantyCost = ConvertFromDataContainer(info);
                toReturn.Add(WarrantyCost);
            }

            return toReturn.ToArray();
        } //End GetAll method

        /// <summary> Adds a record to the database via a WarrantyCostSql container. </summary>
        /// <param name="info"> The WarrantyCost business container that needs to be added. </param>
        /// <returns> The WarrantyCost ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.WarrantyCost info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.WarrantyCostSql WarrantyCostSql = new Data.Sql.WarrantyCostSql();
            int toReturn = WarrantyCostSql.Add(data);
            return toReturn;

        } //End Add method

        /// <summary> Updates a record via the WarrantyCostSql container. </summary>
        /// <param name="info"> The WarrantyCost container object we want to update. </param>
        public static void Update(Biz.Containers.WarrantyCost info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.WarrantyCostSql WarrantyCostSql = new Data.Sql.WarrantyCostSql();
            WarrantyCostSql.Update(data);

        } //End update method

        /// <summary> Deletes an WarrantyCost record via WarrantyCostSql container. </summary>
        /// <param name="ID"> The ID of the WarrantyCost record you want deleted. </param>
        public static void Delete(int id)
        {
            Data.Sql.WarrantyCostSql WarrantyCostSql = new Data.Sql.WarrantyCostSql();
            WarrantyCostSql.Delete(id);
        } //End Delete method
        #endregion

        #region Conversion Methods
        /// <summary> Converts Data Container object to Business Container object. </summary>
        /// <param name="data"> The data container Iteration object. </param>
        /// <returns> The business container Iteration object. </returns>
        internal static Biz.Containers.WarrantyCost ConvertFromDataContainer(Data.Containers.WarrantyCost data)
        {
            return new Biz.Containers.WarrantyCost()
            {
                ID = data.ID,
                Date = data.Date,
                Cost = data.Cost,
                Warranty = Warranty.ConvertFromDataContainer(Data.Sql.WarrantySql.CachedValues.Value.FirstOrDefault(x => x.ID == data.WarrantyID)),
                Configuration = Configuration.ConvertFromDataContainer(Data.Sql.ConfigurationSql.CachedValues.Value.FirstOrDefault(x => x.ID == data.ConfigurationID)),
            };
        } //End method

        /// <summary> Converts Business Container object to Data Container object. </summary>
        /// <param name="info">The business container Iteration object. </param>
        /// <returns> The data container Iteration object. </returns>
        internal static Data.Containers.WarrantyCost ConvertToDataContainer(Biz.Containers.WarrantyCost info)
        {
            return new Data.Containers.WarrantyCost()
            {
                ID = info.ID,
                Date = info.Date,
                Cost = info.Cost,
                WarrantyID = info.Warranty.ID,
                ConfigurationID = info.Configuration.ID
            };
        } //End method
        #endregion
    } //End class

} //End namespace
