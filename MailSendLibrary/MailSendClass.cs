using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using ErrorMessageLibrary;
using System.IO;

namespace MailSendLibrary
{
    public class MailSendClass
    {
        ErrorMessageClass ErrMsg;
        public bool TheEmailSend(string mSenderID, string mSenderName,
                                 string mTo, string mCC, string mBCC,
                                 string mSubject, string mBody,
                                 string mVerificationID, string mVerificationPwd,
                                 int mSMTPPort, string mSMTPServer,
                                 bool mSSL)
        {
            try
            {
                ErrMsg = new ErrorMessageClass();
                ErrMsg._WorkPath = Directory.GetCurrentDirectory();         //錯誤訊息儲存路徑
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
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg._error("Send email message error.", ex);
                return false;
            }
        }
    }
}
