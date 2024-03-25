
using JD_Proc.Log;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms.VisualStyles;
using static JD_Proc.Log.LogManager;

namespace JD_Proc
{
    public partial class SettingForm : Form
    {
        #region var
        public System.Timers.Timer _timer = new System.Timers.Timer();
        Form1 _form1;
        AlignSettingForm _alignsettingform;
        TempGraphForm _tempgraphform;
        Service.SettingsService service;

        #endregion

        #region 생성자
        public SettingForm(Form1 form1, AlignSettingForm alignSettingForm, TempGraphForm tempgraphform)
        {
            InitializeComponent();
            _form1 = form1;
            _alignsettingform = alignSettingForm;
            _tempgraphform = tempgraphform;
            service = new Service.SettingsService();

            GetResolution_1();
            GetResolution_2();

            _timer = new System.Timers.Timer();
            _timer.Interval = 200;
            _timer.Elapsed += new ElapsedEventHandler(PlcCheckTimer);

            // Developing
            dPanel_login.Visible = false;

            Point parentPoint = this.Location;

            _alignsettingform.Show();
            _alignsettingform.StartPosition = FormStartPosition.Manual;
            _alignsettingform.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            _tempgraphform.Show();
            _tempgraphform.StartPosition = FormStartPosition.Manual;
            _tempgraphform.Location = new Point(this.Location.X, this.Location.Y + this.Height);


            cyberCheckBox1.Checked = true;
            cyberCheckBox2.Checked = true;
            _alignsettingform.Visible = true;
            _tempgraphform.Visible = true;


        }
        #endregion

