using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using Serilog;

namespace MailSendLibrary
{
    public class MailSendClass
    {
        public MailSendClass()
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\maillog\\log-.txt",
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();        //宣告Serilog初始化
        }
        /// <summary>
        /// 基本Mail傳送物件函式
        /// </summary>
        /// <param name="mSenderID">寄件者</param>
        /// <param name="mSenderName">寄件者名稱</param>
        /// <param name="mTo">收件者</param>
        /// <param name="mCC">副本</param>
        /// <param name="mBCC">密件副本</param>
        /// <param name="mSubject">主旨</param>
        /// <param name="mBody">內文</param>
        /// <param name="mVerificationID">Mail Server 帳號</param>
        /// <param name="mVerificationPwd">Mail Server 密碼</param>
        /// <param name="mSMTPPort">SMTP Port</param>
        /// <param name="mSMTPServer">SMTP Server</param>
        /// <param name="mSSL">SSL加密</param>
        /// <returns></returns>
        public bool TheEmailSend(string mSenderID, string mSenderName,
                                 string mTo, string mCC, string mBCC,
                                 string mSubject, string mBody,
                                 string mVerificationID, string mVerificationPwd,
                                 int mSMTPPort, string mSMTPServer,
                                 bool mSSL)
        {
            try
            {
                MailMessage myMail = new MailMessage();
                myMail.From = new MailAddress(mSenderID, mSenderName);
                if (!(mTo.Trim().Length == 0))
                    myMail.To.Add(mTo);
                if (!(mCC.Trim().Length == 0))
                    myMail.CC.Add(mCC);
                if (!(mBCC.Trim().Length == 0))
                    myMail.Bcc.Add(mBCC);
                myMail.SubjectEncoding = Encoding.UTF8;
                myMail.Subject = mSubject;
                myMail.IsBodyHtml = true;
                myMail.Body = mBody;
                SmtpClient mySmtp = new SmtpClient();
                mySmtp.Credentials = new NetworkCredential(mVerificationID.Trim(), mVerificationPwd.Trim());
                mySmtp.Port = mSMTPPort;
                mySmtp.Host = mSMTPServer;
                mySmtp.EnableSsl = mSSL;
                mySmtp.Send(myMail);
                Log.Information($"Send email success,from {mSenderID} to {mTo} use smtp setting is {mSMTPServer}:{mSMTPPort} SSL {mSSL.ToString()}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Send email message failed, from {mSenderID} to {mTo} use smtp setting is{mSMTPServer}:{mSMTPPort} SSL {mSSL.ToString()}");
                return false;
            }
        }
    }
}
