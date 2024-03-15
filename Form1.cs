#nullable disable warnings
// 아래 JD를 조건부 컴파일 기호로 설정하여 실제 현장에서 사용하는 경우와 
// 개발하는 경우를 나우어서 사용할 수 있게끔 하였습니다.
// JD는 프로젝트 빌드속성에서 기본 기호로 추가하여 평소에는 JD는 기호로 인식하고
// 아래 undef를 해놓으면 JD를 기호에서 해제합니다.
// 즉 개발환경에서는 아래 주석을 해제하고, 현장에서는 주석을 활성화한다.
#undef JD  


using JD_Proc.DocImageConvert;
using JD_Proc.ICam;
using JD_Proc.ImageFilter;
using JD_Proc.Lock;
using JD_Proc.Log;
using JD_Proc.Service;
using JD_Proc.TempController;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows.Forms.DataVisualization.Charting;
using static JD_Proc.Log.LogManager;

namespace JD_Proc
{
    public partial class Form1 : Form
    {
        #region var
        public static Model.Models _Model = new Model.Models();

        SettingForm _settingForm;
        AlignSettingForm _AlignSettingform;
        TempGraphForm _TempGraphform;
        AutoSimulation _AutoSimulation;

        SerialCommunication TempController;

        public ThermalPaletteImage _images;

        IrDirectInterface _irDirectInterface_1;
        IrDirectInterface _irDirectInterface_2;

        SerialCommunication _SerialCommunication;

        Thread _imageGrabberThread_1;
        Thread _imageGrabberThread_2;

        bool _grabImages_1 = false;
        bool _grabImages_2 = false;
        bool _bVision_Heartbit = false;

        Thread _snapThread_1;
        Thread _snapThread_2;

        Bitmap originBmap_L;
        Bitmap originBmap_R;

        Bitmap cloneBmap_L;
        Bitmap cloneBmap_R;

        Bitmap grayBmap_L;
        Bitmap grayBmap_R;

        int brighestDrawLine_L = 0;
        int brighestDrawLine_R = 0;

        public static PLC.Melsec _MELSEC;
        public static PLC.Melsec MELSEC_JOG;
        public static PLC.Melsec _MELSEC_HEART;

        double[,] _tempData_L = new double[640, 480];
        double[,] _tempData_R = new double[640, 480];
        List<List<double>> TempVariation_L = new List<List<double>>();
        List<List<double>> TempVariation_R = new List<List<double>>();
        List<List<double>> TempDDList = new List<List<double>>();

        int[,] GaussianArray = new int[640, 480];
        List<List<int>> GaussianBmp = new List<List<int>>();


        List<Model.ProcessData> _Data_1_L = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_2_L = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_3_L = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_4_L = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_5_L = new List<Model.ProcessData>();

        List<Model.ProcessData> _Data_1_R = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_2_R = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_3_R = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_4_R = new List<Model.ProcessData>();
        List<Model.ProcessData> _Data_5_R = new List<Model.ProcessData>();

        RectTracker _LEFT_ROI_1;
        RectTracker _LEFT_ROI_2;
        RectTracker _LEFT_ROI_3;
        RectTracker _LEFT_ROI_4;
        RectTracker _LEFT_ROI_5;

        RectTracker _RIGHT_ROI_1;
        RectTracker _RIGHT_ROI_2;
        RectTracker _RIGHT_ROI_3;
        RectTracker _RIGHT_ROI_4;
        RectTracker _RIGHT_ROI_5;

        System.Windows.Forms.Button _LEFT_ROI_BTN_1;
        System.Windows.Forms.Button _LEFT_ROI_BTN_2;
        System.Windows.Forms.Button _LEFT_ROI_BTN_3;
        System.Windows.Forms.Button _LEFT_ROI_BTN_4;
        System.Windows.Forms.Button _LEFT_ROI_BTN_5;

        System.Windows.Forms.Button _RIGHT_ROI_BTN_1;
        System.Windows.Forms.Button _RIGHT_ROI_BTN_2;
        System.Windows.Forms.Button _RIGHT_ROI_BTN_3;
        System.Windows.Forms.Button _RIGHT_ROI_BTN_4;
        System.Windows.Forms.Button _RIGHT_ROI_BTN_5;

        DataTable dataTable_L = new DataTable();
        DataTable dataTable_R = new DataTable();

        Rockey2 rockey;

        string state = "manual";

        string _Model_path = "";

        int PLC_Heartbit_Count = 0;

        int PLC_Heartbit_Checker = 0;

        int gapDistAvg_L = 0;
        int gapDistAvg_R = 0;

        string _MODE = "";

        System.Timers.Timer _AutoTimer = new System.Timers.Timer();
        System.Threading.Timer _HeartbitTimer;
        System.Threading.Timer _TempControllerTimer;

        object lockObject = new object();

        bool[] isCheckboxROI_L = new bool[5];
        bool[] isCheckboxROI_R = new bool[5];

        bool bSimulationMode = false;
        bool bIsPLC = false;

        Image bmpforscv = null;
        GaussianBlur Gblur = null;

        Service.SettingsService service;
        #endregion

