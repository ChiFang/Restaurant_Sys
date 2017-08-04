using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // for MessageBox
using System.Drawing; // for Size

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
    public enum POST_DATA_TYPE
    {
        LOGIN = 0,
        ORGANIZER_NO = 1,
        NEW_LIST = 2,
        NEW_CONTENT = 3,
        CATEGORY = 4,
        PRODUCT = 5
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

    /// <summary>  Category info </summary>
    public struct CategoryInfo
    {
        public int Sort;
        public double service;
        public double discount;

        public string type;
        public string categoryID;
        public string categoryName;
        public string categoryForeignName;
        public string code;

        public string parentID;
        public string parentIDs;
        public string categoryImage;
        public string categoryImageThumb;


        /// <summary> reset coordinate </summary>
        public void Init()
        {
            Sort = 0;
            type = "0";
        }
    }

    /// <summary>  Product info </summary>
    public struct ProductInfo
    {
        public int Sort;
        public double service;
        public double discount;

        public string type;
        public string productType;
        public string productID;
        public string productName;
        public string productForeignName;

        public string productShortName;
        public string isCurrentPrice;
        public string commission;
        public string note;


        /// <summary> reset coordinate </summary>
        public void Init()
        {
         
        }
    }

    public class Global
    {   // 這裡擺放全域變數以供表單間溝通或是static變數需求
        public static bool bPlayingVideo = false;
        public static Capture AdFrameGrabber;
        public static int AdtimerIntervalBuffer = 0;
        public static ADInfo[] atAD_ContentInfo = null;
        public static CategoryInfo[] atCategoryInfo = null;
        public static ProductInfo[] atProductInfo = null;
        public static string TempDatadirPath = Application.StartupPath + @"\temp_file\";

        public static int DEBUG_FLAG = 1;

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

    class WebCommunication
    {
        private static string SetURL(POST_DATA_TYPE a_Type)
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

        private static string SetPostData(POST_DATA_TYPE a_Type)
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

        private static string POST_GrapInfo(string a_URL, string a_Params)
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


        public static object GetJASONResult(POST_DATA_TYPE a_Type)
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

        public static void GetSessionID()
        {   // 這裡必須確定"帳號"&"密碼"都已設定好了
            var JasonString = GetJASONResult(POST_DATA_TYPE.LOGIN);
            dynamic json = JValue.Parse(JasonString.ToString());

            // values require casting
            string name = json.name;
            string sessionID = json.sessionID;
            Global.SessionID = sessionID;
        }

        private static void organizerNO_Selcet(object sender, EventArgs e)
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

        public static void GetOrganizerNO()
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


        public static void GetSystemID()
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

        public static void DownLoad_AD_File()
        {   // 這裡必須確定廣告資料都已載入

            // 建立檔案串流（@ 可取消跳脫字元 escape sequence） for log
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"AD_PlayList_Log.txt");

            // start to download all ad element
            for (int cnt = 0; cnt < Global.atAD_ContentInfo.Length; cnt++)
            {
                string TempADName = Global.TempDatadirPath + Global.atAD_ContentInfo[cnt].name + Global.atAD_ContentInfo[cnt].FilenameExtension;
                DownloadFile(Global.atAD_ContentInfo[cnt].URL, TempADName);
                sw.WriteLine(TempADName); // 寫入文字
                
            }

            sw.Close(); // 關閉串流
        }

        private static bool IsMovie(JToken a_JItem)
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

        public static void GetAD_Content()
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

        public static void Get_CategoryContent()
        {   // 這裡必須確定"SessionID"&"organizerNO"都已設定好了

            // Global.DEBUG_FLAG = 1;

            var JasonString = GetJASONResult(POST_DATA_TYPE.CATEGORY);

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
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"CategoryContent_Log.txt");
            sw.WriteLine(JasonString.ToString());               // 寫入文字
            sw.Close();                                         // 關閉串流

            #region With_List

            // 抓取 Jason 中的 "category"內容清單並轉成List
            var JList = JObject.Parse(JasonString.ToString()).SelectToken("category").ToList();

            // 取得category長度
            long cnt_JList = JList.LongCount();

            // 創建相對應的結構
            Global.atCategoryInfo = new CategoryInfo[cnt_JList];

            // 將每組輪播資料載入結構
            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];

                Global.atCategoryInfo[cnt].Sort = Convert.ToInt32(JItem.SelectToken("sort").ToString());

                Global.atCategoryInfo[cnt].type = JItem.SelectToken("type").ToString();
                Global.atCategoryInfo[cnt].categoryID = JItem.SelectToken("categoryID").ToString();
                Global.atCategoryInfo[cnt].categoryName = JItem.SelectToken("categoryName").ToString();
                Global.atCategoryInfo[cnt].categoryForeignName = JItem.SelectToken("categoryForeignName").ToString();
                Global.atCategoryInfo[cnt].code = JItem.SelectToken("code").ToString();
                Global.atCategoryInfo[cnt].parentID = JItem.SelectToken("parentID").ToString();
                Global.atCategoryInfo[cnt].parentIDs = JItem.SelectToken("parentIDs").ToString();

                //if(cnt == 7)
                //{
                //    var ttt = JItem.SelectToken("discount");
                //}
                
                Global.atCategoryInfo[cnt].discount = Convert.ToDouble(JItem.SelectToken("discount").ToString());
                Global.atCategoryInfo[cnt].service = Convert.ToDouble(JItem.SelectToken("service").ToString());
                Global.atCategoryInfo[cnt].categoryImage = JItem.SelectToken("categoryImage").ToString();
                Global.atCategoryInfo[cnt].categoryImageThumb = JItem.SelectToken("categoryImageThumb").ToString();


            }
            #endregion

            // sort array by "Sort" element
            Array.Sort(Global.atCategoryInfo, delegate (CategoryInfo item1, CategoryInfo item2)
            {
                return item1.Sort.CompareTo(item2.Sort); // (user1.Sort - user2.Sort)
            });
        }

        public static void Get_Product()
        {   // 這裡必須確定"SessionID"&"organizerNO"都已設定好了

            // Global.DEBUG_FLAG = 1;

            var JasonString = GetJASONResult(POST_DATA_TYPE.PRODUCT);

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
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"Product_Log.txt");
            sw.WriteLine(JasonString.ToString());               // 寫入文字
            sw.Close();                                         // 關閉串流

            #region With_List

            // 抓取 Jason 中的 "product"內容清單並轉成List
            var JList = JObject.Parse(JasonString.ToString()).SelectToken("product").ToList();

            // 取得category長度
            long cnt_JList = JList.LongCount();

            // 創建相對應的結構
            Global.atProductInfo = new ProductInfo[cnt_JList];

            // 將每組輪播資料載入結構
            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];
            }
            #endregion
        }

        public static void DownLoad_Category_File()
        {   // 這裡必須確定Category資料都已載入

            // start to download all ad element
            for (int cnt = 0; cnt < Global.atCategoryInfo.Length; cnt++)
            {
                if (Global.atCategoryInfo[cnt].categoryImage != "" && Global.atCategoryInfo[cnt].categoryImageThumb != "")
                {
                    DownloadFile
                        (Global.atCategoryInfo[cnt].categoryImage,
                        Global.TempDatadirPath + Global.atCategoryInfo[cnt].categoryName + ".jpg"
                        );

                    DownloadFile
                        (Global.atCategoryInfo[cnt].categoryImageThumb,
                        Global.TempDatadirPath + Global.atCategoryInfo[cnt].categoryName + "Thumb.jpg"
                        );
                }
                else
                {
                    MessageBox.Show("Category " + cnt.ToString() + " URL is null");
                }
            }
        }
    }
}
