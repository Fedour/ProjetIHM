namespace LoveLetter
{
    partial class Form_game_settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_game_settings));
            this.plusOnePlayer_btn = new System.Windows.Forms.Button();
            this.lessOnePlayer_btn = new System.Windows.Forms.Button();
            this.nbrOfPlayer_answer_lbl = new System.Windows.Forms.Label();
            this.nbrOfPlayer_lbl = new System.Windows.Forms.Label();
            this.tittleNewGame_lbl = new System.Windows.Forms.Label();
            this.play_btn = new System.Windows.Forms.Button();
            this.menu_btn = new System.Windows.Forms.Button();
            this.difficultySetting_lbl = new System.Windows.Forms.Label();
            this.rb_noob = new System.Windows.Forms.RadioButton();
            this.rb_casu = new System.Windows.Forms.RadioButton();
            this.rb_god = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbox_name = new System.Windows.Forms.TextBox();
            this.x = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // plusOnePlayer_btn
            // 
            resources.ApplyResources(this.plusOnePlayer_btn, "plusOnePlayer_btn");
            this.plusOnePlayer_btn.Name = "plusOnePlayer_btn";
            this.plusOnePlayer_btn.UseVisualStyleBackColor = true;
            this.plusOnePlayer_btn.Click += new System.EventHandler(this.plusOnePlayer_btn_Click);
            // 
            // lessOnePlayer_btn
            // 
            resources.ApplyResources(this.lessOnePlayer_btn, "lessOnePlayer_btn");
            this.lessOnePlayer_btn.Name = "lessOnePlayer_btn";
            this.lessOnePlayer_btn.UseVisualStyleBackColor = true;
            this.lessOnePlayer_btn.Click += new System.EventHandler(this.lessOnePlayer_btn_Click);
            // 
            // nbrOfPlayer_answer_lbl
            // 
            this.nbrOfPlayer_answer_lbl.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.nbrOfPlayer_answer_lbl, "nbrOfPlayer_answer_lbl");
            this.nbrOfPlayer_answer_lbl.Name = "nbrOfPlayer_answer_lbl";
            // 
            // nbrOfPlayer_lbl
            // 
            this.nbrOfPlayer_lbl.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.nbrOfPlayer_lbl, "nbrOfPlayer_lbl");
            this.nbrOfPlayer_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nbrOfPlayer_lbl.Name = "nbrOfPlayer_lbl";
            // 
            // tittleNewGame_lbl
            // 
            resources.ApplyResources(this.tittleNewGame_lbl, "tittleNewGame_lbl");
            this.tittleNewGame_lbl.BackColor = System.Drawing.Color.Transparent;
            this.tittleNewGame_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tittleNewGame_lbl.Name = "tittleNewGame_lbl";
            this.tittleNewGame_lbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tittleSettings_lbl_MouseDown);
            // 
            // play_btn
            // 
            this.play_btn.BackColor = System.Drawing.Color.BurlyWood;
            this.play_btn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.play_btn.FlatAppearance.BorderSize = 2;
            resources.ApplyResources(this.play_btn, "play_btn");
            this.play_btn.Name = "play_btn";
            this.play_btn.UseVisualStyleBackColor = false;
            this.play_btn.Click += new System.EventHandler(this.play_btn_Click);
            // 
            // menu_btn
            // 
            this.menu_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.menu_btn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.menu_btn, "menu_btn");
            this.menu_btn.Name = "menu_btn";
            this.menu_btn.UseVisualStyleBackColor = false;
            this.menu_btn.Click += new System.EventHandler(this.menu_btn_Click);
            // 
            // difficultySetting_lbl
            // 
            this.difficultySetting_lbl.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.difficultySetting_lbl, "difficultySetting_lbl");
            this.difficultySetting_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.difficultySetting_lbl.Name = "difficultySetting_lbl";
            this.difficultySetting_lbl.Click += new System.EventHandler(this.difficultySetting_lbl_Click);
            // 
            // rb_noob
            // 
            resources.ApplyResources(this.rb_noob, "rb_noob");
            this.rb_noob.BackColor = System.Drawing.Color.Transparent;
            this.rb_noob.Name = "rb_noob";
            this.rb_noob.TabStop = true;
            this.rb_noob.UseVisualStyleBackColor = false;
            this.rb_noob.CheckedChanged += new System.EventHandler(this.rb_noob_CheckedChanged);
            // 
            // rb_casu
            // 
            resources.ApplyResources(this.rb_casu, "rb_casu");
            this.rb_casu.BackColor = System.Drawing.Color.Transparent;
            this.rb_casu.Name = "rb_casu";
            this.rb_casu.TabStop = true;
            this.rb_casu.UseVisualStyleBackColor = false;
            this.rb_casu.CheckedChanged += new System.EventHandler(this.rb_casu_CheckedChanged);
            // 
            // rb_god
            // 
            resources.ApplyResources(this.rb_god, "rb_god");
            this.rb_god.BackColor = System.Drawing.Color.Transparent;
            this.rb_god.Name = "rb_god";
            this.rb_god.TabStop = true;
            this.rb_god.UseVisualStyleBackColor = false;
            this.rb_god.CheckedChanged += new System.EventHandler(this.rb_god_CheckedChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Name = "label1";
            // 
            // txtbox_name
            // 
            resources.ApplyResources(this.txtbox_name, "txtbox_name");
            this.txtbox_name.Name = "txtbox_name";
            // 
            // x
            // 
            resources.ApplyResources(this.x, "x");
            this.x.BackColor = System.Drawing.Color.Transparent;
            this.x.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.x.Name = "x";
            this.x.Click += new System.EventHandler(this.x_Click);
            this.x.MouseEnter += new System.EventHandler(this.x_MouseEnter);
            this.x.MouseLeave += new System.EventHandler(this.x_MouseLeave);
            // 
            // Form_game_settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OliveDrab;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.Controls.Add(this.x);
            this.Controls.Add(this.txtbox_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rb_god);
            this.Controls.Add(this.rb_casu);
            this.Controls.Add(this.rb_noob);
            this.Controls.Add(this.difficultySetting_lbl);
            this.Controls.Add(this.menu_btn);
            this.Controls.Add(this.play_btn);
            this.Controls.Add(this.tittleNewGame_lbl);
            this.Controls.Add(this.nbrOfPlayer_lbl);
            this.Controls.Add(this.nbrOfPlayer_answer_lbl);
            this.Controls.Add(this.lessOnePlayer_btn);
            this.Controls.Add(this.plusOnePlayer_btn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_game_settings";
            this.Load += new System.EventHandler(this.game_settings_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_game_settings_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button plusOnePlayer_btn;
        private System.Windows.Forms.Button lessOnePlayer_btn;
        private System.Windows.Forms.Label nbrOfPlayer_answer_lbl;
        private System.Windows.Forms.Label nbrOfPlayer_lbl;
        private System.Windows.Forms.Label tittleNewGame_lbl;
        private System.Windows.Forms.Button play_btn;
        private System.Windows.Forms.Button menu_btn;
        private System.Windows.Forms.Label difficultySetting_lbl;
        private System.Windows.Forms.RadioButton rb_noob;
        private System.Windows.Forms.RadioButton rb_casu;
        private System.Windows.Forms.RadioButton rb_god;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbox_name;
        private System.Windows.Forms.Label x;
    }
}