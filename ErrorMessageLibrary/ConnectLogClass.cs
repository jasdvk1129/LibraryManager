using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ErrorMessageLibrary
{
    public class ConnectLogClass
    {
        public ErrorMessageClass ErrMsg = null;
        public string WorkPath { get; set; }
        private object _thislock = new object();
        private Encoding coding = Encoding.Default;
        public RichTextBox DisplayTextBox;

        public void ConnectStateLogWriter_Text(string _Message)
        {
            lock (_thislock)
            {
                try
                {
                    DateTime NowTime = DateTime.Now;
                    if (Directory.Exists(WorkPath + "\\Connected") == false)
                        Directory.CreateDirectory(WorkPath + "\\Connected");
                    string FileName = WorkPath + "\\Connected\\" + NowTime.ToString("yyyyMMdd") + ".txt";
                    StreamWriter connectstatefile = new StreamWriter(FileName, true, coding);
                    connectstatefile.WriteLine(NowTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\t\t\t" + _Message);
                    DisplayTextBox.Text += NowTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\t\t" + _Message + Environment.NewLine;
                    connectstatefile.Close();
                }
                catch (Exception ex)
                {
                    ErrMsg._errorText("Connect state log error.", ex);
                }
            }
        }
    }
}
