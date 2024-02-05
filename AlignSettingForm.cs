using JD_Proc.Service;

namespace JD_Proc
{
    public partial class AlignSettingForm : Form
    {
        Form1 _form1;
        PictureBox _picbox1;
        PictureBox _picbox2;

        public AlignSettingForm(Form1 form1, PictureBox picbox1, PictureBox picbox2)
        {
            InitializeComponent();

            _form1 = form1;
            _picbox1 = picbox1;
            _picbox2 = picbox2;
        }

        private void AlignSettingForm_Load(object sender, EventArgs e)
        {
            Draw_Gridview_L();
            //Draw_Gridview_R();
            pictureBox1.Image = _picbox1.Image;
        }

        #region method - Grid view
        public void Draw_Gridview_L()
        {
            dPan_grid_L_1.Visible = true;
            dPan_grid_L_2.Visible = true;
            dPan_grid_L_3.Visible = true;
            dPan_grid_L_4.Visible = true;
            dPan_grid_L_5.Visible = true;
        }

        public void Draw_Gridview_R()
        {
            dPan_grid_R_1.Visible = true;
            dPan_grid_R_2.Visible = true;
            dPan_grid_R_3.Visible = true;
            dPan_grid_R_4.Visible = true;
            dPan_grid_R_5.Visible = true;
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


            dPan_grid_R_1.Top = pictureBox1.Top + JD_Proc.Form1._Model.Grid_Y_R;
            dPan_grid_R_2.Top = dPan_grid_R_1.Bottom + JD_Proc.Form1._Model.Grid_pixel_R + panelBorder;
            dPan_grid_R_3.Top = dPan_grid_R_2.Bottom + JD_Proc.Form1._Model.Grid_pixel_R + panelBorder;
            dPan_grid_R_4.Top = dPan_grid_R_3.Bottom + JD_Proc.Form1._Model.Grid_pixel_R + panelBorder;
            dPan_grid_R_5.Top = dPan_grid_R_4.Bottom + JD_Proc.Form1._Model.Grid_pixel_R + panelBorder;


        }
        #endregion
    }
}
