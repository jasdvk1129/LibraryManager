using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramLibrary.Modules;

namespace TelegramLibrary
{
    public class TelegramBotClass
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
        /// 傳送訊息-群組
        /// </summary>
        /// <param name="Message">訊息</param>
        public void Send_Message_Group(string Message)
        {
            Send_Message(Chat_ID, Message);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Message(Chat_ID, Message);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
        }
        /// <summary>
        /// 傳送訊息-個人
        /// </summary>
        /// <param name="Message">訊息</param>
        public void Send_Message_Personal(string Message)
        {
            Send_Message(Chat_ID, Message);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.first_name == PersonalName.Substring(0, 1) & g.last_name == PersonalName.Substring(1, 2) & g.type == "private").First();
            //    Send_Message(Chat_ID, Message);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到個人姓名"); }
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
        /// 上傳檔案-群組
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Filed_Group(string Filed, string Caption = null)
        {
            Send_Filed(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Filed(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
        }
        /// <summary>
        /// 上傳檔案-個人
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Filed_Personal(string Filed, string Caption = null)
        {
            Send_Filed(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.first_name == PersonalName.Substring(0, 1) & g.last_name == PersonalName.Substring(1, 2) & g.type == "private").First();
            //    Send_Filed(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到個人姓名"); }
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
        /// 上傳圖片-群組
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Photo_Group(string Filed, string Caption = null)
        {
            Send_Photo(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Photo(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
        }
        /// <summary>
        /// 上傳圖片-個人
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Photo_Personal(string Filed, string Caption)
        {
            Send_Photo(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Photo(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
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
        /// 上傳影片(mp4)-群組
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Video_Group(string Filed, string Caption = null)
        {
            Send_Video(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Video(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
        }
        /// <summary>
        /// 上傳影片(mp4)-個人
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Video_Personal(string Filed, string Caption = null)
        {
            Send_Video(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Video(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
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
        /// 上傳動畫(GIF、有聲音video)-群組
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Animation_Group(string Filed, string Caption = null)
        {
            Send_Animation(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Animation(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
        }
        /// <summary>
        /// 上傳動畫(GIF、有聲音video)-個人
        /// </summary>
        /// <param name="Filed">檔案絕對路徑</param>
        /// <param name="Caption">描述 (不使用請輸入 null)</param>
        public void Send_Animation_Personal(string Filed, string Caption = null)
        {
            Send_Animation(Chat_ID, Filed, Caption);
            //try
            //{
            //    GetUpDate getUpDate = GetUpDates.Where(g => g.all_members_are_administrators == true & g.title == GroupName & g.type == "group").First();
            //    Send_Animation(Chat_ID, Filed, Caption);
            //}
            //catch (Exception ex) { Log.Error(ex, "找不到群組名稱"); }
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
    }
}
