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

    /// <summary> 
    /// the enum of POST data type</summary>
    public enum POST_DATA_TYPE
    {
        LOGIN = 0,
        ORGANIZER_NO = 1,
        NEW_LIST = 2,
        NEW_CONTENT = 3,
        CATEGORY = 4,
        PRODUCT = 5
    }

    /// <summary> the struct of ad info </summary>
    public struct ADInfo
    {
        /// <summary> 排序 </summary>
        public int Sort;

        /// <summary> 0:新增   1:刪除 </summary>
        public string Type;

        /// <summary> 圖片或影片網址 </summary>
        public string URL;

        /// <summary> 縮圖網址 </summary>
        public string URLThumb;

        /// <summary> 最新消息標題 </summary>
        public string name;

        /// <summary> 最新消息ID </summary>
        public string newsID;

        /// <summary> 副檔名: 區分圖片影片用 </summary>
        public string FilenameExtension;


        /// <summary> reset struct </summary>
        public void Init()
        {
            Sort = 0;
            Type = "0";
        }
    }

    /// <summary>  the struct of  Category info </summary>
    public struct CategoryInfo
    {
        /// <summary> 排序(數字越小排在前) </summary>
        public int Sort;

        /// <summary> 服務費 </summary>
        public double service;

        /// <summary> 打折折數 </summary>
        public double discount;

        /// <summary> 0:新增   1:刪除 </summary>
        public string type;

        /// <summary> 分類ID </summary>
        public string categoryID;

        /// <summary> 分類名稱 </summary>
        public string categoryName;

        /// <summary> 外語名稱 </summary>
        public string categoryForeignName;

        /// <summary> 分類碼 </summary>
        public string code;

        /// <summary> 父層分類ID(第一層為0) </summary>
        public string parentID;

        /// <summary> 所有父層分類ID(第一層為空字串) </summary>
        public string parentIDs;

        /// <summary> 分類圖片 </summary>
        public string categoryImage;

        /// <summary> 分類圖片縮圖  </summary>
        public string categoryImageThumb;

        /// <summary> 子分類陣列: for 巢狀分類 </summary>
        public CategoryInfo[] SubCategory; 


        /// <summary> reset the struct </summary>
        public void Init()
        {
            Sort = 0;
            type = "0";
        }
    }

    /// <summary>  the struct of Product info </summary>
    public struct ProductInfo
    {
        /// <summary> 0:新增   1:刪除 </summary>
        public string type;

        /// <summary> 0:商品; 1:包裝 </summary>
        public string productType;

        /// <summary> 商品規格ID(包裝ID) </summary>
        public string productID;

        /// <summary> 商品名稱 </summary>
        public string productName;

        /// <summary> 外文名稱 </summary>
        public string productForeignName;

        /// <summary> 商品簡稱 </summary>
        public string productShortName;

        /// <summary> 以時價計算 0:否 1:是 </summary>
        public string isCurrentPrice;

        /// <summary> 外部公司抽成(%) </summary>
        public string commission;

        /// <summary> 備註 </summary>
        public string note;

        /// <summary> 說明(HTML) </summary>
        public string content;

        /// <summary> 分類(Array) </summary>
        public string[] category;

        /// <summary> 商品排序(主排序)(數字越小排在前) </summary>
        public string productSort;

        /// <summary> 格排序(次排序)(數字越小排在前) </summary>
        public string specSort;

        /// <summary> 折抵點數 </summary>
        public string redeemedPoint;

        /// <summary> 條碼 </summary>
        public string barcode;

        /// <summary> 商品原價  </summary>
        public double oriPrice;

        /// <summary> 商品價錢 </summary>
        public double price;

        /// <summary> 商品圖片 </summary>
        public string productImage;

        /// <summary> 商品圖片縮圖 </summary>
        public string productImageThumb;

        /// <summary> 商品組合(字串all或是Array) </summary>
        public string combine;

        /// <summary> 商品註記(Array) </summary>
        public string mark;

        /// <summary> 預設選項 </summary>
        public string DefaultOption;


        /// <summary> reset the struct </summary>
        public void Init()
        {

        }
    }

    /// <summary>  the class for Global variable or function </summary>
    public class Global
    {   // 這裡擺放全域變數以供表單間溝通或是static變數需求

        #region ForCoding
        /// <summary> 是否在輪播影片 </summary>
        public static bool bPlayingVideo = false;

        /// <summary> emguCV 抓取影片的結構 </summary>
        public static Capture AdFrameGrabber;

        /// <summary> 輪播時間間隔暫存: 撥放影片時間隔得是 1/fps 所以要暫存下來 </summary>
        public static int AdtimerIntervalBuffer = 0;

        /// <summary> 廣告輪播清單 </summary>
        public static ADInfo[] atAD_ContentInfo = null;

        /// <summary> 產品分類清單 </summary>
        public static CategoryInfo[] atCategoryInfo = null;

        /// <summary> 產品分類清單: 階層化後 </summary>
        public static CategoryInfo[] atCategoryInfoHierarchical = null;

        /// <summary> 產品清單 </summary>
        public static ProductInfo[] atProductInfo = null;

        /// <summary> 程式暫存檔案路徑 </summary>
        public static string TempDatadirPath = Application.StartupPath + @"\temp_file\";

        /// <summary> Ddbug flag </summary>
        public static int DEBUG_FLAG = 1;
        #endregion

        #region UserInput
        /// <summary> 系統名稱 </summary>
        public static string System = "realtouchapp";

        /// <summary> 帳號 </summary>
        public static string Account = "ismyaki@gmail.com";

        /// <summary> 密碼 </summary>
        public static string Password = "123456";
        #endregion

        #region ByWeb
        /// <summary> 驗證身份用，代表有效登入，之後要資料需一起傳到後台 </summary>
        public static string SessionID = "";

        /// <summary> 廠商編號 </summary>
        public static string organizerNO = "";

        /// <summary> systemID: 尋找keyword = "ADEBOARD" 的輪播 systemID </summary>
        public static string systemID = "";
        #endregion

        #region ConstVariable
        /// <summary> systemID: 尋找keyword = "ADEBOARD" 的輪播 systemID </summary>
        public const string ADKEYWORD = "ADEBOARD";

        /// <summary> 登入網址 </summary>
        public const string URL_LogIn = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/login";

        /// <summary> 廠商編號網址 </summary>
        public const string URL_OrganizerNO = "http://dev.realtouchapp.com/api/v1/windows/zh-Hant/realtouch/getName";

        /// <summary> 消息清單網址 </summary>
        public const string URL_NewsList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/info/news/system";

        /// <summary> 消息內容網址 </summary>
        public const string URL_NewsContent = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/info/news/list";

        /// <summary> 產品分類網址 </summary>
        public const string URL_CategoryList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/product/category/list";

        /// <summary> 產品網址 </summary>
        public const string URL_ProductList = "http://dev.realtouchapp.com/api/business/v1/windows/zh-Hant/product/product/list";
        #endregion


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


    /// <summary>  the class for Realtouch Board function </summary>
    class RealtouchBoard
    {

        /// <summary>
        /// Set URL
        /// </summary>
        /// <param name="a_Type">[IN] POST data type</param>
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

        /// <summary>
        /// Set PostData
        /// </summary>
        /// <param name="a_Type">[IN] POST data type</param>
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

        /// <summary>
        /// Get SessionID
        /// </summary>
        /// <remarks> 
        /// 這裡必須確定"帳號"&"密碼"都已設定好了 </remarks>
        public static void GetSessionID()
        {
            var JasonString = GetJASONResult(POST_DATA_TYPE.LOGIN);
            dynamic json = JValue.Parse(JasonString.ToString());

            // values require casting
            string name = json.name;
            string sessionID = json.sessionID;
            Global.SessionID = sessionID;

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
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"SessionID.txt");
            sw.WriteLine(Global.SessionID);               // 寫入文字
            sw.Close();                                         // 關閉串流
        }


        /// <summary>
        /// 選擇廠商
        /// </summary>
        /// <remarks> 
        /// 選擇廠商並取得廠商編號"organizerNO" </remarks>
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
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"OrganizerNO.txt");
            sw.WriteLine(Global.organizerNO);               // 寫入文字
            sw.Close();                                         // 關閉串流
        }


        /// <summary>
        /// Get JASON result
        /// </summary>
        /// <param name="a_Type">[IN] POST data type</param>
        public static object GetJASONResult(POST_DATA_TYPE a_Type)
        {
            object JASONResult = "";

            string POST = SetURL(a_Type);
            string postData = SetPostData(a_Type);
            string PostResult = WebCommunication.POST_GrapInfo(POST, postData);
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


        /// <summary>
        /// select OrganizerNO by generating a form with bottun: 這裡必須確定"SessionID"都已設定好了
        /// </summary>
        public static void GetOrganizerNO()
        {
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

        /// <summary>
        /// get SystemID: 這裡必須確定"SessionID"&"organizerNO"都已設定好了
        /// </summary>
        public static void GetSystemID()
        {
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


        /// <summary>
        /// juge if the jason item is movie: for AD info
        /// </summary>
        /// <param name="a_JItem">[IN] jason item</param>
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

        /// <summary>
        ///  Get AD Content
        /// </summary>
        /// <remarks> 
        /// 取得廣告內容載入結構、保存並且排序: 這裡必須確定"SessionID"&"organizerNO"&"systemID"都已設定好了 </remarks>
        public static void GetAD_Content()
        {
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


        /// <summary>
        ///  Get Category Content
        /// </summary>
        /// <remarks> 
        /// 取得產品分類內容載入結構、保存: 這裡必須確定"SessionID"&"organizerNO"都已設定好了 </remarks>
        public static void Get_CategoryContent()
        {
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
                Global.atCategoryInfo[cnt].discount = Convert.ToDouble(JItem.SelectToken("discount").ToString());
                Global.atCategoryInfo[cnt].service = Convert.ToDouble(JItem.SelectToken("service").ToString());
                Global.atCategoryInfo[cnt].categoryImage = JItem.SelectToken("categoryImage").ToString();
                Global.atCategoryInfo[cnt].categoryImageThumb = JItem.SelectToken("categoryImageThumb").ToString();
            }
            #endregion

            // sort array by "Sort" element
            //Array.Sort(Global.atCategoryInfo, delegate (CategoryInfo item1, CategoryInfo item2)
            //{
            //    return item1.Sort.CompareTo(item2.Sort); // (user1.Sort - user2.Sort)
            //});
        }


        /// <summary>
        /// 將產品分類結構階層化: 為了處理巢狀問題(只處裡兩層&一個父分類)
        /// </summary>
        /// <param name="a_atCategoryInfo">[IN] 產品分類結構清單</param>
        public static CategoryInfo[] MakeCategoryInfoHierarchical_two_layer_one_parentID(CategoryInfo[] a_atCategoryInfo)
        {   // 概念是從最底層往上接
            CategoryInfo[] atCategoryInfoModified = null;

            List<CategoryInfo> List = new List<CategoryInfo>(Global.atCategoryInfo);

            int IndexTmp_1 = List.FindIndex(x => x.categoryName == "湯品");
            IndexTmp_1 = List.FindIndex(x => x.categoryID == "397");


            for (int cnt = 0; cnt < List.Count; cnt++)
            {
                if (List[cnt].parentID != "0")
                {   // 表示不是最上層

                    // 將產品分類加入其父分類
                    List =  AddToParentCategory(List, cnt);

                    cnt--; // 因為刪除當前的類別 所以下一個類別會變成目前的

                }
            }
            atCategoryInfoModified = List.ToArray();
            return atCategoryInfoModified;
        }

        /// <summary>
        /// 將產品分類結構階層化: 為了處理巢狀問題(只處裡多層&一個父分類)
        /// </summary>
        /// <param name="a_atCategoryInfo">[IN] 產品分類結構清單</param>
        public static CategoryInfo[] MakeCategoryInfoHierarchical_multi_layer_one_parentID(CategoryInfo[] a_atCategoryInfo)
        {   // 概念是從最底層往上接
            CategoryInfo[] atCategoryInfoModified = null;

            List<CategoryInfo> List = new List<CategoryInfo>(Global.atCategoryInfo);

            int IndexTmp_1 = List.FindIndex(x => x.categoryName == "湯品");
            IndexTmp_1 = List.FindIndex(x => x.categoryID == "397");


            for (int cnt = 0; cnt < List.Count; cnt++)
            {
                bool IsLowestLayerTmp = IsLowestLayer(List, cnt);
                if (IsLowestLayerTmp)
                {   // 表示是最底層

                    // 將產品分類加入其父分類
                    List = AddToParentCategory(List, cnt);

                    cnt--; // 因為刪除當前的類別 所以下一個類別會變成目前的

                }
            }
            atCategoryInfoModified = List.ToArray();
            return atCategoryInfoModified;
        }

        /// <summary>
        /// 判斷是否是最底層分類
        /// </summary>
        /// <param name="a_atCategoryInfo">[IN] 產品分類結構清單</param>
        public static bool IsLowestLayer(List<CategoryInfo> a_CategoryList, int a_CurrentIndex)
        {
            if (a_CategoryList[a_CurrentIndex].parentID == "0")
            {   // 是最頂層 絕對不是底層
                return false;
            }
            else
            {
                for (int cnt = 0; cnt < a_CategoryList.Count; cnt++)
                {
                    if (a_CategoryList[cnt].parentID == a_CategoryList[a_CurrentIndex].categoryID && cnt != a_CurrentIndex)
                    {   // 是某個分類的父分類 >> 表示不是最底層
                        return false;
                    }
                }
            }

            // 都沒條件 hit >> 最底層
            return true;
        }

        /// <summary>
        /// 將產品分類加入其父分類: 為了處理巢狀問題
        /// </summary>
        /// <param name="a_atCategoryInfo">[IN] 產品分類結構清單</param>
        public static List<CategoryInfo> AddToParentCategory(List<CategoryInfo> a_List, int a_CurrentIndex)
        {
            // 找出他的所屬的主類別 index
            int IndexTmp = a_List.FindIndex(x => x.categoryID == a_List[a_CurrentIndex].parentID);

            // 將目前的類別加入其所屬類別
            List<CategoryInfo> ListTmp = null;
            if (a_List[IndexTmp].SubCategory == null)
            {
                ListTmp = new List<CategoryInfo>(); // 所屬的主類別的子類別暫時為空的 >> 創建
            }
            else
            {
                ListTmp = new List<CategoryInfo>(a_List[IndexTmp].SubCategory); // 將主類別的子類別轉成list
            }
            ListTmp.Add(a_List[a_CurrentIndex]);                                                     // 將當前類別加入 所屬主類別的子類別 list
            CategoryInfo TmpSwap = a_List[IndexTmp];                                           // list不能修改內容 只好用個結構暫存
            TmpSwap.SubCategory = ListTmp.ToArray();                                    // 改變其子類別    
            a_List.RemoveAt(IndexTmp);                                                    // 刪除原本主類別
            a_List.Insert(IndexTmp, TmpSwap);                                             // 加入子類別已被更新的主類別

            // 目前類別已被加入其他類別的子類別 所以要刪除
            a_List.RemoveAt(a_CurrentIndex);

            return a_List;
        }


        /// <summary>
        /// 取得產品資料載入結構並且保存: 這裡必須確定"SessionID"&"organizerNO"都已設定好了
        /// </summary>
        public static void Get_Product()
        {
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
                Global.atProductInfo[cnt].type = JItem.SelectToken("type").ToString();
                Global.atProductInfo[cnt].productType = JItem.SelectToken("productType").ToString();
                Global.atProductInfo[cnt].productID = JItem.SelectToken("productID").ToString();
                Global.atProductInfo[cnt].productName = JItem.SelectToken("productName").ToString();
                // Global.atProductInfo[cnt].productForeignName = JItem.SelectToken("productForeignName").ToString();
                Global.atProductInfo[cnt].productShortName = JItem.SelectToken("productShortName").ToString();
                Global.atProductInfo[cnt].commission = JItem.SelectToken("commission").ToString();
                Global.atProductInfo[cnt].note = JItem.SelectToken("note").ToString();
                Global.atProductInfo[cnt].content = JItem.SelectToken("content").ToString();

                // category is a list
                var CategoryList = JItem.SelectToken("category").ToList();
                long cnt_CategoryList = CategoryList.LongCount();
                Global.atProductInfo[cnt].category = new string[cnt_CategoryList];
                for(int CntTmp = 0; CntTmp < cnt_CategoryList; CntTmp++)
                {
                    Global.atProductInfo[cnt].category[CntTmp] = CategoryList[CntTmp].ToString();
                }

                Global.atProductInfo[cnt].productSort = JItem.SelectToken("productSort").ToString();
                Global.atProductInfo[cnt].specSort = JItem.SelectToken("specSort").ToString();
                Global.atProductInfo[cnt].redeemedPoint = JItem.SelectToken("redeemedPoint").ToString();
                Global.atProductInfo[cnt].barcode = JItem.SelectToken("barcode").ToString();

                Global.atProductInfo[cnt].oriPrice = Convert.ToDouble(JItem.SelectToken("oriPrice").ToString());
                Global.atProductInfo[cnt].price = Convert.ToDouble(JItem.SelectToken("price").ToString());

                // Global.atProductInfo[cnt].combine = JItem.SelectToken("combine").ToString(); // 不一定有 要另外處裡
                Global.atProductInfo[cnt].mark = JItem.SelectToken("mark").ToString();
                // Global.atProductInfo[cnt].DefaultOption = JItem.SelectToken("default").ToString(); // 不一定有 要另外處裡

                var ImageList = JItem.SelectToken("image"); // ImageList 會是個array
                // var TmpData = ImageList[0].SelectToken("productImage");

                if(ImageList.Children().Count() == 0)
                {   // 表示沒有網址... 連空字串都不是.....ORZ
                    Global.atProductInfo[cnt].productImage = "";
                    Global.atProductInfo[cnt].productImageThumb = "";
                }
                else
                {
                    Global.atProductInfo[cnt].productImage = ImageList[0].SelectToken("productImage").ToString();
                    Global.atProductInfo[cnt].productImageThumb = ImageList[0].SelectToken("productImageThumb").ToString();
                }

            }
            #endregion
        }

        /// <summary>
        /// 下載產品相關檔案: 這裡必須確定 Product 資料都已載入
        /// </summary>
        public static void DownLoad_Product_File()
        {
            // start to download all ad element
            for (int cnt = 0; cnt < Global.atProductInfo.Length; cnt++)
            {
                if (Global.atProductInfo[cnt].productImage != "" && Global.atProductInfo[cnt].productImageThumb != "")
                {
                    WebCommunication.DownloadFile
                        (Global.atProductInfo[cnt].productImage,
                        Global.TempDatadirPath + Global.atProductInfo[cnt].productName + ".jpg"
                        );

                    WebCommunication.DownloadFile
                        (Global.atProductInfo[cnt].productImageThumb,
                        Global.TempDatadirPath + Global.atProductInfo[cnt].productName + "Thumb.jpg"
                        );
                }
                else
                {
                    MessageBox.Show("Product " + cnt.ToString() + " URL is null");
                }

                Console.WriteLine("Product {0} already download.", cnt);
            }
        }

        /// <summary>
        /// 下載產品分類相關檔案: 這裡必須確定Category資料都已載入
        /// </summary>
        public static void DownLoad_Category_File()
        {
            // start to download all ad element
            for (int cnt = 0; cnt < Global.atCategoryInfo.Length; cnt++)
            {
                if (Global.atCategoryInfo[cnt].categoryImage != "" && Global.atCategoryInfo[cnt].categoryImageThumb != "")
                {
                    WebCommunication.DownloadFile
                        (Global.atCategoryInfo[cnt].categoryImage,
                        Global.TempDatadirPath + Global.atCategoryInfo[cnt].categoryName + ".jpg"
                        );

                    WebCommunication.DownloadFile
                        (Global.atCategoryInfo[cnt].categoryImageThumb,
                        Global.TempDatadirPath + Global.atCategoryInfo[cnt].categoryName + "Thumb.jpg"
                        );
                }
                else
                {
                    // MessageBox.Show("Category " + cnt.ToString() + " URL is null");
                    Console.WriteLine("Category " + cnt.ToString() + " URL is null");
                }
            }
        }

        /// <summary>
        /// 下載廣告資料相關檔案: 這裡必須確定廣告資料都已載入
        /// </summary>
        public static void DownLoad_AD_File()
        {
            // 建立檔案串流（@ 可取消跳脫字元 escape sequence） for log
            StreamWriter sw = new StreamWriter(Global.TempDatadirPath + @"AD_PlayList_Log.txt");

            // start to download all ad element
            for (int cnt = 0; cnt < Global.atAD_ContentInfo.Length; cnt++)
            {
                string TempADName = Global.TempDatadirPath + Global.atAD_ContentInfo[cnt].name + Global.atAD_ContentInfo[cnt].FilenameExtension;
                WebCommunication.DownloadFile(Global.atAD_ContentInfo[cnt].URL, TempADName);
                sw.WriteLine(TempADName); // 寫入文字
            }

            sw.Close(); // 關閉串流
        }

        /// <summary>
        /// 從文字檔重新載入廣告資料: 必須確定檔案已下載
        /// </summary>
        public static void Reload_AD_Content()
        {
            var AD_Content_Log = File.ReadAllText(Application.StartupPath + @"\temp_file\" + "AD_Content_Log.txt");

            #region With_List

            // 抓取 Jason 中的 "news"內容清單並轉成List
            var JList = JObject.Parse(AD_Content_Log).SelectToken("news").ToList();

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

        /// <summary>
        ///  Reload Category Content by .txt: 必須確定檔案已下載
        /// </summary>
        /// <remarks> 
        /// 取得產品分類內容載入結構 </remarks>
        public static void Reload_CategoryContent()
        {
            var Category_Log = File.ReadAllText(Application.StartupPath + @"\temp_file\" + "CategoryContent_Log.txt");
            
            #region With_List
            // 抓取 Jason 中的 "category"內容清單並轉成List
            var JList = JObject.Parse(Category_Log).SelectToken("category").ToList();

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
                Global.atCategoryInfo[cnt].discount = Convert.ToDouble(JItem.SelectToken("discount").ToString());
                Global.atCategoryInfo[cnt].service = Convert.ToDouble(JItem.SelectToken("service").ToString());
                Global.atCategoryInfo[cnt].categoryImage = JItem.SelectToken("categoryImage").ToString();
                Global.atCategoryInfo[cnt].categoryImageThumb = JItem.SelectToken("categoryImageThumb").ToString();
            }
            #endregion

            // sort array by "Sort" element
            //Array.Sort(Global.atCategoryInfo, delegate (CategoryInfo item1, CategoryInfo item2)
            //{
            //    return item1.Sort.CompareTo(item2.Sort); // (user1.Sort - user2.Sort)
            //});
        }

        /// <summary>
        /// 從文字檔重新載入產品資料載入結構: : 必須確定檔案已下載
        /// </summary>
        public static void Reload_Product()
        {

            var Product_Log = File.ReadAllText(Application.StartupPath + @"\temp_file\" + "Product_Log.txt");

            #region With_List
            // 抓取 Jason 中的 "product"內容清單並轉成List
            var JList = JObject.Parse(Product_Log).SelectToken("product").ToList();

            // 取得category長度
            long cnt_JList = JList.LongCount();

            // 創建相對應的結構
            Global.atProductInfo = new ProductInfo[cnt_JList];

            // 將每組輪播資料載入結構
            for (int cnt = 0; cnt < cnt_JList; cnt++)
            {
                var JItem = JList[cnt];
                Global.atProductInfo[cnt].type = JItem.SelectToken("type").ToString();
                Global.atProductInfo[cnt].productType = JItem.SelectToken("productType").ToString();
                Global.atProductInfo[cnt].productID = JItem.SelectToken("productID").ToString();
                Global.atProductInfo[cnt].productName = JItem.SelectToken("productName").ToString();
                // Global.atProductInfo[cnt].productForeignName = JItem.SelectToken("productForeignName").ToString();
                Global.atProductInfo[cnt].productShortName = JItem.SelectToken("productShortName").ToString();
                Global.atProductInfo[cnt].commission = JItem.SelectToken("commission").ToString();
                Global.atProductInfo[cnt].note = JItem.SelectToken("note").ToString();
                Global.atProductInfo[cnt].content = JItem.SelectToken("content").ToString();

                // category is a list
                var CategoryList = JItem.SelectToken("category").ToList();
                long cnt_CategoryList = CategoryList.LongCount();
                Global.atProductInfo[cnt].category = new string[cnt_CategoryList];
                for (int CntTmp = 0; CntTmp < cnt_CategoryList; CntTmp++)
                {
                    Global.atProductInfo[cnt].category[CntTmp] = CategoryList[CntTmp].ToString();
                }

                Global.atProductInfo[cnt].productSort = JItem.SelectToken("productSort").ToString();
                Global.atProductInfo[cnt].specSort = JItem.SelectToken("specSort").ToString();
                Global.atProductInfo[cnt].redeemedPoint = JItem.SelectToken("redeemedPoint").ToString();
                Global.atProductInfo[cnt].barcode = JItem.SelectToken("barcode").ToString();

                Global.atProductInfo[cnt].oriPrice = Convert.ToDouble(JItem.SelectToken("oriPrice").ToString());
                Global.atProductInfo[cnt].price = Convert.ToDouble(JItem.SelectToken("price").ToString());

                // Global.atProductInfo[cnt].combine = JItem.SelectToken("combine").ToString(); // 不一定有 要另外處裡
                Global.atProductInfo[cnt].mark = JItem.SelectToken("mark").ToString();
                // Global.atProductInfo[cnt].DefaultOption = JItem.SelectToken("default").ToString(); // 不一定有 要另外處裡

                var ImageList = JItem.SelectToken("image"); // ImageList 會是個array
                // var TmpData = ImageList[0].SelectToken("productImage");

                if (ImageList.Children().Count() == 0)
                {   // 表示沒有網址... 連空字串都不是.....ORZ
                    Global.atProductInfo[cnt].productImage = "";
                    Global.atProductInfo[cnt].productImageThumb = "";
                }
                else
                {
                    Global.atProductInfo[cnt].productImage = ImageList[0].SelectToken("productImage").ToString();
                    Global.atProductInfo[cnt].productImageThumb = ImageList[0].SelectToken("productImageThumb").ToString();
                }

            }
            #endregion
        }

        /// <summary>
        /// 從文字檔重新載入SessionID
        /// </summary>
        public static void Reload_SessionID()
        {
            using (StreamReader sr = new StreamReader(Global.TempDatadirPath + @"SessionID.txt"))     //小寫TXT
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Global.SessionID = line;
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// 從文字檔重新載入OrganizerNO
        /// </summary>
        public static void Reload_OrganizerNO()
        {
            using (StreamReader sr = new StreamReader(Global.TempDatadirPath + @"OrganizerNO.txt"))     //小寫TXT
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Global.organizerNO = line;
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// 從文字檔重新載入OrganizerNO
        /// </summary>
        public static List<string> Get_CategoryID(List<CategoryInfo> a_CategoryList)
        {
            List<CategoryInfo> CategoryListTmp = a_CategoryList;

            int ListNumber = CategoryListTmp.Count;


            List<string> Result = new List<string>();

            for (int cnt = 0; cnt < ListNumber; cnt++)
            {
                Result.Add(CategoryListTmp[cnt].categoryID);

                if(CategoryListTmp[cnt].SubCategory != null)
                {   // 有子類別 加入子類別id
                    List<string> SubTmp = Get_CategoryID(CategoryListTmp[cnt].SubCategory.ToList());
                    Result.AddRange(SubTmp);
                }
            }



            return Result;
        }
    }
}
