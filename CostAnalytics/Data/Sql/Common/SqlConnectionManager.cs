
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Global = Dell.GOAnalytics.Global;

namespace Dell.CostAnalytics.Data.Sql.Common
{
    /// <summary>
    /// SQL Connection Manager
    /// </summary>
    public static class SqlConnectionManager
    {
        #region Methods
        /// <summary>
        /// Gets transaction for the specified data store
        /// </summary>
        /// <param name="dataStore"></param>
        public static SqlTransaction GetTransaction(string transactionName = null)
        {
            SqlTransaction toReturn = null;

            SqlConnectionInfo conn = SqlConnectionManager.GetSQLConnection();  //Get connection
            //Return transaction
            try
            {
                if (conn.Connection.State == ConnectionState.Closed)
                {
                    conn.Connection.Open();
                }

                toReturn = ((transactionName ?? string.Empty) != string.Empty) ? conn.Connection.BeginTransaction(transactionName) : conn.Connection.BeginTransaction();

            }
            catch (Exception ex)
            {
                if (conn.Connection.State == ConnectionState.Open)
                {
                    conn.Connection.Close();
                }
                throw ex;
            }
            return toReturn;
        }//end method

        /// <summary>
        /// Gets the default SQL connection for this application
        /// </summary>
        /// <param name="dataStore"></param>
        /// <param name="executionEnvironment"></param>
        /// <returns></returns>
        internal static SqlConnectionInfo GetSQLConnection()
        {
            return _RetrieveConnection();
        }//end method

        /// <summary>
        /// Gets the SQL connection for the requested data store and environment
        /// </summary>
        /// <param name="dataStore"></param>
        /// <param name="executionEnvironment"></param>
        /// <param name="commandTimeOut">Command time out specified by caller</param>
        /// <returns></returns>
        internal static SqlConnectionInfo GetSQLConnection(int commandTimeOut)
        {
            return _RetrieveConnection(commandTimeOut);
        }//end method

        /// <summary>
        /// Internal method to retrieve connection
        /// </summary>
        /// <param name="dataStore"></param>
        /// <param name="executionEnvironment"></param>
        /// <param name="commandTimeOut">Command time out specified by caller</param>
        /// <returns></returns>
        private static SqlConnectionInfo _RetrieveConnection(int commandTimeOut = Int32.MinValue)
        {
            SqlConnectionInfo toReturn = null;

            string connectionString = String.Format("data source={0};initial catalog={1};integrated security=True;", Global.AppSettings.DBServer, Global.AppSettings.DBName);

            /*string connectionString = string.Format("data source={0};initial catalog={1};user id={2};password={3};persist security info={4};packet size={5};",
                Global.AppSettings.DBServer,
                Global.AppSettings.DBName,
                Global.AppSettings.DBAccountName,
                Global.AppSettings.DBPassword,
                Global.AppSettings.DBPersistentSecurity,
                Global.AppSettings.DBPacketSize);*/

            toReturn = new SqlConnectionInfo(new SqlConnection(connectionString), Global.AppSettings.DBNumberOfRetries, Global.AppSettings.DBDefaultTimeout);

            return toReturn;
        }//end method
        #endregion
    }//end class

}//end namespace
