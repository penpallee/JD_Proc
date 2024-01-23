using JD_Proc.ICam;
using JD_Proc.Service;
using Microsoft.Win32;
using OpenCvSharp;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Collections.Specialized.BitVector32;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.DataVisualization.Charting;
using ReaLTaiizor.Controls;
using System;

namespace JD_Proc
{
    public partial class Form1 : Form
    {
        #region var
        IrDirectInterface _irDirectInterface_1;
        IrDirectInterface _irDirectInterface_2;

        Thread _imageGrabberThread_1;
        Thread _imageGrabberThread_2;

        bool _grabImages_1 = false;
        bool _grabImages_2 = false;

        Thread _snapThread_1;
        Thread _snapThread_2;

        Bitmap originBmap_L;
        Bitmap originBmap_R;

        Bitmap cloneBmap_L;
        Bitmap cloneBmap_R;

        Bitmap grayBmap;

        Model.Models _Model = new Model.Models();

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

        string state = "manual";
        #endregion

        #region 생성자
        public Form1()
        {
            InitializeComponent();

            //카메라 연결
            //Connect("generic1.xml", 1);
            //Connect("generic2.xml", 2);


            //MakeROI();

            Service.SettingsService service = new Service.SettingsService();
            string modelPath = service.Read("model", "path");
            //SetModel(modelPath);

            InitChartDesign();


            //Mat img = Cv2.ImRead(@"aa.png", ImreadModes.Color);
            //Cv2.ImShow("img", img);
            //Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
            //Cv2.ImShow("gray_img", img);
            //Cv2.WaitKey(0);
            //Cv2.DestroyAllWindows();

            //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            //timer.Interval = 1000;
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }

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

        #region event(live / live stop) - click
        private void dBtn_live1_Click(object sender, EventArgs e)
        {
            _imageGrabberThread_1 = new Thread(new ThreadStart(ImageGrabberMethode1));
            _grabImages_1 = true;
            _imageGrabberThread_1.Start();
        }

        private void dBtn_live2_Click(object sender, EventArgs e)
        {
            _imageGrabberThread_2 = new Thread(new ThreadStart(ImageGrabberMethode2));
            _grabImages_2 = true;
            _imageGrabberThread_2.Start();
        }

        private void dBtn_stop1_Click(object sender, EventArgs e)
        {
            if (_imageGrabberThread_1 != null)
            {
                _grabImages_1 = false;
                _imageGrabberThread_1.Join();
            }
        }

        private void dBtn_stop2_Click(object sender, EventArgs e)
        {
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
                dLabel_autoState.Text = "AUTO";
                dLabel_autoState.ForeColor = Color.Lime;
                state = "auto";
            }
            else if (state == "auto")
            {
                dLabel_autoState.Text = "MANUAL";
                dLabel_autoState.ForeColor = Color.FromArgb(114, 118, 127);
                state = "manual";
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
            grayBmap = new Bitmap(640, 480, originBmap_L.PixelFormat);

            for (int xx = 0; xx < cloneBmap_L.Width; xx++)
            {
                for (int yy = 0; yy < cloneBmap_L.Height; yy++)
                {
                    Color old = cloneBmap_L.GetPixel(xx, yy);
                    int grayScale = (int)((old.R * 0.3) + (old.G * 0.59) + (old.B * 0.11));
                    Color newC = Color.FromArgb(old.A, grayScale, grayScale, grayScale);

                    grayBmap.SetPixel(xx, yy, newC);

                }
            }

            Porcess("CAM1", 1);
            Porcess("CAM1", 2);
            Porcess("CAM1", 3);
            Porcess("CAM1", 4);
            Porcess("CAM1", 5);

            DrawRoiZoom_L(1);
            DrawRoiZoom_L(2);
            DrawRoiZoom_L(3);
            DrawRoiZoom_L(4);
            DrawRoiZoom_L(5);

            DrawChart_L();
        }

