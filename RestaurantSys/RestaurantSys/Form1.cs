using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region EmguCV_lib
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
#endregion

using System.Net.Http;
using System.Net;
using System.Collections.Specialized;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RestaurantSys
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AdopenFileDialog.InitialDirectory = System.IO.Path.Combine(Application.StartupPath, @"");
            if (AdopenFileDialog.ShowDialog() == DialogResult.OK)
                AdListBox.Items.Add(AdopenFileDialog.FileName);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (AdListBox.SelectedIndex != -1)
                AdListBox.Items.RemoveAt(AdListBox.SelectedIndex);
        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            Adtimer.Interval = int.Parse(AdIntervalText.Text);
        }

        private void Adtimer_Tick(object sender, EventArgs e)
        {

            if (AdListBox.Items.Count != 0)
            {
                if (!Global.bPlayingVideo)
                {
                    AdListBox.SelectedIndex = (AdListBox.SelectedIndex + 1) % AdListBox.Items.Count;
                    AdRotator.ImageLocation = AdListBox.SelectedItem.ToString();

                    int IsVedeo = AdRotator.ImageLocation.IndexOf(".avi");
                    if (IsVedeo >= 0)
                    {   // 遇到影片 必須改變 timer1.Interval 為 1/fps 並且停止更新 AdListBox.SelectedIndex 直到影片播完

                        Global.AdFrameGrabber = new Capture(AdRotator.ImageLocation);

                        //Get the frame rate
                        double rate = Global.AdFrameGrabber.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                        //Get the frame number
                        double FrameNum = Global.AdFrameGrabber.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

                        Global.AdtimerIntervalBuffer = Adtimer.Interval;
                        int NewInterval = (int)(1000/rate);
                        Adtimer.Interval = NewInterval;

                        Global.bPlayingVideo = true;

                    }
                    // AdRotator.ImageLocation = "https://i0.wp.com/free.com.tw/blog/wp-content/uploads/2014/08/Placekitten480-g.jpg?resize=640%2C480&ssl=1";
                    // AdRotator.ImageLocation = "C:\\Users\\user\\Desktop\\Out_just_track.avi";
                }
                else
                {
                    Image<Bgr, Byte> currentFrame;
                    Emgu.CV.Mat frame = null;

                    //read next frame if any
                    frame = Global.AdFrameGrabber.QueryFrame();

                    if (frame == null)
                    {
                        // 影片結束 回到輪播
                        //Close the video file. Not required since called by destructor
                        Global.AdFrameGrabber.Dispose();
                        Adtimer.Interval = Global.AdtimerIntervalBuffer;
                        Global.bPlayingVideo = false;
                    }
                    else
                    {
                        currentFrame = frame.ToImage<Bgr, byte>(false);
                        AdRotator.Image = currentFrame.Bitmap;
                    }
                }


            }
        }

        private void AdRotator_Click(object sender, EventArgs e)
        {
            if (TestPanel.Visible)
            {
                TestPanel.Visible = false;
            }
            else
            {
                TestPanel.Visible = true;
            }
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            // frm.Show();

            //設定Form2為Form1的上層，並開啟Form2視窗。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            frm.ShowDialog(this);
        }

        private string POST_GrapInfo(string a_URL, string a_Params)
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

            if(((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {   // 表示擷取成功 回傳200
                Result = responseFromServer;
            }
            return Result;
        }


        private async void LogInButton_ClickAsync(object sender, EventArgs e)
        {
            string POST = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/login";
            string system = "realtouchapp";
            string account = "ismyaki@gmail.com";
            string password = "123456";
            // string password = "123";

            #region HttpClient
            // Available in: .NET Framework 4.5+, .NET Standard 1.1+, .NET Core 1.0+

            var PostParams = new Dictionary<string, string>
            {
               { "system", system },
               { "account", account },
                { "password", password }
            };

            var content = new FormUrlEncodedContent(PostParams);
            var response = await client.PostAsync(POST, content);
            var StatusCodeString = response.StatusCode.ToString();
            var StatusCode = response.StatusCode.GetHashCode();

            if (response.StatusCode == HttpStatusCode.OK)
            {   // 成功才開始後續動作
                var responseString = response.Content.ReadAsStringAsync().Result;
                var JasonString = JsonConvert.DeserializeObject(responseString);

                MessageBox.Show(responseString);
                MessageBox.Show(JasonString.ToString());

                dynamic json = JValue.Parse(JasonString.ToString());

                // values require casting
                string name = json.name;
                string sessionID = json.sessionID;
                Global.MainSessionID = sessionID;

                // var results = JObject.Parse(json).SelectToken("results") as JArray;

            }
            else
            {   // 可編寫錯誤動作

            }
            #endregion
        }


        private void LogIn_HttpWebRequest_Click(object sender, EventArgs e)
        {
            string POST = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/login";
            string system = "realtouchapp";
            string account = "ismyaki@gmail.com";
            string password = "123456";

            var postData = "system=";
            postData += system + "&";
            postData += "account=";
            postData += account + "&";
            postData += "password=";
            postData += password;

            string PostResult = POST_GrapInfo(POST, postData);
            MessageBox.Show(PostResult);

            var JasonString = JsonConvert.DeserializeObject(PostResult);
            MessageBox.Show(JasonString.ToString());

            dynamic json = JValue.Parse(JasonString.ToString());

            // values require casting
            string name = json.name;
            string sessionID = json.sessionID;
            Global.MainSessionID = sessionID;
        }

        private void organizerNO_Button_Click(object sender, EventArgs e)
        {
            string POST = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/realtouch/getName";
            string system = "realtouchapp";

            var postData = "sessionID=";
            postData += Global.MainSessionID + "&";
            postData += "system=";
            postData += system;

            string PostResult = POST_GrapInfo(POST, postData);
            MessageBox.Show(PostResult);

            var JasonString = JsonConvert.DeserializeObject(PostResult);
            MessageBox.Show(JasonString.ToString());
        }
    }

    public class Global
    {   // 這裡擺放全域變數以供表單間溝通或是static變數需求
        public static bool bPlayingVideo = false;
        public static Capture AdFrameGrabber;
        public static int AdtimerIntervalBuffer = 0;
        public static string MainSessionID = "";
        public static string organizerNO = "";
    }
}
