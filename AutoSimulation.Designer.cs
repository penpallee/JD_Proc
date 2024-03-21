namespace JD_Proc
{
    partial class AutoSimulation
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
            tableLayoutPanel1 = new TableLayoutPanel();
            Btn_VISION_End = new Button();
            Btn_VISION_Busy = new Button();
            Btn_VISION_Ready = new Button();
            Btn_PLC_StartL = new Button();
            Btn_VISION_Auto = new Button();
            label1 = new Label();
            label2 = new Label();
            Btn_PLC_Auto = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(Btn_VISION_End, 1, 4);
            tableLayoutPanel1.Controls.Add(Btn_VISION_Busy, 1, 3);
            tableLayoutPanel1.Controls.Add(Btn_VISION_Ready, 1, 2);
            tableLayoutPanel1.Controls.Add(Btn_PLC_StartL, 0, 2);
            tableLayoutPanel1.Controls.Add(Btn_VISION_Auto, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 1, 0);
            tableLayoutPanel1.Controls.Add(Btn_PLC_Auto, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // Btn_VISION_End
            // 
            Btn_VISION_End.Dock = DockStyle.Fill;
            Btn_VISION_End.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_VISION_End.Location = new Point(403, 363);
            Btn_VISION_End.Name = "Btn_VISION_End";
            Btn_VISION_End.Size = new Size(394, 84);
            Btn_VISION_End.TabIndex = 9;
            Btn_VISION_End.Text = "VISION_END";
            Btn_VISION_End.UseVisualStyleBackColor = true;
            Btn_VISION_End.Click += Btn_VISION_End_Click;
            // 
            // Btn_VISION_Busy
            // 
            Btn_VISION_Busy.Dock = DockStyle.Fill;
            Btn_VISION_Busy.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_VISION_Busy.Location = new Point(403, 273);
            Btn_VISION_Busy.Name = "Btn_VISION_Busy";
            Btn_VISION_Busy.Size = new Size(394, 84);
            Btn_VISION_Busy.TabIndex = 7;
            Btn_VISION_Busy.Text = "VISION_BUSY";
            Btn_VISION_Busy.UseVisualStyleBackColor = true;
            Btn_VISION_Busy.Click += Btn_VISION_Busy_Click;
            // 
            // Btn_VISION_Ready
            // 
            Btn_VISION_Ready.Dock = DockStyle.Fill;
            Btn_VISION_Ready.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_VISION_Ready.Location = new Point(403, 183);
            Btn_VISION_Ready.Name = "Btn_VISION_Ready";
            Btn_VISION_Ready.Size = new Size(394, 84);
            Btn_VISION_Ready.TabIndex = 5;
            Btn_VISION_Ready.Text = "VISION_READY";
            Btn_VISION_Ready.UseVisualStyleBackColor = true;
            Btn_VISION_Ready.Click += Btn_VISION_Ready_Click;
            // 
            // Btn_PLC_StartL
            // 
            Btn_PLC_StartL.Dock = DockStyle.Fill;
            Btn_PLC_StartL.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_PLC_StartL.Location = new Point(3, 183);
            Btn_PLC_StartL.Name = "Btn_PLC_StartL";
            Btn_PLC_StartL.Size = new Size(394, 84);
            Btn_PLC_StartL.TabIndex = 4;
            Btn_PLC_StartL.Text = "PLC_START_L";
            Btn_PLC_StartL.UseVisualStyleBackColor = true;
            Btn_PLC_StartL.Click += Btn_PLC_StartL_Click;
            // 
            // Btn_VISION_Auto
            // 
            Btn_VISION_Auto.Dock = DockStyle.Fill;
            Btn_VISION_Auto.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_VISION_Auto.Location = new Point(403, 93);
            Btn_VISION_Auto.Name = "Btn_VISION_Auto";
            Btn_VISION_Auto.Size = new Size(394, 84);
            Btn_VISION_Auto.TabIndex = 3;
            Btn_VISION_Auto.Text = "VISION_AUTO";
            Btn_VISION_Auto.UseVisualStyleBackColor = true;
            Btn_VISION_Auto.Click += Btn_VISION_Auto_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(394, 90);
            label1.TabIndex = 0;
            label1.Text = "PLC_SIGNAL";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("맑은 고딕", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(403, 0);
            label2.Name = "label2";
            label2.Size = new Size(394, 90);
            label2.TabIndex = 1;
            label2.Text = "VISION_SIGNAL";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Btn_PLC_Auto
            // 
            Btn_PLC_Auto.Dock = DockStyle.Fill;
            Btn_PLC_Auto.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_PLC_Auto.Location = new Point(3, 93);
            Btn_PLC_Auto.Name = "Btn_PLC_Auto";
            Btn_PLC_Auto.Size = new Size(394, 84);
            Btn_PLC_Auto.TabIndex = 2;
            Btn_PLC_Auto.Text = "PLC_AUTO";
            Btn_PLC_Auto.UseVisualStyleBackColor = true;
            Btn_PLC_Auto.Click += Btn_PLC_Auto_Click;
            // 
            // AutoSimulation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "AutoSimulation";
            Text = "AutoSimulation";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button Btn_VISION_End;
        private Button Btn_VISION_Busy;
        private Button Btn_VISION_Ready;
        private Button Btn_PLC_StartL;
        private Button Btn_VISION_Auto;
        private Label label1;
        private Label label2;
        private Button Btn_PLC_Auto;
    }
}