        private void dBtn_Process2_Click(object sender, EventArgs e)
        {
            originBmap_R = (Bitmap)pictureBox2.Image;
            cloneBmap_R = (Bitmap)originBmap_R.Clone();
            grayBmap = new Bitmap(640, 480, originBmap_R.PixelFormat);

            //그래이 이미지로 변환
            for (int xx = 0; xx < cloneBmap_R.Width; xx++)
            {
                for (int yy = 0; yy < cloneBmap_R.Height; yy++)
                {
                    Color old = cloneBmap_R.GetPixel(xx, yy);
                    int grayScale = (int)((old.R * 0.3) + (old.G * 0.59) + (old.B * 0.11));
                    Color newC = Color.FromArgb(old.A, grayScale, grayScale, grayScale);

                    grayBmap.SetPixel(xx, yy, newC);
                }
            }

            Porcess("CAM2", 1);
            Porcess("CAM2", 2);
            Porcess("CAM2", 3);
            Porcess("CAM2", 4);
            Porcess("CAM2", 5);

            DrawRoiZoom_R(1);
            DrawRoiZoom_R(2);
            DrawRoiZoom_R(3);
            DrawRoiZoom_R(4);
            DrawRoiZoom_R(5);
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
                pictureBox1.Image = Bitmap.FromFile(image_file);

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
                pictureBox2.Image = Bitmap.FromFile(image_file);

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
        }

