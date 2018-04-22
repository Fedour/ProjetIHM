using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    public partial class Form_Menu : Form
    {
       const UInt32 WM_SYSCOMMAND = 0x0112;
       const UInt32 SC_MINIMIZE = 0xF020;
       const UInt32 SC_RESTORE = 0xF120;
       const UInt32 WM_ERASEBKGND = 0x0014; 

        //Remove flickering during painting
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                //cp.ExStyle |= 0x00000200; // WS_EX_CLIENTEDGE <------ bordure
                return cp;
            }
        }

        public Form_Menu()
        {
            //initialize the currentCultureName
            Properties.Settings.Default.currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }

        public Form_Menu(Point p)
        {
            InitializeComponent();
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
        }

        private void soloPlayer_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form game_settings = new Form_game_settings(this.Location);
            game_settings.ShowDialog();
            this.Close();
        }

        private void multiplayerNetwork_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Pseudo form = new Form_Pseudo(this.Location);
            form.ShowDialog();
            this.Close();
        }

        private void multiplayerLocal_btn_Click(object sender, EventArgs e)
        {

        }

        private void Form_Menu_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.SuspendLayout();
            //prepating sound for the game
            
            try
            {
                //card sound
                MemoryStream ms = new MemoryStream();
                Properties.Resources.cardSoundv2.CopyTo(ms);
                Byte[] msb = ms.ToArray();
                FileStream f = new FileStream("cardSoundv2.wav", FileMode.Create);
                BinaryWriter wr = new BinaryWriter(f);
                wr.Write(msb);

                //ambiance sound
                MemoryStream ms2 = new MemoryStream();
                Properties.Resources.Puzzle_Theme1.CopyTo(ms2);
                Byte[] msb2 = ms2.ToArray();
                FileStream f2 = new FileStream("ambianceSound.wav", FileMode.Create);
                BinaryWriter wr2 = new BinaryWriter(f2);
                wr2.Write(msb2);

                //pick effect
                MemoryStream ms3 = new MemoryStream();
                Properties.Resources.cardPick.CopyTo(ms3);
                Byte[] msb3 = ms3.ToArray();
                FileStream f3 = new FileStream("cardPick.wav", FileMode.Create);
                BinaryWriter wr3 = new BinaryWriter(f3);
                wr3.Write(msb3);

                f.Close();
                f2.Close();
                f3.Close();
            }
            catch
            {

            }
            

            
            this.Visible = true;
            this.ResumeLayout();
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form_Menu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void tittleMenu_lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void multiplayerNetwork_btn_MouseEnter(object sender, EventArgs e)
        {
            multiplayerNetwork_btn.Focus();
        }

        private void movePenGifToSolo()
        {
            picturebox_pen_1.Visible = false;
            picturebox_pen_2.Visible = false;
            Point p1 = new Point(this.soloPlayer_btn.Location.X - picturebox_pen_1.Width, this.soloPlayer_btn.Location.Y +(this.soloPlayer_btn.Height-picturebox_pen_1.Height)/2);
            Point p2 = new Point(this.soloPlayer_btn.Location.X  + this.soloPlayer_btn.Width, this.soloPlayer_btn.Location.Y + (this.soloPlayer_btn.Height - picturebox_pen_1.Height)/2);
            picturebox_pen_1.Location = p1;
            picturebox_pen_2.Location = p2;
            picturebox_pen_1.Visible = true;
            picturebox_pen_2.Visible = true;
        }

        private void movePenGifToMultiNet()
        {
            picturebox_pen_1.Visible = false;
            picturebox_pen_2.Visible = false;
            Point p1 = new Point(this.multiplayerNetwork_btn.Location.X - picturebox_pen_2.Width, this.multiplayerNetwork_btn.Location.Y + (this.soloPlayer_btn.Height - picturebox_pen_1.Height)/2);
            Point p2 = new Point(this.multiplayerNetwork_btn.Location.X + this.multiplayerNetwork_btn.Location.X, this.multiplayerNetwork_btn.Location.Y + (this.soloPlayer_btn.Height - picturebox_pen_1.Height) / 2);
            picturebox_pen_1.Location = p1;
            picturebox_pen_2.Location = p2;
            picturebox_pen_1.Visible = true;
            picturebox_pen_2.Visible = true;
        }

        private void hidePenGif()
        {
            picturebox_pen_1.Visible = false;
            picturebox_pen_2.Visible = false;
        }

        private void multiplayerNetwork_btn_Enter(object sender, EventArgs e)
        {
            movePenGifToMultiNet();
        }

        private void soloPlayer_btn_MouseEnter(object sender, EventArgs e)
        {
            soloPlayer_btn.Focus();
        }

        private void soloPlayer_btn_Enter(object sender, EventArgs e)
        {
            movePenGifToSolo();
        }

        private void credits_btn_Enter(object sender, EventArgs e)
        {
            hidePenGif();
        }

        private void exit_btn_Enter(object sender, EventArgs e)
        {
            hidePenGif();
        }

        private void credits_btn_MouseEnter(object sender, EventArgs e)
        {
            credits_btn.Focus();
        }

        private void exit_btn_MouseEnter(object sender, EventArgs e)
        {
            settings_btn.Focus();
        }

        private void x_MouseEnter(object sender, EventArgs e)
        {
            x.Focus();
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

        private void x_Enter(object sender, EventArgs e)
        {
            hidePenGif();
        }

        private void settings_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form global_Settings = new Form_Global_Settings(this.Location);
            global_Settings.ShowDialog();
            this.Close();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void __Enter(object sender, EventArgs e)
        {
            hidePenGif();
        }

        private void tittleMenu_lbl_Click(object sender, EventArgs e)
        {

        }

        private void picturebox_pen_2_Click(object sender, EventArgs e)
        {

        }
    }
}
