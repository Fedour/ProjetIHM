using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    public partial class Form_game_settings : Form
    {
        //Remove flickering during painting
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public Form_game_settings()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public Form_game_settings(Point p)
        {
            InitializeComponent();
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void game_settings_Load(object sender, EventArgs e)
        {
            rb_noob.Checked = true;
            this.ActiveControl = txtbox_name;
        }

        private void Form_game_settings_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void tittleSettings_lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void menu_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();
        }

        private void lessOnePlayer_btn_Click(object sender, EventArgs e)
        {
            int nb = int.Parse(nbrOfPlayer_answer_lbl.Text);
            if(nb > 2)
            {
                nb--;
            }
            nbrOfPlayer_answer_lbl.Text = nb.ToString();
        }

        private void plusOnePlayer_btn_Click(object sender, EventArgs e)
        {
            int nb = int.Parse(nbrOfPlayer_answer_lbl.Text);
            if (nb < 4)
            {
                nb++;
            }
            nbrOfPlayer_answer_lbl.Text = nb.ToString();
        }

        private void play_btn_Click(object sender, EventArgs e)
        {
            if (txtbox_name.Text!="")
            {
                this.Hide();
                this.Close();
                GameController.newGame(int.Parse(this.nbrOfPlayer_answer_lbl.Text), this.txtbox_name.Text);
            }
            else
            {
                //Window in the 'Letter' theme ?
                MessageBox.Show("You need to choose a name !", "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbox_name.Focus();
            }
        }

        private void x_MouseEnter(object sender, EventArgs e)
        {
            x.ForeColor = Color.Red;
        }

        private void x_MouseLeave(object sender, EventArgs e)
        {
            x.ForeColor = Color.FromArgb(64, 0, 0);
        }

        private void x_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void difficultySetting_lbl_Click(object sender, EventArgs e)
        {

        }

        private void rb_noob_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkbox = (RadioButton)sender;
            if (checkbox.Checked)
                Properties.Settings.Default.difficulty = 0; 
        }

        private void rb_casu_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkbox = (RadioButton)sender;
            if (checkbox.Checked)
                Properties.Settings.Default.difficulty = 1;
        }

        private void rb_god_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkbox = (RadioButton)sender;
            if (checkbox.Checked)
                Properties.Settings.Default.difficulty = 2;
        }
    }
}
