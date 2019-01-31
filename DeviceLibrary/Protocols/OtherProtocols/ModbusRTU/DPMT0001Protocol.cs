using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLibrary;
using DatabaseLibrary;

namespace DeviceLibrary.Protocols.OtherProtocols.ModbusRTU
{
    public class DPMT0001Protocol : AbsOtherProtocol
    {
        private MathClass Calculate = new MathClass();
        const short DataCmd = 32;
        const short DataLength = 6;
        /// <summary>
        /// 設備站號(1~255)
        /// </summary>
        public byte ID { get; set; }
        /// <summary>
        /// 斷路器電源側A相溫度
        /// </summary>
        public float POWER_TEMP_A { get; private set; }
        /// <summary>
        /// 斷路器電源側B相溫度
        /// </summary>
        public float POWER_TEMP_B { get; private set; }
        /// <summary>
        /// 斷路器電源側C相溫度
        /// </summary>
        public float POWER_TEMP_C { get; private set; }
        /// <summary>
        /// 斷路器負載側A相溫度
        /// </summary>
        public float LOAD_TEMP_A { get; private set; }
        /// <summary>
        /// 斷路器負載側B相溫度
        /// </summary>
        public float LOAD_TEMP_B { get; private set; }
        /// <summary>
        /// 斷路器負載側C相溫度
        /// </summary>
        public float LOAD_TEMP_C { get; private set; }
        /// <summary>
        /// 解析資料函式
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Inbyte"></param>
        /// <returns></returns>
        public override bool AnalysisDataFunction(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Inbyte) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                var k = 3;
                POWER_TEMP_A = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F; k += 2;
                POWER_TEMP_B = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F; k += 2;
                POWER_TEMP_C = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F; k += 2;
                LOAD_TEMP_A = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F; k += 2;
                LOAD_TEMP_B = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F; k += 2;
                LOAD_TEMP_C = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.01F;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 命令建構函式
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
        /// Log紀錄函式
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
                if (database.CheckDataTableExistFunction($"DPMT0001_{ID}", ConnStrLog))
                {
                    if (!database.CheckDataLogExistFunction($"DPMT0001_{ID}", ConnStrLog))
                    {
                        database.AddNewDataLogFunction($"DPMT0001_{ID}", "ttime,ttimen,tpowertempA,tpowertempB,tpowertempC,tloadtempA,tloadtempB,tloadtempC", $"'{DateTime.Now.ToString("yyyyMMddHHmm")}00','{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}:00',{POWER_TEMP_A},{POWER_TEMP_B},{POWER_TEMP_C},{LOAD_TEMP_A},{LOAD_TEMP_B},{LOAD_TEMP_C}", ConnStrLog);
                    }
                }
                else
                {
                    database.CreateDataTableFunction($"DPMT0001_{ID}",
                                                      "ttime NCHAR(14) DEFAULT '',ttimen DATETIME," +
                                                      "tpowertempA FLOAT DEFAULT 0,tpowertempB FLOAT DEFAULT 0,tpowertempC FLOAT DEFAULT 0," +
                                                      "tloadtempA FLOAT DEFAULT 0,tloadtempB FLOAT DEFAULT 0,tloadtempC FLOAT DEFAULT 0 PRIMARY KEY(ttime)", ConnStrLog);
                }
            }
        }
    }
}
