using System;
using System.Text;

namespace ModbusLibrary
{
    public class ModbusClass
    {
        private string Right(string sSource, int iLength)
        {
            if (sSource.Trim().Length > 0)
                return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
            else
                return "";
        }

        public byte[] MakeModTcpWriteCmd(byte id, byte func, short cmd, short val)
        {
            byte[] MCmd = BitConverter.GetBytes(cmd);
            byte[] MVal = BitConverter.GetBytes(val);
            byte[] RevCmd = new byte[12];
            RevCmd[0] = 0x00;
            RevCmd[1] = 0x00;
            RevCmd[2] = 0x00;
            RevCmd[3] = 0x00;
            RevCmd[4] = 0x00;
            RevCmd[5] = 0x06;
            RevCmd[6] = id;
            RevCmd[7] = func;
            RevCmd[8] = MCmd[1];
            RevCmd[9] = MCmd[0];
            RevCmd[10] = MVal[1];
            RevCmd[11] = MVal[0];
            return RevCmd;
        }

        public byte[] MakeModbusReadCmd(byte id, byte func, short cmd, short len)
        {
            byte[] MCmd = BitConverter.GetBytes(cmd);
            byte[] MLen = BitConverter.GetBytes(len);
            byte[] RevCmd = new byte[8];
            RevCmd[0] = id;
            RevCmd[1] = func;
            RevCmd[2] = MCmd[1];
            RevCmd[3] = MCmd[0];
            RevCmd[4] = MLen[1];
            RevCmd[5] = MLen[0];
            byte[] crc16 = MakeCRC16(RevCmd, 0, 6);
            RevCmd[6] = crc16[1];
            RevCmd[7] = crc16[0];
            return RevCmd;
        }

        public byte[] MakeModTcpReadCmd(byte id, byte func, short cmd, short len)
        {
            byte[] MCmd = BitConverter.GetBytes(cmd);
            byte[] MLen = BitConverter.GetBytes(len);
            byte[] RevCmd = new byte[12];
            RevCmd[0] = 0x00;
            RevCmd[1] = 0x00;
            RevCmd[2] = 0x00;
            RevCmd[3] = 0x00;
            RevCmd[4] = 0x00;
            RevCmd[5] = 0x06;
            RevCmd[6] = id;
            RevCmd[7] = func;
            RevCmd[8] = MCmd[1];
            RevCmd[9] = MCmd[0];
            RevCmd[10] = MLen[1];
            RevCmd[11] = MLen[0];
            return RevCmd;
        }



        public byte[] MakeCRC16(byte[] data, byte startIndex, byte len)
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
                CRC16Lo = Convert.ToByte(CRC16Lo ^ data[i]);//每一個數据与CRC寄存器進行异或
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi = Convert.ToByte(CRC16Hi / 2);     // '高位右移一位
                    CRC16Lo = Convert.ToByte(CRC16Lo / 2);     //'低位右移一位
                    if ((SaveHi & 0x01) == 0x01)  //'如果高位字節最后一位為1
                    {
                        CRC16Lo = Convert.ToByte(CRC16Lo | 0x80);   //'則低位字節右移后前面補1
                    }             // '否則自動補0
                    if ((SaveLo & 0x01) == 0x01)  //'如果LSB為1，則与多項式碼進行异或
                    {
                        CRC16Hi = Convert.ToByte(CRC16Hi ^ CH);
                        CRC16Lo = Convert.ToByte(CRC16Lo ^ CL);
                    }
                }
            }
            Byte[] ReturnData = new Byte[2];
            ReturnData[0] = CRC16Hi;       //'CRC高位
            ReturnData[1] = CRC16Lo;      //'CRC低位
            return ReturnData;
        }

        public bool CheckCRC16(byte[] response)
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
