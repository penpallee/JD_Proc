using JD_Proc.Model;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc.Service
{
    public class ModelsService
    {
        string _path = "C:\\JD\\model\\";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public void WriteAll(Model.Models models)
        {
            string path = _path + models.Name + ".ini";

            WritePrivateProfileString("name", "name", models.Name, path);
            WritePrivateProfileString("date", "date", models.Date, path);
            
            // LEFT
            WritePrivateProfileString("roi_L", "x", models.X_1_L.ToString(), path);
            WritePrivateProfileString("roi_L", "y", models.Y_1_L.ToString(), path);
            WritePrivateProfileString("roi_L", "w", models.Width_L.ToString(), path);
            WritePrivateProfileString("roi_L", "h", models.Height_L.ToString(), path);
            WritePrivateProfileString("roi_L", "gap", models.Gap_L.ToString(), path);
            WritePrivateProfileString("roi_L", "gridview", models.Grid_L.ToString(), path);
            WritePrivateProfileString("roi_L", "gridview_y", models.Grid_Y_L.ToString(), path);

            // RIGHT
            WritePrivateProfileString("roi_R", "x", models.X_1_R.ToString(), path);
            WritePrivateProfileString("roi_R", "y", models.Y_1_R.ToString(), path);
            WritePrivateProfileString("roi_R", "w", models.Width_R.ToString(), path);
            WritePrivateProfileString("roi_R", "h", models.Height_R.ToString(), path);
            WritePrivateProfileString("roi_R", "gap", models.Gap_R.ToString(), path);
            WritePrivateProfileString("roi_R", "gridview", models.Grid_R.ToString(), path);
            WritePrivateProfileString("roi_R", "gridview_y", models.Grid_Y_R.ToString(), path);
        }

        public string ReadOne(string Section, string Key, string path)
        {
            StringBuilder temp = new StringBuilder(256);
            int i = GetPrivateProfileString(Section, Key, "", temp, 256, path);
            if (temp != null && temp.Length > 0) return temp.ToString();
            else return "null";
        }

        public Model.Models SetModels(string path)
        {
            Model.Models model = new Model.Models();

            model.Name = ReadOne("name", "name", path);
            model.Date = ReadOne("date", "date", path);

            //LEFT
            model.X_1_L = int.Parse(ReadOne("roi_L", "x", path));
            model.Y_1_L = int.Parse(ReadOne("roi_L", "y", path));
            model.Width_L = int.Parse(ReadOne("roi_L", "w", path));
            model.Height_L = int.Parse(ReadOne("roi_L", "h", path));
            model.Gap_L = int.Parse(ReadOne("roi_L", "gap", path));
            model.Grid_L = int.Parse(ReadOne("roi_L", "gridview", path));
            model.Grid_Y_L = int.Parse(ReadOne("roi_L", "gridview_y", path));

            model.X_2_L = model.X_1_L + model.Width_L + model.Gap_L;
            model.Y_2_L = model.Y_1_L;

            model.X_3_L = model.X_2_L + model.Width_L + model.Gap_L;
            model.Y_3_L = model.Y_2_L;

            model.X_4_L = model.X_3_L + model.Width_L + model.Gap_L;
            model.Y_4_L = model.Y_3_L;

            model.X_5_L = model.X_4_L + model.Width_L + model.Gap_L;
            model.Y_5_L = model.Y_4_L;

            //RIGHT
            model.X_1_R = int.Parse(ReadOne("roi_R", "x", path));
            model.Y_1_R = int.Parse(ReadOne("roi_R", "y", path));
            model.Width_R = int.Parse(ReadOne("roi_R", "w", path));
            model.Height_R = int.Parse(ReadOne("roi_R", "h", path));
            model.Gap_R = int.Parse(ReadOne("roi_R", "gap", path));
            model.Grid_R = int.Parse(ReadOne("roi_R", "gridview", path));
            model.Grid_Y_R = int.Parse(ReadOne("roi_R", "gridview_y", path));

            model.X_2_R = model.X_1_R + model.Width_R + model.Gap_R;
            model.Y_2_R = model.Y_1_R;

            model.X_3_R = model.X_2_R + model.Width_R + model.Gap_R;
            model.Y_3_R = model.Y_2_R;

            model.X_4_R = model.X_3_R + model.Width_R + model.Gap_R;
            model.Y_4_R = model.Y_3_R;

            model.X_5_R = model.X_4_R + model.Width_R + model.Gap_R;
            model.Y_5_R = model.Y_4_R;

            //grid view 셋팅
            Service.SettingsService service = new Service.SettingsService();

            double resolu_x1 = double.Parse(service.Read("resolution", "x_1"));
            double resolu_y1 = double.Parse(service.Read("resolution", "y_1"));

            double resolu_x2 = double.Parse(service.Read("resolution", "x_2"));
            double resolu_y2 = double.Parse(service.Read("resolution", "y_2"));

            double grid_gap_L = (double)model.Grid_L;
            double grid_gap_R = (double)model.Grid_R;

            double dev1 = grid_gap_L / resolu_y1;
            double dev2 = grid_gap_R / resolu_y2;

            model.Grid_pixel_L = (int)Math.Round(dev1);
            model.Grid_pixel_R = (int)Math.Round(dev2);

            return model;
        }

        public void Delete(string name)
        {
            string path = _path + name + ".ini";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
