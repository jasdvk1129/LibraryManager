using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ErrorMessageLibrary
{
    public class ErrorMessageClass
    {
        private string WorkPath;
        public string _WorkPath
        {
            get { return WorkPath; }
            set
            {
                if (WorkPath == "") value = Directory.GetCurrentDirectory();
                WorkPath = value;
            }
        }
        private Encoding coding = Encoding.Default;
        public RichTextBox DisplayTextBox;
        private object _thislock = new object();

        #region TextObject更新委派
        private delegate void UpdateUICallBack_Text(Control TextUI, string Newtext);
        private void UpdateUIText(Control TextUI, string Newtext)
        {
            if (TextUI.InvokeRequired)
            {
                UpdateUICallBack_Text invokeDelegate = new UpdateUICallBack_Text(UpdateUIText);
                TextUI.Invoke(invokeDelegate, TextUI, Newtext);
            }
            else
            {
                TextUI.Text = Newtext;
            }
        }
        #endregion

        /// <summary>
        /// 錯誤訊息攔截、紀錄及顯示
        /// </summary>
        /// <param name="errormessage">對於錯誤誤的描述</param>
        /// <param name="ex">例外</param>
        public void _errorText(string errormessage, Exception ex)
        {
            lock (_thislock)
            {
                string errtext = "";
                DateTime path = DateTime.Now;
                if (Directory.Exists(WorkPath + "\\error\\" + path.ToString("yyyyMM")) == false)
                    Directory.CreateDirectory(WorkPath + "\\error\\" + path.ToString("yyyyMM"));
                string filename = WorkPath + "\\error\\" + path.ToString("yyyyMM") + "\\" + path.ToString("yyyyMMddHHmm") + ".err";
                StreamWriter errfile = new StreamWriter(filename, true, coding);
                errfile.WriteLine(Directory.GetCurrentDirectory());
                errfile.WriteLine("時間:" + path.ToString("yyyy/MM/dd HH:mm:ss"));
                errfile.WriteLine(errormessage);
                errfile.WriteLine("ex.ToString = " + ex.ToString());
                errfile.WriteLine("ex.Message = " + ex.Message);
                errfile.WriteLine("ex.StackTrace = " + ex.StackTrace);
                errfile.WriteLine("***********************************************************************************************");
                errfile.WriteLine();
                errfile.Close();
                errtext += Directory.GetCurrentDirectory() + "/n";
                errtext += "時間:" + path.ToString("yyyy/MM/dd HH:mm:ss") + "/n";
                errtext += errormessage + "/n";
                errtext += "ex.ToString = " + ex.ToString() + "/n";
                errtext += "ex.Message = " + ex.Message + "/n";
                errtext += "ex.StackTrace = " + ex.StackTrace + "/n";
                UpdateUIText(DisplayTextBox, errtext);
            }
        }
        /// <summary>
        /// 錯誤訊息攔截、紀錄及顯示
        /// </summary>
        /// <param name="errormessage">對於錯誤誤的描述</param>
        /// <param name="ex">例外</param>
        public void _error(string errormessage, Exception ex)
        {
            lock (_thislock)
            {
                DateTime path = DateTime.Now;
                if (Directory.Exists(WorkPath + "\\error\\" + path.ToString("yyyyMM")) == false)
                    Directory.CreateDirectory(WorkPath + "\\error\\" + path.ToString("yyyyMM"));
                string filename = WorkPath + "\\error\\" + path.ToString("yyyyMM") + "\\" + path.ToString("yyyyMMddHHmm") + ".err";
                StreamWriter errfile = new StreamWriter(filename, true, coding);
                errfile.WriteLine(Directory.GetCurrentDirectory());
                errfile.WriteLine("時間:" + path.ToString("yyyy/MM/dd HH:mm:ss"));
                errfile.WriteLine(errormessage);
                errfile.WriteLine("ex.ToString = " + ex.ToString());
                errfile.WriteLine("ex.Message = " + ex.Message);
                errfile.WriteLine("ex.StackTrace = " + ex.StackTrace);
                errfile.WriteLine("***********************************************************************************************");
                errfile.WriteLine();
                errfile.Close();
                if (MessageBox.Show("程式發生錯誤，是否觀看錯誤訊息？", "錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(filename);
            }
        }
        /// <summary>
        /// 錯誤訊息攔截及紀錄
        /// </summary>
        /// <param name="errormessage">對於錯誤誤的描述</param>
        /// <param name="ex">例外</param>
        public void _errorHide(string errormessage, Exception ex)
        {
            lock (_thislock)
            {
                DateTime path = DateTime.Now;
                if (Directory.Exists(WorkPath + "\\error\\" + path.ToString("yyyyMM")) == false)
                    Directory.CreateDirectory(WorkPath + "\\error\\" + path.ToString("yyyyMM"));
                string filename = WorkPath + "\\error\\" + path.ToString("yyyyMM") + "\\" + path.ToString("yyyyMMddHHmm") + ".err";
                StreamWriter errfile = new StreamWriter(filename, true, coding);
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

        public void _log(string functionname, string logmessage)
        {
            lock (_thislock)
            {
                DateTime path = DateTime.Now;
                if (Directory.Exists(WorkPath + "\\log\\" + path.ToString("yyyyMM")) == false)
                    Directory.CreateDirectory(WorkPath + "\\log\\" + path.ToString("yyyyMM"));
                string filename = WorkPath + "\\log\\" + path.ToString("yyyyMMdd") + ".log";
                StreamWriter WriteLog = new StreamWriter(filename, true, coding);
                if (!File.Exists(filename))
                    WriteLog.WriteLine("time,function,message");
                WriteLog.WriteLine(path.ToString("yyyy/MM/dd HH:mm:ss") + "," + functionname + "," + logmessage);
                WriteLog.Close();
            }
        }

        public void TimeDelay(int iMilliSeconds)
        {
            int iStart;
            iStart = Environment.TickCount;
            while ((Environment.TickCount - iStart) <= iMilliSeconds)
            {
                Application.DoEvents();
            }
        }
    }
}
