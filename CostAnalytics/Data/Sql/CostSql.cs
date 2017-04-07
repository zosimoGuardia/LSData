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
    class CostSql
    {
        #region Members
        static Lazy<List<Cont.Cost>> m_CachedValues = null;
        #endregion

        #region Properties
        // <summary> Property for m_CachedValues </summary>
        public static Lazy<List<Cont.Cost>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    CostSql sql = new CostSql();
                    m_CachedValues = new Lazy<List<Cont.Cost>>(() => new List<Cont.Cost>(sql.GetAll()));
                }
                return m_CachedValues;
            }
            set { m_CachedValues = value; }
        }
        #endregion

        #region Standard Methods
        /** <summary> Adds a Cost record to the database. </summary>
          * <param name="info"> The Cost record that needs adding. </param>
          * <param name="transaction"> The SQL transaction object. </param>
          * <returns> The DB record ID. </returns>
          **/
        public int Add(Cont.Cost info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@IterationID", SqlDbType.Int, info.IterationID, ParameterDirection.Input, true);
                sql.AddParameter("@Date", SqlDbType.Date, info.Date, ParameterDirection.Input, true);
                sql.AddParameter("@CurrentCost", SqlDbType.Float, info.CurrentCost, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext1", SqlDbType.Float, info.CostNext1, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext2", SqlDbType.Float, info.CostNext2, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext3", SqlDbType.Float, info.CostNext3, ParameterDirection.Input, true);

                sql.ExecuteSP(""); //TODO: Pass in the Stored Procedure Name
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

        /** <summary> Updates an existing DB record with provided information. </summary>
          * <param name="info"> The Cost record that needs updating. </param>
          * <param name="transaction"> The SQL transaction object. </param>
          **/
        public void Update(Cont.Cost info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@IterationID", SqlDbType.Int, info.IterationID, ParameterDirection.Input, true);
                sql.AddParameter("@Date", SqlDbType.Date, info.Date, ParameterDirection.Input, true);
                sql.AddParameter("@CurrentCost", SqlDbType.Float, info.CurrentCost, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext1", SqlDbType.Float, info.CostNext1, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext2", SqlDbType.Float, info.CostNext2, ParameterDirection.Input, true);
                sql.AddParameter("@CostNext3", SqlDbType.Float, info.CostNext3, ParameterDirection.Input, true);

                sql.ExecuteSP(""); //TODO: Pass in the Stored Procedure Name

                // Update cached values
                Cont.Cost cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
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

        /** <summary> This method deletes a Cost DB record. </summary>
          * <param name="ID"> The record ID you want removed. </param>
          * <param name="transaction"> The SQL Transaction object. </param>
          **/
        public void Delete(int ID, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);
                sql.ExecuteSP(""); //TODO: Pass in the Stored Procedure Name

                // Update cached values
                Cont.Cost cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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

        /** <summary> This method gets a Cost Record by ID. </summary>
          * <param name="ID"> The DB ID of the record you want to retrieve. </param>
          * <returns> The Cost record in Object-Oriented form. </returns>
          **/
        public Cont.Cost GetByID(int ID)
        {
            Cont.Cost toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
            if (toReturn == null)
            {
                SqlService sql = null;
                SqlDataReader reader = null;
                try
                {
                    sql = new SqlService();
                    sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);

                    reader = sql.ExecuteSPReader(""); //TODO: Pass in the Stored Procedure Name
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

        /** <summary> This method gets all Cost records from DB. </summary>
          * <returns> A list of Cost records in Object-Oriented form. </returns>
          **/
        public Cont.Cost[] GetAll()
        {
            Cont.Cost[] toReturn = new Cont.Cost[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader(""); //TODO: Pass in the Stored Procedure Name
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
        /** <summary> This method converts DB output to an Object-Oriented form. </summary>
          * <param name="reader"> The SQL Data reader object. </param>
          * <returns> An array of Cost objects. </returns>
          **/
        private Cont.Cost[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.Cost()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                IterationID = (int)Convert.ChangeType(!Convert.IsDBNull(row["IterationID"]) ? row["IterationID"] : null, typeof(int)),
                                Date = (DateTime)Convert.ChangeType(!Convert.IsDBNull(row["Date"]) ? row["Date"] : null, typeof(DateTime)),
                                CurrentCost = (float)Convert.ChangeType(!Convert.IsDBNull(row["CurrentCost"]) ? row["CurrentCost"] : null, typeof(float)),
                                CostNext1 = (float)Convert.ChangeType(!Convert.IsDBNull(row["CostNext1"]) ? row["CostNext1"] : null, typeof(float)),
                                CostNext2 = (float)Convert.ChangeType(!Convert.IsDBNull(row["CostNext2"]) ? row["CostNext2"] : null, typeof(float)),
                                CostNext3 = (float)Convert.ChangeType(!Convert.IsDBNull(row["CostNext3"]) ? row["CostNext3"] : null, typeof(float))
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

    } //end class

} //end namespace