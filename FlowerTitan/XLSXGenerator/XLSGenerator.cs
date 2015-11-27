using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlowerTitan.MeasuringLines;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace FlowerTitan.XLSXGenerator
{
    /// <summary>
    /// Singleton class which generates XLSX output file.
    /// </summary>
    public class XLSXGenerator
    {
        //singleton instance
        private static XLSXGenerator xlsxGenerator = null;

        private LengthConverter.LengthConverter lengthConverter;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private XLSXGenerator()
        {
            lengthConverter = LengthConverter.LengthConverter.GetInstance();
        }

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <returns>XLSGenerator instance.</returns>
        public static XLSXGenerator GetInstance()
        {
            if (xlsxGenerator == null)
            {
                xlsxGenerator = new XLSXGenerator();
            }
            return xlsxGenerator;
        }

        /// <summary>
        /// Exports measurements into XLSX file to default specified path.
        /// </summary>
        /// <param name="tID">template id</param>
        /// <param name="tName">template name</param>
        /// <param name="names">all lines' names</param>
        /// <param name="allLines">all lines of all images</param>
        /// <param name="savePath">default save path</param>
        /// <param name="scale">template's scale</param>
        public void ExportData(string tID, string tName, string[] names, AllLines[] allLines, string savePath, double scale)
        {
            try
            {
                string path = savePath + @"\" + tID + ".xlsx";
                FileInfo newFile = new FileInfo(path);
                Color top = Color.FromArgb(100, 100, 100);
                Color header = Color.FromArgb(170, 170, 170);
                Color line = Color.FromArgb(230, 230, 230);
                int length = 5;
                if (names.Length == 0) length += 1;
                else length += names.Length;
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path);
                }
                ExcelPackage package = new ExcelPackage(newFile);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(tID);

                package.Workbook.Properties.Title = tID;
                package.Workbook.Properties.Author = System.Windows.Forms.Application.ProductName;
                package.Workbook.Properties.Subject = tName;
                package.Workbook.Properties.Application = System.Windows.Forms.Application.ProductName + " " + System.Windows.Forms.Application.ProductVersion;
                package.Workbook.Properties.Company = System.Windows.Forms.Application.CompanyName;
                package.Workbook.Properties.Created = DateTime.Now;
                package.Workbook.Properties.Keywords = System.Windows.Forms.Application.ProductName + ", " + System.Windows.Forms.Application.CompanyName;

                worksheet.HeaderFooter.OddHeader.LeftAlignedText = tID;
                worksheet.HeaderFooter.OddHeader.RightAlignedText = ExcelHeaderFooter.CurrentDate + " " + ExcelHeaderFooter.CurrentTime;
                worksheet.HeaderFooter.OddFooter.RightAlignedText = string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName + ".xlsx";

                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                worksheet.PrinterSettings.HorizontalCentered = true;

                worksheet.Cells["A:XFD"].Style.Font.Name = "Calibri";
                worksheet.Cells["A:XFD"].Style.Font.Size = 12f;
                
                worksheet.Row(1).Height = 30;
                worksheet.Row(2).Height = 30;
                worksheet.Row(4).Height = 25;
                worksheet.Row(5).Height = 25;
                worksheet.Column(1).Width = 4;
                worksheet.Column(2).Width = 20;

                worksheet.Cells["A1:N2"].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                worksheet.Cells["A1:B1"].Merge = true;
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["C1:N1"].Merge = true;
                worksheet.Cells["C2:N2"].Merge = true;
                worksheet.Cells["A1:N1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:N2"].Style.Font.Bold = true;
                worksheet.Cells["A1:N2"].Style.Font.Size = 15f;
                worksheet.Cells["A1:A2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:A2"].Style.Fill.BackgroundColor.SetColor(top);
                worksheet.Cells["C1:N2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C1:N2"].Style.Fill.BackgroundColor.SetColor(header);
                worksheet.Cells["B1:B2"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["A1:N2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["C1:N2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["C1:N2"].Style.Font.Italic = true;

                worksheet.Cells[4, 1, length, 14].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                worksheet.Cells[6, 1, length, 1].Merge = true;
                worksheet.Cells["A4:B4"].Merge = true;
                worksheet.Cells["A5:B5"].Merge = true;
                worksheet.Cells["C4:N4"].Merge = true;
                worksheet.Cells["A6:A6"].Style.TextRotation = 90;
                worksheet.Cells["A6:A6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A6:A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A4:A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A4:A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5:A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C4:N5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["C4:N5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B4:B4"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["B5:B5"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["A5:B5"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["C4:N4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["C5:N5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["C4:N5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C4:N5"].Style.Fill.BackgroundColor.SetColor(header);
                worksheet.Cells["A4:B5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4:B5"].Style.Fill.BackgroundColor.SetColor(top);
                worksheet.Cells[6, 1, length, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[6, 1, length, 1].Style.Fill.BackgroundColor.SetColor(header);
                worksheet.Cells[6, 1, length, 1].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["C5:M5"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A6:A9"].Style.Font.Bold = true;
                worksheet.Cells["A4:N5"].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = Properties.Resources.XLSXGenerator_id;
                worksheet.Cells["C1:C1"].Value = int.Parse(tID);
                worksheet.Cells[2, 1].Value = Properties.Resources.XLSXGenerator_name;
                worksheet.Cells["C2:C2"].Value = tName;
                worksheet.Cells[4, 1].Value = Properties.Resources.XLSXGenerator_lengths;
                worksheet.Cells[5, 1].Value = Properties.Resources.XLSXGenerator_mm;
                worksheet.Cells[4, 3].Value = Properties.Resources.XLSXGenerator_flower;
                worksheet.Cells[6, 1].Value = Properties.Resources.XLSXGenerator_lines;

                for (int i = 3, j = 1; i < 15; i++, j++)
                {
                    worksheet.Cells[5, i].Value = j;
                    worksheet.Column(i).Width = 8;
                }
                for (int i = 0, j = 6; i < names.Length; i++, j++)
                {
                    worksheet.Cells[j, 2].Value = names[i];
                    worksheet.Cells[j, 2, j, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[j, 2].Style.WrapText = true;
                    if (j % 2 == 1)
                    {
                        worksheet.Cells[j, 2, j, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[j, 2, j, 14].Style.Fill.BackgroundColor.SetColor(line);
                    }
                    for (int k = 2; k < 14; k++)
                    {
                        worksheet.Cells[j, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }
                }
                foreach (AllLines al in allLines)
                {
                    int row = 6;
                    foreach (Line l in al.Lines)
                    {
                        int col = (int)al.ImageBoxID + 2;
                        worksheet.Cells[row, col].Value = Math.Round(lengthConverter.ConvertLineLengthToMM(l, scale), 2);
                        worksheet.Cells[row, col].Style.Numberformat.Format = "0.00";
                        row++;
                    }
                }

                package.Save();
            }
            catch (Exception e)
            {
                showError(e);
            }
        }

        private void showError(Exception e)
        {
            System.Windows.Forms.MessageBox.Show(Properties.Resources.XLSXGenerator_error_text + "\n" + e.Message, Properties.Resources.XLSXGenerator_error_title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
