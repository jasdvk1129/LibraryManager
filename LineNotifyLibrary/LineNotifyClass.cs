using Microsoft.Win32.SafeHandles;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
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
        /// 使用者ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Line Notify初始物件
        /// </summary>
        /// <param name="token">Line 權杖</param>
        /// <param name="userid">使用者ID</param>
        public LineNotifyClass(string token,string userid)
        {
            Token = token;
            UserID = userid;
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
                var option = new RestClientOptions("https://notify-api.line.me/api/notify");
                var client = new RestClient(option);
                var request = new RestRequest("", method: Method.Post);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {Token}");//發行權杖   vCRRckH7mjKweZ2YnOR0ziR14nJfCqfGrYHe8CshuYz
                request.AddHeader("content-type", "multipart/form-data; boundary=----Line");
                request.AddParameter("multipart/form-data; boundary=----Line",
                                     $"------Line\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\n{Message}\r\n" +
                                     "------Line--", ParameterType.RequestBody);
                var response = client.Execute(request);
                Console.WriteLine($"{response.Content}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Line notify傳送發生錯誤 訊息內容:{Message}");
            }
        }
        /// <summary>
        /// LINE傳送訊息
        /// </summary>
        /// <param name="Message"></param>
        public async void LineManagerFunction(string Message)
        {
            try
            {
                var option = new RestClientOptions("https://api.line.me/v2/bot/message/push");
                var client = new RestClient(option);
                var request = new RestRequest("", method: Method.Post);
                request.Timeout = TimeSpan.FromSeconds(3);
                request.AddHeader("Authorization", $"Bearer {Token}");
                request.AddHeader("Content-Type", "application/json");
                SendText JsonBody = new SendText();
                JsonBody = new SendText
                {
                    to = UserID,
                    messages = new List<Message> { new Message{
                    type = "text",
                    text = $"{Message}" }}
                };
                request.AddJsonBody(JsonBody);
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    await Console.Out.WriteLineAsync("成功回應");
                }
                else
                {
                    await Console.Out.WriteLineAsync("失敗回應");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Line Manager傳送發生錯誤 訊息內容:{Message}");
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
                var option = new RestClientOptions("https://notify-api.line.me/api/notify");
                var client = new RestClient(option);
                var request = new RestRequest("", method: Method.Post);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {Token}");                          //發行權杖   vCRRckH7mjKweZ2YnOR0ziR14nJfCqfGrYHe8CshuYz
                request.AddHeader("content-type", "multipart/form-data; boundary=----Line");
                request.AddParameter("multipart/form-data; boundary=----Line",
                                     $"------Line\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\n{Message}\r\n" +
                                     $"------Line\r\nContent-Disposition: form-data; name=\"imageThumbnail\"\r\n\r\n{ImageUrl}\r\n" +
                                     $"------Line\r\nContent-Disposition: form-data; name=\"imageFullsize\"\r\n\r\n{ImageUrl}\r\n" +
                                     "------Line--", ParameterType.RequestBody);
                var response = client.Execute(request);
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
