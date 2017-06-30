using Dell.CostAnalytics.Data.Sql.Common;
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
    public sealed class WarrantyCostSql : BaseSql, Interfaces.IWarrantyCostSql
    {
        #region Properties
        /// <summary> Property for m_CachedValues </summary>
        public static Lazy<List<Cont.WarrantyCost>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    WarrantyCostSql sql = new WarrantyCostSql();
                    m_CachedValues = new Lazy<List<Cont.WarrantyCost>>(() => new List<Cont.WarrantyCost>(sql.GetAll()));
                }
                return m_CachedValues;
            }
            set { m_CachedValues = value; }
        }
        #endregion

        #region Standard Methods
        /// <summary> Adds a WarrantyCost record to the database. </summary>
        /// <param name="info"> The WarrantyCost record that needs adding. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        /// <returns> The DB record ID. </returns>
        public int Add(Cont.WarrantyCost info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@Date", SqlDbType.Date, info.Date, ParameterDirection.Input, true);
                sql.AddParameter("@Cost", SqlDbType.Float, info.Cost, ParameterDirection.Input, true);
                sql.AddParameter("@WarrantyID", SqlDbType.Int, info.WarrantyID, ParameterDirection.Input, true);
                sql.AddParameter("@ConfigurationID", SqlDbType.Int, info.ConfigurationID, ParameterDirection.Input, true);

                sql.ExecuteSP("AddWarrantyCost");
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
        /// <param name="info"> The WarrantyCost record that needs updating. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        public void Update(Cont.WarrantyCost info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@Date", SqlDbType.Date, info.Date, ParameterDirection.Input, true);
                sql.AddParameter("@Cost", SqlDbType.Float, info.Cost, ParameterDirection.Input, true);
                sql.AddParameter("WarrantyID", SqlDbType.Int, info.WarrantyID, ParameterDirection.Input, true);
                sql.AddParameter("@ConfigurationID", SqlDbType.Int, info.ConfigurationID, ParameterDirection.Input, true);

                sql.ExecuteSP("UpdateWarrantyCost");

                // Update cached values
                Cont.WarrantyCost cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
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

        /// <summary> This method deletes a WarrantyCost DB record. </summary>
        /// <param name="ID"> The record ID you want removed. </param>
        /// <param name="transaction"> The SQL Transaction object. </param>
        public void Delete(int ID, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);
                sql.ExecuteSP("DeleteWarrantyCost");

                // Update cached values
                Cont.WarrantyCost cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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
        } //End Delete method

        /// <summary> This method gets a WarrantyCost Record by ID. </summary>
        /// <param name="ID"> The DB ID of the record you want to retrieve. </param>
        /// <returns> The WarrantyCost record in Object-Oriented form. </returns>
        public Cont.WarrantyCost GetByID(int ID)
        {
            Cont.WarrantyCost toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
            if (toReturn == null)
            {
                SqlService sql = null;
                SqlDataReader reader = null;
                try
                {
                    sql = new SqlService();
                    sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);

                    reader = sql.ExecuteSPReader("GetWarrantyCost");
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

        /// <summary> This method gets all WarrantyCost records from DB. </summary>
        /// <returns> A list of WarrantyCost records in Object-Oriented form. </returns>
        public Cont.WarrantyCost[] GetAll()
        {
            Cont.WarrantyCost[] toReturn = new Cont.WarrantyCost[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader("GetWarrantyCostAll");
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
        /// <summary> This method converts DB output to an Object-Oriented form. </summary>
        /// <param name="reader"> The SQL Data reader object. </param>
        /// <returns> An array of WarrantyCost objects. </returns>
        private Cont.WarrantyCost[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.WarrantyCost()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                Date = (DateTime)Convert.ChangeType(!Convert.IsDBNull(row["Date"]) ? row["Date"] : null, typeof(DateTime)),
                                Cost = (double)Convert.ChangeType(!Convert.IsDBNull(row["Cost"]) ? row["Cost"] : 0, typeof(double)),
                                WarrantyID = (int)Convert.ChangeType(!Convert.IsDBNull(row["WarrantyID"]) ? row["WarrantyID"] : null, typeof(int)),
                                ConfigurationID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ConfigurationID"]) ? row["ConfigurationID"] : null, typeof(int)),
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

        #region Members
        private static Lazy<List<Cont.WarrantyCost>> m_CachedValues = null;
        #endregion
    } //end class
} //end namespace
