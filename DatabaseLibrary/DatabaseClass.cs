using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ErrorMessageLibrary;

namespace DatabaseLibrary
{
    public class DatabaseClass
    {
        public ErrorMessageClass ErrMsg;
        public string ConnStr = "";
        public string mSQLDatabase = "";
        public bool mErrFlag = false;
        private string mWorkPath;               //資料路徑
        public string WorkPath
        {
            get { return mWorkPath; }
            set
            {
                if (WorkPath == "") value = Directory.GetCurrentDirectory();
                mWorkPath = value;
            }
        }
        /// <summary>
        /// Check log database exist.
        /// </summary>
        /// <param name="_time">System time</param>
        /// <returns>Database is exist(true) or not(false)</returns>
        public bool CheckDatabaseExistFunction(DateTime _time)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDatabaseExist = "SELECT * FROM sys.databases WHERE name = '" + mSQLDatabase + "_" + _time.ToString("yyyy") + "'";
            try
            {
                SqlCommand CmdCheckDatabaseExist = new SqlCommand(SelectCheckDatabaseExist, ConnTxt);
                SqlDataReader CheckDatabaseExist = CmdCheckDatabaseExist.ExecuteReader();
                if (!CheckDatabaseExist.HasRows)
                    RecordFlog = false;         //資料庫不存在
                else
                    RecordFlog = true;          //資料庫存在
                CheckDatabaseExist.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDatabaseExist, ex);
                else
                    ErrMsg._errorText(SelectCheckDatabaseExist, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool CheckDatabaseExistFunction(string _databasename)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDatabaseExist = "SELECT * FROM sys.databases WHERE name = '" + _databasename + "'";
            try
            {
                SqlCommand CmdCheckDatabaseExist = new SqlCommand(SelectCheckDatabaseExist, ConnTxt);
                SqlDataReader CheckDatabaseExist = CmdCheckDatabaseExist.ExecuteReader();
                if (!CheckDatabaseExist.HasRows)
                    RecordFlog = false;         //資料庫不存在
                else
                    RecordFlog = true;          //資料庫存在
                CheckDatabaseExist.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDatabaseExist, ex);
                else
                    ErrMsg._errorText(SelectCheckDatabaseExist, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// Create log database.
        /// </summary>
        /// <param name="_time">System time</param>
        /// <returns>Database create success(true) or not(false)</returns>
        public bool CreateDatabaseFunction(DateTime _time)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDatabase = "CREATE DATABASE [" + mSQLDatabase + "_" + _time.ToString("yyyy") + "]";
            try
            {
                SqlCommand CmdDatabase = new SqlCommand(CreateDatabase, ConnTxt);
                SqlDataReader Database = CmdDatabase.ExecuteReader();
                Database.Close();
                RecordFlog = true;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(CreateDatabase, ex);
                else
                    ErrMsg._errorText(CreateDatabase, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool CreateDatabaseFunction(string _databasename)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDatabase = "CREATE DATABASE [" + _databasename + "]";
            try
            {
                SqlCommand CmdDatabase = new SqlCommand(CreateDatabase, ConnTxt);
                SqlDataReader Database = CmdDatabase.ExecuteReader();
                Database.Close();
                RecordFlog = true;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(CreateDatabase, ex);
                else
                    ErrMsg._errorText(CreateDatabase, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// Check data table exist.
        /// </summary>
        /// <param name="_datatablename">Data table name</param>
        /// <returns>Return data table exist(true) or not(false)</returns>
        public bool CheckDataTableExistFunction(string _datatablename)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataTableExist = "SELECT COUNT(*) AS tcount FROM sysobjects WHERE name = '" + _datatablename + "'";
            try
            {
                SqlCommand CmdCheckDataTableExist = new SqlCommand(SelectCheckDataTableExist, ConnTxt);
                SqlDataReader CheckDataTableExist = CmdCheckDataTableExist.ExecuteReader();
                if (CheckDataTableExist.HasRows)
                    if (CheckDataTableExist.Read())
                    {
                        int mTableCount = Convert.ToInt32(CheckDataTableExist["tcount"]);
                        if (mTableCount == 1)
                            RecordFlog = true;          //資料表存在
                        else
                            RecordFlog = false;         //資料表不存在
                    }
                    else
                    {
                        RecordFlog = false;             //資料表不存在
                    }
                else
                    RecordFlog = false;                 //資料表不存在
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDataTableExist, ex);
                else
                    ErrMsg._errorText(SelectCheckDataTableExist, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// Check data table exist.
        /// </summary>
        /// <param name="_datatablename">Data table name</param>
        /// <param name="_connstr">Database connect command</param>
        /// <returns>Return data table exist(true) or not(false)</returns>
        public bool CheckDataTableExistFunction(string _datatablename, string _connstr)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataTableExist = "SELECT COUNT(*) AS tcount FROM sysobjects WHERE name = '" + _datatablename + "'";
            try
            {
                SqlCommand CmdCheckDataTableExist = new SqlCommand(SelectCheckDataTableExist, ConnTxt);
                SqlDataReader CheckDataTableExist = CmdCheckDataTableExist.ExecuteReader();
                if (CheckDataTableExist.HasRows)
                    if (CheckDataTableExist.Read())
                    {
                        int mTableCount = Convert.ToInt32(CheckDataTableExist["tcount"]);
                        if (mTableCount == 1)
                            RecordFlog = true;          //資料表存在
                        else
                            RecordFlog = false;         //資料表不存在
                    }
                    else
                    {
                        RecordFlog = false;             //資料表不存在
                    }
                else
                    RecordFlog = false;                 //資料表不存在
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDataTableExist, ex);
                else
                    ErrMsg._errorText(SelectCheckDataTableExist, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// Create data table.
        /// </summary>
        /// <param name="_datatablename">Data table name</param>
        /// <param name="_createcommand">Table create command</param>
        /// <returns>Return create success(true) or not(false)</returns>
        public bool CreateDataTableFunction(string _datatablename, string _createcommand)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDataTable = "CREATE TABLE [" + _datatablename + "](" + _createcommand + ")";
            try
            {
                SqlCommand CmdDataTable = new SqlCommand(CreateDataTable, ConnTxt);
                SqlDataReader DataTable = CmdDataTable.ExecuteReader();
                DataTable.Close();
                RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(CreateDataTable, ex);
                else
                    ErrMsg._errorText(CreateDataTable, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// Create data table.
        /// </summary>
        /// <param name="_datatablename">Data table name</param>
        /// <param name="_createcommand">Table create command</param>
        /// <param name="_connstr">Database connect command</param>
        /// <returns>Return create success(true) or not(false)</returns>
        public bool CreateDataTableFunction(string _datatablename, string _createcommand, string _connstr)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDataTable = "CREATE TABLE [" + _datatablename + "](" + _createcommand + ")";
            try
            {
                SqlCommand CmdDataTable = new SqlCommand(CreateDataTable, ConnTxt);
                SqlDataReader DataTable = CmdDataTable.ExecuteReader();
                DataTable.Close();
                RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(CreateDataTable, ex);
                else
                    ErrMsg._errorText(CreateDataTable, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool CheckDataLogExistFunction(string _tablename)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataLog = "SELECT * FROM " + _tablename;
            try
            {
                SqlCommand CmdCheckDataLog = new SqlCommand(SelectCheckDataLog, ConnTxt);
                SqlDataReader CheckDataLog = CmdCheckDataLog.ExecuteReader();
                if (CheckDataLog.HasRows)
                    RecordFlog = true;
                else
                    RecordFlog = false;
                CheckDataLog.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDataLog, ex);
                else
                    ErrMsg._errorText(SelectCheckDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool CheckDataLogExistFunction(string _tablename, string _condition)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataLog = "SELECT * FROM " + _tablename + " WHERE " + _condition;
            try
            {
                SqlCommand CmdCheckDataLog = new SqlCommand(SelectCheckDataLog, ConnTxt);
                SqlDataReader CheckDataLog = CmdCheckDataLog.ExecuteReader();
                if (CheckDataLog.HasRows)
                    RecordFlog = true;
                else
                    RecordFlog = false;
                CheckDataLog.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDataLog, ex);
                else
                    ErrMsg._errorText(SelectCheckDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool CheckDataLogExistFunction(string _tablename, string _condition, string _connstr)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataLog = "SELECT * FROM " + _tablename + " WHERE " + _condition;
            try
            {
                SqlCommand CmdCheckDataLog = new SqlCommand(SelectCheckDataLog, ConnTxt);
                SqlDataReader CheckDataLog = CmdCheckDataLog.ExecuteReader();
                if (CheckDataLog.HasRows)
                    RecordFlog = true;
                else
                    RecordFlog = false;
                CheckDataLog.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectCheckDataLog, ex);
                else
                    ErrMsg._errorText(SelectCheckDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool AddNewDataLogFunction(string _tablename, string _columns, string _data)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string InsertNewDataLog = "INSERT INTO " + _tablename + "(" + _columns + ") VALUES (" + _data + ")";
            try
            {
                SqlCommand CmdNewDataLog = new SqlCommand(InsertNewDataLog, ConnTxt);
                int NewDataLog = CmdNewDataLog.ExecuteNonQuery();
                if (NewDataLog == 1)
                    RecordFlog = true;
                else
                    RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(InsertNewDataLog, ex);
                else
                    ErrMsg._errorText(InsertNewDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool AddNewDataLogFunction(string _tablename, string _columns, string _data, string _connstr)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string InsertNewDataLog = "INSERT INTO " + _tablename + "(" + _columns + ") VALUES (" + _data + ")";
            try
            {
                SqlCommand CmdNewDataLog = new SqlCommand(InsertNewDataLog, ConnTxt);
                int NewDataLog = CmdNewDataLog.ExecuteNonQuery();
                if (NewDataLog == 1)
                    RecordFlog = true;
                else
                    RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(InsertNewDataLog, ex);
                else
                    ErrMsg._errorText(InsertNewDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool EditDataLogFunction(string _tablename, string _command, string _condition)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string UpdateEditDataLog = "UPDATE [" + _tablename + "] SET " + _command + " WHERE " + _condition;
            try
            {
                SqlCommand CmdEditDataLog = new SqlCommand(UpdateEditDataLog, ConnTxt);
                int EditDataLog = CmdEditDataLog.ExecuteNonQuery();
                if (EditDataLog == 1)
                    RecordFlog = true;
                else
                    RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(UpdateEditDataLog, ex);
                else
                    ErrMsg._errorText(UpdateEditDataLog, ex);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }

        public bool EditDataLogFunction(string _tablename, string _command)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string UpdateEditDataLog = "UPDATE [" + _tablename + "] SET " + _command;
            try
            {
                SqlCommand CmdEditDataLog = new SqlCommand(UpdateEditDataLog, ConnTxt);
                int EditDataLog = CmdEditDataLog.ExecuteNonQuery();
                if (EditDataLog == 1)
                    RecordFlog = true;
                else
                    RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(UpdateEditDataLog, ex);
                else
                    ErrMsg._errorText(UpdateEditDataLog, ex);
                RecordFlog = false;
            }

            ConnTxt.Close();
            return RecordFlog;
        }

        public bool EditDataLogFunction(string _tablename, string _command, string _condition, string _connstr)
        {
            bool RecordFlog = false;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string UpdateEditDataLog = "UPDATE [" + _tablename + "] SET " + _command + " WHERE " + _condition;
            try
            {
                SqlCommand CmdEditDataLog = new SqlCommand(UpdateEditDataLog, ConnTxt);
                int EditDataLog = CmdEditDataLog.ExecuteNonQuery();
                if (EditDataLog == 1)
                    RecordFlog = true;
                else
                    RecordFlog = false;
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(UpdateEditDataLog, ex);
                else
                    ErrMsg._errorText(UpdateEditDataLog, ex);
                RecordFlog = false;
            }

            ConnTxt.Close();
            return RecordFlog;
        }

        public int GetDataAmountFunction(string _tablename, string _condition)
        {
            int mAmount = 0;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = "SELECT COUNT(*) AS amount FROM " + _tablename + " WHERE " + _condition;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        mAmount = Convert.ToInt32(DataAmount["amount"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectDataAmount, ex);
                else
                    ErrMsg._errorText(SelectDataAmount, ex);
            }
            ConnTxt.Close();
            return mAmount;
        }

        public int GetDataAmountFunction(string _tablename, string _condition, string _connstr)
        {
            int mAmount = 0;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = "SELECT COUNT(*) AS amount FROM " + _tablename + " WHERE " + _condition;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        mAmount = Convert.ToInt32(DataAmount["amount"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectDataAmount, ex);
                else
                    ErrMsg._errorText(SelectDataAmount, ex);
            }
            ConnTxt.Close();
            return mAmount;
        }

        public int GetTotalDataAmountFunction(string _tablename)
        {
            int mAmount = 0;
            SqlConnection ConnTxt = new SqlConnection(ConnStr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = "SELECT COUNT(*) AS amount FROM " + _tablename;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        mAmount = Convert.ToInt32(DataAmount["amount"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectDataAmount, ex);
                else
                    ErrMsg._errorText(SelectDataAmount, ex);
            }
            ConnTxt.Close();
            return mAmount;
        }

        public int GetTotalDataAmountFunction(string _tablename, string _connstr)
        {
            int mAmount = 0;
            SqlConnection ConnTxt = new SqlConnection(_connstr);
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = "SELECT COUNT(*) AS amount FROM " + _tablename;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        mAmount = Convert.ToInt32(DataAmount["amount"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                if (mErrFlag)
                    ErrMsg._error(SelectDataAmount, ex);
                else
                    ErrMsg._errorText(SelectDataAmount, ex);
            }
            ConnTxt.Close();
            return mAmount;
        }
    }
}
