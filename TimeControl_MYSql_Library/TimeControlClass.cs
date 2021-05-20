using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeControl_MYSql_Library.Enums;
using TimeControl_MYSql_Library.Methods;
using TimeControl_MYSql_Library.Modules;

namespace TimeControl_MYSql_Library
{
    /// <summary>
    /// 時控
    /// </summary>
    public class TimeControlClass
    {
        /// <summary>
        /// 群組編號
        /// </summary>
        public int GroupIndex { get; set; }
        /// <summary>
        /// MYSql方法
        /// </summary>
        public MySqlMethod MySqlMethod { get; set; }
        /// <summary>
        /// 時控
        /// </summary>
        /// <param name="groupIndex">群組編號</param>
        /// <param name="DataSource">資料庫位址</param>
        /// <param name="InitialCatalog">資料庫名稱</param>
        /// <param name="UserID">帳號</param>
        /// <param name="Password">密碼</param>
        public TimeControlClass(int groupIndex, string DataSource, string InitialCatalog, string UserID, string Password)
        {
            GroupIndex = groupIndex;
            MySqlMethod = new MySqlMethod(GroupIndex, DataSource, InitialCatalog, UserID, Password);
            MySqlMethod.CheckDataTableExistFunction();
        }
        /// <summary>
        /// 永久觸發控制方法
        /// </summary>
        /// <returns></returns>
        public DefaultBoolean Time_Keep_Control_Function()
        {
            DefaultBoolean Switch = DefaultBoolean.None;
            List<TimeControlConfig> TimeControlConfig = new List<TimeControlConfig>();
            TimeControlSetting SpecificDate = null;
            TimeControlConfig = MySqlMethod.SearchTimeControl();
            SpecificDate = MySqlMethod.SearchTimeControlSetting();
            int Week = (int)DateTime.Now.DayOfWeek;
            var Generalday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == Week & Convert.ToDateTime(g.StartTime) < DateTime.Now & Convert.ToDateTime(g.EndTime) >= DateTime.Now).ToList();
            var OfficialHoliday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == 7 & Convert.ToDateTime(g.StartTime) < DateTime.Now & Convert.ToDateTime(g.EndTime) >= DateTime.Now).ToList();
            var Specialday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == 8 & Convert.ToDateTime(g.StartTime) < DateTime.Now & Convert.ToDateTime(g.EndTime) >= DateTime.Now).ToList();
            if (SpecificDate != null)
            {

                if (SpecificDate.SpecificDateIndex == 0)
                {
                    if (OfficialHoliday.Count > 0)
                    {
                        if (Convert.ToDateTime(OfficialHoliday[0].StartTime) <= DateTime.Now)
                        {
                            Switch = DefaultBoolean.True;
                        }
                        else if (Convert.ToDateTime(OfficialHoliday[0].EndTime) >= DateTime.Now)
                        {
                            Switch = DefaultBoolean.False;
                        }
                    }
                }
                else if (SpecificDate.SpecificDateIndex == 1)
                {
                    if (Specialday.Count > 0)
                    {
                        if (Convert.ToDateTime(Specialday[0].StartTime) <= DateTime.Now)
                        {
                            Switch = DefaultBoolean.True;
                        }
                        else if (Convert.ToDateTime(Specialday[0].EndTime) <= DateTime.Now)
                        {
                            Switch = DefaultBoolean.False;
                        }
                    }
                }
            }
            else
            {
                if (Generalday.Count > 0)
                {
                    if (Convert.ToDateTime(Generalday[0].StartTime) <= DateTime.Now)
                    {
                        Switch = DefaultBoolean.True;
                    }
                    else if (Convert.ToDateTime(Generalday[0].EndTime) >= DateTime.Now)
                    {
                        Switch = DefaultBoolean.False;
                    }
                }
            }
            return Switch;
        }
        /// <summary>
        /// 一段觸發控制方法
        /// </summary>
        /// <returns></returns>
        public DefaultBoolean Time_Pulse_Control_Function()
        {
            DefaultBoolean Switch = DefaultBoolean.None;
            List<TimeControlConfig> TimeControlConfig = new List<TimeControlConfig>();
            TimeControlSetting SpecificDate = null;
            TimeControlConfig = MySqlMethod.SearchTimeControl();
            SpecificDate = MySqlMethod.SearchTimeControlSetting();
            int Week = (int)DateTime.Now.DayOfWeek;
            var Generalday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == Week).ToList();
            var OfficialHoliday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == 7).ToList();
            var Specialday = TimeControlConfig.Where(g => g.GroupIndex == GroupIndex & g.WeekIndex == 8).ToList();
            if (SpecificDate != null)
            {
                if (SpecificDate.SpecificDateIndex == 0)
                {
                    if (OfficialHoliday.Count > 0)
                    {
                        foreach (var item in OfficialHoliday)
                        {
                            if (Convert.ToDateTime(item.StartTime) == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00")))
                            {
                                Switch = (DefaultBoolean)Convert.ToInt32(item.SwitchIndex);
                            }
                        }
                    }
                }
                else if (SpecificDate.SpecificDateIndex == 1)
                {
                    if (Specialday.Count > 0)
                    {
                        foreach (var item in Specialday)
                        {
                            if (Convert.ToDateTime(item.StartTime) == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00")))
                            {
                                Switch = (DefaultBoolean)Convert.ToInt32(item.SwitchIndex);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Generalday.Count > 0)
                {
                    foreach (var item in Generalday)
                    {
                        if (Convert.ToDateTime(item.StartTime) == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00")))
                        {
                            Switch = (DefaultBoolean)Convert.ToInt32(item.SwitchIndex);
                        }
                    }
                }
            }
            return Switch;
        }
    }
}