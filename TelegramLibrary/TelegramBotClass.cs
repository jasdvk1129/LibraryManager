using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TelegramLibrary.Modules;

namespace TelegramLibrary
{
    public class TelegramBotClass :IDisposable
    {
        /// <summary>
        /// Telegram初始化(所有上傳檔案都有容量限制，請依照Telegram官網為準)
        /// </summary>
        /// <param name="Telegram_Http_API">Telegram機器人API網址(必填)</param>
        /// <param name="Chat_ID">機器人 傳送訊息ID (必填)</param>
        /// <param name="groupname">群組名稱 (不使用請輸入 null)</param>
        /// <param name="personalname">個人名稱 (不使用請輸入 null)</param>
        public TelegramBotClass(string Telegram_Http_API, string Chat_ID, string groupname = null, string personalname = null)
        {
            this.Chat_ID = Chat_ID;
            if (Telegram_Http_API != null)
            {
                Telegram_HTTP_API = Telegram_Http_API;
                Serch_TelegramID();
            }
            if (groupname != null)
            {
                GroupName = groupname;
            }
            if (personalname != null)
            {
                PersonalName = personalname;
            }
        }
        /// <summary>
        /// 機器人 API網址
        /// </summary>
        public string Telegram_HTTP_API { get; set; }
        /// <summary>
        /// 機器人 傳送訊息ID (群組 = 有負號， 個人 = 沒有負號)
        /// </summary>
        public string Chat_ID { get; set; }

        /// <summary>
        /// Telegram取到的ID(group = 群組， Private = 個人)
        /// </summary>
        public List<GetUpDate> GetUpDates = new List<GetUpDate>();
        /// <summary>
        /// 群組名稱
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 個人名稱
        /// </summary>
        public string PersonalName { get; set; }

        #region  搜尋Telegram機器人被加入的 群組或個人ID
        /// <summary>
        /// 搜尋Telegram機器人被加入的 群組或個人ID
        /// </summary>
        public void Serch_TelegramID()
        {
            try
            {
                var client = new RestClient($"https://api.telegram.org/bot{Telegram_HTTP_API}/getUpdates?");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);
                JObject jsondatas = JsonConvert.DeserializeObject<JObject>(response.Content);
                for (int Index = 0; Index < jsondatas["result"].Count(); Index++)
                {
                    JObject jsondata = JsonConvert.DeserializeObject<JObject>(jsondatas["result"][Index].ToString());
                    GetUpDate getUpdate = JsonConvert.DeserializeObject<GetUpDate>(jsondata["message"]["chat"].ToString());
                    GetUpDates.Add(getUpdate);
                }
            }
            catch (Exception ex) { Log.Error(ex, $"Telegram機器人 API網址錯誤 API網址: {Telegram_HTTP_API}"); }
        }
        #endregion

        #region 傳送訊息
        /// <summary>
        /// 傳送訊息
        /// </summary>
        /// <param name="Message">訊息</param>
        public void Send_Message(string Message)
        {
            Send_Message(Chat_ID, Message);
        }
        /// <summary>
        /// 傳送訊息方法
        /// </summary>
        /// <param name="Chat_ID">群組或個人資訊</param>
        /// <param name="Message">訊息</param>
        private void Send_Message(string Chat_ID, string Message)
        {
            try
            {
                var client = new RestClient("https://api.telegram.org/bot"+Telegram_HTTP_API+"/sendMessage?");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("chat_id", Chat_ID);
                request.AddParameter("text", Message);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) { Log.Error(ex, $"傳送訊息失敗 傳送位址: {Telegram_HTTP_API} chat_id: {Chat_ID}"); }
        }
        #endregion

        #region 上傳檔案
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Filed(string Filed, string Caption = null)
        {
            Send_Filed(Chat_ID, Filed, Caption);
        }
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="Chat_ID">群組或個人資訊</param>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述</param>
        private void Send_Filed(string Chat_ID, string Filed, string Caption)
        {
            try
            {
                var client = new RestClient("https://api.telegram.org/bot" + Telegram_HTTP_API + "/sendDocument?");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddParameter("chat_id", Chat_ID);
                request.AddFile("document", Filed);
                if (Caption != null)
                {
                    request.AddParameter("caption", Caption);
                }
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) { Log.Error(ex, $"傳送檔案失敗 傳送位址: {Telegram_HTTP_API} chat_id: {Chat_ID}"); }
        }
        #endregion

        #region 上傳圖片
        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Photo(string Filed, string Caption)
        {
            Send_Photo(Chat_ID, Filed, Caption);
        }
        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="Chat_ID">群組或個人資訊</param>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述</param>
        private void Send_Photo(string Chat_ID, string Filed, string Caption = null)
        {
            try
            {
                var client = new RestClient("https://api.telegram.org/bot" + Telegram_HTTP_API + "/sendPhoto?");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddParameter("chat_id", Chat_ID);
                request.AddFile("photo", Filed);
                if (Caption != null)
                {
                    request.AddParameter("caption", Caption);
                }
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) { Log.Error(ex, $"傳送圖片失敗 傳送位址: {Telegram_HTTP_API} chat_id: {Chat_ID}"); }
        }
        #endregion

        #region 上傳影片
        /// <summary>
        /// 上傳影片(mp4)
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Video(string Filed, string Caption = null)
        {
            Send_Video(Chat_ID, Filed, Caption);
        }
        /// <summary>
        /// 上傳影片
        /// </summary>
        /// <param name="Chat_ID">群組或個人資訊</param>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述</param>
        private void Send_Video(string Chat_ID, string Filed, string Caption)
        {
            try
            {
                var client = new RestClient("https://api.telegram.org/bot" + Telegram_HTTP_API + "/sendVideo?");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddParameter("chat_id", Chat_ID);
                request.AddFile("video", Filed);
                if (Caption != null)
                {
                    request.AddParameter("caption", Caption);
                }
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) { Log.Error(ex, $"傳送影片失敗 傳送位址: {Telegram_HTTP_API} chat_id: {Chat_ID}"); }
        }
        #endregion

        #region 上傳動畫
        /// <summary>
        /// 上傳動畫(GIF、有聲音video)
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Animation(string Filed, string Caption = null)
        {
            Send_Animation(Chat_ID, Filed, Caption);
        }
        /// <summary>
        /// 上傳動畫
        /// </summary>
        /// <param name="Chat_ID">群組或個人資訊</param>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述</param>
        private void Send_Animation(string Chat_ID, string Filed, string Caption)
        {
            try
            {
                var client = new RestClient("https://api.telegram.org/bot" + Telegram_HTTP_API + "/sendVideo?");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddParameter("chat_id", Chat_ID);
                request.AddFile("animation", Filed);
                if (Caption != null)
                {
                    request.AddParameter("caption", Caption);
                }
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) { Log.Error(ex, $"傳送動畫失敗 傳送位址: {Telegram_HTTP_API} chat_id: {Chat_ID}"); }
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
