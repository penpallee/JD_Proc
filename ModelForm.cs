using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JD_Proc
{
    public partial class ModelForm : Form
    {
        #region 생성자
        Form1 _form1;

        public ModelForm(Form1 form1)
        {
            InitializeComponent();

            _form1 = form1;

            dListView_model.GridLines = true;
            dListView_model.FullRowSelect = true;

            dListView_model.Columns.Add("name", 200, HorizontalAlignment.Center);
            dListView_model.Columns.Add("생성일", 200, HorizontalAlignment.Center);

            Reflash_ListView();
        }
        #endregion

        #region Event(dListView_model) - Index changed 
        private void dListView_model_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dListView_model.SelectedItems.Count != 0)
            {
                int SelectRow = dListView_model.SelectedItems[0].Index;
                string name = dListView_model.Items[SelectRow].SubItems[0].Text;

                Service.ModelsService service = new Service.ModelsService();
                string path = "C:\\JD\\model\\" + name + ".ini";

                dTbox_name.TextButton = service.ReadOne("name", "name", path);
                dTbox_date.TextButton = service.ReadOne("date", "date", path);

                //LEFT
                dNum_x_L.Value = int.Parse(service.ReadOne("roi_L", "x", path));
                dNum_y_L.Value = int.Parse(service.ReadOne("roi_L", "y", path));
                dNum_w_L.Value = int.Parse(service.ReadOne("roi_L", "w", path));
                dNum_h_L.Value = int.Parse(service.ReadOne("roi_L", "h", path));
                dNum_gap_L.Value = int.Parse(service.ReadOne("roi_L", "gap", path));
                dNum_gv_L.Text = service.ReadOne("roi_L", "gridview", path);
                dNum_GY_L.Text = service.ReadOne("roi_L", "gridview_y", path);

                //RIGHT
                dNum_x_R.Value = int.Parse(service.ReadOne("roi_R", "x", path));
                dNum_y_R.Value = int.Parse(service.ReadOne("roi_R", "y", path));
                dNum_w_R.Value = int.Parse(service.ReadOne("roi_R", "w", path));
                dNum_h_R.Value = int.Parse(service.ReadOne("roi_R", "h", path));
                dNum_gap_R.Value = int.Parse(service.ReadOne("roi_R", "gap", path));
                dNum_gv_R.Text = service.ReadOne("roi_R", "gridview", path);
                dNum_GY_R.Text = service.ReadOne("roi_R", "gridview_y", path);
            }
        }
        #endregion

        #region Event(dBtn_new) - click
        private void dBtn_new_Click(object sender, EventArgs e)
        {
            string isNullStr_model = dTbox_name.TextButton.Replace(" ", string.Empty);

            if (RegxChar(dTbox_name.TextButton) == 0)
            {
                dTbox_name.TextButton = "특수문자를 제거하세요.\\ / : * ? \" < > | ";
                return;
            }
            else if (isNullStr_model == string.Empty)
            {
                dTbox_name.TextButton = "name을 입력해주세요.";
                return;
            }
            else
            {
                Service.ModelsService service = new Service.ModelsService();
                Model.Models model = new Model.Models();

                model.Name = dTbox_name.TextButton;
                model.Date = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");

                //LEFT
                model.X_1_L = (int)dNum_x_L.Value;
                model.Y_1_L = (int)dNum_y_L.Value;
                model.Width_L = (int)dNum_w_L.Value;
                model.Height_L = (int)dNum_h_L.Value;
                model.Gap_L = (int)dNum_gap_L.Value;
                model.Grid_L = int.Parse(dNum_gv_L.Text);
                model.Grid_Y_L = int.Parse(dNum_GY_L.Text);

                //RIGHT
                model.X_1_R = (int)dNum_x_R.Value;
                model.Y_1_R = (int)dNum_y_R.Value;
                model.Width_R = (int)dNum_w_R.Value;
                model.Height_R = (int)dNum_h_R.Value;
                model.Gap_R = (int)dNum_gap_R.Value;
                model.Grid_R = int.Parse(dNum_gv_R.Text);
                model.Grid_Y_R = int.Parse(dNum_GY_R.Text);

                service.WriteAll(model);
            }

            Reflash_ListView();
        }
        #endregion

        #region Event(dBtn_open) - click
        private void dBtn_open_Click(object sender, EventArgs e)
        {
            string name = dTbox_name.TextButton;
            string path = "C:\\JD\\model\\" + name + ".ini";

            string isNullStr_model = dTbox_name.TextButton.Replace(" ", string.Empty);
            if (isNullStr_model == string.Empty)
            {
                dTbox_name.TextButton = "name을 입력해주세요.";
                return;
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists) _form1.ModelChanged(path);
                else
                    dTbox_name.TextButton = "존재하지 않는 파일 입니다";
            }
        }
        #endregion

        #region method - 리스트뷰 데이터 로딩
        void Reflash_ListView()
        {
            dListView_model.BeginUpdate();
            dListView_model.Items.Clear();

            Service.ModelsService service = new Service.ModelsService();

            string folderPath = "C:\\JD\\model\\";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);

            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".ini") == 0)
                {
                    //string FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                    string FullFileName = File.FullName;

                    string name = service.ReadOne("name", "name", FullFileName);
                    string date = service.ReadOne("date", "date", FullFileName);

                    ListViewItem item = new ListViewItem(new string[] { name, date });
                    dListView_model.Items.Add(item);
                }
            }

            dListView_model.EndUpdate();
        }
        #endregion

        #region 폴더명 문자 검증
        private int RegxChar(string txt)
        {
            Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
            string result_regex = regex.Replace(txt, "");

            if (txt.Length != result_regex.Length)
                return 0;

            return 1;
        }
        #endregion

        #region Event(delete) - click
        private void dBtn_delete_Click(object sender, EventArgs e)
        {
            string isNullStr_model = dTbox_name.TextButton.Replace(" ", string.Empty);

            if (RegxChar(dTbox_name.TextButton) == 0)
            {
                dTbox_name.TextButton = "특수문자를 제거하세요.\\ / : * ? \" < > | ";
                return;
            }
            else if (isNullStr_model == string.Empty)
            {
                dTbox_name.TextButton = "name을 입력해주세요.";
                return;
            }
            else
            {
                Service.ModelsService service = new Service.ModelsService();
                service.Delete(dTbox_name.TextButton);
            }

            Reflash_ListView();
        }
        #endregion
    }
}
