using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModbusLibrary;
using MathLibrary;
using System.Data;
using System.Data.SqlClient;
using DatabaseLibrary;

namespace DeviceLibrary.Protocols.ElectricProtocols.ModbusRTU
{
    public class SPM8Protocol : AbsElectricProtocol
    {
        private MathClass Calculate = new MathClass();
        const short DataCmd = 4107;
        const short DataLength = 98;
        /// <summary>
        /// 設備站號(1~255)
        /// </summary>
        public byte ID { get; set; }

        public override bool AnalysisDataFunction(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Inbyte) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                var k = 3;
                RV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                SV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                TV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float VN_AVG = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                RSV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                STV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                TRV = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float V_AVG = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                RA = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                SA = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                TA = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float I_AVG = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float I_N = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                HZ = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KW_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KW_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KW_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                KW = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVAR_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVAR_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVAR_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                KVAR = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVA_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVA_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVA_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                KVA = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float PFE_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float PFE_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float PFE_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                PFE = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float V_ANGLE_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float V_ANGLE_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float V_ANGLE_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float I_ANGLE_A = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float I_ANGLE_B = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float I_ANGLE_C = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float NO_1 = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float NO_2 = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float NO_3 = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KWH_DEL = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KWH_REC = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                KWH = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KWH_NET = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVARH_DEL = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVARH_REC = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                KVARH = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVARH_NET = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]); k += 4;
                float KVARH_TOT = Calculate.work16to754(Inbyte[k], Inbyte[k + 1], Inbyte[k + 2], Inbyte[k + 3]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通訊命令建立
        /// </summary>
        /// <returns></returns>
        public override byte[] CommandCreater()
        {
            byte[] Cmd = new byte[8];
            Cmd[0] = ID;
            Cmd[1] = 0x04;
            byte[] Address = BitConverter.GetBytes(DataCmd);
            Cmd[2] = Address[1];
            Cmd[3] = Address[0];
            byte[] Length = BitConverter.GetBytes(DataLength);
            Cmd[4] = Length[1];
            Cmd[5] = Length[0];
            byte[] crc16 = MakeCRC16(Cmd, 0, 6);
            Cmd[6] = crc16[1];
            Cmd[7] = crc16[0];
            return Cmd;
        }
        /// <summary>
        /// 歷史資料寫入
        /// </summary>
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
