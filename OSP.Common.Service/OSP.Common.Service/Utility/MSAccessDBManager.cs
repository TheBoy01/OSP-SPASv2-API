using System.Data.Common;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace OSP.Common.Service.Utility
{
    public class MSAccessDBManager
    {

        #region Private Member Variables

        private bool disposedValue = false;
        private string dataProvider;
        private string connectionString;

        private string SQL;
        private DbDataReader dr;
        private DbDataAdapter da;
        private DbCommand cmd;
        private DbConnection conn;
        private DbProviderFactory factory;

        #endregion

        #region Private Methods

        private void OpenConnection()
        {
            try
            {
                conn.Open();
            }
            catch (InvalidOperationException InvalidOperationExceptionErr)
            {
                throw new Exception(InvalidOperationExceptionErr.Message, InvalidOperationExceptionErr.InnerException);
            }
        }

        private void CloseConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

            }
            catch (InvalidOperationException InvalidOperationExceptionErr)
            {
                throw new Exception(InvalidOperationExceptionErr.Message, InvalidOperationExceptionErr.InnerException);
            }

        }

        private void InitializeCommand()
        {
            if (cmd == null)
            {
                try
                {
                    cmd = factory.CreateCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = SQL;
                    cmd.CommandTimeout = 1000;
                }
                catch (DbException sqlExceptionErr)
                {
                    throw new Exception(sqlExceptionErr.Message, sqlExceptionErr.InnerException);
                }

            }
        }

        private void InitializeDataAdapter()
        {
            try
            {
                da = factory.CreateDataAdapter();
                da.SelectCommand = cmd;
            }
            catch (DbException sqlExceptionErr)
            {
                throw new Exception(sqlExceptionErr.Message, sqlExceptionErr.InnerException);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing == true)
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr = null;
                    }

                    if (da != null)
                    {
                        da.Dispose();
                        da = null;
                    }

                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (conn != null)
                    {
                        conn.Close();
                        conn = null;
                    }

                    if (factory != null)
                    {
                        factory = null;
                    }
                }
            }
        }

        #endregion

        #region Private Functions

        private string Encrypt(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hash.Length - 1; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        #endregion

        #region Constructors

        //OleDb Connection
        public MSAccessDBManager(string DataSource, string Password)
        {
            dataProvider = "System.Data.OleDb";
            factory = DbProviderFactories.GetFactory(dataProvider);
            connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @DataSource + ";Persist Security Info=True;Mode=Share Deny Read|Share Deny Write;Jet OLEDB:Database Password=" + Password;
            conn = factory.CreateConnection();
            conn.ConnectionString = connectionString;
        }

        public MSAccessDBManager(string DataSource, string UserID, string Password)
        {
            dataProvider = "System.Data.Odbc";
            factory = DbProviderFactories.GetFactory(dataProvider);
            connectionString = @"DSN=" + DataSource + ";Uid=" + UserID + ";pwd=" + Password + ";Connection Timeout=120000";
            conn = factory.CreateConnection();
            conn.ConnectionString = connectionString;
        }

        #endregion

        #region Enums

        public enum DatabaseProvider
        {
            SqlClient = 1,
            MySqlClient = 2,
            ODBC = 3,
            OleDb = 4
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Functions

        public DataTable FetchData(string SQLQuery)
        {
            DataTable dt = new DataTable();

            try
            {
                SQL = SQLQuery;
                OpenConnection();
                InitializeCommand();
                InitializeDataAdapter();
                da.Fill(dt);
                return dt;
            }
            catch (DbException sqlExceptionErr)
            {
                throw new Exception(sqlExceptionErr.Message, sqlExceptionErr.InnerException);
            }
            finally
            {
                CloseConnection();

                cmd.Dispose();
                cmd = null;

                da.Dispose();
                da = null;

                dt.Dispose();
                dt = null;
            }

        }

        public string FetchScalar(string SQLQuery)
        {
            try
            {
                SQL = SQLQuery;
                OpenConnection();
                InitializeCommand();
                dr = cmd.ExecuteReader();
                if (dr.Read() == true)
                {
                    return dr[0].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (DbException SqlExceptionErr)
            {
                throw new Exception(SqlExceptionErr.Message, SqlExceptionErr.InnerException);
            }
            finally
            {
                dr = null;
                CloseConnection();
                cmd.Dispose();
                cmd = null;
            }
        }

        public bool RecordExist(string SQLQuery)
        {
            try
            {
                SQL = SQLQuery;
                OpenConnection();
                InitializeCommand();
                dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbException SqlExceptionErr)
            {
                throw new Exception(SqlExceptionErr.Message, SqlExceptionErr.InnerException);
            }
            finally
            {
                CloseConnection();

                cmd.Dispose();
                cmd = null;

                dr.Close();
                dr = null;

            }
        }

        #endregion

        #region Public Procedures

        public void AddParameter(string Name, object Value)
        {
            try
            {
                cmd.Parameters[Name].Value = Value;
            }
            catch (DbException sqlExceptionErr)
            {
                throw new Exception(sqlExceptionErr.Message, sqlExceptionErr.InnerException);
            }
        }

        public void ExecuteQuery(string SQLQuery)
        {
            try
            {
                SQL = SQLQuery;
                OpenConnection();
                InitializeCommand();
                //cmd.Prepare();
                //cmd.CommandTimeout = 5000;
                cmd.ExecuteNonQuery();
            }
            catch (DbException SqlExceptionErr)
            {
                throw new Exception(SqlExceptionErr.Message, SqlExceptionErr.InnerException);
            }
            finally
            {
                CloseConnection();

                cmd.Dispose();
                cmd = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public DbConnection DBConnection
        {
            get
            {
                conn.ConnectionString = connectionString;
                return conn;
            }
            set { conn = value; }
        }

    }
}
