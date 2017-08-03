namespace EAkzg
{
    partial class CzekajWnd
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
            this.generowanieLbl = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGetElemByID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblElementID = new System.Windows.Forms.Label();
            this.lblReqLoop = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // generowanieLbl
            // 
            this.generowanieLbl.AutoEllipsis = true;
            this.generowanieLbl.AutoSize = true;
            this.generowanieLbl.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.generowanieLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.generowanieLbl.ForeColor = System.Drawing.Color.Red;
            this.generowanieLbl.Location = new System.Drawing.Point(39, 110);
            this.generowanieLbl.Name = "generowanieLbl";
            this.generowanieLbl.Size = new System.Drawing.Size(92, 31);
            this.generowanieLbl.TabIndex = 2;
            this.generowanieLbl.Text = "label1";
            this.generowanieLbl.UseWaitCursor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 251);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(597, 23);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "GetElemByID";
            // 
            // lblGetElemByID
            // 
            this.lblGetElemByID.AutoSize = true;
            this.lblGetElemByID.Location = new System.Drawing.Point(0, 327);
            this.lblGetElemByID.Name = "lblGetElemByID";
            this.lblGetElemByID.Size = new System.Drawing.Size(10, 13);
            this.lblGetElemByID.TabIndex = 9;
            this.lblGetElemByID.Text = " ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 351);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "ReqLoop";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "ElementID";
            // 
            // lblElementID
            // 
            this.lblElementID.AutoSize = true;
            this.lblElementID.Location = new System.Drawing.Point(0, 290);
            this.lblElementID.Name = "lblElementID";
            this.lblElementID.Size = new System.Drawing.Size(10, 13);
            this.lblElementID.TabIndex = 12;
            this.lblElementID.Text = " ";
            // 
            // lblReqLoop
            // 
            this.lblReqLoop.AutoSize = true;
            this.lblReqLoop.Location = new System.Drawing.Point(0, 364);
            this.lblReqLoop.Name = "lblReqLoop";
            this.lblReqLoop.Size = new System.Drawing.Size(10, 13);
            this.lblReqLoop.TabIndex = 13;
            this.lblReqLoop.Text = " ";
            // 
            // CzekajWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EAkzg.Properties.Resources.czekaj;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(621, 383);
            this.Controls.Add(this.lblReqLoop);
            this.Controls.Add(this.lblElementID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblGetElemByID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.generowanieLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CzekajWnd";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CzekajWnd";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.UseWaitCursor = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label generowanieLbl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGetElemByID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblElementID;
        private System.Windows.Forms.Label lblReqLoop;
    }
}