        private void dBtn_imageSave2_Click(object sender, EventArgs e)
        {
            string saveFolder = @"C:\JD\images\" + DateTime.Now.ToString("yyyy-MM-dd");

            if (!System.IO.Directory.Exists(saveFolder))
                System.IO.Directory.CreateDirectory(saveFolder);

            pictureBox2.Image.Save(saveFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
        #endregion

        #region event(settings) - click
        private void dBtn_settings_Click(object sender, EventArgs e)
        {
            SettingForm modelForm = new SettingForm();
            modelForm.StartPosition = FormStartPosition.Manual;
            modelForm.Location = new System.Drawing.Point(500, 100);

            modelForm.ShowDialog();
        }
        #endregion

        #region event(grid view) - click
        private void dBtn_gridView1_Click(object sender, EventArgs e)
        {
            if (dPan_grid_L_1.Visible == true)
            {
                dPan_grid_L_1.Visible = false;
                dPan_grid_L_2.Visible = false;
                dPan_grid_L_3.Visible = false;
                dPan_grid_L_4.Visible = false;
                dPan_grid_L_5.Visible = false;
            }
            else if (dPan_grid_L_1.Visible == false)
            {
                dPan_grid_L_1.Visible = true;
                dPan_grid_L_2.Visible = true;
                dPan_grid_L_3.Visible = true;
                dPan_grid_L_4.Visible = true;
                dPan_grid_L_5.Visible = true;
            }
        }

        private void dBtn_gridView2_Click(object sender, EventArgs e)
        {
            if (dPan_grid_R_1.Visible == true)
            {
                dPan_grid_R_1.Visible = false;
                dPan_grid_R_2.Visible = false;
                dPan_grid_R_3.Visible = false;
                dPan_grid_R_4.Visible = false;
                dPan_grid_R_5.Visible = false;
            }
            else if (dPan_grid_R_1.Visible == false)
            {
                dPan_grid_R_1.Visible = true;
                dPan_grid_R_2.Visible = true;
                dPan_grid_R_3.Visible = true;
                dPan_grid_R_4.Visible = true;
                dPan_grid_R_5.Visible = true;
            }
        }
        #endregion

        #region event(dComboBox) - 콤보박스 인덱스 바꿀때

        private void dComboBox_scale1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _irDirectInterface_1.SetPaletteFormat(
                (OptrisColoringPalette)Enum.Parse(typeof(OptrisColoringPalette), (string)dComboBox_cam1.SelectedItem),
                (OptrisPaletteScalingMethod)Enum.Parse(typeof(OptrisPaletteScalingMethod), (string)dComboBox_scale1.SelectedItem));
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

            if (_imageGrabberThread_2 != null)
                _imageGrabberThread_2.Join(3000);

            if (_snapThread_1 != null)
                _snapThread_1.Join(3000);

            if (_snapThread_1 != null)
                _snapThread_1.Join(3000);

            //clean up
            //_irDirectInterface_1.Disconnect();
            //_irDirectInterface_2.Disconnect();
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
        }
        #endregion

        #region method - 영상 live 
        private void ImageGrabberMethode1()
        {
            while (_grabImages_1)
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
                    dLable_tmp1.Text = Math.Round(mean, 1) + " ℃";
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
                    dLable_tmp2.Text = Math.Round(mean, 1) + " ℃";
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
                dLable_tmp1.Text = Math.Round(mean, 1) + " ℃";

            }));
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
                dLable_tmp2.Text = Math.Round(mean, 1) + " ℃";
            }));
        }

        #endregion

        #region method - make Roi
        void MakeROI()
        {
            (_LEFT_ROI_BTN_5, _LEFT_ROI_5) = MakeRect("LEFT_ROI_5", 0, 0, 5, 5, Brushes.Yellow, "LEFT_5", Color.White, "LEFT");
            (_LEFT_ROI_BTN_4, _LEFT_ROI_4) = MakeRect("LEFT_ROI_4", 0, 0, 5, 5, Brushes.Yellow, "LEFT_4", Color.White, "LEFT");
            (_LEFT_ROI_BTN_3, _LEFT_ROI_3) = MakeRect("LEFT_ROI_3", 0, 0, 5, 5, Brushes.Yellow, "LEFT_3", Color.White, "LEFT");
            (_LEFT_ROI_BTN_2, _LEFT_ROI_2) = MakeRect("LEFT_ROI_2", 0, 0, 5, 5, Brushes.Yellow, "LEFT_2", Color.White, "LEFT");
            (_LEFT_ROI_BTN_1, _LEFT_ROI_1) = MakeRect("LEFT_ROI_1", 0, 0, 5, 5, Brushes.Yellow, "LEFT_1", Color.White, "LEFT");

            (_RIGHT_ROI_BTN_5, _RIGHT_ROI_5) = MakeRect("RIGHT_ROI_5", 0, 0, 5, 5, Brushes.Yellow, "RIGHT_5", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_4, _RIGHT_ROI_4) = MakeRect("RIGHT_ROI_4", 0, 0, 5, 5, Brushes.Yellow, "RIGHT_4", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_3, _RIGHT_ROI_3) = MakeRect("RIGHT_ROI_3", 0, 0, 5, 5, Brushes.Yellow, "RIGHT_3", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_2, _RIGHT_ROI_2) = MakeRect("RIGHT_ROI_2", 0, 0, 5, 5, Brushes.Yellow, "RIGHT_2", Color.White, "RIGHT");
            (_RIGHT_ROI_BTN_1, _RIGHT_ROI_1) = MakeRect("RIGHT_ROI_1", 0, 0, 5, 5, Brushes.Yellow, "RIGHT_1", Color.White, "RIGHT");
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
                _ROIRect.roiLabel.Size = new System.Drawing.Size(80, 20);
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

            dTbox_grid_L.Text = _Model.Grid_L.ToString();

            dPan_grid_L_1.Top = pictureBox1.Top + _Model.Grid_Y_L;
            dPan_grid_L_2.Top = pictureBox1.Top + _Model.Grid_Y_L + _Model.Grid_pixel_L;
            dPan_grid_L_3.Top = pictureBox1.Top + _Model.Grid_Y_L + (_Model.Grid_pixel_L * 2);
            dPan_grid_L_4.Top = pictureBox1.Top + _Model.Grid_Y_L + (_Model.Grid_pixel_L * 3);
            dPan_grid_L_5.Top = pictureBox1.Top + _Model.Grid_Y_L + (_Model.Grid_pixel_L * 4);

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

            dTbox_grid_R.Text = _Model.Grid_R.ToString();

            dPan_grid_R_1.Top = pictureBox2.Top + _Model.Grid_Y_R;
            dPan_grid_R_2.Top = pictureBox2.Top + _Model.Grid_Y_R + _Model.Grid_pixel_R;
            dPan_grid_R_3.Top = pictureBox2.Top + _Model.Grid_Y_R + (_Model.Grid_pixel_R * 2);
            dPan_grid_R_4.Top = pictureBox2.Top + _Model.Grid_Y_R + (_Model.Grid_pixel_R * 3);
            dPan_grid_R_5.Top = pictureBox2.Top + _Model.Grid_Y_R + (_Model.Grid_pixel_R * 4);

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
        void Porcess(string cam, int roi)
        {
            //double resoluton = 0.0;
            //Service.SettingsService settingService = new SettingsService();

            //if (cam == "CAM1")
            //    resoluton = double.Parse(settingService.Read("resolution", "y_1"));
            //else if (cam == "CAM2")
            //    resoluton = double.Parse(settingService.Read("resolution", "y_2"));

            int roiX = 0;
            int roiY = 0;
            int roiWidth = 0;
            int roiHeight = 0;

            if (cam == "CAM1")
            {
                if (roi == 1) { roiX = _LEFT_ROI_BTN_1.Left; roiY = _LEFT_ROI_BTN_1.Top; roiWidth = _LEFT_ROI_BTN_1.Width; roiHeight = _LEFT_ROI_BTN_1.Height; }
                else if (roi == 2) { roiX = _LEFT_ROI_BTN_2.Left; roiY = _LEFT_ROI_BTN_2.Top; roiWidth = _LEFT_ROI_BTN_2.Width; roiHeight = _LEFT_ROI_BTN_2.Height; }
                else if (roi == 3) { roiX = _LEFT_ROI_BTN_3.Left; roiY = _LEFT_ROI_BTN_3.Top; roiWidth = _LEFT_ROI_BTN_3.Width; roiHeight = _LEFT_ROI_BTN_3.Height; }
                else if (roi == 4) { roiX = _LEFT_ROI_BTN_4.Left; roiY = _LEFT_ROI_BTN_4.Top; roiWidth = _LEFT_ROI_BTN_4.Width; roiHeight = _LEFT_ROI_BTN_4.Height; }
                else if (roi == 5) { roiX = _LEFT_ROI_BTN_5.Left; roiY = _LEFT_ROI_BTN_5.Top; roiWidth = _LEFT_ROI_BTN_5.Width; roiHeight = _LEFT_ROI_BTN_5.Height; }
            }
            else if (cam == "CAM2")
            {
                if (roi == 1) { roiX = _RIGHT_ROI_BTN_1.Left; roiY = _RIGHT_ROI_BTN_1.Top; roiWidth = _RIGHT_ROI_BTN_1.Width; roiHeight = _RIGHT_ROI_BTN_1.Height; }
                else if (roi == 2) { roiX = _RIGHT_ROI_BTN_2.Left; roiY = _RIGHT_ROI_BTN_2.Top; roiWidth = _RIGHT_ROI_BTN_2.Width; roiHeight = _RIGHT_ROI_BTN_2.Height; }
                else if (roi == 3) { roiX = _RIGHT_ROI_BTN_3.Left; roiY = _RIGHT_ROI_BTN_3.Top; roiWidth = _RIGHT_ROI_BTN_3.Width; roiHeight = _RIGHT_ROI_BTN_3.Height; }
                else if (roi == 4) { roiX = _RIGHT_ROI_BTN_4.Left; roiY = _RIGHT_ROI_BTN_4.Top; roiWidth = _RIGHT_ROI_BTN_4.Width; roiHeight = _RIGHT_ROI_BTN_4.Height; }
                else if (roi == 5) { roiX = _RIGHT_ROI_BTN_5.Left; roiY = _RIGHT_ROI_BTN_5.Top; roiWidth = _RIGHT_ROI_BTN_5.Width; roiHeight = _RIGHT_ROI_BTN_5.Height; }
            }

            pictureBox2.Image = grayBmap;

            List<Model.ProcessData> procDataList = new List<Model.ProcessData>();

            //가로 스캔
            for (int xx = 0; xx < roiWidth; xx++)
            {
                int x = 0;
                int y = 0;

                bool overNum_flag = false;

                int overCnt = 0;
                int overcnt_temp = 1;

                int blobStartY = 0;
                int blobStartY_temp = 0;

                //세로 스캔
                for (int yy = 0; yy < roiHeight; yy++)
                {
                    x = xx + roiX;
                    y = yy + roiY;

                    int pixelValue = grayBmap.GetPixel(x, y).R;

                    if (cam == "CAM1" && roi == 5 && xx == 0)
                    {
                        int pixelValueaa = grayBmap.GetPixel(x - 1, y - 1).R;
                        int gra = Math.Abs(pixelValueaa - pixelValue);
                        Debug.WriteLine(pixelValueaa + ", " + gra);
                    }

                    // 픽셀영역의 값이 100보다 크고 다음 영역의 연속성 (count 갯수)의 갯수를 보고 제일 카운팅 갯수가 많은 y값을 저장한다
                    if (pixelValue >= 100)
                    {
                        if (overNum_flag == false)
                        {
                            blobStartY_temp = y;
                            overNum_flag = true;
                        }
                        else if (overNum_flag == true)
                        {
                            overcnt_temp = overcnt_temp + 1;
                        }
                    }
                    else
                    {
                        if (overNum_flag == true)
                        {
                            if (overcnt_temp > overCnt)
                            {
                                blobStartY = blobStartY_temp;
                                overCnt = overcnt_temp;
                                overcnt_temp = 1;
                            }
                        }

                        overNum_flag = false;
                    }
                }

                Model.ProcessData data = new Model.ProcessData();

                data.X = x;
                data.StartY = blobStartY;
                data.EndY = blobStartY + overCnt - 1;
                data.Count = overCnt;
                data.SubPixelValue_up = grayBmap.GetPixel(x, blobStartY - 1).R;
                data.SubPixelValue_dw = grayBmap.GetPixel(x, blobStartY + overCnt).R;

                procDataList.Add(data);

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
        }
        #endregion

        #region method - DrawZoom
        void DrawRoiZoom_L(int roi)
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
                x = _LEFT_ROI_BTN_1.Left;
                tempX = _LEFT_ROI_BTN_1.Left;
                width = _LEFT_ROI_BTN_1.Width;
            }
            else if (roi == 2)
            {
                data_list = _Data_2_L;
                x = _LEFT_ROI_BTN_2.Left;
                tempX = _LEFT_ROI_BTN_2.Left;
                width = _LEFT_ROI_BTN_2.Width;
            }
            else if (roi == 3)
            {
                data_list = _Data_3_L;
                x = _LEFT_ROI_BTN_3.Left;
                tempX = _LEFT_ROI_BTN_3.Left;
                width = _LEFT_ROI_BTN_3.Width;
            }
            else if (roi == 4)
            {
                data_list = _Data_4_L;
                x = _LEFT_ROI_BTN_4.Left;
                tempX = _LEFT_ROI_BTN_4.Left;
                width = _LEFT_ROI_BTN_4.Width;
            }
            else if (roi == 5)
            {
                data_list = _Data_5_L;
                x = _LEFT_ROI_BTN_5.Left;
                tempX = _LEFT_ROI_BTN_5.Left;
                width = _LEFT_ROI_BTN_5.Width;
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
                dPic_roi_1_L.Image = bMap;
                dPic_roi_1_L.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 2)
            {
                dPic_roi_2_L.Image = bMap;
                dPic_roi_2_L.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 3)
            {
                dPic_roi_3_L.Image = bMap;
                dPic_roi_3_L.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 4)
            {
                dPic_roi_4_L.Image = bMap;
                dPic_roi_4_L.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 5)
            {
                dPic_roi_5_L.Image = bMap;
                dPic_roi_5_L.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        void DrawRoiZoom_R(int roi)
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

            Bitmap bMap = cloneBmap_R.Clone(new Rectangle(x, y, width, height), originBmap_L.PixelFormat);

            if (roi == 1)
            {
                dPic_roi_1_R.Image = bMap;
                dPic_roi_1_R.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 2)
            {
                dPic_roi_2_R.Image = bMap;
                dPic_roi_2_R.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 3)
            {
                dPic_roi_3_R.Image = bMap;
                dPic_roi_3_R.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 4)
            {
                dPic_roi_4_R.Image = bMap;
                dPic_roi_4_R.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (roi == 5)
            {
                dPic_roi_5_R.Image = bMap;
                dPic_roi_5_R.SizeMode = PictureBoxSizeMode.StretchImage;
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

            //차트 이름 변경
            dChart_L_1.Series[0].LegendText = "LEFT 1";
            dChart_L_2.Series[0].LegendText = "LEFT 2";
            dChart_L_3.Series[0].LegendText = "LEFT 3";
            dChart_L_4.Series[0].LegendText = "LEFT 4";
            dChart_L_5.Series[0].LegendText = "LEFT 5";

            //x 라인 색상 변경
            dChart_L_1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_4.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            dChart_L_5.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;

            //y 라인 색상 변경
            dChart_L_1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_4.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            dChart_L_5.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;

            //x 라인 라벨 색상 변경
            dChart_L_1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_3.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_4.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            dChart_L_5.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;

            //y 라인 라벨 색상 변경
            dChart_L_1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_3.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_4.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            dChart_L_5.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            //x 라인 격자로 변경
            dChart_L_1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_3.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_4.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_5.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //y 라인 격자로 변경
            dChart_L_1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_3.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_4.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            dChart_L_5.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //라인 색상 변경
            dChart_L_1.Series[0].Color = Color.Crimson;
            dChart_L_2.Series[0].Color = Color.Crimson;
            dChart_L_3.Series[0].Color = Color.Crimson;
            dChart_L_4.Series[0].Color = Color.Crimson;
            dChart_L_5.Series[0].Color = Color.Crimson;

            //라인 두깨 변경
            dChart_L_1.Series[0].BorderWidth = 2;
            dChart_L_2.Series[0].BorderWidth = 2;
            dChart_L_3.Series[0].BorderWidth = 2;
            dChart_L_4.Series[0].BorderWidth = 2;
            dChart_L_5.Series[0].BorderWidth = 2;

            //제목 배경 색상 변경
            dChart_L_1.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_2.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_3.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_4.Legends[0].BackColor = Color.FromArgb(45, 45, 45);
            dChart_L_5.Legends[0].BackColor = Color.FromArgb(45, 45, 45);

            dChart_L_1.Legends[0].ForeColor = Color.White;
            dChart_L_2.Legends[0].ForeColor = Color.White;
            dChart_L_3.Legends[0].ForeColor = Color.White;
            dChart_L_4.Legends[0].ForeColor = Color.White;
            dChart_L_5.Legends[0].ForeColor = Color.White;
        }
        #endregion

        #region method - Chart
        void DrawChart_L()
        {
            dChart_L_1.Series[0].Points.Clear();
            dChart_L_2.Series[0].Points.Clear();
            dChart_L_3.Series[0].Points.Clear();
            dChart_L_4.Series[0].Points.Clear();
            dChart_L_5.Series[0].Points.Clear();

            List<int> roi1 = new List<int>();
            List<int> roi2 = new List<int>();
            List<int> roi3 = new List<int>();
            List<int> roi4 = new List<int>();
            List<int> roi5 = new List<int>();

            List<int> xAxis = new List<int>();

            for (int i = 0; i < _Data_1_L.Count; i++)
            {
                xAxis.Add(i);

                roi1.Add(_Data_1_L[i].Count * 30);
                roi2.Add(_Data_2_L[i].Count * 30);
                roi3.Add(_Data_3_L[i].Count * 30);
                roi4.Add(_Data_4_L[i].Count * 30);
                roi5.Add(_Data_5_L[i].Count * 30);
            }

            dChart_L_1.Series[0].Points.DataBindXY(xAxis, roi1);
            dChart_L_2.Series[0].Points.DataBindXY(xAxis, roi2);
            dChart_L_3.Series[0].Points.DataBindXY(xAxis, roi3);
            dChart_L_4.Series[0].Points.DataBindXY(xAxis, roi4);
            dChart_L_5.Series[0].Points.DataBindXY(xAxis, roi5);
        }
        #endregion

        #region 잡동사니
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion


    }
}