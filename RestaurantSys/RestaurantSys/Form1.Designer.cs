namespace RestaurantSys
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AdRotator = new System.Windows.Forms.PictureBox();
            this.AdopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Adtimer = new System.Windows.Forms.Timer(this.components);
            this.AdListBox = new System.Windows.Forms.ListBox();
            this.AdIntervalText = new System.Windows.Forms.TextBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SetButton = new System.Windows.Forms.Button();
            this.OrderButton = new System.Windows.Forms.Button();
            this.TestPanel = new System.Windows.Forms.Panel();
            this.NewsContentButton = new System.Windows.Forms.Button();
            this.NewsListButton = new System.Windows.Forms.Button();
            this.organizerNO_Button = new System.Windows.Forms.Button();
            this.LogIn_HttpWebRequest = new System.Windows.Forms.Button();
            this.LogInButton = new System.Windows.Forms.Button();
            this.DownloadButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AdRotator)).BeginInit();
            this.TestPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AdRotator
            // 
            this.AdRotator.Location = new System.Drawing.Point(12, 12);
            this.AdRotator.Name = "AdRotator";
            this.AdRotator.Size = new System.Drawing.Size(371, 241);
            this.AdRotator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AdRotator.TabIndex = 0;
            this.AdRotator.TabStop = false;
            this.AdRotator.Click += new System.EventHandler(this.AdRotator_Click);
            // 
            // AdopenFileDialog
            // 
            this.AdopenFileDialog.FileName = "openFileDialog1";
            // 
            // Adtimer
            // 
            this.Adtimer.Enabled = true;
            this.Adtimer.Interval = 2000;
            this.Adtimer.Tick += new System.EventHandler(this.Adtimer_Tick);
            // 
            // AdListBox
            // 
            this.AdListBox.FormattingEnabled = true;
            this.AdListBox.ItemHeight = 12;
            this.AdListBox.Location = new System.Drawing.Point(36, 8);
            this.AdListBox.Name = "AdListBox";
            this.AdListBox.Size = new System.Drawing.Size(156, 112);
            this.AdListBox.TabIndex = 1;
            // 
            // AdIntervalText
            // 
            this.AdIntervalText.Location = new System.Drawing.Point(36, 157);
            this.AdIntervalText.Name = "AdIntervalText";
            this.AdIntervalText.Size = new System.Drawing.Size(75, 22);
            this.AdIntervalText.TabIndex = 2;
            this.AdIntervalText.Text = "2000";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(36, 126);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(117, 126);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // SetButton
            // 
            this.SetButton.Location = new System.Drawing.Point(117, 155);
            this.SetButton.Name = "SetButton";
            this.SetButton.Size = new System.Drawing.Size(75, 23);
            this.SetButton.TabIndex = 5;
            this.SetButton.Text = "Set (ms)";
            this.SetButton.UseVisualStyleBackColor = true;
            this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // OrderButton
            // 
            this.OrderButton.Location = new System.Drawing.Point(160, 358);
            this.OrderButton.Name = "OrderButton";
            this.OrderButton.Size = new System.Drawing.Size(137, 29);
            this.OrderButton.TabIndex = 6;
            this.OrderButton.Text = "Order";
            this.OrderButton.UseVisualStyleBackColor = true;
            this.OrderButton.Click += new System.EventHandler(this.OrderButton_Click);
            // 
            // TestPanel
            // 
            this.TestPanel.Controls.Add(this.DownloadButton);
            this.TestPanel.Controls.Add(this.NewsContentButton);
            this.TestPanel.Controls.Add(this.NewsListButton);
            this.TestPanel.Controls.Add(this.organizerNO_Button);
            this.TestPanel.Controls.Add(this.LogIn_HttpWebRequest);
            this.TestPanel.Controls.Add(this.LogInButton);
            this.TestPanel.Controls.Add(this.AdListBox);
            this.TestPanel.Controls.Add(this.AddButton);
            this.TestPanel.Controls.Add(this.SetButton);
            this.TestPanel.Controls.Add(this.AdIntervalText);
            this.TestPanel.Controls.Add(this.RemoveButton);
            this.TestPanel.Location = new System.Drawing.Point(429, 4);
            this.TestPanel.Name = "TestPanel";
            this.TestPanel.Size = new System.Drawing.Size(200, 341);
            this.TestPanel.TabIndex = 7;
            // 
            // NewsContentButton
            // 
            this.NewsContentButton.Location = new System.Drawing.Point(36, 243);
            this.NewsContentButton.Name = "NewsContentButton";
            this.NewsContentButton.Size = new System.Drawing.Size(75, 23);
            this.NewsContentButton.TabIndex = 11;
            this.NewsContentButton.Text = "NewsContent";
            this.NewsContentButton.UseVisualStyleBackColor = true;
            this.NewsContentButton.Click += new System.EventHandler(this.NewsContentButton_Click);
            // 
            // NewsListButton
            // 
            this.NewsListButton.Location = new System.Drawing.Point(117, 214);
            this.NewsListButton.Name = "NewsListButton";
            this.NewsListButton.Size = new System.Drawing.Size(75, 23);
            this.NewsListButton.TabIndex = 10;
            this.NewsListButton.Text = "NewsList";
            this.NewsListButton.UseVisualStyleBackColor = true;
            this.NewsListButton.Click += new System.EventHandler(this.NewsListButton_Click);
            // 
            // organizerNO_Button
            // 
            this.organizerNO_Button.Location = new System.Drawing.Point(36, 214);
            this.organizerNO_Button.Name = "organizerNO_Button";
            this.organizerNO_Button.Size = new System.Drawing.Size(75, 23);
            this.organizerNO_Button.TabIndex = 9;
            this.organizerNO_Button.Text = "OrganizerNO";
            this.organizerNO_Button.UseVisualStyleBackColor = true;
            this.organizerNO_Button.Click += new System.EventHandler(this.organizerNO_Button_Click);
            // 
            // LogIn_HttpWebRequest
            // 
            this.LogIn_HttpWebRequest.Location = new System.Drawing.Point(117, 185);
            this.LogIn_HttpWebRequest.Name = "LogIn_HttpWebRequest";
            this.LogIn_HttpWebRequest.Size = new System.Drawing.Size(75, 23);
            this.LogIn_HttpWebRequest.TabIndex = 8;
            this.LogIn_HttpWebRequest.Text = "LogIn_1";
            this.LogIn_HttpWebRequest.UseVisualStyleBackColor = true;
            this.LogIn_HttpWebRequest.Click += new System.EventHandler(this.LogIn_HttpWebRequest_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(36, 185);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(75, 23);
            this.LogInButton.TabIndex = 6;
            this.LogInButton.Text = "LogIn";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_ClickAsync);
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(117, 243);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(75, 23);
            this.DownloadButton.TabIndex = 12;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 399);
            this.Controls.Add(this.TestPanel);
            this.Controls.Add(this.OrderButton);
            this.Controls.Add(this.AdRotator);
            this.Name = "Form1";
            this.Text = "Realtouch Restaurant System";
            ((System.ComponentModel.ISupportInitialize)(this.AdRotator)).EndInit();
            this.TestPanel.ResumeLayout(false);
            this.TestPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox AdRotator;
        private System.Windows.Forms.OpenFileDialog AdopenFileDialog;
        private System.Windows.Forms.Timer Adtimer;
        private System.Windows.Forms.ListBox AdListBox;
        private System.Windows.Forms.TextBox AdIntervalText;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button SetButton;
        private System.Windows.Forms.Button OrderButton;
        private System.Windows.Forms.Panel TestPanel;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.Button LogIn_HttpWebRequest;
        private System.Windows.Forms.Button organizerNO_Button;
        private System.Windows.Forms.Button NewsListButton;
        private System.Windows.Forms.Button NewsContentButton;
        private System.Windows.Forms.Button DownloadButton;
    }
}

