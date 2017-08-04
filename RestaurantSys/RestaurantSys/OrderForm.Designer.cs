namespace RestaurantSys
{
    partial class OrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.DisplayPanel = new System.Windows.Forms.Panel();
            this.BillButton = new System.Windows.Forms.Button();
            this.DetailButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.AutoScroll = true;
            this.MainPanel.Location = new System.Drawing.Point(12, 35);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(451, 100);
            this.MainPanel.TabIndex = 0;
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.AutoScroll = true;
            this.DisplayPanel.Location = new System.Drawing.Point(12, 141);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(451, 336);
            this.DisplayPanel.TabIndex = 1;
            // 
            // BillButton
            // 
            this.BillButton.Location = new System.Drawing.Point(37, 485);
            this.BillButton.Name = "BillButton";
            this.BillButton.Size = new System.Drawing.Size(95, 46);
            this.BillButton.TabIndex = 2;
            this.BillButton.Text = "Bill";
            this.BillButton.UseVisualStyleBackColor = true;
            this.BillButton.Click += new System.EventHandler(this.BillButton_Click);
            // 
            // DetailButton
            // 
            this.DetailButton.Location = new System.Drawing.Point(177, 485);
            this.DetailButton.Name = "DetailButton";
            this.DetailButton.Size = new System.Drawing.Size(95, 46);
            this.DetailButton.TabIndex = 3;
            this.DetailButton.Text = "Detail";
            this.DetailButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(311, 485);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(95, 46);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 541);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.DetailButton);
            this.Controls.Add(this.BillButton);
            this.Controls.Add(this.DisplayPanel);
            this.Controls.Add(this.MainPanel);
            this.Name = "OrderForm";
            this.Text = "OrderForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel DisplayPanel;
        private System.Windows.Forms.Button BillButton;
        private System.Windows.Forms.Button DetailButton;
        private System.Windows.Forms.Button CancelButton;
    }
}