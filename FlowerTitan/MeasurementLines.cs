using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using FlowerTitan.MeasurementLines;
using System.Drawing;

namespace FlowerTitan
{
    /// <summary>
    /// Partial class for MainWindow which includes event handlers used for measuring lines.
    /// </summary>
    public partial class MainWindow : Form
    {
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
        //holds reference to the selected line
        private int selectedLine = 0;
        //holds reference to the selected point
        private int selectedPoint = 0;
        //holds sender's image
        private Emgu.CV.UI.ImageBox imageSender;
        //holds sender's image's lines
        private AllLines linesSender;
        //holds all lines of all images which are used by measuring lines
        private List<AllLines> alllines = new List<AllLines>();
        //holds all images which are used by measuring lines
        private List<Emgu.CV.UI.ImageBox> allImages = new List<Emgu.CV.UI.ImageBox>();

        /// <summary>
        /// Creates deep copy of an AllLines object.
        /// </summary>
        /// <returns>Deep copy of the very first image's lines.</returns>
        private AllLines getMeasuringLines()
        {
            AllLines deepCopy = new AllLines();
            //copy line's counter
            deepCopy.LinesCounter = alllines[0].LinesCounter;
            int i = 0;
            foreach (Line l in alllines[0].Lines)
            {
                //copy every line
                deepCopy.Lines.Add(new Line(alllines[0].Lines[i].Color));
                //copy line's name
                deepCopy.Lines[i].Name = alllines[0].Lines[i].Name;
                int j = 0;
                foreach (Point p in alllines[0].Lines[i].Points)
                {
                    //copy every line's point
                    deepCopy.Lines[i].Points.Add(new Point(alllines[0].Lines[i].Points[j].X, alllines[0].Lines[i].Points[j].Y));
                    j++;
                }
                i++;
            }
            return deepCopy;
        }

        /// <summary>
        /// Adds needed event handlers to image and set its needed properties.
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
            //disable right click context menu
            imageBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            //add to working collection
            alllines.Add((AllLines)imageBox.Tag);
            allImages.Add(imageBox);
            //force new line (in case line wasn't ended before processing)
            isNew = true;
            //force line drawing
            imageBox.Refresh();
        }

        /// <summary>
        /// Prepare measuring lines function.
        /// </summary>
        private void prepareMeasuringLines(Emgu.CV.UI.ImageBox imageBox)
        {
            this.KeyDown += MainWindow_KeyDown;
            listBoxLines.SelectedIndexChanged += listBoxLines_SelectedIndexChanged;
            textBoxLine.TextChanged += textBoxLine_TextChanged;
            //addition of the very first image
            imageBox.Tag = new AllLines();
            addHandlersToImage(imageBox);
            //set color picker color
            buttonColor.BackColor = Color.Black;
        }

        /// <summary>
        /// Prepare images and logic for a new template.
        /// </summary>
        private void prepareForNewTemplate()
        {
            //reseting listBoxe's items
            isMoving = true;
            listBoxLines.Items.Clear();
            listBoxLines.ClearSelected();
            isMoving = false;
            imageSender = allImages[0];
            //removing all added event handlers and measurement lines
            foreach (Emgu.CV.UI.ImageBox iB in allImages)
            {
                iB.MouseEnter -= image_MouseEnter;
                iB.MouseMove -= image_MouseMove;
                iB.MouseDown -= image_MouseDown;
                iB.MouseClick -= image_MouseClick;
                iB.MouseUp -= image_MouseUp;
                iB.Paint -= image_Paint;
                iB.Tag = null;
            }
            //clearing working collection set
            allImages.Clear();
            alllines.Clear();
            //recreating settings for the first image
            imageSender.Tag = new AllLines();
            addHandlersToImage(imageSender);
            //reseting settings to default
            buttonColor.BackColor = Color.Black;
            isOnPoint = false;
            isMoving = false;
            isSelected = false;
            isNew = true;
            indexMove = false;
            selectedLine = 0;
            selectedPoint = 0;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void loadMeasuringLinesFromDB()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler for line's name text box.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        private void textBoxLine_TextChanged(object sender, EventArgs e)
        {
            //prevents exception during renaming
            if (!indexMove)
            {
                //allows projection of line's name changes to list box
                indexMove = true;
                listBoxLines.Items[selectedLine] = textBoxLine.Text;
                linesSender.Lines[selectedLine].Name = textBoxLine.Text;
                indexMove = false;
            }
        }

        /// <summary>
        /// Event handler for list box holding line's names.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">change event</param>
        void listBoxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            //prevents IndexOutOfRange exception while deleting and renaming lines
            if (!indexMove)
            {
                //set selected line and point
                selectedLine = listBoxLines.SelectedIndex;
                selectedPoint = linesSender.Lines[listBoxLines.SelectedIndex].Points.Count - 1;
                isSelected = true;
                //projection of name to text box
                textBoxLine.Text = listBoxLines.Items[selectedLine].ToString();
                isNew = true;
                //forced refresh to display line's selection
                imageSender.Refresh();
            }
        }

