namespace NKVPdfApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            richTextBox1 = new RichTextBox();
            btnSelect = new Button();
            dataGridView1 = new DataGridView();
            ExtractPDF = new Button();
            progressBar = new ProgressBar();
            colSelected = new DataGridViewCheckBoxColumn();
            colFileName = new DataGridViewTextBoxColumn();
            colFileSize = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();

            textBox1.Location = new Point(65, 69);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(592, 27);
            textBox1.TabIndex = 0;

            richTextBox1.BackColor = SystemColors.Highlight;
            richTextBox1.ForeColor = SystemColors.InactiveBorder;
            richTextBox1.Location = new Point(275, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(253, 31);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "NKV COMPANY - PDF Data Extractor";

            btnSelect.Location = new Point(663, 68);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(94, 29);
            btnSelect.TabIndex = 3;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += Select_Click;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                colSelected,
                colFileName,
                colFileSize,
                colStatus
            });
            dataGridView1.Location = new Point(65, 115);
            dataGridView1.MultiSelect = true;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(692, 178);
            dataGridView1.TabIndex = 4;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;

            ExtractPDF.Location = new Point(663, 325);
            ExtractPDF.Name = "ExtractPDF";
            ExtractPDF.Size = new Size(94, 29);
            ExtractPDF.TabIndex = 5;
            ExtractPDF.Text = "Extract PDF";
            ExtractPDF.UseVisualStyleBackColor = true;
            ExtractPDF.Click += ExtractPDF_Click;

            progressBar.Location = new Point(65, 325);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(592, 29);
            progressBar.TabIndex = 6;

            colSelected.HeaderText = "Select";
            colSelected.MinimumWidth = 6;
            colSelected.Name = "colSelected";
            colSelected.Width = 60;

            colFileName.HeaderText = "File Name";
            colFileName.MinimumWidth = 6;
            colFileName.Name = "colFileName";
            colFileName.Width = 250;

            colFileSize.HeaderText = "Size";
            colFileSize.MinimumWidth = 6;
            colFileSize.Name = "colFileSize";
            colFileSize.Width = 150;

            colStatus.HeaderText = "Status";
            colStatus.MinimumWidth = 6;
            colStatus.Name = "colStatus";
            colStatus.Width = 160;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);

            Controls.Add(progressBar);
            Controls.Add(ExtractPDF);
            Controls.Add(dataGridView1);
            Controls.Add(btnSelect);
            Controls.Add(richTextBox1);
            Controls.Add(textBox1);

            Name = "MainForm";
            Text = "NKV PDF Data Extractor";

            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBox1;
        private RichTextBox richTextBox1;
        private Button btnSelect;
        private DataGridView dataGridView1;
        private Button ExtractPDF;
        private ProgressBar progressBar;
        private DataGridViewCheckBoxColumn colSelected;
        private DataGridViewTextBoxColumn colFileName;
        private DataGridViewTextBoxColumn colFileSize;
        private DataGridViewTextBoxColumn colStatus;
    }
}