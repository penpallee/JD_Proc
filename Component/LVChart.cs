using LVChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace JD_Proc.Component
{
    public class LVChart
    {
        public Lvchart _Lvchart;
        public string AutoCSVPath;

        public LVChart(Panel pan, Label Lb_Max, Label Lb_Min, string CSVPath) {
            _Lvchart = new Lvchart(pan, Lb_Max, Lb_Min);

            _Lvchart.FontSize = 9;

            // Load CSV File to Graph
            AutoCSVPath = CSVPath;
            if (AutoCSVPath.Length > 0)
            {

                _Lvchart.GetData(AutoCSVPath, 640, 480); // CSV 경로 및 이미지 사이즈
            }

            _Lvchart.GraphicRow(_Lvchart.RowValue); //Graph 표시
        }


        //private void btn_LoadCsv_Click(object sender, EventArgs e)
        //{
        //    string tmpStr = AutoCSVPath;
        //    if (tmpStr.Length > 0)
        //    {

        //        _Lvchart.GetData(tmpStr, 640, 480); // CSV 경로 및 이미지 사이즈
        //    }
        //}

        //public string LoadCSVPath()
        //{
        //    string filename = string.Empty;
        //    using (OpenFileDialog open = new OpenFileDialog())
        //    {
        //        open.Filter = "All files (*.csv)|*.csv|All files (*.*)|*.*";
        //        open.RestoreDirectory = true;
        //        DialogResult result = open.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            filename = open.FileName;
        //        }
        //    }
        //    return filename;
        //}

        //private void num_Row_ValueChanged(object sender, EventArgs e)
        //{
        //    int Value = (int)num_Row.Value - 1;
        //    if (Value < 0)
        //        Value = 0;
        //    if (Value >= (int)num_Row.Maximum)
        //        Value = (int)num_Row.Maximum - 1;
        //    lvchart.RowValue = Value;
        //    lvchart.GraphicRow(lvchart.RowValue);//Graph 표시
        //}

        //private void btnGraphShow_Click(object sender, EventArgs e)
        //{
        //    lvchart.GraphicRow(lvchart.RowValue); //Graph 표시
        //}

        //private void btn_AxisReset_Click(object sender, EventArgs e)
        //{
        //    lvchart.AxisXchangeRest(); //특정 구간에서 전체로 원복 한 경우
        //}

        //private void btn_AxisChange_Click(object sender, EventArgs e)
        //{
        //    lvchart.AxisXchangeShow(); // 특정 구간 불러오기
        //}
        //private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //{
        //    lvchart.startX = (int)num_AxisStartX.Value;  //특정 구간의 시작X 축값
        //}
        //private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        //{
        //    lvchart.EndX = (int)num_AxisEndX.Value;      //특정 구간의 시작Y 축값
        //}

    }
}
