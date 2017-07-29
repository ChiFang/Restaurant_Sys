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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            // 測試用 產生N個按鈕 EX: 20
            GenButton(MainMenuPanel, 20, 100, MainMenuPanel.Size.Height-20);

            GenButton(panel1, 10, 50, MainMenuPanel.Size.Height - 20, false);


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void GenButton(Panel a_panel, int a_ButtomNum, int a_BtnWidth =100, int a_BtnHeight = 30, bool a_Mode = true)
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

                if(a_Mode)
                {
                    btn.Left = a_BtnWidth * i;
                    btn.Top = 0;
                    btn.Text = i.ToString();
                    btn.Click += new EventHandler(MainMenuClick);
                }
                else
                {
                    if(a_BtnWidth * ColIndex >= WidthLimit)
                    {
                        ColIndex = 0;
                        RowIndex++;
                    }

                    btn.Left = a_BtnWidth * ColIndex;
                    btn.Top = RowIndex* a_BtnHeight;
                    btn.Text = i.ToString();
                    btn.Click += new EventHandler(DetailMenuClick);

                    ColIndex++;
                }
                
            }
        }

        private void MainMenuClick(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Text);
        }

        private void DetailMenuClick(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Text);
        }
    }
}
