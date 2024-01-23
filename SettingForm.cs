using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JD_Proc
{
    public partial class SettingForm : Form
    {
        #region 생성자
        public SettingForm()
        {
            InitializeComponent();

            GetResolution_1();
            GetResolution_2();
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




    }
}
