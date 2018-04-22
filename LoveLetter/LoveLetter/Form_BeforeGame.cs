using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace LoveLetter
{


    public partial class Form_BeforeGame : Form
    {
        Player p;
        Network.Client client;
        Network.Client clientLobby;
        String lobbyName;

        String readText;
        String readPseudo;

        Boolean freeThread = false;
        Thread ctThread;
        int port;

        List<Panel> panelList = new List<Panel>();


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

        public Form_BeforeGame()
        {
            InitializeComponent();
        }

        public Form_BeforeGame(Point p, Player player, String lobby, Network.Client c, int port)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
            this.p = player;
            this.client = c;
            this.port = port;
            lobbyName = lobby;


        }

        private void Form_BeforeGame_Load(object sender, EventArgs e)
        {
            panel_lobbyList.BackColor = Color.FromArgb(50, Color.Black);

            this.AcceptButton = btn_Send;
            txt_Send.Focus();

            //Suppression client
            client.CloseSocket();
            client = null;


            clientLobby = new Network.Client(p, port);

            getPlayerList();

            Thread.Sleep(500);
            ctThread = new Thread(getMessageBeforeGame);
            ctThread.Start();


            

     

        }

        private void ClearPanelList()
        {
            foreach (Panel p in panelList)
            {
                p.Controls.Clear();
            }
            panel_lobbyList.Controls.Clear();
            panelList.Clear();
        }

        private void getMessageBeforeGame()
        {

            while (!freeThread)
            {
                
                
                try
                {
                    Network.Message m = clientLobby.ClientReceiveMsgText();
                    if (m.pseudo == "server")
                    {
                        switch (m.commande)
                        {
                            case Network.Commande.FREETHREAD:
                                freeThread = true;
                                break;

                            case Network.Commande.BEGIN:
                                if (m.data == "ok")
                                {
                                    BeginOK();
                                 
                                }
                                else
                                {
                                    BeginNoOk();
                                }
                                break;

                            case Network.Commande.REFRESH:
                                this.Invoke((Action)delegate
                                {
                                    ClearPanelList();
                                    CreatePanelList(m.data);
                                });
                                break;
                        }
                    }
                    else
                    {
                        //Affichage message tchat
                        if (readText != "")
                        {
                            readText = "" + m.data; readPseudo = "" + m.pseudo;
                            this.Invoke((Action)delegate
                            {
                                txtBoxTchat.AppendText(Environment.NewLine + readPseudo + " >> " + readText);
                            });

                        }

                    }
                    Console.WriteLine("FIN THREAD BEFORE");
                }

                catch (Exception e)
                {
                    freeThread = true;
                }
            }



        }
        private List<String> GetAllPlayerName()
        {
            List<String> res = new List<string>();

            String data = clientLobby.ClientAskPlayerInLobby(lobbyName);
            String[] dataArray = data.Split('|');
            String[] lobbyArray;
            int lobbySize = Int32.Parse(dataArray[0]);

            for (int i = 0; i < lobbySize; i++)
            {
                lobbyArray = dataArray[i + 1].Split('|');
                res.Add(lobbyArray[0]);
            }

            return res;
        }
        private void getPlayerList()
        {

            String data = clientLobby.ClientAskPlayerInLobby(lobbyName);
            CreatePanelList(data);


        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            KillTheThread();
            Thread.Sleep(500);


            clientLobby.ClientLeaveLobby(lobbyName);

            clientLobby.CloseSocket();

            clientLobby = null;
            this.Hide();
            this.Close();
            Form_Lobby fm = new Form_Lobby(this.Location, p.getName());
            fm.ShowDialog();



        }

        private void x_MouseLeave(object sender, EventArgs e)
        {
            x.ForeColor = Color.FromArgb(64, 0, 0);
        }

        private void x_MouseEnter(object sender, EventArgs e)
        {
            x.Focus();
            x.ForeColor = Color.Red;
        }


        private void x_Click(object sender, EventArgs e)
        {
            KillTheThread();
            clientLobby.ClientUnsubscribe();
            Thread.Sleep(1000);
           

            clientLobby.CloseSocket();
            clientLobby = null;
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {

                Application.OpenForms[i].Close();
            }
        }

        private void panel_lobbyList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreatePanelList(String data)
        {


            String[] dataArray = data.Split('|');
            int lobby = Int32.Parse(dataArray[0]);

            for (int i = 0; i < lobby; i++)
            {
                Panel panel = new Panel();
                panel.BackColor = Color.FromArgb(90, Color.Black);
                panel.Width = panel_lobbyList.Width - 30;
                panel.Height = 60;
                panel.MouseDown += new MouseEventHandler(moveForm);


                if (i == 0)
                    panel.Location = new Point(15, 10);

                else
                    panel.Location = new Point(15, 70 * i + 10);

                // panel.Margin = new Padding(50, 50, 50, 50);

                panel_lobbyList.Controls.Add(panel);

                //Gestion des sous panel
                String[] lobbyArray;
                lobbyArray = dataArray[i + 1].Split('|');


                Label playerName = new Label();
                playerName.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                playerName.Text = "Player: " + lobbyArray[0];
                playerName.Location = new Point(5, 19);
                playerName.BackColor = Color.Transparent;
                panel.Controls.Add(playerName);
                playerName.AutoSize = true;
                playerName.MouseDown += new MouseEventHandler(moveForm);
                panelList.Add(panel);

            }


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

       private void btn_Send_Click_1(object sender, EventArgs e)
        {
            if(txt_Send.Text!="")
            {
                String msg = txt_Send.Text;
                clientLobby.ClientSendMsgText(msg);
                txt_Send.Clear();
            }
          

        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Send.PerformClick();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void KillTheThread()
        {


            Console.WriteLine("THREAD KILL BEFORE GAME");
            clientLobby.ClientFreeThread();
        }

        private void btn_beginLobby_Click(object sender, EventArgs e)
        {
            //Demande debut partie
            clientLobby.ClientBeginGame();
        }

        private void BeginOK()
        {
            View.Fm_Dialog fm;
            List<String> nameList = GetAllPlayerName();

            this.Invoke((Action)delegate
            {
                
                KillTheThread();


                fm = new View.Fm_Dialog(this.Location, String.Format(Properties._string.GameWillBegin), false);
                fm.Show();

                Task.Factory.StartNew(() => { System.Threading.Thread.Sleep(1000); }).ContinueWith(_ =>
                {
                    Thread.Sleep(2000);
                    this.Hide();
                    this.Close();
                    this.Enabled = true;
                    fm.Hide();
                    fm.Close();

                }, TaskScheduler.FromCurrentSynchronizationContext());
              
                
                

            });
            clientLobby.ClientReadyBegin();
            GameControllerMulti.newGame(nameList.Count(), nameList, clientLobby,port);
           

        }
        private void BeginNoOk()
        {
            this.Invoke((Action)delegate
            {
               
                View.Fm_Dialog fm = new View.Fm_Dialog(this.Location, String.Format(Properties._string.NotEnoughPlayer), true);
                fm.ShowDialog();
                Thread.Sleep(3000);
                fm.Hide();
                fm.Close();
            });
          

        }

        private void ShutDown()
        {
            KillTheThread();
            Thread.Sleep(1000);
           
        }
    }
    
}
