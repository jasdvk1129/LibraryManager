﻿using System;

namespace MathLibrary
{
    public class MathClass
    {
        /// <summary>
        /// A -> 1<br/>
        /// B -> 2<br/>
        /// C -> 3<br/>
        /// ...
        /// </summary>
        /// <param name="column">輸入字母</param>
        /// <returns></returns>
        public static int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }
        /// <summary>
        /// 10進制轉整數
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ushort[] work10to16(uint data)
        {
            ushort[] ans = new ushort[2];
            byte[] databyte = BitConverter.GetBytes(data);
            ans[0] = Convert.ToUInt16(databyte[1] * 256 + databyte[0]);
            ans[1] = Convert.ToUInt16(databyte[3] * 256 + databyte[2]);
            return ans;
        }
        /// <summary>
        /// 16進制字串轉IEEE754
        /// </summary>
        /// <param name="_val"></param>
        /// <returns></returns>
        public float work16to754(string _val)
        {
            float ans = 0;
            byte[] _temp = new byte[4];
            _temp[3] = Convert.ToByte(_val.Substring(0, 2), 16);
            _temp[2] = Convert.ToByte(_val.Substring(2, 2), 16);
            _temp[1] = Convert.ToByte(_val.Substring(4, 2), 16);
            _temp[0] = Convert.ToByte(_val.Substring(6, 2), 16);
            ans = BitConverter.ToSingle(_temp, 0);
            return ans;
        }
        /// <summary>
        /// 10進制整數轉IEEE754(CDAB)
        /// </summary>
        /// <param name="Hibyte"></param>
        /// <param name="Lobyte"></param>
        /// <returns></returns>
        public float work16to754(ushort Hibyte, ushort Lobyte)
        {
            byte[] HighByte = BitConverter.GetBytes(Hibyte);
            byte[] LowByte = BitConverter.GetBytes(Lobyte);
            float ans = work16to754(HighByte[1], HighByte[0], LowByte[1], LowByte[0]);
            return ans;
        }
        /// <summary>
        /// 10進制浮點數IEEE754轉16進制
        /// </summary>
        /// <param name="Value">數值</param>
        /// <returns></returns>
        public ushort[] work754to16(float Value)
        {
            ushort[] uintData = new ushort[2];
            float[] value = new float[1] { Value };
            Buffer.BlockCopy(value, 0, uintData, 0, 4);
            return uintData;
        }
        /// <summary>
        /// 16進制轉IEEE754
        /// </summary>
        /// <param name="dataHH"></param>
        /// <param name="dataHL"></param>
        /// <param name="dataLH"></param>
        /// <param name="dataLL"></param>
        /// <returns></returns>
        public float work16to754(byte dataHH, byte dataHL, byte dataLH, byte dataLL)
        {
            float ans = 0;
            byte[] _temp = new byte[4];
            _temp[3] = dataHH;
            _temp[2] = dataHL;
            _temp[1] = dataLH;
            _temp[0] = dataLL;
            ans = BitConverter.ToSingle(_temp, 0);
            return ans;
        }
        /// <summary>
        /// 16進制轉32位元10進制
        /// </summary>
        /// <param name="HighData"></param>
        /// <param name="LowData"></param>
        /// <returns></returns>
        public int work16to10(ushort HighData, ushort LowData)
        {
            int ans = Convert.ToInt32(HighData * 65536 + LowData);
            return ans;
        }
        /// <summary>
        /// 16進制轉10進制整數
        /// </summary>
        /// <param name="dataH">高位元</param>
        /// <param name="dataL">低位元</param>
        /// <param name="negative">是否為負數</param>
        /// <returns></returns>
        public short work16to10(byte dataH, byte dataL)
        {
            short ans = Convert.ToInt16(Convert.ToUInt16(dataH * 256 + dataL) > 32767 ? Convert.ToUInt16(dataH * 256 + dataL) - 65536 : Convert.ToUInt16(dataH * 256 + dataL));
            return ans;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHH"></param>
        /// <param name="dataHL"></param>
        /// <param name="dataLH"></param>
        /// <param name="dataLL"></param>
        /// <returns></returns>
        public uint work16to10(byte dataHH, byte dataHL, byte dataLH, byte dataLL)
        {
            uint H = Convert.ToUInt32(dataHH * 256 + dataHL);
            uint L = Convert.ToUInt32(dataLH * 256 + dataLL);
            uint ans = H * 65536 + L;
            return ans;
        }
        /// <summary>
        /// 16進制轉10進制長整數
        /// </summary>
        /// <param name="dataHHH"></param>
        /// <param name="dataHHL"></param>
        /// <param name="dataHH"></param>
        /// <param name="dataHL"></param>
        /// <param name="dataLH"></param>
        /// <param name="dataLL"></param>
        /// <param name="dataLLH"></param>
        /// <param name="dataLLL"></param>
        /// <returns></returns>
        public long work16to10(byte dataHHH, byte dataHHL, byte dataHH, byte dataHL, byte dataLH, byte dataLL, byte dataLLH, byte dataLLL)
        {
            long ans = dataLLH * 256 + dataLLL;
            int HH = dataHHH * 256 + dataHHL;
            int HL = dataHH * 256 + dataHL;
            int LH = dataLH * 256 + dataLL;
            int LL = dataLLH * 256 + dataLLL;
            int H = HH * 65536 + HL;
            int L = LH * 65536 + LL;
            ans = H * 65536 + L;
            return ans;
        }
        /// <summary>
        /// 16進制轉10進制長整數
        /// </summary>
        /// <param name="dataHH"></param>
        /// <param name="dataHL"></param>
        /// <param name="dataLH"></param>
        /// <param name="dataLL"></param>
        /// <param name="negative"></param>
        /// <returns></returns>
        public long work16to10(byte dataHH, byte dataHL, byte dataLH, byte dataLL, bool negative = false)
        {
            long H = dataHH * 256 + dataHL;
            long L = dataLH * 256 + dataLL;
            long ans = H * 65536 + L;
            if (negative)
                if (ans > 2147483648)
                    ans -= 4294967295;
            return ans;
        }
        /// <summary>
        /// 2進制轉10進制
        /// </summary>
        /// <param name="str2">字串</param>
        /// <returns></returns>
        public int work2to10(string str2)
        {
            int ans = Convert.ToInt32(str2, 2);
            return ans;
        }
        /// <summary>
        /// 2進制轉10進制
        /// </summary>
        /// <param name="state">狀態陣列</param>
        /// <returns></returns>
        public int work2to10(byte[] state)
        {
            string str = "";
            foreach (var item in state)
            {
                str += item;
            }
            int ans = Convert.ToInt32(str, 2);
            return ans;
        }
        /// <summary>
        /// 16進制字串轉10進制
        /// </summary>
        /// <param name="_val"></param>
        /// <returns></returns>
        public int work16to10(string _val)
        {
            int ans = Convert.ToInt32(_val, 16);
            return ans;
        }
        /// <summary>
        /// 16進制陣列轉10進制
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public int work16to10(byte[] _data)
        {
            int ans = BitConverter.ToInt32(_data, 0);
            return ans;
        }
        /// <summary>
        /// 16進制陣列轉10進制
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="startindex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int work16to10(byte[] _data, int startindex, int len)
        {
            byte[] newArray = new byte[len];            //建立新陣列
            for (int i = 0; i < len; i++)
                newArray[i] = _data[i + startindex];
            int ans = BitConverter.ToInt16(newArray, 0);
            return ans;
        }
        /// <summary>
        /// 陣列切割
        /// </summary>
        /// <param name="orgArray"></param>
        /// <param name="startindex"></param>
        /// <param name="newlength"></param>
        /// <returns></returns>
        public byte[] SpiltArray(byte[] orgArray, int startindex, int newlength)
        {
            byte[] newArray = new byte[newlength];
            for (int i = startindex; i < newlength; i++)
                newArray[i - startindex] = orgArray[i];
            return newArray;
        }
        /// <summary>
        /// AI解析函式
        /// </summary>
        /// <param name="_kind">解析類型</param>
        /// <param name="_val">AI原始值</param>
        /// <param name="_Emend">校正值</param>
        /// <returns></returns>
        public double AiSwitchNumber(int _kind, int _val, double _Emend)
        {
            double ans = 0;
            switch (_kind)
            {
                case 0:             //AI不需解析
                    {
                        ans = _val;
                        break;
                    }
                case 1:             //AI需解析DI
                    {
                        if (_val < 500)
                            ans = 0;
                        else
                            ans = 1;
                        break;
                    }
                case 2:             //AI需解析溫度K Type
                    {
                        ans = AiTransform(_val) + _Emend;
                        break;
                    }
                case 3:             //AI不需解析帶正負號除10
                    {
                        if (_val < 32768)
                            ans = _val * 0.1;
                        else
                            ans = (_val - 65535) * -0.1;
                        break;
                    }
                case 4:             //AI數值除10不帶正負號
                    {
                        ans = _val * 0.1;
                        break;
                    }
                case 5:             //AI數值不解析帶正負號除100
                    {
                        if (_val < 32768)
                            ans = _val * 0.01;
                        else
                            ans = (_val - 65535) * -0.01;
                        break;
                    }
                case 6:             //AI數值不解析除100
                    {
                        ans = _val * 0.01;
                        break;
                    }
                case 7:             //濕度需解析
                    {
                        ans = 70 * _val / 408 - 6;
                        break;
                    }
                case 8:             //EE16-FT6A26溫度解析
                    {
                        ans = _val * 0.48828125;
                        break;
                    }
                case 9:             //EE16-FT6A26濕度解析
                    {
                        ans = _val * 0.09765625;
                        break;
                    }
            }
            ans += _Emend;
            return ans;
        }
        /// <summary>
        /// AI轉換
        /// </summary>
        /// <param name="heat"></param>
        /// <returns></returns>
        private double AiTransform(int heat)
        {
            int rt = heat;
            double T = 0;
            double lnx = Math.Log(Convert.ToDouble(heat));
            double d3 = Math.Pow(lnx, 3);
            double d2 = Math.Pow(lnx, 2);
            double nd8 = Math.Pow(10, -8);
            double rt4 = Math.Pow((rt - 784), 4);
            double nd6 = Math.Pow(10, -6);
            double nd4 = Math.Pow(10, -4);
            double rt3 = Math.Pow((rt - 784), 3);
            double rt2 = Math.Pow((rt - 784), 2);
            if (lnx >= 0 && rt < 33)
                T = 125;
            else if (rt >= 33 && rt < 512)
                T = -1.437 * d3 + 20.994 * d2 - 136.23 * (lnx) + 406.8;
            else if (rt >= 512 && rt < 783)
                T = -64.189 * d3 + 1203.9 * d2 - 7575.1 * (lnx) + 16012.5;
            else if (rt >= 783 && rt < 879)
                T = -1 * nd8 * rt4 + (2 * nd6) * rt3 - (3 * nd4) * rt2 - 0.0989 * (rt - 784) - 1;
            else if (rt >= 879 && rt < 955)
                T = -1 * nd8 * rt4 + (2 * nd6) * rt3 - (3 * nd4) * rt2 - 0.106 * (rt - 784) - 1;
            else if (rt >= 955)
                T = -20;
            T = Math.Round(T, 1);
            return T;
        }
        /// <summary>
        /// 三數字取最大值
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="val3"></param>
        /// <returns></returns>
        public double MaxValue(double val1, double val2, double val3)
        {
            double ans;
            if (val1 > val2)
                ans = val1;
            else
                ans = val2;
            if (ans < val3)
                ans = val3;
            return ans;
        }
        /// <summary>
        /// 四數字取最大值
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="val3"></param>
        /// <param name="val4"></param>
        /// <returns></returns>
        public double MaxValue(double val1, double val2, double val3, double val4)
        {
            double ans = 0;
            if (val1 > val2)
                ans = val1;
            else
                ans = val2;
            if (ans < val3)
                ans = val3;
            if (ans < val4)
                ans = val4;
            return ans;
        }
        /// <summary>
        /// 五數字取最大值
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="val3"></param>
        /// <param name="val4"></param>
        /// <param name="val5"></param>
        /// <returns></returns>
        public double MaxValue(double val1, double val2, double val3, double val4, double val5)
        {
            double ans = 0;
            if (val1 > val2)
                ans = val1;
            else
                ans = val2;
            if (ans < val3)
                ans = val3;
            if (ans < val4)
                ans = val4;
            if (ans < val5)
                ans = val5;
            return ans;
        }
        /// <summary>
        /// 12小時轉24小時
        /// </summary>
        /// <param name="_hour"></param>
        /// <returns></returns>
        public int Clock24h(int _hour)
        {
            int ans = _hour + 8;
            if (_hour >= 24)
                ans = _hour - 24;
            return ans;
        }
        /// <summary>
        /// 1byte 10進制轉2進制
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string work10to2(byte Data)
        {
            string bin = "00000000";
            bin = ("00000000" + Convert.ToString(Data, 2)).Right(8);
            return bin;
        }
        /// <summary>
        /// 2byte 10進制轉2進制
        /// </summary>
        /// <param name="DataH"></param>
        /// <param name="DataL"></param>
        /// <returns></returns>
        public string work10to2(byte DataH, byte DataL)
        {
            string bin = "0000000000000000";
            bin = ("00000000" + Convert.ToString(DataH, 2)).Right(8) + ("00000000" + Convert.ToString(DataL, 2)).Right(8);
            return bin;
        }
        /// <summary>
        /// ushort 10進制轉2進制
        /// </summary>
        /// <param name="DataH"></param>
        /// <param name="DataL"></param>
        /// <returns></returns>
        public string work10to2(ushort data)
        {
            string bin = "0000000000000000";
            bin = ("0000000000000000" + Convert.ToString(data, 2)).Right(16);
            return bin;
        }
        /// <summary>
        /// 多位元10進制轉2進制
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="startindex"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public string work10to2(byte[] Data, int startindex, int dataLength)
        {
            string bin = "";
            for (int i = startindex; i < (startindex + dataLength); i++)
            {
                string nowData = ("00000000" + Convert.ToString(Data[i], 2)).Right(8);
                bin += StrReverse(nowData);
            }
            return bin;
        }
        /// <summary>
        /// 字串反轉
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public string StrReverse(string _str)
        {
            string str = _str;
            char[] ArrayStr = str.ToCharArray();
            Array.Reverse(ArrayStr);
            str = new string(ArrayStr);
            return str;
        }
        /// <summary>
        /// 數字星期轉文字
        /// </summary>
        /// <param name="WeekValue"></param>
        /// <returns></returns>
        public string WeekValueToString(int WeekValue)
        {
            string ValueWeek = "";
            switch (WeekValue)
            {
                case 0:
                    {
                        ValueWeek = "Sunday";
                        break;
                    }
                case 1:
                    {
                        ValueWeek = "Monday";
                        break;
                    }
                case 2:
                    {
                        ValueWeek = "Tuesday";
                        break;
                    }
                case 3:
                    {
                        ValueWeek = "Wednesday";
                        break;
                    }
                case 4:
                    {
                        ValueWeek = "Thursday";
                        break;
                    }
                case 5:
                    {
                        ValueWeek = "Friday";
                        break;
                    }
                case 6:
                    {
                        ValueWeek = "Saturday";
                        break;
                    }
            }
            return ValueWeek;
        }
    }
}
