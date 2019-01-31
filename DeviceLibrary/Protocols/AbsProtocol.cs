using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceLibrary.Protocols
{
    public abstract class AbsProtocol
    {
        /// <summary>
        /// 資料庫IP位址
        /// </summary>
        public string SQLServerIp { get; set; }
        /// <summary>
        /// 資料庫名稱
        /// </summary>
        public string SQLDatabase { get; set; }
        /// <summary>
        /// 資料庫使用者帳號
        /// </summary>
        public string SQLUserId { get; set; }
        /// <summary>
        /// 資料庫使用者密碼
        /// </summary>
        public string SQLUserPwd { get; set; }
        /// <summary>
        /// 命令建構字串
        /// </summary>
        /// <returns></returns>
        public abstract byte[] CommandCreater();
        /// <summary>
        /// 資料解析函數
        /// </summary>
        /// <returns></returns>
        public abstract bool AnalysisDataFunction(byte[] Cmd,byte[] Inbyte);
        /// <summary>
        /// 歷史資料紀錄函數
        /// </summary>
        public abstract void HistoryLogger();
        /// <summary>
        /// CRC計算
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        protected byte[] MakeCRC16(byte[] data, byte startIndex, byte len)
        {
            byte CRC16Lo, CRC16Hi;   //CRC寄存器
            byte CL, CH;       //多項式碼&HA001
            byte SaveHi, SaveLo;
            int i;
            int Flag;
            CRC16Lo = 0xFF;
            CRC16Hi = 0xFF;
            CL = 0x1;
            CH = 0xA0;
            for (i = startIndex; i <= (startIndex + len - 1); i++)
            {
                CRC16Lo = Convert.ToByte(CRC16Lo ^ data[i]);            //每一個數据与CRC寄存器進行异或
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi = Convert.ToByte(CRC16Hi / 2);              //高位右移一位
                    CRC16Lo = Convert.ToByte(CRC16Lo / 2);              //低位右移一位
                    if ((SaveHi & 0x01) == 0x01)                        //如果高位字節最后一位為1
                    {
                        CRC16Lo = Convert.ToByte(CRC16Lo | 0x80);       //則低位字節右移后前面補1
                    }             //否則自動補0
                    if ((SaveLo & 0x01) == 0x01)                        //如果LSB為1，則与多項式碼進行异或
                    {
                        CRC16Hi = Convert.ToByte(CRC16Hi ^ CH);
                        CRC16Lo = Convert.ToByte(CRC16Lo ^ CL);
                    }
                }
            }
            Byte[] ReturnData = new Byte[2];
            ReturnData[0] = CRC16Hi;        //CRC高位
            ReturnData[1] = CRC16Lo;        //CRC低位
            return ReturnData;
        }
        /// <summary>
        /// 驗證CRC
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected bool CheckCRC16(byte[] response)
        {
            try
            {
                byte[] crc16 = MakeCRC16(response, 0, Convert.ToByte(response[2] + 3));
                if ((response[response.Length - 2] != crc16[1]) || (response[response.Length - 1] != crc16[0]))
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
