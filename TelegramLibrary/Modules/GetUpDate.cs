using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary.Modules
{
    public class GetUpDate
    {
        /// <summary>
        /// 傳訊ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 個人姓
        /// </summary>
        public string first_name { get; set; }
        /// <summary>
        /// 個人名
        /// </summary>
        public string last_name { get; set; }
        /// <summary>
        /// 群組名稱 (NULL = 個人)
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// false = 被踢出群組，true = 被加入群組
        /// </summary>
        public bool all_members_are_administrators { get; set; }
        /// <summary>
        /// 類型 個人或公開
        /// </summary>
        public string type { get; set; }
    }
}
