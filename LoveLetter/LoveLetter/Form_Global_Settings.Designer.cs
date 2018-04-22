namespace LoveLetter
{
    partial class Form_Global_Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Global_Settings));
            this.settings_lbl = new System.Windows.Forms.Label();
            this.languages_cmbBox = new System.Windows.Forms.ComboBox();
            this.languages_lbl = new System.Windows.Forms.Label();
            this.applySave_btn = new System.Windows.Forms.Button();
            this.menu_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // settings_lbl
            // 
            resources.ApplyResources(this.settings_lbl, "settings_lbl");
            this.settings_lbl.BackColor = System.Drawing.Color.Transparent;
            this.settings_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.settings_lbl.Name = "settings_lbl";
            this.settings_lbl.Click += new System.EventHandler(this.settings_lbl_Click);
            this.settings_lbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.settings_lbl_MouseDown);
            // 
            // languages_cmbBox
            // 
            resources.ApplyResources(this.languages_cmbBox, "languages_cmbBox");
            this.languages_cmbBox.FormattingEnabled = true;
            this.languages_cmbBox.Name = "languages_cmbBox";
            // 
            // languages_lbl
            // 
            resources.ApplyResources(this.languages_lbl, "languages_lbl");
            this.languages_lbl.BackColor = System.Drawing.Color.Transparent;
            this.languages_lbl.ForeColor = System.Drawing.Color.White;
            this.languages_lbl.Name = "languages_lbl";
            // 
            // applySave_btn
            // 
            resources.ApplyResources(this.applySave_btn, "applySave_btn");
            this.applySave_btn.BackColor = System.Drawing.Color.BurlyWood;
            this.applySave_btn.Name = "applySave_btn";
            this.applySave_btn.UseVisualStyleBackColor = false;
            this.applySave_btn.Click += new System.EventHandler(this.applySave_btn_Click);
            // 
            // menu_btn
            // 
            resources.ApplyResources(this.menu_btn, "menu_btn");
            this.menu_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.menu_btn.ForeColor = System.Drawing.Color.Black;
            this.menu_btn.Name = "menu_btn";
            this.menu_btn.UseVisualStyleBackColor = false;
            this.menu_btn.Click += new System.EventHandler(this.menu_btn_Click_1);
            // 
            // Form_Global_Settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.Controls.Add(this.menu_btn);
            this.Controls.Add(this.applySave_btn);
            this.Controls.Add(this.languages_lbl);
            this.Controls.Add(this.languages_cmbBox);
            this.Controls.Add(this.settings_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Global_Settings";
            this.Load += new System.EventHandler(this.Form_Global_Settings_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_Global_Settings_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settings_lbl;
        private System.Windows.Forms.ComboBox languages_cmbBox;
        private System.Windows.Forms.Label languages_lbl;
        private System.Windows.Forms.Button applySave_btn;
        private System.Windows.Forms.Button menu_btn;
    }
}