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

        public byte[] FatekCmd16bits_Write(byte id, byte func,byte amount,string reg,int no,int val)
        {
            byte[] _id = Encoding.ASCII.GetBytes(Right("00" + id.ToString(), 2));
            byte[] _func = Encoding.ASCII.GetBytes(Right("00" + func.ToString(), 2));
            byte[] _amount = Encoding.ASCII.GetBytes(Right("00" + amount.ToString(), 2));
            byte[] _reg = Encoding.ASCII.GetBytes(reg);
            byte[] _no = Encoding.ASCII.GetBytes(Right("00000" + no.ToString(), 5));
            byte[] _val = Encoding.ASCII.GetBytes(Right("0000" + val.ToString(), 4));
            byte[] RevCmd = new byte[20];
            RevCmd[0] = 0x02;
            RevCmd[1] = _id[0];
            RevCmd[2] = _id[1];
            RevCmd[3] = _func[0];
            RevCmd[4] = _func[1];
            RevCmd[5] = _amount[0];
            RevCmd[6] = _amount[1];
            RevCmd[7] = _reg[0];
            RevCmd[8] = _no[0];
            RevCmd[9] = _no[1];
            RevCmd[10] = _no[2];
            RevCmd[11] = _no[3];
            RevCmd[12] = _no[4];
            RevCmd[13] = _val[0];
            RevCmd[14] = _val[1];
            RevCmd[15] = _val[2];
            RevCmd[16] = _val[3];
            int _total = 0;
            for (int i = 0; i < 17; i++)
                _total += RevCmd[i];
            string hex = Convert.ToString(_total, 16).ToUpper();
            byte[] _check = Encoding.ASCII.GetBytes(hex);
            RevCmd[17] = _check[1];
            RevCmd[18] = _check[2];
            RevCmd[19] = 0x03;
            return RevCmd;
        }

        public byte[] FatekCmd16bits(byte id, byte func, byte amount, string reg, int no)
        {
            byte[] _id = Encoding.ASCII.GetBytes(Right("00" + id.ToString(), 2));
            byte[] _func = Encoding.ASCII.GetBytes(Right("00" + func.ToString(), 2));
            byte[] _amount = Encoding.ASCII.GetBytes(Right("00" + amount.ToString(), 2));
            byte[] _reg = Encoding.ASCII.GetBytes(reg);
            byte[] _no = Encoding.ASCII.GetBytes(Right("00000" + no.ToString(), 5));
            byte[] RevCmd = new byte[16];
            RevCmd[0] = 0x02;
            RevCmd[1] = _id[0];
            RevCmd[2] = _id[1];
            RevCmd[3] = _func[0];
            RevCmd[4] = _func[1];
            RevCmd[5] = _amount[0];
            RevCmd[6] = _amount[1];
            RevCmd[7] = _reg[0];
            RevCmd[8] = _no[0];
            RevCmd[9] = _no[1];
            RevCmd[10] = _no[2];
            RevCmd[11] = _no[3];
            RevCmd[12] = _no[4];
            int _total = 0;
            for (int i = 0; i < 14; i++)
                _total += RevCmd[i];
            string hex = Convert.ToString(_total, 16).ToUpper();
            byte[] _check = Encoding.ASCII.GetBytes(hex);
            RevCmd[13] = _check[1];
            RevCmd[14] = _check[2];
            RevCmd[15] = 0x03;
            return RevCmd;
        }

        public int FatekData16bits(byte data1, byte data2, byte data3, byte data4)
        {
            int Ans = 0;
            string[] data = new string[4];
            data[0] = Convert.ToString((char)data1);
            data[1] = Convert.ToString((char)data2);
            data[2] = Convert.ToString((char)data3);
            data[3] = Convert.ToString((char)data4);
            Ans = Convert.ToUInt16(data[0] + data[1] + data[2] + data[3], 16);
            return Ans;
        }

        public string FatekData16bitstoString(byte data1, byte data2, byte data3, byte data4)
        {
            string Ans;
            string[] data = new string[4];
            data[0] = Convert.ToString((char)data1);
            data[1] = Convert.ToString((char)data2);
            data[2] = Convert.ToString((char)data3);
            data[3] = Convert.ToString((char)data4);
            Ans = data[0] + data[1] + data[2] + data[3];
            return Ans;
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
            RevCmd[2] = MCmd[0];
            RevCmd[3] = MCmd[1];
            RevCmd[4] = MLen[0];
            RevCmd[5] = MLen[1];
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
