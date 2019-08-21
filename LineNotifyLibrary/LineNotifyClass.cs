using RestSharp;

namespace LineNotifyLibrary
{
    public class LineNotifyClass
    {
        /// <summary>
        /// LINE傳送訊息
        /// </summary>
        /// <param name="Token">權杖</param>
        /// <param name="Message">訊息內容</param>
        public void LineNotifyFunction(string Token, string Message)
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
        }
        /// <summary>
        /// LINE傳送訊息與圖片
        /// </summary>
        /// <param name="Token">權杖</param>
        /// <param name="Message">訊息內容</param>
        /// <param name="Image">網址圖片</param>
        public void LineNotifyFunction(string Token, string Message, string Image)
        {
            var client = new RestClient("https://notify-api.line.me/api/notify");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {Token}");//發行權杖   vCRRckH7mjKweZ2YnOR0ziR14nJfCqfGrYHe8CshuYz
            request.AddHeader("content-type", "multipart/form-data; boundary=----Line");
            request.AddParameter("multipart/form-data; boundary=----Line",
                $"------Line\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\n{Message}\r\n" +
                $"------Line\r\nContent-Disposition: form-data; name=\"imageThumbnail\"\r\n\r\n{Image}\r\n" +
                $"------Line\r\nContent-Disposition: form-data; name=\"imageFullsize\"\r\n\r\n{Image}\r\n" +
                 "------Line--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}
