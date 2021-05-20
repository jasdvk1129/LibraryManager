using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeControl_MYSql_Library.Modules
{
    public class TimeControlSetting
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 群組編號
        /// </summary>
        public int GroupIndex { get; set; }
        /// <summary>
        /// (年-月-日 00:00:00)
        /// </summary>
        public DateTime ttimen { get; set; }
        /// <summary>
        /// 特殊日類型
        /// <para> 0 = 例假</para>
        /// <para> 1 = 特殊</para>
        /// </summary>
        public int SpecificDateIndex { get; set; }
    }
}
