using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKVPdfApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Select_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Chọn thư mục chứa file PDF";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.SelectedPath;
                    LoadPdfFiles(dialog.SelectedPath);
                }
            }
        }

        private void LoadPdfFiles(string folderPath)
        {
            try
            {
                dataGridView1.Rows.Clear();

                string[] pdfFiles = Directory.GetFiles(
                    folderPath,
                    "*.pdf",
                    SearchOption.TopDirectoryOnly
                );

                foreach (string filePath in pdfFiles)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    int rowIndex = dataGridView1.Rows.Add();

                    dataGridView1.Rows[rowIndex].Cells["colSelected"].Value = false;
                    dataGridView1.Rows[rowIndex].Cells["colFileName"].Value = fileInfo.Name;
                    dataGridView1.Rows[rowIndex].Cells["colFileSize"].Value = FormatFileSize(fileInfo.Length);
                    dataGridView1.Rows[rowIndex].Cells["colStatus"].Value = "Ready";
                    dataGridView1.Rows[rowIndex].Tag = filePath;
                }

                progressBar.Value = 0;

                MessageBox.Show(
                    $"Đã tìm thấy {pdfFiles.Length} file PDF.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể đọc thư mục:\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes >= 1024 * 1024)
            {
                return $"{bytes / 1024.0 / 1024.0:F2} MB";
            }

            if (bytes >= 1024)
            {
                return $"{bytes / 1024.0:F2} KB";
            }

            return $"{bytes} B";
        }

        private List<string> GetSelectedPdfFiles()
        {
            List<string> files = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool selected = Convert.ToBoolean(
                    row.Cells["colSelected"].Value ?? false
                );

                if (selected && row.Tag is string filePath)
                {
                    files.Add(filePath);
                }
            }

            return files;
        }

        private async void ExtractPDF_Click(
            object sender,
            EventArgs e)
        {
            List<string> selectedFiles =
                GetSelectedPdfFiles();

            if (selectedFiles.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn ít nhất một file PDF.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            ExtractPDF.Enabled = false;
            btnSelect.Enabled = false;

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            try
            {
                List<byte[]> allData =
                    new List<byte[]>();

                PdfExtractor extractor =
                    new PdfExtractor();

                for (int i = 0; i < selectedFiles.Count; i++)
                {
                    string filePath =
                        selectedFiles[i];

                    UpdateFileStatus(
                        filePath,
                        "Processing..."
                    );

                    List<byte[]> fileData =
                        await Task.Run(
                            () => extractor.Extract(filePath)
                        );

                    List<byte[]> uniqueFileData =
                        RemoveDuplicates(fileData);

                    foreach (byte[] data in uniqueFileData)
                    {
                        if (!ContainsData(allData, data))
                        {
                            allData.Add(data);
                        }
                    }

                    UpdateFileStatus(
                        filePath,
                        $"Completed - {uniqueFileData.Count} data"
                    );

                    progressBar.Value =
                        (i + 1) * 100 /
                        selectedFiles.Count;
                }

                if (allData.Count == 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy dữ liệu nào thỏa điều kiện Min >= 25 và Max <= 200.",
                        "Kết quả",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    return;
                }

                string folderPath =
                    Path.GetDirectoryName(
                        selectedFiles[0]
                    ) ?? "";

                string outputFolder =
                    Path.Combine(
                        folderPath,
                        "Output"
                    );

                Directory.CreateDirectory(
                    outputFolder
                );

                string outputFile =
                    Path.Combine(
                        outputFolder,
                        "Output.pdf"
                    );

                PdfExporter exporter =
                    new PdfExporter();

                await Task.Run(
                    () => exporter.Export(
                        allData,
                        outputFile
                    )
                );

                MessageBox.Show(
                    $"Đã hoàn tất.\n\n" +
                    $"Tổng dữ liệu duy nhất: {allData.Count}\n\n" +
                    $"File kết quả:\n{outputFile}",
                    "Hoàn tất",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Có lỗi xảy ra:\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                ExtractPDF.Enabled = true;
                btnSelect.Enabled = true;
            }
        }

        private List<byte[]> RemoveDuplicates(
            List<byte[]> dataList)
        {
            List<byte[]> result =
                new List<byte[]>();

            foreach (byte[] data in dataList)
            {
                if (!ContainsData(result, data))
                {
                    result.Add(data);
                }
            }

            return result;
        }

        private bool ContainsData(
            List<byte[]> list,
            byte[] data)
        {
            foreach (byte[] item in list)
            {
                if (item.SequenceEqual(data))
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateFileStatus(
            string filePath,
            string status)
        {
            foreach (DataGridViewRow row
                in dataGridView1.Rows)
            {
                if (row.Tag is string path &&
                    string.Equals(
                        path,
                        filePath,
                        StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["colStatus"].Value =
                        status;

                    break;
                }
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(
            object sender,
            EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(
                    DataGridViewDataErrorContexts.Commit
                );
            }
        }
    }
}