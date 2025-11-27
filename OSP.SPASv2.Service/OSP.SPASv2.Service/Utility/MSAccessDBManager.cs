using System.Data.Common;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Data.OleDb;
using OSP.SPASv2.Domain.View;
using System.IO;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;

namespace OSP.SPASv2.Service.Utility
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
        private OleDbCommand OledDb_Cmd;
        private OleDbConnection OledDb_conn;

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

        public MSAccessDBManager(string DataSource, string Password, ServiceParams ServiceParams)
        {
            StringBuilder sb = new StringBuilder();
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + @DataSource + ";Persist Security Info=True;Mode=Share Deny Read|Share Deny Write;Jet OLEDB:Database Password='197ospLpi@2024$pg5E'" + Password;
            OledDb_conn = new OleDbConnection(connectionString);
            OledDb_conn.Open();
             

            foreach (var item in ServiceParams.qryCMSPOHdrList)
            {
                sb = new StringBuilder();
                sb.Append("Insert into TblPurchaseOrderHdr values (");
                sb.Append("'" + item.PONo + "', ");
                sb.Append("'" + item.FactoryCode + "', ");
                sb.Append("'" + item.CompanyCode + "', ");
                sb.Append("'" + item.ChapelCode + "', ");
                sb.Append("'" + item.PODate + "', ");
                sb.Append("'" + item.POReceivedDate + "', ");
                sb.Append("'" + item.Terms + "', ");
                sb.Append("'" + item.Remarks + "', ");
                sb.Append("'" + item.POAmount + "', ");
                sb.Append("'" + item.AuditUser + "', ");
                sb.Append("'" + item.AuditDate + "', ");
                sb.Append("'" + item.EditUser + "', ");
                sb.Append("'" + item.EditDate + "', ");
                sb.Append("0,");//sb.Append("'" + item.Void + "', ");
                sb.Append("'" + item.VoidUser + "', ");
                sb.Append("'" + item.VoidDate + "') ");

                OledDb_Cmd = new OleDbCommand(sb.ToString(), OledDb_conn);
                OledDb_Cmd.ExecuteNonQuery();
            }

            foreach (var item in ServiceParams.qryCMSRefChapelList)
            {
                sb = new StringBuilder();
                sb.Append("Insert into RefChapel values (");
                sb.Append("'" + item.ChapelCode + "', ");
                sb.Append("'" + item.ChapelDesc + "', ");
                sb.Append("'" + item.CompanyCode + "', ");
                sb.Append("'" + item.Address + "', ");
                sb.Append("'" + item.ChapelMngr + "', ");
                sb.Append("'" + item.ContactNo + "', ");
                sb.Append("'" + item.Email + "', ");
                sb.Append("'" + item.ChapelType + "', ");
                sb.Append("1, ");
                sb.Append("'" + item.Class + "', ");
                sb.Append("'" + item.RegionCode + "', ");
                sb.Append("'" + item.TerritoryCode + "', "); 
                sb.Append("'" + item.AuditUser + "', ");
                sb.Append("'" + item.AuditDate + "', ");
                sb.Append("0, ");
                sb.Append("'" + item.EditUser + "', ");
                sb.Append("'" + item.EditDate + "') ");

                OledDb_Cmd = new OleDbCommand(sb.ToString(), OledDb_conn);
                OledDb_Cmd.ExecuteNonQuery();
            }

            //if (qryCMSPOHdrList.Count > 0)
            //{

            //} 


            foreach (var item in ServiceParams.qryCMSPODtlList)
            {
                sb = new StringBuilder();
                sb.Append("Insert into TblPurchaseOrderDtl values ( ");
                sb.Append("'" + item.FactoryCode + "', ");
                sb.Append("'" + item.PONo + "', ");
                sb.Append("'" + item.CasketCode + "', ");
                sb.Append("'" + item.OrderQty + "', ");
                sb.Append("'" + item.POAmount + "', ");
                sb.Append("'" + item.AuditUser + "', ");
                sb.Append("'" + item.AuditDate + "', ");
                sb.Append("'" + item.EditUser + "', ");
                sb.Append("'" + item.EditDate + "') ");

                OledDb_Cmd = new OleDbCommand(sb.ToString(), OledDb_conn);
                OledDb_Cmd.ExecuteNonQuery();

            }
            //if (qryCMSPODtlList.Count > 0)
            //{

            //}

            sb = new StringBuilder();
            sb.Append("Insert into BKPName values ( ");
            sb.Append("'" + ServiceParams.qryBKPName.BKPName + "', ");
            sb.Append("'" + ServiceParams.qryBKPName.FactoryCode + "', ");
            sb.Append("'" + ServiceParams.qryBKPName.SystemCode + "', ");
            sb.Append("'" + ServiceParams.qryBKPName.BKPType + "', ");
            sb.Append("'" + ServiceParams.qryBKPName.StartDate + "', ");
            sb.Append("'" + ServiceParams.qryBKPName.EndDate + "' ) ");

            OledDb_Cmd = new OleDbCommand(sb.ToString(), OledDb_conn);
            OledDb_Cmd.ExecuteNonQuery();

            string strpw = $"ALTER DATABASE PASSWORD [" + Utilities.MdbPw + "] NULL;";

            OledDb_Cmd = new OleDbCommand(strpw, OledDb_conn);
            OledDb_Cmd.ExecuteNonQuery();

            //using (OleDbCommand cmd = new OleDbCommand(sb.ToString(), OledDb_conn))
            //{
            //    int rowsAffected = cmd.ExecuteNonQuery(); // Execute the query 
            //}

            //using (OleDbConnection conn = new OleDbConnection(connectionString))
            //{
            //    conn.Open(); // Open the connection

            //    sb = new StringBuilder();
            //    foreach (var item in qryCMSPOHdrList)
            //    {
            //        sb = new StringBuilder();
            //        sb.Append("Select ");
            //        sb.Append("'" + item.PONo + "' as [LPANo], ");
            //        sb.Append("'" + item.FactoryCode + "' as [FactoryCode], ");
            //        sb.Append("'" + item.ChapelCode + "' as [ChapelCode], ");
            //        sb.Append("'" + item.PODate + "' as [PODate], ");
            //        sb.Append("'" + item.POReceivedDate + "' as [POReceivedDate], ");
            //        sb.Append("'" + item.Terms + "' as [Terms], ");
            //        sb.Append("'" + item.Remarks + "' as [Remarks], ");
            //        sb.Append("'" + item.POAmount + "' as [POAmount], ");
            //        sb.Append("'" + item.AuditUser + "' as [AuditUser], ");
            //        sb.Append("'" + item.AuditDate + "' as [AuditDate], ");
            //        sb.Append("'" + item.EditUser + "' as [EditUser], ");
            //        sb.Append("'" + item.EditDate + "' as [EditDate], ");
            //        sb.Append("'" + item.Void + "' as [Void], ");
            //        sb.Append("'" + item.VoidUser + "' as [VoidUser], ");
            //        sb.Append("'" + item.VoidDate + "' as [VoidDate]  Into qryCMSPOHdrList"); 
            //    }

            //    using (OleDbCommand cmd = new OleDbCommand(sb.ToString(), conn))
            //    { 
            //        int rowsAffected = cmd.ExecuteNonQuery(); // Execute the query 
            //    }

            //    using (OleDbCommand cmd = new OleDbCommand("ALTER DATABASE PASSWORD " + Utilities.MdbPw + " NULL ;" conn))
            //    { 
            //       cmd.ExecuteNonQuery(); // Execute the query 
            //    }
            OledDb_Cmd.Dispose();
            OledDb_conn.Dispose();
            OledDb_conn = null;
            //}

        }

        //public MSAccessDBManager(string DataSource, string Password)
        //{
        //    dataProvider = "System.Data.OleDb";
        //    factory = DbProviderFactories.GetFactory(dataProvider);
        //    connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @DataSource + ";Persist Security Info=True;Mode=Share Deny Read|Share Deny Write;Jet OLEDB:Database Password=" + Password;
        //    conn = factory.CreateConnection();
        //    conn.ConnectionString = connectionString;

        //}

        //public MSAccessDBManager(string DataSource, string UserID, string Password)
        //{
        //    dataProvider = "System.Data.Odbc";
        //    factory = DbProviderFactories.GetFactory(dataProvider);
        //    connectionString = @"DSN=" + DataSource + ";Uid=" + UserID + ";pwd=" + Password + ";Connection Timeout=120000";
        //    conn = factory.CreateConnection();
        //    conn.ConnectionString = connectionString;
        //}

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
