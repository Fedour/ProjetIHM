namespace LoveLetter
{
    partial class Form_Lobby
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Lobby));
            this.btn_backMenu = new System.Windows.Forms.Button();
            this.lbl_partieDispo = new System.Windows.Forms.Label();
            this.panel_lobbyList = new System.Windows.Forms.Panel();
            this.btn_addLobby = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Send = new System.Windows.Forms.Button();
            this.txtBox_Tchat = new System.Windows.Forms.TextBox();
            this.textBox_Send = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_backMenu
            // 
            this.btn_backMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.btn_backMenu, "btn_backMenu");
            this.btn_backMenu.Name = "btn_backMenu";
            this.btn_backMenu.UseVisualStyleBackColor = false;
            this.btn_backMenu.Click += new System.EventHandler(this.btn_backMenu_Click);
            // 
            // lbl_partieDispo
            // 
            resources.ApplyResources(this.lbl_partieDispo, "lbl_partieDispo");
            this.lbl_partieDispo.BackColor = System.Drawing.Color.Transparent;
            this.lbl_partieDispo.Name = "lbl_partieDispo";
            this.lbl_partieDispo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // panel_lobbyList
            // 
            resources.ApplyResources(this.panel_lobbyList, "panel_lobbyList");
            this.panel_lobbyList.Name = "panel_lobbyList";
            this.panel_lobbyList.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_lobbyList_Paint);
            this.panel_lobbyList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // btn_addLobby
            // 
            this.btn_addLobby.BackColor = System.Drawing.Color.BurlyWood;
            resources.ApplyResources(this.btn_addLobby, "btn_addLobby");
            this.btn_addLobby.Name = "btn_addLobby";
            this.btn_addLobby.UseVisualStyleBackColor = false;
            this.btn_addLobby.Click += new System.EventHandler(this.btn_addLobby_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            this.label3.MouseEnter += new System.EventHandler(this.label3_MouseEnter);
            this.label3.MouseLeave += new System.EventHandler(this.label3_MouseLeave);
            // 
            // btn_Send
            // 
            this.btn_Send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.btn_Send, "btn_Send");
            this.btn_Send.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.UseVisualStyleBackColor = false;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // txtBox_Tchat
            // 
            this.txtBox_Tchat.BackColor = System.Drawing.SystemColors.Info;
            resources.ApplyResources(this.txtBox_Tchat, "txtBox_Tchat");
            this.txtBox_Tchat.Name = "txtBox_Tchat";
            // 
            // textBox_Send
            // 
            this.textBox_Send.BackColor = System.Drawing.SystemColors.Info;
            resources.ApplyResources(this.textBox_Send, "textBox_Send");
            this.textBox_Send.Name = "textBox_Send";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // Form_Lobby
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txtBox_Tchat);
            this.Controls.Add(this.textBox_Send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_addLobby);
            this.Controls.Add(this.panel_lobbyList);
            this.Controls.Add(this.lbl_partieDispo);
            this.Controls.Add(this.btn_backMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Lobby";
            this.Load += new System.EventHandler(this.Form_Lobby_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_backMenu;
        private System.Windows.Forms.Label lbl_partieDispo;
        private System.Windows.Forms.Panel panel_lobbyList;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btn_addLobby;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txtBox_Tchat;
        private System.Windows.Forms.TextBox textBox_Send;
        private System.Windows.Forms.Label label2;
    }
}