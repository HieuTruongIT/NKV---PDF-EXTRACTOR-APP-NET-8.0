using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace NKVPdfApp
{
    public class PdfExporter
    {
        public void Export(
            List<byte[]> data,
            string outputFile)
        {
            if (data == null || data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không có dữ liệu để xuất."
                );
            }

            PdfDocumentBuilder builder = new PdfDocumentBuilder();

            PdfDocumentBuilder.AddedFont font =
                builder.AddStandard14Font(
                    Standard14Font.Helvetica
                );

            PdfPageBuilder page = builder.AddPage(
                595,
                842
            );

            float y = 800;

            foreach (byte[] item in data)
            {
                string text = Encoding.UTF8.GetString(item);

                if (text.Length > 90)
                {
                    text = text.Substring(0, 90);
                }

                page.AddText(
                    text,
                    10,
                    new UglyToad.PdfPig.Core.PdfPoint(30, y),
                    font
                );

                y -= 20;

                if (y < 40)
                {
                    page = builder.AddPage(
                        595,
                        842
                    );

                    y = 800;
                }
            }

            byte[] pdfBytes = builder.Build();

            Directory.CreateDirectory(
                Path.GetDirectoryName(outputFile) ?? ""
            );

            File.WriteAllBytes(
                outputFile,
                pdfBytes
            );
        }
    }
}