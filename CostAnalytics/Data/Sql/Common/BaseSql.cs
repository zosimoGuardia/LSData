using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Sql.Common
{
    /// <summary>
    /// Base SQL Class
    /// </summary>
    public class BaseSql
    {

        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        public BaseSql()
        {
        }//end BaseSql
        #endregion

        #region Methods
        /// <summary>
        /// Flushes data when needed cache
        /// </summary>
        public static void FlushDataCache()
        {
            Data.Sql.MeasureSql.CachedValues = null;
            Data.Sql.ConfigurationSql.CachedValues = null;
            Data.Sql.CostSql.CachedValues = null;
            Data.Sql.IterationSql.CachedValues = null;
            Data.Sql.PhaseSql.CachedValues = null;
            Data.Sql.ProductSql.CachedValues = null;
            Data.Sql.RegionSql.CachedValues = null;
            Data.Sql.SKUSql.CachedValues = null;
        }//end method

        /// <summary>
        /// Populates the provided object using the provided reader
        /// </summary>
        /// <param name="objectToUpdate"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected static object GetObjectFromReader(object objectToUpdate, SqlDataReader reader)
        {
            Type myType = Type.GetType(objectToUpdate.GetType().AssemblyQualifiedName);  //Get object type

            //Retrieve object properties via reflection and update each one.  The data reader's column must match the property name
            PropertyInfo[] properties = myType.GetProperties();

            for (int cntProperties = 0; cntProperties < properties.Length; cntProperties++)
            {

                PropertyInfo propertyInfo = (PropertyInfo)properties[cntProperties];  //Cast property

                object defaultValue = PrepareDefaultValue(propertyInfo.GetValue(objectToUpdate, null));  //Get default value

                try
                {
                    //Set value for property
                    if (propertyInfo.CanWrite)
                    {
                        if (reader[propertyInfo.Name] != null && !Convert.IsDBNull(reader[propertyInfo.Name]))
                        {
                            propertyInfo.SetValue(objectToUpdate, Convert.ChangeType(((defaultValue is System.Boolean || defaultValue is System.Guid || defaultValue is System.Byte[]) ? reader[propertyInfo.Name] : reader[propertyInfo.Name].ToString().TrimEnd()), propertyInfo.PropertyType), null);
                        }//end if
                        else
                        {
                            propertyInfo.SetValue(objectToUpdate, Convert.ChangeType(defaultValue, propertyInfo.PropertyType), null);
                        }//end else
                    }//end if
                }//end try
                catch (IndexOutOfRangeException)
                {
                    //If we hit an out of range exception then set property with default value
                    try
                    {
                        if (propertyInfo.CanWrite)
                            propertyInfo.SetValue(objectToUpdate, Convert.ChangeType(defaultValue, propertyInfo.PropertyType), null);
                    }
                    catch (InvalidCastException)
                    {
                        //If we have a cast failure just return object as is
                        return objectToUpdate;
                    }//end catch
                }//end catch
            }//end for
            return objectToUpdate;
        }//end method

        /// <summary>
        /// Prepares default values based on the provided type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static object PrepareDefaultValue(object value)
        {
            //Attempt to return type-specific default value.  Otherwise return empty string by default
            if (value is System.String)
            {
                return string.Empty;
            }//end if
            else if ((value is System.Int16) || (value is System.Int32) || (value is System.Int64) ||
                (value is Single) || (value is Double) || (value is Decimal) || (value is System.Byte))
            {
                return 0;
            }//end else if
            else if (value is System.Boolean)
            {
                return false;
            }//end else if
            else if (value is System.DateTime)
            {
                return "1/1/0001 12:00:00 AM";
            }//end else if
            else if (value is System.Guid)
            {
                return new System.Guid("00000000000000000000000000000000");
            }//end else if
            else if (value is System.Byte[])
            {
                return new System.Byte[0];
            }//end else if
            else
            {
                return string.Empty;
            }//end else if
        }//end method

        #endregion
    }//end class
}//end namespace
