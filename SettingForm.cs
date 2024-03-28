using JD_Proc.PLC;

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
        PLC.Melsec _SettingMelsec;
        System.Threading.Timer _SettingJogTimer;
        
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

            _SettingMelsec = new PLC.Melsec(int.Parse(service.Read("PLC_LOGICAL_STATION_NUMBER", "PLC_LOGICAL_STATION_NUMBER")));
            


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



        #region event(resolution) - click
        private void dBtn_pixelSave_Click(object sender, EventArgs e)
        {
            string x = dTbox_x1.Text;
            string y = dTbox_y1.Text;
            string CValue = dTbox_CValue.Text;

            Service.SettingsService service = new Service.SettingsService();

            service.Write("resolution", "x_1", x);
            service.Write("resolution", "y_1", y);
            service.Write("Calibration", "L_Value", CValue);
        }

        private void dBtn_pixelSave2_Click(object sender, EventArgs e)
        {
            string x = dTbox_x2.Text;
            string y = dTbox_y2.Text;
            string CValue = dTbox_CValue2.Text;

            Service.SettingsService service = new Service.SettingsService();

            service.Write("resolution", "x_2", x);
            service.Write("resolution", "y_2", y);
            service.Write("Calibration", "R_Value", CValue);
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
            dTbox_CValue.Text = service.Read("Calibration", "L_Value");
        }

        void GetResolution_2()
        {
            Service.SettingsService service = new Service.SettingsService();

            dTbox_x2.Text = service.Read("resolution", "x_2");
            dTbox_y2.Text = service.Read("resolution", "y_2");
            dTbox_CValue2.Text = service.Read("Calibration", "R_Value");


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

                TxtBox_JogVelocity_LY.Enabled = false;
                TxtBox_JogPosition_LY.Enabled = false;

                TxtBox_JogVelocity_LZ.Enabled = false;
                TxtBox_JogPosition_LZ.Enabled = false;

                TxtBox_JogVelocity_RY.Enabled = false;
                TxtBox_JogPosition_RY.Enabled = false;

                TxtBox_JogVelocity_RZ.Enabled = false;
                Btn_SetJogPosition_RZ.Enabled = false;


            }
            else
            {
                _SettingJogTimer = new System.Threading.Timer(SettingJogTimer_Check, null, 0, 500);

                
            }
        }



        private void Btn_MoveToPosition_Click(object sender, EventArgs e)
        {
            _form1.selectedJogMoveToInputPosition();
        }

        private void Btn_JogValueSave_Click(object sender, EventArgs e)
        {
            service.Write("PLC_JOG_OFFSET", "L_Y_VELOCITY", TxtBox_JogVelocity_LY.Text);
            service.Write("PLC_JOG_OFFSET", "L_Y_POSITION", TxtBox_JogPosition_LY.Text);

            service.Write("PLC_JOG_OFFSET", "L_Z_VELOCITY", TxtBox_JogVelocity_LZ.Text);
            service.Write("PLC_JOG_OFFSET", "L_Z_POSITION", TxtBox_JogPosition_LZ.Text);

            service.Write("PLC_JOG_OFFSET", "R_Y_VELOCITY", TxtBox_JogVelocity_RY.Text);
            service.Write("PLC_JOG_OFFSET", "R_Y_POSITION", TxtBox_JogPosition_RY.Text);

            service.Write("PLC_JOG_OFFSET", "R_Z_VELOCITY", TxtBox_JogVelocity_RZ.Text);
            service.Write("PLC_JOG_OFFSET", "R_Z_POSITION", Btn_SetJogPosition_RZ.Text);
        }

        private void Btn_AutoMoveToSavePosition_Click(object sender, EventArgs e)
        {
            TxtBox_JogVelocity_LY.Text = service.Read("PLC_JOG_OFFSET", "VELOCITY");
            TxtBox_JogPosition_LY.Text = service.Read("PLC_JOG_OFFSET", "POSITION");
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

        #region [event - JogSetBtnClick]
        private void Btn_SetJogVelocity_LY_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetVelocityValue(int.Parse(TxtBox_JogVelocity_LY.Text));
        }

        private void Btn_SetJogPosition_LY_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetPositionValue(int.Parse(TxtBox_JogPosition_LY.Text));
        }

        private void Btn_SetJogVelocity_LZ_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetVelocityValue(int.Parse(TxtBox_JogVelocity_LZ.Text));
        }

        private void Btn_SetJogPosition_LZ_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetPositionValue(int.Parse(TxtBox_JogPosition_LZ.Text));
        }

        private void Btn_SetJogVelocity_RY_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetVelocityValue(int.Parse(TxtBox_JogVelocity_RY.Text));
        }

        private void Btn_SetJogPosition_RY_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetPositionValue(int.Parse(TxtBox_JogPosition_RY.Text));
        }

        private void Btn_SetJogVelocity_RZ_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetVelocityValue(int.Parse(TxtBox_JogVelocity_RZ.Text));
        }

        private void TxtBox_JogPosition_RZ_Click(object sender, EventArgs e)
        {
            _form1.PLCMotorSetPositionValue(int.Parse(Btn_SetJogPosition_RZ.Text));
        }
        #endregion

        private void SettingJogTimer_Check(object? state)
        {
            int Pos;
            int Val;

            
            _SettingMelsec.actUtlType64.ReadDeviceBlock("W110", 2, out Pos);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                TxtBox_JogCurrPosition_LY.Text = Pos.ToString();
            }));
            _SettingMelsec.actUtlType64.ReadDeviceBlock("W112", 2, out Pos);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                TxtBox_JogCurrPosition_LZ.Text = Pos.ToString();
            }));
            _SettingMelsec.actUtlType64.ReadDeviceBlock("W114", 2, out Pos);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                TxtBox_JogCurrPosition_RY.Text = Pos.ToString();
            }));
            _SettingMelsec.actUtlType64.ReadDeviceBlock("W116", 2, out Pos);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                TxtBox_JogCurrPosition_RZ.Text = Pos.ToString();
            }));




            _SettingMelsec.actUtlType64.GetDevice("B120", out Val);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (Val == 1) Btn_Jog_L_Y.BorderColor = Color.IndianRed;
                else
                {
                    Btn_Jog_L_Y.BorderColor = Color.Transparent;
                    TxtBox_JogVelocity_LY.Enabled = false;
                    TxtBox_JogPosition_LY.Enabled = false;
                }
            }));
            _SettingMelsec.actUtlType64.GetDevice("B121", out Val);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (Val == 1) Btn_Jog_L_Z.BorderColor = Color.IndianRed;
                else
                {
                    TxtBox_JogVelocity_LZ.Enabled = false;
                    TxtBox_JogPosition_LZ.Enabled = false;
                    Btn_Jog_L_Z.BorderColor = Color.Transparent;
                }
            }));
            _SettingMelsec.actUtlType64.GetDevice("B122", out Val);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (Val == 1) Btn_Jog_R_Y.BorderColor = Color.IndianRed;
                else
                {
                    TxtBox_JogVelocity_RY.Enabled = false;
                    TxtBox_JogPosition_RY.Enabled = false;
                    Btn_Jog_R_Y.BorderColor = Color.Transparent;
                }
            }));
            _SettingMelsec.actUtlType64.GetDevice("B123", out Val);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (Val == 1) Btn_Jog_R_Z.BorderColor = Color.IndianRed;
                else
                {
                    TxtBox_JogVelocity_RZ.Enabled = false;
                    Btn_SetJogPosition_RZ.Enabled = false;
                    Btn_Jog_R_Z.BorderColor = Color.Transparent;
                }
            }));
        }
    }
}

