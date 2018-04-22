namespace LoveLetter.View
{
    partial class comboBoxDialog
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
            this.target_cmbBox = new System.Windows.Forms.ComboBox();
            this.validateButtonTarget_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // target_cmbBox
            // 
            this.target_cmbBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.target_cmbBox.FormattingEnabled = true;
            this.target_cmbBox.Location = new System.Drawing.Point(48, 12);
            this.target_cmbBox.Name = "target_cmbBox";
            this.target_cmbBox.Size = new System.Drawing.Size(230, 23);
            this.target_cmbBox.TabIndex = 0;
            this.target_cmbBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.target_cmbBox_DrawItem_1);
            this.target_cmbBox.SelectedIndexChanged += new System.EventHandler(this.target_cmbBox_SelectedIndexChanged);
            // 
            // validateButtonTarget_btn
            // 
            this.validateButtonTarget_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateButtonTarget_btn.Location = new System.Drawing.Point(124, 58);
            this.validateButtonTarget_btn.Name = "validateButtonTarget_btn";
            this.validateButtonTarget_btn.Size = new System.Drawing.Size(75, 32);
            this.validateButtonTarget_btn.TabIndex = 1;
            this.validateButtonTarget_btn.Text = "Ok";
            this.validateButtonTarget_btn.UseVisualStyleBackColor = true;
            this.validateButtonTarget_btn.Click += new System.EventHandler(this.validateButtonTarget_btn_Click);
            // 
            // comboBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoveLetter.Properties.Resources.what_to_write_in_a_love_letter2;
            this.ClientSize = new System.Drawing.Size(334, 102);
            this.Controls.Add(this.validateButtonTarget_btn);
            this.Controls.Add(this.target_cmbBox);
            this.Name = "comboBoxDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose a target !";
            this.Load += new System.EventHandler(this.comboBoxDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox target_cmbBox;
        private System.Windows.Forms.Button validateButtonTarget_btn;
    }
}