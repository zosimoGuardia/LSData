﻿using Dell.CostAnalytics.Data.Sql.Common;
using Cont = Dell.CostAnalytics.Data.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Sql
{
    public sealed class SKUSql: BaseSql, Interfaces.ISKUSql
    {
        #region Properties
        /// <summary>
        /// Property for m_CachedValues
        /// </summary>
        public static Lazy<List<Cont.SKU>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    SKUSql sql = new SKUSql();
                    m_CachedValues = new Lazy<List<Cont.SKU>>(() => new List<Cont.SKU>(sql.GetAll()));
                }
                return m_CachedValues;
            }
            set { m_CachedValues = value; }
        }
        #endregion

        #region Standard Methods
        /// <summary>
        ///  Adds a SKU record to the database. 
        /// </summary>
        /// <param name="info"> The SKU record that needs adding. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        /// <returns> The DB record ID. </returns>
        public int Add(Cont.SKU info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);
                sql.AddParameter("@Description", SqlDbType.VarChar, info.Description, 50, ParameterDirection.Input, true);
                sql.AddParameter("@Commodity", SqlDbType.VarChar, info.Commodity, 50, ParameterDirection.Input, true);

                sql.ExecuteSP("AddSKU");
                SqlParameter param = sql.ResultParameters["@ID"];
                info.ID = Convert.ToInt32(param.Value);

                // Update cached values
                CachedValues.Value.Add(info);
            }//end try
            catch (Exception exc)
            {
                throw exc;
            }//end catch
            finally
            {
                if (sql != null)
                    sql.Disconnect();
            }//end finally

            return info.ID;
        } //End Add method

        /// <summary>
        ///  Updates an existing DB record with provided information. 
        /// </summary>
        /// <param name="info"> The SKU record that needs updating. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        public void Update(Cont.SKU info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);
                sql.AddParameter("@Description", SqlDbType.VarChar, info.Description, 50, ParameterDirection.Input, true);
                sql.AddParameter("@Commodity", SqlDbType.VarChar, info.Commodity, 50, ParameterDirection.Input, true);

                sql.ExecuteSP("UpdateSKU");

                // Update cached values
                Cont.SKU cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
                if (cacheItemToRemove != null)
                {
                    CachedValues.Value.Remove(cacheItemToRemove);  //Remove item
                    CachedValues.Value.Add(info);  //Add item
                }//end if
            }//end try
            catch (Exception exc)
            {
                throw exc;
            }//end catch
            finally
            {
                if (sql != null)
                    sql.Disconnect();
            }//end finally
        } //End update method

        /// <summary>
        ///  This method deletes a SKU DB record. 
        /// </summary>
        /// <param name="ID"> The record ID you want removed. </param>
        /// <param name="transaction"> The SQL Transaction object. </param>
        public void Delete(int ID, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);
                sql.ExecuteSP("DeleteSKU");

                // Update cached values
                Cont.SKU cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
                if (cacheItemToRemove != null)
                {
                    CachedValues.Value.Remove(cacheItemToRemove);  //Remove item
                }//end if
            }//end try
            catch (Exception exc)
            {
                throw exc;
            }//end catch
            finally
            {
                if (sql != null)
                    sql.Disconnect();
            }//end finally
        } //End Delete method.

        /// <summary>
        ///  This method gets a SKU Record by ID. 
        /// </summary>
        /// <param name="ID"> The DB ID of the record you want to retrieve. </param>
        /// <returns> The SKU record in Object-Oriented form. </returns>
        public Cont.SKU GetByID(int ID)
        {
            Cont.SKU toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
            if (toReturn == null)
            {
                SqlService sql = null;
                SqlDataReader reader = null;
                try
                {
                    sql = new SqlService();
                    sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);

                    reader = sql.ExecuteSPReader("GetSKU");
                    toReturn = ConvertToContainer(reader).FirstOrDefault(); //MARK: Make sure row IDs corresponds to Database fields

                    //Append to cached values
                    if (toReturn != null)
                    {
                        CachedValues.Value.Add(toReturn);
                    }//endif
                }//end try
                catch (Exception exc)
                {
                    throw exc;
                }//end catch
                finally
                {
                    if (reader != null && !reader.IsClosed)
                        reader.Close();
                    if (sql != null)
                        sql.Disconnect();
                }//end finally
            } //end if
            return toReturn;
        } //End method getByID
        
        /// <summary>
        ///  This method gets all SKU records from DB. 
        /// </summary>
        /// <returns> A list of SKU records in Object-Oriented form. </returns>
        public Cont.SKU[] GetAll()
        {
            Cont.SKU[] toReturn = new Cont.SKU[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader("GetSKUAll");
                toReturn = ConvertToContainer(reader); //MARK: Make sure row IDs corresponds to Database fields
            }//end try
            catch (Exception exc)
            {
                throw exc;
            }//end catch
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                if (sql != null)
                    sql.Disconnect();
            }//end finally

            return toReturn;
        } //End method getAll
        #endregion

        #region Custom Methods
        /// <summary>
        ///  This method converts DB output to an Object-Oriented form. 
        /// </summary>
        /// <param name="reader"> The SQL Data reader object. </param>
        /// <returns> An array of SKU objects. </returns>
        private Cont.SKU[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.SKU()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                Name = (string)Convert.ChangeType(!Convert.IsDBNull(row["Name"]) ? row["Name"] : null, typeof(string)),
                                Description = (string)Convert.ChangeType(!Convert.IsDBNull(row["Description"]) ? row["Description"] : null, typeof(string)),
                                Commodity = (string)Convert.ChangeType(!Convert.IsDBNull(row["Commodity"]) ? row["Commodity"] : null, typeof(string))
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

        #region Members
        private static Lazy<List<Cont.SKU>> m_CachedValues = null;
        #endregion
    } //end class
} //end namespace
