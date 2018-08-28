using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceLibrary
{
    public class PA310Class : DeviceClass
    {
        public PA310Class()
        {
            StartAddrHigh = 0x00;
            StartAddrLow = 0x00;
            LengthHigh = 0x00;
            LengthLow = 0x00;
        }

        public bool AnalysisDataByte()
        {
            try
            {

            }
            catch (Exception)
            { return false; }
            return true;
        }
    }
}
