using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    public partial class Form_Pseudo : Form
    {
        public Form_Pseudo()
        {
            InitializeComponent();
        }

        public Form_Pseudo(Point p)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
            

        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        
    
        private void moveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form_Pseudo_Load(object sender, EventArgs e)
        {
            this.AcceptButton=btn_Create;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Create.PerformClick();
              
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            Boolean res = false;
            try
            {
                res = Network.Client.CheckForName(txtBox_Pseudo.Text);
                if (res)
                {
                    MessageBox.Show("Pseudo deja utilisé");
                }
                else
                {
                    this.Hide();
                    Form_Lobby fm = new Form_Lobby(this.Location, txtBox_Pseudo.Text);
                    fm.ShowDialog();
                    this.Close();
                }

            }
            catch(Exception)
            {
                this.Close();
                this.Hide();
                AskIPFormcs fm = new AskIPFormcs(this.Location, txtBox_Pseudo.Text);
                fm.ShowDialog();
                
            }
            
        
    
           
            
        }

    }
}
