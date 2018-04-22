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


    public partial class Form_AddGame : Form
    {
        Network.Client c;
        Player player;
        Form Form_Lobby;

        public Form_AddGame()
        {
            InitializeComponent();
        }

        public Form_AddGame(Point p, Network.Client c,Player player,Form lobbyForm)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;

            Point p2 = new Point(p.X + 100, p.Y + 100);
            this.Location = p2;
            this.StartPosition = FormStartPosition.Manual;
            this.c = c;
            
            this.player = player;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form_AddGame_Load(object sender, EventArgs e)
        {
            cmb_nbPlayer.Items.Add(2);
            cmb_nbPlayer.Items.Add(3);
            cmb_nbPlayer.Items.Add(4);
            cmb_nbPlayer.SelectedIndex = 0;
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {

            c.ClientCreateLobby(txtBox_Name.Text, cmb_nbPlayer.Text);
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void JoinLobby(String lobbyName)
        {

            Form fm = new Form_Lobby(this.Location, player.getName());
            fm.ShowDialog();

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i]!=this)
                    Application.OpenForms[i].Close();
            }


        }
    }
}
