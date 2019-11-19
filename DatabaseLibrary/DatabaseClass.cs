using System;
using System.Data;
using System.Data.SqlClient;
using Serilog;
using System.ServiceProcess;

namespace DatabaseLibrary
{
    public class DatabaseClass
    {
        public DatabaseClass()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\db_log\\log-.txt",
                                      rollingInterval: RollingInterval.Day,
                                      outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();        //宣告Serilog初始化
        }
        ///// <summary>
        ///// SQL連接字串
        ///// </summary>
        //private SqlConnectionStringBuilder ConnStr = null;
        /// <summary>
        /// 工作路徑
        /// </summary>
        private readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 確認資料庫是否存在
        /// </summary>
        /// <param name="dBName">資料庫名稱</param>
        /// <param name="ConnTxt">資料庫連線物件</param>
        /// <returns>資料庫存在 (true),不存在(false)或異常(false)</returns>
        public bool CheckDatabaseExistFunction(string dBName, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDatabaseExist = $"SELECT * FROM sys.databases WHERE name = '{dBName}'";
            bool dbExsit;
            try
            {
                SqlCommand CmdCheckDatabaseExist = new SqlCommand(SelectCheckDatabaseExist, ConnTxt);
                SqlDataReader CheckDatabaseExist = CmdCheckDatabaseExist.ExecuteReader();
                if (!CheckDatabaseExist.HasRows)
                    dbExsit = false;
                else
                    dbExsit = true;
                CheckDatabaseExist.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectCheckDatabaseExist);
                dbExsit = false;
            }
            return dbExsit;
        }
        /// <summary>
        /// 確認資料庫是否存在
        /// </summary>
        /// <param name="dbName">資料庫名稱</param>
        /// <param name="ConnStr">資料庫字串</param>
        /// <returns>資料庫存在 (true),不存在(false)或異常(false)</returns>
        public bool CheckDatabaseExistFunction(string dbName, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDatabaseExist = $"SELECT * FROM sys.databases WHERE name = '{dbName}'";
            bool dbExsit;
            try
            {
                SqlCommand CmdCheckDatabaseExist = new SqlCommand(SelectCheckDatabaseExist, ConnTxt);
                SqlDataReader CheckDatabaseExist = CmdCheckDatabaseExist.ExecuteReader();
                if (!CheckDatabaseExist.HasRows)
                    dbExsit = false;
                else
                    dbExsit = true;
                CheckDatabaseExist.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectCheckDatabaseExist);
                dbExsit = false;
            }
            ConnTxt.Close();
            return dbExsit;
        }
        /// <summary>
        /// 建立新的空資料庫
        /// </summary>
        /// <param name="dbName">資料庫名稱</param>
        /// <param name="ConnStr">資料庫字串</param>
        /// <returns>建立成功(true),失敗(false)</returns>
        public bool CreateDatabaseFunction(string dbName, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDatabase = $"CREATE DATABASE [{dbName}]";
            try
            {
                SqlCommand CmdDatabase = new SqlCommand(CreateDatabase, ConnTxt);
                SqlDataReader Database = CmdDatabase.ExecuteReader();
                Database.Close();
                ConnTxt.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, CreateDatabase);
                ConnTxt.Close();
                return false;
            }
        }
        /// <summary>
        /// 建立新的空資料庫
        /// </summary>
        /// <param name="dbName">資料庫名稱</param>
        /// <param name="ConnTxt">資料庫字串</param>
        /// <returns>建立成功(true),失敗(false)</returns>
        public bool CreateDatabaseFunction(string dbName, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDatabase = $"CREATE DATABASE [{dbName}]";
            try
            {
                SqlCommand CmdDatabase = new SqlCommand(CreateDatabase, ConnTxt);
                SqlDataReader Database = CmdDatabase.ExecuteReader();
                Database.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, CreateDatabase);
                return false;
            }
        }
        /// <summary>
        /// 確認資料表是否存在
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="ConnStr">資料庫連接字串</param>
        /// <returns>資料表存在(true),不存在(false)或檢查時發生異常(false)</returns>
        public bool CheckDataTableExistFunction(string dtName, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataTableExist = $"SELECT o.name, i.rows FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE i.indid = 1 AND o.name = '{dtName}' ORDER BY i.rows DESC";
            bool RecordFlog;
            try
            {
                SqlCommand CmdCheckDataTableExist = new SqlCommand(SelectCheckDataTableExist, ConnTxt);
                SqlDataReader CheckDataTableExist = CmdCheckDataTableExist.ExecuteReader();
                if (CheckDataTableExist.HasRows)
                    RecordFlog = true;
                else
                    RecordFlog = false;                 //資料表不存在
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectCheckDataTableExist);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 確認資料表是否存在
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="ConnTxt">外部共用資料庫連結</param>
        /// <returns>資料表存在(true),不存在(false)或檢查時發生異常(false)</returns>
        public bool CheckDataTableExistFunction(string dtName, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataTableExist = $"SELECT o.name, i.rows FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE i.indid = 1 AND o.name = '{dtName}' ORDER BY i.rows DESC";
            bool RecordFlog;
            try
            {
                SqlCommand CmdCheckDataTableExist = new SqlCommand(SelectCheckDataTableExist, ConnTxt);
                SqlDataReader CheckDataTableExist = CmdCheckDataTableExist.ExecuteReader();
                if (CheckDataTableExist.HasRows)
                    RecordFlog = true;
                else
                    RecordFlog = false;                 //資料表不存在
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectCheckDataTableExist);
                RecordFlog = false;
            }
            return RecordFlog;
        }
        /// <summary>
        /// 建立資料表
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="CreateCommand">建立命令</param>
        /// <param name="ConnTxt">外部資料庫連接物件</param>
        /// <returns>Return create success(true) or not(false)</returns>
        public bool CreateDataTableFunction(string dtName, string CreateCommand, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDataTable = $"CREATE TABLE [{dtName}]({CreateCommand})";
            bool RecordFlog;
            try
            {
                SqlCommand CmdDataTable = new SqlCommand(CreateDataTable, ConnTxt);
                SqlDataReader DataTable = CmdDataTable.ExecuteReader();
                DataTable.Close();
                RecordFlog = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, CreateDataTable);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 建立資料表
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="CreateCommand">建立命令</param>
        /// <param name="ConnStr">資料庫連接設定</param>
        /// <returns>Return create success(true) or not(false)</returns>
        public bool CreateDataTableFunction(string dtName, string CreateCommand, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string CreateDataTable = $"CREATE TABLE [{dtName}]({CreateCommand})";
            bool RecordFlog;
            try
            {
                SqlCommand CmdDataTable = new SqlCommand(CreateDataTable, ConnTxt);
                SqlDataReader DataTable = CmdDataTable.ExecuteReader();
                DataTable.Close();
                RecordFlog = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, CreateDataTable);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 確認該筆資料是否存在
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="Condition">過濾條件SQL命令</param>
        /// <param name="ConnStr">資料庫連接設定</param>
        /// <returns></returns>
        public bool CheckDataLogExistFunction(string dtName, string Condition, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataLog = $"SELECT * FROM {dtName} WHERE {Condition}";
            bool RecordFlog;
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
                Log.Error(ex, SelectCheckDataLog);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 確認該筆資料是否存在
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="Condition">過濾條件SQL命令</param>
        /// <param name="ConnTxt">資料庫連接物件</param>
        /// <returns></returns>
        public bool CheckDataLogExistFunction(string dtName, string Condition, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectCheckDataLog = $"SELECT * FROM {dtName} WHERE {Condition}";
            bool RecordFlog;
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
                Log.Error(ex, SelectCheckDataLog);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="dtColumns">資料欄位</param>
        /// <param name="newData">資料</param>
        /// <param name="ConnStr">資料庫連接設定</param>
        /// <returns></returns>
        public bool AddNewDataLogFunction(string dtName, string dtColumns, string newData, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string InsertNewDataLog = $"INSERT INTO {dtName}({dtColumns}) VALUES ({newData})";
            bool RecordFlog;
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
                Log.Error(ex, InsertNewDataLog);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="dtColumns">資料欄位</param>
        /// <param name="newData">資料</param>
        /// <param name="ConnTxt">資料庫連接物件</param>
        /// <returns></returns>
        public bool AddNewDataLogFunction(string dtName, string dtColumns, string newData, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string InsertNewDataLog = $"INSERT INTO {dtName}({dtColumns}) VALUES ({newData})";
            bool RecordFlog;
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
                Log.Error(ex, InsertNewDataLog);
                RecordFlog = false;
            }
            ConnTxt.Close();
            return RecordFlog;
        }
        /// <summary>
        /// 取得總資料筆數函式
        /// </summary>
        /// <param name="dtName">資料表名稱</param>
        /// <param name="Condition">過濾條件</param>
        /// <param name="ConnStr">資料庫連接設定</param>
        /// <returns>資料筆數</returns>
        public int GetTotalDataAmountFunction(string dtName, SqlConnectionStringBuilder ConnStr)
        {
            SqlConnection ConnTxt = new SqlConnection();
            ConnTxt.ConnectionString = ConnStr.ConnectionString;
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = $"SELECT o.name, i.rows FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE i.indid = 1 AND o.name = '{dtName}' ORDER BY i.rows DESC";
            int Amount = 0;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        Amount = Convert.ToInt32(DataAmount["rows"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectDataAmount);
            }
            ConnTxt.Close();
            return Amount;
        }
        /// <summary>
        /// 取得總資料筆數函式
        /// </summary>
        /// <param name="_tablename">資料表名稱</param>
        /// <param name="_condition">過濾條件</param>
        /// <param name="_connstr">資料庫連接字串</param>
        /// <returns>資料筆數</returns>
        public int GetTotalDataAmountFunction(string dtName, SqlConnection ConnTxt)
        {
            if (ConnTxt.State == ConnectionState.Closed)
                ConnTxt.Open();
            string SelectDataAmount = $"SELECT o.name, i.rows FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE i.indid = 1 AND o.name = '{dtName}' ORDER BY i.rows DESC";
            int Amount = 0;
            try
            {
                SqlCommand CmdDataAmount = new SqlCommand(SelectDataAmount, ConnTxt);
                SqlDataReader DataAmount = CmdDataAmount.ExecuteReader();
                if (DataAmount.HasRows)
                {
                    if (DataAmount.Read())
                    {
                        Amount = Convert.ToInt32(DataAmount["rows"]);
                    }
                }
                DataAmount.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectDataAmount);
            }
            ConnTxt.Close();
            return Amount;
        }
        /// <summary>
        /// 重啟SQLserver服務
        /// 請開啟最高權限使用，點選專案屬性→安全性（勾選"啟用ClickOnce安全設置"），在項目名稱裡的"Properties"下面一個"app.manifest"
        /// 找尋 level="asInvoker" uiAccess="false"改為 level="requireAdministrator" uiAccess="false" 
        /// 再(取消勾選"啟用ClickOnce安全設置")，重新建置專案
        /// </summary>
        public void SQLserverReStart(string SQLServiceName = "MSSQLSERVER")
        {
            ServiceController controller = new ServiceController();
            try
            {
                controller.MachineName = Environment.MachineName;
                controller.ServiceName = SQLServiceName;
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error("服務名稱請勿輸入NULL", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Error("確認服務名稱是否正確", ex);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("使用者權限錯誤程式請以最高權限執行", ex);
            }
        }
        ///// <summary>
        ///// 告警資訊歷史訊息清單
        ///// </summary>
        ///// <param name="nowTime">現在時間</param>
        ///// <param name="GID">Gateway編號</param>
        ///// <param name="DeviceIndex">設備編號</param>
        ///// <param name="MessageText">告警訊息</param>
        ///// <param name="ConnStrLog">資料庫連接字串</param>
        ///// <returns></returns>
        //public bool AlarmInfomationLogger(DateTime nowTime, int GID, int DeviceIndex, string MessageText, string ConnStrLog)
        //{
        //    if (CheckDatabaseExistFunction(nowTime))
        //    {
        //        if (!CheckDataTableExistFunction("Alarm_Log", ConnStrLog))
        //        {
        //            CreateDataTableFunction("Alarm_Log", "AlarmID INT IDENTITY PRIMARY KEY,ttime VARCHAR(14) DEFAULT '',ttimen DATETIME,GID INT DEFAULT 0,DeviceIndex INT DEFAULT 0,MessageText NVARCHAR(100) DEFAULT '',SendFlag INT DEFAULT 0", ConnStrLog);
        //        }
        //        SqlConnection ConnTxt = new SqlConnection(ConnStrLog);
        //        if (ConnTxt.State == ConnectionState.Closed)
        //            ConnTxt.Open();
        //        string SelectCheckAlarm = $"SELECT * FROM Alarm_Log WHERE MessageText = '{MessageText}'";
        //        try
        //        {
        //            SqlCommand CmdCheckAlarm = new SqlCommand(SelectCheckAlarm, ConnTxt);
        //            SqlDataReader CheckAlarm = CmdCheckAlarm.ExecuteReader();
        //            if (!CheckAlarm.HasRows)
        //            {
        //                CheckAlarm.Close();
        //                string InsertNewAlarmData = $"INSERT INTO Alarm_Log(ttime,ttimen,GID,DeviceIndex,MessageText) VALUES ('{nowTime.ToString("yyyyMMddHHmmss")}','{nowTime.ToString("yyyy/MM/dd HH:mm:ss")}',{GID},{DeviceIndex},'{MessageText}')";
        //                try
        //                {
        //                    SqlCommand CmdNewAlarmData = new SqlCommand(InsertNewAlarmData, ConnTxt);
        //                    int NewAlarmData = CmdNewAlarmData.ExecuteNonQuery();
        //                    if (NewAlarmData > 0)
        //                    {
        //                        ConnTxt.Close();
        //                        return true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            else
        //            {
        //                CheckAlarm.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        ConnTxt.Close();
        //    }
        //    return false;
        //}
    }
}
