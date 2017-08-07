using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantSys
{
    public partial class OrderForm : Form
    {
        /// <summary> 產品分類清單: 階層化後 </summary>
        public static CategoryInfo[] atCategoryInfoHierarchicalTmp = null;


        /// <summary> 產品清單: 顯示區內 </summary>
        public static List<ProductInfo> atProductInfoTmp = null;

        /// <summary> true: 顯示菜單 false: 顯示目前點餐細項 </summary>
        public static bool IsDisplay = true;

        public OrderForm()
        {
            InitializeComponent();


            // 測試用 產生N個按鈕 EX: 載入階層化後的產品分類清單
            atCategoryInfoHierarchicalTmp = Global.atCategoryInfoHierarchical;
            GenButton(MainPanel, atCategoryInfoHierarchicalTmp.Length, 100, MainPanel.Size.Height - 20);

            // 測試用 產生N個按鈕 EX: 10
            // GenButton(DisplayPanel, 10, 100, MainPanel.Size.Height - 20, false);
        }

        private void BillButton_Click(object sender, EventArgs e)
        {

        }

        private void GenButton(Panel a_panel, int a_ButtomNum, int a_BtnWidth = 100, int a_BtnHeight = 30, bool a_Mode = true)
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
                    // btn.Text = i.ToString();
                    btn.Text = atCategoryInfoHierarchicalTmp[i].categoryName;
                    
                    btn.Click += new EventHandler(MainMenuClick);
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
                    btn.Text = atProductInfoTmp[i].productName;
                    btn.Click += new EventHandler(DetailMenuClick);

                    ColIndex++;
                }

            }
        }

        private void MainMenuClick(object sender, EventArgs e)
        {
            bool IsSubCategory = false;


            // for debug
            if (Global.DEBUG_FLAG > 1)
            {
                MessageBox.Show(((Button)sender).Text);
            }

            List<CategoryInfo> ListTmp = new List<CategoryInfo>(atCategoryInfoHierarchicalTmp);

            // 找出按鈕所屬的 index
            int IndexTmp = ListTmp.FindIndex(x => x.categoryName == ((Button)sender).Text);

            if (atCategoryInfoHierarchicalTmp[IndexTmp].SubCategory != null)
            {
                IsSubCategory = true;
                atCategoryInfoHierarchicalTmp = atCategoryInfoHierarchicalTmp[IndexTmp].SubCategory;

                MainPanel.Controls.Clear();

                GenButton(MainPanel, atCategoryInfoHierarchicalTmp.Length, 100, MainPanel.Size.Height - 20);
            }
            else
            {
                IsSubCategory = false;
            }


            // 要刷新顯示區域 >> DisplayPanel
            List<string> Result = new List<string>();

            if (IsSubCategory)
            {
                Result = RealtouchBoard.Get_CategoryID(atCategoryInfoHierarchicalTmp.ToList());
            }
            else
            {
                List<CategoryInfo> tmpList = new List<CategoryInfo>();
                tmpList.Add(atCategoryInfoHierarchicalTmp[IndexTmp]);
                Result = RealtouchBoard.Get_CategoryID(tmpList);
            }

            atProductInfoTmp = new List<ProductInfo>();
            for (int cnt = 0; cnt < Global.atProductInfo.Length; cnt++)
            {

                if (Global.atProductInfo[cnt].category.Length > 0)
                {   // 理論上一定要有 產品分類... 但居然遇到沒有分類的 ORZ.....
                    for (int CntSub = 0; CntSub < Global.atProductInfo[cnt].category.Length; CntSub++)
                    {
                        string TmpSearch = Global.atProductInfo[cnt].category[CntSub];

                        int IndexProduct = Result.FindIndex(x => x == TmpSearch);

                        if (IndexProduct >= 0)
                        {
                            atProductInfoTmp.Add(Global.atProductInfo[cnt]);
                            break;
                        }
                    }
                }
            }


            DisplayPanel.Controls.Clear();
            if (atProductInfoTmp.Count >0)
            {
                GenButton(DisplayPanel, atProductInfoTmp.Count, 100, MainPanel.Size.Height - 20, false);
            }
        }

        private void DetailMenuClick(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Text);
        }

        private void MainMenuPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // close its form after select
            ((Button)sender).FindForm().Close();
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            atCategoryInfoHierarchicalTmp = Global.atCategoryInfoHierarchical;

            MainPanel.Controls.Clear();

            GenButton(MainPanel, atCategoryInfoHierarchicalTmp.Length, 100, MainPanel.Size.Height - 20);


            // 要刷新顯示區域 >> DisplayPanel
            List<string> Result = new List<string>();

          
            Result = RealtouchBoard.Get_CategoryID(atCategoryInfoHierarchicalTmp.ToList());
          

            atProductInfoTmp = new List<ProductInfo>();
            for (int cnt = 0; cnt < Global.atProductInfo.Length; cnt++)
            {

                if (Global.atProductInfo[cnt].category.Length > 0)
                {   // 理論上一定要有 產品分類... 但居然遇到沒有分類的 ORZ.....
                    for (int CntSub = 0; CntSub < Global.atProductInfo[cnt].category.Length; CntSub++)
                    {
                        string TmpSearch = Global.atProductInfo[cnt].category[CntSub];

                        int IndexProduct = Result.FindIndex(x => x == TmpSearch);

                        if (IndexProduct >= 0)
                        {
                            atProductInfoTmp.Add(Global.atProductInfo[cnt]);
                            break;
                        }
                    }
                }
            }


            DisplayPanel.Controls.Clear();
            if (atProductInfoTmp.Count > 0)
            {
                GenButton(DisplayPanel, atProductInfoTmp.Count, 100, MainPanel.Size.Height - 20, false);
            }
        }

        private void DetailButton_Click(object sender, EventArgs e)
        {
            IsDisplay = !IsDisplay;

            if(IsDisplay)
            {
                DetailButton.Text = "Detail";
            }
            else
            {
                DetailButton.Text = "Display";
            }
        }
    }
}
