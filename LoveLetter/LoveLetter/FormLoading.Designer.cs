namespace LoveLetter
{
    partial class FormLoading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoading));
            this.tittleMenu_lbl = new System.Windows.Forms.Label();
            this.picturebox_pen_1 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_pen_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tittleMenu_lbl
            // 
            resources.ApplyResources(this.tittleMenu_lbl, "tittleMenu_lbl");
            this.tittleMenu_lbl.BackColor = System.Drawing.Color.Transparent;
            this.tittleMenu_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tittleMenu_lbl.Name = "tittleMenu_lbl";
            this.tittleMenu_lbl.UseWaitCursor = true;
            // 
            // picturebox_pen_1
            // 
            resources.ApplyResources(this.picturebox_pen_1, "picturebox_pen_1");
            this.picturebox_pen_1.BackColor = System.Drawing.Color.Transparent;
            this.picturebox_pen_1.Image = global::LoveLetter.Properties.Resources.stylo_a_plume_0003;
            this.picturebox_pen_1.Name = "picturebox_pen_1";
            this.picturebox_pen_1.TabStop = false;
            this.picturebox_pen_1.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::LoveLetter.Properties.Resources.stylo_a_plume_0003;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            // 
            // FormLoading
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.picturebox_pen_1);
            this.Controls.Add(this.tittleMenu_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLoading";
            this.TopMost = true;
            this.UseWaitCursor = true;
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_pen_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tittleMenu_lbl;
        private System.Windows.Forms.PictureBox picturebox_pen_1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}