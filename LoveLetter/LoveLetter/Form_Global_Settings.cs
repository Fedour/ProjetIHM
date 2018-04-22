using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    public partial class Form_Global_Settings : Form
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


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form_Global_Settings()
        {
            InitializeComponent();


        }

        public Form_Global_Settings(Point p)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture; 
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
        }



        private void Form_Global_Settings_Load(object sender, EventArgs e)
        {
            this.languages_cmbBox.Items.Add(Properties._string.English);
            this.languages_cmbBox.Items.Add(Properties._string.French);
            this.languages_cmbBox.Items.Add(Properties._string.German);
            this.languages_cmbBox.SelectedIndex = 0;
        }

        private void applySave_btn_Click(object sender, EventArgs e)
        {
            switchLanguage();
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();
        }

        private void switchLanguage()
        {
            switch (this.languages_cmbBox.SelectedIndex)
            {
                //english
                case 0:
                    Localization.Change(Language.English); 
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Properties.Settings.Default.currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;
                    break;
                case 1:
                    Localization.Change(Language.Français);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
                    Properties.Settings.Default.currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;
                    break;
                case 2:
                    Localization.Change(Language.Deutsch);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                    Properties.Settings.Default.currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;
                    break;
            }
        }

        private void menu_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();
        }

        private void menu_btn_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();
        }

        private void Form_Global_Settings_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void settings_lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void settings_lbl_Click(object sender, EventArgs e)
        {

        }
    }
}
