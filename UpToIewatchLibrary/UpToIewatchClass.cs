using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UpToIewatchLibrary
{
    public class UpToIewatchClass
    {
        /// <summary>
        /// UDP上傳指定 Port 4660
        /// </summary>
        UdpClient myUdpClient = new UdpClient(4660);
        /// <summary>
        /// AI 初始化
        /// </summary>
        public int[] aizer { get; set; } = new int[16];
        /// <summary>
        /// DI 初始化
        /// </summary>
        public byte[] dizer { get; set; } = new byte[8];
        /// <summary>
        /// 電表資訊 初始化
        /// </summary>
        public float[] outbytezer { get; set; } = new float[14];
        private CRC16 crc16 = new CRC16();
        #region 上傳平台
        /// <summary>
        /// 上傳平台數值
        /// </summary>
        /// <param name="tcardno">卡號</param>
        /// <param name="tboardno">版號</param>
        /// <param name="ai1out">AI數值</param>
        /// <param name="diout">DI數值</param>
        /// <param name="outbyte">點表數值  
        /// <para>陣列[0]~[2] 電壓</para>
        /// <para>陣列[3]~[5] 電流</para>
        /// <para>陣列[6] 功率</para>
        /// <para>陣列[7] 總功率</para>
        /// <para>陣列[8] 功率因數</para>
        /// </param>
        /// <param name="serverip">上傳IP</param>
        /// <param name="serverport">上傳Port號</param>
        public void scan_adioco(string tcardno, string tboardno, int[] ai1out, byte[] diout,
                                float rv, float sv, float tv,
                                float ra, float sa, float ta,
                                float kw, float kwh, float pfe,
                                string serverip = "59.124.241.114", int serverport = 8888)
        {
            tcardno = ("000000" + tcardno).Substring(tcardno.Length, 6);
            DateTime datetime = DateTime.Now;
            try
            {
                byte[] sendstringbyet = new byte[109]; byte[] sendstringbyettmp = new byte[109];
                sendstringbyet[0] = 0x2;
                sendstringbyet[1] = 0x6A;        //0x3C
                sendstringbyet[2] = 0x0;
                sendstringbyet[3] = 0xA5;
                sendstringbyet[4] = 0x1F;
                crc16.stringtobyte(datetime.ToString("yyyy"), 2, out sendstringbyet[5]);    // 年
                crc16.stringtobyte(datetime.ToString("yyyy"), 1, out sendstringbyet[6]);    // 年
                sendstringbyet[7] = Convert.ToByte(Convert.ToInt16(datetime.ToString("MM")));                      // 月
                sendstringbyet[8] = Convert.ToByte(Convert.ToInt16(datetime.ToString("dd")));                      // 日
                sendstringbyet[9] = Convert.ToByte(Convert.ToInt16(datetime.ToString("HH")));                      // 分
                sendstringbyet[10] = Convert.ToByte(Convert.ToInt16(datetime.ToString("mm")));                      // 分
                sendstringbyet[11] = Convert.ToByte(Convert.ToInt16(datetime.ToString("ss")));                      // 秒
                Byte[] asci = new Byte[6];
                asci = Encoding.ASCII.GetBytes(tcardno);
                sendstringbyet[12] = asci[0];
                sendstringbyet[13] = asci[1];
                sendstringbyet[14] = asci[2];
                sendstringbyet[15] = asci[3];
                sendstringbyet[16] = asci[4];
                sendstringbyet[17] = asci[5];
                asci = Encoding.ASCII.GetBytes(tboardno);
                sendstringbyet[18] = asci[0];
                sendstringbyet[19] = asci[1];
                sendstringbyet[20] = 1;
                byte[] ai1byte = BitConverter.GetBytes(ai1out[0]);
                sendstringbyet[21] = ai1byte[1];
                sendstringbyet[22] = ai1byte[0];
                byte[] ai2byte = BitConverter.GetBytes(ai1out[1]);
                sendstringbyet[23] = ai2byte[1];
                sendstringbyet[24] = ai2byte[0];
                byte[] ai3byte = BitConverter.GetBytes(ai1out[2]);
                sendstringbyet[25] = ai3byte[1];
                sendstringbyet[26] = ai3byte[0];
                byte[] ai4byte = BitConverter.GetBytes(ai1out[3]);
                sendstringbyet[27] = ai4byte[1];
                sendstringbyet[28] = ai4byte[0];
                byte[] ai5byte = BitConverter.GetBytes(ai1out[4]);
                sendstringbyet[29] = ai5byte[1];
                sendstringbyet[30] = ai5byte[0];
                byte[] ai6byte = BitConverter.GetBytes(ai1out[5]);
                sendstringbyet[31] = ai6byte[1];
                sendstringbyet[32] = ai6byte[0];
                byte[] ai7byte = BitConverter.GetBytes(ai1out[6]);
                sendstringbyet[33] = ai7byte[1];
                sendstringbyet[34] = ai7byte[0];
                byte[] ai8byte = BitConverter.GetBytes(ai1out[7]);
                sendstringbyet[35] = ai8byte[1];
                sendstringbyet[36] = ai8byte[0];
                sendstringbyet[37] = diout[0];     // di;
                sendstringbyet[38] = dizer[0];     // do
                sendstringbyet[39] = 0;
                sendstringbyet[40] = 0;
                sendstringbyet[41] = 0;
                sendstringbyet[42] = 0;
                sendstringbyet[43] = 0;
                sendstringbyet[44] = 0;
                sendstringbyet[45] = 0;
                sendstringbyet[46] = 0;
                sendstringbyet[47] = 0;
                sendstringbyet[48] = 0;
                sendstringbyet[49] = 0;
                sendstringbyet[50] = 0;
                sendstringbyet[51] = 0;
                sendstringbyet[52] = 0;
                sendstringbyet[53] = 0;
                sendstringbyet[54] = 0;
                sendstringbyet[55] = 0;
                sendstringbyet[56] = 0;
                sendstringbyet[57] = 0;
                sendstringbyet[58] = 0;
                sendstringbyet[59] = 0xA1;
                sendstringbyet[60] = 0x01;
                Byte[] _temp1 = BitConverter.GetBytes(0);
                sendstringbyet[61] = _temp1[1];
                sendstringbyet[62] = _temp1[0];
                sendstringbyet[63] = _temp1[3];
                sendstringbyet[64] = _temp1[2];
                Byte[] _temp2 = BitConverter.GetBytes(0);
                sendstringbyet[65] = _temp2[1];
                sendstringbyet[66] = _temp2[0];
                sendstringbyet[67] = _temp2[3];
                sendstringbyet[68] = _temp2[2];
                byte[] tkwh = BitConverter.GetBytes(kwh);
                sendstringbyet[69] = tkwh[1];      //strtohex(tkwh.Substring(4, 2));電表
                sendstringbyet[70] = tkwh[0];      //strtohex(tkwh.Substring(6, 2));
                sendstringbyet[71] = tkwh[3];     //strtohex(tkwh.Substring(0, 2));
                sendstringbyet[72] = tkwh[2];      //strtohex(tkwh.Substring(2, 2));    
                byte[] tkw = BitConverter.GetBytes(kw);
                sendstringbyet[73] = tkw[1];     //strtohex(tkw.Substring(4, 2));
                sendstringbyet[74] = tkw[0];    //strtohex(tkw.Substring(6, 2));
                sendstringbyet[75] = tkw[3];    //strtohex(tkw.Substring(0, 2));
                sendstringbyet[76] = tkw[2];    //strtohex(tkw.Substring(2, 2));
                byte[] tav = BitConverter.GetBytes(rv);
                sendstringbyet[77] = tav[1];         //outbyte[2];    //strtohex(tav.Substring(4, 2));
                sendstringbyet[78] = tav[0];         //outbyte[3];    //strtohex(tav.Substring(6, 2));
                sendstringbyet[79] = tav[3];         //outbyte[0];    //strtohex(tav.Substring(0, 2));
                sendstringbyet[80] = tav[2];         //outbyte[1];    //strtohex(tav.Substring(2, 2));
                byte[] tbv = BitConverter.GetBytes(sv);
                sendstringbyet[81] = tbv[1];         //outbyte[7];    //strtohex(tbv.Substring(4, 2));
                sendstringbyet[82] = tbv[0];         //outbyte[6];    //strtohex(tbv.Substring(6, 2));
                sendstringbyet[83] = tbv[3];         //outbyte[4];    //strtohex(tbv.Substring(0, 2));
                sendstringbyet[84] = tbv[2];         //outbyte[5];    //strtohex(tbv.Substring(2, 2));
                byte[] tcv = BitConverter.GetBytes(tv);
                sendstringbyet[85] = tcv[1];        //outbyte[10];    //strtohex(tcv.Substring(4, 2));
                sendstringbyet[86] = tcv[0];           //outbyte[11];    //strtohex(tcv.Substring(6, 2));
                sendstringbyet[87] = tcv[3];            //outbyte[8];    //strtohex(tcv.Substring(0, 2));
                sendstringbyet[88] = tcv[2];            //outbyte[9];    //strtohex(tcv.Substring(2, 2));
                byte[] tai = BitConverter.GetBytes(ra);
                sendstringbyet[89] = tai[1];    //strtohex(tai.Substring(4, 2));
                sendstringbyet[90] = tai[0];    //strtohex(tai.Substring(6, 2));
                sendstringbyet[91] = tai[3];    //strtohex(tai.Substring(0, 2));
                sendstringbyet[92] = tai[2];    //strtohex(tai.Substring(2, 2));
                byte[] tbi = BitConverter.GetBytes(sa);
                sendstringbyet[93] = tbi[1];    //strtohex(tbi.Substring(4, 2));
                sendstringbyet[94] = tbi[0];      //strtohex(tbi.Substring(6, 2));
                sendstringbyet[95] = tbi[3];     //strtohex(tbi.Substring(0, 2));
                sendstringbyet[96] = tbi[2];    //strtohex(tbi.Substring(2, 2));
                byte[] tci = BitConverter.GetBytes(ta);
                sendstringbyet[97] = tci[1];    //strtohex(tci.Substring(4, 2));
                sendstringbyet[98] = tci[0];      //strtohex(tci.Substring(6, 2));
                sendstringbyet[99] = tci[3];     //strtohex(tci.Substring(0, 2));
                sendstringbyet[100] = tci[2];      // strtohex(tci.Substring(2, 2));
                byte[] tpfe = BitConverter.GetBytes(pfe);
                sendstringbyet[101] = tpfe[1];    // strtohex(tpre.Substring(4, 2));
                sendstringbyet[102] = tpfe[0];    // strtohex(tpre.Substring(6, 2));
                sendstringbyet[103] = tpfe[3];    // strtohex(tpre.Substring(0, 2));
                sendstringbyet[104] = tpfe[2];    // strtohex(tpre.Substring(2, 2));
                sendstringbyet[105] = 0x00;
                sendstringbyet[106] = 0x03;
                Byte[] aa = new Byte[2];
                aa = crc16.MakeCRC16(sendstringbyet, 1, 106);
                sendstringbyet[107] = aa[1];
                sendstringbyet[108] = aa[0];
                myUdpClient.Send(sendstringbyet, sendstringbyet.Length, new IPEndPoint(IPAddress.Parse(serverip), serverport));
                Thread.Sleep(500);
            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
        }
        #endregion
    }
}
public class CRC16
{
    public UdpClient myUdpClient = new UdpClient();
    byte[] dizer = new byte[2];
    public void stringtobyte(String value1, int kind, out byte ou)
    {
        ou = 0;
        int value = Convert.ToInt32(value1);
        if (value < 0)
        {
            value += 65536;
        }
        if (kind == 1)
        {
            //hibyte
            ou = Convert.ToByte(((value / 256) % 256));
        }
        else
        {
            if (kind == 2)
            {
                //lowbyte
                ou = Convert.ToByte((value % 256));
            }
            else
            {
                if (kind == 3)
                {
                    ou = Convert.ToByte(((((value / 256) / 256) / 256) % 256));
                }
                else
                {
                    if (kind == 4)
                    {
                        ou = Convert.ToByte((((value / 256) / 256) % 256));
                    }
                }
            }
        }
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
    public float ex(Byte in1, Byte in2)
    {
        float outv;
        outv = in1 * 256 + in2;
        return outv;
    }
    public Single work754to10(byte val1, byte val2, byte val3, byte val4)
    {
        Single ans = 0;
        Byte[] _temp = new byte[4];
        _temp[0] = val1;
        _temp[1] = val2;
        _temp[2] = val3;
        _temp[3] = val4;
        ans = BitConverter.ToSingle(_temp, 0);
        return ans;
    }

}