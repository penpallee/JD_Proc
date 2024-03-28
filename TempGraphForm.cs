using JD_Proc.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static JD_Proc.Log.LogManager;

namespace JD_Proc
{
    public partial class TempGraphForm : Form
    {
        public System.Timers.Timer _timer = new System.Timers.Timer();

        public TempGraphForm()
        {
            InitializeComponent();

            _timer = new System.Timers.Timer();
            _timer.Interval = 200;
            _timer.Elapsed += new ElapsedEventHandler(PlcCheckTimer);
        }

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
                dPanel_visionCam2Ready.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1Ready.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B10", short.Parse("0"));

                dPanel_visionCam1Ready.BackColor = Color.Red;
                dPanel_visionCam2Ready.BackColor = Color.Red;
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
                dPanel_visionCam2End.BackColor = Color.Lime;
            }
            else if (dPanel_visionCam1End.BackColor == Color.Lime)
            {
                Form1._MELSEC.actUtlType64.SetDevice("B12", short.Parse("0"));

                dPanel_visionCam1End.BackColor = Color.Red;
                dPanel_visionCam2End.BackColor = Color.Red;
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
    }





}
