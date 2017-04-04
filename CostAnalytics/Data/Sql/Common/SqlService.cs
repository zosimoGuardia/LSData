using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Sql.Common
{

    /// <summary>
    /// SQL Service class
    /// </summary>
    internal class SqlService
    {

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlService(SqlTransaction transaction = null)
        {
            m_Transaction = transaction;  //Set transaction value
        }//end constructor

        /// <summary>
        /// Constructor accepting specific connection info
        /// </summary>
        /// <param name="connectionInfo"></param>
        public SqlService(SqlConnectionInfo connectionInfo, SqlTransaction transaction = null)
        {
            this.m_ConnectionInfo = connectionInfo;
            m_Transaction = transaction;  //Set transaction value
        }//end constructor
        #endregion

        #region Properties
        /// <summary>
        /// Transaction property.  Read-only.  Set via constructor
        /// </summary>
        public System.Data.SqlClient.SqlTransaction Transaction
        {
            get { return m_Transaction; }
        }//end property

        /// <summary>
        /// Denotes if this is a single row cursor
        /// </summary>
        public bool IsSingleRow
        {
            get
            {
                return m_isSingleRow;
            }//end get
            set
            {
                m_isSingleRow = value;
            }//end set
        }//end property

        /// <summary>
        /// Denotes whether or not the connection should auto-closed
        /// </summary>
        public bool AutoCloseConnection
        {
            get
            {
                return m_autoCloseConnection;
            }//end get
            set
            {
                m_autoCloseConnection = value;
            }//end set
        }//end property

        /// <summary>
        /// Denotes whether or not the client should auto-enlist in a distributed transaction
        /// </summary>
        public bool AutoEnlist
        {
            get
            {
                return m_autoEnlist;
            }//end get
            set
            {
                m_autoEnlist = value;
            }//end set
        }//end property

        /// <summary>
        /// Denotes whether or not empty values should be converted to DB Null
        /// </summary>
        public bool ConvertEmptyValuesToDbNull
        {
            get
            {
                return m_convertEmptyValuesToDbNull;
            }//end get
            set
            {
                m_convertEmptyValuesToDbNull = value;
            }//end set
        }//end property

        /// <summary>
        /// Denotes whether or not minimum values for properties should be converted to DB Null
        /// </summary>
        public bool ConvertMinValuesToDbNull
        {
            get
            {
                return m_convertMinValuesToDbNull;
            }//end get
            set
            {
                m_convertMinValuesToDbNull = value;
            }//end set
        }//end property

        /// <summary>
        /// Denotes whether or not maximum values for properties should be converted to DB Null
        /// </summary>
        public bool ConvertMaxValuesToDbNull
        {
            get
            {
                return m_convertMaxValuesToDbNull;
            }//end get
            set
            {
                m_convertMaxValuesToDbNull = value;
            }//end set
        }//end property

        public ArrayList InputParameters
        {
            get
            {
                if (m_InputParameters == null)
                {
                    m_InputParameters = new ArrayList();
                }//end if
                return m_InputParameters;
            }//end get
        }//end property

        /// <summary>
        /// List of Sql Parameters
        /// </summary>
        public SqlParameterCollection ResultParameters
        {
            get
            {
                return m_ResultParameters;
            }//end get
        }//end property

        /// <summary>
        /// Return value for last call
        /// </summary>
        public int ReturnValue
        {
            get
            {
                if (m_ResultParameters.Contains("@ReturnValue"))
                {
                    return (int)m_ResultParameters["@ReturnValue"].Value;
                }//end if
                else
                {
                    throw new Exception("You must call the AddReturnValueParameter method before executing your request.");
                }//end else
            }//end get
        }//end property

        /// <summary>
        /// SQL connection info object
        /// </summary>
        private SqlConnectionInfo ConnectionInfo
        {
            get
            {
                if (m_ConnectionInfo == null)
                {
                    //If the connection is not initialized then get default connection.
                    m_ConnectionInfo = Sql.Common.SqlConnectionManager.GetSQLConnection();  //Get default connection
                }//end if
                return m_ConnectionInfo;
            }//end get
        }//end property


        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates and ad-hoc SQL command
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private SqlCommand CreateSQLAdHocCommand(string commandText)
        {
            SqlCommand toReturn = new SqlCommand()
            {
                CommandTimeout = this.ConnectionInfo.CommandTimeout,
                CommandText = commandText,
                Connection = (this.Transaction != null ? this.Transaction.Connection : this.ConnectionInfo.Connection),
                CommandType = CommandType.Text
            };
            //If we have a transaction then bind it to command
            if (this.Transaction != null)
            {
                toReturn.Transaction = this.Transaction;
            }//end if
            return toReturn;
        }//end method

        /// <summary>
        /// Creates and ad-hoc SQL command
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="cntRetries"></param>
        /// <returns></returns>
        private SqlCommand CreateSQLStoredProcedureCommand(string procedureName, int cntRetries)
        {
            SqlCommand toReturn = new SqlCommand()
            {
                CommandTimeout = this.ConnectionInfo.CommandTimeout,
                CommandText = procedureName,
                Connection = (this.Transaction != null ? this.Transaction.Connection : this.ConnectionInfo.Connection),
                CommandType = CommandType.StoredProcedure
            };
            //If this is the first try then populate parameters
            if (cntRetries == 0)
            {
                //Only copy parameters on first retry
                this.CopyParameters(toReturn);
            }//end if
            //If we have a transaction then bind it to command
            if (this.Transaction != null)
            {
                toReturn.Transaction = this.Transaction;
            }//end if
            return toReturn;
        }//end method

        /// <summary>
        /// Processes Sql Exception and decides whether or not to continue workflow
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cntRetries"></param>
        /// <param name="ex"></param>
        private void ProcessSqlException(SqlCommand cmd, int cntRetries, Exception ex)
        {
            if (this.ConnectionInfo.NumberOfRetries <= cntRetries)
            {
                throw new Exception(String.Format("Exception executing DB Operation = \"{0}\",\nParameters:\n{1}", (cmd != null && cmd.CommandText != null) ? cmd.CommandText : "NULL", (cmd != null) ? GetParamsAsString() : string.Empty), ex);
            }//end if
            int sleeper = new Random().Next(1000, 5000); //have thread sleep for at least one second before retry, no more than five
            System.Threading.Thread.Sleep(sleeper);
        }//end method

        /// <summary>
        /// Executes ad-hoc SQL statement
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        public void ExecuteSql(string sql, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    cmd.ExecuteNonQuery();
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection)
                    {
                        this.Disconnect();
                    }//end if
                }//end finally
            }//end while
        }//end method

        /// <summary>
        /// Executes ad-hoc SQL statement and returns data reader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteSqlReader(string sql, int commandTimeout = -1)
        {
            SqlDataReader toReturn = null;
            SqlCommand cmd = null;
            CommandBehavior behavior = CommandBehavior.Default;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);
                    cmd.ExecuteNonQuery();

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    flgSuccess = true;

                    if (this.AutoCloseConnection)
                    {
                        behavior = behavior | CommandBehavior.CloseConnection;
                    }//end if

                    if (this.IsSingleRow)
                    {
                        behavior = behavior | CommandBehavior.SingleRow;
                    }//end if
                    toReturn = cmd.ExecuteReader(behavior);
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executes SQL statement and returns XML reader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public XmlReader ExecuteSqlXmlReader(string sql, int commandTimeout = -1)
        {
            XmlReader toReturn = null;
            SqlCommand cmd = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    cmd.ExecuteNonQuery();
                    flgSuccess = true;
                    toReturn = cmd.ExecuteXmlReader();
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executes ad-hoc SQL and returns Data Set
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSqlDataSet(string sql, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            DataSet toReturn = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(toReturn);
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection)
                    {
                        this.Disconnect();
                    }//end if
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executed ad-hoc SQL and return a dataset based on the provided table schema
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSqlDataSet(string sql, string tableName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet toReturn = new DataSet();
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(toReturn, tableName);
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executes ad-hoc SQL and based on the provided table
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <param name="commandTimeout"></param>
        public void ExecuteSqlDataSet(ref DataSet dataSet, string sql, string tableName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLAdHocCommand(sql);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(dataSet, tableName);
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
        }//end method

        /// <summary>
        /// Executes stored procedure and returns dataset
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSPDataSet(string procedureName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet toReturn = new DataSet();
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(toReturn);
                    this.m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executes stored procedure for and returns dataset based on specified table
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSPDataSet(string procedureName, string tableName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet toReturn = new DataSet();
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(toReturn);
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();

                    if (cmd != null)
                        cmd.Dispose();

                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
            return toReturn;
        }//end method

        /// <summary>
        /// Executes stored procedure and returns in provided dataset based on provided table
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="procedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="commandTimeout"></param>
        public void ExecuteSPDataSet(ref DataSet dataSet, string procedureName, string tableName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    da = new SqlDataAdapter() { SelectCommand = cmd };
                    da.Fill(dataSet, tableName);
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (da != null)
                        da.Dispose();
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
        }//end method

        /// <summary>
        /// Executes provided stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="commandTimeout"></param>
        public void ExecuteSP(string procedureName, int commandTimeout = -1)
        {
            SqlCommand cmd = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    cmd.ExecuteNonQuery();
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                    if (this.AutoCloseConnection) this.Disconnect();
                }//end finally
            }//end while
        }//end method

        /// <summary>
        /// Executes stored procedure and returns data reader
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteSPReader(string procedureName, int commandTimeout = -1)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            CommandBehavior behavior = CommandBehavior.Default;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);
                    if (this.AutoCloseConnection && this.Transaction == null)
                    {
                        behavior = behavior | CommandBehavior.CloseConnection;
                    }//end if

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    if (this.IsSingleRow)
                    {
                        behavior = behavior | CommandBehavior.SingleRow;
                    }//end if
                    reader = cmd.ExecuteReader(behavior);
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                }//end finally
            }//end while
            return reader;
        }//end method

        /// <summary>
        /// Executes stored procedure and returns data reader
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> ExecuteSPReaderAsync(string procedureName, int commandTimeout = -1)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            CommandBehavior behavior = CommandBehavior.Default;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);
                    if (this.AutoCloseConnection && this.Transaction == null)
                    {
                        behavior = behavior | CommandBehavior.CloseConnection;
                    }//end if

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    if (this.IsSingleRow)
                    {
                        behavior = behavior | CommandBehavior.SingleRow;
                    }//end if
                    reader = await cmd.ExecuteReaderAsync(behavior);
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                }//end finally
            }//end while
            return reader;
        }//end method

        /// <summary>
        /// Executes stored procedure and returns XML reader
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public XmlReader ExecuteSPXmlReader(string procedureName, int commandTimeout = -1)
        {
            XmlReader reader = null;
            SqlCommand cmd = null;
            bool flgSuccess = false;
            int cntRetries = 0;  //Number of retries
            while (!flgSuccess && cntRetries <= this.ConnectionInfo.NumberOfRetries)
            {
                try
                {
                    this.Connect();
                    cmd = CreateSQLStoredProcedureCommand(procedureName, cntRetries);

                    //Set timeout if provided
                    if (commandTimeout != -1)
                    {
                        cmd.CommandTimeout = commandTimeout;
                    }//end if

                    reader = cmd.ExecuteXmlReader();
                    m_ResultParameters = cmd.Parameters;
                    flgSuccess = true;
                }//end try
                catch (Exception exc)
                {
                    cntRetries++;
                    ProcessSqlException(cmd, cntRetries, exc);
                }//end catch
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                }//end finally
            }//end while
            return reader;
        }//end method
        #endregion Execute Methods

        #region AddParameter
        public SqlParameter AddParameter(string name, SqlDbType type, object value)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value);

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, ParameterDirection direction, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size, ParameterDirection direction, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            this.InputParameters.Add(prm);

            return prm;
        }

        public void AddParameter(SqlParameter parameter)
        {
            this.InputParameters.Add(parameter);
        }
        #endregion AddParameter

        #region Specialized AddParameter Methods
        public SqlParameter AddOutputParameter(string name, SqlDbType type)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.SqlDbType = type;

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddOutputParameter(string name, SqlDbType type, int size)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddReturnValueParameter()
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.ReturnValue;
            prm.ParameterName = "@ReturnValue";
            prm.SqlDbType = SqlDbType.Int;

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddStreamParameter(string name, Stream value)
        {
            return this.AddStreamParameter(name, value, SqlDbType.Image);
        }

        public SqlParameter AddStreamParameter(string name, Stream value, SqlDbType type)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;

            value.Position = 0;
            byte[] data = new byte[value.Length];
            value.Read(data, 0, (int)value.Length);
            prm.Value = data;

            this.InputParameters.Add(prm);

            return prm;
        }

        public SqlParameter AddTextParameter(string name, string value)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = SqlDbType.Text;
            prm.Value = this.PrepareSqlValue(value);

            this.InputParameters.Add(prm);

            return prm;
        }
        #endregion Specialized AddParameter Methods

        #region Private Methods
        private string GetParamsAsString()
        {
            if (this.InputParameters == null || this.InputParameters.Count == 0) return "no paramters";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < this.InputParameters.Count; ++i)
            {
                SqlParameter param = (SqlParameter)this.InputParameters[i];
                sb.AppendFormat("\tParameter #{3}: Name = \"{0}\", Type = {1}, Value = \"{2}\"\n",
                    param.ParameterName, param.DbType, ((param.Value == null || Convert.IsDBNull(param.Value)) ? "NULL" : param.Value.ToString()), i);
            }
            return sb.ToString();
        }


        private void CopyParameters(SqlCommand command)
        {
            for (int i = 0; i < this.InputParameters.Count; i++)
            {
                command.Parameters.Add(this.InputParameters[i]);
            }
        }

        private object PrepareSqlValue(object value)
        {
            return this.PrepareSqlValue(value, false);
        }

        private object PrepareSqlValue(object value, bool convertZeroToDBNull)
        {
            if (value is String)
            {
                if (convertZeroToDBNull && (string)value == String.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Guid)
            {
                if (convertZeroToDBNull && (Guid)value == Guid.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is DateTime)
            {
                if ((this.ConvertMinValuesToDbNull && (DateTime)value == DateTime.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (DateTime)value == DateTime.MaxValue))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int16)
            {
                if ((this.ConvertMinValuesToDbNull && (Int16)value == Int16.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int16)value == Int16.MaxValue)
                    || (convertZeroToDBNull && (Int16)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int32)
            {
                if ((this.ConvertMinValuesToDbNull && (Int32)value == Int32.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int32)value == Int32.MaxValue)
                    || (convertZeroToDBNull && (Int32)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int64)
            {
                if ((this.ConvertMinValuesToDbNull && (Int64)value == Int64.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int64)value == Int64.MaxValue)
                    || (convertZeroToDBNull && (Int64)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Single)
            {
                if ((this.ConvertMinValuesToDbNull && (Single)value == Single.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Single)value == Single.MaxValue)
                    || (convertZeroToDBNull && (Single)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Double)
            {
                if ((this.ConvertMinValuesToDbNull && (Double)value == Double.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Double)value == Double.MaxValue)
                    || (convertZeroToDBNull && (Double)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Decimal)
            {
                if ((this.ConvertMinValuesToDbNull && (Decimal)value == Decimal.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Decimal)value == Decimal.MaxValue)
                    || (convertZeroToDBNull && (Decimal)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                if (value == null)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
        }
        #endregion Private Methods

        #region Public Methods
        public void Connect()
        {
            if (this.ConnectionInfo.Connection.State != ConnectionState.Open)
            {
                this.ConnectionInfo.Connection.Open();
            }
        }

        public void Disconnect()
        {
            if ((this.ConnectionInfo.Connection != null) && (this.ConnectionInfo.Connection.State != ConnectionState.Closed))
            {
                this.ConnectionInfo.Connection.Close();
            }

            if (this.ConnectionInfo.Connection != null) this.ConnectionInfo.Connection.Dispose();

            //Reset connection
            this.m_ConnectionInfo = null;
        }

        /// <summary>
        /// Resets reader
        /// </summary>
        public void Reset()
        {
            this.InputParameters.Clear();
            m_ResultParameters = null;
        }//end method
        #endregion Methods

        #region Members
        /// <summary>
        /// Member for Transaction property
        /// </summary>
        private SqlTransaction m_Transaction = null;
        /// <summary>
        /// Member for ResultParameters property
        /// </summary>
        private SqlParameterCollection m_ResultParameters;
        /// <summary>
        /// Member for InputParameters property
        /// </summary>
        private ArrayList m_InputParameters = null;
        /// <summary>
        /// Member for IsSingleRow property
        /// </summary>
        private bool m_isSingleRow = false;
        /// <summary>
        /// Member for ConvertEmptyValuesToDbNull property
        /// </summary>
        private bool m_convertEmptyValuesToDbNull = true;
        /// <summary>
        /// Member for ConvertMinValuesToDbNull property
        /// </summary>
        private bool m_convertMinValuesToDbNull = true;
        /// <summary>
        /// Member for ConvertMaxValuesToDbNull property
        /// </summary>
        private bool m_convertMaxValuesToDbNull = false;
        /// <summary>
        /// Member for AutoCloseConnection property
        /// </summary>
        private bool m_autoCloseConnection = true;
        /// <summary>
        /// Member for AutoEnlist property
        /// </summary>
        private bool m_autoEnlist = true;
        /// <summary>
        /// Member for ConnectionInfo property
        /// </summary>
        private SqlConnectionInfo m_ConnectionInfo = null;
        #endregion Members


    }
}
