namespace CoursesPFE
{
    partial class alert2
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ImgMssg = new FontAwesome.Sharp.IconButton();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblMessg = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ImgMssg
            // 
            this.ImgMssg.Dock = System.Windows.Forms.DockStyle.Left;
            this.ImgMssg.FlatAppearance.BorderSize = 0;
            this.ImgMssg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImgMssg.IconChar = FontAwesome.Sharp.IconChar.ExclamationCircle;
            this.ImgMssg.IconColor = System.Drawing.Color.White;
            this.ImgMssg.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.ImgMssg.IconSize = 35;
            this.ImgMssg.Location = new System.Drawing.Point(0, 0);
            this.ImgMssg.Name = "ImgMssg";
            this.ImgMssg.Size = new System.Drawing.Size(40, 63);
            this.ImgMssg.TabIndex = 1;
            this.ImgMssg.UseVisualStyleBackColor = true;
            // 
            // iconButton1
            // 
            this.iconButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconButton1.FlatAppearance.BorderSize = 0;
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.iconButton1.IconColor = System.Drawing.Color.White;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 35;
            this.iconButton1.Location = new System.Drawing.Point(317, 0);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(40, 63);
            this.iconButton1.TabIndex = 1;
            this.iconButton1.UseVisualStyleBackColor = true;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.LblMessg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(40, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 63);
            this.panel1.TabIndex = 2;
            // 
            // LblMessg
            // 
            this.LblMessg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblMessg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblMessg.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessg.ForeColor = System.Drawing.Color.White;
            this.LblMessg.Location = new System.Drawing.Point(0, 0);
            this.LblMessg.Name = "LblMessg";
            this.LblMessg.Size = new System.Drawing.Size(277, 63);
            this.LblMessg.TabIndex = 4;
            this.LblMessg.Text = "the html is one of important languages for building websitesthe html is one of im" +
    "portant languages for building websites";
            this.LblMessg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alert2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(58)))), ((int)(((byte)(193)))));
            this.ClientSize = new System.Drawing.Size(357, 63);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ImgMssg);
            this.Controls.Add(this.iconButton1);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "alert2";
            this.Text = "alert2";
            this.Load += new System.EventHandler(this.alert2_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton ImgMssg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblMessg;
    }
}