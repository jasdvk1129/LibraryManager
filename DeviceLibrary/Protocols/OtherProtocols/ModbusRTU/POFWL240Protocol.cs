using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLibrary;
using DatabaseLibrary;

namespace DeviceLibrary.Protocols.OtherProtocols.ModbusRTU
{
    public class POFWL240Protocol : AbsOtherProtocol
    {
        private MathClass Calculate = new MathClass();
        const short DataCmd = 4368;
        const short DataLength = 5;
        /// <summary>
        /// 設備站號(1~255)
        /// </summary>
        public byte ID { get; set; }            //
        /// <summary>
        /// 銅排溫度
        /// </summary>
        public float TEMP_CUBUS { get; private set; }
        /// <summary>
        /// 紫外光功率
        /// </summary>
        public float UV_LEVEL { get; private set; }
        /// <summary>
        /// 可見光功率
        /// </summary>
        public float LIGHT_LEVEL { get; private set; }
        public override bool AnalysisDataFunction(byte[] Cmd, byte[] Inbyte)
        {
            if (CheckCRC16(Inbyte) && Cmd[0] == Inbyte[0] && Cmd[1] == Inbyte[1])
            {
                var k = 3;
                TEMPTURE = Calculate.work16to10(Inbyte[k], Inbyte[k + 1], true) * 0.1F; k += 2;
                RH = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                TEMP_CUBUS = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                UV_LEVEL = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F; k += 2;
                LIGHT_LEVEL = Calculate.work16to10(Inbyte[k], Inbyte[k + 1]) * 0.1F;
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
                if (database.CheckDataTableExistFunction($"POFWL240_{ID}", ConnStrLog))
                {
                    if (!database.CheckDataLogExistFunction($"POFWL240_{ID}", ConnStrLog))
                    {
                        database.AddNewDataLogFunction($"POFWL240_{ID}", "ttime,ttimen,ttemp,trh,tcutemp,tuvlevel,tlightlevel", $"'{DateTime.Now.ToString("yyyyMMddHHmm")}00','{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}:00',{TEMPTURE},{RH},{TEMP_CUBUS},{UV_LEVEL},{LIGHT_LEVEL}", ConnStrLog);
                    }
                }
                else
                {
                    database.CreateDataTableFunction($"POFWL2400_{ID}",
                                                      "ttime NCHAR(14) DEFAULT '',ttimen DATETIME," +
                                                      "ttemp FLOAT DEFAULT 0,trh FLOAT DEFAULT 0," +
                                                      "tcutemp FLOAT DEFAULT 0,tuvlevel FLOAT DEFAULT 0," +
                                                      "tlightlevel FLOAT DEFAULT 0 PRIMARY KEY(ttime)", ConnStrLog);
                }
            }
        }
    }
}
