using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace NKVPdfApp
{
    public class PdfExtractor
    {
        private readonly Regex minRegex = new Regex(
            @"\bmin\b\s*[:=]?\s*(-?\d+(?:[.,]\d+)?)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        private readonly Regex maxRegex = new Regex(
            @"\bmax\b\s*[:=]?\s*(-?\d+(?:[.,]\d+)?)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        public List<byte[]> Extract(string filePath)
        {
            List<byte[]> result = new List<byte[]>();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file PDF.",
                    filePath
                );
            }

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    string pageText = page.Text;

                    if (string.IsNullOrWhiteSpace(pageText))
                    {
                        continue;
                    }

                    List<PdfData> pageData = ExtractPageData(
                        Path.GetFileName(filePath),
                        page.Number,
                        pageText
                    );

                    foreach (PdfData item in pageData)
                    {
                        result.Add(item.Data);
                    }
                }
            }

            return RemoveDuplicates(result);
        }

        private List<PdfData> ExtractPageData(
            string fileName,
            int pageNumber,
            string pageText)
        {
            List<PdfData> result = new List<PdfData>();

            string[] lines = pageText
                .Split(
                    new[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                );

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (line.Length == 0)
                {
                    continue;
                }

                Match minMatch = minRegex.Match(line);
                Match maxMatch = maxRegex.Match(line);

                if (!minMatch.Success || !maxMatch.Success)
                {
                    continue;
                }

                if (!TryParseNumber(minMatch.Groups[1].Value, out double min))
                {
                    continue;
                }

                if (!TryParseNumber(maxMatch.Groups[1].Value, out double max))
                {
                    continue;
                }

                if (min >= 25 && max <= 200)
                {
                    byte[] data = Encoding.UTF8.GetBytes(line);

                    result.Add(
                        new PdfData
                        {
                            FileName = fileName,
                            PageNumber = pageNumber,
                            Min = min,
                            Max = max,
                            Data = data
                        }
                    );
                }
            }

            return result;
        }

        private bool TryParseNumber(string value, out double number)
        {
            value = value.Replace(',', '.');

            return double.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out number
            );
        }

        private List<byte[]> RemoveDuplicates(List<byte[]> dataList)
        {
            List<byte[]> result = new List<byte[]>();

            foreach (byte[] data in dataList)
            {
                bool exists = result.Any(
                    x => x.SequenceEqual(data)
                );

                if (!exists)
                {
                    result.Add(data);
                }
            }

            return result;
        }
    }
}