        #region 생성자
        public Form1()
        {
            InitializeComponent();

            service = new Service.SettingsService();

            // settings.ini file을 읽어 simulation section의 debug값이 0이면 bSimulationMode 를 True로 하여
            // 프로그램이 debug모드로 돌아가게끔 하고 
            // false이면 프로그램이 현장에서 돌아가게끔 세팅한다.
            if (service.Read("SIMULATION", "DEBUG") == "1") bSimulationMode = true;
            else bSimulationMode = false;
            if (service.Read("SIMULATION", "PLC") == "1") bIsPLC = true;
            else bIsPLC = false;

            TempController = new SerialCommunication();

            // Debug mode일때는 License검사를 하지 않도록 한다.
            // USB 동글 Lock, 아래 주석을 해제하면 Dongle USB꽂지 않으면 프로그램 실행 X
            //if (!bSimulationMode) rockey = new Rockey2();

            //카메라 연결
            //_MODE = service.Read("MODE", "MODE");



            if (!bSimulationMode)
            {
                if (true)
                {
                    try
                    {
                        if (TempController.Comport_Open(service.Read("TempController", "Comport")) == true)
                        {
                            Lbl_TempController.Text = "Temp_OK";
                            Lbl_TempController.BackColor = Color.Aquamarine;

                        }
                        else
                        {
                            Lbl_TempController.BackColor = Color.Red;
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionLog.FileSave("Exception", exception.TargetSite.ReflectedType.FullName, exception.TargetSite.Name, exception.StackTrace.ToString().Trim(), exception.Message.Trim());
                        MessageBox.Show("Temperature Controller Error, Please Check Temperature Controller has connection Correctly", "Temperature Controller Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        Connect("generic1.xml", 1);
                    }
                    catch (Exception exception)
                    {
                        ExceptionLog.FileSave("Exception", exception.TargetSite.ReflectedType.FullName, exception.TargetSite.Name, exception.StackTrace.ToString().Trim(), exception.Message.Trim());
                        MessageBox.Show("1번 카메라 연결 에러, generic1.xml serial 번호확인 및 카메라 프로그램 구동중인지 확인 부탁드립니다.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //System.Environment.Exit(0);

                        dBtn_auto.Enabled = false;
                        dBtn_auto.BackColor = Color.Gray;
                        dBtn_live1.Enabled = false;
                        dBtn_Measure1.Enabled = false;
                        dBtn_imageSave1.Enabled = false;
                        dBtn_snap1.Enabled = false;
                        dBtn_stop1.Enabled = false;


                    }

                    try
                    {
                        Connect("generic2.xml", 2);
                    }
                    catch (Exception exception)
                    {
                        ExceptionLog.FileSave("Exception", exception.TargetSite.ReflectedType.FullName, exception.TargetSite.Name, exception.StackTrace.ToString().Trim(), exception.Message.Trim());
                        MessageBox.Show("2번 카메라 연결 에러, generic2.xml serial 번호확인 및 카메라 프로그램 구동중인지 확인 부탁드립니다.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dBtn_stop2.Enabled = false;
                        dBtn_snap2.Enabled = false;
                        dBtn_Measure2.Enabled = false;
                        dBtn_live2.Enabled = false;
                        dBtn_imageSave2.Enabled = false;
                    }

                    if (bIsPLC)
                    {
                        try
                        {
                            _MELSEC_HEART = new PLC.Melsec(int.Parse(service.Read("PLC_LOGICAL_STATION_NUMBER", "PLC_LOGICAL_STATION_NUMBER")));
                            _MELSEC = new PLC.Melsec(int.Parse(service.Read("PLC_LOGICAL_STATION_NUMBER", "PLC_LOGICAL_STATION_NUMBER")));
                            _MELSEC.Open();
                            _MELSEC_HEART.Open();
                            if (_MELSEC.IsConnected() == true) dRadio_plc.Checked = true;
                            if (_MELSEC_HEART.IsConnected() == true) Debug.Print("MELSEC_HEART OK");
                            MELSEC_JOG = new PLC.Melsec(int.Parse(service.Read("PLC_LOGICAL_STATION_NUMBER", "PLC_LOGICAL_STATION_NUMBER")));
                            MELSEC_JOG.Open();
                            if (MELSEC_JOG.IsConnected() == false) MessageBox.Show("PLC가 연결되어 있지 않습니다. PLC관련 기능사용에 제한이 생깁니다.", "PLC Alarm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception exception)
                        {
                            ExceptionLog.FileSave("Exception", exception.TargetSite.ReflectedType.FullName, exception.TargetSite.Name, exception.StackTrace.ToString().Trim(), exception.Message.Trim());
                            MessageBox.Show("PLC 연결을 확인해 주세요.", "PLC Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
                dRadio_cam1.Checked = true;
                dRadio_cam2.Checked = true;
            }
            else if (_MODE == "manual")
            {
                dBtn_auto.Enabled = false;
                dBtn_live1.Enabled = false;
                dBtn_live2.Enabled = false;

                dBtn_stop1.Enabled = false;
                dBtn_stop2.Enabled = false;

                dBtn_snap1.Enabled = false;
                dBtn_snap2.Enabled = false;
            }


            MakeROI();
            _Model_path = service.Read("model", "path");
            SetModel(_Model_path);
            InitChartDesign();

            if (bIsPLC)
            {
                _AutoTimer.Interval = 500;
                _AutoTimer.Elapsed += new ElapsedEventHandler(AutoTimer);
                _HeartbitTimer = new System.Threading.Timer(VISION_Heartbit, null, 1000, 400);
            }

            _TempControllerTimer = new System.Threading.Timer(TempController_TimerCheck, null, 1000, 400);


        }

        //현재 참조 없음
        void timer_Tick(object sender, EventArgs e)
        {
            if (_imageGrabberThread_1 != null && _imageGrabberThread_1.IsAlive)
            {
                _grabImages_1 = false;
                _imageGrabberThread_1.Join();
            }

            _snapThread_1 = new Thread(new ThreadStart(SnapMethod_1));
            _snapThread_1.Start();

            if (_imageGrabberThread_2 != null && _imageGrabberThread_2.IsAlive)
            {
                _grabImages_2 = false;
                _imageGrabberThread_2.Join();
            }

            _snapThread_2 = new Thread(new ThreadStart(SnapMethod_2));
            _snapThread_2.Start();
        }
        #endregion

        #region event(auto) - Timer
        void AutoTimer(object sender, ElapsedEventArgs e)
        {

            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (dataTable_L.Rows.Count > 20)
                {
                    dataTable_L.Rows.RemoveAt(0);
                    dataTable_R.Rows.RemoveAt(0);
                }
                double avgValue = new Random().NextDouble() * 100;
                dataTable_L.Rows.Add(avgValue);
                dataTable_R.Rows.Add(avgValue);

                dataGridView1.Refresh();
                dataGridView2.Refresh();
            }));


            bool PLC_AUTO = false;

            bool PLC_START_L = false;
            bool PLC_START_R = false;

            bool VISION_AUTO = false;

            bool VISION_READY_L = false;
            bool VISION_BUSY_L = false;
            bool VISION_END_L = false;

            bool VISION_READY_R = false;
            bool VISION_BUSY_R = false;
            bool VISION_END_R = false;


            PLC_AUTO = PLC_Check_Status("B101"); //TJ 추가, PLC의 B111번 디바이스 값 읽어서 Boolean값으로 반환
            //PLC_AUTO = PLC_Check_Status("M20288"); //TJ 추가, PLC의 B111번 디바이스 값 읽어서 Boolean값으로 반환
            if (PLC_AUTO)
            {

                if (state == "auto") VISION_AUTO = true; //TJ 추가
                else VISION_AUTO = false; //TJ 추가

                if (VISION_AUTO)
                {
                    VISION_READY_L = true;  // TJ 추가
                    // Left camera
                    if (VISION_READY_L)
                    {
                        PLC_START_L = PLC_Check_Status("B110"); //TJ 추가, PLC의 B110번 디바이스 값 읽어서 Boolean값으로 반환
                        //PLC_START_L = PLC_Check_Status("M20289"); //TJ 추가, PLC의 B110번 디바이스 값 읽어서 Boolean값으로 반환
                        if (PLC_START_L)
                        {
                            VISION_READY_L = false;
                            VISION_END_L = false;
                            VISION_BUSY_L = true;

                            //AutoSnap_L();
                            //AutoProcess_L();

                            lock (lockObject)
                            {
                                //[Developing]
                                //ParrotGraphGenerateData(VISION_BUSY_L == true, gapDistAvg_L);
                                //[end developing]
                            }

                            VISION_BUSY_L = false;
                            VISION_END_L = true;
                        }
                    }

                    VISION_READY_R = true;  // TJ 추가

                    // Right camera
                    if (VISION_READY_R)
                    {
                        PLC_START_R = PLC_Check_Status("B111"); //TJ 추가, PLC의 B111번 디바이스 값 읽어서 Boolean값으로 반환
                        if (PLC_START_R)
                        {
                            VISION_READY_R = false;
                            VISION_END_R = false;
                            VISION_BUSY_R = true;

                            AutoSnap_R();
                            AutpProcess_R();

                            lock (lockObject)
                            {
                                //[Developing]
                                //ParrotGraphGenerateData(VISION_BUSY_L == true, gapDistAvg_R);
                                //[end developing]
                            }

                            VISION_BUSY_R = false;
                            VISION_END_R = true;
                        }
                    }
                }
            }

        }

        void AutoSnap_L()
        {
            ThermalPaletteImage images = _irDirectInterface_1.GetThermalPaletteImage();

            int rows = images.ThermalImage.GetLength(0);
            int columns = images.ThermalImage.GetLength(1);

            double mean = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ushort value = images.ThermalImage[row, column];
                    mean += value;
                    _tempData_L[column, row] = ((double)value - 1000.0) / 10.0;
                }
            }

            mean /= rows * columns;
            mean = (mean - 1000.0) / 10.0;

            originBmap_L = images.PaletteImage;

            this.BeginInvoke((MethodInvoker)(() =>
            {
                dLable_tmp1.Text = Math.Round(mean, 1).ToString();

            }));
        }

        void AutoSnap_R()
        {
            ThermalPaletteImage images = _irDirectInterface_2.GetThermalPaletteImage();

            int rows = images.ThermalImage.GetLength(0);
            int columns = images.ThermalImage.GetLength(1);

            double mean = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ushort value = images.ThermalImage[row, column];
                    mean += value;
                    _tempData_R[column, row] = ((double)value - 1000.0) / 10.0;
                }
            }

            mean /= rows * columns;
            mean = (mean - 1000.0) / 10.0;

            originBmap_R = images.PaletteImage;

            this.BeginInvoke((MethodInvoker)(() =>
            {
                dLable_tmp2.Text = Math.Round(mean, 1).ToString();
            }));
        }

        void AutoProcess_L()
        {
            cloneBmap_L = (Bitmap)originBmap_L.Clone();
            grayBmap_L = new Bitmap(640, 480, originBmap_L.PixelFormat);

            // gray image로 변환
            ToGray("cam1");

            //blob 처리
            int threshold = 100;
            Service.BlobService blobService = new Service.BlobService();
            List<Model.Blob> blobs = blobService.FindBlobs(grayBmap_L, threshold);
            List<Model.Blob> okBlobs = GetOkBlob(blobs, "cam1");

            if (okBlobs.Count == 1)
            {
                Service.SettingsService settingsService = new Service.SettingsService();
                string cal_mode = settingsService.Read("cal_mode", "cal_mode");

                Porcess("CAM1", 1, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 2, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 3, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 4, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 5, okBlobs[0], threshold, cal_mode);

                DrawRoiZoom_L(1, true);
                DrawRoiZoom_L(2, true);
                DrawRoiZoom_L(3, true);
                DrawRoiZoom_L(4, true);
                DrawRoiZoom_L(5, true);

                DrawChart_L(true);

                gapDistAvg_L = (int)(WriteGapAvg("cam1", true));



                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (dataTable_L.Rows.Count > 20)
                    {
                        dataTable_L.Rows.RemoveAt(0);
                    }
                    dataTable_L.Rows.Add(gapDistAvg_L);   // Automode에서 data gridview에 gapdata를 뿌려주는 코드
                    dLabel_Ng_L.ForeColor = Color.Lime;
                    dLabel_Ng_L.Text = "OK - " + DateTime.Now.ToString("HH:mm:ss");
                }));
            }
            else
            {
                if (okBlobs.Count == 0)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dLabel_Ng_L.ForeColor = Color.Red;
                        dLabel_Ng_L.Text = "정상적인 이미지가 아닙니다. (영역을 찾지 못함) - " + DateTime.Now.ToString("HH:mm:ss");
                    }));
                }
                else if (okBlobs.Count > 1)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dLabel_Ng_L.ForeColor = Color.Red;
                        dLabel_Ng_L.Text = "정상적인 이미지가 아닙니다. (영역이 두개 이상) - " + DateTime.Now.ToString("HH:mm:ss");
                    }));
                }
            }
        }

        void AutpProcess_R()
        {
            cloneBmap_R = (Bitmap)originBmap_R.Clone();
            grayBmap_R = new Bitmap(640, 480, originBmap_R.PixelFormat);

            //그래이 이미지로 변환
            ToGray("cam2");

            //blob 처리
            int threshold = 100;
            Service.BlobService blobService = new Service.BlobService();
            List<Model.Blob> blobs = blobService.FindBlobs(grayBmap_R, threshold);
            List<Model.Blob> okBlobs = GetOkBlob(blobs, "cam2");

            if (okBlobs.Count == 1)
            {
                Service.SettingsService settingsService = new Service.SettingsService();
                string cal_mode = settingsService.Read("cal_mode", "cal_mode");

                Porcess("CAM2", 1, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 2, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 3, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 4, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 5, okBlobs[0], threshold, cal_mode);

                DrawRoiZoom_R(1, true);
                DrawRoiZoom_R(2, true);
                DrawRoiZoom_R(3, true);
                DrawRoiZoom_R(4, true);
                DrawRoiZoom_R(5, true);

                DrawChart_R(true);

                gapDistAvg_R = (int)(WriteGapAvg("cam2", true));



                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (dataTable_R.Rows.Count > 20)
                    {
                        dataTable_R.Rows.RemoveAt(0);
                    }
                    dataTable_R.Rows.Add(gapDistAvg_R);   // Automode에서 data gridview에 gapdata를 뿌려주는 코드
                    dLabel_Ng_R.ForeColor = Color.Lime;
                    dLabel_Ng_R.Text = "OK - " + DateTime.Now.ToString("HH:mm:ss");
                }));
            }
            else
            {
                if (okBlobs.Count == 0)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dLabel_Ng_R.ForeColor = Color.Red;
                        dLabel_Ng_R.Text = "정상적인 이미지가 아닙니다. (영역을 찾지 못함) - " + DateTime.Now.ToString("HH:mm:ss");
                    }));
                }
                else if (okBlobs.Count > 1)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dLabel_Ng_R.ForeColor = Color.Red;
                        dLabel_Ng_R.Text = "정상적인 이미지가 아닙니다. (영역이 두개 이상) - " + DateTime.Now.ToString("HH:mm:ss");
                    }));
                }
            }

        }
        #endregion


        #region event(live / live stop) - click
        private void dBtn_live1_Click(object sender, EventArgs e)
        {
            dBtn_load1.Enabled = false;
            dBtn_imageSave1.Enabled = false;
            dBtn_live1.Enabled = false;
            dBtn_stop1.Enabled = true;
            dBtn_snap1.Enabled = false;
            dBtn_Process1.Enabled = false;

            _imageGrabberThread_1 = new Thread(new ThreadStart(ImageGrabberMethode1));
            _grabImages_1 = true;
            _imageGrabberThread_1.Start();
        }

        private void dBtn_live2_Click(object sender, EventArgs e)
        {
            dBtn_load2.Enabled = false;
            dBtn_imageSave2.Enabled = false;
            dBtn_live2.Enabled = false;
            dBtn_stop2.Enabled = true;
            dBtn_snap2.Enabled = false;
            dBtn_Process2.Enabled = false;

            _imageGrabberThread_2 = new Thread(new ThreadStart(ImageGrabberMethode2));
            _grabImages_2 = true;
            _imageGrabberThread_2.Start();
        }

        private void dBtn_stop1_Click(object sender, EventArgs e)
        {
            dBtn_load1.Enabled = true;
            dBtn_imageSave1.Enabled = true;
            dBtn_live1.Enabled = true;
            dBtn_stop1.Enabled = false;
            dBtn_snap1.Enabled = true;
            dBtn_Process1.Enabled = true;

            if (_imageGrabberThread_1 != null)
            {
                _grabImages_1 = false;
                _imageGrabberThread_1.Join();
            }
        }

        private void dBtn_stop2_Click(object sender, EventArgs e)
        {
            dBtn_load2.Enabled = true;
            dBtn_imageSave2.Enabled = true;
            dBtn_live2.Enabled = true;
            dBtn_stop2.Enabled = false;
            dBtn_snap2.Enabled = true;
            dBtn_Process2.Enabled = true;

            if (_imageGrabberThread_2 != null)
            {
                _grabImages_2 = false;
                _imageGrabberThread_2.Join();
            }
        }
        #endregion 

        #region event(dBtn_modelOpen) - click
        private void dBtn_modelOpen_Click(object sender, EventArgs e)
        {
            ModelForm modelForm = new ModelForm(this);
            modelForm.StartPosition = FormStartPosition.Manual;
            modelForm.Location = new System.Drawing.Point(500, 500);

            modelForm.ShowDialog();
        }
        #endregion

        #region event(dBtn_auto) - click
        private void dBtn_auto_Click(object sender, EventArgs e)
        {
            if (state == "manual")
            {
                if (_imageGrabberThread_1 != null)
                {
                    _grabImages_1 = false;
                    _imageGrabberThread_1.Join();
                }

                if (_imageGrabberThread_2 != null)
                {
                    _grabImages_2 = false;
                    _imageGrabberThread_2.Join();
                }

                dLabel_autoState.Text = "AUTO";
                dLabel_autoState.ForeColor = Color.Lime;
                state = "auto";

                dBtn_modelOpen.Enabled = false;
                dBtn_save_L.Enabled = false;
                dBtn_load1.Enabled = false;
                dBtn_load2.Enabled = false;
                dBtn_imageSave1.Enabled = false;
                dBtn_imageSave2.Enabled = false;
                dBtn_live1.Enabled = false;
                dBtn_live2.Enabled = false;
                dBtn_stop1.Enabled = false;
                dBtn_stop2.Enabled = false;
                dBtn_snap1.Enabled = false;
                dBtn_snap2.Enabled = false;
                dBtn_Process1.Enabled = false;
                dBtn_Process2.Enabled = false;
                dBtn_settings.Enabled = false;
                tableLayoutPanel_Auto.Visible = true;

                dataTable_L.Columns.Add("Average", typeof(double));
                dataGridView1.DataSource = dataTable_L;

                dataTable_R.Columns.Add("Average", typeof(double));
                dataGridView2.DataSource = dataTable_R;

                if (bSimulationMode)
                {
                    _AutoSimulation = new AutoSimulation();
                    _AutoSimulation.Show();
                }
                if (!bSimulationMode) _AutoTimer.Start();

            }
            else if (state == "auto")
            {
                dLabel_autoState.Text = "MANUAL";
                dLabel_autoState.ForeColor = Color.FromArgb(114, 118, 127);
                state = "manual";

                dBtn_modelOpen.Enabled = true;
                dBtn_save_L.Enabled = true;
                dBtn_load1.Enabled = true;
                dBtn_load2.Enabled = true;
                dBtn_imageSave1.Enabled = true;
                dBtn_imageSave2.Enabled = true;
                dBtn_live1.Enabled = true;
                dBtn_live2.Enabled = true;
                dBtn_stop1.Enabled = true;
                dBtn_stop2.Enabled = true;
                dBtn_snap1.Enabled = true;
                dBtn_snap2.Enabled = true;
                dBtn_Process1.Enabled = true;
                dBtn_Process2.Enabled = true;
                dBtn_settings.Enabled = true;
                tableLayoutPanel_Auto.Visible = false;

                dataTable_L.Columns.Clear();
                dataTable_R.Columns.Clear();
                dataTable_L.Rows.Clear();
                dataTable_R.Rows.Clear();
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;

                if (!bSimulationMode) _AutoTimer.Stop();
            }
        }
        #endregion

        #region event(snap) - click
        private void dBtn_snap1_Click(object sender, EventArgs e)
        {
            if (_imageGrabberThread_1 != null && _imageGrabberThread_1.IsAlive)
            {
                _grabImages_1 = false;
                _imageGrabberThread_1.Join();
            }

            _snapThread_1 = new Thread(new ThreadStart(SnapMethod_1));
            _snapThread_1.Start();
        }

        private void dBtn_snap2_Click(object sender, EventArgs e)
        {
            if (_imageGrabberThread_2 != null && _imageGrabberThread_2.IsAlive)
            {
                _grabImages_2 = false;
                _imageGrabberThread_2.Join();
            }

            _snapThread_2 = new Thread(new ThreadStart(SnapMethod_2));
            _snapThread_2.Start();
        }
        #endregion

        #region event(process) - click
        private void dBtn_Process1_Click(object sender, EventArgs e)
        {
            originBmap_L = (Bitmap)pictureBox1.Image;
            cloneBmap_L = (Bitmap)originBmap_L.Clone();
            grayBmap_L = new Bitmap(640, 480, originBmap_L.PixelFormat);

            // gray image로 변환
            ToGray("cam1");
            grayBmap_L.Save(@"C:\JD\Images\MidOutput\GrayScaled.bmp", ImageFormat.Bmp);

            //blob 처리
            int threshold = 100;
            Service.BlobService blobService = new Service.BlobService();
            List<Model.Blob> blobs = blobService.FindBlobs(grayBmap_L, threshold);
            List<Model.Blob> okBlobs = GetOkBlob(blobs, "cam1");

            Debug.Print($@"X: {okBlobs[0].X.ToString()}, Y: {okBlobs[0].Y.ToString()}, Width: {okBlobs[0].Width.ToString()}, Height: {okBlobs[0].Height.ToString()}");


            if (okBlobs.Count == 1)
            {
                Service.SettingsService settingsService = new Service.SettingsService();
                string cal_mode = settingsService.Read("cal_mode", "cal_mode");

                Porcess("CAM1", 1, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 2, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 3, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 4, okBlobs[0], threshold, cal_mode);
                Porcess("CAM1", 5, okBlobs[0], threshold, cal_mode);

                DrawRoiZoom_L(1, false);
                DrawRoiZoom_L(2, false);
                DrawRoiZoom_L(3, false);
                DrawRoiZoom_L(4, false);
                DrawRoiZoom_L(5, false);

                DrawChart_L(false);

                WriteGapAvg("cam1", false);

                TempDDList.Clear();

                dLabel_Ng_L.ForeColor = Color.Lime;
                dLabel_Ng_L.Text = "OK - " + DateTime.Now.ToString("HH:mm:ss");
            }
            else
            {
                if (okBlobs.Count == 0)
                {
                    dLabel_Ng_L.ForeColor = Color.Red;
                    dLabel_Ng_L.Text = "정상적인 이미지가 아닙니다. (영역을 찾지 못함) - " + DateTime.Now.ToString("HH:mm:ss");
                }
                else if (okBlobs.Count > 1)
                {
                    dLabel_Ng_L.ForeColor = Color.Red;
                    dLabel_Ng_L.Text = "정상적인 이미지가 아닙니다. (영역이 두개 이상) - " + DateTime.Now.ToString("HH:mm:ss");
                }
            }
        }

        private void dBtn_Process2_Click(object sender, EventArgs e)
        {
            originBmap_R = (Bitmap)pictureBox2.Image;
            cloneBmap_R = (Bitmap)originBmap_R.Clone();
            grayBmap_R = new Bitmap(640, 480, originBmap_R.PixelFormat);

            //그래이 이미지로 변환
            ToGray("cam2");

            //blob 처리
            int threshold = 100;
            Service.BlobService blobService = new Service.BlobService();
            List<Model.Blob> blobs = blobService.FindBlobs(grayBmap_R, threshold);
            List<Model.Blob> okBlobs = GetOkBlob(blobs, "cam2");

            if (okBlobs.Count == 1)
            {
                Service.SettingsService settingsService = new Service.SettingsService();
                string cal_mode = settingsService.Read("cal_mode", "cal_mode");

                Porcess("CAM2", 1, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 2, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 3, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 4, okBlobs[0], threshold, cal_mode);
                Porcess("CAM2", 5, okBlobs[0], threshold, cal_mode);

                DrawRoiZoom_R(1, false);
                DrawRoiZoom_R(2, false);
                DrawRoiZoom_R(3, false);
                DrawRoiZoom_R(4, false);
                DrawRoiZoom_R(5, false);

                DrawChart_R(false);

                WriteGapAvg("cam2", false);

                TempDDList.Clear();

                dLabel_Ng_R.ForeColor = Color.Lime;
                dLabel_Ng_R.Text = "OK - " + DateTime.Now.ToString("HH:mm:ss");
            }
            else
            {
                if (okBlobs.Count == 0)
                {
                    dLabel_Ng_R.ForeColor = Color.Red;
                    dLabel_Ng_R.Text = "정상적인 이미지가 아닙니다. (영역을 찾지 못함) - ";
                }
                else if (okBlobs.Count > 1)
                {
                    dLabel_Ng_R.ForeColor = Color.Red;
                    dLabel_Ng_R.Text = "정상적인 이미지가 아닙니다. (영역이 두개 이상) - ";
                }
            }
        }
        #endregion

        #region event(Measure) - click
        private void dBtn_Measure1_Click(object sender, EventArgs e)
        {
            measureFunc1(sender, e);
        }

        private void dBtn_Measure2_Click(object sender, EventArgs e)
        {
            measureFunc2(sender, e);
        }
        #endregion

        #region event(lmage load) - click
        private void dBtn_load1_Click(object sender, EventArgs e)
        {


            string image_file = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG,*.PNG";
            dialog.InitialDirectory = @"C:\JD\Images";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image_file = dialog.FileName;

                //csv파일 읽기
                string csvFile = image_file.Replace(".bmp", ".csv");

                //var GaussedDDList = DataConvertFormat.ConvertCSVToDLList(csvFile.Replace(".csv", "GProcessed.csv"), 640, 480);
                //var A_GaussedDDList = new List<List<Double>>();
                TempDDList = new List<List<Double>>();

                if (File.Exists(csvFile))
                {
                    pictureBox1.Image = Bitmap.FromFile(image_file);
                    pictureBox1_Auto.Image = pictureBox1.Image;

                    SnaptoGaussianImagewithCSV(pictureBox1.Image);
                    CalculateGaussianfileterToOriginalTempCSV(csvFile);

                    try
                    {
                        StreamReader sr = new StreamReader(csvFile);
                        //StreamReader srGaussian = new StreamReader(@"C:\JD\Images\GaussianBlur\GaussianBlur.csv");

                        int row = 0;


                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] data = line.Split(',');



                            for (int x = 0; x < 640; x++)
                            {
                                _tempData_L[x, row] = double.Parse(data[x]);//(double.Parse(data[x]) * 10.0d) + 1000;
                                //GaussianArray[x, row] = int.Parse(GaussianData[x]);//(double.Parse(data[x]) * 10.0d) + 1000;
                            }


                            row = row + 1;
                        }

                        for (int column = 0; column < 640; column++)
                        {

                            TempVariation_L.Add(new List<double>());
                            GaussianBmp.Add(new List<int>());
                            TempDDList.Add(new List<double>());

                            for (int r = 0; r < 479; r++)
                            {
                                TempVariation_L[column].Add((_tempData_L[column, r] > 40) || (_tempData_L[column, r + 1] > 40) ? 0 : Math.Round(_tempData_L[column, r] - _tempData_L[column, r + 1], 2));
                                GaussianBmp[column].Add(GaussianArray[column, r]);

                                TempDDList[column].Add(_tempData_L[column, r]);
                            }
                        }

                        brighestDrawLine_L = 0;
                        Panel_BrighestLine_L.Visible = false;
                    }

                    catch (Exception ex)
                    {
                        Debug.Print(ex.Message);
                        MessageBox.Show("이미지와 연결된 csv파일이 사용중입니다.", "CSV Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("csv 파일이 존재 하지 않습니다");
                }


            }
        }

        private void dBtn_load2_Click(object sender, EventArgs e)
        {
            string image_file = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG,*.PNG";
            dialog.InitialDirectory = @"C:\JD\Images";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image_file = dialog.FileName;

                //csv파일 읽기
                string csvFile = image_file.Replace(".bmp", ".csv");

                if (File.Exists(csvFile))
                {
                    pictureBox2.Image = Bitmap.FromFile(image_file);
                    pictureBox2_Auto.Image = pictureBox2.Image;


                    try
                    {
                        StreamReader sr = new StreamReader(csvFile);

                        int row = 0;

                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            string[] data = line.Split(',');

                            for (int x = 0; x < 640; x++)
                            {
                                _tempData_R[x, row] = double.Parse(data[x]);  //(double.Parse(data[x]) * 10.0d) + 1000;
                            }

                            row = row + 1;
                        }
                        brighestDrawLine_R = 0;

                        Panel_BrighestLine_R.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("이미지와 연결된 csv파일이 사용중입니다.", "CSV Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("csv 파일이 존재 하지 않습니다");
                }

            }
        }
        #endregion

        #region event(image save) - click
        private void dBtn_imageSave1_Click(object sender, EventArgs e)
        {
            string saveFolder = @"C:\JD\images\" + DateTime.Now.ToString("yyyy-MM-dd");

            if (!System.IO.Directory.Exists(saveFolder))
                System.IO.Directory.CreateDirectory(saveFolder);

            pictureBox1.Image.Save(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            pictureBox1_Auto.Image.Save(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            //csv 파일 저장
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
            {
                for (int y = 0; y < _tempData_L.GetLength(1); y++)
                {
                    string sendText = "";

                    for (int x = 0; x < _tempData_L.GetLength(0); x++)
                    {
                        //프로세싱 온도 데이터에서 실제 온도 데이터로 변환
                        double saveData = _tempData_L[x, y];
                        saveData = Math.Round(saveData, 2);
                        sendText = sendText + "," + saveData.ToString();
                    }

                    sendText = sendText.Remove(0, 1);
                    file.WriteLine(sendText);
                }
            }
        }

        private void dBtn_imageSave2_Click(object sender, EventArgs e)
        {
            string saveFolder = @"C:\JD\images\" + DateTime.Now.ToString("yyyy-MM-dd");

            if (!System.IO.Directory.Exists(saveFolder))
                System.IO.Directory.CreateDirectory(saveFolder);

            pictureBox2.Image.Save(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            pictureBox2_Auto.Image.Save(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            //csv 파일 저장
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
            {
                for (int y = 0; y < _tempData_R.GetLength(1); y++)
                {
                    string sendText = "";

                    for (int x = 0; x < _tempData_R.GetLength(0); x++)
                    {
                        //프로세싱 온도 데이터에서 실제 온도 데이터로 변환
                        double saveData = _tempData_R[x, y];
                        saveData = Math.Round(saveData, 2);
                        sendText = sendText + "," + saveData.ToString();
                    }

                    sendText = sendText.Remove(0, 1);
                    file.WriteLine(sendText);
                }
            }
        }
        #endregion

        #region event(settings) - click
        private void dBtn_settings_Click(object sender, EventArgs e)
        {

            _AlignSettingform = new AlignSettingForm(this, pictureBox1, pictureBox2);
            _TempGraphform = new TempGraphForm();

            _settingForm = new SettingForm(this, _AlignSettingform, _TempGraphform);





            //settingForm.SetPlc();
            //settingForm._timer.Start();

            //ASform.ShowDialog();
            //Temperform.ShowDialog();
            _settingForm.Show();

            //settingForm._timer.Stop();
        }
        #endregion

        #region event(save) - click 
        private void dBtn_save_L_Click(object sender, EventArgs e)
        {
            //int y_L = dPan_grid_L_1.Top - pictureBox1.Top;
            //int y_R = dPan_grid_L_2.Top - pictureBox1.Top;

            Service.ModelsService modelService = new Service.ModelsService();

            //modelService.WriteOne("roi_L", "gridview_y", y_L.ToString(), _Model_path);
            //modelService.WriteOne("roi_R", "gridview_y", y_R.ToString(), _Model_path);

            int num_L = 0;
            int num_R = 0;

            //bool isNumeric_L = int.TryParse(dTbox_grid_L.Text, out num_L);
            //bool isNumeric_R = int.TryParse(dTbox_grid_R.Text, out num_R);

            //if (isNumeric_L) modelService.WriteOne("roi_L", "gridview", num_L.ToString(), _Model_path);
            //if (isNumeric_R) modelService.WriteOne("roi_R", "gridview", num_R.ToString(), _Model_path);

            SetModel(_Model_path);
        }
        #endregion

        #region event(jog) - click
        private void DBtn_jogUp_L_Click(object sender, EventArgs e)
        {
            // MELSEC_JOG 객체를 이용한다. 
            MELSEC_JOG.actUtlType64.SetDevice("B16", short.Parse("1"));
        }

        private void DBtn_jogDown_L_Click(object sender, EventArgs e)
        {
            MELSEC_JOG.actUtlType64.SetDevice("B17", short.Parse("1"));
        }

        private void DBtn_jogUp_R_Click(object sender, EventArgs e)
        {
            MELSEC_JOG.actUtlType64.SetDevice("B18", short.Parse("1"));
        }

        private void DBtn_jogDown_R_Click(object sender, EventArgs e)
        {
            MELSEC_JOG.actUtlType64.SetDevice("B19", short.Parse("1"));
        }
        #endregion

        #region event(dComboBox) - 콤보박스 인덱스 바꿀때

        private void dComboBox_scale1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _irDirectInterface_1.SetPaletteFormat(
                (OptrisColoringPalette)Enum.Parse(typeof(OptrisColoringPalette), (string)dComboBox_cam1.SelectedItem),
                (OptrisPaletteScalingMethod)Enum.Parse(typeof(OptrisPaletteScalingMethod), (string)dComboBox_scale1.SelectedItem));

            Debug.WriteLine(dComboBox_cam1.SelectedIndex);
        }

        private void dComboBox_cam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _irDirectInterface_1.SetPaletteFormat(
                (OptrisColoringPalette)Enum.Parse(typeof(OptrisColoringPalette), (string)dComboBox_cam1.SelectedItem),
                (OptrisPaletteScalingMethod)Enum.Parse(typeof(OptrisPaletteScalingMethod), (string)dComboBox_scale1.SelectedItem));
        }

        private void dComboBox_scale2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _irDirectInterface_2.SetPaletteFormat(
                (OptrisColoringPalette)Enum.Parse(typeof(OptrisColoringPalette), (string)dComboBox_cam2.SelectedItem),
                (OptrisPaletteScalingMethod)Enum.Parse(typeof(OptrisPaletteScalingMethod), (string)dComboBox_scale2.SelectedItem));
        }

        private void dComboBox_cam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _irDirectInterface_2.SetPaletteFormat(
                (OptrisColoringPalette)Enum.Parse(typeof(OptrisColoringPalette), (string)dComboBox_cam2.SelectedItem),
                (OptrisPaletteScalingMethod)Enum.Parse(typeof(OptrisPaletteScalingMethod), (string)dComboBox_scale2.SelectedItem));
        }
        #endregion

        #region event(form1) - closing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stop image grabber thread
            _grabImages_1 = false;
            _grabImages_2 = false;

            //wait for finish thread

            if (_imageGrabberThread_1 != null)
                _imageGrabberThread_1.Join(3000);

            //if (_imageGrabberThread_2 != null)
            //    _imageGrabberThread_2.Join(3000);

            if (_snapThread_1 != null)
                _snapThread_1.Join(3000);

            //if (_snapThread_2 != null)
            //    _snapThread_2.Join(3000);

            //clean up
            //_irDirectInterface_1.Disconnect();
            //_irDirectInterface_2.Disconnect();

            //rockey.Rockey_Closing();

        }
        #endregion

        #region [event - SupprotLineDraw]
        private void dBtn_CameraLine_L_Click(object sender, EventArgs e)
        {
            cameraCenterLineDraw_L();
        }

        private void dBtn_BrightestLine_L_Click(object sender, EventArgs e)
        {
            if (brighestDrawLine_L != 0) brightestLineDraw_L();
            else MessageBox.Show("Process first", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dBtn_CameraLine_R_Click(object sender, EventArgs e)
        {
            cameraCenterLineDraw_R();
        }

        private void dBtn_BrightestLine_R_Click(object sender, EventArgs e)
        {
            if (brighestDrawLine_R != 0) brightestLineDraw_R();
            else MessageBox.Show("Process first", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region [event - BottomLineSetting]
        private void Btn_BottomLineSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (TempDDList.Count != 0) BottomLineSetting();
                else MessageBox.Show("Error, Empty Data set, Please Load & Process First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region method - 카메라 connect
        public void Connect(string configPath, int index)
        {
            if (index == 1)
            {
                _irDirectInterface_1 = new IrDirectInterface();
                _irDirectInterface_1.Connect(configPath);


                _irDirectInterface_1.SetPaletteManualTemperatureRange(20, 30);
            }
            if (index == 2)
            {
                _irDirectInterface_2 = new IrDirectInterface();
                _irDirectInterface_2.Connect(configPath);

                _irDirectInterface_2.SetPaletteManualTemperatureRange(20, 30);
            }
        }
        #endregion

        #region method - model Form에서 모델이 바뀔때
        public void ModelChanged(string path)
        {
            SetModel(path);
            _Model_path = path;
        }
        #endregion

        #region method - 영상 live 
        private void ImageGrabberMethode1()
        {
            while (_grabImages_1)
            {
                //get the newest image, blocks till new image
                _images = _irDirectInterface_1.GetThermalPaletteImage();

                //calculate mean temperature
                int rows = _images.ThermalImage.GetLength(0);
                int columns = _images.ThermalImage.GetLength(1);

                double mean = 0;
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        ushort value = _images.ThermalImage[row, column];
                        mean += value;
                        _tempData_L[column, row] = ((double)value - 1000.0) / 10.0;
                    }
                }
                for (int column = 0; column < columns; column++)
                {
                    for (int row = 0; row < rows - 1; row++)
                    {

                    }
                }

                //Calculates mean value: meanSum / pixelCount
                mean /= rows * columns;

                //convert to real temperature value
                mean = (mean - 1000.0) / 10.0;

                //Invoke UI-Thread for update of ui
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    pictureBox1.Image = _images.PaletteImage;
                    //pictureBox1.Image = grayBmap_L;
                    pictureBox1_Auto.Image = _images.PaletteImage;
                    //_AlignSettingform.BeginInvoke((MethodInvoker)(() =>
                    //{
                    //    pictureBox1.Image = images.PaletteImage;
                    //}));
                    dLable_tmp1.Text = Math.Round(mean, 2).ToString();
                }));
            }
        }

        private void ImageGrabberMethode2()
        {
            while (_grabImages_2)
            {
                //get the newest image, blocks till new image
                ThermalPaletteImage images = _irDirectInterface_2.GetThermalPaletteImage();

                //calculate mean temperature
                int rows = images.ThermalImage.GetLength(0);
                int columns = images.ThermalImage.GetLength(1);

                double mean = 0;
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        ushort value = images.ThermalImage[row, column];
                        mean += value;
                        _tempData_R[column, row] = ((double)value - 1000.0) / 10.0;
                    }
                }
                for (int column = 0; column < columns; column++)
                {
                    for (int row = 0; row < rows - 1; row++)
                    {
                        TempVariation_R[column][row] = _tempData_R[column, row] - _tempData_R[column, row + 1];
                    }
                }

                //Calculates mean value: meanSum / pixelCount
                mean /= rows * columns;

                //convert to real temperature value
                mean = (mean - 1000.0) / 10.0;

                //Invoke UI-Thread for update of ui
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    pictureBox2.Image = images.PaletteImage;
                    pictureBox2_Auto.Image = images.PaletteImage;
                    dLable_tmp2.Text = Math.Round(mean, 2).ToString();
                }));
            }
        }
        #endregion

        #region method - snap
        private void SnapMethod_1()
        {
            //get the newest image, blocks till new image
            ThermalPaletteImage images = _irDirectInterface_1.GetThermalPaletteImage();

            //calculate mean temperature
            int rows = images.ThermalImage.GetLength(0);
            int columns = images.ThermalImage.GetLength(1);

            double mean = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ushort value = images.ThermalImage[row, column];
                    mean += value;
                    _tempData_L[column, row] = ((double)value - 1000.0) / 10.0;
                }
            }

            //Calculates mean value: meanSum / pixelCount
            mean /= rows * columns;

            //convert to real temperature value
            mean = (mean - 1000.0) / 10.0;

            //Invoke UI-Thread for update of ui
            this.BeginInvoke((MethodInvoker)(() =>
            {
                pictureBox1.Image = images.PaletteImage;
                pictureBox1_Auto.Image = images.PaletteImage;
                dLable_tmp1.Text = Math.Round(mean, 1).ToString();

            }));

            SnaptoGaussianImagewithCSV(pictureBox1.Image);
        }
        private void SnapMethod_2()
        {
            //get the newest image, blocks till new image
            ThermalPaletteImage images = _irDirectInterface_2.GetThermalPaletteImage();

            //calculate mean temperature
            int rows = images.ThermalImage.GetLength(0);
            int columns = images.ThermalImage.GetLength(1);

            double mean = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ushort value = images.ThermalImage[row, column];
                    mean += value;
                    _tempData_R[column, row] = ((double)value - 1000.0) / 10.0;
                }
            }

            //Calculates mean value: meanSum / pixelCount
            mean /= rows * columns;

            //convert to real temperature value
            mean = (mean - 1000.0) / 10.0;

            //Invoke UI-Thread for update of ui
            this.BeginInvoke((MethodInvoker)(() =>
            {
                pictureBox2.Image = images.PaletteImage;
                pictureBox2_Auto.Image = images.PaletteImage;
                dLable_tmp2.Text = Math.Round(mean, 1).ToString();
            }));
        }

        #endregion

        #region method - make Roi
        void MakeROI()
        {
            (_LEFT_ROI_BTN_5, _LEFT_ROI_5) = MakeRect("LEFT_ROI_5", 0, 0, 5, 5, Brushes.Yellow, "L_CAM_AREA5", Color.White, "LEFT");
            (_LEFT_ROI_BTN_4, _LEFT_ROI_4) = MakeRect("LEFT_ROI_4", 0, 0, 5, 5, Brushes.Yellow, "L_CAM_AREA4", Color.White, "LEFT");
            (_LEFT_ROI_BTN_3, _LEFT_ROI_3) = MakeRect("LEFT_ROI_3", 0, 0, 5, 5, Brushes.Yellow, "L_CAM_AREA3", Color.White, "LEFT");
            (_LEFT_ROI_BTN_2, _LEFT_ROI_2) = MakeRect("LEFT_ROI_2", 0, 0, 5, 5, Brushes.Yellow, "L_CAM_AREA2", Color.White, "LEFT");
            (_LEFT_ROI_BTN_1, _LEFT_ROI_1) = MakeRect("LEFT_ROI_1", 0, 0, 5, 5, Brushes.Yellow, "L_CAM_AREA1", Color.White, "LEFT");

            (_RIGHT_ROI_BTN_5, _RIGHT_ROI_5) = MakeRect("RIGHT_ROI_5", 0, 0, 5, 5, Brushes.Yellow, "R_CAM_AREA5", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_4, _RIGHT_ROI_4) = MakeRect("RIGHT_ROI_4", 0, 0, 5, 5, Brushes.Yellow, "R_CAM_AREA4", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_3, _RIGHT_ROI_3) = MakeRect("RIGHT_ROI_3", 0, 0, 5, 5, Brushes.Yellow, "R_CAM_AREA3", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_2, _RIGHT_ROI_2) = MakeRect("RIGHT_ROI_2", 0, 0, 5, 5, Brushes.Yellow, "R_CAM_AREA2", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_1, _RIGHT_ROI_1) = MakeRect("RIGHT_ROI_1", 0, 0, 5, 5, Brushes.Yellow, "R_CAM_AREA1", Color.White, "RIGHT");
        }

        private (System.Windows.Forms.Button, RectTracker) MakeRect(string roiName, int x, int y, int width, int height, Brush roiColor, string labalName, Color labelColor, string cam)
        {
            string roiButton = "btn" + roiName;

            System.Windows.Forms.Button _ROI = new System.Windows.Forms.Button();
            RectTracker _ROIRect = new RectTracker();

            _ROI.Location = new System.Drawing.Point(x + 3, y + 3);
            _ROI.Size = new System.Drawing.Size(width - 7, height - 7);

            if (cam == "LEFT")
                _ROI.Parent = pictureBox1;
            else if (cam == "RIGHT")
                _ROI.Parent = pictureBox2;

            _ROI.Name = roiButton;

            _ROI.BringToFront();
            _ROI.Capture = false;

            _ROIRect = new RectTracker(_ROI);

            _ROIRect.Name = roiName;
            _ROIRect.Parent = _ROI;
            this.Controls.Add(_ROIRect);
            _ROIRect.BringToFront();

            if (cam == "LEFT")
                _ROIRect.Draw(pictureBox1, roiColor);
            else if (cam == "RIGHT")
                _ROIRect.Draw(pictureBox2, roiColor);

            if (y < 20)
            {
                if (cam == "LEFT")
                    _ROIRect.roiLabel.Parent = pictureBox1;
                else if (cam == "RIGHT")
                    _ROIRect.roiLabel.Parent = pictureBox2;

                _ROIRect.roiLabel.BackColor = Color.Transparent;
                _ROIRect.roiLabel.Name = "lbl" + roiName;
                _ROIRect.roiLabel.Text = labalName;
                _ROIRect.roiLabel.Location = new System.Drawing.Point(x - 5, y + height + 10);
                _ROIRect.roiLabel.Size = new System.Drawing.Size(100, 20);
                _ROIRect.roiLabel.ForeColor = labelColor;
                _ROIRect.roiLabel.BringToFront();
                _ROIRect.roiLabel.Capture = false;
            }
            else
            {
                if (cam == "LEFT")
                    _ROIRect.roiLabel.Parent = pictureBox1;
                else if (cam == "RIGHT")
                    _ROIRect.roiLabel.Parent = pictureBox2;

                _ROIRect.roiLabel.BackColor = Color.Transparent;
                _ROIRect.roiLabel.Name = "lbl" + roiName;
                _ROIRect.roiLabel.Text = labalName;
                _ROIRect.roiLabel.Location = new System.Drawing.Point(x - 5, y - 20);
                _ROIRect.roiLabel.Size = new System.Drawing.Size(80, 20);
                _ROIRect.roiLabel.ForeColor = labelColor;
                _ROIRect.roiLabel.BringToFront();
                _ROIRect.roiLabel.Capture = false;
            }

            _ROI.Hide();

            return (_ROI, _ROIRect);
        }
        #endregion

        #region method - set Model
        void SetModel(string path)
        {
            ModelsService service = new ModelsService();
            _Model = service.SetModels(path);

            //LEFT
            _LEFT_ROI_5.Left = _Model.X_5_L;
            _LEFT_ROI_4.Left = _Model.X_4_L;
            _LEFT_ROI_3.Left = _Model.X_3_L;
            _LEFT_ROI_2.Left = _Model.X_2_L;
            _LEFT_ROI_1.Left = _Model.X_1_L;

            _LEFT_ROI_5.Top = _Model.Y_5_L;
            _LEFT_ROI_4.Top = _Model.Y_4_L;
            _LEFT_ROI_3.Top = _Model.Y_3_L;
            _LEFT_ROI_2.Top = _Model.Y_2_L;
            _LEFT_ROI_1.Top = _Model.Y_1_L;

            _LEFT_ROI_5.Width = _Model.Width_L;
            _LEFT_ROI_4.Width = _Model.Width_L;
            _LEFT_ROI_3.Width = _Model.Width_L;
            _LEFT_ROI_2.Width = _Model.Width_L;
            _LEFT_ROI_1.Width = _Model.Width_L;

            _LEFT_ROI_5.Height = _Model.Height_L;
            _LEFT_ROI_4.Height = _Model.Height_L;
            _LEFT_ROI_3.Height = _Model.Height_L;
            _LEFT_ROI_2.Height = _Model.Height_L;
            _LEFT_ROI_1.Height = _Model.Height_L;

            _LEFT_ROI_BTN_5.Width = _Model.Width_L;
            _LEFT_ROI_BTN_4.Width = _Model.Width_L;
            _LEFT_ROI_BTN_3.Width = _Model.Width_L;
            _LEFT_ROI_BTN_2.Width = _Model.Width_L;
            _LEFT_ROI_BTN_1.Width = _Model.Width_L;

            _LEFT_ROI_BTN_5.Height = _Model.Height_L;
            _LEFT_ROI_BTN_4.Height = _Model.Height_L;
            _LEFT_ROI_BTN_3.Height = _Model.Height_L;
            _LEFT_ROI_BTN_2.Height = _Model.Height_L;
            _LEFT_ROI_BTN_1.Height = _Model.Height_L;

            //dTbox_grid_L.Text = _Model.Grid_L.ToString();

            //RIGHT
            _RIGHT_ROI_5.Left = _Model.X_5_R;
            _RIGHT_ROI_4.Left = _Model.X_4_R;
            _RIGHT_ROI_3.Left = _Model.X_3_R;
            _RIGHT_ROI_2.Left = _Model.X_2_R;
            _RIGHT_ROI_1.Left = _Model.X_1_R;

            _RIGHT_ROI_5.Top = _Model.Y_5_R;
            _RIGHT_ROI_4.Top = _Model.Y_4_R;
            _RIGHT_ROI_3.Top = _Model.Y_3_R;
            _RIGHT_ROI_2.Top = _Model.Y_2_R;
            _RIGHT_ROI_1.Top = _Model.Y_1_R;

            _RIGHT_ROI_5.Width = _Model.Width_R;
            _RIGHT_ROI_4.Width = _Model.Width_R;
            _RIGHT_ROI_3.Width = _Model.Width_R;
            _RIGHT_ROI_2.Width = _Model.Width_R;
            _RIGHT_ROI_1.Width = _Model.Width_R;

            _RIGHT_ROI_5.Height = _Model.Height_R;
            _RIGHT_ROI_4.Height = _Model.Height_R;
            _RIGHT_ROI_3.Height = _Model.Height_R;
            _RIGHT_ROI_2.Height = _Model.Height_R;
            _RIGHT_ROI_1.Height = _Model.Height_R;

            _RIGHT_ROI_BTN_5.Width = _Model.Width_R;
            _RIGHT_ROI_BTN_4.Width = _Model.Width_R;
            _RIGHT_ROI_BTN_3.Width = _Model.Width_R;
            _RIGHT_ROI_BTN_2.Width = _Model.Width_R;
            _RIGHT_ROI_BTN_1.Width = _Model.Width_R;

            _RIGHT_ROI_BTN_5.Height = _Model.Height_R;
            _RIGHT_ROI_BTN_4.Height = _Model.Height_R;
            _RIGHT_ROI_BTN_3.Height = _Model.Height_R;
            _RIGHT_ROI_BTN_2.Height = _Model.Height_R;
            _RIGHT_ROI_BTN_1.Height = _Model.Height_R;

            //dTbox_grid_R.Text = _Model.Grid_R.ToString();

            _LEFT_ROI_1.Visible = false;
            _LEFT_ROI_2.Visible = false;
            _LEFT_ROI_3.Visible = false;
            _LEFT_ROI_4.Visible = false;
            _LEFT_ROI_5.Visible = false;

            _LEFT_ROI_1.Visible = true;
            _LEFT_ROI_2.Visible = true;
            _LEFT_ROI_3.Visible = true;
            _LEFT_ROI_4.Visible = true;
            _LEFT_ROI_5.Visible = true;

            _RIGHT_ROI_1.Visible = false;
            _RIGHT_ROI_2.Visible = false;
            _RIGHT_ROI_3.Visible = false;
            _RIGHT_ROI_4.Visible = false;
            _RIGHT_ROI_5.Visible = false;

            _RIGHT_ROI_1.Visible = true;
            _RIGHT_ROI_2.Visible = true;
            _RIGHT_ROI_3.Visible = true;
            _RIGHT_ROI_4.Visible = true;
            _RIGHT_ROI_5.Visible = true;
        }
        #endregion

        #region method - Porcess
        void Porcess(string cam, int roi, Model.Blob blob, int threshold, string mode)
        {
            //roi 정보를 읽어온다(x, y, widht, height)
            (int roiX, int roiY, int roiWidth, int roiHeight) = GetRoiInfo(cam, roi);

            //평균을 구한다 (model == pixle)  이면 픽셀 평균,  (model == temper) 이면 온도 평균
            double roi_avg = GetAvg(roiX, roiY, roiWidth, roiHeight, cam, mode);
            roi_avg = (int)Math.Round((double)(roi_avg / (double)(roiWidth * roiHeight)), 0);

            //blob의 중심점, blob영역의 평균을 구한다
            List<Model.ProcessData> procDataList = new List<Model.ProcessData>();
            double blob_avg = GetBlobCenter_GetBlobAvg(ref procDataList, roiX, roiWidth, blob.Y, blob.Height, threshold, mode, cam, roi);

            //위아래로 스캔하면서 gap영역을 찾는다
            ScanUp(ref procDataList, roiY, blob_avg, roi_avg, mode, cam, roi);
            ScanDw(ref procDataList, roiY, roiHeight, blob_avg, roi_avg, mode, cam, roi);

            //결과 저장
            if (cam == "CAM1")
            {
                if (roi == 1) { _Data_1_L = procDataList; }
                else if (roi == 2) { _Data_2_L = procDataList; }
                else if (roi == 3) { _Data_3_L = procDataList; }
                else if (roi == 4) { _Data_4_L = procDataList; }
                else if (roi == 5) { _Data_5_L = procDataList; }
            }
            else if (cam == "CAM2")
            {
                if (roi == 1) { _Data_1_R = procDataList; }
                else if (roi == 2) { _Data_2_R = procDataList; }
                else if (roi == 3) { _Data_3_R = procDataList; }
                else if (roi == 4) { _Data_4_R = procDataList; }
                else if (roi == 5) { _Data_5_R = procDataList; }
            }
        }

        void ToGray(string cam)
        {
            if (cam == "cam1")
            {
                for (int xx = 0; xx < cloneBmap_L.Width; xx++)
                {
                    for (int yy = 0; yy < cloneBmap_L.Height; yy++)
                    {
                        Color old = cloneBmap_L.GetPixel(xx, yy);
                        int grayScale = (int)((old.R * 0.3) + (old.G * 0.59) + (old.B * 0.11));
                        Color newC = Color.FromArgb(old.A, grayScale, grayScale, grayScale);

                        grayBmap_L.SetPixel(xx, yy, newC);
                    }
                }

            }
            else if (cam == "cam2")
            {
                for (int xx = 0; xx < cloneBmap_R.Width; xx++)
                {
                    for (int yy = 0; yy < cloneBmap_R.Height; yy++)
                    {
                        Color old = cloneBmap_R.GetPixel(xx, yy);
                        int grayScale = (int)((old.R * 0.3) + (old.G * 0.59) + (old.B * 0.11));
                        Color newC = Color.FromArgb(old.A, grayScale, grayScale, grayScale);

                        grayBmap_R.SetPixel(xx, yy, newC);
                    }
                }
            }

        }

        List<Model.Blob> GetOkBlob(List<Model.Blob> blobs, string cam)
        {
            List<Model.Blob> okBlobs = new List<Model.Blob>();

            if (cam == "cam1")
            {
                foreach (Model.Blob blob in blobs)
                {
                    //blob의 가로길이가 이미지의 가로길이만큼 여부 확인
                    if (blob.Width == grayBmap_L.Width && blob.Height >= 3)
                    {
                        int startY = blob.Y;
                        int endY = startY + blob.Height;

                        //blob의 시작점 Y와 끝점 Y가 roi영역을 넘어서는지 여부 확인
                        if (startY >= _LEFT_ROI_BTN_1.Top && endY <= _LEFT_ROI_BTN_1.Top + _LEFT_ROI_BTN_1.Height)
                        {
                            Model.Blob resultBlob = new Model.Blob(blob.X, blob.Y, blob.Width, blob.Height);
                            okBlobs.Add(resultBlob);
                        }
                    }
                }
            }
            else if (cam == "cam2")
            {
                foreach (Model.Blob blob in blobs)
                {
                    //blob의 가로길이가 이미지의 가로길이만큼 여부 확인
                    if (blob.Width == grayBmap_R.Width && blob.Height >= 3)
                    {
                        int startY = blob.Y;
                        int endY = startY + blob.Height;

                        //blob의 시작점 Y와 끝점 Y가 roi영역을 넘어서는지 여부 확인
                        if (startY >= _RIGHT_ROI_BTN_1.Top && endY <= _RIGHT_ROI_BTN_1.Top + _RIGHT_ROI_BTN_1.Height)
                        {
                            Model.Blob resultBlob = new Model.Blob(blob.X, blob.Y, blob.Width, blob.Height);
                            okBlobs.Add(resultBlob);
                        }
                    }
                }
            }

            return okBlobs;
        }

        (int, int, int, int) GetRoiInfo(string cam, int roi)
        {
            int roiX = 0;
            int roiY = 0;
            int roiWidth = 0;
            int roiHeight = 0;

            if (cam == "CAM1")
            {
                if (roi == 1) { roiX = _LEFT_ROI_1.Left; roiY = _LEFT_ROI_1.Top; roiWidth = _LEFT_ROI_1.Width; roiHeight = _LEFT_ROI_1.Height; }
                else if (roi == 2) { roiX = _LEFT_ROI_2.Left; roiY = _LEFT_ROI_2.Top; roiWidth = _LEFT_ROI_2.Width; roiHeight = _LEFT_ROI_2.Height; }
                else if (roi == 3) { roiX = _LEFT_ROI_3.Left; roiY = _LEFT_ROI_3.Top; roiWidth = _LEFT_ROI_3.Width; roiHeight = _LEFT_ROI_3.Height; }
                else if (roi == 4) { roiX = _LEFT_ROI_4.Left; roiY = _LEFT_ROI_4.Top; roiWidth = _LEFT_ROI_4.Width; roiHeight = _LEFT_ROI_4.Height; }
                else if (roi == 5) { roiX = _LEFT_ROI_5.Left; roiY = _LEFT_ROI_5.Top; roiWidth = _LEFT_ROI_5.Width; roiHeight = _LEFT_ROI_5.Height; }
            }
            else if (cam == "CAM2")
            {
                if (roi == 1) { roiX = _RIGHT_ROI_1.Left; roiY = _RIGHT_ROI_1.Top; roiWidth = _RIGHT_ROI_1.Width; roiHeight = _RIGHT_ROI_1.Height; }
                else if (roi == 2) { roiX = _RIGHT_ROI_2.Left; roiY = _RIGHT_ROI_2.Top; roiWidth = _RIGHT_ROI_2.Width; roiHeight = _RIGHT_ROI_2.Height; }
                else if (roi == 3) { roiX = _RIGHT_ROI_3.Left; roiY = _RIGHT_ROI_3.Top; roiWidth = _RIGHT_ROI_3.Width; roiHeight = _RIGHT_ROI_3.Height; }
                else if (roi == 4) { roiX = _RIGHT_ROI_4.Left; roiY = _RIGHT_ROI_4.Top; roiWidth = _RIGHT_ROI_4.Width; roiHeight = _RIGHT_ROI_4.Height; }
                else if (roi == 5) { roiX = _RIGHT_ROI_5.Left; roiY = _RIGHT_ROI_5.Top; roiWidth = _RIGHT_ROI_5.Width; roiHeight = _RIGHT_ROI_5.Height; }
            }

            return (roiX, roiY, roiWidth, roiHeight);
        }

        double GetAvg(int roiX, int roiY, int roiWidth, int roiHeight, string cam, string mode)
        {
            double roi_avg = 0;

            for (int xx = 0; xx < roiWidth; xx++)
            {
                int x = 0;
                int y = 0;

                x = xx + roiX;

                for (int yy = 0; yy < roiHeight; yy++)
                {
                    y = yy + roiY;

                    if (mode == "pixel")
                    {
                        int pixelValue = 0;

                        if (cam == "CAM1")
                            pixelValue = grayBmap_L.GetPixel(x, y).R;
                        else if (cam == "CAM2")
                            pixelValue = grayBmap_R.GetPixel(x, y).R;

                        roi_avg = roi_avg + pixelValue;

                    }
                    else if (mode == "temper")
                    {
                        if (cam == "CAM1")
                            roi_avg = roi_avg + _tempData_L[x, y];
                        else if (cam == "CAM2")
                            roi_avg = roi_avg + _tempData_R[x, y];
                    }

                }
            }

            return roi_avg;
        }

        double GetBlobCenter_GetBlobAvg(ref List<Model.ProcessData> procDataList, int roiX, int roiWidth, int bolb_Y, int bolb_Height, int threshold, string mode, string cam, int roi)
        {
            double blob_avg = 0;
            //Blob의 중심점을 구한다
            //roi 가로 스캔
            for (int xx = 0; xx < roiWidth; xx++)
            {
                int x = 0;
                int y = 0;

                x = xx + roiX;

                bool isCount_flag = false;

                int blobStartY = 0;
                int blobEndY = 0;

                //blob 세로 스캔
                for (int yy = 0; yy < bolb_Height; yy++)
                {
                    y = yy + bolb_Y;

                    int pixelValue = 0;

                    if (cam == "CAM1")
                    {
                        pixelValue = grayBmap_L.GetPixel(x, y).R;
                        if (roi == 3)
                        {
                            if (x == originBmap_L.Width / 2)
                            {
                                if (pixelValue > grayBmap_L.GetPixel(x, brighestDrawLine_L).R) brighestDrawLine_L = y;
                            }
                        }
                    }
                    else if (cam == "CAM2")
                    {
                        pixelValue = grayBmap_R.GetPixel(x, y).R;
                        if (roi == 3)
                        {
                            if (x == originBmap_R.Width / 2)
                            {
                                if (pixelValue > grayBmap_R.GetPixel(x, brighestDrawLine_R).R) brighestDrawLine_R = y;
                            }
                        }
                    }
                    if (isCount_flag == false)
                    {
                        if (pixelValue > threshold)
                        {
                            blobStartY = y;
                            isCount_flag = true;
                        }
                    }
                    else if (isCount_flag == true)
                    {
                        if (pixelValue < threshold)
                            blobEndY = y;
                    }

                    if (mode == "pixel")
                        blob_avg = blob_avg + pixelValue;
                    else if (mode == "temper")
                        if (cam == "CAM1")
                            blob_avg = blob_avg + _tempData_L[x, y];
                        else if (cam == "CAM2")
                            blob_avg = blob_avg + _tempData_R[x, y];
                }

                if (blobEndY == 0)
                    blobEndY = bolb_Y + bolb_Height;

                Model.ProcessData data = new Model.ProcessData();

                int centerY = blobStartY + ((blobEndY - blobStartY) / 2);

                data.BlobCenterY = centerY;
                data.X = x;

                procDataList.Add(data);
            }

            blob_avg = (int)Math.Round((double)(blob_avg / (double)(roiWidth * bolb_Height)), 0);

            return blob_avg;
        }

        void ScanUp(ref List<Model.ProcessData> procDataList, int roiY, double blob_avg, double roi_avg, string mode, string cam, int roi)
        {
            for (int i = 0; i < procDataList.Count; i++)
            {
                bool isWhile_upScan = true;
                int scanIdx_up = procDataList[i].BlobCenterY;

                //위쪽으로 스캔
                while (isWhile_upScan)
                {
                    if (scanIdx_up <= roiY)
                    {
                        isWhile_upScan = false;
                        procDataList[i].StartY = procDataList[i].BlobCenterY;
                    }
                    else
                    {
                        int x = procDataList[i].X;

                        int y1 = scanIdx_up - 1;
                        int y2 = scanIdx_up;

                        if (mode == "pixel")
                        {
                            int nextPixelValue = 0;
                            int nowPixelValue = 0;

                            if (cam == "CAM1")
                            {
                                nextPixelValue = grayBmap_L.GetPixel(x, y1).R;
                                nowPixelValue = grayBmap_L.GetPixel(x, y2).R;
                            }
                            else if (cam == "CAM2")
                            {
                                nextPixelValue = grayBmap_R.GetPixel(x, y1).R;
                                nowPixelValue = grayBmap_R.GetPixel(x, y2).R;
                            }

                            double per = blob_avg - ((blob_avg - roi_avg) * 0.3);


                            if (nextPixelValue < per)
                            {
                                procDataList[i].StartY = y2;
                                procDataList[i].SubPixelValue_up = GetSubPixel(nowPixelValue, nextPixelValue);
                                isWhile_upScan = false;
                            }
                        }
                        else if (mode == "temper")
                        {
                            int nextTemperValue = 0;
                            int nowTemperValue = 0;

                            if (cam == "CAM1")
                            {
                                nextTemperValue = (int)_tempData_L[x, y1];
                                nowTemperValue = (int)_tempData_L[x, y2];
                            }
                            else if (cam == "CAM2")
                            {
                                nextTemperValue = (int)_tempData_R[x, y1];
                                nowTemperValue = (int)_tempData_R[x, y2];
                            }

                            double per = blob_avg - ((blob_avg - roi_avg) * 0.8);

                            if (nextTemperValue < per)
                            {
                                procDataList[i].StartY = y2;
                                procDataList[i].SubPixelValue_up = GetSubPixel(nowTemperValue, nextTemperValue);
                                isWhile_upScan = false;
                            }
                        }
                    }

                    scanIdx_up = scanIdx_up - 1;
                }
            }
        }

        void ScanDw(ref List<Model.ProcessData> procDataList, int roiY, int roiHeight, double blob_avg, double roi_avg, string mode, string cam, int roi)
        {
            for (int i = 0; i < procDataList.Count; i++)
            {
                bool isWhile_dwScan = true;
                int scanIdx_dw = procDataList[i].BlobCenterY;

                //아래쪽으로 스캔
                while (isWhile_dwScan)
                {
                    if (scanIdx_dw >= roiY + roiHeight - 1)
                    {
                        isWhile_dwScan = false;
                        procDataList[i].EndY = procDataList[i].BlobCenterY + 1;
                        procDataList[i].Count = procDataList[i].EndY - procDataList[i].StartY;
                    }
                    else
                    {
                        int x = procDataList[i].X;

                        int y1 = scanIdx_dw;
                        int y2 = scanIdx_dw + 1;

                        if (mode == "pixel")
                        {
                            int nextPixelValue = 0;
                            int nowPixelValue = 0;

                            if (cam == "CAM1")
                            {
                                nextPixelValue = grayBmap_L.GetPixel(x, y2).R;
                                nowPixelValue = grayBmap_L.GetPixel(x, y1).R;
                            }
                            else if (cam == "CAM2")
                            {
                                nextPixelValue = grayBmap_R.GetPixel(x, y2).R;
                                nowPixelValue = grayBmap_R.GetPixel(x, y1).R;
                            }

                            //double per = blob_avg - ((blob_avg - roi_avg) * 0.1);
                            double per = blob_avg - ((blob_avg - roi_avg) * 0.3);
                            //blob_avg - (blob_avg * 0.2);
                            //double per = blob_avg * 1.2;

                            if (nextPixelValue < per)
                            {
                                procDataList[i].EndY = y1;
                                procDataList[i].SubPixelValue_dw = GetSubPixel(nowPixelValue, nextPixelValue);
                                procDataList[i].Count = procDataList[i].EndY - procDataList[i].StartY + 1;
                                isWhile_dwScan = false;
                            }
                        }
                        else if (mode == "temper")
                        {
                            int nextTemperValue = 0;
                            int nowTemperValue = 0;

                            if (cam == "CAM1")
                            {
                                nextTemperValue = (int)_tempData_L[x, y2];
                                nowTemperValue = (int)_tempData_L[x, y1];
                            }
                            else if (cam == "CAM2")
                            {
                                nextTemperValue = (int)_tempData_R[x, y2];
                                nowTemperValue = (int)_tempData_R[x, y1];
                            }

                            double per = blob_avg - ((blob_avg - roi_avg) * 0.8);
                            //double per = blob_avg * 0.6;

                            if (nextTemperValue < per)
                            {
                                procDataList[i].EndY = y1;
                                procDataList[i].SubPixelValue_dw = GetSubPixel(nowTemperValue, nextTemperValue);
                                procDataList[i].Count = procDataList[i].EndY - procDataList[i].StartY + 1;
                                isWhile_dwScan = false;
                            }
                        }
                    }

                    scanIdx_dw = scanIdx_dw + 1;
                }
            }
        }
        #endregion

        #region method - sub pixel 연산
        int GetSubPixel(int value, int nextValue)
        {
            double result = 0;

            double gap = (double)value / 10.0;

            result = Math.Round(nextValue / gap, 0);


            return (int)result;
        }
        #endregion

        #region method - DrawZoom
        void DrawRoiZoom_L(int roi, bool isThread)
        {
            List<Model.ProcessData> data_list = new List<Model.ProcessData>();

            int startY_temp = 480;
            int endY_temp = 0;

            int startY = 480;
            int endY = 0;

            int x = 0;
            int tempX = 0;
            int width = 0;

            if (roi == 1)
            {
                data_list = _Data_1_L;
                x = _LEFT_ROI_1.Left;
                tempX = _LEFT_ROI_1.Left;
                width = _LEFT_ROI_1.Width;
            }
            else if (roi == 2)
            {
                data_list = _Data_2_L;
                x = _LEFT_ROI_2.Left;
                tempX = _LEFT_ROI_2.Left;
                width = _LEFT_ROI_2.Width;
            }
            else if (roi == 3)
            {
                data_list = _Data_3_L;
                x = _LEFT_ROI_3.Left;
                tempX = _LEFT_ROI_3.Left;
                width = _LEFT_ROI_3.Width;
            }
            else if (roi == 4)
            {
                data_list = _Data_4_L;
                x = _LEFT_ROI_4.Left;
                tempX = _LEFT_ROI_4.Left;
                width = _LEFT_ROI_4.Width;
            }
            else if (roi == 5)
            {
                data_list = _Data_5_L;
                x = _LEFT_ROI_5.Left;
                tempX = _LEFT_ROI_5.Left;
                width = _LEFT_ROI_5.Width;
            }

            // ROI 확대 영역의 y값의 최소값과  최대값을 구한다
            for (int i = 0; i < data_list.Count; i++)
            {
                startY_temp = data_list[i].StartY;
                endY_temp = data_list[i].EndY;

                if (startY > startY_temp)
                    startY = startY_temp;

                if (endY < endY_temp)
                    endY = endY_temp;
            }

            //ROI 확대 영역의 경계선라인을 그린다
            for (int i = 0; i < data_list.Count; i++)
            {
                int up = data_list[i].StartY;
                int dw = data_list[i].EndY;

                Color newC = Color.Black;

                cloneBmap_L.SetPixel(tempX, up - 1, newC);
                cloneBmap_L.SetPixel(tempX, dw + 1, newC);

                tempX = tempX + 1;
            }

            int y = startY - 4;
            int height = (endY - startY) + 9;

            Bitmap bMap = cloneBmap_L.Clone(new Rectangle(x, y, width, height), originBmap_L.PixelFormat);

            if (roi == 1)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_1_L.Image = bMap;
                        dPic_roi_1_L.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_1_L.Image = bMap;
                    dPic_roi_1_L.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 2)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_2_L.Image = bMap;
                        dPic_roi_2_L.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_2_L.Image = bMap;
                    dPic_roi_2_L.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 3)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_3_L.Image = bMap;
                        dPic_roi_3_L.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_3_L.Image = bMap;
                    dPic_roi_3_L.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 4)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_4_L.Image = bMap;
                        dPic_roi_4_L.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_4_L.Image = bMap;
                    dPic_roi_4_L.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 5)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_5_L.Image = bMap;
                        dPic_roi_5_L.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_5_L.Image = bMap;
                    dPic_roi_5_L.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        void DrawRoiZoom_R(int roi, bool isThread)
        {
            List<Model.ProcessData> data_list = new List<Model.ProcessData>();

            int startY_temp = 480;
            int endY_temp = 0;

            int startY = 480;
            int endY = 0;

            int x = 0;
            int tempX = 0;
            int width = 0;

            if (roi == 1)
            {
                data_list = _Data_1_R;
                x = _RIGHT_ROI_BTN_1.Left;
                tempX = _RIGHT_ROI_BTN_1.Left;
                width = _RIGHT_ROI_BTN_1.Width;
            }
            else if (roi == 2)
            {
                data_list = _Data_2_R;
                x = _RIGHT_ROI_BTN_2.Left;
                tempX = _RIGHT_ROI_BTN_2.Left;
                width = _RIGHT_ROI_BTN_2.Width;
            }
            else if (roi == 3)
            {
                data_list = _Data_3_R;
                x = _RIGHT_ROI_BTN_3.Left;
                tempX = _RIGHT_ROI_BTN_3.Left;
                width = _RIGHT_ROI_BTN_3.Width;
            }
            else if (roi == 4)
            {
                data_list = _Data_4_R;
                x = _RIGHT_ROI_BTN_4.Left;
                tempX = _RIGHT_ROI_BTN_4.Left;
                width = _RIGHT_ROI_BTN_4.Width;
            }
            else if (roi == 5)
            {
                data_list = _Data_5_R;
                x = _RIGHT_ROI_BTN_5.Left;
                tempX = _RIGHT_ROI_BTN_5.Left;
                width = _RIGHT_ROI_BTN_5.Width;
            }

            // ROI 확대 영역의 y값의 최소값과  최대값을 구한다
            for (int i = 0; i < data_list.Count; i++)
            {
                startY_temp = data_list[i].StartY;
                endY_temp = data_list[i].EndY;

                if (startY > startY_temp)
                    startY = startY_temp;

                if (endY < endY_temp)
                    endY = endY_temp;
            }

            //ROI 확대 영역의 경계선라인을 그린다
            for (int i = 0; i < data_list.Count; i++)
            {
                int up = data_list[i].StartY;
                int dw = data_list[i].EndY;

                Color newC = Color.Black;

                cloneBmap_R.SetPixel(tempX, up - 1, newC);
                cloneBmap_R.SetPixel(tempX, dw + 1, newC);

                tempX = tempX + 1;
            }

            int y = startY - 4;
            int height = (endY - startY) + 9;

            Bitmap bMap = cloneBmap_R.Clone(new Rectangle(x, y, width, height), originBmap_R.PixelFormat);

            if (roi == 1)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_1_R.Image = bMap;
                        dPic_roi_1_R.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_1_R.Image = bMap;
                    dPic_roi_1_R.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 2)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_2_R.Image = bMap;
                        dPic_roi_2_R.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_2_R.Image = bMap;
                    dPic_roi_2_R.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 3)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_3_R.Image = bMap;
                        dPic_roi_3_R.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_3_R.Image = bMap;
                    dPic_roi_3_R.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 4)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_4_R.Image = bMap;
                        dPic_roi_4_R.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_4_R.Image = bMap;
                    dPic_roi_4_R.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else if (roi == 5)
            {
                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dPic_roi_5_R.Image = bMap;
                        dPic_roi_5_R.SizeMode = PictureBoxSizeMode.StretchImage;
                    }));
                }
                else
                {
                    dPic_roi_5_R.Image = bMap;
                    dPic_roi_5_R.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }
        #endregion

        #region method - 차트 디자인 변경
        void InitChartDesign()
        {
            //차트 배경 변경
            dChart_L_1.ChartAreas[0].BackColor = Color.Black;
            dChart_L_2.ChartAreas[0].BackColor = Color.Black;
            dChart_L_3.ChartAreas[0].BackColor = Color.Black;
            dChart_L_4.ChartAreas[0].BackColor = Color.Black;
            dChart_L_5.ChartAreas[0].BackColor = Color.Black;

            dChart_R_1.ChartAreas[0].BackColor = Color.Black;
            dChart_R_2.ChartAreas[0].BackColor = Color.Black;
            dChart_R_3.ChartAreas[0].BackColor = Color.Black;
            dChart_R_4.ChartAreas[0].BackColor = Color.Black;
            dChart_R_5.ChartAreas[0].BackColor = Color.Black;

            //차트 이름 변경
            dChart_L_1.Series[0].LegendText = "LEFT 1";
            dChart_L_2.Series[0].LegendText = "LEFT 2";
            dChart_L_3.Series[0].LegendText = "LEFT 3";
            dChart_L_4.Series[0].LegendText = "LEFT 4";
            dChart_L_5.Series[0].LegendText = "LEFT 5";

            dChart_R_1.Series[0].LegendText = "RIGHT 1";
            dChart_R_2.Series[0].LegendText = "RIGHT 2";
            dChart_R_3.Series[0].LegendText = "RIGHT 3";
            dChart_R_4.Series[0].LegendText = "RIGHT 4";
            dChart_R_5.Series[0].LegendText = "RIGHT 5";

            //x 라인 색상 변경
            dChart_L_1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_4.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_5.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;

            dChart_R_1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_R_2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_R_3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_R_4.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_R_5.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;

            //y 라인 색상 변경
            dChart_L_1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_4.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_5.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;

            dChart_R_1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_R_2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_R_3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_R_4.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_R_5.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;

            //x 라인 라벨 색상 변경
            dChart_L_1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_3.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_4.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_5.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;

            dChart_R_1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_R_2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_R_3.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_R_4.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_R_5.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;

            //y 라인 라벨 색상 변경
            dChart_L_1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_3.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_4.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_5.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            dChart_R_1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_R_2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_R_3.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_R_4.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_R_5.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            //x 라인 격자로 변경
            dChart_L_1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_3.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_4.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_5.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            dChart_R_1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_3.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_4.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_5.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //y 라인 격자로 변경
            dChart_L_1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_3.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_4.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_5.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            dChart_R_1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_3.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_4.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_R_5.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //라인 색상 변경
            dChart_L_1.Series[0].Color = Color.Crimson;
            dChart_L_2.Series[0].Color = Color.Crimson;
            dChart_L_3.Series[0].Color = Color.Crimson;
            dChart_L_4.Series[0].Color = Color.Crimson;
            dChart_L_5.Series[0].Color = Color.Crimson;

            dChart_R_1.Series[0].Color = Color.Crimson;
            dChart_R_2.Series[0].Color = Color.Crimson;
            dChart_R_3.Series[0].Color = Color.Crimson;
            dChart_R_4.Series[0].Color = Color.Crimson;
            dChart_R_5.Series[0].Color = Color.Crimson;

            //라인 두깨 변경
            dChart_L_1.Series[0].BorderWidth = 2;
            dChart_L_2.Series[0].BorderWidth = 2;
            dChart_L_3.Series[0].BorderWidth = 2;
            dChart_L_4.Series[0].BorderWidth = 2;
            dChart_L_5.Series[0].BorderWidth = 2;

            dChart_R_1.Series[0].BorderWidth = 2;
            dChart_R_2.Series[0].BorderWidth = 2;
            dChart_R_3.Series[0].BorderWidth = 2;
            dChart_R_4.Series[0].BorderWidth = 2;
            dChart_R_5.Series[0].BorderWidth = 2;

            //제목 배경 색상 변경
            dChart_L_1.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_2.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_3.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_4.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_5.Legends[0].BackColor = Color.FromArgb(45, 45, 45);

            dChart_R_1.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_R_2.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_R_3.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_R_4.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_R_5.Legends[0].BackColor = Color.FromArgb(45, 45, 45);

            // 제목 글씨 색상 변경
            dChart_L_1.Legends[0].ForeColor = Color.White;
            dChart_L_2.Legends[0].ForeColor = Color.White;
            dChart_L_3.Legends[0].ForeColor = Color.White;
            dChart_L_4.Legends[0].ForeColor = Color.White;
            dChart_L_5.Legends[0].ForeColor = Color.White;

            dChart_R_1.Legends[0].ForeColor = Color.White;
            dChart_R_2.Legends[0].ForeColor = Color.White;
            dChart_R_3.Legends[0].ForeColor = Color.White;
            dChart_R_4.Legends[0].ForeColor = Color.White;
            dChart_R_5.Legends[0].ForeColor = Color.White;



        }
        #endregion

        #region method - Chart
        void DrawChart_L(bool isThread)
        {
            if (isThread == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dChart_L_1.Series[0].Points.Clear();
                    dChart_L_2.Series[0].Points.Clear();
                    dChart_L_3.Series[0].Points.Clear();
                    dChart_L_4.Series[0].Points.Clear();
                    dChart_L_5.Series[0].Points.Clear();
                }));
            }
            else
            {
                dChart_L_1.Series[0].Points.Clear();
                dChart_L_2.Series[0].Points.Clear();
                dChart_L_3.Series[0].Points.Clear();
                dChart_L_4.Series[0].Points.Clear();
                dChart_L_5.Series[0].Points.Clear();
            }

            List<int> roi1 = new List<int>();
            List<int> roi2 = new List<int>();
            List<int> roi3 = new List<int>();
            List<int> roi4 = new List<int>();
            List<int> roi5 = new List<int>();

            List<int> xAxis = new List<int>();

            int minAxisY = 10000;

            SettingsService settingService = new SettingsService();
            double resolution = double.Parse(settingService.Read("resolution", "y_1"));

            for (int i = 0; i < _Data_1_L.Count; i++)
            {
                xAxis.Add(i);

                double gap_1 = Math.Round(_Data_1_L[i].Count * resolution, 0);
                double gap_2 = Math.Round(_Data_2_L[i].Count * resolution, 0);
                double gap_3 = Math.Round(_Data_3_L[i].Count * resolution, 0);
                double gap_4 = Math.Round(_Data_4_L[i].Count * resolution, 0);
                double gap_5 = Math.Round(_Data_5_L[i].Count * resolution, 0);

                double subGap_1 = (resolution / (10 - (double)_Data_1_L[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_1_L[i].SubPixelValue_dw));
                double subGap_2 = (resolution / (10 - (double)_Data_2_L[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_2_L[i].SubPixelValue_dw));
                double subGap_3 = (resolution / (10 - (double)_Data_3_L[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_3_L[i].SubPixelValue_dw));
                double subGap_4 = (resolution / (10 - (double)_Data_4_L[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_4_L[i].SubPixelValue_dw));
                double subGap_5 = (resolution / (10 - (double)_Data_5_L[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_5_L[i].SubPixelValue_dw));

                roi1.Add((int)gap_1 + (int)subGap_1);
                roi2.Add((int)gap_2 + (int)subGap_2);
                roi3.Add((int)gap_3 + (int)subGap_3);
                roi4.Add((int)gap_4 + (int)subGap_4);
                roi5.Add((int)gap_5 + (int)subGap_5);

                if (minAxisY > gap_1) minAxisY = (int)gap_1 + (int)subGap_1;
                if (minAxisY > gap_2) minAxisY = (int)gap_2 + (int)subGap_2;
                if (minAxisY > gap_3) minAxisY = (int)gap_3 + (int)subGap_3;
                if (minAxisY > gap_4) minAxisY = (int)gap_4 + (int)subGap_4;
                if (minAxisY > gap_5) minAxisY = (int)gap_5 + (int)subGap_5;

            }

            //차트 y축 시작점 설정
            if (minAxisY > 20)
                minAxisY = minAxisY - 10;

            if (isThread == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dChart_L_1.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_L_2.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_L_3.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_L_4.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_L_5.ChartAreas[0].AxisY.Minimum = minAxisY;

                    //차트 데이터 바인딩
                    dChart_L_1.Series[0].Points.DataBindXY(xAxis, roi1);
                    dChart_L_2.Series[0].Points.DataBindXY(xAxis, roi2);
                    dChart_L_3.Series[0].Points.DataBindXY(xAxis, roi3);
                    dChart_L_4.Series[0].Points.DataBindXY(xAxis, roi4);
                    dChart_L_5.Series[0].Points.DataBindXY(xAxis, roi5);
                }));
            }
            else
            {
                dChart_L_1.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_L_2.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_L_3.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_L_4.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_L_5.ChartAreas[0].AxisY.Minimum = minAxisY;

                //차트 데이터 바인딩
                dChart_L_1.Series[0].Points.DataBindXY(xAxis, roi1);
                dChart_L_2.Series[0].Points.DataBindXY(xAxis, roi2);
                dChart_L_3.Series[0].Points.DataBindXY(xAxis, roi3);
                dChart_L_4.Series[0].Points.DataBindXY(xAxis, roi4);
                dChart_L_5.Series[0].Points.DataBindXY(xAxis, roi5);
            }

        }

        void DrawChart_R(bool isThread)
        {
            if (isThread == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dChart_R_1.Series[0].Points.Clear();
                    dChart_R_2.Series[0].Points.Clear();
                    dChart_R_3.Series[0].Points.Clear();
                    dChart_R_4.Series[0].Points.Clear();
                    dChart_R_5.Series[0].Points.Clear();
                }));
            }
            else
            {
                dChart_R_1.Series[0].Points.Clear();
                dChart_R_2.Series[0].Points.Clear();
                dChart_R_3.Series[0].Points.Clear();
                dChart_R_4.Series[0].Points.Clear();
                dChart_R_5.Series[0].Points.Clear();
            }

            List<int> roi1 = new List<int>();
            List<int> roi2 = new List<int>();
            List<int> roi3 = new List<int>();
            List<int> roi4 = new List<int>();
            List<int> roi5 = new List<int>();

            List<int> xAxis = new List<int>();

            int minAxisY = 10000;

            SettingsService settingService = new SettingsService();
            double resolution = double.Parse(settingService.Read("resolution", "y_2"));

            for (int i = 0; i < _Data_1_R.Count; i++)
            {
                xAxis.Add(i);

                double gap_1 = Math.Round(_Data_1_R[i].Count * resolution, 0);
                double gap_2 = Math.Round(_Data_2_R[i].Count * resolution, 0);
                double gap_3 = Math.Round(_Data_3_R[i].Count * resolution, 0);
                double gap_4 = Math.Round(_Data_4_R[i].Count * resolution, 0);
                double gap_5 = Math.Round(_Data_5_R[i].Count * resolution, 0);

                double subGap_1 = (resolution / (10 - (double)_Data_1_R[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_1_R[i].SubPixelValue_dw));
                double subGap_2 = (resolution / (10 - (double)_Data_2_R[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_2_R[i].SubPixelValue_dw));
                double subGap_3 = (resolution / (10 - (double)_Data_3_R[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_3_R[i].SubPixelValue_dw));
                double subGap_4 = (resolution / (10 - (double)_Data_4_R[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_4_R[i].SubPixelValue_dw));
                double subGap_5 = (resolution / (10 - (double)_Data_5_R[i].SubPixelValue_up)) + (resolution / (10 - (double)_Data_5_R[i].SubPixelValue_dw));

                roi1.Add((int)gap_1 + (int)subGap_1);
                roi2.Add((int)gap_2 + (int)subGap_2);
                roi3.Add((int)gap_3 + (int)subGap_3);
                roi4.Add((int)gap_4 + (int)subGap_4);
                roi5.Add((int)gap_5 + (int)subGap_5);

                if (minAxisY > gap_1) minAxisY = (int)gap_1 + (int)subGap_1;
                if (minAxisY > gap_2) minAxisY = (int)gap_2 + (int)subGap_2;
                if (minAxisY > gap_3) minAxisY = (int)gap_3 + (int)subGap_3;
                if (minAxisY > gap_4) minAxisY = (int)gap_4 + (int)subGap_4;
                if (minAxisY > gap_5) minAxisY = (int)gap_5 + (int)subGap_5;
            }

            //차트 y축 시작점 설정
            if (minAxisY > 20)
                minAxisY = minAxisY - 10;

            if (isThread == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dChart_R_1.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_R_2.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_R_3.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_R_4.ChartAreas[0].AxisY.Minimum = minAxisY;
                    dChart_R_5.ChartAreas[0].AxisY.Minimum = minAxisY;

                    //차트 데이터 바인딩
                    dChart_R_1.Series[0].Points.DataBindXY(xAxis, roi1);
                    dChart_R_2.Series[0].Points.DataBindXY(xAxis, roi2);
                    dChart_R_3.Series[0].Points.DataBindXY(xAxis, roi3);
                    dChart_R_4.Series[0].Points.DataBindXY(xAxis, roi4);
                    dChart_R_5.Series[0].Points.DataBindXY(xAxis, roi5);
                }));
            }
            else
            {
                dChart_R_1.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_R_2.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_R_3.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_R_4.ChartAreas[0].AxisY.Minimum = minAxisY;
                dChart_R_5.ChartAreas[0].AxisY.Minimum = minAxisY;

                //차트 데이터 바인딩
                dChart_R_1.Series[0].Points.DataBindXY(xAxis, roi1);
                dChart_R_2.Series[0].Points.DataBindXY(xAxis, roi2);
                dChart_R_3.Series[0].Points.DataBindXY(xAxis, roi3);
                dChart_R_4.Series[0].Points.DataBindXY(xAxis, roi4);
                dChart_R_5.Series[0].Points.DataBindXY(xAxis, roi5);
            }

        }
        #endregion

        #region method - gap평균 텍스에 찍어주기
        double WriteGapAvg(string cam, bool isThread)
        {
            if (cam == "cam1")
            {
                SettingsService settingService = new SettingsService();
                double resolution = double.Parse(settingService.Read("resolution", "y_1"));
                List<double> avg = new List<double>() { 0, 0, 0, 0, 0 };

                avg[0] = Math.Round(_Data_1_L.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[1] = Math.Round(_Data_2_L.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[2] = Math.Round(_Data_3_L.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[3] = Math.Round(_Data_4_L.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[4] = Math.Round(_Data_5_L.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);


                //double totalAVg = avg1 + avg2 + avg3 + avg4 + avg5;
                double totalAVg = isIncludeROI(isCheckboxROI_L[0], avg[0]) +
                    isIncludeROI(isCheckboxROI_L[1], avg[1]) +
                    isIncludeROI(isCheckboxROI_L[2], avg[2]) +
                    isIncludeROI(isCheckboxROI_L[3], avg[3]) +
                    isIncludeROI(isCheckboxROI_L[4], avg[4]);

                totalAVg = Math.Round(totalAVg / getNumberIncludeROIAverage(isCheckboxROI_L), 2);

                //double gappixelcount = 0;
                //for (int i=280; i<360; i++)
                //{
                //    gappixelcount +=
                //    TempVariation_L[i].IndexOf(TempVariation_L[i].Max(x => x)) - TempVariation_L[i].IndexOf(TempVariation_L[i].Min(x => x));
                //}
                //gappixelcount = Math.Round(gappixelcount / 80, 2);

                CalculateGap();

                string reesult =
                                 //"GAP 평균 \r\n" + "\r\n" +
                                 //             "LEFT_1 : " + avg[0].ToString() + "\r\n" + "\r\n" +
                                 //             "LEFT_2 : " + avg[1].ToString() + "\r\n" + "\r\n" +
                                 //             "LEFT_3 : " + avg[2].ToString() + "\r\n" + "\r\n" +
                                 //             "LEFT_4 : " + avg[3].ToString() + "\r\n" + "\r\n" +
                                 //             "LEFT_5 : " + avg[4].ToString() + "\r\n" + "\r\n" + "\r\n" +
                                 "Gap Distance : " + Math.Round(CalculateGap() * resolution, 2);
                //"전체평균 : " + Math.Round((gappixelcount) * resolution,2);
                //"전체평균 : " + totalAVg;


                //TempVariation_L.Clear();

                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dTxt_cam1.Text = reesult;
                    }));
                }
                else
                {
                    dTxt_cam1.Text = reesult;
                }

                return totalAVg;
            }
            else if (cam == "cam2")
            {
                SettingsService settingService = new SettingsService();
                double resolution = double.Parse(settingService.Read("resolution", "y_2"));
                List<double> avg = new List<double>() { 0, 0, 0, 0, 0 };

                avg[0] = Math.Round(_Data_1_R.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[1] = Math.Round(_Data_2_R.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[2] = Math.Round(_Data_3_R.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[3] = Math.Round(_Data_4_R.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);
                avg[4] = Math.Round(_Data_5_R.Average(item => item.Count * resolution + (resolution / (10 - item.SubPixelValue_up)) + (resolution / (10 - item.SubPixelValue_dw))), 2);

                double totalAVg = isIncludeROI(isCheckboxROI_R[0], avg[0]) +
                    isIncludeROI(isCheckboxROI_R[1], avg[1]) +
                    isIncludeROI(isCheckboxROI_R[2], avg[2]) +
                    isIncludeROI(isCheckboxROI_R[3], avg[3]) +
                    isIncludeROI(isCheckboxROI_R[4], avg[4]);

                totalAVg = Math.Round(totalAVg / getNumberIncludeROIAverage(isCheckboxROI_R), 2);

                string reesult = "GAP 평균 \r\n" + "\r\n" +
                                 "LEFT_1 : " + avg[0].ToString() + "\r\n" + "\r\n" +
                                 "LEFT_2 : " + avg[1].ToString() + "\r\n" + "\r\n" +
                                 "LEFT_3 : " + avg[2].ToString() + "\r\n" + "\r\n" +
                                 "LEFT_4 : " + avg[3].ToString() + "\r\n" + "\r\n" +
                                 "LEFT_5 : " + avg[4].ToString() + "\r\n" + "\r\n" + "\r\n" +
                                 "전체평균 : " + totalAVg;

                if (isThread == true)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        dTxt_cam2.Text = reesult;
                    }));
                }
                else
                {
                    dTxt_cam2.Text = reesult;
                }

                return totalAVg;
            }
            return 0;
        }
        #endregion

        #region [method - PLC Check Statue]
        public bool PLC_Check_Status(string PLCDeviceaddr)
        {
            Form1._MELSEC.actUtlType64.GetDevice(PLCDeviceaddr, out int value);
            return (value == 1) ? true : false;
            //return bool.Parse(value.ToString());
        }
        #endregion

        #region [method - VISION Auto]
        public void VISION_Auto()
        {
            Form1._MELSEC.actUtlType64.SetDevice("B3", 1);
        }
        #endregion

        #region [method - VISION Manual]
        public void VISION_Manual()
        {
            Form1._MELSEC.actUtlType64.SetDevice("B3", 0);
        }
        #endregion

        #region [method - mesure function]
        private void measureFunc1(object sender, EventArgs e)
        {
            brighestDrawLine_L = 0;
            Panel_BrighestLine_L.Visible = false;
            dBtn_snap1_Click(sender, e);
            dBtn_Process1_Click(sender, e);
        }

        private void measureFunc2(object sender, EventArgs e)
        {
            brighestDrawLine_R = 0;
            Panel_BrighestLine_R.Visible = false;
            dBtn_snap2_Click(sender, e);
            dBtn_Process2_Click(sender, e);
        }
        #endregion

        #region [method - check each roi isinclude]
        public double isIncludeROI(bool isCheckboxROI, double value)
        {

            if (isCheckboxROI == true)
            {
                return value;
            }
            else return 0;
        }
        #endregion

        #region [method - count include roi]
        public int getNumberIncludeROIAverage(bool[] isCheckboxROI)
        {
            Debug.Print(isCheckboxROI.Count(t => t).ToString());
            return isCheckboxROI.Count(t => t);
        }
        #endregion

        #region [method - Select Jog Axis]
        public void selectJogAxis_L_Y()
        {
            //D31051~4 = 해당 축 넘버
            //M30175 = 정회전
            //M30176 = 역회전

            // D31051 - L_Y축
            MELSEC_JOG.actUtlType64.SetDevice("B20", short.Parse("1"));

            // 나머지 축값 0으로
            MELSEC_JOG.actUtlType64.SetDevice("B21", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B22", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B23", short.Parse("0"));
        }
        public void selectJogAxis_L_Z()
        {

            MELSEC_JOG.actUtlType64.SetDevice("B21", short.Parse("1"));

            // 나머지 축값 0으로
            MELSEC_JOG.actUtlType64.SetDevice("B20", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B22", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B23", short.Parse("0"));
        }
        public void selectJogAxis_R_Y()
        {

            MELSEC_JOG.actUtlType64.SetDevice("B22", short.Parse("1"));

            // 나머지 축값 0으로
            MELSEC_JOG.actUtlType64.SetDevice("B21", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B20", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B23", short.Parse("0"));
        }
        public void selectJogAxis_R_Z()
        {

            MELSEC_JOG.actUtlType64.SetDevice("B23", short.Parse("1"));

            // 나머지 축값 0으로
            MELSEC_JOG.actUtlType64.SetDevice("B21", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B22", short.Parse("0"));
            MELSEC_JOG.actUtlType64.SetDevice("B20", short.Parse("0"));
        }
        #endregion

        #region [method - JogAxisMoveToInputPosition]
        public void selectedJogMoveToInputPosition()
        {
            //W10, W12에 적절한 데이터가 있는지 확인후 없으면 SET해주고 MOVE할수 있게 한다.
            MELSEC_JOG.actUtlType64.SetDevice("B2C", short.Parse("1"));
        }
        #endregion

        #region [method - PLCMotorSetVelocityValue]
        public void PLCMotorSetVelocityValue()
        {
            //W10~W11 - Motor_ Command Velocity value [0.01mm/s단위] -> 10[mm/s] = 1000
            //MELSEC_JOG.actUtlType64.WriteDeviceBlock("W10", );
        }
        #endregion

        #region [method - PLCMotorSetPositionValue]
        public void PLCMotorSetPositionValue()
        {
            //W12~W13 - Motor_ Command position value [0.0001mm단위] (Absolute 위치) -> 10[mm/s] = 100000
            //MELSEC_JOG.actUtlType64.WriteDeviceBlock("W12", );
        }
        #endregion

        #region [method - JogAutoMoveforSavedValue]
        public void PLCJogAutoMoveToSavedValue()
        {
            int vel = int.Parse(service.Read("PLC_JOG_OFFSET", "VELOCITY"));
            int pos = int.Parse(service.Read("PLC_JOG_OFFSET", "POSITION "));

            MELSEC_JOG.actUtlType64.WriteDeviceBlock("W10", 2, ref vel);
            MELSEC_JOG.actUtlType64.WriteDeviceBlock("W12", 2, ref pos);

            MELSEC_JOG.actUtlType64.SetDevice("B2C", short.Parse("1"));
        }
        #endregion

        #region [method - JogButtonMouseEvent]
        public void jogButtonOriginalClickDown()
        {
            MELSEC_JOG.actUtlType64.SetDevice("B2A", short.Parse("1"));
        }
        public void jogButtonOriginalClickUp()
        {
            MELSEC_JOG.actUtlType64.SetDevice("B2A", short.Parse("0"));
        }
        public void jogButtonReverseClickDown()
        {
            MELSEC_JOG.actUtlType64.SetDevice("B2B", short.Parse("1"));
        }
        public void jogButtonReverseClickUp()
        {
            MELSEC_JOG.actUtlType64.SetDevice("B2B", short.Parse("0"));
        }
        #endregion

        #region [method - cameraCenterLineDraw]
        public void cameraCenterLineDraw_L()
        {
            if (Panel_CameraCenterLine_L.Visible == true) Panel_CameraCenterLine_L.Visible = false;
            else Panel_CameraCenterLine_L.Visible = true;
            Panel_CameraCenterLine_L.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + pictureBox1.Height / 2 - 1);
        }
        public void cameraCenterLineDraw_R()
        {
            if (Panel_CameraCenterLine_R.Visible == true) Panel_CameraCenterLine_R.Visible = false;
            else Panel_CameraCenterLine_R.Visible = true;
            Panel_CameraCenterLine_R.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + pictureBox2.Height / 2 - 1);
        }
        #endregion

        #region [method - brightestLineDraw]
        public void brightestLineDraw_L()
        {
            if (Panel_BrighestLine_L.Visible == true) Panel_BrighestLine_L.Visible = false;
            else Panel_BrighestLine_L.Visible = true;
            Panel_BrighestLine_L.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + brighestDrawLine_L - 1);
        }
        public void brightestLineDraw_R()
        {
            if (Panel_BrighestLine_R.Visible == true) Panel_BrighestLine_R.Visible = false;
            else Panel_BrighestLine_R.Visible = true;
            Panel_BrighestLine_R.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + brighestDrawLine_R - 1);
        }
        #endregion

        #region [PLC, VISION Heartbit]
        public void PLC_HealthCheck()
        {
            _MELSEC_HEART.actUtlType64.GetDevice("B100", out int heartbitvalue);
            //_MELSEC_HEART.actUtlType64.GetDevice("M20303", out int heartbitvalue);
            if (PLC_Heartbit_Checker == heartbitvalue)
            {
                PLC_Heartbit_Count++;
                PLC_Heartbit_Checker = heartbitvalue;
            }
            else
            {
                PLC_Heartbit_Count = 0;
                PLC_Heartbit_Checker = heartbitvalue;
            }

            if (PLC_Heartbit_Count > 20)
            {
                // PLC의 heartbit값을 검사하며 정상적이지 않을때 health Check 경고알람
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dLabel_Ng_L.ForeColor = Color.Red;
                    dLabel_Ng_L.Text = "PLC HealthCheck Error. - " + DateTime.Now.ToString("HH:mm:ss");
                }));

            }
        }

        public void VISION_Heartbit(object? o)
        {
            //PLC_HealthCheck();

            try
            {
                if (_bVision_Heartbit == false)
                {
                    _MELSEC_HEART.actUtlType64.SetDevice2("B0", 1);
                    //_MELSEC_HEART.actUtlType64.SetDevice2("M20305", 1);
                    _bVision_Heartbit = true;
                    //Debug.Print("hoo");
                }
                else
                {
                    _MELSEC_HEART.actUtlType64.SetDevice2("B0", 0);
                    //_MELSEC_HEART.actUtlType64.SetDevice2("M20305", 0);
                    _bVision_Heartbit = false;
                    //Debug.Print("woo");
                }
            }
            catch
            {
                LogManager log = new LogManager(@"C:\JD\log\", LogType.Daily);
                log.WriteLine("VISION HEARTBIT PLC에 Writing Error");
            }
        }

        #endregion

        #region [method - CalculateGap]
        private double CalculateGap()
        {

            double avg_high_pixel = 0;
            int low_std = 236;
            int gap_pixel = 0;
            double subpixel = 0;

            for (int r = 248; r < 400; r++)
            {

                if (TempDDList[320][r] > avg_high_pixel)
                {
                    avg_high_pixel = TempDDList[320][r];
                }

            }

            var UpperStd = TempDDList[320].FindIndex(x => x > avg_high_pixel);
            gap_pixel = low_std - UpperStd;
            Debug.Print("오른쪽 가장 높은값(index) : " + avg_high_pixel + "(" + TempDDList[320].FindLastIndex(x => x == avg_high_pixel).ToString() + "), " + "Left First Edge point : " + UpperStd.ToString());


            subpixel = (TempDDList[320][UpperStd] - avg_high_pixel) / (TempDDList[320][UpperStd] - TempDDList[320][UpperStd - 1]);
            Debug.Print("픽셀 수: " + gap_pixel.ToString() + ", Gap : " + (Math.Round((double)gap_pixel + subpixel, 2).ToString()));

            return low_std - Math.Round(GetXLinearEquation(UpperStd, TempDDList[320][UpperStd], UpperStd - 1, TempDDList[320][UpperStd - 1], avg_high_pixel), 2);
            //return (Math.Round((double)gap_pixel + subpixel, 2));
        }
        #endregion

        #region [method - LinearEquation]
        private double GetXLinearEquation(double x1, double y1, double x2, double y2, double y3)
        {
            double a = ((y1 - y2) / (x1 - x2));
            double b = (x1 * y2 - x2 * y1) / (x1 - x2);


            return Math.Round((y3 - b) / a, 2);
        }
        #endregion

        #region [method - BottomLineSetting]
        private void BottomLineSetting()
        {
            double pVariation = 0;
            int c_PixelIndex = 0;
            for (int i = 0; i < 479; i++)
            {
                if (Math.Abs(TempDDList[320][i] - TempDDList[320][i + 1]) > pVariation)
                {
                    c_PixelIndex = i;
                    pVariation = Math.Abs(TempDDList[320][i] - TempDDList[320][i + 1]);
                }
            }

            if (c_PixelIndex == int.Parse(service.Read("STANDARD", "BtmIdx"))) MessageBox.Show("OK, BottomLine Setting OK", "Setting OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Fail, BottomLine Setting Fail, Please Retry Setting", "Setting Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region [method - GaussianBlurJPROCAlgorithm]
        private double GaussianBlurJPROCAlgorithm()
        {
            StreamReader srGaussian = new StreamReader(@"C:\JD\Images\GaussianBlur\GaussianBlur.csv");

            int row = 0;


            while (!srGaussian.EndOfStream)
            {
                string lineGaussian = srGaussian.ReadLine();
                string[] GaussianData = lineGaussian.Split(',');

                for (int x = 0; x < 640; x++) GaussianArray[x, row] = int.Parse(GaussianData[x]);//(double.Parse(data[x]) * 10.0d) + 1000;

                row = row + 1;
            }

            double total = 0, utotal = 0, dtotal = 0;

            for (int column = 0; column < 640; column++)
            {
                GaussianBmp.Add(new List<int>());

                for (int r = 0; r < 479; r++)
                {
                    GaussianBmp[column].Add(GaussianArray[column, r]);
                }

                var sUp = GaussianBmp[column].IndexOf(GaussianBmp[column].Max());
                var sDown = GaussianBmp[column].LastIndexOf(GaussianBmp[column].Max());
                var iUp = 0;
                var iDown = 0;


                for (int i = sUp; i > 0; i--)
                {
                    var std = GaussianBmp[column][i] - GaussianBmp[column][i - 1];
                    if (std == 0)
                    {
                        iUp = i;

                        Debug.Print(iUp.ToString());
                        break;
                    }
                }
                for (int i = sDown; i > 0; i++)
                {
                    var std = GaussianBmp[column][i] - GaussianBmp[column][i + 1];
                    if (std == 0)
                    {
                        iDown = i;
                        Debug.Print(iDown.ToString());
                        break;
                    }
                }

                total += iDown - iUp;
                utotal += iUp;
                dtotal += iDown;


            }


            Debug.Print((utotal / 640).ToString());
            Debug.Print((dtotal / 640).ToString());
            Debug.Print((total / 640).ToString());

            return (double)(total / 640);
        }


        #endregion

        #region [method - SnaptoGaussianImagewithCSV]
        private void SnaptoGaussianImagewithCSV(Image beforeGaussian)
        {
            bmpforscv = beforeGaussian;
            Gblur = new GaussianBlur(bmpforscv as Bitmap);
            var result = Gblur.Process(3);
            result.Save(@"C:\JD\Images\GaussianBlur\GaussianBlur.bmp", ImageFormat.Bmp);
            Gblur.ConvertBmpToCsv(@"C:\JD\Images\GaussianBlur\GaussianBlur.bmp", @"C:\JD\Images\GaussianBlur\GaussianBlur.csv");

        }
        #endregion

        #region [method - WhenImageLoadCalculateGaussianfileterToOriginalTempCSV]
        private void CalculateGaussianfileterToOriginalTempCSV(string TempCSV)
        {
            var CSVDirPath = Path.GetDirectoryName(TempCSV);
            var CSVFilePath = Path.GetFileNameWithoutExtension(TempCSV);
            var CSVExtensionPath = Path.GetExtension(TempCSV);

            var GussedCSVpath = Path.Combine(CSVDirPath, $"{CSVFilePath}GProcessed{CSVExtensionPath}");

            var DoubleDList = DataConvertFormat.ConvertCSVToDLList(TempCSV, 640, 480);
            GaussianBlur.ApplyGaussianBlurForCSV(DoubleDList, 480, 640, GussedCSVpath);
        }
        #endregion

        #region [method - TempController_TimerCheck]
        private void TempController_TimerCheck(object? state)
        {
            if (TempController.IsOpened() == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    Lbl_TempController.Text = "Temp_OK";
                    Lbl_TempController.BackColor = Color.Aquamarine;
                    Label_PV_Value.Text = TempController.GetCurrentTemperature(TempController);
                    Label_SV_Value.Text = TempController.GetCurrentTargetTemperature(TempController);
                }));

            }
            else
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    Lbl_TempController.BackColor = Color.Red;
                }));

            }
        }
        #endregion


        private void Btn_SetSV_Click(object sender, EventArgs e)
        {
            TempController.SetTargetTemperature(TempController, int.Parse(TextBox_InputSV.textBox.Text));
            TextBox_InputSV.textBox.Clear();
        }
    }
}