        #region event(PLC Check) Timer
        void PlcCheckTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                // plc -> vision
                int plcAuto = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B101", out plcAuto);

                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (plcAuto == 0)
                        dPanel_plcAuto.BackColor = Color.Red;
                    else
                        dPanel_plcAuto.BackColor = Color.Lime;
                }));

                int cam1Start = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B110", out cam1Start);

                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (cam1Start == 0)
                        dPanel_plcCam1Cap.BackColor = Color.Red;
                    else
                        dPanel_plcCam1Cap.BackColor = Color.Lime;
                }));

                int cam2Start = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B111", out cam2Start);

                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (cam2Start == 0)
                        dPanel_plcCam2Cap.BackColor = Color.Red;
                    else
                        dPanel_plcCam2Cap.BackColor = Color.Lime;
                }));
            }
            catch
            {
                LogManager log = new LogManager(@"C:\JD\log\", LogType.Daily);
                log.WriteLine("셋팅 페이지에서 PLC 통신 에러");
            }

        }
        #endregion

        #region event(resolution) - click
        private void dBtn_pixelSave_Click(object sender, EventArgs e)
        {
            string x = dTbox_x1.Text;
            string y = dTbox_y1.Text;

            Service.SettingsService service = new Service.SettingsService();

            service.Write("resolution", "x_1", x);
            service.Write("resolution", "y_1", y);
        }

        private void dBtn_pixelSave2_Click(object sender, EventArgs e)
        {
            string x = dTbox_x2.Text;
            string y = dTbox_y2.Text;

            Service.SettingsService service = new Service.SettingsService();

            service.Write("resolution", "x_2", x);
            service.Write("resolution", "y_2", y);
        }

        private void dBtn_pixelLoad_Click(object sender, EventArgs e)
        {
            GetResolution_1();
        }

        private void dBtn_pixelLoad2_Click(object sender, EventArgs e)
        {
            GetResolution_2();
        }
        #endregion

        #region event(plc) - click

        private void dPanel_visionCam1Auto_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam1Auto.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B3", short.Parse("1"));

                dPanel_visionCam1Auto.BackColor = Color.Lime;
                dPanel_visionCam2Auto.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1Auto.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B3", short.Parse("0"));

                dPanel_visionCam1Auto.BackColor = Color.Red;
                dPanel_visionCam2Auto.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam1Ready_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam1Ready.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B10", short.Parse("1"));

                dPanel_visionCam1Ready.BackColor = Color.Lime;
                dPanel_visionCam1Ready.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1Ready.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B10", short.Parse("0"));

                dPanel_visionCam1Ready.BackColor = Color.Red;
                dPanel_visionCam1Ready.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam1Busy_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam1Busy.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B11", short.Parse("1"));

                dPanel_visionCam1Busy.BackColor = Color.Lime;
                dPanel_visionCam1Busy.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1Busy.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B11", short.Parse("0"));

                dPanel_visionCam1Busy.BackColor = Color.Red;
                dPanel_visionCam1Busy.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam1End_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam1End.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B12", short.Parse("1"));

                dPanel_visionCam1End.BackColor = Color.Lime;
                dPanel_visionCam1End.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1End.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B12", short.Parse("0"));

                dPanel_visionCam1End.BackColor = Color.Red;
                dPanel_visionCam1End.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam2Auto_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam2Auto.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice2("B3", short.Parse("1"));

                dPanel_visionCam1Auto.BackColor = Color.Lime;
                dPanel_visionCam2Auto.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam2Auto.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice2("B3", short.Parse("0"));

                dPanel_visionCam1Auto.BackColor = Color.Red;
                dPanel_visionCam2Auto.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam2Ready_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam2Ready.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B13", short.Parse("1"));

                dPanel_visionCam2Ready.BackColor = Color.Lime;
                dPanel_visionCam2Ready.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam2Ready.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B13", short.Parse("0"));

                dPanel_visionCam2Ready.BackColor = Color.Red;
                dPanel_visionCam2Ready.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam2Busy_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam2Busy.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B14", short.Parse("1"));

                dPanel_visionCam2Busy.BackColor = Color.Lime;
                dPanel_visionCam2Busy.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam2Busy.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B14", short.Parse("0"));

                dPanel_visionCam2Busy.BackColor = Color.Red;
                dPanel_visionCam2Busy.BackColor = Color.Red;
            }
        }

        private void dPanel_visionCam2End_Click(object sender, EventArgs e)
        {
            if (dPanel_visionCam2End.BackColor == Color.Red)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B15", short.Parse("1"));

                dPanel_visionCam2End.BackColor = Color.Lime;
                dPanel_visionCam2End.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam2End.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B15", short.Parse("0"));

                dPanel_visionCam2End.BackColor = Color.Red;
                dPanel_visionCam2End.BackColor = Color.Red;
            }
        }
        #endregion

        #region event(login) - click
        private void dButton_login_Click(object sender, EventArgs e)
        {
            Service.SettingsService settingService = new Service.SettingsService();

            string realPassword = settingService.Read("password", "password");

            string password = dTxtBox_password.TextButton;
            if (password == realPassword)
            {
                dPanel_login.Visible = false;
            }
        }
        #endregion

        #region event(form) - closing
        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            _alignsettingform.Close();
            _tempgraphform.Close();
        }
        #endregion

        #region method - resolution 값 로드
        void GetResolution_1()
        {
            Service.SettingsService service = new Service.SettingsService();

            dTbox_x1.Text = service.Read("resolution", "x_1");
            dTbox_y1.Text = service.Read("resolution", "y_1");
        }

        void GetResolution_2()
        {
            Service.SettingsService service = new Service.SettingsService();

            dTbox_x2.Text = service.Read("resolution", "x_2");
            dTbox_y2.Text = service.Read("resolution", "y_2");
        }

        #endregion

        #region method - Set PLC
        public void SetPlc()
        {
            try
            {
                // plc -> vision
                int plcAuto = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B101", out plcAuto);
                if (plcAuto == 0) dPanel_plcAuto.BackColor = Color.Red;
                else dPanel_plcAuto.BackColor = Color.Lime;

                int cam1Start = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B103", out cam1Start);
                if (cam1Start == 0) dPanel_plcCam1Cap.BackColor = Color.Red;
                else dPanel_plcCam1Cap.BackColor = Color.Lime;

                int cam2Start = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B104", out cam2Start);

                if (cam2Start == 0) dPanel_plcCam2Cap.BackColor = Color.Red;
                else dPanel_plcCam2Cap.BackColor = Color.Lime;

                // vision -> plc ...공통
                int visionAuto = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B3", out visionAuto);
                if (visionAuto == 0) dPanel_visionCam1Auto.BackColor = Color.Red;
                else dPanel_visionCam1Auto.BackColor = Color.Lime;
                if (visionAuto == 0) dPanel_visionCam2Auto.BackColor = Color.Red;
                else dPanel_visionCam2Auto.BackColor = Color.Lime;

                // vision -> plc ...cam1
                int cam1Ready = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B4", out cam1Ready);
                if (cam1Ready == 0) dPanel_visionCam1Ready.BackColor = Color.Red;
                else dPanel_visionCam1Ready.BackColor = Color.Lime;

                int cam1Busy = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B5", out cam1Busy);
                if (cam1Busy == 0) dPanel_visionCam1Busy.BackColor = Color.Red;
                else dPanel_visionCam1Busy.BackColor = Color.Lime;

                int cam1End = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B6", out cam1End);
                if (cam1End == 0) dPanel_visionCam1End.BackColor = Color.Red;
                else dPanel_visionCam1End.BackColor = Color.Lime;

                // vision -> plc ...cam2
                int cam2Ready = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B7", out cam2Ready);
                if (cam2Ready == 0) dPanel_visionCam2Ready.BackColor = Color.Red;
                else dPanel_visionCam2Ready.BackColor = Color.Lime;

                int cam2Busy = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B8", out cam2Busy);
                if (cam2Busy == 0) dPanel_visionCam2Busy.BackColor = Color.Red;
                else dPanel_visionCam2Busy.BackColor = Color.Lime;

                int cam2End = int.MaxValue;
                Form1._MELSEC.actUtlType64.GetDevice("B9", out cam2End);
                if (cam2End == 0) dPanel_visionCam2End.BackColor = Color.Red;
                else dPanel_visionCam2End.BackColor = Color.Lime;
            }
            catch
            {
                LogManager log = new LogManager(@"C:\JD\log\", LogType.Daily);
                log.WriteLine("셋팅 페이지에서 PLC 통신 에러");
            }
        }
        #endregion

        #region 잡동사니
        private void dungeonForm1_Click(object sender, EventArgs e)
        {

        }

        private void dPanel_visionCam1Auto_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dCheckBox_jog_Load(object sender, EventArgs e)
        {

        }
        private void dCheckBox_jog_VisibleChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region event(Additional Window) - On/Off
        private void cyberCheckBox1_CheckedChanged()
        {
            if (cyberCheckBox1.Checked != true)
                _alignsettingform.Visible = false;
            else _alignsettingform.Visible = true;

        }

        private void cyberCheckBox2_CheckedChanged()
        {
            if (cyberCheckBox2.Checked != true)
                _tempgraphform.Visible = false;
            else _tempgraphform.Visible = true;
        }
        #endregion

        private void SettingForm_Move(object sender, EventArgs e)
        {
            _tempgraphform.Location = new Point(this.Location.X, this.Location.Y + this.Height);
            _alignsettingform.Location = new Point(this.Location.X + this.Width, this.Location.Y);
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            if (Form1.MELSEC_JOG == null || Form1.MELSEC_JOG.IsConnected() == false)
            {
                Btn_Jog_L_Y.Enabled = false;
                Btn_Jog_L_Z.Enabled = false;
                Btn_Jog_R_Y.Enabled = false;
                Btn_Jog_R_Z.Enabled = false;
                Btn_JogOriginal.Enabled = false;
                Btn_JogReverse.Enabled = false;
            }
        }

        private void Btn_SetJogVelocity_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetVelocityValue(int.Parse(TxtBox_JogVelocity.Text));
        }

        private void Btn_SetJogPosition_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetPositionValue(int.Parse(TxtBox_JogPosition.Text));
        }

        private void Btn_MoveToPosition_Click(object sender, EventArgs e)
        {
            _form1.selectedJogMoveToInputPosition();
        }

        private void Btn_JogValueSave_Click(object sender, EventArgs e)
        {
            service.Write("PLC_JOG_OFFSET", "VELOCITY", TxtBox_JogVelocity.Text);
            service.Write("PLC_JOG_OFFSET", "POSITION", TxtBox_JogPosition.Text);
        }

        private void Btn_AutoMoveToSavePosition_Click(object sender, EventArgs e)
        {
            TxtBox_JogVelocity.Text = service.Read("PLC_JOG_OFFSET", "VELOCITY");
            TxtBox_JogPosition.Text = service.Read("PLC_JOG_OFFSET", "POSITION");
            _form1.PLCJogAutoMoveToSavedValue();
        }

        #region [event - MouseClick Jog Button]
        private void Btn_JogOriginal_MouseDown(object sender, MouseEventArgs e)
        {
            _form1.jogButtonOriginalClickDown();
        }

        private void Btn_JogOriginal_MouseUp(object sender, MouseEventArgs e)
        {
            _form1.jogButtonOriginalClickUp();
        }

        private void Btn_JogReverse_MouseDown(object sender, MouseEventArgs e)
        {
            _form1.jogButtonReverseClickDown();
        }

        private void Btn_JogReverse_MouseUp(object sender, MouseEventArgs e)
        {
            _form1.jogButtonReverseClickUp();
        }
        #endregion

        #region [event - Select Jog Axis Button]
        private void Btn_Jog_L_Y_Click(object sender, EventArgs e)
        {
            _form1.selectJogAxis_L_Y();
        }

        private void Btn_Jog_L_Z_Click(object sender, EventArgs e)
        {
            _form1.selectJogAxis_L_Z();
        }

        private void Btn_Jog_R_Y_Click(object sender, EventArgs e)
        {
            _form1.selectJogAxis_R_Y();
        }

        private void Btn_Jog_R_Z_Click(object sender, EventArgs e)
        {
            _form1.selectJogAxis_R_Z();
        }
        #endregion

        #region [event - JogButton]
        private void Btn_JogOriginal_MouseDown_1(object sender, MouseEventArgs e)
        {
            _form1.jogButtonOriginalClickDown();
        }

        private void Btn_JogOriginal_MouseUp_1(object sender, MouseEventArgs e)
        {
            _form1.jogButtonOriginalClickUp();
        }

        private void Btn_JogReverse_MouseDown_1(object sender, MouseEventArgs e)
        {
            _form1.jogButtonReverseClickDown();
        }

        private void Btn_JogReverse_MouseUp_1(object sender, MouseEventArgs e)
        {
            _form1.jogButtonReverseClickUp();
        }
        #endregion
    }
}