        /// <summary>
        /// Event handler which serves key's presses on an active form - main window
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">key event</param>
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //prevent deleting line/point if nothing is selected or text box has focus - letters deleting
            if (isSelected && !textBoxLine.Focused)
            {
                switch (e.KeyCode)
                {
                    //delete selected point
                    case Keys.Delete:
                        //delete selected point on every image
                        foreach (AllLines al in alllines)
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
                //repaint every working image
                foreach (Emgu.CV.UI.ImageBox iB in allImages)
                {
                    iB.Refresh();
                }
            }
        }

        /// <summary>
        /// Delete whole line.
        /// </summary>
        private void deleteLine()
        {
            //force a new line if current wasn't ended
            isNew = true;
            //remove line from every working image
            foreach (AllLines al in alllines)
            {
                al.Lines.RemoveAt(selectedLine);
            }
            //change selected line
            indexMove = true;
            listBoxLines.Items.RemoveAt(selectedLine);
            //select the last line in set
            listBoxLines.SelectedIndex = linesSender.Lines.Count - 1;
            //change text in text box
            if (listBoxLines.Items.Count > 0)
            {
                textBoxLine.Text = listBoxLines.Items[linesSender.Lines.Count - 1].ToString();
            }
            else
            {
                //if there is no line left
                textBoxLine.Text = "";
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
        void image_MouseEnter(object sender, EventArgs e)
        {
            //get sender's image
            imageSender = (Emgu.CV.UI.ImageBox)sender;
            //get sender's lines
            linesSender = (AllLines)imageSender.Tag;
        }

        /// <summary>
        /// Changes mouse's cursor on points to Sizeable.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        void image_MouseMove(object sender, MouseEventArgs e)
        {
            //if left button is pressed, move with line
            if (isMoving)
            {
                //check whether cursor is on image, not elswhere
                if (checkImageBounds(e.X, e.Y))
                {
                    //move with point, subsequently with line
                    linesSender.Lines[selectedLine].Points[selectedPoint] = new Point(e.X, e.Y);
                    //force refresh to changes can be visible
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
            if (((x >= 0) && (x <= (imageSender.Width - (linesSender.Size.Width / 2)))) && ((y >= 0) && (y <= (imageSender.Height - (linesSender.Size.Height / 2)))))
            {
                isInImage = true;
            }
            return isInImage;
        }

        /// <summary>
        /// Checks whether the cursor is on a point.
        /// Returns if point is found to the global variable.
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
                    int offsetX = linesSender.Size.Width / 2;
                    int offsetY = linesSender.Size.Height / 2;
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
        /// Allows point's move.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        void image_MouseDown(object sender, MouseEventArgs e)
        {
            //cursor is on point and left mouse button is pressed
            if (isOnPoint && (e.Button == System.Windows.Forms.MouseButtons.Left))
            {
                isMoving = true;
                isSelected = true;
                //changes color picker's color according to the selected line
                buttonColor.BackColor = linesSender.Lines[selectedLine].Color;
                indexMove = true;
                //select according line in list box
                listBoxLines.SelectedIndex = selectedLine;
                //set text in text box
                textBoxLine.Text = listBoxLines.Items[selectedLine].ToString();
                indexMove = false;
            }
            else
            {
                isMoving = false;
                isSelected = false;
            }
            //image repainting due to line selecting/deselecting on every image in working set
            foreach (Emgu.CV.UI.ImageBox iB in allImages)
            {
                iB.Refresh();
            }
        }

        /// <summary>
        /// Adds lines functions to mouse buttons.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        void image_MouseClick(object sender, MouseEventArgs e)
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
                            foreach (AllLines al in alllines)
                            {
                                al.Lines.Add(new Line(buttonColor.BackColor));
                                al.LinesCounter++;
                                //counter reset
                                if (al.LinesCounter == 100)
                                {
                                    al.LinesCounter = 0;
                                }
                            }
                            //adding line to list box and creating default name
                            listBoxLines.Items.Add("Line " + (linesSender.LinesCounter).ToString());
                            isNew = false;
                        }
                        //add line's point to every image in working set and set color
                        foreach (AllLines al in alllines)
                        {
                            //set current line's color
                            al.Lines[linesSender.Lines.Count - 1].Color = buttonColor.BackColor;
                            //new point
                            al.Lines[linesSender.Lines.Count - 1].Points.Add(new Point(e.X, e.Y));
                        }
                        //selecting added line
                        indexMove = true;
                        listBoxLines.SelectedIndex = listBoxLines.Items.Count - 1;
                        textBoxLine.Text = listBoxLines.Items[listBoxLines.Items.Count - 1].ToString();
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
                        listBoxLines.ClearSelected();
                        textBoxLine.Text = "";
                        indexMove = false;
                        break;
                    default:
                        //show tool tip on MMB
                        imageToolTip.Show("LMB - new point/new line/select point\nRMB - end of line/deselect\nLMB pressed - move point\nDEL - delete current point\nBACKSPACE - delete whole line", this, e.X, e.Y, 6000);
                        break;
                }
                //image repainting for every image in working set
                foreach (Emgu.CV.UI.ImageBox iB in allImages)
                {
                    iB.Refresh();
                }
            }
        }

