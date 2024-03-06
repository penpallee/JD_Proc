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
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button6 = new Button();
            button8 = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(button8, 1, 4);
            tableLayoutPanel1.Controls.Add(button6, 1, 3);
            tableLayoutPanel1.Controls.Add(button4, 1, 2);
            tableLayoutPanel1.Controls.Add(button3, 0, 2);
            tableLayoutPanel1.Controls.Add(button2, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 1, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 1);
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
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(3, 93);
            button1.Name = "button1";
            button1.Size = new Size(394, 84);
            button1.TabIndex = 2;
            button1.Text = "PLC_AUTO";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(403, 93);
            button2.Name = "button2";
            button2.Size = new Size(394, 84);
            button2.TabIndex = 3;
            button2.Text = "VISION_AUTO";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(3, 183);
            button3.Name = "button3";
            button3.Size = new Size(394, 84);
            button3.TabIndex = 4;
            button3.Text = "PLC_START_L";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(403, 183);
            button4.Name = "button4";
            button4.Size = new Size(394, 84);
            button4.TabIndex = 5;
            button4.Text = "VISION_READY";
            button4.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Dock = DockStyle.Fill;
            button6.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button6.Location = new Point(403, 273);
            button6.Name = "button6";
            button6.Size = new Size(394, 84);
            button6.TabIndex = 7;
            button6.Text = "VISION_BUSY";
            button6.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Dock = DockStyle.Fill;
            button8.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button8.Location = new Point(403, 363);
            button8.Name = "button8";
            button8.Size = new Size(394, 84);
            button8.TabIndex = 9;
            button8.Text = "VISION_END";
            button8.UseVisualStyleBackColor = true;
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
        private Button button8;
        private Button button6;
        private Button button4;
        private Button button3;
        private Button button2;
        private Label label1;
        private Label label2;
        private Button button1;
    }
}