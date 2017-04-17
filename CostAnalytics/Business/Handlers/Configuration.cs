using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Biz = Dell.CostAnalytics.Business;

namespace Dell.CostAnalytics.Business.Handlers
{
    public class Configuration
    {
        #region Methods
        /// <summary>
        /// Returns the Configuration ID instance that matches this ID
        /// </summary>
        /// <param name="ID">The ID of a Configuration object we want retrieved. </param>
        /// <returns> A Configuration business container with Configuration ID record information. </returns>
        public static Biz.Containers.Configuration Get(int ID)
        {
            Data.Sql.ConfigurationSql ConfigurationSql = new Data.Sql.ConfigurationSql();
            Data.Containers.Configuration data = ConfigurationSql.GetByID(ID);

            Biz.Containers.Configuration toReturn = ConvertFromDataContainer(data);

            return toReturn;
        }//end method

        /// <summary>
        /// Returns all Configuration items
        /// </summary>
        /// <returns> An array of Configuration Business containers. </returns>
        public static Biz.Containers.Configuration[] GetAll()
        {
            Data.Sql.ConfigurationSql ConfigurationSql = new Data.Sql.ConfigurationSql();
            Data.Containers.Configuration[] data = ConfigurationSql.GetAll();

            List<Biz.Containers.Configuration> toReturn = new List<Biz.Containers.Configuration>();
            foreach (var info in data)
            {
                Biz.Containers.Configuration Configuration = ConvertFromDataContainer(info);
                toReturn.Add(Configuration);
            }
            return toReturn.ToArray();
        }//end method

        /// <summary>
        /// Adds a record to the database via a ConfigurationSql container.
        /// </summary>
        /// <param name="info"> The Configuration business container that needs to be added. </param>
        /// <returns> The Configuration ID of the item added to the database. </returns>
        public static int Add(Biz.Containers.Configuration info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.ConfigurationSql ConfigurationSql = new Data.Sql.ConfigurationSql();
            int toReturn = ConfigurationSql.Add(data);
            return toReturn;
        }//end method

        /// <summary>
        /// Updates a record via the ConfigurationSql container.
        /// </summary>
        /// <param name="info"> The Configuration container object we want to update. </param>
        public static void Update(Biz.Containers.Configuration info)
        {
            var data = ConvertToDataContainer(info);
            Data.Sql.ConfigurationSql ConfigurationSql = new Data.Sql.ConfigurationSql();
            ConfigurationSql.Update(data);
        }//end method

        /// <summary> Deletes an Configuration record via ConfigurationSql container. </summary>
        /// <param name="ID"> The ID of the Configuration record you want deleted. </param>
        public static void Delete(int ID)
        {
            Data.Sql.ConfigurationSql ConfigurationSql = new Data.Sql.ConfigurationSql();
            ConfigurationSql.Delete(ID);
        }//end method
        #endregion

        #region Conversion Methods
        /// <summary>
        /// Converts Data Container object to Business Container object
        /// </summary>
        /// <param name="data"> The data container Configuration object. </param>
        /// <returns> The business container Configuration object. </returns>
        internal static Biz.Containers.Configuration ConvertFromDataContainer(Data.Containers.Configuration data)
        {
            return new Biz.Containers.Configuration()
            {
                ID = data.ID,
                Name = data.Name,
                Type = data.Type
            };
        }//end Method

        /// <summary>
        /// Converts Business Container object to Data Container object 
        /// </summary>
        /// <param name="info">The business container Configuration object. </param>
        /// <returns> The data container Configuration object. </returns>
        internal static Data.Containers.Configuration ConvertToDataContainer(Biz.Containers.Configuration info)
        {
            return new Data.Containers.Configuration()
            {
                ID = info.ID,
                Name = info.Name,
                Type = info.Type
            };
        }//end method
        #endregion

    } //End class Configuration
} //End namespace