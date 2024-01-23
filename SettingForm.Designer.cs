namespace JD_Proc
{
    partial class SettingForm
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
            ReaLTaiizor.Controls.FoxLabel foxLabel1;
            ReaLTaiizor.Controls.FoxLabel foxLabel2;
            ReaLTaiizor.Controls.FoxLabel foxLabel3;
            ReaLTaiizor.Controls.FoxLabel foxLabel4;
            dungeonForm1 = new ReaLTaiizor.Forms.DungeonForm();
            thunderGroupBox2 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dTbox_y2 = new ReaLTaiizor.Controls.CrownTextBox();
            dTbox_x2 = new ReaLTaiizor.Controls.CrownTextBox();
            dBtn_pixelLoad2 = new ReaLTaiizor.Controls.DreamButton();
            dBtn_pixelSave2 = new ReaLTaiizor.Controls.DreamButton();
            thunderGroupBox1 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dTbox_y1 = new ReaLTaiizor.Controls.CrownTextBox();
            dTbox_x1 = new ReaLTaiizor.Controls.CrownTextBox();
            dBtn_pixelLoad = new ReaLTaiizor.Controls.DreamButton();
            dBtn_pixelSave = new ReaLTaiizor.Controls.DreamButton();
            controlBoxEdit1 = new ReaLTaiizor.Controls.ControlBoxEdit();
            foxLabel1 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel2 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel3 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel4 = new ReaLTaiizor.Controls.FoxLabel();
            dungeonForm1.SuspendLayout();
            thunderGroupBox2.SuspendLayout();
            thunderGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // foxLabel1
            // 
            foxLabel1.AllowDrop = true;
            foxLabel1.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel1.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel1.Location = new Point(20, 35);
            foxLabel1.Margin = new Padding(3, 10, 3, 3);
            foxLabel1.Name = "foxLabel1";
            foxLabel1.Size = new Size(142, 29);
            foxLabel1.TabIndex = 2;
            foxLabel1.Text = "분해능(가로) μm  : ";
            // 
            // foxLabel2
            // 
            foxLabel2.AllowDrop = true;
            foxLabel2.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel2.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel2.Location = new Point(20, 77);
            foxLabel2.Margin = new Padding(3, 10, 3, 3);
            foxLabel2.Name = "foxLabel2";
            foxLabel2.Size = new Size(142, 29);
            foxLabel2.TabIndex = 3;
            foxLabel2.Text = "분해능(세로) μm  : ";
            // 
            // foxLabel3
            // 
            foxLabel3.AllowDrop = true;
            foxLabel3.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel3.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel3.Location = new Point(20, 35);
            foxLabel3.Margin = new Padding(3, 10, 3, 3);
            foxLabel3.Name = "foxLabel3";
            foxLabel3.Size = new Size(142, 29);
            foxLabel3.TabIndex = 2;
            foxLabel3.Text = "분해능(가로) μm  : ";
            // 
            // foxLabel4
            // 
            foxLabel4.AllowDrop = true;
            foxLabel4.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel4.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel4.Location = new Point(20, 77);
            foxLabel4.Margin = new Padding(3, 10, 3, 3);
            foxLabel4.Name = "foxLabel4";
            foxLabel4.Size = new Size(142, 29);
            foxLabel4.TabIndex = 3;
            foxLabel4.Text = "분해능(세로) μm  : ";
            // 
            // dungeonForm1
            // 
            dungeonForm1.BackColor = Color.FromArgb(40, 45, 45);
            dungeonForm1.BorderColor = Color.FromArgb(38, 38, 38);
            dungeonForm1.Controls.Add(thunderGroupBox2);
            dungeonForm1.Controls.Add(thunderGroupBox1);
            dungeonForm1.Controls.Add(controlBoxEdit1);
            dungeonForm1.Dock = DockStyle.Fill;
            dungeonForm1.FillEdgeColorA = Color.FromArgb(69, 68, 63);
            dungeonForm1.FillEdgeColorB = Color.FromArgb(69, 68, 63);
            dungeonForm1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dungeonForm1.FooterEdgeColor = Color.FromArgb(69, 68, 63);
            dungeonForm1.ForeColor = Color.FromArgb(223, 219, 210);
            dungeonForm1.HeaderEdgeColorA = Color.FromArgb(87, 85, 77);
            dungeonForm1.HeaderEdgeColorB = Color.FromArgb(69, 68, 63);
            dungeonForm1.Location = new Point(0, 0);
            dungeonForm1.Name = "dungeonForm1";
            dungeonForm1.Padding = new Padding(20, 56, 20, 16);
            dungeonForm1.RoundCorners = true;
            dungeonForm1.Sizable = true;
            dungeonForm1.Size = new Size(612, 261);
            dungeonForm1.SmartBounds = true;
            dungeonForm1.StartPosition = FormStartPosition.WindowsDefaultLocation;
            dungeonForm1.TabIndex = 0;
            dungeonForm1.Text = "Settings";
            dungeonForm1.TitleColor = Color.FromArgb(223, 219, 210);
            // 
            // thunderGroupBox2
            // 
            thunderGroupBox2.BackColor = Color.Transparent;
            thunderGroupBox2.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox2.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox2.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox2.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox2.Controls.Add(dTbox_y2);
            thunderGroupBox2.Controls.Add(dTbox_x2);
            thunderGroupBox2.Controls.Add(dBtn_pixelLoad2);
            thunderGroupBox2.Controls.Add(dBtn_pixelSave2);
            thunderGroupBox2.Controls.Add(foxLabel3);
            thunderGroupBox2.Controls.Add(foxLabel4);
            thunderGroupBox2.ForeColor = Color.WhiteSmoke;
            thunderGroupBox2.Location = new Point(320, 59);
            thunderGroupBox2.Name = "thunderGroupBox2";
            thunderGroupBox2.Size = new Size(272, 169);
            thunderGroupBox2.TabIndex = 20;
            thunderGroupBox2.Text = "Resolution CAM 2";
            // 
            // dTbox_y2
            // 
            dTbox_y2.BackColor = Color.FromArgb(69, 73, 74);
            dTbox_y2.BorderStyle = BorderStyle.FixedSingle;
            dTbox_y2.ForeColor = Color.FromArgb(220, 220, 220);
            dTbox_y2.Location = new Point(168, 77);
            dTbox_y2.Name = "dTbox_y2";
            dTbox_y2.Size = new Size(88, 23);
            dTbox_y2.TabIndex = 24;
            dTbox_y2.Text = "0";
            // 
            // dTbox_x2
            // 
            dTbox_x2.BackColor = Color.FromArgb(69, 73, 74);
            dTbox_x2.BorderStyle = BorderStyle.FixedSingle;
            dTbox_x2.ForeColor = Color.FromArgb(220, 220, 220);
            dTbox_x2.Location = new Point(168, 38);
            dTbox_x2.Name = "dTbox_x2";
            dTbox_x2.Size = new Size(88, 23);
            dTbox_x2.TabIndex = 23;
            dTbox_x2.Text = "0";
            // 
            // dBtn_pixelLoad2
            // 
            dBtn_pixelLoad2.ColorA = Color.FromArgb(31, 31, 31);
            dBtn_pixelLoad2.ColorB = Color.FromArgb(41, 41, 41);
            dBtn_pixelLoad2.ColorC = Color.FromArgb(51, 51, 51);
            dBtn_pixelLoad2.ColorD = Color.FromArgb(0, 0, 0, 0);
            dBtn_pixelLoad2.ColorE = Color.FromArgb(25, 255, 255, 255);
            dBtn_pixelLoad2.ForeColor = Color.FromArgb(40, 218, 255);
            dBtn_pixelLoad2.Location = new Point(151, 112);
            dBtn_pixelLoad2.Name = "dBtn_pixelLoad2";
            dBtn_pixelLoad2.Size = new Size(105, 40);
            dBtn_pixelLoad2.TabIndex = 19;
            dBtn_pixelLoad2.Text = "불러오기";
            dBtn_pixelLoad2.UseVisualStyleBackColor = true;
            dBtn_pixelLoad2.Click += dBtn_pixelLoad2_Click;
            // 
            // dBtn_pixelSave2
            // 
            dBtn_pixelSave2.ColorA = Color.FromArgb(31, 31, 31);
            dBtn_pixelSave2.ColorB = Color.FromArgb(41, 41, 41);
            dBtn_pixelSave2.ColorC = Color.FromArgb(51, 51, 51);
            dBtn_pixelSave2.ColorD = Color.FromArgb(0, 0, 0, 0);
            dBtn_pixelSave2.ColorE = Color.FromArgb(25, 255, 255, 255);
            dBtn_pixelSave2.ForeColor = Color.FromArgb(40, 218, 255);
            dBtn_pixelSave2.Location = new Point(20, 112);
            dBtn_pixelSave2.Name = "dBtn_pixelSave2";
            dBtn_pixelSave2.Size = new Size(105, 40);
            dBtn_pixelSave2.TabIndex = 18;
            dBtn_pixelSave2.Text = "저 장";
            dBtn_pixelSave2.UseVisualStyleBackColor = true;
            dBtn_pixelSave2.Click += dBtn_pixelSave2_Click;
            // 
            // thunderGroupBox1
            // 
            thunderGroupBox1.BackColor = Color.Transparent;
            thunderGroupBox1.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox1.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox1.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox1.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox1.Controls.Add(dTbox_y1);
            thunderGroupBox1.Controls.Add(dTbox_x1);
            thunderGroupBox1.Controls.Add(dBtn_pixelLoad);
            thunderGroupBox1.Controls.Add(dBtn_pixelSave);
            thunderGroupBox1.Controls.Add(foxLabel1);
            thunderGroupBox1.Controls.Add(foxLabel2);
            thunderGroupBox1.ForeColor = Color.WhiteSmoke;
            thunderGroupBox1.Location = new Point(23, 59);
            thunderGroupBox1.Name = "thunderGroupBox1";
            thunderGroupBox1.Size = new Size(272, 169);
            thunderGroupBox1.TabIndex = 18;
            thunderGroupBox1.Text = "Resolution CAM 1";
            // 
            // dTbox_y1
            // 
            dTbox_y1.BackColor = Color.FromArgb(69, 73, 74);
            dTbox_y1.BorderStyle = BorderStyle.FixedSingle;
            dTbox_y1.ForeColor = Color.FromArgb(220, 220, 220);
            dTbox_y1.Location = new Point(168, 77);
            dTbox_y1.Name = "dTbox_y1";
            dTbox_y1.Size = new Size(88, 23);
            dTbox_y1.TabIndex = 22;
            dTbox_y1.Text = "0";
            // 
            // dTbox_x1
            // 
            dTbox_x1.BackColor = Color.FromArgb(69, 73, 74);
            dTbox_x1.BorderStyle = BorderStyle.FixedSingle;
            dTbox_x1.ForeColor = Color.FromArgb(220, 220, 220);
            dTbox_x1.Location = new Point(168, 38);
            dTbox_x1.Name = "dTbox_x1";
            dTbox_x1.Size = new Size(88, 23);
            dTbox_x1.TabIndex = 21;
            dTbox_x1.Text = "0";
            // 
            // dBtn_pixelLoad
            // 
            dBtn_pixelLoad.ColorA = Color.FromArgb(31, 31, 31);
            dBtn_pixelLoad.ColorB = Color.FromArgb(41, 41, 41);
            dBtn_pixelLoad.ColorC = Color.FromArgb(51, 51, 51);
            dBtn_pixelLoad.ColorD = Color.FromArgb(0, 0, 0, 0);
            dBtn_pixelLoad.ColorE = Color.FromArgb(25, 255, 255, 255);
            dBtn_pixelLoad.ForeColor = Color.FromArgb(40, 218, 255);
            dBtn_pixelLoad.Location = new Point(151, 112);
            dBtn_pixelLoad.Name = "dBtn_pixelLoad";
            dBtn_pixelLoad.Size = new Size(105, 40);
            dBtn_pixelLoad.TabIndex = 19;
            dBtn_pixelLoad.Text = "불러오기";
            dBtn_pixelLoad.UseVisualStyleBackColor = true;
            dBtn_pixelLoad.Click += dBtn_pixelLoad_Click;
            // 
            // dBtn_pixelSave
            // 
            dBtn_pixelSave.ColorA = Color.FromArgb(31, 31, 31);
            dBtn_pixelSave.ColorB = Color.FromArgb(41, 41, 41);
            dBtn_pixelSave.ColorC = Color.FromArgb(51, 51, 51);
            dBtn_pixelSave.ColorD = Color.FromArgb(0, 0, 0, 0);
            dBtn_pixelSave.ColorE = Color.FromArgb(25, 255, 255, 255);
            dBtn_pixelSave.ForeColor = Color.FromArgb(40, 218, 255);
            dBtn_pixelSave.Location = new Point(20, 112);
            dBtn_pixelSave.Name = "dBtn_pixelSave";
            dBtn_pixelSave.Size = new Size(105, 40);
            dBtn_pixelSave.TabIndex = 18;
            dBtn_pixelSave.Text = "저 장";
            dBtn_pixelSave.UseVisualStyleBackColor = true;
            dBtn_pixelSave.Click += dBtn_pixelSave_Click;
            // 
            // controlBoxEdit1
            // 
            controlBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            controlBoxEdit1.BackColor = Color.Transparent;
            controlBoxEdit1.DefaultLocation = true;
            controlBoxEdit1.Location = new Point(531, -1);
            controlBoxEdit1.Name = "controlBoxEdit1";
            controlBoxEdit1.Size = new Size(77, 19);
            controlBoxEdit1.TabIndex = 1;
            controlBoxEdit1.Text = "controlBoxEdit1";
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(612, 261);
            Controls.Add(dungeonForm1);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(261, 65);
            Name = "SettingForm";
            Text = "Settings";
            TransparencyKey = Color.Fuchsia;
            dungeonForm1.ResumeLayout(false);
            thunderGroupBox2.ResumeLayout(false);
            thunderGroupBox2.PerformLayout();
            thunderGroupBox1.ResumeLayout(false);
            thunderGroupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.DungeonForm dungeonForm1;
        private ReaLTaiizor.Controls.ControlBoxEdit controlBoxEdit1;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox1;
        private ReaLTaiizor.Controls.DreamButton dBtn_pixelLoad;
        private ReaLTaiizor.Controls.DreamButton dBtn_pixelSave;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox2;
        private ReaLTaiizor.Controls.DreamButton dBtn_pixelLoad2;
        private ReaLTaiizor.Controls.DreamButton dBtn_pixelSave2;
        private ReaLTaiizor.Controls.CrownTextBox dTbox_y2;
        private ReaLTaiizor.Controls.CrownTextBox dTbox_x2;
        private ReaLTaiizor.Controls.CrownTextBox dTbox_y1;
        private ReaLTaiizor.Controls.CrownTextBox dTbox_x1;
    }
}