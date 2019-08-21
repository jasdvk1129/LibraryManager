using System;
using System.IO;
using System.ServiceProcess;
using System.Text;

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
        /// <summary>
        /// 錯誤檔存入路徑
        /// </summary>
        public string WorkPath { get; set; }
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
            catch (ArgumentNullException ex) { _errorHide("服務名稱請勿輸入NULL", ex); }
            catch (ArgumentException ex) { _errorHide("確認服務名稱是否正確", ex); }
            catch (InvalidOperationException ex) { _errorHide("使用者權限錯誤程式請以最高權限執行", ex); }
        }
        private object _thislock = new object();
        private void _errorHide(string errormessage, Exception ex)
        {
            lock (_thislock)
            {
                DateTime path = DateTime.Now;
                if (Directory.Exists(WorkPath + "\\error\\" + path.ToString("yyyyMM")) == false)
                    Directory.CreateDirectory(WorkPath + "\\error\\" + path.ToString("yyyyMM"));
                string filename = WorkPath + "\\error\\" + path.ToString("yyyyMM") + "\\" + path.ToString("yyyyMMddHHmm") + ".err";
                StreamWriter errfile = new StreamWriter(filename, true, Encoding.Default);
                errfile.WriteLine(Directory.GetCurrentDirectory());
                errfile.WriteLine("時間:" + path.ToString("yyyy/MM/dd HH:mm:ss"));
                errfile.WriteLine(errormessage);
                errfile.WriteLine("ex.ToString = " + ex.ToString());
                errfile.WriteLine("ex.Message = " + ex.Message);
                errfile.WriteLine("ex.StackTrace = " + ex.StackTrace);
                errfile.WriteLine("***********************************************************************************************");
                errfile.WriteLine();
                errfile.Close();
            }
        }
    }
}
