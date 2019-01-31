using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceLibrary.Protocols.OtherProtocols
{
    public abstract class AbsOtherProtocol : AbsProtocol
    {
        /// <summary>
        /// 溫度
        /// </summary>
        public float TEMPTURE { get; protected set; }
        /// <summary>
        /// 濕度
        /// </summary>
        public float RH { get; protected set; }

    }
}
