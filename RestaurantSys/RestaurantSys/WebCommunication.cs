using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // for MessageBox
using System.Drawing; // for Size


using System.Net.Http;
using System.Net;
using System.Collections.Specialized;

using System.IO;



namespace RestaurantSys
{
    class WebCommunication
    {
        /// <summary>
        /// 利用POST像網頁取得資訊
        /// </summary>
        /// <param name="a_URL">[IN] 網頁 URL</param>
        /// <param name="a_Params">[IN] 輸入參數</param>
        public static string POST_GrapInfo(string a_URL, string a_Params)
        {   // HttpWebRequest Method 
            string Result = "";

            var request = (HttpWebRequest)WebRequest.Create(a_URL);

            byte[] byteArray = Encoding.UTF8.GetBytes(a_Params);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content.
            // Console.WriteLine(responseFromServer);

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {   // 表示擷取成功 回傳200
                Result = responseFromServer;
            }
            return Result;
        }


        /// <summary>
        /// 從網路下載檔案
        /// </summary>
        /// <param name="a_RemoteURL">[IN] 網頁 URL</param>
        /// <param name="a_FileName">[IN] 檔案名稱(包含路徑)</param>
        public static void DownloadFile(string a_RemoteURL, string a_FileName)
        {
            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();

            // Concatenate the domain with the Web resource filename.
            Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", a_FileName, a_RemoteURL);

            // Download the Web resource and save it into the current filesystem folder.
            myWebClient.DownloadFile(a_RemoteURL, a_FileName);
            Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", a_FileName, a_RemoteURL);
            Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t" + Application.StartupPath);
        }

        

    }
}