        /// <summary>
        /// Disallow point's move on mouse button release.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        void image_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
        }

        /// <summary>
        /// Adds lines and points to paint function.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">graphics context</param>
        void image_Paint(object sender, PaintEventArgs e)
        {
            //get sender's image
            Emgu.CV.UI.ImageBox image = (Emgu.CV.UI.ImageBox)sender;
            //get sender's lines
            AllLines lines = (AllLines)image.Tag;
            //enable antialiasing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            int l = 0;
            foreach (Line line in lines.Lines)
            {
                //set pen
                Pen pen = new Pen(line.Color, lines.Thickness);
                //set filling brush for ellipses
                SolidBrush brush = new SolidBrush(line.Color);
                foreach (Point point in line.Points)
                {
                    //draw filled ellipse, ellipse is moved due to drawing from top left corner to its middle
                    Rectangle rec = new Rectangle(point.X - (lines.Size.Width / 2), point.Y - (lines.Size.Height / 2), lines.Size.Width, lines.Size.Height);
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
                if (isSelected && (selectedLine == l))
                {
                    //coords for ellipse's rectangle
                    int x = line.Points[selectedPoint].X - (lines.Size.Width / 2);
                    int y = line.Points[selectedPoint].Y - (lines.Size.Height / 2);
                    //setting line's opposite color for filling
                    brush.Color = Color.FromArgb(255 - line.Color.R, 255 - line.Color.G, 255 - line.Color.B);
                    e.Graphics.FillEllipse(brush, new Rectangle(x, y, lines.Size.Width, lines.Size.Height));
                }
                //dispose graphics objects
                pen.Dispose();
                brush.Dispose();
                l++;
            }
        }

        /// <summary>
        /// Allows user to pick line's color.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">click event</param>
        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                //changes button color according to user's choice
                buttonColor.BackColor = colorDialog.Color;
                //if line is selected
                if (isSelected)
                {
                    //repaint selected line on every image in working set
                    foreach (AllLines al in alllines)
                    {
                        al.Lines[selectedLine].Color = buttonColor.BackColor;
                    }
                    //make repainting visible
                    foreach (Emgu.CV.UI.ImageBox iB in allImages)
                    {
                        iB.Refresh();
                    }
                }
            }
        }
}
