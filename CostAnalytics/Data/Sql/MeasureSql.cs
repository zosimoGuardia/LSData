using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dell.CostAnalytics.Data.Sql.Common;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql
{
    class MeasureSql: BaseSql, Interfaces.IMeasureSql
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

        public int Add(Cont.Measure info, SqlTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public void Update(Cont.Measure info, SqlTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ID, SqlTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public Cont.Measure GetByID(int ID)
        {
            throw new NotImplementedException();
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
