using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeControl_MYSql_Library.Modules
{
    public class TimeControlConfig
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 群組編號
        /// </summary>
        public int GroupIndex { get; set; }
        /// <summary>
        /// 星期幾編號
        /// <para>0~6 = 日~六</para>
        /// <para>7 = 例假</para>
        /// <para>8 = 特殊</para>
        /// </summary>
        public int WeekIndex { get; set; }
        /// <summary>
        /// 開關功能
        /// </summary>
        public bool SwitchIndex { get; set; }
        /// <summary>
        /// 開啟時間
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 關閉時間
        /// </summary>
        public string EndTime { get; set; }
    }
}
