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

        /// <summary> true: 顯示菜單 false: 顯示目前點餐細項 </summary>
        public static bool IsDisplay = true;

        public OrderForm()
        {
            InitializeComponent();


            // 測試用 產生N個按鈕 EX: 載入階層化後的產品分類清單
            atCategoryInfoHierarchicalTmp = Global.atCategoryInfoHierarchical;
            GenButton(MainPanel, atCategoryInfoHierarchicalTmp.Length, 100, MainPanel.Size.Height - 20);

            // 測試用 產生N個按鈕 EX: 10
            GenButton(DisplayPanel, 10, 100, MainPanel.Size.Height - 20, false);
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
                    btn.Text = i.ToString();
                    btn.Click += new EventHandler(DetailMenuClick);

                    ColIndex++;
                }

            }
        }

        private void MainMenuClick(object sender, EventArgs e)
        {
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
                atCategoryInfoHierarchicalTmp = atCategoryInfoHierarchicalTmp[IndexTmp].SubCategory;

                MainPanel.Controls.Clear();

                GenButton(MainPanel, atCategoryInfoHierarchicalTmp.Length, 100, MainPanel.Size.Height - 20);
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
