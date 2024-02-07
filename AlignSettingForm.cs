﻿using JD_Proc.Service;
using System.Diagnostics;

namespace JD_Proc
{
    public partial class AlignSettingForm : Form
    {
        Service.SettingsService service = new Service.SettingsService();

        //dTbox_x1.Text = service.Read("resolution", "x_1");
        //    dTbox_y1.Text = service.Read("resolution", "y_1");
        Form1 _form1;
        PictureBox _picbox1;
        PictureBox _picbox2;

        int GridViewGap = 6;
        int res;
        

        public AlignSettingForm(Form1 form1, PictureBox picbox1, PictureBox picbox2)
        {
            InitializeComponent();
            res = int.Parse(service.Read("resolution", "y_1"));

            _form1 = form1;
            _picbox1 = picbox1;
            _picbox2 = picbox2;
        }

        #region event(Foam_Load)
        private void AlignSettingForm_Load(object sender, EventArgs e)
        {
            Draw_Gridview_L();
            pictureBox1.Image = _picbox1.Image;
            Tbox_GridViewValue.Text = (GridViewGap * res).ToString();
        }
        #endregion

        #region method - Grid view
        public void Draw_Gridview_L()
        {
            dPan_grid_L_1.Location = new Point(dPan_grid_L_1.Parent.Location.X - 7, dPan_grid_L_1.Parent.Location.Y);
            dPan_grid_L_2.Location = new Point(dPan_grid_L_1.Parent.Location.X - 7, dPan_grid_L_1.Parent.Location.Y + GridViewGap * 1);
            dPan_grid_L_3.Location = new Point(dPan_grid_L_1.Parent.Location.X - 7, dPan_grid_L_1.Parent.Location.Y + GridViewGap * 2);
            dPan_grid_L_4.Location = new Point(dPan_grid_L_1.Parent.Location.X - 7, dPan_grid_L_1.Parent.Location.Y + GridViewGap * 3);
            dPan_grid_L_5.Location = new Point(dPan_grid_L_1.Parent.Location.X - 7, dPan_grid_L_1.Parent.Location.Y + GridViewGap * 4);
            dPan_grid_L_1.Visible = true;
            dPan_grid_L_2.Visible = true;
            dPan_grid_L_3.Visible = true;
            dPan_grid_L_4.Visible = true;
            dPan_grid_L_5.Visible = true;
        }
        #endregion

        #region method - set Model
        void SetModel(string path)
        {
            int panelBorder = 2;
            ModelsService service = new ModelsService();
            JD_Proc.Form1._Model = service.SetModels(path);

            //LEFT

            dPan_grid_L_1.Top = pictureBox1.Top + JD_Proc.Form1._Model.Grid_Y_L;
            dPan_grid_L_2.Top = dPan_grid_L_1.Bottom + JD_Proc.Form1._Model.Grid_pixel_L + panelBorder;
            dPan_grid_L_3.Top = dPan_grid_L_2.Bottom + JD_Proc.Form1._Model.Grid_pixel_L + panelBorder;
            dPan_grid_L_4.Top = dPan_grid_L_3.Bottom + JD_Proc.Form1._Model.Grid_pixel_L + panelBorder;
            dPan_grid_L_5.Top = dPan_grid_L_4.Bottom + JD_Proc.Form1._Model.Grid_pixel_L + panelBorder;


        }
        #endregion

        #region event(VisionRulerButton) - Click
        private void dBtn_up_L_Click_1(object sender, EventArgs e)
        {
            dPan_grid_L_1.Location = new Point(dPan_grid_L_1.Location.X, dPan_grid_L_1.Location.Y - 1);
            dPan_grid_L_2.Location = new Point(dPan_grid_L_2.Location.X, dPan_grid_L_2.Location.Y - 1);
            dPan_grid_L_3.Location = new Point(dPan_grid_L_3.Location.X, dPan_grid_L_3.Location.Y - 1);
            dPan_grid_L_4.Location = new Point(dPan_grid_L_4.Location.X, dPan_grid_L_4.Location.Y - 1);
            dPan_grid_L_5.Location = new Point(dPan_grid_L_5.Location.X, dPan_grid_L_5.Location.Y - 1);
        }

        private void dBtn_dw_L_Click_1(object sender, EventArgs e)
        {
            dPan_grid_L_1.Location = new Point(dPan_grid_L_1.Location.X, dPan_grid_L_1.Location.Y + 1);
            dPan_grid_L_2.Location = new Point(dPan_grid_L_2.Location.X, dPan_grid_L_2.Location.Y + 1);
            dPan_grid_L_3.Location = new Point(dPan_grid_L_3.Location.X, dPan_grid_L_3.Location.Y + 1);
            dPan_grid_L_4.Location = new Point(dPan_grid_L_4.Location.X, dPan_grid_L_4.Location.Y + 1);
            dPan_grid_L_5.Location = new Point(dPan_grid_L_5.Location.X, dPan_grid_L_5.Location.Y + 1);
        }
        #endregion

        #region event(SelectViewButton) - Click
        private void Btn_SelectCam1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != _picbox1.Image)
                pictureBox1.Image = _picbox1.Image;
        }

        private void Btn_SelectCam2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != _picbox2.Image)
                pictureBox1.Image = _picbox2.Image;
        }
        #endregion

        #region event(GridViewCheckButton) - Click
        private void Btn_GridView_Check_Click(object sender, EventArgs e)
        {
            
            GridViewGap = (int.Parse(Tbox_GridViewValue.Text) / res);
            Draw_Gridview_L();
        }
        #endregion

        private void Btn_JogMoveValue_Check_Click(object sender, EventArgs e)
        {

        }
    }
}