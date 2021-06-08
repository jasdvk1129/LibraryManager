using Microsoft.Win32.SafeHandles;
using RestSharp;
using Serilog;
using System;
using System.Runtime.InteropServices;

namespace LineNotifyLibrary
{
    public class LineNotifyClass : IDisposable
    {
        /// <summary>
        /// Line Notify權杖
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Line Notify初始物件
        /// </summary>
        /// <param name="token">Line Notify權杖</param>
        public LineNotifyClass(string token)
        {
            Token = token;
        }

        #region LINE傳送訊息
        /// <summary>
        /// LINE傳送訊息
        /// </summary>
        /// <param name="Message">訊息內容</param>
        public void LineNotifyFunction(string Message)
        {
            try
            {
                var client = new RestClient("https://notify-api.line.me/api/notify");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {Token}");//發行權杖   vCRRckH7mjKweZ2YnOR0ziR14nJfCqfGrYHe8CshuYz
                request.AddHeader("content-type", "multipart/form-data; boundary=----Line");
                request.AddParameter("multipart/form-data; boundary=----Line",
                                     $"------Line\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\n{Message}\r\n" +
                                     "------Line--", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine($"{response.Content}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Line notify傳送發生錯誤 訊息內容:{Message}");
            }
        }
        #endregion

        #region LINE傳送訊息與圖片
        /// <summary>
        /// LINE傳送訊息與圖片
        /// </summary>
        /// <param name="Message">訊息內容</param>
        /// <param name="ImageUrl">網址圖片</param>
        public void LineNotifyFunction(string Message, string ImageUrl)
        {
            try
            {
                var client = new RestClient("https://notify-api.line.me/api/notify");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {Token}");                          //發行權杖   vCRRckH7mjKweZ2YnOR0ziR14nJfCqfGrYHe8CshuYz
                request.AddHeader("content-type", "multipart/form-data; boundary=----Line");
                request.AddParameter("multipart/form-data; boundary=----Line",
                                     $"------Line\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\n{Message}\r\n" +
                                     $"------Line\r\nContent-Disposition: form-data; name=\"imageThumbnail\"\r\n\r\n{ImageUrl}\r\n" +
                                     $"------Line\r\nContent-Disposition: form-data; name=\"imageFullsize\"\r\n\r\n{ImageUrl}\r\n" +
                                     "------Line--", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine($"{response.Content}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Line notify傳送發生錯誤 訊息內容:{Message} 圖片URL:{ImageUrl}");
            }
        }
        #endregion

        #region 釋放
        private bool _disposed = false;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _safeHandle?.Dispose();
            }
            _disposed = true;
        }
        #endregion
    }
}
