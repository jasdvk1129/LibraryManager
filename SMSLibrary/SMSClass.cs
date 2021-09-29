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
