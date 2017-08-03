namespace EAkzg_instalacja
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.InfoLbl = new System.Windows.Forms.Label();
            this.EASciezkaLbl = new System.Windows.Forms.Label();
            this.PluginSciezkaLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wersja EA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ścieżka";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Folder docelowy";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Instaluj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(103, 114);
            this.progressBar1.Maximum = 7;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(305, 23);
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Visible = false;
            // 
            // InfoLbl
            // 
            this.InfoLbl.AutoSize = true;
            this.InfoLbl.Location = new System.Drawing.Point(116, 22);
            this.InfoLbl.Name = "InfoLbl";
            this.InfoLbl.Size = new System.Drawing.Size(140, 13);
            this.InfoLbl.TabIndex = 5;
            this.InfoLbl.Text = "Brak zainstalowanego EA !!!";
            // 
            // EASciezkaLbl
            // 
            this.EASciezkaLbl.AutoSize = true;
            this.EASciezkaLbl.Location = new System.Drawing.Point(116, 48);
            this.EASciezkaLbl.Name = "EASciezkaLbl";
            this.EASciezkaLbl.Size = new System.Drawing.Size(143, 13);
            this.EASciezkaLbl.TabIndex = 6;
            this.EASciezkaLbl.Text = "Brak zainstalowanego EA !!!!";
            // 
            // PluginSciezkaLbl
            // 
            this.PluginSciezkaLbl.AutoSize = true;
            this.PluginSciezkaLbl.Location = new System.Drawing.Point(116, 75);
            this.PluginSciezkaLbl.Name = "PluginSciezkaLbl";
            this.PluginSciezkaLbl.Size = new System.Drawing.Size(59, 13);
            this.PluginSciezkaLbl.TabIndex = 7;
            this.PluginSciezkaLbl.Text = "D:\\EAkzg\\";
            this.PluginSciezkaLbl.Click += new System.EventHandler(this.PluginSciezkaLbl_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 8;
            // 
            // rtb
            // 
            this.rtb.Location = new System.Drawing.Point(16, 143);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(392, 131);
            this.rtb.TabIndex = 9;
            this.rtb.Text = "Instalator wtyczki do programu Sparx Enterprise Architect.  \n\nGenerator HLD -> HT" +
    "ML\n\nAutor: Krzysztof Zagawa";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(333, 75);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Zmień";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 286);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PluginSciezkaLbl);
            this.Controls.Add(this.EASciezkaLbl);
            this.Controls.Add(this.InfoLbl);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Instalator pluginu do Sparx EA";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label InfoLbl;
        private System.Windows.Forms.Label EASciezkaLbl;
        private System.Windows.Forms.Label PluginSciezkaLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button2;
    }
}

