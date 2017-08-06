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

                    int IsVedeo = AdRotator.ImageLocation.IndexOf(".mp4");
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
            OrderForm frm = new OrderForm();
            // frm.Show();

            //設定 OrderForm 為Form1的上層，並開啟 OrderForm 視窗。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            frm.ShowDialog(this);
        }

        private void LogIn_Click(object sender, EventArgs e)
        {
            // WebCommunication.GetSessionID();

            LogInFrom frm = new LogInFrom();
            // frm.Show();

            //設定 LogInFrom 為Form1的上層，並開啟Form2視窗。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            frm.ShowDialog(this);
        }

        private void organizerNO_Button_Click(object sender, EventArgs e)
        {
            RealtouchBoard.GetSessionID();
            RealtouchBoard.GetOrganizerNO();
        }

        private void NewsListButton_Click(object sender, EventArgs e)
        {
            RealtouchBoard.GetSystemID();
        }

        private void NewsContentButton_Click(object sender, EventArgs e)
        {
            SetADContentFileAndSetPlayList();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            // Global.DEBUG_FLAG = 1;

            // step 1: 取得廣告資訊與檔案
            SetADInfoAndFile();

            // step 2: 取得產品分類資訊與檔案
            SetCategoryInfoAndFile();

            // step 3: 取得產品資訊與檔案
            SetProductInfoAndFile();
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            SetCategoryInfoAndFile();
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            SetProductInfoAndFile();
        }

        private void SetADInfoAndFile()
        {
            // step 1: 取得 SystemID >> keyword = "ADEBOARD" 的輪播 systemID
            RealtouchBoard.GetSystemID();

            // step 2: 取得輪播內容、檔案 & 加入撥放清單
            SetADContentFileAndSetPlayList();
        }

        private void SetADContentFileAndSetPlayList()
        {
            // 取得輪播內容
            RealtouchBoard.GetAD_Content();

            // 下載所有輪播檔案 並將輪播廣告加入撥放清單
            RealtouchBoard.DownLoad_AD_File();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All AD file are downloaded.");
            }

            // add to play list
            for (int cnt = 0; cnt < Global.atAD_ContentInfo.Length; cnt++)
            {
                string TempADName = Global.TempDatadirPath + Global.atAD_ContentInfo[cnt].name + Global.atAD_ContentInfo[cnt].FilenameExtension;
                AdListBox.Items.Add(TempADName);
            }

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All AD file are added in play list.");
            }
        }

        private void SetCategoryInfoAndFile()
        {
            RealtouchBoard.Get_CategoryContent();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("There are " + Global.atCategoryInfo.Length.ToString() + " Category.");
            }

            // for making Category Info Hierarchical
            // Global.atCategoryInfoHierarchical = RealtouchBoard.MakeCategoryInfoHierarchical_two_layer_one_parentID(Global.atCategoryInfo);
            Global.atCategoryInfoHierarchical = RealtouchBoard.MakeCategoryInfoHierarchical_multi_layer_one_parentID(Global.atCategoryInfo);

            RealtouchBoard.DownLoad_Category_File();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All Category file are downloaded.");
            }
        }

        private void SetProductInfoAndFile()
        {
            RealtouchBoard.Get_Product();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("There are " + Global.atProductInfo.Length.ToString() + " Product.");
            }

            RealtouchBoard.DownLoad_Product_File();

            // for debug
            if (Global.DEBUG_FLAG > 0)
            {
                MessageBox.Show("All Product file are downloaded.");
            }
        }
    }

}
