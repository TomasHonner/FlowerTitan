using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace FlowerTitan.MeasuringLines
{
    /// <summary>
    /// Singleton class which enables measuring lines operations.
    /// </summary>
    public class MeasuringLines
    {
        //singleton instance
        private static MeasuringLines measuringLines = null;
        //main window instance
        private static MainWindow mainWindow = null;

        //determines whether the process button was pressed for the first time
        private bool firstProcessing = true;
        //determines whether enabling lines was called for the first time
        private bool firstCall = true;
        //determines whether the mouse's cursor is on a point
        private bool isOnPoint = false;
        //determines whether the selected point should be moved
        private bool isMoving = false;
        //determines whether a point is selected
        private bool isSelected = false;
        //determines whether a next click will be a new line
        private bool isNew = true;
        //determines whether listBoxe's selected index is changing
        private bool indexMove = false;
        //counts added images if process button was pressed more than once
        private int addedImages = 0;
        //counts whether it was first processing or a next one
        private int processingCount = 0;
        //holds reference to the selected line
        private int selectedLine = 0;
        //holds reference to the selected point
        private int selectedPoint = 0;
        //counts lines' count for naming purpose
        private int linesCounter = 0;
        //holds lines' line thickness
        private float thickness = 0f;
        //holds images scale
        private double scale = 0;
        //holds sender's image
        private Emgu.CV.UI.ImageBox imageSender;
        //holds sender's image's lines
        private AllLines linesSender;
        //holds movable points (ellipse) size
        private Size pointSize = new Size(7, 7);
        //holds all lines of all images which are used by measuring lines
        private List<AllLines> allLines = new List<AllLines>();
        //holds all images which are used by measuring lines
        private List<Emgu.CV.UI.ImageBox> allImages = new List<Emgu.CV.UI.ImageBox>();
        //holds lines' names
        private List<string> lineNames = new List<string>();
        //holds lines' colors
        private List<Color> lineColors = new List<Color>();

        //determines whether measuring lines were enabled
        private bool isEnabled = false;
        /// <summary>
        /// Returns whether measuring lines are enabled.
        /// </summary>
        public bool IsEnabled { get { return isEnabled; } }

        public Color[] Colors { get { return lineColors.ToArray(); } }

        public string[] Names { get { return lineNames.ToArray(); } }

        /// <summary>
        /// Returns image's scale in DPI.
        /// </summary>
        public double Scale { get { return scale; } }

        public void SetTemplateLines(Emgu.CV.UI.ImageBox image, Line[] tempLines, int[] colors, string[] names)
        {
            double oldScale = scale;
            NewTemplate();
            EnableMeasuringLinesOnFirstImage(image, oldScale);
            addLines(image, tempLines, colors, names);
        }

        private void addLines(Emgu.CV.UI.ImageBox image, Line[] tempLines, int[] colors, string[] names)
        {
            int i = 0;
            foreach (Line line in tempLines)
            {
                foreach (Point p in line.Points)
                {
                    image_MouseClick(image, new MouseEventArgs(MouseButtons.Left, 1, p.X, p.Y, 0));
                }
                image_MouseClick(image, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
                mainWindow.textBoxLine.Text = names[i];
                mainWindow.listBoxLines.Items[i] = mainWindow.textBoxLine.Text;
                lineNames[i] = mainWindow.textBoxLine.Text;
                mainWindow.buttonColor.BackColor = Color.FromArgb(colors[i]);
                lineColors[i] = mainWindow.buttonColor.BackColor;
                i++;
            }
            repaintAllImages();
        }

        public void SetAllTemplateLines(AllLines[] allLines, int[] colors, string[] names, byte[][] images, string name, double scale, long tempID)
        {
            Emgu.CV.UI.ImageBox[] allBoxes = { mainWindow.iB1, mainWindow.iB2, mainWindow.iB3, mainWindow.iB4, mainWindow.iB5, mainWindow.iB6, mainWindow.iB7, mainWindow.iB8, mainWindow.iB9, mainWindow.iB10, mainWindow.iB11, mainWindow.iB12 };
            NewTemplate();
            mainWindow.tID.Text = tempID.ToString();
            mainWindow.tBtemplateName.Text = name;
            EnableMeasuringLinesOnFirstImage(allBoxes[0], scale);
            addLines(allBoxes[0], allLines[0].Lines.ToArray(), colors, names);
            ImageConverter converter = new ImageConverter();
            int i = 0;
            foreach (AllLines al in allLines)
            {
                long id = al.ImageBoxID - 1;
                allBoxes[id].Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(new Bitmap((Image)converter.ConvertFrom(images[i])));
                if (i > 0) AddMeasuringLinesToImage(allBoxes[id], al.Lines.ToArray(), al.ImageBoxID);
                i++;
            }
        }

        /// <summary>
        /// Returns all lines of all active images.
        /// </summary>
        public AllLines[] ActiveImagesLines
        {
            get
            {
                List<AllLines> al = new List<AllLines>();
                foreach (Emgu.CV.UI.ImageBox image in allImages)
                {
                    al.Add((AllLines)image.Tag);
                }
                return al.ToArray();
            }
        }

        public Bitmap[] ActiveImagesImages
        {
            get
            {
                List<Bitmap> img = new List<Bitmap>();
                foreach (Emgu.CV.UI.ImageBox image in allImages)
                {
                    img.Add(image.Image.Bitmap);
                }
                return img.ToArray();
            }
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private MeasuringLines() {}

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <param name="mW">Instance of MainWindow which has needed UI elements.</param>
        /// <returns>MeasuringLines instance.</returns>
        public static MeasuringLines GetInstance(MainWindow mW)
        {            
            if (measuringLines == null)
            {
                measuringLines = new MeasuringLines();
            }
            mainWindow = mW;
            return measuringLines;
        }

        /// <summary>
        /// Enables measuring lines on one picture (usually the first one).
        /// </summary>
        /// <param name="image">first image</param>
        /// <param name="scale">imported image's scale in DPI(e.g. 300 DPI means that 300 pixels are 25,4 mm in reality; if image is zoomed, e.g. from 1000 px to 100 px with DPI=200 to fit imagebox, then scale = 20 [DPI/(original size/new size)])</param>
        public void EnableMeasuringLinesOnFirstImage(Emgu.CV.UI.ImageBox image, double scale)
        {
            //if it is called for the first time
            if (firstCall)
            {
                //adds main handlers to UI
                mainWindow.buttonColor.Click += buttonColor_Click;
                mainWindow.trackBarPointSize.ValueChanged += trackBarPointSize_ValueChanged;
                mainWindow.trackBarThickness.ValueChanged += trackBarThickness_ValueChanged;
                mainWindow.KeyDown += MainWindow_KeyDown;
                mainWindow.listBoxLines.SelectedIndexChanged += listBoxLines_SelectedIndexChanged;
                mainWindow.textBoxLine.TextChanged += textBoxLine_TextChanged;
                firstCall = false;
                isEnabled = true;
            }
            //addition of the very first image
            image.Tag = new AllLines(1);
            addHandlersToImage(image);
            linesSender = allLines[0];
            imageSender = allImages[0];
            this.scale = scale;
        }

        /// <summary>
        /// Adds processed lines to an image.
        /// </summary>
        /// <param name="image">Image to which measuring lines are added.</param>
        /// <param name="lines">Array of lines for the particular image.</param>
        public void AddMeasuringLinesToImage(Emgu.CV.UI.ImageBox image, Line[] lines, long imageBoxID)
        {
            addedImages++;
            //creates deep copy of added lines
            AllLines al = new AllLines();
            int i = 0;
            foreach (Line l in lines)
            {
                al.Lines.Add(new Line());
                int j = 0;
                foreach (Point p in lines[i].Points)
                {
                    al.Lines[i].Points.Add(new Point(lines[i].Points[j].X, lines[i].Points[j].Y));
                    j++;
                }
                i++;
            }
            image.Tag = al;
            if (firstProcessing)
            {
                //adding handlers to image
                addHandlersToImage(image);
            }
            else
            {
                //handlers are already added, just change of image's lines reference
                allLines[addedImages] = al;
            }
            al.ImageBoxID = imageBoxID;
            //force line drawing
            image.Refresh();
        }

        /// <summary>
        /// Returns deep copy of reference lines.
        /// </summary>
        /// <returns></returns>
        public Line[] GetReferenceMeasuringLines()
        {
            List<Line> deepCopy = new List<Line>();
            int i = 0;
            foreach (Line l in allLines[0].Lines)
            {
                //copy every line
                deepCopy.Add(new Line());
                int j = 0;
                foreach (Point p in allLines[0].Lines[i].Points)
                {
                    //copy every line's point
                    deepCopy[i].Points.Add(new Point(allLines[0].Lines[i].Points[j].X, allLines[0].Lines[i].Points[j].Y));
                    j++;
                }
                i++;
            }
            isNew = true;
            //prevent adding event handlers more than once if process button was pressed more than once
            if (processingCount == 1)
            {
                firstProcessing = false;
                processingCount = 0;
            }
            processingCount++;
            addedImages = 0;
            return deepCopy.ToArray();
        }

        /// <summary>
        /// Prepares for a new template and restores default settings.
        /// </summary>
        public void NewTemplate()
        {
            //reseting listBoxe's items
            indexMove = true;
            mainWindow.listBoxLines.Items.Clear();
            mainWindow.listBoxLines.ClearSelected();
            mainWindow.textBoxLine.Text = "";
            indexMove = false;
            //removing all added event handlers and measuring lines
            foreach (Emgu.CV.UI.ImageBox iB in allImages)
            {
                iB.MouseEnter -= image_MouseEnter;
                iB.MouseMove -= image_MouseMove;
                iB.MouseDown -= image_MouseDown;
                iB.MouseClick -= image_MouseClick;
                iB.MouseUp -= image_MouseUp;
                iB.Paint -= image_Paint;
                iB.Tag = null;
                iB.Image = null;
                //delete lines from image
                iB.Refresh();
            }
            //clearing working collection sets
            allImages.Clear();
            allLines.Clear();
            lineNames.Clear();
            lineColors.Clear();
            //reseting settings to default
            mainWindow.trackBarThickness.Value = 20;
            mainWindow.trackBarPointSize.Value = 5;
            thickness = mainWindow.trackBarThickness.Value / 10f;
            pointSize = new Size(mainWindow.trackBarPointSize.Value, mainWindow.trackBarPointSize.Value);
            mainWindow.buttonColor.BackColor = Color.Black;
            isOnPoint = false;
            isMoving = false;
            isSelected = false;
            isNew = true;
            indexMove = false;
            selectedLine = 0;
            selectedPoint = 0;
            linesCounter = 0;
            firstProcessing = true;
            scale = 0;
            addedImages = 0;
            processingCount = 0;
        }
        
        /// <summary>
        /// Changes lines' thickness.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        private void trackBarThickness_ValueChanged(object sender, EventArgs e)
        {
            thickness = mainWindow.trackBarThickness.Value / 10f;
            repaintAllImages();
        }

        /// <summary>
        /// Repaints all images in working set.
        /// </summary>
        private void repaintAllImages()
        {
            //repaint every image in the working set
            foreach (Emgu.CV.UI.ImageBox iB in allImages)
            {
                iB.Refresh();
            }
        }

        /// <summary>
        /// Changes point's size.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        private void trackBarPointSize_ValueChanged(object sender, EventArgs e)
        {
            pointSize = new Size(mainWindow.trackBarPointSize.Value, mainWindow.trackBarPointSize.Value);
            repaintAllImages();
        }

        /// <summary>
        /// Adds needed event handlers to image and adds references to lists.
        /// </summary>
        /// <param name="imageBox">Image box which event handlers will be added to.</param>
        private void addHandlersToImage(Emgu.CV.UI.ImageBox imageBox)
        {
            imageBox.MouseEnter += image_MouseEnter;
            imageBox.MouseMove += image_MouseMove;
            imageBox.MouseDown += image_MouseDown;
            imageBox.MouseClick += image_MouseClick;
            imageBox.MouseUp += image_MouseUp;
            imageBox.Paint += image_Paint;
            //add to working collection
            allLines.Add((AllLines)imageBox.Tag);
            allImages.Add(imageBox);
        }

        /// <summary>
        /// Event handler for line's name text box.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        private void textBoxLine_TextChanged(object sender, EventArgs e)
        {
            //prevents exception during renaming
            if (!indexMove && isSelected)
            {
                //allows projection of line's name changes to list box
                indexMove = true;
                mainWindow.listBoxLines.Items[selectedLine] = mainWindow.textBoxLine.Text;
                lineNames[selectedLine] = mainWindow.textBoxLine.Text;
                indexMove = false;
            }
        }

        /// <summary>
        /// Event handler for list box holding lines' names.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        private void listBoxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            //prevents IndexOutOfRange exception while deleting and renaming lines
            if (!indexMove)
            {
                //set selected line and point
                selectedLine = mainWindow.listBoxLines.SelectedIndex;
                selectedPoint = linesSender.Lines[mainWindow.listBoxLines.SelectedIndex].Points.Count - 1;
                isSelected = true;
                //projection of name to text box
                mainWindow.textBoxLine.Text = mainWindow.listBoxLines.Items[selectedLine].ToString();
                isNew = true;
                //changes color picker's color according to the selected line
                mainWindow.buttonColor.BackColor = lineColors[selectedLine];
                repaintAllImages();
            }
        }

        /// <summary>
        /// Event handler which serves key's presses on an active form - main window.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">key event</param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //prevent deleting line/point if nothing is selected or text box has focus - letters deleting
            if (isSelected && !mainWindow.textBoxLine.Focused)
            {
                switch (e.KeyCode)
                {
                    //delete selected point
                    case Keys.Delete:
                        //delete selected point on every image
                        foreach (AllLines al in allLines)
                        {
                            al.Lines[selectedLine].Points.RemoveAt(selectedPoint);
                        }
                        //select a new point - the last one
                        if (linesSender.Lines[selectedLine].Points.Count > 0)
                        {
                            selectedPoint = linesSender.Lines[selectedLine].Points.Count - 1;
                        }
                        else
                        {
                            //if there is no remaining point delete whole line
                            deleteLine();
                        }
                        break;
                        //delete whole line
                    case Keys.Back:
                        deleteLine();
                        break;
                    default:
                        //deselect point for safety
                        isSelected = false;
                        break;
                }
                repaintAllImages();
            }
        }

        /// <summary>
        /// Deletes whole line.
        /// </summary>
        private void deleteLine()
        {
            //force a new line if current wasn't ended
            isNew = true;
            //remove line from every working image
            foreach (AllLines al in allLines)
            {
                al.Lines.RemoveAt(selectedLine);
            }
            //change selected line
            indexMove = true;
            mainWindow.listBoxLines.Items.RemoveAt(selectedLine);
            //select the last line in set
            mainWindow.listBoxLines.SelectedIndex = linesSender.Lines.Count - 1;
            //change text in text box
            if (mainWindow.listBoxLines.Items.Count > 0)
            {
                mainWindow.textBoxLine.Text = mainWindow.listBoxLines.Items[linesSender.Lines.Count - 1].ToString();
            }
            else
            {
                //if there is no line left
                mainWindow.textBoxLine.Text = "";
            }
            indexMove = false;
            //if there is a line left select the last one
            if (linesSender.Lines.Count > 0)
            {
                selectedLine = linesSender.Lines.Count - 1;
                selectedPoint = linesSender.Lines[selectedLine].Points.Count - 1;
            }
            else
            {
                //no line, no selection, no deleting
                isSelected = false;
            }
        }

        /// <summary>
        /// Handler for mouse enter event. It stores active image and its lines.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void image_MouseEnter(object sender, EventArgs e)
        {
            //get sender's image
            imageSender = (Emgu.CV.UI.ImageBox)sender;
            //get sender's lines
            linesSender = (AllLines)imageSender.Tag;
        }

        /// <summary>
        /// Allows points movement if LMB is pressed.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            //if left button is pressed, move with line
            if (isMoving)
            {
                //check whether cursor is on image, not elswhere
                if (checkImageBounds(e.X, e.Y))
                {
                    //move with point, subsequently with line
                    linesSender.Lines[selectedLine].Points[selectedPoint] = new Point(e.X, e.Y);
                    //force refresh to make changes to be visible
                    imageSender.Refresh();
                }
            }
            else
            {
                //check whether cursor is on a point
                scanPoints(e);
            }
        }

        /// <summary>
        /// Checks whether cursor is on image.
        /// </summary>
        /// <param name="x">x-coord</param>
        /// <param name="y">y-coord</param>
        /// <returns></returns>
        private bool checkImageBounds(int x, int y)
        {
            bool isInImage = false;
            //cursor inside of an image, offset due to ellipse drawing from top left corner
            if (((x >= 0) && (x <= (imageSender.Width - (pointSize.Width / 2)))) && ((y >= 0) && (y <= (imageSender.Height - (pointSize.Height / 2)))))
            {
                isInImage = true;
            }
            return isInImage;
        }

        /// <summary>
        /// Checks whether the cursor is on a point.
        /// Returns if point is found to the global variable and changes mouse's cursor on points to Sizeable cursor and backwards.
        /// </summary>
        /// <param name="e">mouse event</param>
        private void scanPoints(MouseEventArgs e)
        {
            int l = 0;
            foreach (Line line in linesSender.Lines)
            {
                int p = 0;
                foreach (Point point in line.Points)
                {
                    //offset due to ellipse drawing from top left corner
                    int offsetX = pointSize.Width / 2;
                    int offsetY = pointSize.Height / 2;
                    int xS = point.X - offsetX;
                    int yS = point.Y - offsetY;
                    int xE = point.X + offsetX;
                    int yE = point.Y + offsetY;
                    //if cursor is on positon of point or point plus point width/height (moved due to the drawn point's offset)
                    if (((e.X >= xS) && (e.X <= xE)) && ((e.Y >= yS) && (e.Y <= yE)))
                    {
                        Cursor.Current = Cursors.SizeAll;
                        isOnPoint = true;
                        //set selected line, point
                        selectedLine = l;
                        selectedPoint = p;
                        return;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Arrow;
                        isOnPoint = false;
                    }
                    p++;
                }
                l++;
            }
        }

        /// <summary>
        /// Allows point's move in mouse move event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void image_MouseDown(object sender, MouseEventArgs e)
        {
            //cursor is on point and left mouse button is pressed
            if (isOnPoint && (e.Button == System.Windows.Forms.MouseButtons.Left))
            {
                isMoving = true;
                isSelected = true;
                //changes color picker's color according to the selected line
                mainWindow.buttonColor.BackColor = lineColors[selectedLine];
                indexMove = true;
                //select according line in list box
                mainWindow.listBoxLines.SelectedIndex = selectedLine;
                //set text in text box
                mainWindow.textBoxLine.Text = mainWindow.listBoxLines.Items[selectedLine].ToString();
                indexMove = false;
            }
            else
            {
                isMoving = false;
                isSelected = false;
            }
            repaintAllImages();
        }

        /// <summary>
        /// Adds lines functions to mouse buttons.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void image_MouseClick(object sender, MouseEventArgs e)
        {
            //point is not moving
            if (!isMoving)
            {
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        //new line
                        if (isNew)
                        {
                            //create new line on every image in working set
                            foreach (AllLines al in allLines)
                            {
                                al.Lines.Add(new Line());
                            }
                            //set line's color
                            lineColors.Add(mainWindow.buttonColor.BackColor);
                            linesCounter++;
                            //counter reset
                            if (linesCounter == 100)
                            {
                                linesCounter = 0;
                            }
                            //adding line to list box and creating default name
                            string name = "Line " + (linesCounter).ToString();
                            mainWindow.listBoxLines.Items.Add(name);
                            lineNames.Add(name);
                            isNew = false;
                        }
                        //add line's point to every image in working set
                        foreach (AllLines al in allLines)
                        {
                            //new point
                            al.Lines[linesSender.Lines.Count - 1].Points.Add(new Point(e.X, e.Y));
                        }
                        //set current line's color in case it was changed
                        lineColors[linesSender.Lines.Count - 1] = mainWindow.buttonColor.BackColor;
                        //selecting just added line
                        indexMove = true;
                        mainWindow.listBoxLines.SelectedIndex = mainWindow.listBoxLines.Items.Count - 1;
                        mainWindow.textBoxLine.Text = mainWindow.listBoxLines.Items[mainWindow.listBoxLines.Items.Count - 1].ToString();
                        indexMove = false;
                        selectedLine = linesSender.Lines.Count - 1;
                        selectedPoint = linesSender.Lines[linesSender.Lines.Count - 1].Points.Count - 1;
                        isSelected = true;
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        //end of line
                        isNew = true;
                        //deselection
                        isSelected = false;
                        indexMove = true;
                        mainWindow.listBoxLines.ClearSelected();
                        mainWindow.textBoxLine.Text = "";
                        indexMove = false;
                        break;
                    default:
                        //show tool tip on MMB
                        mainWindow.imageToolTip.Show(Properties.Resources.MeasuringLines_help, imageSender, e.X, e.Y, 6000);
                        break;
                }
                repaintAllImages();
            }
        }

        /// <summary>
        /// Disallows point's move on mouse button release.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void image_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
        }

        /// <summary>
        /// Adds lines and points to image's paint function.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">graphics context</param>
        private void image_Paint(object sender, PaintEventArgs e)
        {
            //get sender's image
            Emgu.CV.UI.ImageBox image = (Emgu.CV.UI.ImageBox)sender;
            //get sender's lines
            AllLines lines = (AllLines)image.Tag;
            //enable antialiasing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            int i = 0;
            foreach (Line line in lines.Lines)
            {
                //set pen
                Pen pen = new Pen(lineColors[i], thickness);
                //set filling brush for ellipses
                SolidBrush brush = new SolidBrush(lineColors[i]);
                foreach (Point point in line.Points)
                {
                    //draw filled ellipse, ellipse is moved due to drawing from top left corner to its middle
                    Rectangle rec = new Rectangle(point.X - (pointSize.Width / 2), point.Y - (pointSize.Height / 2), pointSize.Width, pointSize.Height);
                    e.Graphics.DrawEllipse(pen, rec);
                    e.Graphics.FillEllipse(brush, rec);
                }
                //if there is more than one ellipse
                if (line.Points.Count > 1)
                {
                    //draw lines between ellipses
                    e.Graphics.DrawLines(pen, line.Points.ToArray());
                }
                //if a point is selected on currently processed line
                if (isSelected && (selectedLine == i))
                {
                    //coords for ellipse's rectangle
                    int x = line.Points[selectedPoint].X - (pointSize.Width / 2);
                    int y = line.Points[selectedPoint].Y - (pointSize.Height / 2);
                    //setting line's opposite color for filling
                    brush.Color = Color.FromArgb(255 - lineColors[i].R, 255 - lineColors[i].G, 255 - lineColors[i].B);
                    e.Graphics.FillEllipse(brush, new Rectangle(x, y, pointSize.Width, pointSize.Height));
                }
                //dispose graphics objects
                pen.Dispose();
                brush.Dispose();
                i++;
            }
        }

        /// <summary>
        /// Allows user to pick line's color.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">click event</param>
        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (mainWindow.colorDialog.ShowDialog() == DialogResult.OK)
                //changes button color according to user's choice
                mainWindow.buttonColor.BackColor = mainWindow.colorDialog.Color;
                //if line is selected change its color
                if (isSelected)
                {
                    lineColors[selectedLine] = mainWindow.buttonColor.BackColor;
                    repaintAllImages();
                }
        }
    }
}