using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Sql.Common
{
    /// <summary>
    /// Container for all necessary Sql Connection Info
    /// </summary>
    internal class SqlConnectionInfo
    {

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="numberOfRetries"></param>
        public SqlConnectionInfo(SqlConnection connection)
        {
            m_Connection = connection;
            //Set default retry and timeout values
            this.m_NumberOfRetries = DEFAULT_NUMBER_OF_RETRIES;
            this.m_CommandTimeout = DEFAULT_COMMAND_TIMEOUT;
        }//end method


        /// <summary>
        /// Constructor to force connection and retry info
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="numberOfRetries"></param>
        public SqlConnectionInfo(SqlConnection connection, int numberOfRetries, int commandTimeout)
        {
            m_Connection = connection;
            if (numberOfRetries >= 0)
            {
                m_NumberOfRetries = numberOfRetries;
            }//end if
            else
            {
                throw new Exception(String.Format("Cannot initialize SQL collection with invalid retry count value of {0}", numberOfRetries));
            }//end else
            if (commandTimeout >= 0)
            {
                m_CommandTimeout = commandTimeout;
            }//end if
            else
            {
                throw new Exception(String.Format("Cannot initialize SQL collection with invalid retry count value of {0}", commandTimeout));
            }//end else
        }//end method
        #endregion

        #region Properties
        /// <summary>
        /// SQL Connection
        /// </summary>
        internal SqlConnection Connection
        {
            get { return m_Connection; }
        }//end property

        /// <summary>
        /// Number of Retries
        /// </summary>
        internal int NumberOfRetries
        {
            get { return m_NumberOfRetries; }
        }//end property

        /// <summary>
        /// Command Timeout
        /// </summary>
        internal int CommandTimeout
        {
            get { return m_CommandTimeout; }
        }//end property
        #endregion 

        #region Constants
        /// <summary>
        /// Default Number of Retries
        /// </summary>
        private const int DEFAULT_NUMBER_OF_RETRIES = 0;
        /// <summary>
        /// Default command timeout
        /// </summary>
        private const int DEFAULT_COMMAND_TIMEOUT = 30;
        #endregion

        #region Members
        /// <summary>
        /// SQL Connection
        /// </summary>
        private SqlConnection m_Connection = null;

        /// <summary>
        /// Number of Retrieve
        /// </summary>
        private int m_NumberOfRetries = -1;

        /// <summary>
        /// Command Timeout
        /// </summary>
        private int m_CommandTimeout;
        #endregion

    }//end class
}//end namespace
