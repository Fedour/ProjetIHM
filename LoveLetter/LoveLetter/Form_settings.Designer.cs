namespace LoveLetter
{
    partial class Form_settings
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
            this.settingsTittle_lbl = new System.Windows.Forms.Label();
            this.x_lbl = new System.Windows.Forms.Label();
            this.fullscreen_chkBox = new System.Windows.Forms.CheckBox();
            this.save_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // settingsTittle_lbl
            // 
            this.settingsTittle_lbl.AutoSize = true;
            this.settingsTittle_lbl.BackColor = System.Drawing.Color.Transparent;
            this.settingsTittle_lbl.Font = new System.Drawing.Font("Times New Roman", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsTittle_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.settingsTittle_lbl.Location = new System.Drawing.Point(254, 19);
            this.settingsTittle_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.settingsTittle_lbl.Name = "settingsTittle_lbl";
            this.settingsTittle_lbl.Size = new System.Drawing.Size(299, 89);
            this.settingsTittle_lbl.TabIndex = 5;
            this.settingsTittle_lbl.Text = "Settings";
            // 
            // x_lbl
            // 
            this.x_lbl.AutoSize = true;
            this.x_lbl.BackColor = System.Drawing.Color.Transparent;
            this.x_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.x_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.x_lbl.Location = new System.Drawing.Point(804, 9);
            this.x_lbl.Name = "x_lbl";
            this.x_lbl.Size = new System.Drawing.Size(35, 38);
            this.x_lbl.TabIndex = 9;
            this.x_lbl.Text = "x";
            this.x_lbl.Click += new System.EventHandler(this.x_lbl_Click);
            this.x_lbl.MouseEnter += new System.EventHandler(this.x_lbl_MouseEnter);
            this.x_lbl.MouseLeave += new System.EventHandler(this.x_lbl_MouseLeave);
            // 
            // fullscreen_chkBox
            // 
            this.fullscreen_chkBox.AutoSize = true;
            this.fullscreen_chkBox.BackColor = System.Drawing.Color.Transparent;
            this.fullscreen_chkBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fullscreen_chkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fullscreen_chkBox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.fullscreen_chkBox.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.fullscreen_chkBox.Location = new System.Drawing.Point(116, 150);
            this.fullscreen_chkBox.Name = "fullscreen_chkBox";
            this.fullscreen_chkBox.Size = new System.Drawing.Size(500, 36);
            this.fullscreen_chkBox.TabIndex = 10;
            this.fullscreen_chkBox.Text = "Fullscreen (only for game window)";
            this.fullscreen_chkBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.fullscreen_chkBox.UseVisualStyleBackColor = false;
            // 
            // save_btn
            // 
            this.save_btn.BackColor = System.Drawing.Color.DarkOrange;
            this.save_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_btn.Font = new System.Drawing.Font("Times New Roman", 22.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_btn.ForeColor = System.Drawing.Color.Black;
            this.save_btn.Location = new System.Drawing.Point(338, 450);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(146, 66);
            this.save_btn.TabIndex = 11;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = false;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // Form_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.ClientSize = new System.Drawing.Size(851, 543);
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.fullscreen_chkBox);
            this.Controls.Add(this.x_lbl);
            this.Controls.Add(this.settingsTittle_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_settings";
            this.Text = "Form_settings";
            this.Load += new System.EventHandler(this.Form_settings_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_settings_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settingsTittle_lbl;
        private System.Windows.Forms.Label x_lbl;
        private System.Windows.Forms.CheckBox fullscreen_chkBox;
        private System.Windows.Forms.Button save_btn;
    }
}