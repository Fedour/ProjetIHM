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
    public partial class Form_settings : Form
    {
        //takes the location of the calling form
        public Form_settings(Point previousLocation)
        {
            InitializeComponent();
            this.Location = previousLocation;
            this.StartPosition = FormStartPosition.Manual;

            //update the components states depending on the properties file
            fullscreen_chkBox.Checked = Properties.Settings.Default.fullscreen;
        }

        private void Form_settings_Load(object sender, EventArgs e)
        {
        }

        private void x_lbl_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void x_lbl_MouseEnter(object sender, EventArgs e)
        {
            x_lbl.Focus();
            x_lbl.ForeColor = Color.Red;
        }

        private void x_lbl_MouseLeave(object sender, EventArgs e)
        {
            x_lbl.ForeColor = Color.FromArgb(64, 0, 0);
        }

        // movable borderless form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form_settings_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
           
        //save the settings in the .NET properties file
        private void save_btn_Click(object sender, EventArgs e)
        {

                Properties.Settings.Default.fullscreen = fullscreen_chkBox.Checked;
                this.Hide();
                Form Form_Menu = new Form_Menu(this.Location);
                Form_Menu.ShowDialog();
                this.Close();
        }
    }
}
