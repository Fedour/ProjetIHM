namespace LoveLetter
{
    partial class game_window_multi
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(game_window_multi));
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_pause = new System.Windows.Forms.Button();
            this.btn_tuto = new System.Windows.Forms.Button();
            this.back_btn = new System.Windows.Forms.Button();
            this.btn_settings = new System.Windows.Forms.Button();
            this.btn_visualSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btn_exit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_exit, "btn_exit");
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.x_Click);
            // 
            // btn_pause
            // 
            this.btn_pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btn_pause.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_pause, "btn_pause");
            this.btn_pause.Name = "btn_pause";
            this.btn_pause.UseVisualStyleBackColor = false;
            this.btn_pause.Click += new System.EventHandler(this.btn_pause_Click);
            // 
            // btn_tuto
            // 
            this.btn_tuto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btn_tuto.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_tuto, "btn_tuto");
            this.btn_tuto.Name = "btn_tuto";
            this.btn_tuto.UseVisualStyleBackColor = false;
            this.btn_tuto.Click += new System.EventHandler(this.btn_tuto_Click);
            // 
            // back_btn
            // 
            this.back_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.back_btn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.back_btn, "back_btn");
            this.back_btn.Name = "back_btn";
            this.back_btn.UseVisualStyleBackColor = false;
            this.back_btn.Click += new System.EventHandler(this.back_btn_Click);
            // 
            // btn_settings
            // 
            this.btn_settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btn_settings.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_settings, "btn_settings");
            this.btn_settings.Name = "btn_settings";
            this.btn_settings.UseVisualStyleBackColor = false;
            this.btn_settings.Click += new System.EventHandler(this.btn_settings_Click);
            // 
            // btn_visualSettings
            // 
            this.btn_visualSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btn_visualSettings.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_visualSettings, "btn_visualSettings");
            this.btn_visualSettings.Name = "btn_visualSettings";
            this.btn_visualSettings.UseVisualStyleBackColor = false;
            this.btn_visualSettings.Click += new System.EventHandler(this.btn_visualSettings_Click);
            // 
            // game_window_multi
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.wildtextures_old_paper_texture_3_reworked_version;
            this.Controls.Add(this.btn_visualSettings);
            this.Controls.Add(this.btn_settings);
            this.Controls.Add(this.btn_tuto);
            this.Controls.Add(this.btn_pause);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.back_btn);
            this.Name = "game_window_multi";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_pause;
        private System.Windows.Forms.Button btn_tuto;
        private System.Windows.Forms.Button back_btn;
        private System.Windows.Forms.Button btn_settings;
        private System.Windows.Forms.Button btn_visualSettings;
    }
}
