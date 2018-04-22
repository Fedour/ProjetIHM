namespace LoveLetter
{
    partial class Form_Loading
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
            this.tittleLoading_lbl = new System.Windows.Forms.Label();
            this.picturebox_pen_1 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_pen_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tittleLoading_lbl
            // 
            this.tittleLoading_lbl.AutoSize = true;
            this.tittleLoading_lbl.BackColor = System.Drawing.Color.Transparent;
            this.tittleLoading_lbl.Font = new System.Drawing.Font("Times New Roman", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.tittleLoading_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tittleLoading_lbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tittleLoading_lbl.Location = new System.Drawing.Point(143, 73);
            this.tittleLoading_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tittleLoading_lbl.Name = "tittleLoading_lbl";
            this.tittleLoading_lbl.Size = new System.Drawing.Size(299, 72);
            this.tittleLoading_lbl.TabIndex = 5;
            this.tittleLoading_lbl.Text = "Loading...";
            // 
            // picturebox_pen_1
            // 
            this.picturebox_pen_1.BackColor = System.Drawing.Color.Transparent;
            this.picturebox_pen_1.Image = global::LoveLetter.Properties.Resources.stylo_a_plume_0003;
            this.picturebox_pen_1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picturebox_pen_1.Location = new System.Drawing.Point(70, 84);
            this.picturebox_pen_1.Name = "picturebox_pen_1";
            this.picturebox_pen_1.Size = new System.Drawing.Size(78, 61);
            this.picturebox_pen_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturebox_pen_1.TabIndex = 8;
            this.picturebox_pen_1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::LoveLetter.Properties.Resources.stylo_a_plume_0003;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(426, 84);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(78, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Form_Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.ClientSize = new System.Drawing.Size(561, 222);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.picturebox_pen_1);
            this.Controls.Add(this.tittleLoading_lbl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Loading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Loading";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.Form_Loading_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_pen_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tittleLoading_lbl;
        private System.Windows.Forms.PictureBox picturebox_pen_1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}