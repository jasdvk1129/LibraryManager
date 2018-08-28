using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceLibrary
{
    public class DeviceClass
    {
        public byte ID { get; set; }                                    //設備ID
        public byte StartAddrHigh { get; protected set; } = 0x10;       //
        public byte StartAddrLow { get; protected set; } = 0x01;        //
        public byte LengthHigh { get; protected set; } = 0x00;          //
        public byte LengthLow { get; protected set; } = 0x37;           //
    }
}
