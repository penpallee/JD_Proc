using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JD_Proc
{
    public partial class AutoSimulation : Form
    {
        public Form1 _form1;
        public AutoSimulation(Form1 form)
        {
            InitializeComponent();
            _form1 = form;
        }

        private void Btn_PLC_Auto_Click(object sender, EventArgs e)
        {
            if (_form1.AT_PLC_AUTO == false)
            {
                Btn_PLC_Auto.BackColor = Color.Aquamarine;
                _form1.AT_PLC_AUTO = true;
            }
            else
            {
                Btn_PLC_Auto.BackColor= SystemColors.Control;
                _form1.AT_PLC_AUTO = false;
            }
        }

        private void Btn_PLC_StartL_Click(object sender, EventArgs e)
        {
            _form1.AT_PLC_START = true;
        }

        private void Btn_VISION_Auto_Click(object sender, EventArgs e)
        {

        }

        private void Btn_VISION_Ready_Click(object sender, EventArgs e)
        {

        }

        private void Btn_VISION_Busy_Click(object sender, EventArgs e)
        {

        }

        private void Btn_VISION_End_Click(object sender, EventArgs e)
        {

        }
    }
}
