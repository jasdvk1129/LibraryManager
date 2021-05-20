using Dapper;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeControl_MYSql_Library.Modules;

namespace TimeControl_MYSql_Library.Methods
{
    /// <summary>
    /// MYSql方法
    /// </summary>
    public class MySqlMethod
    {
        /// <summary>
        /// 群組編號
        /// </summary>
        public int GroupIndex { get; set; }
        /// <summary>
        /// server資料庫連結資訊
        /// </summary>
        public MySqlConnectionStringBuilder Serverscsb;
        /// <summary>
        /// MariaDB資料庫連結資訊
        /// </summary>
        public MySqlConnectionStringBuilder scsb;
        /// <summary>
        /// MYSql方法初始化
        /// </summary>
        /// <param name="groupIndex">群組編號</param>
        /// <param name="DataSource">資料庫位址</param>
        /// <param name="InitialCatalog">資料庫名稱</param>
        /// <param name="UserID">帳號</param>
        /// <param name="Password">密碼</param>
        public MySqlMethod(int groupIndex, string DataSource, string InitialCatalog, string UserID, string Password)
        {
            Serverscsb = new MySqlConnectionStringBuilder()
            {
                Database = "mysql",
                Server = DataSource,
                UserID = UserID,
                Password = Password,
                CharacterSet = "utf8"
            };
            scsb = new MySqlConnectionStringBuilder()
            {
                Database = InitialCatalog,
                Server = DataSource,
                UserID = UserID,
                Password = Password,
                CharacterSet = "utf8"
            };
        }
        /// <summary>
        /// 確認資料表是否存在
        /// </summary>
        public void CheckDataTableExistFunction()
        {
            string SelectCheckDataTableExist = "SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TimerControlConfig' ";
            string SelectCheckDataTableExist1 = "SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TimerControlSetting' ";
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    var Exist = conn.QueryFirstOrDefault<bool>(SelectCheckDataTableExist);
                    var Exist1 = conn.QueryFirstOrDefault<bool>(SelectCheckDataTableExist1);
                    if (!Exist)
                    {
                        CreateDataTableFunction(0);
                    }
                    if (!Exist1)
                    {
                        CreateDataTableFunction(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, SelectCheckDataTableExist);
            }
        }
        /// <summary>
        /// 建立資料表
        /// </summary>
        public void CreateDataTableFunction(int TableIndex)
        {
            string CreateCommand = string.Empty;
            switch (TableIndex)
            {
                case 0:
                    {
                        CreateCommand = "CREATE TABLE TimerControlConfig (" +
                                  "PK  MEDIUMINT NOT NULL AUTO_INCREMENT ," +
                                  "GroupIndex INT," +
                                  "WeekIndex INT," +
                                  "SwitchIndex BIT," +
                                  "StartTime varchar(5) DEFAULT '00:00'," +
                                  "EndTime varchar(5) DEFAULT '00:00'," +
                                  "PRIMARY KEY (PK,GroupIndex,WeekIndex))";
                    }
                    break;
                case 1:
                    {
                        CreateCommand = "CREATE TABLE TimerControlSetting (" +
                                 "PK  MEDIUMINT NOT NULL AUTO_INCREMENT ," +
                                 "GroupIndex INT," +
                                 "ttimen DATETIME," +
                                 "SpecificDateIndex INT," +
                                 "PRIMARY KEY (PK,GroupIndex))";
                    }
                    break;
            }
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    var Exist = conn.Execute(CreateCommand);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, CreateCommand);
            }
        }
        /// <summary>
        /// 查詢時控資訊
        /// </summary>
        /// <returns></returns>
        public List<TimeControlConfig> SearchTimeControl()
        {
            List<TimeControlConfig> configs = null;
            string sql = "SELECT * FROM TimerControlConfig WHERE GroupIndex = @GroupIndex";
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    configs = conn.Query<TimeControlConfig>(sql, new { GroupIndex }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, sql);
            }
            return configs;
        }
        /// <summary>
        /// 查詢特殊日期資訊
        /// </summary>
        /// <returns></returns>
        public TimeControlSetting SearchTimeControlSetting()
        {
            TimeControlSetting setting = null;
            string sql = "SELECT * FROM TimerControlSetting WHERE GroupIndex = @GroupIndex AND ttimen = @ttimen";
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    setting = conn.QuerySingleOrDefault<TimeControlSetting>(sql, new { GroupIndex,ttimen = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")) });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, sql);
            }
            return setting;
        }
    }
}
