using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusLibrary
{
    public class FatekClass
    {
        private string Right(string sSource, int iLength)
        {
            if (sSource.Trim().Length > 0)
                return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
            else
                return "";
        }

        public bool FatekCheckSum(byte[] receive)
        {
            bool CheckFlag = false;
            int ReceiveLength = receive.Length;         //長度
            byte SumH = receive[ReceiveLength - 3];
            byte SumL = receive[ReceiveLength - 4];
            int SumValueLength = ReceiveLength - 3;
            int SumValue = 0;
            for (int i = 0; i < SumValueLength; i++)
                SumValue += receive[i];
            string ModSum = (SumValue % 256).ToString();
            byte[] _modsum = Encoding.ASCII.GetBytes(Right("" + ModSum, 2));
            if (_modsum[0] == SumH && _modsum[1] == SumL)
                CheckFlag = true;
            else
                CheckFlag = false;
            return CheckFlag;
        }

        public byte[] FatekCmd16bits_Write(byte id, byte func, byte amount, string reg, int no, int val)
        {
            byte[] _id = Encoding.ASCII.GetBytes(Right("00" + id.ToString(), 2));
            byte[] _func = Encoding.ASCII.GetBytes(Right("00" + func.ToString(), 2));
            string HexAmount = Convert.ToString(amount, 16).ToUpper();                  //實際數量轉成16進制
            byte[] _amount = Encoding.ASCII.GetBytes(Right("00" + HexAmount, 2));       //整理數量字串為ASCII
            byte[] _reg = Encoding.ASCII.GetBytes(reg);
            byte[] _no = Encoding.ASCII.GetBytes(Right("00000" + no.ToString(), 5));
            string HexString = Convert.ToString(val, 16).ToUpper();
            byte[] _val = Encoding.ASCII.GetBytes(Right("0000" + HexString, 4));
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
            byte[] LRC = MakeLRCFatek(RevCmd, 0, 17);
            RevCmd[17] = LRC[0];
            RevCmd[18] = LRC[1];
            RevCmd[19] = 0x03;
            return RevCmd;
        }

        public byte[] FatekCmd16bits(byte id, byte func, byte amount, string reg, int no)
        {
            byte[] _id = Encoding.ASCII.GetBytes(Right("00" + id.ToString(), 2));
            byte[] _func = Encoding.ASCII.GetBytes(Right("00" + func.ToString(), 2));
            string HexAmount = Convert.ToString(amount, 16).ToUpper();                  //實際數量轉成16進制
            byte[] _amount = Encoding.ASCII.GetBytes(Right("00" + HexAmount, 2));       //整理數量字串為ASCII
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
            byte[] LRC = MakeLRCFatek(RevCmd, 0, 13);
            RevCmd[13] = LRC[0];
            RevCmd[14] = LRC[1];
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


        public byte[] MakeLRCFatek(byte[] data, int startIndex, int len)
        {
            int TotalSum = 0;
            for (int i = startIndex; i < len; i++)
                TotalSum += data[i];
            string HexString = Convert.ToString(TotalSum, 16).ToUpper();        //轉變16進制
            string HexLRC = Right("00" + HexString, 2);                         //LRC數值計算
            byte[] LRC = Encoding.ASCII.GetBytes(HexLRC);
            return LRC;
        }

        public bool CheckLRCFatek(byte[] response)
        {
            try
            {
                int ResLen = response.Length;
                byte[] lrc = MakeLRCFatek(response, 0, (ResLen - 3));
                if (response[ResLen - 3] == lrc[0] && response[ResLen - 2] == lrc[1])
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
