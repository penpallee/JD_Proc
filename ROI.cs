
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc
{
    public class RectTracker : System.Windows.Forms.UserControl
    {
        Control ROIparent;
        int nPicBoxX;
        int nPicBoxY;

        Brush colorSelect;

        private System.ComponentModel.Container components = null;
        //original rect
        Rectangle baseRect;

        //new rect with bounds
        Rectangle ControlRect;

        //all small rects
        Rectangle[] SmallRect = new Rectangle[8];

        //sqare size
        System.Drawing.Size Sqare = new System.Drawing.Size(3, 3);

        int lineBold;

        //graphics object
        Graphics g;

        //the control that is tracked
        Control currentControl;

        // To store the location of previous mouse left click in the form
        // so that we can use it to calculate the new form location during dragging
        private System.Drawing.Point prevLeftClick;

        bool reSizerect = false;

        // To determine if it is the first time entry for every dragging of the form
        private bool isFirst = true;

        //defines the color surrounding the tracker
        Color MyBackColor = Color.Red;

        //defines the color of the sqares
        Color SqareColor = Color.White;

        //defines the line color of the sqares 
        Color SqareLineColor = Color.Black;
        public Label roiLabel;

        //The Control the tracker is used on 
        public Control Control
        {
            get { return currentControl; }
            set
            {
                currentControl = value;
                Rect = currentControl.Bounds;
            }
        }

        /// <summary>
        /// keep track of which border of the box is to be resized.
        /// </summary>
        enum RESIZE_BORDER
        {
            RB_NONE = 0,
            RB_TOP = 1,
            RB_RIGHT = 2,
            RB_BOTTOM = 3,
            RB_LEFT = 4,
            RB_TOPLEFT = 5,
            RB_TOPRIGHT = 6,
            RB_BOTTOMLEFT = 7,
            RB_BOTTOMRIGHT = 8
        }

        //currently resize mouse location
        private RESIZE_BORDER CurrBorder;

        /// <summary>
        /// The current position of the rectangle in client coordinates (pixels).
        /// </summary>
        public Rectangle Rect
        {
            get { return baseRect; }
            set
            {
                int X = Sqare.Width;
                int Y = Sqare.Height;
                int Height = value.Height;
                int Width = value.Width;
                baseRect = new Rectangle(X, Y, Width, Height);
                SetRectangles();
            }

        }


        /// <summary>
        /// Set the Sqares positions
        /// </summary>
        void SetRectangles()
        {
            //TopLeft
            SmallRect[0] = new Rectangle(new System.Drawing.Point(baseRect.X - Sqare.Width, baseRect.Y - Sqare.Height), Sqare);
            //TopRight
            SmallRect[1] = new Rectangle(new System.Drawing.Point(baseRect.X + baseRect.Width, baseRect.Y - Sqare.Height), Sqare);
            //BottomLeft
            SmallRect[2] = new Rectangle(new System.Drawing.Point(baseRect.X - Sqare.Width, baseRect.Y + baseRect.Height), Sqare);
            //BottomRight
            SmallRect[3] = new Rectangle(new System.Drawing.Point(baseRect.X + baseRect.Width, baseRect.Y + baseRect.Height), Sqare);
            //TopMiddle
            SmallRect[4] = new Rectangle(new System.Drawing.Point(baseRect.X + (baseRect.Width / 2) - (Sqare.Width / 2), baseRect.Y - Sqare.Height), Sqare);
            //BottomMiddle
            SmallRect[5] = new Rectangle(new System.Drawing.Point(baseRect.X + (baseRect.Width / 2) - (Sqare.Width / 2), baseRect.Y + baseRect.Height), Sqare);
            //LeftMiddle
            SmallRect[6] = new Rectangle(new System.Drawing.Point(baseRect.X - Sqare.Width, baseRect.Y + (baseRect.Height / 2) - (Sqare.Height / 2)), Sqare);
            //RightMiddle
            SmallRect[7] = new Rectangle(new System.Drawing.Point(baseRect.X + baseRect.Width, baseRect.Y + (baseRect.Height / 2) - (Sqare.Height / 2)), Sqare);

            //the whole tracker rect
            ControlRect = new Rectangle(new System.Drawing.Point(0, 0), this.Bounds.Size);
        }

        /// <summary>
        /// Draws Sqares
        /// </summary>
        public void Draw()
        {
            try
            {
                g.FillRectangle(Brushes.Red, ControlRect);
                //g.DrawRectangle(Pens.Red, baseRect);
                //draw sqares
                g.FillRectangles(Brushes.White, SmallRect);
                //fill sqares
                //g.DrawRectangles(Pens.Red, SmallRect);              
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Draw(Control Parent, Brush color)
        {
            try
            {
                g.FillRectangle(color, ControlRect);
                //g.DrawRectangle(Pens.Red, baseRect);
                //draw sqares
                g.FillRectangles(Brushes.White, SmallRect);
                //fill sqares
                //g.DrawRectangles(Pens.Red, SmallRect);

                this.Parent = Parent;
                ROIparent = Parent;
                colorSelect = color;

                nPicBoxX = Parent.Size.Width;
                nPicBoxY = Parent.Size.Height;

                this.Focus();

                this.GotFocus += OnFocus;
                this.LostFocus += OnDeFocus;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnFocus(object sender, EventArgs e)
        {
            g.FillRectangles(Brushes.White, SmallRect);
            this.Location = new System.Drawing.Point(this.Left, this.Top);
        }

        private void OnDeFocus(object sender, EventArgs e)
        {
            g.FillRectangles(colorSelect, SmallRect);
            this.Location = new System.Drawing.Point(this.Left, this.Top);
        }

        /// <summary>
        /// Check point position to see if it's on tracker
        /// </summary>
        /// <param name="point">Current Point Position</param>
        /// <returns></returns>
        public bool Hit_Test(System.Drawing.Point point)
        {
            //Check if the point is somewhere on the tracker
            if (!ControlRect.Contains(point))
            {
                //should never happen
                Cursor.Current = Cursors.Arrow;

                return false;
            }
            else if (SmallRect[0].Contains(point))
            {
                Cursor.Current = Cursors.SizeNWSE;
                CurrBorder = RESIZE_BORDER.RB_TOPLEFT;
            }
            else if (SmallRect[3].Contains(point))
            {
                Cursor.Current = Cursors.SizeNWSE;
                CurrBorder = RESIZE_BORDER.RB_BOTTOMRIGHT;
            }
            else if (SmallRect[1].Contains(point))
            {
                Cursor.Current = Cursors.SizeNESW;
                CurrBorder = RESIZE_BORDER.RB_TOPRIGHT;
            }
            else if (SmallRect[2].Contains(point))
            {
                Cursor.Current = Cursors.SizeNESW;
                CurrBorder = RESIZE_BORDER.RB_BOTTOMLEFT;
            }
            else if (SmallRect[4].Contains(point))
            {
                Cursor.Current = Cursors.SizeNS;
                CurrBorder = RESIZE_BORDER.RB_TOP;
            }
            else if (SmallRect[5].Contains(point))
            {
                Cursor.Current = Cursors.SizeNS;
                CurrBorder = RESIZE_BORDER.RB_BOTTOM;
            }
            else if (SmallRect[6].Contains(point))
            {
                Cursor.Current = Cursors.SizeWE;
                CurrBorder = RESIZE_BORDER.RB_LEFT;
            }
            else if (SmallRect[7].Contains(point))
            {
                Cursor.Current = Cursors.SizeWE;
                CurrBorder = RESIZE_BORDER.RB_RIGHT;
            }
            else if (ControlRect.Contains(point))
            {
                Cursor.Current = Cursors.SizeAll;
                CurrBorder = RESIZE_BORDER.RB_NONE;

            }


            return true;
        }

        public bool Hit_Test(int x, int y)
        {
            return Hit_Test(new System.Drawing.Point(x, y));
        }

        public RectTracker(Control theControl)
        {
            InitializeComponent();

            currentControl = theControl;

            Create();
        }

        public RectTracker()
        {
            InitializeComponent();
        }

        private void Create()
        {
            int X;
            int Y;
            int Height;
            int Width;

            X = currentControl.Bounds.X - Sqare.Width;
            Y = currentControl.Bounds.Y - Sqare.Height;
            Height = currentControl.Bounds.Height + (Sqare.Height * 2);
            Width = currentControl.Bounds.Width + (Sqare.Width * 2);

            //set bounds
            this.Bounds = new Rectangle(X, Y, Width + 1, Height + 1);

            this.BringToFront();

            //create inside rect bounds
            Rect = currentControl.Bounds;

            //create transparent area around the control
            this.Region = new Region(BuildFrame());

            //create graphics
            g = this.CreateGraphics();
        }

        private void Create(int moveX, int moveY, int moveHeight, int moveWidth)
        {
            int X;
            int Y;
            int Height;
            int Width;

            //create tracker bounds

            X = (currentControl.Bounds.X - Sqare.Width) + moveX;
            Y = (currentControl.Bounds.Y - Sqare.Height) + moveY;
            Height = (currentControl.Bounds.Height + (Sqare.Height * 2)) + moveHeight;
            Width = (currentControl.Bounds.Width + (Sqare.Width * 2)) + moveWidth;

            //set bounds
            this.Bounds = new Rectangle(X, Y, Width + 1, Height + 1);

            this.BringToFront();

            //create inside rect bounds
            Rect = currentControl.Bounds;

            //create transparent area around the control
            this.Region = new Region(BuildFrame());

            //create graphics
            g = this.CreateGraphics();
        }


        /// <summary>
        /// Transparents the control area in the tracker
        /// </summary>
        /// <returns>Graphics Path made for transparenting</returns>
        private GraphicsPath BuildFrame()
        {
            //make the tracker to "contain" the control like this:
            //
            //
            //+++++++++++++++++++++++
            //+						+
            //+						+
            //+	 	 CONTROL		+
            //+						+
            //+						+
            //+++++++++++++++++++++++
            //

            GraphicsPath path = new GraphicsPath();

            //Top Rectagle
            path.AddRectangle(new Rectangle(0, 0, currentControl.Width + (Sqare.Width * 2) + 1, Sqare.Height + 1)); ;
            //Left Rectangle
            path.AddRectangle(new Rectangle(0, Sqare.Height + 1, Sqare.Width + 1, currentControl.Bounds.Height + Sqare.Height + 1));
            //Bottom Rectagle
            path.AddRectangle(new Rectangle(Sqare.Width + 1, currentControl.Bounds.Height + Sqare.Height, currentControl.Width + Sqare.Width + 1, Sqare.Height + 1));
            //Right Rectagle
            path.AddRectangle(new Rectangle(currentControl.Width + Sqare.Width, Sqare.Height + 1, Sqare.Width + 1, currentControl.Height - 1));

            return path;
        }





        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.roiLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // roiLabel
            // 
            this.roiLabel.BackColor = System.Drawing.Color.Transparent;
            this.roiLabel.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roiLabel.Location = new System.Drawing.Point(13, 11);
            this.roiLabel.Name = "roiLabel";
            this.roiLabel.Size = new System.Drawing.Size(33, 11);
            this.roiLabel.TabIndex = 0;
            this.roiLabel.Text = "label1";
            // 
            // RectTracker
            // 
            this.BackColor = System.Drawing.Color.Red;
            this.Controls.Add(this.roiLabel);
            this.Location = new System.Drawing.Point(15, 32);
            this.Name = "RectTracker";
            this.Size = new System.Drawing.Size(165, 141);
            this.LocationChanged += new System.EventHandler(this.RectTracker_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.RectTracker_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.RectTracker_VisibleChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RectTracker_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RectTracker_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RectTracker_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RectTracker_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RectTracker_MouseUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Arrow_PreviewKeyDown);
            this.ResumeLayout(false);

        }
        #endregion

        private void Arrow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Up:
            //        e.IsInputKey = true;
            //        break;
            //    case Keys.Down:
            //        e.IsInputKey = true;
            //        break;
            //    case Keys.Left:
            //        e.IsInputKey = true;
            //        break;
            //    case Keys.Right:
            //        e.IsInputKey = true;
            //        break;
            //}
        }


        public void Mouse_Move(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ////minimum size for the control is 8x8
            //if (currentControl.Height < 8)
            //{
            //    currentControl.Height = 8;
            //    return;
            //}
            //else if (currentControl.Width < 8)
            //{
            //    currentControl.Width = 8;
            //    return;
            //}

            //if (currentControl.Height > nPicBoxY)
            //{
            //    currentControl.Height = nPicBoxY;
            //    return;
            //}
            //else if (currentControl.Width > nPicBoxX)
            //{
            //    currentControl.Width = nPicBoxX;
            //    return;
            //}

            //switch (this.CurrBorder)
            //{
            //    case RESIZE_BORDER.RB_TOP:
            //        reSizerect = true;
            //        currentControl.Height = currentControl.Height - e.Y + prevLeftClick.Y;
            //        if (currentControl.Height > 8)
            //            currentControl.Top = currentControl.Top + e.Y - prevLeftClick.Y;
            //        break;
            //    case RESIZE_BORDER.RB_TOPLEFT:
            //        reSizerect = true;
            //        currentControl.Height = currentControl.Height - e.Y + prevLeftClick.Y;
            //        if (currentControl.Height > 8)
            //            currentControl.Top = currentControl.Top + e.Y - prevLeftClick.Y;
            //        currentControl.Width = currentControl.Width - e.X + prevLeftClick.X;
            //        if (currentControl.Width > 8)
            //            currentControl.Left = currentControl.Left + e.X - prevLeftClick.X;
            //        break;
            //    case RESIZE_BORDER.RB_TOPRIGHT:
            //        reSizerect = true;
            //        currentControl.Height = currentControl.Height - e.Y + prevLeftClick.Y;
            //        if (currentControl.Height > 8)
            //            currentControl.Top = currentControl.Top + e.Y - prevLeftClick.Y;
            //        currentControl.Width = currentControl.Width + e.X - prevLeftClick.X;

            //        if (currentControl.Width + currentControl.Left >= nPicBoxX)
            //        {
            //            currentControl.Width = nPicBoxX - currentControl.Left;
            //            currentControl.Left = currentControl.Left;
            //        }

            //        break;
            //    case RESIZE_BORDER.RB_RIGHT:
            //        reSizerect = true;
            //        currentControl.Width = currentControl.Width + e.X - prevLeftClick.X;
            //        if (currentControl.Width + currentControl.Left >= nPicBoxX)
            //        {
            //            currentControl.Width = nPicBoxX - currentControl.Left;
            //            currentControl.Left = currentControl.Left;
            //        }

            //        break;
            //    case RESIZE_BORDER.RB_BOTTOM:
            //        reSizerect = true;
            //        currentControl.Height = currentControl.Height + e.Y - prevLeftClick.Y;

            //        if (currentControl.Height + currentControl.Top >= nPicBoxY)
            //        {
            //            currentControl.Height = nPicBoxY - currentControl.Top;
            //            currentControl.Top = currentControl.Top;
            //        }
            //        break;
            //    case RESIZE_BORDER.RB_BOTTOMLEFT:
            //        reSizerect = true;

            //        currentControl.Height = currentControl.Height + e.Y - prevLeftClick.Y;
            //        currentControl.Width = currentControl.Width - e.X + prevLeftClick.X;
            //        if (currentControl.Width > 8)
            //            currentControl.Left = currentControl.Left + e.X - prevLeftClick.X;

            //        if (currentControl.Height + currentControl.Top >= nPicBoxY)
            //        {
            //            currentControl.Height = nPicBoxY - currentControl.Top;
            //            currentControl.Top = currentControl.Top;
            //        }
            //        break;
            //    case RESIZE_BORDER.RB_BOTTOMRIGHT:
            //        reSizerect = true;
            //        currentControl.Height = currentControl.Height + e.Y - prevLeftClick.Y;
            //        currentControl.Width = currentControl.Width + e.X - prevLeftClick.X;

            //        if (currentControl.Width + currentControl.Left >= nPicBoxX)
            //        {
            //            currentControl.Width = nPicBoxX - currentControl.Left;
            //            currentControl.Left = currentControl.Left;
            //        }

            //        if (currentControl.Height + currentControl.Top >= nPicBoxY)
            //        {
            //            currentControl.Height = nPicBoxY - currentControl.Top;
            //            currentControl.Top = currentControl.Top;
            //        }

            //        break;
            //    case RESIZE_BORDER.RB_LEFT:
            //        reSizerect = true;
            //        currentControl.Width = currentControl.Width - e.X + prevLeftClick.X;
            //        if (currentControl.Width > 8)
            //            currentControl.Left = currentControl.Left + e.X - prevLeftClick.X;
            //        break;
            //    case RESIZE_BORDER.RB_NONE:
            //        reSizerect = false;
            //        currentControl.Location = new System.Drawing.Point(currentControl.Location.X + e.X - prevLeftClick.X, currentControl.Location.Y + e.Y - prevLeftClick.Y);
            //        break;
            //}

        }

        private void RectTracker_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;


            if (e.Button == MouseButtons.Left)
            {
                // If this is the first mouse move event for left click dragging of the form,
                // store the current point clicked so that we can use it to calculate the form's
                // new location in subsequent mouse move events due to left click dragging of the form
                if (isFirst == true)
                {
                    // Store previous left click position
                    prevLeftClick = new System.Drawing.Point(e.X, e.Y);

                    //Create();
                    // Subsequent mouse move events will not be treated as first time, until the
                    // left mouse click is released or other mouse click occur
                    isFirst = false;
                }
                else
                {
                    //hide tracker

                    this.Visible = true;
                    //forward mouse position to append changes
                    Mouse_Move(this, e);

                    // Store new previous left click position
                    //prevLeftClick = new Point(e.X, e.Y);
                    if (!reSizerect)
                    {
                        Create();

                        //prevLeftClick = new Point(e.X, e.Y);

                        this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
                    }

                    else if (reSizerect)
                    {
                        Create();
                        prevLeftClick = new System.Drawing.Point(e.X, e.Y);
                    }

                }
            }
            else
            {
                // This is a new mouse move event so reset flag
                isFirst = true;
                //show the tracker
                this.Visible = true;

                //update the mouse pointer to other cursors by checking it's position
                Hit_Test(e.X, e.Y);
            }

        }

        private void RectTracker_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //redraw sqares
            Draw(ROIparent, colorSelect);
        }

        private void RectTracker_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            ////the mouse is up, recreate the control to append changes
            //if (this.Focused)
            //{
            //    Create();
            //    this.Visible = true;
            //    if (this.Left < 0)
            //    {
            //        this.Left = 0;
            //        this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);

            //        if (this.Width >= nPicBoxX)
            //        {
            //            this.Width = nPicBoxX;
            //        }
            //    }

            //    if (this.Top < 20)
            //    {
            //        if (this.Top < 0)
            //        {
            //            this.Top = 0;
            //        }
            //        this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);

            //        if (this.Height >= nPicBoxY)
            //        {
            //            this.Height = nPicBoxY;
            //        }
            //    }

            //    if ((this.Left >= 0) && (this.Top >= 20))
            //    {
            //        this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //    }

            //    if (this.Left + this.Width >= nPicBoxX)
            //    {
            //        if (this.Width >= nPicBoxX)
            //        {
            //            this.Width = nPicBoxX - 1;
            //            this.Left = 0;
            //        }

            //        if (!reSizerect)
            //            this.Left = nPicBoxX - this.Width;
            //        if (reSizerect)
            //            this.Left = this.Left;
            //        this.Width = nPicBoxX - this.Left;

            //        if (this.Top < 20)
            //        {
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
            //        }
            //        else
            //        {
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //        }
            //    }

            //    if (this.Top + this.Height >= nPicBoxY)
            //    {
            //        if (this.Height >= nPicBoxY)
            //        {
            //            this.Height = nPicBoxY;
            //        }
            //        this.Top = nPicBoxY - this.Height;
            //        this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //    }


            //}

            //if (reSizerect)
            //    reSizerect = false;
        }

        public void StartTracking(Control ctl)
        {
            if ((ctl != null) && (ctl.Parent != null))
            {
                ctl.BringToFront();

                ctl.Capture = false;

                if (ctl.Parent.Controls.Contains(this))
                    ctl.Parent.Controls.Remove(this);

                this.currentControl = ctl;

                Create();

                ctl.Parent.Controls.Add(this);
                this.BringToFront();
                this.Draw();
            }
        }

        private void RectTracker_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Up:
            //        currentControl.Top = currentControl.Top - 1;
            //        this.Location = new System.Drawing.Point(this.Left, this.Top - 1);

            //        this.roiLabel.Location = new System.Drawing.Point(this.roiLabel.Left, this.roiLabel.Top - 1);
            //        if (this.Top < 20)
            //        {
            //            if (this.Top < 0)
            //            {
            //                this.Top = 0;
            //            }
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
            //        }

            //        if ((this.Left >= 0) && (this.Top >= 20))
            //        {
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //        }

            //        break;

            //    case Keys.Down:
            //        currentControl.Top = currentControl.Top + 1;
            //        this.Location = new System.Drawing.Point(this.Left, this.Top + 1);
            //        this.roiLabel.Location = new System.Drawing.Point(this.roiLabel.Left, this.roiLabel.Top + 1);
            //        if (this.Top < 20)
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
            //        else
            //            this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //        if (this.Top + this.Height > nPicBoxY)
            //        {
            //            this.Top = nPicBoxY - this.Height;
            //        }

            //        break;

            //    case Keys.Left:
            //        currentControl.Left = currentControl.Left - 1;
            //        this.Location = new System.Drawing.Point(this.Left - 1, this.Top);
            //        this.roiLabel.Location = new System.Drawing.Point(this.roiLabel.Left - 1, this.roiLabel.Top);
            //        if (this.Left < 0)
            //        {
            //            this.Left = 0;
            //            if (this.Top < 20)
            //                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
            //            else
            //                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //        }
            //        break;

            //    case Keys.Right:
            //        currentControl.Left = currentControl.Left + 1;
            //        this.Location = new System.Drawing.Point(this.Left + 1, this.Top);
            //        this.roiLabel.Location = new System.Drawing.Point(this.roiLabel.Left + 1, this.roiLabel.Top);
            //        if (this.Left + this.Width > nPicBoxX)
            //        {
            //            this.Left = nPicBoxX - this.Width;
            //            if (this.Top < 20)
            //            {
            //                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
            //            }
            //            else
            //            {
            //                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
            //            }
            //        }
            //        break;
            //}
        }

        private void RectTracker_KeyUp(object sender, KeyEventArgs e)
        {
            //int X;
            //int Y;
            //int HEIGHT;
            //int WIDTH;

            //X = currentControl.Bounds.X;
            //Y = currentControl.Bounds.Y;
            //HEIGHT = currentControl.Bounds.Height;
            //WIDTH = currentControl.Bounds.Width;

            ////Create();
            //switch (e.KeyCode)
            //{
            //    case Keys.Up:
            //        this.Location = new System.Drawing.Point(this.Left, this.Top);
            //        break;
            //    case Keys.Down:
            //        this.Location = new System.Drawing.Point(this.Left, this.Top);
            //        break;
            //    case Keys.Left:
            //        this.Location = new System.Drawing.Point(this.Left, this.Top);
            //        break;
            //    case Keys.Right:
            //        this.Location = new System.Drawing.Point(this.Left, this.Top);
            //        break;
            //}

            ////Create();
        }

        private void RectTracker_LocationChanged(object sender, EventArgs e)
        {
            lineBold = Sqare.Height;

            if (this.Left < 0)
            {
                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
                currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                //currentControl.Size = new Size(this.Width, this.Height);
            }

            if (this.Top < 20)
            {
                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
                currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                //currentControl.Size = new Size(this.Width, this.Height);
            }

            if ((this.Left >= 0) && (this.Top >= 20))
            {
                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
                currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                //currentControl.Size = new Size(this.Width, this.Height);
            }

            if (this.Left + this.Width >= nPicBoxX)
            {
                if (this.Top < 20)
                {
                    this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top + this.Height + 5);
                    currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                    //currentControl.Size = new Size(this.Width, this.Height);
                }
                else
                {
                    this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
                    currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                    //currentControl.Size = new Size(this.Width, this.Height);
                }
            }

            if (this.Top + this.Height >= nPicBoxY)
            {
                this.roiLabel.Location = new System.Drawing.Point(this.Left, this.Top - 15);
                currentControl.Location = new System.Drawing.Point(this.Left + lineBold, this.Top + lineBold);
                //currentControl.Size = new Size(this.Width, this.Height);
            }

            //currentControl.Location = new Point(this.Left, this.Top);
            //currentControl.Size = new Size(this.Width, this.Height);

            g = this.CreateGraphics();
            Create();
        }

        private void RectTracker_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.roiLabel.Visible = true;
                Create();
            }
            else
                this.roiLabel.Visible = false;
        }

        private void RectTracker_SizeChanged(object sender, EventArgs e)
        {
            //Create();
        }
    }
}
