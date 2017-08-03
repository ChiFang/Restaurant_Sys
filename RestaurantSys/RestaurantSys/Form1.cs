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
                        int NewInterval = (int)(1000 / rate);
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

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
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
                Global.SessionID = sessionID;



            }
            else
            {   // 可編寫錯誤動作

            }
            #endregion
        }


        private void LogIn_HttpWebRequest_Click(object sender, EventArgs e)
        {
            GetSessionID();
        }

        private void GetSessionID()
        {   // 這裡必須確定"帳號"&"密碼"都已設定好了
            var JasonString = GetJASONResult(POST_DATA_TYPE.LOGIN);
            dynamic json = JValue.Parse(JasonString.ToString());

            // values require casting
            string name = json.name;
            string sessionID = json.sessionID;
            Global.SessionID = sessionID;
        }

        private void GetOrganizerNO()
        {   // 這裡必須確定"SessionID"都已設定好了
            var JasonString = GetJASONResult(POST_DATA_TYPE.ORGANIZER_NO);

            #region With_List
            var JList = JObject.Parse(JasonString.ToString()).SelectToken("shopInfo").ToList();
            long cnt_JList = JList.LongCount();

            // 產生新表單給使用者選shop
            Form frmOrganizerNO = new Form();
            int m_btnWidth = 100;
            int m_btnHeight = 40;

            Panel panelTmp = new Panel();
            panelTmp.AutoScroll = true;
            frmOrganizerNO.Controls.Add(panelTmp);
            panelTmp.Size = new Size(m_btnWidth * 3, m_btnHeight * 2);

            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];
                var organizerNO_tmp = JItem.SelectToken("organizerNO");
                var name_tmp = JItem.SelectToken("name");
                Button btn = new Button();

                // 放在panel上
                panelTmp.Controls.Add(btn);

                btn.Left = m_btnWidth * cnt;
                btn.Top = 0;
                btn.Width = m_btnWidth;
                btn.Height = m_btnHeight;

                btn.Text = name_tmp.ToString();
                btn.Name = organizerNO_tmp.ToString();

                btn.Click += new EventHandler(organizerNO_Selcet);
            }
            frmOrganizerNO.Show();
            #endregion

            #region With_JArray
            //var albums = JObject.Parse(JasonString.ToString()).SelectToken("shopInfo") as JArray;
            //long cnt_JArray = albums.LongCount();
            //foreach (dynamic album in albums)
            //{
            //    Console.WriteLine(album.organizerNO);
            //    //foreach (dynamic song in album.Songs)
            //    //{
            //    //    Console.WriteLine("\t" + song.SongName);
            //    //}
            //}
            #endregion
        }


        private void GetSystemID()
        {   // 這裡必須確定"SessionID"&"organizerNO"都已設定好了
            var JasonString = GetJASONResult(POST_DATA_TYPE.NEW_LIST);

            #region With_List
            var JList = JObject.Parse(JasonString.ToString()).SelectToken("system").ToList();
            long cnt_JList = JList.LongCount();

            // 尋找keyword = "ADEBOARD" 的輪播 systemID
            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];
                var Keyword_tmp = JItem.SelectToken("keyword");

                if (Keyword_tmp.ToString() == Global.ADKEYWORD)
                {
                    Global.systemID = JItem.SelectToken("systemID").ToString();
                }
            }
            #endregion
        }

        private void GetAD_Content()
        {   // 這裡必須確定"SessionID"&"organizerNO"&"systemID"都已設定好了
            var JasonString = GetJASONResult(POST_DATA_TYPE.NEW_CONTENT);

            if (Directory.Exists(Global.TempDatadirPath))
            {
                Console.WriteLine("The directory {0} already exists.", Global.TempDatadirPath);
            }
            else
            {
                Directory.CreateDirectory(Global.TempDatadirPath);
                Console.WriteLine("The directory {0} was created.", Global.TempDatadirPath);
            }

            // 建立檔案串流（@ 可取消跳脫字元 escape sequence） for log
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"AD_Content_Log.txt");
            sw.WriteLine(JasonString.ToString());               // 寫入文字
            sw.Close();                                         // 關閉串流

            #region With_List

            // 抓取 Jason 中的 "news"內容清單並轉成List
            var JList = JObject.Parse(JasonString.ToString()).SelectToken("news").ToList();

            // 取得清單長度
            long cnt_JList = JList.LongCount();

            // 創建相對應的結構
            Global.atAD_ContentInfo = new ADInfo[cnt_JList];

            // 將每組輪播資料載入結構
            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];

                Global.atAD_ContentInfo[cnt].Sort = Convert.ToInt32(JItem.SelectToken("sort").ToString());
                Global.atAD_ContentInfo[cnt].name = JItem.SelectToken("name").ToString();
                Global.atAD_ContentInfo[cnt].newsID = JItem.SelectToken("newsID").ToString();
                Global.atAD_ContentInfo[cnt].Type = JItem.SelectToken("type").ToString();
                if (IsMovie(JItem))
                {   // it's movie  
                    Global.atAD_ContentInfo[cnt].URL = JItem.SelectToken("movie").ToString();
                    Global.atAD_ContentInfo[cnt].URLThumb = JItem.SelectToken("pic").ToString();

                    Global.atAD_ContentInfo[cnt].FilenameExtension = ".mp4";
                }
                else
                {   // it's photo
                    Global.atAD_ContentInfo[cnt].URL = JItem.SelectToken("pic").ToString();
                    Global.atAD_ContentInfo[cnt].URLThumb = JItem.SelectToken("picThumb").ToString();
                    Global.atAD_ContentInfo[cnt].FilenameExtension = ".jpg";
                }
            }
            #endregion

            // sort array by "Sort" element
            Array.Sort(Global.atAD_ContentInfo, delegate (ADInfo item1, ADInfo item2)
            {
                return item1.Sort.CompareTo(item2.Sort); // (user1.Sort - user2.Sort)
            });
        }

        private void DownLoad_AD_File()
        {   // 這裡必須確定廣告資料都已載入

            // start to download all ad element
            for (int cnt = 0; cnt < Global.atAD_ContentInfo.Length; cnt++)
            {
                DownloadFile
                    (Global.atAD_ContentInfo[cnt].URL,
                    Global.TempDatadirPath + Global.atAD_ContentInfo[cnt].name + Global.atAD_ContentInfo[cnt].FilenameExtension
                    );
            }
        }

        private void organizerNO_Button_Click(object sender, EventArgs e)
        {
            GetOrganizerNO();
        }



        private void organizerNO_Selcet(object sender, EventArgs e)
        {
            Global.organizerNO = ((Button)sender).Name;

            if (Global.DEBUG_FLAG > 1)
            {
                // for debug
                MessageBox.Show("organizerNO = " + ((Button)sender).Name);
            }

            // close its form after select
            ((Button)sender).FindForm().Close();
        }

        public enum POST_DATA_TYPE
        {
            LOGIN = 0,
            ORGANIZER_NO = 1,
            NEW_LIST = 2,
            NEW_CONTENT = 3,
            CATEGORY = 4,
            PRODUCT = 5
        }

        private string SetURL(POST_DATA_TYPE a_Type)
        {
            string URL = "";

            switch (a_Type)
            {
                case POST_DATA_TYPE.LOGIN:
                    URL = Global.URL_LogIn;
                    break;
                case POST_DATA_TYPE.ORGANIZER_NO:
                    URL = Global.URL_OrganizerNO;
                    break;
                case POST_DATA_TYPE.NEW_LIST:
                    URL = Global.URL_NewsList;
                    break;
                case POST_DATA_TYPE.NEW_CONTENT:
                    URL = Global.URL_NewsContent;
                    break;
                case POST_DATA_TYPE.CATEGORY:
                    URL = Global.URL_CategoryList;
                    break;
                case POST_DATA_TYPE.PRODUCT:
                    URL = Global.URL_ProductList;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return URL;
        }

        private string SetPostData(POST_DATA_TYPE a_Type)
        {
            string postData = "";
            
            switch (a_Type)
            {
                case POST_DATA_TYPE.LOGIN:
                    postData += "system=";
                    postData += Global.System + "&";
                    postData += "account=";
                    postData += Global.Account + "&";
                    postData += "password=";
                    postData += Global.Password;
                    break;
                case POST_DATA_TYPE.ORGANIZER_NO:
                    postData += "sessionID=";
                    postData += Global.SessionID + "&";
                    postData += "system=";
                    postData += Global.System;
                    break;
                case POST_DATA_TYPE.NEW_LIST:
                    postData += "sessionID=";
                    postData += Global.SessionID + "&";
                    postData += "organizerNO=";
                    postData += Global.organizerNO;
                    break;
                case POST_DATA_TYPE.NEW_CONTENT:
                    postData += "sessionID=";
                    postData += Global.SessionID + "&";
                    postData += "organizerNO=";
                    postData += Global.organizerNO + "&";
                    postData += "systemID=";
                    postData += Global.systemID;
                    break;
                case POST_DATA_TYPE.CATEGORY:
                    postData += "sessionID=";
                    postData += Global.SessionID + "&";
                    postData += "organizerNO=";
                    postData += Global.organizerNO;
                    break;
                case POST_DATA_TYPE.PRODUCT:
                    postData += "sessionID=";
                    postData += Global.SessionID + "&";
                    postData += "organizerNO=";
                    postData += Global.organizerNO;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return postData;
        }

        private object GetJASONResult(POST_DATA_TYPE a_Type)
        {
            object JASONResult = "";

            string POST = SetURL(a_Type);
            string postData = SetPostData(a_Type);
            string PostResult = POST_GrapInfo(POST, postData);
            var JasonString = JsonConvert.DeserializeObject(PostResult);
            JASONResult = JsonConvert.DeserializeObject(PostResult);


            if (Global.DEBUG_FLAG > 1)
            {
                // for debug
                MessageBox.Show(PostResult);
                MessageBox.Show(JasonString.ToString());
            }

            return JASONResult;
        }


        private void NewsListButton_Click(object sender, EventArgs e)
        {
            GetSystemID();
        }

        private void NewsContentButton_Click(object sender, EventArgs e)
        {
            GetAD_Content();

            DownLoad_AD_File();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All AD file are downloaded.");
            }
        }

        private bool IsMovie(JToken a_JItem)
        {
            if (a_JItem.SelectToken("movie") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DownloadFile(string a_RemoteURL, string a_FileName)
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

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            // Global.DEBUG_FLAG = 1;


            // step 0: 登入 >> 必須輸入帳號、密碼

            // step 1: 取得 SessionID
            GetSessionID();

            // step 2: 取得 OrganizerNO
            GetOrganizerNO();

            // step 3: 取得 SystemID >> keyword = "ADEBOARD" 的輪播 systemID
            GetSystemID();

            // step 4: 取得輪播內容
            GetAD_Content();

            // step 5: 下載所有輪播檔案
            DownLoad_AD_File();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All AD file are downloaded.");
            }
        }
    }

    /// <summary> ad info </summary>
    public struct ADInfo
    {
        public int Sort;
        public string Type;

        public string URL;
        public string URLThumb;
        public string name;
        public string newsID;

        public string FilenameExtension;


        /// <summary> reset coordinate </summary>
        public void Init()
        {
            Sort = 0;
            Type = "0";
        }
    }

    public class Global
    {   // 這裡擺放全域變數以供表單間溝通或是static變數需求
        public static bool bPlayingVideo = false;
        public static Capture AdFrameGrabber;
        public static int AdtimerIntervalBuffer = 0;
        public static ADInfo[] atAD_ContentInfo = null;
        public static string TempDatadirPath = Application.StartupPath + @"\temp_file\";

        public static int DEBUG_FLAG = 2;

        // by user input
        public static string System = "realtouchapp";
        public static string Account = "ismyaki@gmail.com";
        public static string Password = "123456";

        // by Web
        public static string SessionID = "";
        public static string organizerNO = "";
        public static string systemID = "";

        // const
        public const string ADKEYWORD = "ADEBOARD";
        public const string URL_LogIn = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/login";
        public const string URL_OrganizerNO = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/realtouch/getName";
        public const string URL_NewsList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/info/news/system";
        public const string URL_NewsContent = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/info/news/list";
        public const string URL_CategoryList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/product/category/list";
        public const string URL_ProductList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/product/product/list";


        public void DetailMenuClick(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Text);
        }

        public void GenButtonDynamic(Global test_o, Panel a_panel, int a_ButtomNum, int a_BtnWidth = 100, int a_BtnHeight = 30, bool a_Mode = true)
        {
            int WidthLimit = a_panel.Size.Width;
            int HeightLimit = a_panel.Size.Height;
            int RowIndex = 0, ColIndex = 0;

            for (int i = 0; i < a_ButtomNum; i++)
            {
                Button btn = new Button();
                a_panel.Controls.Add(btn);
                btn.Width = a_BtnWidth;
                btn.Height = a_BtnHeight;

                if (a_Mode)
                {
                    btn.Left = a_BtnWidth * i;
                    btn.Top = 0;
                    btn.Text = i.ToString();
                    btn.Click += new EventHandler(test_o.DetailMenuClick);
                }
                else
                {
                    if (a_BtnWidth * ColIndex >= WidthLimit)
                    {
                        ColIndex = 0;
                        RowIndex++;
                    }

                    btn.Left = a_BtnWidth * ColIndex;
                    btn.Top = RowIndex * a_BtnHeight;
                    btn.Text = i.ToString();
                    // btn.Click += new EventHandler(DetailMenuClick);

                    ColIndex++;
                }

            }
        }

    }
}
