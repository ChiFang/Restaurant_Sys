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
    public partial class LogInFrom : Form
    {
        public LogInFrom()
        {
            InitializeComponent();

            // 顯示當下的帳號 密碼 系統
            AccountTextBox.Text = Global.Account;
            PasswordTextBox.Text = Global.Password;
            SystemTextBox.Text = Global.System;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            // 顯示當下的帳號 密碼 系統
             Global.Account = AccountTextBox.Text;
             Global.Password = PasswordTextBox.Text;
             Global.System = SystemTextBox.Text;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
