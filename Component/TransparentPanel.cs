using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc.Component
{
    public class TransparentPanel : Panel

    {

        protected override CreateParams CreateParams

        {

            get

            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;

            }

        }



        protected override void OnPaintBackground(PaintEventArgs pevent)

        {

        }



        protected override void OnResize(EventArgs eventargs)

        {

            if (Parent == null)

                return;

            Rectangle rc = new Rectangle(Location, Size);

            Parent.Invalidate(rc, true);

        }

    }
}
