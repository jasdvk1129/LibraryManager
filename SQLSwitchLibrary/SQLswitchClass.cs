using Serilog;
using System;
using System.ServiceProcess;

namespace SQLSwitchLibrary
{
    /// <summary>
    /// 重啟SQLserver服務
    /// WorkPath = 錯誤檔存入路徑
    /// 請開啟最高權限使用，點選專案屬性→安全性（勾選"啟用ClickOnce安全設置"），在項目名稱裡的"Properties"下面一個"app.manifest"
    /// 找尋 level="asInvoker" uiAccess="false"改為 level="requireAdministrator" uiAccess="false" 
    /// 再(取消勾選"啟用ClickOnce安全設置")，重新建置專案
    /// </summary>
    public class SQLswitchClass
    {
        public SQLswitchClass()
        {
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()
                             .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\log\\SQL\\sqllog-.txt",
                                           rollingInterval: RollingInterval.Day,
                                           outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                             .CreateLogger();        //宣告Serilog初始化
        }
        /// <summary>
        /// 重啟SQLserver服務
        /// </summary>
        public void SQLserverReStart()
        {
            ServiceController controller = new ServiceController();
            try
            {
                controller.MachineName = Environment.MachineName;
                controller.ServiceName = "MSSQLSERVER";
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (ArgumentNullException ex) { Log.Error(ex,"服務名稱請勿輸入NULL"); }
            catch (ArgumentException ex) { Log.Error(ex,"確認服務名稱是否正確"); }
            catch (InvalidOperationException ex) { Log.Error(ex,"使用者權限錯誤程式請以最高權限執行"); }
        }
    }
}
