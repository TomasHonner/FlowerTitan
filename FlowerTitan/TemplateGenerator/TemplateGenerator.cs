using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace FlowerTitan.TemplateGenerator
{
    /// <summary>
    /// Singleton class which provides template generation operations.
    /// </summary>
    public class TemplateGenerator
    {
        //singleton private instance
        private static TemplateGenerator templateGenerator = null;

        //stores backup generator number in case of error
        private long templatesCountBackup = 0;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private TemplateGenerator() {}

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <returns>TemplateGenerator instance</returns>
        public static TemplateGenerator GetInstance()
        {
            if (templateGenerator == null)
            {
                templateGenerator = new TemplateGenerator();
            }
            return templateGenerator;
        }

        /// <summary>
        /// Genarates desired count of templates.
        /// </summary>
        /// <param name="templatesCount">count of generated templates</param>
        /// <returns>created pdf</returns>
        public PdfDocument GenerateTemplate(int templatesCount)
        {
            Database.Database db = Database.Database.GetInstance();
            //get lastly used number and set how many templates will be generated to compute a new last number
            long lastNum = db.GetAndSetLastGeneratorNumber(templatesCount);
            templatesCountBackup = -templatesCount;
            PdfDocument pdf = new PdfDocument();
            //initial settings
            pdf.Info.Title = "Template by " + System.Windows.Forms.Application.ProductName;
            pdf.Info.Author = System.Windows.Forms.Application.ProductName + ", " + System.Windows.Forms.Application.CompanyName;
            pdf.Info.CreationDate = DateTime.Now;
            pdf.Info.Creator = System.Windows.Forms.Application.ProductName + " " + System.Windows.Forms.Application.ProductVersion;
            pdf.Info.Keywords = System.Windows.Forms.Application.ProductName + ", template, " + System.Windows.Forms.Application.CompanyName;
            pdf.Info.ModificationDate = DateTime.Now;
            pdf.Info.Subject = "Template";
            pdf.Language = "en-US";
            pdf.SecuritySettings.PermitModifyDocument = false;
            PdfPage pdfPage;
            XGraphics graph = null;
            XFont font = new XFont("Arial", 20, XFontStyle.Bold);
            XPen pen = new XPen(XColors.Black, 3);
            XStringFormat labelFormat = new XStringFormat();
            labelFormat.Alignment = XStringAlignment.Far;
            labelFormat.LineAlignment = XLineAlignment.Center;
            XStringFormat numberFormat = new XStringFormat();
            numberFormat.Alignment = XStringAlignment.Far;
            numberFormat.LineAlignment = XLineAlignment.Near;
            //page size is 595x842
            //rectangle counter
            int counter = 1;
            //text oborder offset
            float offset = 5;
            //margin
            float margin = 30;
            //label space
            float label = 52;
            //space between rectangles x
            float spaceX = 12.5f;
            //space between rectangles y
            float spaceY = 12.5f;
            //label rectangle top
            float lTop = margin;
            //label rectangle bottom
            float lBottom = lTop + label - spaceY;
            //rectangle size
            float rectangle = 170;
            //first rectangle Y top
            float fRYT = margin + label + spaceY;
            //first rectangle Y bottom
            float fRYB = fRYT + rectangle;
            //second rectangle Y top
            float sRYT = fRYB + spaceY;
            //second rectangle Y bottom
            float sRYB = sRYT + rectangle;
            //third rectangle Y top
            float tRYT = sRYB + spaceY;
            //third rectangle Y bottom
            float tRYB = tRYT + rectangle;
            //fourth rectangle Y top
            float foRYT = tRYB + spaceY;
            //fourth rectangle Y bottom
            float foRYB = foRYT + rectangle;
            //first rectangle X L
            float fRXL = margin;
            //first rectangle X R
            float fRXR = fRXL + rectangle;
            //second rectangle X L
            float sRXL = fRXR + spaceX;
            //second rectangle X R
            float sRXR = sRXL + rectangle;
            //third rectangle X L
            float tRXL = sRXR + spaceX;
            //third rectangle X R
            float tRXR = tRXL + rectangle;
            //generate pages
            for (int i = 0; i < templatesCount; i++)
            {
                counter = 1;
                lastNum++;
                pdfPage = pdf.AddPage();
                graph = XGraphics.FromPdfPage(pdfPage);
                //label rectangle
                graph.DrawRectangle(pen, new XRect(new XPoint(tRXL, lTop), new XPoint(tRXR, lBottom)));
                //template number
                graph.DrawString(lastNum.ToString(), font, XBrushes.Black, new XRect(new XPoint(tRXL, lTop), new XPoint(tRXR - 10, lBottom)), labelFormat);
                //first row
                graph.DrawRectangle(pen, new XRect(new XPoint(fRXL, fRYT), new XPoint(fRXR, fRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(fRXL, fRYT + offset), new XPoint(fRXR - offset, fRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(sRXL, fRYT), new XPoint(sRXR, fRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(sRXL, fRYT + offset), new XPoint(sRXR - offset, fRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(tRXL, fRYT), new XPoint(tRXR, fRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(tRXL, fRYT + offset), new XPoint(tRXR - offset, fRYB)), numberFormat);
                //second row
                graph.DrawRectangle(pen, new XRect(new XPoint(fRXL, sRYT), new XPoint(fRXR, sRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(fRXL, sRYT + offset), new XPoint(fRXR - offset, sRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(sRXL, sRYT), new XPoint(sRXR, sRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(sRXL, sRYT + offset), new XPoint(sRXR - offset, sRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(tRXL, sRYT), new XPoint(tRXR, sRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(tRXL, sRYT + offset), new XPoint(tRXR - offset, sRYB)), numberFormat);
                //third row
                graph.DrawRectangle(pen, new XRect(new XPoint(fRXL, tRYT), new XPoint(fRXR, tRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(fRXL, tRYT + offset), new XPoint(fRXR - offset, tRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(sRXL, tRYT), new XPoint(sRXR, tRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(sRXL, tRYT + offset), new XPoint(sRXR - offset, tRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(tRXL, tRYT), new XPoint(tRXR, tRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(tRXL, tRYT + offset), new XPoint(tRXR - offset, tRYB)), numberFormat);
                //fourth row
                graph.DrawRectangle(pen, new XRect(new XPoint(fRXL, foRYT), new XPoint(fRXR, foRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(fRXL, foRYT + offset), new XPoint(fRXR - offset, foRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(sRXL, foRYT), new XPoint(sRXR, foRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(sRXL, foRYT + offset), new XPoint(sRXR - offset, foRYB)), numberFormat);
                graph.DrawRectangle(pen, new XRect(new XPoint(tRXL, foRYT), new XPoint(tRXR, foRYB)));
                graph.DrawString((counter++).ToString(), font, XBrushes.Black, new XRect(new XPoint(tRXL, foRYT + offset), new XPoint(tRXR - offset, foRYB)), numberFormat);
            }
            if (graph != null)
            {
                graph.Dispose();
            }
            return pdf;
        }

        /// <summary>
        /// Saves generated document to a pdf file and opens it.
        /// </summary>
        /// <param name="pdf">pdf document</param>
        /// <param name="path">path where to save it</param>
        public void SavePdf(PdfDocument pdf, string path)
        {
            try
            {
                pdf.Save(path);
                //open document with default viewer
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(Properties.Resources.TemplateGenerator_error_text + e.Message, Properties.Resources.TemplateGenerator_error_title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Database.Database db = Database.Database.GetInstance();
                //rollback generator counter
                db.GetAndSetLastGeneratorNumber(templatesCountBackup);
            }
        }

        /// <summary>
        /// Gets lastly used path for user dialog from DB.
        /// </summary>
        /// <returns>path</returns>
        public string GetSaveFilePath()
        {
            Database.Database db = Database.Database.GetInstance();
            return db.GetSaveFilePath();
        }

        /// <summary>
        /// Sets new user's path to DB.
        /// </summary>
        /// <param name="path">new path</param>
        public void SetSaveFilePath(string path)
        {
            Database.Database db = Database.Database.GetInstance();
            db.SetSaveFilePath(path);
        }
    }
}
