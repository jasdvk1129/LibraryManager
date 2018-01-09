using System;
using MathLibrary;

namespace ModbusReceiveLibrary
{
    public class ModbusReceiveClass
    {
        public int[,] Register;              //資料記憶體陣列
        public int MaxId;

        public byte[] ModbusTCPReceive(byte[] Cmd)
        {
            Array mArray = Register;
            MathClass Calculate = new MathClass();
            byte[] Receive;
            int mCmdAddress = Calculate.work16to10(Cmd[8], Cmd[9], false);              //資料記憶體的位置
            if (Cmd[7] == 0x03)
            {
                if (mCmdAddress <= mArray.GetUpperBound(1) && mCmdAddress >= 0)         //取用位置不可大於記憶體最大值
                {
                    int mCmdLength = Calculate.work16to10(Cmd[10], Cmd[11], false);     //要求的資料長度
                    if ((mCmdLength + mCmdAddress) <= (Register.GetUpperBound(1) + 1))                  //檢查要求的資料數量不可以超過總記憶體的長度
                    {
                        int mReceiveLength = (mCmdLength * 2) + 9;                      //計算回應資料長度(要求*2+6)
                        int mModbusLength = (mCmdLength * 2) + 3;
                        int mDataLength = mCmdLength * 2;
                        Receive = new byte[mReceiveLength];
                        Receive[0] = Cmd[0];
                        Receive[1] = Cmd[1];
                        Receive[2] = Cmd[2];
                        Receive[3] = Cmd[3];
                        Receive[4] = Cmd[4];
                        Receive[5] = Convert.ToByte(mModbusLength);
                        Receive[6] = Cmd[6];
                        Receive[7] = Cmd[7];
                        Receive[8] = Convert.ToByte(mDataLength);
                        for (int i = 0; i < mCmdLength; i++)
                        {
                            ushort SourceData = Convert.ToUInt16(Register[(Cmd[6] - 1), (i + mCmdAddress)]);
                            byte[] Data = BitConverter.GetBytes(SourceData);
                            Receive[9 + (i * 2)] = Data[1];
                            Receive[10 + (i * 2)] = Data[0];
                        }
                    }
                    else
                    {
                        Receive = new byte[9];
                        Receive[0] = Cmd[0];
                        Receive[1] = Cmd[1];
                        Receive[2] = Cmd[2];
                        Receive[3] = Cmd[3];
                        Receive[4] = Cmd[4];
                        Receive[5] = 0x03;
                        Receive[6] = Cmd[6];
                        Receive[7] = Convert.ToByte((Cmd[7] | 0x80));
                        Receive[8] = 0x02;
                    }
                }
                else
                {
                    Receive = new byte[9];
                    Receive[0] = Cmd[0];
                    Receive[1] = Cmd[1];
                    Receive[2] = Cmd[2];
                    Receive[3] = Cmd[3];
                    Receive[4] = Cmd[4];
                    Receive[5] = 0x03;
                    Receive[6] = Cmd[6];
                    Receive[7] = Convert.ToByte((Cmd[7] | 0x80));
                    Receive[8] = 0x02;
                }
            }
            else
            {
                Receive = new byte[9];
                Receive[0] = Cmd[0];
                Receive[1] = Cmd[1];
                Receive[2] = Cmd[2];
                Receive[3] = Cmd[3];
                Receive[4] = Cmd[4];
                Receive[5] = 0x03;
                Receive[6] = Cmd[6];
                Receive[7] = Convert.ToByte((Cmd[7] | 0x80));
                Receive[8] = 0x01;
            }
            return Receive;
        }
    }
}
