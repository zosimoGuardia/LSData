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
    public sealed class IterationSql:BaseSql, Interfaces.IIterationSql
    {
        #region Properties
        /// <summary>
        /// Property for m_CachedValues
        /// </summary>
        public static Lazy<List<Cont.Iteration>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    IterationSql sql = new IterationSql();
                    m_CachedValues = new Lazy<List<Cont.Iteration>>(() => new List<Cont.Iteration>(sql.GetAll()));
                }
                return m_CachedValues;
            }
            set { m_CachedValues = value; }
        }
        #endregion

        #region Standard Methods
        /// <summary>
        ///  Adds a Iteration record to the database. 
        /// </summary>
        /// <param name="info"> The Iteration record that needs adding. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        /// <returns> The DB record ID. </returns>
        public int Add(Cont.Iteration info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@RegionID", SqlDbType.Int, info.RegionID, ParameterDirection.Input, true);
                sql.AddParameter("@ProductID", SqlDbType.Int, info.ProductID, ParameterDirection.Input, true);
                sql.AddParameter("@ConfigurationID", SqlDbType.Int, info.ConfigurationID, ParameterDirection.Input, true);
                sql.AddParameter("@SKUID", SqlDbType.Int, info.SKUID, ParameterDirection.Input, true);
                sql.AddParameter("@MeasureID", SqlDbType.Int, info.MeasureID, ParameterDirection.Input, true);

                sql.ExecuteSP("AddIteration");
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
        /// <param name="info"> The Iteration record that needs updating. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        public void Update(Cont.Iteration info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@RegionID", SqlDbType.Int, info.RegionID, ParameterDirection.Input, true);
                sql.AddParameter("@ProductID", SqlDbType.Int, info.ProductID, ParameterDirection.Input, true);
                sql.AddParameter("@ConfigurationID", SqlDbType.Int, info.ConfigurationID, ParameterDirection.Input, true);
                sql.AddParameter("@SKUID", SqlDbType.Int, info.SKUID, ParameterDirection.Input, true);
                sql.AddParameter("@MeasureID", SqlDbType.Int, info.MeasureID, ParameterDirection.Input, true);

                sql.ExecuteSP("UpdateIteration");

                // Update cached values
                Cont.Iteration cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
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
        ///  This method deletes a Iteration DB record. 
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
                sql.ExecuteSP("DeleteIteration"); //TODO: Pass in the Stored Procedure Name

                // Update cached values
                Cont.Iteration cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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

        /// <summary>
        ///  This method gets a Iteration Record by ID. 
        /// </summary>
        /// <param name="ID"> The DB ID of the record you want to retrieve. </param>
        /// <returns> The Iteration record in Object-Oriented form. </returns>
        public Cont.Iteration GetByID(int ID)
        {
            Cont.Iteration toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
            if (toReturn == null)
            {
                SqlService sql = null;
                SqlDataReader reader = null;
                try
                {
                    sql = new SqlService();
                    sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);

                    reader = sql.ExecuteSPReader("GetIteration");
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
        ///  This method gets all Iteration records from DB. 
        /// </summary>
        /// <returns> A list of Iteration records in Object-Oriented form. </returns>
        public Cont.Iteration[] GetAll()
        {
            Cont.Iteration[] toReturn = new Cont.Iteration[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader("GetIterationAll");
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
        /// This method converts DB output to an Object-Oriented form.
        /// </summary>
        /// <param name="reader">The SQL Data reader object.</param>
        /// <returns>An array of Iteration objects.</returns>
        private Cont.Iteration[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.Iteration()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                RegionID = (int)Convert.ChangeType(!Convert.IsDBNull(row["RegionID"]) ? row["RegionID"] : null, typeof(int)),
                                ProductID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ProductID"]) ? row["ProductID"] : null, typeof(int)),
                                ConfigurationID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ConfigurationID"]) ? row["ConfigurationID"] : null, typeof(int)),
                                SKUID = (int)Convert.ChangeType(!Convert.IsDBNull(row["SKUID"]) ? row["SKUID"] : null, typeof(int)),
                                MeasureID = (int)Convert.ChangeType(!Convert.IsDBNull(row["MeasureID"]) ? row["MeasureID"] : null, typeof(int))
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

        #region Members
        private static Lazy<List<Cont.Iteration>> m_CachedValues = null;
        #endregion
    } //end class
} //end namespace
