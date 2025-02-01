
namespace ESP32_Flash
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.COM_statusStrip = new System.Windows.Forms.StatusStrip();
            this.COM_toolStatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Erase_toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.COM_statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Lime;
            this.textBox1.Location = new System.Drawing.Point(1, -3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(834, 439);
            this.textBox1.TabIndex = 0;
            // 
            // COM_statusStrip
            // 
            this.COM_statusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.COM_statusStrip.AutoSize = false;
            this.COM_statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.COM_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.COM_toolStatusStripLabel,
            this.Erase_toolStripStatusLabel});
            this.COM_statusStrip.Location = new System.Drawing.Point(0, 439);
            this.COM_statusStrip.Name = "COM_statusStrip";
            this.COM_statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.COM_statusStrip.Size = new System.Drawing.Size(835, 26);
            this.COM_statusStrip.TabIndex = 1;
            this.COM_statusStrip.Text = "COM";
            // 
            // COM_toolStatusStripLabel
            // 
            this.COM_toolStatusStripLabel.Name = "COM_toolStatusStripLabel";
            this.COM_toolStatusStripLabel.Size = new System.Drawing.Size(35, 21);
            this.COM_toolStatusStripLabel.Text = "COM";
            this.COM_toolStatusStripLabel.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // Erase_toolStripStatusLabel
            // 
            this.Erase_toolStripStatusLabel.Name = "Erase_toolStripStatusLabel";
            this.Erase_toolStripStatusLabel.Size = new System.Drawing.Size(34, 21);
            this.Erase_toolStripStatusLabel.Text = "Erase";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 465);
            this.Controls.Add(this.COM_statusStrip);
            this.Controls.Add(this.textBox1);
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ESP32 Flash";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.COM_statusStrip.ResumeLayout(false);
            this.COM_statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.StatusStrip COM_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel COM_toolStatusStripLabel;
        private System.Windows.Forms.ToolStripStatusLabel Erase_toolStripStatusLabel;
    }
}

