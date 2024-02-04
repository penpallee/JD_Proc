namespace JD_Proc
{
    partial class AlignSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlignSettingForm));
            dreamForm1 = new ReaLTaiizor.Forms.DreamForm();
            pictureBox1 = new PictureBox();
            thunderGroupBox2 = new ReaLTaiizor.Controls.ThunderGroupBox();
            thunderGroupBox1 = new ReaLTaiizor.Controls.ThunderGroupBox();
            thunderGroupBox3 = new ReaLTaiizor.Controls.ThunderGroupBox();
            thunderGroupBox4 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dBtn_dw_L = new ReaLTaiizor.Controls.Button();
            button1 = new ReaLTaiizor.Controls.Button();
            dBtn_up_L = new ReaLTaiizor.Controls.Button();
            button2 = new ReaLTaiizor.Controls.Button();
            moonLabel1 = new ReaLTaiizor.Controls.MoonLabel();
            dTbox_grid_L = new ReaLTaiizor.Controls.ForeverTextBox();
            dCheckBox_jog = new ReaLTaiizor.Controls.CyberCheckBox();
            dreamForm1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            thunderGroupBox2.SuspendLayout();
            thunderGroupBox1.SuspendLayout();
            thunderGroupBox3.SuspendLayout();
            thunderGroupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // dreamForm1
            // 
            dreamForm1.BackColor = Color.FromArgb(244, 241, 243);
            dreamForm1.ColorA = Color.FromArgb(40, 218, 255);
            dreamForm1.ColorB = Color.FromArgb(63, 63, 63);
            dreamForm1.ColorC = Color.FromArgb(41, 41, 41);
            dreamForm1.ColorD = Color.FromArgb(27, 27, 27);
            dreamForm1.ColorE = Color.FromArgb(0, 0, 0, 0);
            dreamForm1.ColorF = Color.FromArgb(25, 255, 255, 255);
            dreamForm1.Controls.Add(thunderGroupBox4);
            dreamForm1.Controls.Add(thunderGroupBox3);
            dreamForm1.Controls.Add(thunderGroupBox1);
            dreamForm1.Controls.Add(thunderGroupBox2);
            dreamForm1.Controls.Add(pictureBox1);
            dreamForm1.Dock = DockStyle.Fill;
            dreamForm1.Location = new Point(0, 0);
            dreamForm1.MinimumSize = new Size(261, 65);
            dreamForm1.Name = "dreamForm1";
            dreamForm1.Size = new Size(586, 733);
            dreamForm1.TabIndex = 0;
            dreamForm1.TabStop = false;
            dreamForm1.Text = "Focus/Align Setting";
            dreamForm1.TitleAlign = HorizontalAlignment.Center;
            dreamForm1.TitleHeight = 25;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 45);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(562, 392);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // thunderGroupBox2
            // 
            thunderGroupBox2.BackColor = Color.Transparent;
            thunderGroupBox2.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox2.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox2.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox2.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox2.Controls.Add(moonLabel1);
            thunderGroupBox2.Controls.Add(dTbox_grid_L);
            thunderGroupBox2.ForeColor = Color.WhiteSmoke;
            thunderGroupBox2.Location = new Point(12, 459);
            thunderGroupBox2.Name = "thunderGroupBox2";
            thunderGroupBox2.Size = new Size(284, 125);
            thunderGroupBox2.TabIndex = 2;
            thunderGroupBox2.Text = "       Vision Rule 폭 세팅";
            // 
            // thunderGroupBox1
            // 
            thunderGroupBox1.BackColor = Color.Transparent;
            thunderGroupBox1.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox1.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox1.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox1.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox1.Controls.Add(dBtn_up_L);
            thunderGroupBox1.Controls.Add(dBtn_dw_L);
            thunderGroupBox1.ForeColor = Color.WhiteSmoke;
            thunderGroupBox1.Location = new Point(302, 459);
            thunderGroupBox1.Name = "thunderGroupBox1";
            thunderGroupBox1.Size = new Size(131, 262);
            thunderGroupBox1.TabIndex = 3;
            thunderGroupBox1.Text = "상하폭 조절";
            // 
            // thunderGroupBox3
            // 
            thunderGroupBox3.BackColor = Color.Transparent;
            thunderGroupBox3.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox3.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox3.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox3.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox3.Controls.Add(button2);
            thunderGroupBox3.Controls.Add(button1);
            thunderGroupBox3.ForeColor = Color.WhiteSmoke;
            thunderGroupBox3.Location = new Point(439, 459);
            thunderGroupBox3.Name = "thunderGroupBox3";
            thunderGroupBox3.Size = new Size(135, 262);
            thunderGroupBox3.TabIndex = 4;
            thunderGroupBox3.Text = "Focus 조절";
            // 
            // thunderGroupBox4
            // 
            thunderGroupBox4.BackColor = Color.Transparent;
            thunderGroupBox4.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox4.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox4.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox4.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox4.Controls.Add(dCheckBox_jog);
            thunderGroupBox4.ForeColor = Color.WhiteSmoke;
            thunderGroupBox4.Location = new Point(12, 590);
            thunderGroupBox4.Name = "thunderGroupBox4";
            thunderGroupBox4.Size = new Size(284, 131);
            thunderGroupBox4.TabIndex = 5;
            thunderGroupBox4.Text = "                           Gap Temperature Graph On/Off";
            // 
            // dBtn_dw_L
            // 
            dBtn_dw_L.BackColor = Color.Transparent;
            dBtn_dw_L.BorderColor = Color.FromArgb(32, 34, 37);
            dBtn_dw_L.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            dBtn_dw_L.EnteredColor = Color.FromArgb(32, 34, 37);
            dBtn_dw_L.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dBtn_dw_L.Image = (Image)resources.GetObject("dBtn_dw_L.Image");
            dBtn_dw_L.ImageAlign = ContentAlignment.MiddleCenter;
            dBtn_dw_L.InactiveColor = Color.FromArgb(32, 34, 37);
            dBtn_dw_L.Location = new Point(3, 131);
            dBtn_dw_L.Name = "dBtn_dw_L";
            dBtn_dw_L.PressedBorderColor = Color.FromArgb(165, 37, 37);
            dBtn_dw_L.PressedColor = Color.FromArgb(165, 37, 37);
            dBtn_dw_L.Size = new Size(125, 128);
            dBtn_dw_L.TabIndex = 8;
            dBtn_dw_L.TextAlignment = StringAlignment.Center;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BorderColor = Color.FromArgb(32, 34, 37);
            button1.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button1.EnteredColor = Color.FromArgb(32, 34, 37);
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleCenter;
            button1.InactiveColor = Color.FromArgb(32, 34, 37);
            button1.Location = new Point(0, 131);
            button1.Name = "button1";
            button1.PressedBorderColor = Color.FromArgb(165, 37, 37);
            button1.PressedColor = Color.FromArgb(165, 37, 37);
            button1.Size = new Size(132, 128);
            button1.TabIndex = 9;
            button1.TextAlignment = StringAlignment.Center;
            // 
            // dBtn_up_L
            // 
            dBtn_up_L.BackColor = Color.Transparent;
            dBtn_up_L.BorderColor = Color.FromArgb(32, 34, 37);
            dBtn_up_L.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            dBtn_up_L.EnteredColor = Color.FromArgb(32, 34, 37);
            dBtn_up_L.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dBtn_up_L.Image = (Image)resources.GetObject("dBtn_up_L.Image");
            dBtn_up_L.ImageAlign = ContentAlignment.MiddleCenter;
            dBtn_up_L.InactiveColor = Color.FromArgb(32, 34, 37);
            dBtn_up_L.Location = new Point(3, 24);
            dBtn_up_L.Name = "dBtn_up_L";
            dBtn_up_L.PressedBorderColor = Color.FromArgb(165, 37, 37);
            dBtn_up_L.PressedColor = Color.FromArgb(165, 37, 37);
            dBtn_up_L.Size = new Size(125, 119);
            dBtn_up_L.TabIndex = 9;
            dBtn_up_L.TextAlignment = StringAlignment.Center;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BorderColor = Color.FromArgb(32, 34, 37);
            button2.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button2.EnteredColor = Color.FromArgb(32, 34, 37);
            button2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.ImageAlign = ContentAlignment.MiddleCenter;
            button2.InactiveColor = Color.FromArgb(32, 34, 37);
            button2.Location = new Point(0, 24);
            button2.Name = "button2";
            button2.PressedBorderColor = Color.FromArgb(165, 37, 37);
            button2.PressedColor = Color.FromArgb(165, 37, 37);
            button2.Size = new Size(132, 115);
            button2.TabIndex = 10;
            button2.TextAlignment = StringAlignment.Center;
            // 
            // moonLabel1
            // 
            moonLabel1.AutoSize = true;
            moonLabel1.BackColor = Color.Transparent;
            moonLabel1.ForeColor = Color.Gray;
            moonLabel1.Location = new Point(168, 62);
            moonLabel1.Name = "moonLabel1";
            moonLabel1.Size = new Size(31, 20);
            moonLabel1.TabIndex = 13;
            moonLabel1.Text = "μm";
            // 
            // dTbox_grid_L
            // 
            dTbox_grid_L.BackColor = Color.Transparent;
            dTbox_grid_L.BaseColor = Color.FromArgb(45, 47, 49);
            dTbox_grid_L.BorderColor = Color.FromArgb(242, 93, 75);
            dTbox_grid_L.FocusOnHover = false;
            dTbox_grid_L.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            dTbox_grid_L.ForeColor = Color.FromArgb(192, 192, 192);
            dTbox_grid_L.Location = new Point(26, 55);
            dTbox_grid_L.MaxLength = 32767;
            dTbox_grid_L.Multiline = false;
            dTbox_grid_L.Name = "dTbox_grid_L";
            dTbox_grid_L.ReadOnly = false;
            dTbox_grid_L.Size = new Size(140, 33);
            dTbox_grid_L.TabIndex = 12;
            dTbox_grid_L.Text = "450";
            dTbox_grid_L.TextAlign = HorizontalAlignment.Left;
            dTbox_grid_L.UseSystemPasswordChar = false;
            // 
            // dCheckBox_jog
            // 
            dCheckBox_jog.BackColor = Color.Transparent;
            dCheckBox_jog.Background = true;
            dCheckBox_jog.Background_WidthPen = 2F;
            dCheckBox_jog.BackgroundPen = true;
            dCheckBox_jog.Checked = false;
            dCheckBox_jog.ColorBackground = Color.FromArgb(37, 52, 68);
            dCheckBox_jog.ColorBackground_1 = Color.FromArgb(37, 52, 68);
            dCheckBox_jog.ColorBackground_2 = Color.FromArgb(41, 63, 86);
            dCheckBox_jog.ColorBackground_Pen = Color.FromArgb(29, 200, 238);
            dCheckBox_jog.ColorChecked = Color.FromArgb(29, 200, 238);
            dCheckBox_jog.ColorPen_1 = Color.FromArgb(37, 52, 68);
            dCheckBox_jog.ColorPen_2 = Color.FromArgb(41, 63, 86);
            dCheckBox_jog.CyberCheckBoxStyle = ReaLTaiizor.Enum.Cyber.StateStyle.Custom;
            dCheckBox_jog.Effect_1_ColorBackground = Color.FromArgb(29, 200, 238);
            dCheckBox_jog.Effect_1_Transparency = 25;
            dCheckBox_jog.Effect_2 = true;
            dCheckBox_jog.Effect_2_ColorBackground = Color.White;
            dCheckBox_jog.Effect_2_Transparency = 15;
            dCheckBox_jog.Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point);
            dCheckBox_jog.ForeColor = Color.FromArgb(245, 245, 245);
            dCheckBox_jog.LinearGradient_Background = false;
            dCheckBox_jog.LinearGradientPen = false;
            dCheckBox_jog.Location = new Point(23, 54);
            dCheckBox_jog.Margin = new Padding(4);
            dCheckBox_jog.Name = "dCheckBox_jog";
            dCheckBox_jog.RGB = false;
            dCheckBox_jog.Rounding = true;
            dCheckBox_jog.RoundingInt = 100;
            dCheckBox_jog.Size = new Size(219, 45);
            dCheckBox_jog.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            dCheckBox_jog.TabIndex = 2;
            dCheckBox_jog.Tag = "Cyber";
            dCheckBox_jog.TextButton = "Gap Temperature";
            dCheckBox_jog.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            dCheckBox_jog.Timer_Effect_1 = 1;
            dCheckBox_jog.Timer_RGB = 300;
            // 
            // AlignSettingForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(586, 733);
            Controls.Add(dreamForm1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AlignSettingForm";
            Text = "dungeonForm1";
            dreamForm1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            thunderGroupBox2.ResumeLayout(false);
            thunderGroupBox2.PerformLayout();
            thunderGroupBox1.ResumeLayout(false);
            thunderGroupBox3.ResumeLayout(false);
            thunderGroupBox4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.DreamForm dreamForm1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox2;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox4;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox3;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox1;
        private ReaLTaiizor.Controls.Button dBtn_dw_L;
        private ReaLTaiizor.Controls.Button button1;
        private ReaLTaiizor.Controls.Button button2;
        private ReaLTaiizor.Controls.Button dBtn_up_L;
        private ReaLTaiizor.Controls.MoonLabel moonLabel1;
        private ReaLTaiizor.Controls.ForeverTextBox dTbox_grid_L;
        private ReaLTaiizor.Controls.CyberCheckBox dCheckBox_jog;
    }
}