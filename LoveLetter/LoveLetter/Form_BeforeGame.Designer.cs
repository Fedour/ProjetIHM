namespace LoveLetter
{
    partial class Form_BeforeGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_BeforeGame));
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.txtBoxTchat = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_LobbyName = new System.Windows.Forms.Label();
            this.btn_Back = new System.Windows.Forms.Button();
            this.panel_lobbyList = new System.Windows.Forms.Panel();
            this.btn_beginLobby = new System.Windows.Forms.Button();
            this.x = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_Send
            // 
            this.txt_Send.BackColor = System.Drawing.SystemColors.Info;
            resources.ApplyResources(this.txt_Send, "txt_Send");
            this.txt_Send.Name = "txt_Send";
            // 
            // txtBoxTchat
            // 
            this.txtBoxTchat.BackColor = System.Drawing.SystemColors.Info;
            resources.ApplyResources(this.txtBoxTchat, "txtBoxTchat");
            this.txtBoxTchat.Name = "txtBoxTchat";
            this.txtBoxTchat.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.txtBoxTchat.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // btn_Send
            // 
            this.btn_Send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.btn_Send, "btn_Send");
            this.btn_Send.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.UseVisualStyleBackColor = false;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click_1);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // lbl_LobbyName
            // 
            resources.ApplyResources(this.lbl_LobbyName, "lbl_LobbyName");
            this.lbl_LobbyName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LobbyName.Name = "lbl_LobbyName";
            this.lbl_LobbyName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // btn_Back
            // 
            this.btn_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.btn_Back, "btn_Back");
            this.btn_Back.ForeColor = System.Drawing.Color.Black;
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.UseVisualStyleBackColor = false;
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // panel_lobbyList
            // 
            resources.ApplyResources(this.panel_lobbyList, "panel_lobbyList");
            this.panel_lobbyList.Name = "panel_lobbyList";
            this.panel_lobbyList.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_lobbyList_Paint);
            this.panel_lobbyList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            // 
            // btn_beginLobby
            // 
            this.btn_beginLobby.BackColor = System.Drawing.Color.BurlyWood;
            resources.ApplyResources(this.btn_beginLobby, "btn_beginLobby");
            this.btn_beginLobby.Name = "btn_beginLobby";
            this.btn_beginLobby.UseVisualStyleBackColor = false;
            this.btn_beginLobby.Click += new System.EventHandler(this.btn_beginLobby_Click);
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
            // Form_BeforeGame
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.vintage_letters_reworked;
            this.Controls.Add(this.x);
            this.Controls.Add(this.btn_beginLobby);
            this.Controls.Add(this.panel_lobbyList);
            this.Controls.Add(this.btn_Back);
            this.Controls.Add(this.lbl_LobbyName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txtBoxTchat);
            this.Controls.Add(this.txt_Send);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_BeforeGame";
            this.Load += new System.EventHandler(this.Form_BeforeGame_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForm);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.TextBox txtBoxTchat;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_LobbyName;
        private System.Windows.Forms.Button btn_Back;
        private System.Windows.Forms.Panel panel_lobbyList;
        private System.Windows.Forms.Button btn_beginLobby;
        private System.Windows.Forms.Label x;
    }
}