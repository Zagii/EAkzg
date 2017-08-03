namespace EAkzg
{
    partial class WymaganiaFormPodglad
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.stereotypLbl = new System.Windows.Forms.Label();
            this.typLbl = new System.Windows.Forms.Label();
            this.CBstatus = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rtfName = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rtfNotes = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.rtfLinkedDoc = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stereotypLbl);
            this.panel1.Controls.Add(this.typLbl);
            this.panel1.Controls.Add(this.CBstatus);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.rtfName);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 225);
            this.panel1.TabIndex = 1;
            // 
            // stereotypLbl
            // 
            this.stereotypLbl.AutoSize = true;
            this.stereotypLbl.Location = new System.Drawing.Point(122, 23);
            this.stereotypLbl.Name = "stereotypLbl";
            this.stereotypLbl.Size = new System.Drawing.Size(35, 13);
            this.stereotypLbl.TabIndex = 14;
            this.stereotypLbl.Text = "label8";
            // 
            // typLbl
            // 
            this.typLbl.AutoSize = true;
            this.typLbl.Location = new System.Drawing.Point(11, 23);
            this.typLbl.Name = "typLbl";
            this.typLbl.Size = new System.Drawing.Size(35, 13);
            this.typLbl.TabIndex = 13;
            this.typLbl.Text = "label8";
            // 
            // CBstatus
            // 
            this.CBstatus.FormattingEnabled = true;
            this.CBstatus.Location = new System.Drawing.Point(210, 20);
            this.CBstatus.Name = "CBstatus";
            this.CBstatus.Size = new System.Drawing.Size(214, 21);
            this.CBstatus.TabIndex = 12;
            this.CBstatus.SelectionChangeCommitted += new System.EventHandler(this.CBstatus_SelectionChangeCommitted);
            this.CBstatus.SelectedValueChanged += new System.EventHandler(this.CBstatus_SelectedValueChanged);
            this.CBstatus.TextChanged += new System.EventHandler(this.CBstatus_TextChanged);
            this.CBstatus.Leave += new System.EventHandler(this.CBstatus_Leave);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(6, 127);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(418, 95);
            this.listBox1.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Relacje";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(275, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stereotyp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Typ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // rtfName
            // 
            this.rtfName.Location = new System.Drawing.Point(6, 62);
            this.rtfName.Name = "rtfName";
            this.rtfName.Size = new System.Drawing.Size(418, 44);
            this.rtfName.TabIndex = 0;
            this.rtfName.Text = "";
            this.rtfName.TextChanged += new System.EventHandler(this.rtfName_TextChanged);
            this.rtfName.Leave += new System.EventHandler(this.rtfName_Leave);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rtfNotes);
            this.panel2.Location = new System.Drawing.Point(448, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(917, 225);
            this.panel2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Notes";
            // 
            // rtfNotes
            // 
            this.rtfNotes.Location = new System.Drawing.Point(3, 20);
            this.rtfNotes.Name = "rtfNotes";
            this.rtfNotes.Size = new System.Drawing.Size(890, 202);
            this.rtfNotes.TabIndex = 0;
            this.rtfNotes.Text = "";
            this.rtfNotes.TextChanged += new System.EventHandler(this.rtfNotes_TextChanged);
            this.rtfNotes.Leave += new System.EventHandler(this.rtfNotes_Leave);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.rtfLinkedDoc);
            this.panel3.Location = new System.Drawing.Point(13, 243);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1352, 217);
            this.panel3.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-1, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Linked Document";
            // 
            // rtfLinkedDoc
            // 
            this.rtfLinkedDoc.Location = new System.Drawing.Point(2, 35);
            this.rtfLinkedDoc.Name = "rtfLinkedDoc";
            this.rtfLinkedDoc.ReadOnly = true;
            this.rtfLinkedDoc.Size = new System.Drawing.Size(1326, 172);
            this.rtfLinkedDoc.TabIndex = 0;
            this.rtfLinkedDoc.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Potwierdź zmiany";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WymaganiaFormPodglad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 512);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "WymaganiaFormPodglad";
            this.Text = "Detale elementu";
            this.TopMost = true;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WymaganiaFormPodglad_MouseClick);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtfName;
        private System.Windows.Forms.RichTextBox rtfNotes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtfLinkedDoc;
        private System.Windows.Forms.Label typLbl;
        private System.Windows.Forms.ComboBox CBstatus;
        private System.Windows.Forms.Label stereotypLbl;
        private System.Windows.Forms.Button button1;
    }
}