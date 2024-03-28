using System.Windows.Forms.DataVisualization.Charting;

namespace JD_Proc
{
    partial class TempGraphForm
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
            Label_AutoSignal = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            thunderGroupBox1 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dPanel_plcCam2Cap = new Panel();
            foxLabel1 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_plcCam1Cap = new Panel();
            foxLabel2 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel3 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_plcAuto = new Panel();
            thunderGroupBox3 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dPanel_visionCam2End = new Panel();
            foxLabel8 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam2Busy = new Panel();
            foxLabel9 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam2Ready = new Panel();
            foxLabel10 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel11 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam2Auto = new Panel();
            thunderGroupBox2 = new ReaLTaiizor.Controls.ThunderGroupBox();
            dPanel_visionCam1End = new Panel();
            foxLabel4 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam1Busy = new Panel();
            foxLabel5 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam1Ready = new Panel();
            foxLabel6 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel7 = new ReaLTaiizor.Controls.FoxLabel();
            dPanel_visionCam1Auto = new Panel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            thunderGroupBox1.SuspendLayout();
            thunderGroupBox3.SuspendLayout();
            thunderGroupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // Label_AutoSignal
            // 
            Label_AutoSignal.Anchor = AnchorStyles.None;
            Label_AutoSignal.AutoSize = true;
            Label_AutoSignal.BackColor = Color.Transparent;
            Label_AutoSignal.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point);
            Label_AutoSignal.ForeColor = Color.FromArgb(224, 224, 224);
            Label_AutoSignal.Location = new Point(434, 4);
            Label_AutoSignal.Name = "Label_AutoSignal";
            Label_AutoSignal.Size = new Size(200, 45);
            Label_AutoSignal.TabIndex = 1;
            Label_AutoSignal.Text = "Auto Signal";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.ActiveCaptionText;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Label_AutoSignal, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            tableLayoutPanel1.Size = new Size(1068, 365);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(thunderGroupBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(thunderGroupBox3, 1, 1);
            tableLayoutPanel2.Controls.Add(thunderGroupBox2, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 57);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1062, 305);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // thunderGroupBox1
            // 
            thunderGroupBox1.BackColor = Color.Transparent;
            thunderGroupBox1.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox1.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox1.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox1.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox1.Controls.Add(dPanel_plcCam2Cap);
            thunderGroupBox1.Controls.Add(foxLabel1);
            thunderGroupBox1.Controls.Add(dPanel_plcCam1Cap);
            thunderGroupBox1.Controls.Add(foxLabel2);
            thunderGroupBox1.Controls.Add(foxLabel3);
            thunderGroupBox1.Controls.Add(dPanel_plcAuto);
            thunderGroupBox1.Dock = DockStyle.Fill;
            thunderGroupBox1.ForeColor = Color.WhiteSmoke;
            thunderGroupBox1.Location = new Point(3, 3);
            thunderGroupBox1.Name = "thunderGroupBox1";
            thunderGroupBox1.Size = new Size(525, 146);
            thunderGroupBox1.TabIndex = 21;
            thunderGroupBox1.Text = "통신 PLC (IN)";
            // 
            // dPanel_plcCam2Cap
            // 
            dPanel_plcCam2Cap.BackColor = Color.Red;
            dPanel_plcCam2Cap.Location = new Point(195, 66);
            dPanel_plcCam2Cap.Name = "dPanel_plcCam2Cap";
            dPanel_plcCam2Cap.Size = new Size(35, 24);
            dPanel_plcCam2Cap.TabIndex = 5;
            // 
            // foxLabel1
            // 
            foxLabel1.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel1.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel1.Location = new Point(180, 35);
            foxLabel1.Margin = new Padding(3, 10, 3, 3);
            foxLabel1.Name = "foxLabel1";
            foxLabel1.Size = new Size(76, 20);
            foxLabel1.TabIndex = 6;
            foxLabel1.Text = "C2 CapStart";
            // 
            // dPanel_plcCam1Cap
            // 
            dPanel_plcCam1Cap.BackColor = Color.Red;
            dPanel_plcCam1Cap.Location = new Point(109, 66);
            dPanel_plcCam1Cap.Name = "dPanel_plcCam1Cap";
            dPanel_plcCam1Cap.Size = new Size(35, 24);
            dPanel_plcCam1Cap.TabIndex = 1;
            // 
            // foxLabel2
            // 
            foxLabel2.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel2.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel2.Location = new Point(90, 35);
            foxLabel2.Margin = new Padding(3, 10, 3, 3);
            foxLabel2.Name = "foxLabel2";
            foxLabel2.Size = new Size(73, 17);
            foxLabel2.TabIndex = 4;
            foxLabel2.Text = "C1 CapStart";
            // 
            // foxLabel3
            // 
            foxLabel3.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel3.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel3.Location = new Point(20, 31);
            foxLabel3.Margin = new Padding(3, 10, 3, 3);
            foxLabel3.Name = "foxLabel3";
            foxLabel3.Size = new Size(48, 29);
            foxLabel3.TabIndex = 3;
            foxLabel3.Text = "Auto";
            // 
            // dPanel_plcAuto
            // 
            dPanel_plcAuto.BackColor = Color.Red;
            dPanel_plcAuto.Location = new Point(20, 66);
            dPanel_plcAuto.Name = "dPanel_plcAuto";
            dPanel_plcAuto.Size = new Size(35, 24);
            dPanel_plcAuto.TabIndex = 0;
            // 
            // thunderGroupBox3
            // 
            thunderGroupBox3.BackColor = Color.Transparent;
            thunderGroupBox3.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox3.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox3.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox3.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox3.Controls.Add(dPanel_visionCam2End);
            thunderGroupBox3.Controls.Add(foxLabel8);
            thunderGroupBox3.Controls.Add(dPanel_visionCam2Busy);
            thunderGroupBox3.Controls.Add(foxLabel9);
            thunderGroupBox3.Controls.Add(dPanel_visionCam2Ready);
            thunderGroupBox3.Controls.Add(foxLabel10);
            thunderGroupBox3.Controls.Add(foxLabel11);
            thunderGroupBox3.Controls.Add(dPanel_visionCam2Auto);
            thunderGroupBox3.Dock = DockStyle.Fill;
            thunderGroupBox3.ForeColor = Color.WhiteSmoke;
            thunderGroupBox3.Location = new Point(534, 155);
            thunderGroupBox3.Name = "thunderGroupBox3";
            thunderGroupBox3.Size = new Size(525, 147);
            thunderGroupBox3.TabIndex = 23;
            thunderGroupBox3.Text = "통신 CAM 2 (OUT)";
            // 
            // dPanel_visionCam2End
            // 
            dPanel_visionCam2End.BackColor = Color.Red;
            dPanel_visionCam2End.Location = new Point(201, 66);
            dPanel_visionCam2End.Name = "dPanel_visionCam2End";
            dPanel_visionCam2End.Size = new Size(35, 24);
            dPanel_visionCam2End.TabIndex = 7;
            // 
            // foxLabel8
            // 
            foxLabel8.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel8.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel8.Location = new Point(200, 31);
            foxLabel8.Margin = new Padding(3, 10, 3, 3);
            foxLabel8.Name = "foxLabel8";
            foxLabel8.Size = new Size(59, 29);
            foxLabel8.TabIndex = 8;
            foxLabel8.Text = "End";
            // 
            // dPanel_visionCam2Busy
            // 
            dPanel_visionCam2Busy.BackColor = Color.Red;
            dPanel_visionCam2Busy.Location = new Point(143, 66);
            dPanel_visionCam2Busy.Name = "dPanel_visionCam2Busy";
            dPanel_visionCam2Busy.Size = new Size(35, 24);
            dPanel_visionCam2Busy.TabIndex = 5;
            // 
            // foxLabel9
            // 
            foxLabel9.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel9.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel9.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel9.Location = new Point(142, 31);
            foxLabel9.Margin = new Padding(3, 10, 3, 3);
            foxLabel9.Name = "foxLabel9";
            foxLabel9.Size = new Size(59, 29);
            foxLabel9.TabIndex = 6;
            foxLabel9.Text = "Busy";
            // 
            // dPanel_visionCam2Ready
            // 
            dPanel_visionCam2Ready.BackColor = Color.Red;
            dPanel_visionCam2Ready.Location = new Point(81, 66);
            dPanel_visionCam2Ready.Name = "dPanel_visionCam2Ready";
            dPanel_visionCam2Ready.Size = new Size(35, 24);
            dPanel_visionCam2Ready.TabIndex = 1;
            // 
            // foxLabel10
            // 
            foxLabel10.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel10.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel10.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel10.Location = new Point(80, 31);
            foxLabel10.Margin = new Padding(3, 10, 3, 3);
            foxLabel10.Name = "foxLabel10";
            foxLabel10.Size = new Size(59, 29);
            foxLabel10.TabIndex = 4;
            foxLabel10.Text = "Ready";
            // 
            // foxLabel11
            // 
            foxLabel11.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel11.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel11.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel11.Location = new Point(20, 31);
            foxLabel11.Margin = new Padding(3, 10, 3, 3);
            foxLabel11.Name = "foxLabel11";
            foxLabel11.Size = new Size(48, 29);
            foxLabel11.TabIndex = 3;
            foxLabel11.Text = "Auto";
            // 
            // dPanel_visionCam2Auto
            // 
            dPanel_visionCam2Auto.BackColor = Color.Red;
            dPanel_visionCam2Auto.Location = new Point(20, 66);
            dPanel_visionCam2Auto.Name = "dPanel_visionCam2Auto";
            dPanel_visionCam2Auto.Size = new Size(35, 24);
            dPanel_visionCam2Auto.TabIndex = 0;
            // 
            // thunderGroupBox2
            // 
            thunderGroupBox2.BackColor = Color.Transparent;
            thunderGroupBox2.BodyColorA = Color.FromArgb(26, 26, 26);
            thunderGroupBox2.BodyColorB = Color.FromArgb(30, 30, 30);
            thunderGroupBox2.BodyColorC = Color.FromArgb(46, 46, 46);
            thunderGroupBox2.BodyColorD = Color.FromArgb(50, 55, 58);
            thunderGroupBox2.Controls.Add(dPanel_visionCam1End);
            thunderGroupBox2.Controls.Add(foxLabel4);
            thunderGroupBox2.Controls.Add(dPanel_visionCam1Busy);
            thunderGroupBox2.Controls.Add(foxLabel5);
            thunderGroupBox2.Controls.Add(dPanel_visionCam1Ready);
            thunderGroupBox2.Controls.Add(foxLabel6);
            thunderGroupBox2.Controls.Add(foxLabel7);
            thunderGroupBox2.Controls.Add(dPanel_visionCam1Auto);
            thunderGroupBox2.Dock = DockStyle.Fill;
            thunderGroupBox2.ForeColor = Color.WhiteSmoke;
            thunderGroupBox2.Location = new Point(3, 155);
            thunderGroupBox2.Name = "thunderGroupBox2";
            thunderGroupBox2.Size = new Size(525, 147);
            thunderGroupBox2.TabIndex = 22;
            thunderGroupBox2.Text = "통신 CAM 1 (OUT)";
            // 
            // dPanel_visionCam1End
            // 
            dPanel_visionCam1End.BackColor = Color.Red;
            dPanel_visionCam1End.Location = new Point(201, 66);
            dPanel_visionCam1End.Name = "dPanel_visionCam1End";
            dPanel_visionCam1End.Size = new Size(35, 24);
            dPanel_visionCam1End.TabIndex = 7;
            // 
            // foxLabel4
            // 
            foxLabel4.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel4.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel4.Location = new Point(200, 31);
            foxLabel4.Margin = new Padding(3, 10, 3, 3);
            foxLabel4.Name = "foxLabel4";
            foxLabel4.Size = new Size(59, 29);
            foxLabel4.TabIndex = 8;
            foxLabel4.Text = "End";
            // 
            // dPanel_visionCam1Busy
            // 
            dPanel_visionCam1Busy.BackColor = Color.Red;
            dPanel_visionCam1Busy.Location = new Point(143, 66);
            dPanel_visionCam1Busy.Name = "dPanel_visionCam1Busy";
            dPanel_visionCam1Busy.Size = new Size(35, 24);
            dPanel_visionCam1Busy.TabIndex = 5;
            dPanel_visionCam1Busy.Click += dPanel_visionCam1Busy_Click;
            // 
            // foxLabel5
            // 
            foxLabel5.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel5.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel5.Location = new Point(142, 31);
            foxLabel5.Margin = new Padding(3, 10, 3, 3);
            foxLabel5.Name = "foxLabel5";
            foxLabel5.Size = new Size(59, 29);
            foxLabel5.TabIndex = 6;
            foxLabel5.Text = "Busy";
            // 
            // dPanel_visionCam1Ready
            // 
            dPanel_visionCam1Ready.BackColor = Color.Red;
            dPanel_visionCam1Ready.Location = new Point(80, 66);
            dPanel_visionCam1Ready.Name = "dPanel_visionCam1Ready";
            dPanel_visionCam1Ready.Size = new Size(35, 24);
            dPanel_visionCam1Ready.TabIndex = 1;
            dPanel_visionCam1Ready.Click += dPanel_visionCam1Ready_Click;
            // 
            // foxLabel6
            // 
            foxLabel6.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel6.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel6.Location = new Point(80, 31);
            foxLabel6.Margin = new Padding(3, 10, 3, 3);
            foxLabel6.Name = "foxLabel6";
            foxLabel6.Size = new Size(59, 29);
            foxLabel6.TabIndex = 4;
            foxLabel6.Text = "Ready";
            // 
            // foxLabel7
            // 
            foxLabel7.BackColor = Color.FromArgb(46, 46, 46);
            foxLabel7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            foxLabel7.ForeColor = Color.FromArgb(29, 200, 238);
            foxLabel7.Location = new Point(20, 31);
            foxLabel7.Margin = new Padding(3, 10, 3, 3);
            foxLabel7.Name = "foxLabel7";
            foxLabel7.Size = new Size(48, 29);
            foxLabel7.TabIndex = 3;
            foxLabel7.Text = "Auto";
            // 
            // dPanel_visionCam1Auto
            // 
            dPanel_visionCam1Auto.BackColor = Color.Red;
            dPanel_visionCam1Auto.Location = new Point(20, 66);
            dPanel_visionCam1Auto.Name = "dPanel_visionCam1Auto";
            dPanel_visionCam1Auto.Size = new Size(35, 24);
            dPanel_visionCam1Auto.TabIndex = 0;
            dPanel_visionCam1Auto.Click += dPanel_visionCam1Auto_Click;
            // 
            // TempGraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1068, 365);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MinimumSize = new Size(203, 49);
            Name = "TempGraphForm";
            Text = "Gap Temperature Graph";
            TransparencyKey = Color.Fuchsia;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            thunderGroupBox1.ResumeLayout(false);
            thunderGroupBox3.ResumeLayout(false);
            thunderGroupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.BigLabel Label_AutoSignal;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox1;
        private Panel dPanel_plcCam2Cap;
        private ReaLTaiizor.Controls.FoxLabel foxLabel1;
        private Panel dPanel_plcCam1Cap;
        private ReaLTaiizor.Controls.FoxLabel foxLabel2;
        private ReaLTaiizor.Controls.FoxLabel foxLabel3;
        private Panel dPanel_plcAuto;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox3;
        private Panel dPanel_visionCam2End;
        private ReaLTaiizor.Controls.FoxLabel foxLabel8;
        private Panel dPanel_visionCam2Busy;
        private ReaLTaiizor.Controls.FoxLabel foxLabel9;
        private Panel dPanel_visionCam2Ready;
        private ReaLTaiizor.Controls.FoxLabel foxLabel10;
        private ReaLTaiizor.Controls.FoxLabel foxLabel11;
        private Panel dPanel_visionCam2Auto;
        private ReaLTaiizor.Controls.ThunderGroupBox thunderGroupBox2;
        private Panel dPanel_visionCam1End;
        private ReaLTaiizor.Controls.FoxLabel foxLabel4;
        private Panel dPanel_visionCam1Busy;
        private ReaLTaiizor.Controls.FoxLabel foxLabel5;
        private Panel dPanel_visionCam1Ready;
        private ReaLTaiizor.Controls.FoxLabel foxLabel6;
        private ReaLTaiizor.Controls.FoxLabel foxLabel7;
        private Panel dPanel_visionCam1Auto;
    }
}