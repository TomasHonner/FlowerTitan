using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace FlowerTitan.TemplateOCR
{
    /// <summary>
    /// Singleton class which handle template id OCR.
    /// </summary>
    public class TemplateIdOCR
    {
        //singleton instance
        private static TemplateIdOCR templateIdOCR = null;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private TemplateIdOCR() { }

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <returns>TemplateIdOCR instance.</returns>
        public static TemplateIdOCR GetInstance()
        {
            if (templateIdOCR == null)
            {
                templateIdOCR = new TemplateIdOCR();
            }
            return templateIdOCR;
        }

        public string ProcessTemplateID(Bitmap image)
        {
            TesseractEngine engine = new TesseractEngine(@"./tessdata", "eng");
            engine.DefaultPageSegMode = PageSegMode.SingleLine;
            engine.SetVariable("tessedit_char_whitelist", "0123456789");
            engine.SetVariable("textord_tabfind_vertical_text", false);
            engine.SetVariable("classify_font_name", "Arial.ttf");
            engine.SetVariable("classify_enable_learning", false);
            Page page = engine.Process(image);
            string output = page.GetText();
            string result = "";
            foreach (char c in output)
            {
                bool isNum = false;
                for (int i = 0; i < 10; i++)
                {
                    if (c.ToString() == i.ToString())
                    {
                        isNum = true;
                        break;
                    }
                }
                if (isNum) result += c;
            }
            if (result == "") result = "NaN";
            return result;
        }
    }
}
