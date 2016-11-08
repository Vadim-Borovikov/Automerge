namespace AutomergerDemo.View
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLoadSource = new System.Windows.Forms.Button();
            this.buttonLoadVersion1 = new System.Windows.Forms.Button();
            this.buttonLoadVersion2 = new System.Windows.Forms.Button();
            this.buttonMerge = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelChanged1 = new System.Windows.Forms.Label();
            this.labelChanged2 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.labelMerge = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadSource
            // 
            this.buttonLoadSource.Location = new System.Drawing.Point(12, 12);
            this.buttonLoadSource.Name = "buttonLoadSource";
            this.buttonLoadSource.Size = new System.Drawing.Size(88, 23);
            this.buttonLoadSource.TabIndex = 0;
            this.buttonLoadSource.Text = "Load Source";
            this.buttonLoadSource.UseVisualStyleBackColor = true;
            this.buttonLoadSource.Click += new System.EventHandler(this.buttonLoadSource_Click);
            // 
            // buttonLoadVersion1
            // 
            this.buttonLoadVersion1.Location = new System.Drawing.Point(12, 41);
            this.buttonLoadVersion1.Name = "buttonLoadVersion1";
            this.buttonLoadVersion1.Size = new System.Drawing.Size(88, 23);
            this.buttonLoadVersion1.TabIndex = 1;
            this.buttonLoadVersion1.Text = "Load Version 1";
            this.buttonLoadVersion1.UseVisualStyleBackColor = true;
            this.buttonLoadVersion1.Click += new System.EventHandler(this.buttonLoadVersion1_Click);
            // 
            // buttonLoadVersion2
            // 
            this.buttonLoadVersion2.Location = new System.Drawing.Point(12, 70);
            this.buttonLoadVersion2.Name = "buttonLoadVersion2";
            this.buttonLoadVersion2.Size = new System.Drawing.Size(88, 23);
            this.buttonLoadVersion2.TabIndex = 2;
            this.buttonLoadVersion2.Text = "Load Version 2";
            this.buttonLoadVersion2.UseVisualStyleBackColor = true;
            this.buttonLoadVersion2.Click += new System.EventHandler(this.buttonLoadVersion2_Click);
            // 
            // buttonMerge
            // 
            this.buttonMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMerge.Location = new System.Drawing.Point(12, 135);
            this.buttonMerge.Name = "buttonMerge";
            this.buttonMerge.Size = new System.Drawing.Size(102, 23);
            this.buttonMerge.TabIndex = 3;
            this.buttonMerge.Text = "Merge";
            this.buttonMerge.UseVisualStyleBackColor = true;
            this.buttonMerge.Click += new System.EventHandler(this.buttonMerge_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(106, 17);
            this.labelSource.MaximumSize = new System.Drawing.Size(0, 13);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(63, 13);
            this.labelSource.TabIndex = 4;
            this.labelSource.Text = "labelSource";
            // 
            // labelChanged1
            // 
            this.labelChanged1.AutoSize = true;
            this.labelChanged1.Location = new System.Drawing.Point(106, 46);
            this.labelChanged1.MaximumSize = new System.Drawing.Size(852, 13);
            this.labelChanged1.Name = "labelChanged1";
            this.labelChanged1.Size = new System.Drawing.Size(78, 13);
            this.labelChanged1.TabIndex = 5;
            this.labelChanged1.Text = "labelChanged1";
            // 
            // labelChanged2
            // 
            this.labelChanged2.AutoSize = true;
            this.labelChanged2.Location = new System.Drawing.Point(106, 75);
            this.labelChanged2.MaximumSize = new System.Drawing.Size(852, 13);
            this.labelChanged2.Name = "labelChanged2";
            this.labelChanged2.Size = new System.Drawing.Size(78, 13);
            this.labelChanged2.TabIndex = 6;
            this.labelChanged2.Text = "labelChanged2";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // labelMerge
            // 
            this.labelMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMerge.AutoSize = true;
            this.labelMerge.Location = new System.Drawing.Point(120, 140);
            this.labelMerge.MaximumSize = new System.Drawing.Size(852, 13);
            this.labelMerge.Name = "labelMerge";
            this.labelMerge.Size = new System.Drawing.Size(59, 13);
            this.labelMerge.TabIndex = 7;
            this.labelMerge.Text = "labelMerge";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 161);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(206, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(100, 17);
            this.toolStripStatusLabel.Text = "Ready for loading";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(206, 183);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.labelMerge);
            this.Controls.Add(this.labelChanged2);
            this.Controls.Add(this.labelChanged1);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.buttonMerge);
            this.Controls.Add(this.buttonLoadVersion2);
            this.Controls.Add(this.buttonLoadVersion1);
            this.Controls.Add(this.buttonLoadSource);
            this.MaximumSize = new System.Drawing.Size(1024, 2000);
            this.MinimumSize = new System.Drawing.Size(222, 222);
            this.Name = "MainForm";
            this.Text = "Automerger";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadSource;
        private System.Windows.Forms.Button buttonLoadVersion1;
        private System.Windows.Forms.Button buttonLoadVersion2;
        private System.Windows.Forms.Button buttonMerge;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelChanged1;
        private System.Windows.Forms.Label labelChanged2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label labelMerge;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}

