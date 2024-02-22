using System.Windows.Forms.DataVisualization.Charting;

namespace JD_Proc
{
    partial class TempGraphForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            DataPoint dataPoint1 = new DataPoint(0D, 23D);
            DataPoint dataPoint2 = new DataPoint(1D, 28D);
            DataPoint dataPoint3 = new DataPoint(2D, 25D);
            DataPoint dataPoint4 = new DataPoint(3D, 22D);
            DataPoint dataPoint5 = new DataPoint(4D, 20D);
            tableLayoutPanel1 = new TableLayoutPanel();
            TempGraph = new Chart();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TempGraph).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(TempGraph, 0, 1);
            tableLayoutPanel1.Controls.Add(bigLabel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            tableLayoutPanel1.Size = new Size(1068, 365);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // TempGraph
            // 
            TempGraph.BackColor = Color.FromArgb(0, 0, 20);
            TempGraph.BackSecondaryColor = SystemColors.Desktop;
            TempGraph.BorderlineColor = Color.Transparent;
            chartArea1.AxisX.LabelStyle.ForeColor = Color.FromArgb(0, 0, 20);
            chartArea1.AxisX.LineColor = Color.FromArgb(0, 0, 20);
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.LabelStyle.ForeColor = Color.FromArgb(0, 0, 20);
            chartArea1.AxisY.LineColor = Color.FromArgb(0, 0, 20);
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.BackColor = Color.FromArgb(0, 0, 20);
            chartArea1.Name = "ChartArea1";
            TempGraph.ChartAreas.Add(chartArea1);
            TempGraph.Dock = DockStyle.Fill;
            legend1.BackColor = Color.Transparent;
            legend1.ForeColor = Color.White;
            legend1.Name = "Legend1";
            TempGraph.Legends.Add(legend1);
            TempGraph.Location = new Point(3, 57);
            TempGraph.Name = "TempGraph";
            TempGraph.Palette = ChartColorPalette.None;
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = SeriesChartType.Line;
            series1.Color = Color.Lime;
            series1.IsValueShownAsLabel = true;
            series1.LabelForeColor = Color.White;
            series1.Legend = "Legend1";
            series1.MarkerColor = Color.Lime;
            series1.MarkerSize = 10;
            series1.MarkerStyle = MarkerStyle.Circle;
            series1.Name = "Temperature Graph";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            TempGraph.Series.Add(series1);
            TempGraph.Size = new Size(1062, 305);
            TempGraph.TabIndex = 0;
            TempGraph.Text = "Highest Temperature";
            // 
            // bigLabel1
            // 
            bigLabel1.Anchor = AnchorStyles.None;
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(384, 4);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(300, 45);
            bigLabel1.TabIndex = 1;
            bigLabel1.Text = "Temerature Graph";
            // 
            // TempGraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1068, 365);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MinimumSize = new Size(203, 49);
            Name = "TempGraphForm";
            Text = "Gap Temperature Graph";
            TransparencyKey = Color.Fuchsia;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TempGraph).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart TempGraph;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
    }
}