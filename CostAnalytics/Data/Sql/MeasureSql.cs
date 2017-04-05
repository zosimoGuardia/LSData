﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dell.CostAnalytics.Data.Sql.Common;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql
{
    public class MeasureSql: BaseSql, Interfaces.IMeasureSql
    {

        #region Properties
        public static Lazy<List<Cont.Measure>> CachedValues
        {
            get
            {
                if (m_CachedValues == null)
                {
                    MeasureSql sql = new MeasureSql();
                    m_CachedValues = new Lazy<List<Cont.Measure>>(()=>new List<Cont.Measure>(sql.GetAll()));
                }
                return m_CachedValues;
            }

            set { m_CachedValues = value; }
        }
        #endregion

        #region Methods
        public int Add(Cont.Measure info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.InputOutput, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);

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
        }

        public void Update(Cont.Measure info, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, info.ID, ParameterDirection.Input, true);
                sql.AddParameter("@Name", SqlDbType.VarChar, info.Name, 50, ParameterDirection.Input, true);

                sql.ExecuteSP(""); //TODO: Pass in the Stored Procedure Name

                // Update cached values
                Cont.Measure cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == info.ID && !x.Equals(info));
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
        }

        public void Delete(int ID, SqlTransaction transaction = null)
        {
            SqlService sql = null;
            try
            {
                sql = new SqlService(transaction);
                sql.AddParameter("@ID", SqlDbType.Int, ID, ParameterDirection.Input, true);
                sql.ExecuteSP(""); //TODO: Pass in the Stored Procedure Name

                // Update cached values
                Cont.Measure cacheItemToRemove = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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
        }

        public Cont.Measure GetByID(int ID)
        {
            Cont.Measure toReturn = CachedValues.Value.FirstOrDefault(x => x.ID == ID);
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
            }
            return toReturn;
        }

        public Cont.Measure[] GetAll()
        {
            Cont.Measure[] toReturn = new Cont.Measure[0];
            SqlService sql = null;
            SqlDataReader reader = null;
            try
            {
                sql = new SqlService();
                reader = sql.ExecuteSPReader(""); //TODO: Pass in the Stored Procedure Name
                toReturn = ConvertToContainer(reader); //MARK: Make sure row IDs corresponds to Database fields
            }//end try
            catch(Exception exc)
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
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Converts Reader to array of Data Containers
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Cont.Measure[] ConvertToContainer(SqlDataReader reader)
        {
            var toReturn = (from row in reader.Cast<System.Data.Common.DbDataRecord>()
                            select new Cont.Measure()
                            {
                                ID = (int)Convert.ChangeType(!Convert.IsDBNull(row["ID"]) ? row["ID"] : 0, typeof(int)),
                                Name = (string)Convert.ChangeType(!Convert.IsDBNull(row["Name"]) ? row["Name"] : null, typeof(string))
                            }).ToArray();

            return toReturn;
        }//end method
        #endregion

        #region Members
        static Lazy<List<Cont.Measure>> m_CachedValues = null;
        #endregion
    }
}
