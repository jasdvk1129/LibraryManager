using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLibrary;

namespace DeviceLibrary
{
    public class AEMDRAClass
    {
        /* 銓盛電表 AEMDRA */
        public byte ID { get; set; }                            //設備ID
        public byte MA_StartAddrHigh { get; private set; } = 0x10;      //
        public byte MA_StartAddrLow { get; private set; } = 0x01;       //
        public byte MA_LengthHigh { get; private set; } = 0x00;         //
        public byte MA_LengthLow { get; private set; } = 0x37;          //
        public float MA_V1 { get; private set; }
        public float MA_V2 { get; private set; }
        public float MA_V3 { get; private set; }
        public float MA_V { get; private set; }
        public float MA_U12 { get; private set; }
        public float MA_U23 { get; private set; }
        public float MA_U31 { get; private set; }
        public float MA_U { get; private set; }
        public float MA_I1 { get; private set; }
        public float MA_I2 { get; private set; }
        public float MA_I3 { get; private set; }
        public float MA_I { get; private set; }
        public float MA_IN { get; private set; }
        public float MA_P1 { get; private set; }
        public float MA_P2 { get; private set; }
        public float MA_P3 { get; private set; }
        public float MA_P { get; private set; }
        public float MA_Q1 { get; private set; }
        public float MA_Q2 { get; private set; }
        public float MA_Q3 { get; private set; }
        public float MA_Q { get; private set; }
        public float MA_S1 { get; private set; }
        public float MA_S2 { get; private set; }
        public float MA_S3 { get; private set; }
        public float MA_S { get; private set; }
        public float MA_PF1 { get; private set; }
        public float MA_PF2 { get; private set; }
        public float MA_PF3 { get; private set; }
        public float MA_PF { get; private set; }
        public byte[] BA_StartAddrHigh { get; private set; } = { 0x14, 0x17, 0x1A, 0x1D };
        public byte[] BA_StartAddrLow { get; private set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] BA_LengthHigh { get; private set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] BA_LengthLow { get; private set; } = { 0x23, 0x23, 0x23, 0x23 };
        public float[] BA_I { get; private set; } = new float[12];
        public float[] BA_P { get; private set; } = new float[12];
        public float[] BA_Q { get; private set; } = new float[12];
        public float[] BA_S { get; private set; } = new float[12];
        public float[] BA_PF { get; private set; } = new float[12];
        public byte MB_StartAddrHigh { get; private set; } = 0x20;      //
        public byte MB_StartAddrLow { get; private set; } = 0x01;       //
        public byte MB_LengthHigh { get; private set; } = 0x00;         //
        public byte MB_LengthLow { get; private set; } = 0x37;          //
        public float MB_V1 { get; private set; }
        public float MB_V2 { get; private set; }
        public float MB_V3 { get; private set; }
        public float MB_V { get; private set; }
        public float MB_U12 { get; private set; }
        public float MB_U23 { get; private set; }
        public float MB_U31 { get; private set; }
        public float MB_U { get; private set; }
        public float MB_I1 { get; private set; }
        public float MB_I2 { get; private set; }
        public float MB_I3 { get; private set; }
        public float MB_I { get; private set; }
        public float MB_IN { get; private set; }
        public float MB_P1 { get; private set; }
        public float MB_P2 { get; private set; }
        public float MB_P3 { get; private set; }
        public float MB_P { get; private set; }
        public float MB_Q1 { get; private set; }
        public float MB_Q2 { get; private set; }
        public float MB_Q3 { get; private set; }
        public float MB_Q { get; private set; }
        public float MB_S1 { get; private set; }
        public float MB_S2 { get; private set; }
        public float MB_S3 { get; private set; }
        public float MB_S { get; private set; }
        public float MB_PF1 { get; private set; }
        public float MB_PF2 { get; private set; }
        public float MB_PF3 { get; private set; }
        public float MB_PF { get; private set; }
        public byte[] BB_StartAddrHigh { get; private set; } = { 0x24, 0x27, 0x2A, 0x2D };
        public byte[] BB_StartAddrLow { get; private set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] BB_LengthHigh { get; private set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] BB_LengthLow { get; private set; } = { 0x23, 0x23, 0x23, 0x23 };
        public float[] BB_I { get; private set; } = new float[12];
        public float[] BB_P { get; private set; } = new float[12];
        public float[] BB_Q { get; private set; } = new float[12];
        public float[] BB_S { get; private set; } = new float[12];
        public float[] BB_PF { get; private set; } = new float[12];
        /// <summary>
        /// 解析MA的數值
        /// </summary>
        /// <param name="Inbyte">資料匯入</param>
        /// <returns>回傳解析是否成功</returns>
        public bool AnalysisDataByte_MA(byte[] Inbyte)
        {
            try
            {
                MathClass Calculate = new MathClass();
                int k = 3;
                MA_V1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_V2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_V3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_V = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_U12 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_U23 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_U31 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_U = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; k += 4;
                MA_I1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                MA_I2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                MA_I3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                MA_I = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                MA_IN = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_P1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_P2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_P3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_P = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_Q1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_Q2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_Q3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_Q = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_S1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_S2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_S3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_S = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MA_PF1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MA_PF2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MA_PF3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MA_PF = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
            }
            catch (Exception)
            { return false; }
            return true;
        }
        /// <summary>
        /// 解析MB的數值
        /// </summary>
        /// <param name="Inbyte">資料匯入</param>
        /// <returns>回傳解析是否成功</returns>
        public bool AnalysisDataByte_MB(byte[] Inbyte)
        {
            try
            {
                MathClass Calculate = new MathClass();
                int k = 3;
                MB_V1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_V2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_V3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_V = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_U12 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_U23 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_U31 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_U = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.1F; ; k += 4;
                MB_I1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; ; k += 4;
                MB_I2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; ; k += 4;
                MB_I3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; ; k += 4;
                MB_I = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                MB_IN = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_P1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_P2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_P3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_P = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_Q1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_Q2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_Q3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_Q = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_S1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_S2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_S3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_S = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                MB_PF1 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MB_PF2 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MB_PF3 = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                MB_PF = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
            }
            catch (Exception)
            { return false; }
            return true;
        }
        /// <summary>
        /// 解析BA數值
        /// </summary>
        /// <param name="Inbyte">資料匯入</param>
        /// <param name="DataIndex">Channel起始編號(請輸入0,3,6,9)</param>
        /// <returns>回傳解析是否成功</returns>
        public bool AnalysisDataByte_BA(byte[] Inbyte, int DataIndex)
        {
            try
            {
                MathClass Calculate = new MathClass();
                int k = 3; float Space;
                BA_I[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BA_I[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BA_I[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BA_P[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_P[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_P[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_Q[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_Q[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_Q[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_S[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_S[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_S[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BA_PF[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                BA_PF[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                BA_PF[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
            }
            catch (Exception)
            { return false; }
            return true;
        }
        /// <summary>
        /// 解析BB數值
        /// </summary>
        /// <param name="Inbyte">資料匯入</param>
        /// <param name="DataIndex">Channel起始編號(請輸入0,3,6,9)</param>
        /// <returns>回傳解析是否成功</returns>
        public bool AnalysisDataByte_BB(byte[] Inbyte, int DataIndex)
        {
            try
            {
                MathClass Calculate = new MathClass();
                int k = 3; float Space;
                BB_I[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BB_I[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BB_I[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true) * 0.001F; k += 4;
                BB_P[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_P[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_P[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_Q[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_Q[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_Q[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_S[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_S[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_S[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                Space = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3], true); k += 4;
                BB_PF[DataIndex] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                BB_PF[DataIndex + 1] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                BB_PF[DataIndex + 2] = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
            }
            catch (Exception)
            { return false; }
            return true;
        }
    }
}
