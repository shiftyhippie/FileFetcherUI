namespace CompiledFileFetcherUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            DropDown = new ComboBox();
            Fetch = new Button();
            GetLatestDev = new CheckBox();
            Clients = new ComboBox();
            SuspendLayout();
            // 
            // DropDown
            // 
            DropDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DropDown.Location = new Point(12, 52);
            DropDown.Margin = new Padding(3, 4, 3, 4);
            DropDown.Name = "DropDown";
            DropDown.Size = new Size(258, 28);
            DropDown.TabIndex = 2;
            DropDown.Text = "Contract";
            // 
            // Fetch
            // 
            Fetch.Location = new Point(186, 88);
            Fetch.Margin = new Padding(3, 4, 3, 4);
            Fetch.Name = "Fetch";
            Fetch.Size = new Size(86, 31);
            Fetch.TabIndex = 1;
            Fetch.Text = "Fetch";
            Fetch.UseVisualStyleBackColor = true;
            Fetch.Click += Fetch_Click;
            // 
            // GetLatestDev
            // 
            GetLatestDev.AutoSize = true;
            GetLatestDev.Checked = true;
            GetLatestDev.CheckState = CheckState.Checked;
            GetLatestDev.Location = new Point(14, 94);
            GetLatestDev.Margin = new Padding(3, 4, 3, 4);
            GetLatestDev.Name = "GetLatestDev";
            GetLatestDev.Size = new Size(127, 24);
            GetLatestDev.TabIndex = 3;
            GetLatestDev.Text = "Get Latest Dev";
            GetLatestDev.UseVisualStyleBackColor = true;
            // 
            // Clients
            // 
            Clients.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Clients.Location = new Point(12, 13);
            Clients.Margin = new Padding(3, 4, 3, 4);
            Clients.Name = "Clients";
            Clients.Size = new Size(258, 28);
            Clients.TabIndex = 4;
            Clients.Text = "Client";
            Clients.SelectedIndexChanged += Clients_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 132);
            Controls.Add(Clients);
            Controls.Add(GetLatestDev);
            Controls.Add(Fetch);
            Controls.Add(DropDown);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Compiled File Fetcher";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button Fetch;
        public ComboBox DropDown;
        public CheckBox GetLatestDev;
        public ComboBox Clients;
    }
}