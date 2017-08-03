namespace EAkzg
{
    partial class Slownik
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Lp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.skrot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.anuluj = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Lp,
            this.skrot,
            this.opis});
            this.dataGridView1.Location = new System.Drawing.Point(12, 34);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.Size = new System.Drawing.Size(800, 245);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Lp
            // 
            this.Lp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Lp.FillWeight = 10F;
            this.Lp.HeaderText = "Lp";
            this.Lp.Name = "Lp";
            this.Lp.ToolTipText = "Liczba porządkowa";
            // 
            // skrot
            // 
            this.skrot.FillWeight = 76.14214F;
            this.skrot.HeaderText = "Skrót/Pojęcie";
            this.skrot.Name = "skrot";
            this.skrot.ToolTipText = "Skrót do wyjaśnienia";
            this.skrot.Width = 150;
            // 
            // opis
            // 
            this.opis.FillWeight = 150.1574F;
            this.opis.HeaderText = "Opis";
            this.opis.Name = "opis";
            this.opis.ToolTipText = "Wyjaśnienie skrótu";
            this.opis.Width = 505;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(29, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Edycja słownika pojęć i skrótów";
            // 
            // anuluj
            // 
            this.anuluj.Location = new System.Drawing.Point(594, 285);
            this.anuluj.Name = "anuluj";
            this.anuluj.Size = new System.Drawing.Size(87, 23);
            this.anuluj.TabIndex = 2;
            this.anuluj.Text = "Anuluj ";
            this.anuluj.UseVisualStyleBackColor = true;
            this.anuluj.Click += new System.EventHandler(this.anuluj_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(687, 285);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(124, 23);
            this.OkBtn.TabIndex = 3;
            this.OkBtn.Text = "Zatwierdź";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // Slownik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 320);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.anuluj);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Slownik";
            this.Text = "Slownik";
            this.Load += new System.EventHandler(this.Slownik_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn Lp;
        private System.Windows.Forms.DataGridViewTextBoxColumn skrot;
        private System.Windows.Forms.DataGridViewTextBoxColumn opis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button anuluj;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}