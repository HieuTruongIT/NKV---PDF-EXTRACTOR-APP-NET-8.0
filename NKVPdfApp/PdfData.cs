namespace NKVPdfApp
{
    public class PdfData
    {
        public string FileName { get; set; }
        public int PageNumber { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public byte[] Data { get; set; }

        public PdfData()
        {
            FileName = string.Empty;
            Data = System.Array.Empty<byte>();
        }
    }
}