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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempGraphForm));
            dungeonForm1 = new ReaLTaiizor.Forms.DungeonForm();
            parrotLineGraph1 = new ReaLTaiizor.Controls.ParrotLineGraph();
            dungeonForm1.SuspendLayout();
            SuspendLayout();
            // 
            // dungeonForm1
            // 
            dungeonForm1.BackColor = Color.FromArgb(244, 241, 243);
            dungeonForm1.BorderColor = Color.FromArgb(38, 38, 38);
            dungeonForm1.Controls.Add(parrotLineGraph1);
            dungeonForm1.Dock = DockStyle.Fill;
            dungeonForm1.FillEdgeColorA = Color.FromArgb(69, 68, 63);
            dungeonForm1.FillEdgeColorB = Color.FromArgb(69, 68, 63);
            dungeonForm1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dungeonForm1.FooterEdgeColor = Color.FromArgb(69, 68, 63);
            dungeonForm1.ForeColor = Color.FromArgb(223, 219, 210);
            dungeonForm1.HeaderEdgeColorA = Color.FromArgb(87, 85, 77);
            dungeonForm1.HeaderEdgeColorB = Color.FromArgb(69, 68, 63);
            dungeonForm1.Location = new Point(0, 0);
            dungeonForm1.Margin = new Padding(2);
            dungeonForm1.Name = "dungeonForm1";
            dungeonForm1.Padding = new Padding(16, 42, 16, 12);
            dungeonForm1.RoundCorners = true;
            dungeonForm1.Sizable = true;
            dungeonForm1.Size = new Size(1066, 219);
            dungeonForm1.SmartBounds = true;
            dungeonForm1.StartPosition = FormStartPosition.WindowsDefaultLocation;
            dungeonForm1.TabIndex = 0;
            dungeonForm1.Text = "Gap Temperature Graph";
            dungeonForm1.TitleColor = Color.FromArgb(223, 219, 210);
            // 
            // parrotLineGraph1
            // 
            parrotLineGraph1.BackGroundColor = Color.Black;
            parrotLineGraph1.BelowLineColor = Color.AliceBlue;
            parrotLineGraph1.BorderColor = Color.White;
            parrotLineGraph1.GraphStyle = ReaLTaiizor.Controls.ParrotLineGraph.Style.Curved;
            parrotLineGraph1.GraphTitle = "Parrot Line Graph";
            parrotLineGraph1.GraphTitleColor = Color.Gray;
            parrotLineGraph1.Items = (List<int>)resources.GetObject("parrotLineGraph1.Items");
            parrotLineGraph1.LineColor = Color.Lime;
            parrotLineGraph1.Location = new Point(0, 34);
            parrotLineGraph1.Margin = new Padding(2);
            parrotLineGraph1.Name = "parrotLineGraph1";
            parrotLineGraph1.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            parrotLineGraph1.PointSize = 0;
            parrotLineGraph1.ShowBorder = false;
            parrotLineGraph1.ShowPoints = true;
            parrotLineGraph1.ShowTitle = false;
            parrotLineGraph1.ShowVerticalLines = true;
            parrotLineGraph1.Size = new Size(1066, 184);
            parrotLineGraph1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            parrotLineGraph1.TabIndex = 0;
            parrotLineGraph1.Text = "parrotLineGraph1";
            parrotLineGraph1.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            parrotLineGraph1.TitleAlignment = StringAlignment.Center;
            parrotLineGraph1.VerticalLineColor = Color.DimGray;
            // 
            // TempGraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1066, 219);
            Controls.Add(dungeonForm1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MinimumSize = new Size(203, 49);
            Name = "TempGraphForm";
            Text = "Gap Temperature Graph";
            TransparencyKey = Color.Fuchsia;
            dungeonForm1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.DungeonForm dungeonForm1;
        private ReaLTaiizor.Controls.ParrotLineGraph parrotLineGraph1;
    }
}