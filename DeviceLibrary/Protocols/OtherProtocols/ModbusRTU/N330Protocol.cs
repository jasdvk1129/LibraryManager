using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLibrary;
using DatabaseLibrary;

namespace DeviceLibrary.Protocols.OtherProtocols.ModbusRTU
{
    public class N330Protocol : AbsOtherProtocol
    {
        private MathClass Calculate = new MathClass();
        const short DataCmd = 0x00;
        const short DataLength = 0x0C;
        public byte ID { get; set; }            //
        /// <summary>
        /// R相溫度
        /// </summary>
        public float R_TEMP { get; private set; }
        /// <summary>
        /// S相溫度
        /// </summary>
        public float S_TEMP { get; private set; }
        /// <summary>
        /// T相溫度
        /// </summary>
        public float T_TEMP { get; private set; }
        /// <summary>
        /// R相濕度
        /// </summary>
        public float R_RH { get; private set; }
        /// <summary>
        /// S相濕度
        /// </summary>
        public float S_RH { get; private set; }
        /// <summary>
        /// T相濕度
        /// </summary>
        public float T_RH { get; private set; }
        /// <summary>
        /// R相斷線
        /// </summary>
        public int R_BREAK { get; private set; }
        /// <summary>
        /// S相斷線
        /// </summary>
        public int S_BREAK { get; private set; }
        /// <summary>
        /// T相斷線
        /// </summary>
        public int T_BREAK { get; private set; }
        /// <summary>
        /// 從主機位址
        /// </summary>
        public int SLAVE_ID { get; private set; }
        /// <summary>
        /// 繼電器狀態
        /// </summary>
        public int RELAY_ST { get; private set; }
        /// <summary>
        /// 第一路通到開啟
        /// </summary>
        public int CHANNEL1_ST { get; private set; }
        /// <summary>
        /// 解析函式
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public override bool AnalysisDataFunction(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Inbyte) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                int k = 3;
                R_TEMP = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F - 40; k += 2;
                R_RH = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                S_TEMP = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F - 40; k += 2;
                S_RH = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                T_TEMP = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F - 40; k += 2;
                T_RH = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                SLAVE_ID = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]); k += 2;
                RELAY_ST = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]); k += 2;
                CHANNEL1_ST = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]);
                return true;
            }
            else
            {
                return false;
            }
        }

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
                if (database.CheckDataTableExistFunction($"N330_{ID}", ConnStrLog))
                {
                    if (!database.CheckDataLogExistFunction($"N330_{ID}", ConnStrLog))
                    {
                        database.AddNewDataLogFunction($"N330_{ID}", "ttime,ttimen,ttemp1,trh1,ttemp2,trh2,ttemp3,trh3,tbreak1,tbreak2,tbreak3,trelay", $"'{DateTime.Now.ToString("yyyyMMddHHmm")}00','{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}:00',{R_TEMP},{R_RH},{S_TEMP},{S_RH},{T_TEMP},{T_RH},{R_BREAK},{S_BREAK},{T_BREAK},{RELAY_ST}", ConnStrLog);
                    }
                }
                else
                {
                    database.CreateDataTableFunction($"N330_{ID}",
                                                      "ttime NCHAR(14) DEFAULT '',ttimen DATETIME," +
                                                      "ttemp1 FLOAT DEFAULT 0,trh1 FLOAT DEFAULT 0," +
                                                      "ttemp2 FLOAT DEFAULT 0,trh2 FLOAT DEFAULT 0," +
                                                      "ttemp3 FLOAT DEFAULT 0,trh3 FLOAT DEFAULT 0," +
                                                      "tbreak1 FLOAT DEFAULT 0,tbreak2 FLOAT DEFAULT 0,tbreak3 FLOAT DEFAULT 0,trelay FLOAT DEFAULT 0 PRIMARY KEY(ttime)", ConnStrLog);
                }
            }
        }
    }
}
