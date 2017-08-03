namespace EAkzg
{
    partial class Detale
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
            this.label4 = new System.Windows.Forms.Label();
            this.symbolTB = new System.Windows.Forms.TextBox();
            this.nazwaProjektuTB = new System.Windows.Forms.TextBox();
            this.sdITTb = new System.Windows.Forms.TextBox();
            this.sdNTTB = new System.Windows.Forms.TextBox();
            this.AnulujBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.modelCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ZatwierdzBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol projektu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nazwa projektu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "SD IT";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(83, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "SD NT";
            // 
            // symbolTB
            // 
            this.symbolTB.Location = new System.Drawing.Point(129, 87);
            this.symbolTB.Name = "symbolTB";
            this.symbolTB.Size = new System.Drawing.Size(311, 20);
            this.symbolTB.TabIndex = 4;
            // 
            // nazwaProjektuTB
            // 
            this.nazwaProjektuTB.Location = new System.Drawing.Point(129, 116);
            this.nazwaProjektuTB.Name = "nazwaProjektuTB";
            this.nazwaProjektuTB.Size = new System.Drawing.Size(311, 20);
            this.nazwaProjektuTB.TabIndex = 5;
            // 
            // sdITTb
            // 
            this.sdITTb.Location = new System.Drawing.Point(129, 142);
            this.sdITTb.Name = "sdITTb";
            this.sdITTb.Size = new System.Drawing.Size(311, 20);
            this.sdITTb.TabIndex = 6;
            // 
            // sdNTTB
            // 
            this.sdNTTB.Location = new System.Drawing.Point(129, 168);
            this.sdNTTB.Name = "sdNTTB";
            this.sdNTTB.Size = new System.Drawing.Size(311, 20);
            this.sdNTTB.TabIndex = 7;
            // 
            // AnulujBtn
            // 
            this.AnulujBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AnulujBtn.Location = new System.Drawing.Point(475, 152);
            this.AnulujBtn.Name = "AnulujBtn";
            this.AnulujBtn.Size = new System.Drawing.Size(75, 23);
            this.AnulujBtn.TabIndex = 8;
            this.AnulujBtn.Text = "Anuluj";
            this.AnulujBtn.UseVisualStyleBackColor = true;
            this.AnulujBtn.Click += new System.EventHandler(this.AnulujBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.Location = new System.Drawing.Point(475, 123);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 9;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // modelCB
            // 
            this.modelCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.modelCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.modelCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelCB.FormattingEnabled = true;
            this.modelCB.Location = new System.Drawing.Point(129, 41);
            this.modelCB.Name = "modelCB";
            this.modelCB.Size = new System.Drawing.Size(311, 21);
            this.modelCB.TabIndex = 10;
            this.modelCB.SelectionChangeCommitted += new System.EventHandler(this.modelCB_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(83, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Model";
            // 
            // ZatwierdzBtn
            // 
            this.ZatwierdzBtn.Location = new System.Drawing.Point(475, 94);
            this.ZatwierdzBtn.Name = "ZatwierdzBtn";
            this.ZatwierdzBtn.Size = new System.Drawing.Size(75, 23);
            this.ZatwierdzBtn.TabIndex = 12;
            this.ZatwierdzBtn.Text = "Zatwierdź";
            this.ZatwierdzBtn.UseVisualStyleBackColor = true;
            this.ZatwierdzBtn.Click += new System.EventHandler(this.ZatwierdzBtn_Click);
            // 
            // Detale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 219);
            this.Controls.Add(this.ZatwierdzBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.modelCB);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.AnulujBtn);
            this.Controls.Add(this.sdNTTB);
            this.Controls.Add(this.sdITTb);
            this.Controls.Add(this.nazwaProjektuTB);
            this.Controls.Add(this.symbolTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Detale";
            this.Text = "Detale";
            this.Load += new System.EventHandler(this.Detale_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox symbolTB;
        private System.Windows.Forms.TextBox nazwaProjektuTB;
        private System.Windows.Forms.TextBox sdITTb;
        private System.Windows.Forms.TextBox sdNTTB;
        private System.Windows.Forms.Button AnulujBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.ComboBox modelCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ZatwierdzBtn;
    }
}