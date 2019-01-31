using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLibrary;
using DatabaseLibrary;

namespace DeviceLibrary.Protocols.ElectricProtocols.ModbusRTU
{
    public class REF615CProtocol : AbsElectricProtocol
    {
        private MathClass Calculate = new MathClass();
        /// <summary>
        /// 設備站號(1~255)
        /// </summary>
        public byte ID { get; set; }            
        /// <summary>
        /// 資料解析函式(KW,KVAR)
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public override bool AnalysisDataFunction(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Cmd) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                int k = 3;
                KW = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]); k += 2;
                KVAR = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 資料解析函式(電壓)
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public bool AnalysisDataFunction_Valtage(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Cmd) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                int k = 3;
                RV = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                SV = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                TV = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 資料解析函式(電流)
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public bool AnalysisDataFunction_Current(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Cmd) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                int k = 3;
                RA = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                SA = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F; k += 2;
                TA = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.001F;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 資料解析函式(PF,HZ)
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public bool AnalysisDataFunction_PfHz(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Cmd) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                int k = 3;
                PFE = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]); k += 2;
                HZ = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 讀取命令建立函式(KW,KVAR)
        /// </summary>
        /// <returns></returns>
        public override byte[] CommandCreater()
        {
            byte[] Cmd = new byte[7];
            Cmd[0] = ID;
            Cmd[1] = 0x04;
            byte[] Address = BitConverter.GetBytes((short)160);
            Cmd[2] = Address[1];
            Cmd[3] = Address[0];
            byte[] Length = BitConverter.GetBytes((short)4);
            Cmd[4] = Length[1];
            Cmd[5] = Length[0];
            byte[] crc16 = MakeCRC16(Cmd, 0, 6);
            Cmd[6] = crc16[1];
            Cmd[7] = crc16[0];
            return Cmd;
        }
        /// <summary>
        /// 讀取命令建立函式(PF,Hz)
        /// </summary>
        /// <returns></returns>
        public byte[] CommandCreater_PfHz()
        {
            byte[] Cmd = new byte[7];
            Cmd[0] = ID;
            Cmd[1] = 0x04;
            byte[] Address = BitConverter.GetBytes((short)166);
            Cmd[2] = Address[1];
            Cmd[3] = Address[0];
            byte[] Length = BitConverter.GetBytes((short)2);
            Cmd[4] = Length[1];
            Cmd[5] = Length[0];
            byte[] crc16 = MakeCRC16(Cmd, 0, 6);
            Cmd[6] = crc16[1];
            Cmd[7] = crc16[0];
            return Cmd;
        }
        /// <summary>
        /// 讀取命令建立函式(電壓)
        /// </summary>
        /// <returns></returns>
        public byte[] CommandCreater_Valtage()
        {
            byte[] Cmd = new byte[7];
            Cmd[0] = ID;
            Cmd[1] = 0x04;
            byte[] Address = BitConverter.GetBytes((short)154);
            Cmd[2] = Address[1];
            Cmd[3] = Address[0];
            byte[] Length = BitConverter.GetBytes((short)3);
            Cmd[4] = Length[1];
            Cmd[5] = Length[0];
            byte[] crc16 = MakeCRC16(Cmd, 0, 6);
            Cmd[6] = crc16[1];
            Cmd[7] = crc16[0];
            return Cmd;
        }
        /// <summary>
        /// 讀取命令建立函式(電流)
        /// </summary>
        /// <returns></returns>
        public byte[] CommandCreater_Current()
        {
            byte[] Cmd = new byte[7];
            Cmd[0] = ID;
            Cmd[1] = 0x04;
            byte[] Address = BitConverter.GetBytes((short)137);
            Cmd[2] = Address[1];
            Cmd[3] = Address[0];
            byte[] Length = BitConverter.GetBytes((short)3);
            Cmd[4] = Length[1];
            Cmd[5] = Length[0];
            byte[] crc16 = MakeCRC16(Cmd, 0, 6);
            Cmd[6] = crc16[1];
            Cmd[7] = crc16[0];
            return Cmd;
        }

        public override void HistoryLogger()
        {
            string ConnStr = $"server = {SQLServerIp}; database={SQLDatabase}; uid={SQLUserId}; pwd={SQLUserPwd};";          //資料庫連接字串
            string ConnStrLog = $"server = {SQLServerIp}; database={SQLDatabase}_{DateTime.Now.Year.ToString()}; uid={SQLUserId}; pwd={SQLUserPwd};";          //資料庫連接字串
            DatabaseClass database = new DatabaseClass
            {
                WorkPath = AppDomain.CurrentDomain.BaseDirectory,
                mSQLDatabase = SQLDatabase,
                ConnStr = ConnStr
            };
            if (database.CheckDatabaseExistFunction(DateTime.Now))
            {
                if (database.CheckDataTableExistFunction($"Elect_{ID}", ConnStrLog))
                {
                    if (!database.CheckDataLogExistFunction($"Elect_{ID}", ConnStrLog))
                    {
                        database.AddNewDataLogFunction($"Elect_{ID}", "ttime,ttimen,trv,tsv,ttv,trsv,tstv,ttrv,tri,tsi,tti,tkw,tkwh,tkvar,tkvarh,tkva,tkvah,tpfe", $"'{DateTime.Now.ToString("yyyyMMddHHmm")}00','{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}:00',{RV},{SV},{TV},{RSV},{STV},{TRV},{RA},{SA},{TA},{KW},{KWH},{KVAR},{KVARH},{KVA},{KVAH},{PFE}", ConnStrLog);
                    }
                }
                else
                {
                    database.CreateDataTableFunction($"Elect_{ID}",
                                                      "ttime NCHAR(14) DEFAULT '',ttimen DATETIME," +
                                                      "trv FLOAT DEFAULT 0,tsv FLOAT DEFAULT 0,ttv FLOAT DEFAULT 0," +
                                                      "trsv FLOAT DEFAULT 0,tstv FLOAT DEFAULT 0,ttrv FLOAT DEFAULT 0," +
                                                      "tri FLOAT DEFAULT 0,tsi FLOAT DEFAULT 0,tti FLOAT DEFAULT 0," +
                                                      "tkw FLOAT DEFAULT 0,tkwh FLOAT DEFAULT 0,tkvar FLOAT DEFAULT 0,tkvarh FLOAT DEFAULT 0," +
                                                      "tkva FLOAT DEFAULT 0,tkvah FLOAT DEFAULT 0,tpfe FLOAT DEFAULT 0 PRIMARY KEY(ttime)", ConnStrLog);
                }
            }
        }
    }
}
