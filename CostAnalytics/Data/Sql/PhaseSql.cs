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
    public sealed class PhaseSql: BaseSql, Interfaces.IPhaseSql
    {
        #region Properties
        /// <summary>
        /// Property for m_CachedValues
        /// </summary>
        public static Lazy<List<Cont.Phase>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    PhaseSql sql = new PhaseSql();
                    m_CachedValues = new Lazy<List<Cont.Phase>>(() => new List<Cont.Phase>(sql.GetAll()));
                }
                return m_CachedValues;
            }
            set { m_CachedValues = value; }
        }
        #endregion

        #region Standard Methods
        /// <summary>
        ///  Adds a Phase record to the database. 
        /// </summary>
        /// <param name="info"> The Phase record that needs adding. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        /// <returns> The DB record ID. </returns>
        public int Add(Cont.Phase info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);
                sql.AddParameter("@ProductID", SqlDbType.Int, info.ProductID, ParameterDirection.Input, true);

                sql.ExecuteSP("AddPhase");
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
        /// <param name="info"> The Phase record that needs updating. </param>
        /// <param name="transaction"> The SQL transaction object. </param>
        public void Update(Cont.Phase info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);
                sql.AddParameter("@ProductID", SqlDbType.Int, info.ProductID, ParameterDirection.Input, true);

                sql.ExecuteSP("UpdatePhase");

                // Update cached values
                Cont.Phase cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
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
        ///  This method deletes a Phase DB record. 
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
                sql.ExecuteSP("DeletePhase");

                // Update cached values
                Cont.Phase cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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
        /// This method gets a Phase Record by ID. 
        /// </summary>
        /// <param name="ID"> The DB ID of the record you want to retrieve. </param>
        /// <returns> The Phase record in Object-Oriented form. </returns>
        public Cont.Phase GetByID(int ID)
        {
            Cont.Phase toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
            if (toReturn == null)
            {
                SqlService sql = null;
                SqlDataReader reader = null;
                try
                {
                    sql = new SqlService();
                    sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);

                    reader = sql.ExecuteSPReader("GetPhase");
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
        ///  This method gets all Phase records from DB. 
        /// </summary>
        /// <returns> A list of Phase records in Object-Oriented form. </returns>
        public Cont.Phase[] GetAll()
        {
            Cont.Phase[] toReturn = new Cont.Phase[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader("GetPhaseAll");
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
        /// This method converts DB output to an Object-Oriented form.
        /// </summary>
        /// <param name="reader">The SQL Data reader object.</param>
        /// <returns>An array of Phase objects.</returns>
        private Cont.Phase[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.Phase()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                Name = (string)Convert.ChangeType(!Convert.IsDBNull(row["Name"]) ? row["Name"] : null, typeof(string)),
                                ProductID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ProductID"]) ? row["ProductID"] : null, typeof(int))
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

        #region Members
        private static Lazy<List<Cont.Phase>> m_CachedValues = null;
        #endregion
    } //end class
} //end namespace
