using Microsoft.Win32.SafeHandles;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SMSLibrary
{
    public class SMSClass :IDisposable
    {
        /// <summary>
        /// 通訊阜
        /// </summary>
        private SerialPort Rs232 { get; set; }
        /// <summary>
        /// 簡訊機初始物件
        /// </summary>
        /// <param name="serialPort">通訊阜</param>
        /// <param name="PortName">Port號</param>
        public SMSClass(SerialPort serialPort ,string PortName)
        {
            Rs232 = serialPort;
            if (!Rs232.IsOpen)
            {
                try
                {
                    Rs232.PortName = PortName;
                    Rs232.BaudRate = 115200;
                    Rs232.DataBits = 8;
                    Rs232.Parity = Parity.None;
                    Rs232.StopBits = StopBits.One;
                    Rs232.Open();  //開啟通訊埠
                }
                catch (Exception ex)
                {
                    Log.Error(ex,"通訊阜錯誤");
                }
            }
        }

        #region 簡訊發送
        /// <summary>
        /// 簡訊發送
        /// </summary>
        /// <param name="TelNo">電話號碼</param>
        /// <param name="Message">訊息</param>
        public void SMS_Send(string TelNo, string Message)
        {
            if (Rs232.IsOpen)
            {
                string _TelNo = TelNo.Trim();
                string _Message = Message.Trim();
                string Buff_Message = Message.Trim();
                List<string> Send_Messgae = new List<string>(); ;
                if (!_TelNo.All(char.IsDigit))
                {
                    Log.Error("電話號碼中有非數字");
                }
                else
                {
                    if (_TelNo.Length != 12 & _TelNo.Length == 10)
                    {
                        _TelNo = "886" + $"{_TelNo.Substring(1, 9)}";
                    }
                }
                while (Buff_Message.Length > 0)
                {
                    if (Buff_Message.Length > 70)
                    {
                        Send_Messgae.Add(Buff_Message.Substring(0, 70));
                        Buff_Message = Buff_Message.Remove(0, 70);
                    }
                    else
                    {
                        Send_Messgae.Add(Buff_Message.Substring(0, Buff_Message.Length));
                        Buff_Message = Buff_Message.Remove(0, Buff_Message.Length);
                    }
                }
                foreach (var Messageitem in Send_Messgae)
                {
                    byte[] cmd1 = Encoding.ASCII.GetBytes("AT+CSQ" + "\r");
                    byte[] cmd2 = Encoding.ASCII.GetBytes("AT+CMGF=0" + "\r");
                    byte[] cmd3 = Encoding.ASCII.GetBytes("AT+CMGS=" + $"{Messageitem.Length * 2 + 14}" + "\r");
                    byte[] cmd4 = new byte[30 + Messageitem.Length * 4 + 1];
                    string TitalStr = "0011000C91";
                    for (int i = 0; i < TitalStr.Length; i++)//長度10
                    {
                        cmd4[i] = Encoding.ASCII.GetBytes(TitalStr.Substring(i, 1))[0];
                    }
                    for (int i = 0; i < _TelNo.Length; i += 2)//解析電話號碼 長度12
                    {
                        cmd4[i + 10] = Encoding.ASCII.GetBytes(_TelNo.Substring(i + 1, 1))[0];
                        cmd4[i + 11] = Encoding.ASCII.GetBytes(_TelNo.Substring(i, 1))[0];
                    }
                    string MidStr = "0008";
                    for (int i = 0; i < MidStr.Length; i++)//長度4
                    {
                        cmd4[i + 22] = Encoding.ASCII.GetBytes(MidStr.Substring(i, 1))[0];
                    }
                    string EndStr = (Messageitem.Length * 2).ToString("X4");
                    for (int i = 0; i < EndStr.Length; i++)//長度4
                    {
                        cmd4[i + 26] = Encoding.ASCII.GetBytes(EndStr.Substring(i, 1))[0];
                    }
                    char[] Messagedata = Messageitem.ToCharArray();
                    for (int i = 0; i < Messageitem.Length; i++)
                    {
                        string Temp = Convert.ToInt32(Messagedata[i]).ToString("X4");
                        cmd4[29 + i * 4 + 1] = Encoding.ASCII.GetBytes(Temp.Substring(0, 1))[0];
                        cmd4[29 + i * 4 + 2] = Encoding.ASCII.GetBytes(Temp.Substring(1, 1))[0];
                        cmd4[29 + i * 4 + 3] = Encoding.ASCII.GetBytes(Temp.Substring(2, 1))[0];
                        cmd4[29 + i * 4 + 4] = Encoding.ASCII.GetBytes(Temp.Substring(3, 1))[0];
                    }
                    cmd4[30 + Messageitem.Length * 4] = 26;

                    byte[] InByte0 = new byte[Rs232.BytesToRead];
                    int ReadCount0 = Rs232.Read(InByte0, 0, Rs232.BytesToRead);

                    Rs232.Write(cmd1, 0, cmd1.Length);
                    Thread.Sleep(500);
                    byte[] InByte1 = new byte[Rs232.BytesToRead];
                    int ReadCount1 = Rs232.Read(InByte1, 0, Rs232.BytesToRead);
                    if (ReadCount1 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte1, 0, InByte1.Length);
                        if (_TempStr.IndexOf("OK") < -1)
                        {
                            Log.Error($"Error,{_TempStr}");
                        }
                        else
                        {
                            //Log.Error($"{_TempStr}");
                        }
                    }

                    Rs232.Write(cmd2, 0, cmd2.Length);
                    Thread.Sleep(200);
                    byte[] InByte2 = new byte[Rs232.BytesToRead];
                    int ReadCount2 = Rs232.Read(InByte2, 0, Rs232.BytesToRead);
                    if (ReadCount2 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte2, 0, InByte2.Length);
                        if (_TempStr.IndexOf("OK") < -1)
                        {
                            Log.Error($"Error,{_TempStr}");
                        }
                        else
                        {
                            //Log.Error($"{_TempStr}");
                        }
                    }

                    Rs232.Write(cmd3, 0, cmd3.Length);
                    Thread.Sleep(50);
                    Rs232.Write(cmd4, 0, cmd4.Length);
                    Thread.Sleep(2000);
                    byte[] InByte3 = new byte[Rs232.BytesToRead];
                    int ReadCount3 = Rs232.Read(InByte3, 0, Rs232.BytesToRead);
                    if (ReadCount3 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte3, 0, InByte3.Length);
                        if (_TempStr.IndexOf("ERROR") > 0)
                        {
                            Log.Error($"Error,{_TempStr}");
                        }
                        else
                        {
                            //Log.Error($"OK,{_TempStr}");
                        }
                    }
                }
            }
        }
        #endregion

        #region 群睿科技_簡訊發送
        public void Gainwise_SMS_Send(string TelNo, string Message)
        {
            if (Rs232.IsOpen)
            {
                string _TelNo = TelNo.Trim();
                string _Message = Message.Trim();
                string Buff_Message = Message.Trim();
                string SmsCenterNo = "";        // 簡訊中心號碼
                List<string> Send_Messgae = new List<string>(); ;
                if (!_TelNo.All(char.IsDigit))
                {
                    Log.Error("電話號碼中有非數字");
                    return;
                }
                else
                {
                    if (_TelNo.Length != 12 & _TelNo.Length == 10)
                    {
                        _TelNo = "886" + $"{_TelNo.Substring(1, 9)}";
                    }
                }
                while (Buff_Message.Length > 0)
                {
                    if (Buff_Message.Length > 70)
                    {
                        Send_Messgae.Add(Buff_Message.Substring(0, 70));
                        Buff_Message = Buff_Message.Remove(0, 70);
                    }
                    else
                    {
                        Send_Messgae.Add(Buff_Message.Substring(0, Buff_Message.Length));
                        Buff_Message = Buff_Message.Remove(0, Buff_Message.Length);
                    }
                }
                foreach (var Messageitem in Send_Messgae)
                {
                    byte[] InByte0 = new byte[Rs232.BytesToRead];
                    int ReadCount0 = Rs232.Read(InByte0, 0, Rs232.BytesToRead);

                    byte[] cmd1 = Encoding.ASCII.GetBytes("AT+CSCA?" + "\r");   // 第一道命令，抓簡訊中心號碼
                    Rs232.Write(cmd1, 0, cmd1.Length);
                    Thread.Sleep(500);
                    byte[] InByte1 = new byte[Rs232.BytesToRead];
                    int ReadCount1 = Rs232.Read(InByte1, 0, Rs232.BytesToRead);
                    if (ReadCount1 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                        return;
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte1, 0, InByte1.Length);
                        if (_TempStr.IndexOf("OK") < -1)
                        {
                            Log.Error($"Error,{_TempStr}");
                            return;
                        }
                        else
                        {
                            int Index = _TempStr.IndexOf("+CSCA: \"+");
                            if (Index < -1)
                            {
                                Log.Error($"Error,找不到 簡訊中心號碼({_TempStr})");
                                return;
                            }
                            else
                            {
                                SmsCenterNo = _TempStr.Substring(Index + 10 - 1, 12);
                                if (!SmsCenterNo.All(char.IsDigit))
                                {
                                    Log.Error($"Error,簡訊中心號碼異常({_TempStr})");
                                    return;
                                }
                            }
                        }
                    }

                    byte[] cmd2 = Encoding.ASCII.GetBytes("AT+CMGF=0" + "\r");
                    byte[] cmd3 = Encoding.ASCII.GetBytes("AT+CMGS=" + $"{Messageitem.Length * 2 + 14}" + "\r");
                    byte[] cmd4 = new byte[46 + Messageitem.Length * 4];
                    // 079188961300009911000C91
                    // 0123456789 123456789 123
                    // 0791 好像是固定
                    // 889613000099 是 簡訊中心號碼 +886931000099(遠傳) (查詢命令 AT+CSCA? )
                    // 中華電信的簡訊中心電話號碼有3個(886932400821,886932400841,886932400851)
                    // 11000C91 好像是固定
                    string T1Str = "0791";      // 好像固定
                    for (int i = 0; i < T1Str.Length; i++)//長度4
                    {
                        cmd4[i] = Encoding.ASCII.GetBytes(T1Str.Substring(i, 1))[0];
                    }
                    for (int i = 0; i < SmsCenterNo.Length; i += 2)//解析簡訊中心電話號碼 長度12
                    {
                        cmd4[i + 4] = Encoding.ASCII.GetBytes(SmsCenterNo.Substring(i + 1, 1))[0];
                        cmd4[i + 5] = Encoding.ASCII.GetBytes(SmsCenterNo.Substring(i, 1))[0];
                    }
                    string T2Str = "11000C91";      // 好像固定
                    //              12345678
                    for (int i = 0; i < T2Str.Length; i++)//長度8
                    {
                        cmd4[i + 16] = Encoding.ASCII.GetBytes(T2Str.Substring(i, 1))[0];
                    }
                    for (int i = 0; i < _TelNo.Length; i += 2)//解析電話號碼 長度12
                    {
                        cmd4[i + 24] = Encoding.ASCII.GetBytes(_TelNo.Substring(i + 1, 1))[0];
                        cmd4[i + 25] = Encoding.ASCII.GetBytes(_TelNo.Substring(i, 1))[0];
                    }
                    string T3Str = "0008AA";      // 好像固定
                    //              123456
                    for (int i = 0; i < T3Str.Length; i++)//長度6
                    {
                        cmd4[i + 36] = Encoding.ASCII.GetBytes(T3Str.Substring(i, 1))[0];
                    }
                    string EndStr = (Messageitem.Length * 2).ToString("X2");            // 訊息內容長度
                    for (int i = 0; i < EndStr.Length; i++)//長度2
                    {
                        cmd4[i + 42] = Encoding.ASCII.GetBytes(EndStr.Substring(i, 1))[0];
                    }
                    char[] Messagedata = Messageitem.ToCharArray();
                    for (int i = 0; i < Messageitem.Length; i++)
                    {
                        string Temp = Convert.ToInt32(Messagedata[i]).ToString("X4");
                        cmd4[43 + i * 4 + 1] = Encoding.ASCII.GetBytes(Temp.Substring(0, 1))[0];
                        cmd4[43 + i * 4 + 2] = Encoding.ASCII.GetBytes(Temp.Substring(1, 1))[0];
                        cmd4[43 + i * 4 + 3] = Encoding.ASCII.GetBytes(Temp.Substring(2, 1))[0];
                        cmd4[43 + i * 4 + 4] = Encoding.ASCII.GetBytes(Temp.Substring(3, 1))[0];
                    }
                    cmd4[44 + Messageitem.Length * 4] = 26;

                    Rs232.Write(cmd2, 0, cmd2.Length);
                    Thread.Sleep(200);
                    byte[] InByte2 = new byte[Rs232.BytesToRead];
                    int ReadCount2 = Rs232.Read(InByte2, 0, Rs232.BytesToRead);
                    if (ReadCount2 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte2, 0, InByte2.Length);
                        if (_TempStr.IndexOf("OK") < -1)
                        {
                            Log.Error($"Error,{_TempStr}");
                        }
                        else
                        {
                            //Log.Error($"{_TempStr}");
                        }
                    }

                    Rs232.Write(cmd3, 0, cmd3.Length);
                    Thread.Sleep(50);
                    Rs232.Write(cmd4, 0, cmd4.Length);
                    Thread.Sleep(2000);
                    byte[] InByte3 = new byte[Rs232.BytesToRead];
                    int ReadCount3 = Rs232.Read(InByte3, 0, Rs232.BytesToRead);
                    if (ReadCount3 == 0)
                    {
                        Log.Error("Error,簡訊機無回應");
                    }
                    else
                    {
                        string _TempStr = Encoding.ASCII.GetString(InByte3, 0, InByte3.Length);
                        if (_TempStr.IndexOf("ERROR") > 0)
                        {
                            Log.Error($"Error,{_TempStr}");
                        }
                        else
                        {
                            //Log.Error($"OK,{_TempStr}");
                        }
                    }
                }
            }
        }
        #endregion

        #region 釋放
        private bool _disposed = false;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _safeHandle?.Dispose();
            }
            _disposed = true;
        }
        #endregion
    }
}
