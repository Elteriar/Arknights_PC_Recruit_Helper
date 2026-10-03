namespace Arknights_PC_Recruit_Helper
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listBox1 = new ListBox();
            Scan = new Button();
            Screenshot = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(206, 379);
            listBox1.TabIndex = 0;
            // 
            // Scan
            // 
            Scan.Location = new Point(12, 398);
            Scan.Name = "Scan";
            Scan.RightToLeft = RightToLeft.No;
            Scan.Size = new Size(100, 50);
            Scan.TabIndex = 1;
            Scan.Text = "Scan";
            Scan.UseVisualStyleBackColor = true;
            Scan.Click += Scan_Click;
            // 
            // Screenshot
            // 
            Screenshot.Location = new Point(118, 398);
            Screenshot.Name = "Screenshot";
            Screenshot.Size = new Size(100, 50);
            Screenshot.TabIndex = 2;
            Screenshot.Text = "Screenshot";
            Screenshot.UseVisualStyleBackColor = true;
            Screenshot.Click += Screenshot_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(230, 460);
            Controls.Add(Screenshot);
            Controls.Add(Scan);
            Controls.Add(listBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "Recruit Helper";
            FormClosed += Form1_FormClosed;
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private Button Scan;
        private Button Screenshot;
    }
}
