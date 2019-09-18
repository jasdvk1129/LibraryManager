using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static NetworkCommsDotNet.Tools.StreamTools;
using ErrorMessageLibrary;

namespace TCPClientServerLibrary
{
    public class TCPClientServerClass
    {
        public ErrorMessageClass ErrMsg { get; set; } = new ErrorMessageClass() { _WorkPath=AppDomain.CurrentDomain.BaseDirectory };
        #region TCPClient
        /// <summary>
        /// 傳送訊息
        /// </summary>
        /// <param name="Token">權杖</param>
        /// <param name="IPaddress">IP位址</param>
        /// <param name="Port">Port號</param>
        /// <param name="Message">訊息內容</param>
        public void MessageFunction(string Token, string IPaddress, int Port, string Message)
        {
            try
            {
                NetworkComms.SendObject(Token, IPaddress, Port, Message);
            }
            catch (ArgumentException ex) { ErrMsg._errorText("網路通訊異常請檢查", ex); Console.WriteLine("網路通訊異常請檢查"); }
            catch (ConnectionSetupException ex) { ErrMsg._errorText("網路通訊異常請檢查", ex); Console.WriteLine("網路通訊異常請檢查"); }
            catch (ConnectionSendTimeoutException ex) { ErrMsg._errorText("接收端程式異常", ex); Console.WriteLine("接收端程式異常"); }
        }
        /// <summary>
        /// 傳送檔案
        /// </summary>
        /// <param name="Token">權杖</param>
        /// <param name="IPaddress">IP位址</param>
        /// <param name="Port">Port號</param>
        /// <param name="FilePath">檔案路徑</param>
        public void FileFunction(string Token, string IPaddress, int Port, string FilePath)
        {
            try
            {
                FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader streamReader = new StreamReader(fileStream);
                DataTable dataTable = new DataTable();
                bool IsFirst = true;
                string strline = null;
                while ((strline = streamReader.ReadLine()) != null)
                {
                    if (IsFirst)
                    {

                        try
                        {
                            ThreadSafeStream threadSafeStream = new ThreadSafeStream(fileStream);
                            StreamSendWrapper sendWrapper = new StreamSendWrapper(threadSafeStream, 0, fileStream.Length);
                            NetworkComms.SendObject(Token, IPaddress, Port, sendWrapper);
                            fileStream.Close();
                        }
                        catch (ArgumentException ex) { ErrMsg._errorText("網路通訊異常請檢查", ex); Console.WriteLine("網路通訊異常請檢查"); }
                        catch (ConnectionSetupException ex) { ErrMsg._errorText("網路通訊異常請檢查", ex); Console.WriteLine("網路通訊異常請檢查"); }
                        catch (ConnectionSendTimeoutException ex) { ErrMsg._errorText("接收端程式異常", ex); Console.WriteLine("接收端程式異常"); }
                    }
                    break;
                }
            }
            catch (FileNotFoundException ex) { ErrMsg._errorText("搜尋不到或未有檔案產生", ex); Console.WriteLine("搜尋不到或未有檔案產生"); }
            catch (ObjectDisposedException ex) { ErrMsg._errorText("檔案刪除後未有檔案產生", ex); Console.WriteLine("檔案刪除後未有檔案產生"); }
            catch (DirectoryNotFoundException ex) { ErrMsg._errorText("找不到檔案路徑", ex); Console.WriteLine("找不到檔案路徑"); }
        }
        #endregion

        #region TCPServer
        /// <summary>
        /// 訊息內容
        /// </summary>
        public string MessageString { get; set; } = null;
        /// <summary>
        /// 檔案儲存位址
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 辦別Token
        /// </summary>
        public string Tokennum { get; set; }
        /// <summary>
        /// 顯示IP位址與PORT
        /// </summary>
        public string IPstring { get; set; }
        /// <summary>
        /// 開啟TCP連線
        /// </summary>
        /// <param name="Port">設定PORT</param>
        public void ConnectionTCP(int Port)
        {
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, Port));
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            {
                IPstring += "IP位址: " + localEndPoint.Address + "\r\n Port: " + localEndPoint.Port.ToString() + "\r\n";
            }
            WebClient webClient = new WebClient();
            string publicIP = webClient.DownloadString("https://api.ipify.org");
            IPstring += "外部IP:" + publicIP + "\r\n Port:" + Port;
        }
        #region 接收訊息解析
        /// <summary>
        /// 接收訊息
        /// </summary>
        /// <param name="Token">權杖</param>
        public void MessageFunction(string Token)
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<string>(Token, PrintIncomingMessage);
        }
        private void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            MessageString = message;
            Tokennum = header.PacketType;
        }
        #endregion
        #region 接收檔案解析
        /// <summary>
        /// 接收檔案
        /// </summary>
        /// <param name="Token">權杖</param>
        public void FileFunction(string Token)
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<byte[]>(Token, FieldLoad);
        }
        private void FieldLoad(PacketHeader packetHeader, Connection connection, byte[] incomingData)
        {
            FileStream fileStream = File.Create(FilePath, incomingData.Length);
            fileStream.Write(incomingData, 0, incomingData.Length);
            fileStream.Close();
            Tokennum = packetHeader.PacketType;
        }
        #endregion
        #endregion
    }
}
