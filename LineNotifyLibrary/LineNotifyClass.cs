using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace LineNotifyLibrary
{
    public class LineNotifyClass
    {
        public string Token { get; set; }
        private string ContentType = "application/x-www-form-urlencoded";
        private string Method = "POST";
        private string URL = "https://notify-api.line.me/api/notify";
        /// <summary>
        /// Line發送純文字
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string SendLineNotify(string message)
        {
            string BearerToken = "Bearer " + Token;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = Method;
            request.ContentType = ContentType;
            request.Headers.Add("Authorization", BearerToken);
            byte[] byteArray = Encoding.UTF8.GetBytes("message=" + HttpUtility.UrlEncode(message));
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }
            string responseStr = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            { responseStr = ex.Message.ToString(); }
            return responseStr;
        }
        /// <summary>
        /// Line發送含貼圖功能
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stickerPackageId"></param>
        /// <param name="stickerId"></param>
        /// <returns></returns>
        public string SendLineNotify(string message, int stickerPackageId, int stickerId)
        {
            Token = string.Format("Bearer {0}", Token);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = Method;
            request.ContentType = ContentType;
            request.Headers.Add("Authorization", Token);
            NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
            postParams.Add("stickerPackageId", stickerPackageId.ToString());
            postParams.Add("stickerId", stickerId.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode("message=" + message + "&" + postParams.ToString()));
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }
            string responseStr = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            { responseStr = ex.Message.ToString(); }
            return responseStr;
        }
    }
}
