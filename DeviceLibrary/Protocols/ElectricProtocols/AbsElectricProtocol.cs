using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceLibrary.Protocols.ElectricProtocols
{
    public abstract class AbsElectricProtocol : AbsProtocol
    {
        /// <summary>
        /// 資料表索引值
        /// </summary>
        public int DataIndex { get; set; }
        /// <summary>
        /// R線電壓
        /// </summary>
        public float RV { get; protected set; }
        /// <summary>
        /// S線電壓
        /// </summary>
        public float SV { get; protected set; } 
        /// <summary>
        /// T線電壓
        /// </summary>
        public float TV { get; protected set; }
        /// <summary>
        /// R相電壓
        /// </summary>
        public float RSV { get; protected set; }
        /// <summary>
        /// S相電壓
        /// </summary>
        public float STV { get; protected set; }
        /// <summary>
        /// T相電壓
        /// </summary>
        public float TRV { get; protected set; }
        /// <summary>
        /// R相電流
        /// </summary>
        public float RA { get; protected set; }
        /// <summary>
        /// S相電流
        /// </summary>
        public float SA { get; protected set; }
        /// <summary>
        /// T相電流
        /// </summary>
        public float TA { get; protected set; }
        /// <summary>
        /// 瞬間功率
        /// </summary>
        public float KW { get; protected set; }
        /// <summary>
        /// 累積功率
        /// </summary>
        public float KWH { get; protected set; }
        /// <summary>
        /// 功率因數
        /// </summary>
        public float PFE { get; protected set; }
        /// <summary>
        /// 頻率
        /// </summary>
        public float HZ { get; protected set; }
        /// <summary>
        /// 瞬間視在功率
        /// </summary>
        public float KVA { get; protected set; }
        /// <summary>
        /// 累積視在功率
        /// </summary>
        public float KVAH { get; protected set; }
        /// <summary>
        /// 瞬間虛功率
        /// </summary>
        public float KVAR { get; protected set; }
        /// <summary>
        /// 累積虛功率
        /// </summary>
        public float KVARH { get; protected set; }
    }
}
