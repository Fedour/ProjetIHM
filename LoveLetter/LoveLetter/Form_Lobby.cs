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

namespace LoveLetter
{
    public partial class Form_Lobby : Form
    {
        string pseudo;
        public Network.Client client;
        public static Player player;
        
        string readPseudo;
        string readText;
        Boolean freeThread = false;

        private Thread ctThread;
        static List<Panel> panelList = new List<Panel>();
        static Panel panelMain;

        Boolean sendHello = false;
        Boolean serverOnligne = false;
        
      

        public Form_Lobby()
        {
            InitializeComponent();
        }

        public Form_Lobby(Point p, String pseudo)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
            this.pseudo = pseudo;
            player = new Player(pseudo);
            this.MaximizeBox = false;
            client = new Network.Client(new Player(pseudo));
            sendHello = true;
            


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

        private void Form_Lobby_Load(object sender, EventArgs e)
        {
          
         
                panel_lobbyList.BackColor = Color.FromArgb(50, Color.Black);
                this.AcceptButton = btn_Send;
                textBox_Send.Focus();


              

                client.SubscribeClient();
                getLobbyList();


                panel_lobbyList.AutoScroll = false;
                panel_lobbyList.HorizontalScroll.Enabled = false;
                panel_lobbyList.HorizontalScroll.Visible = false;
                panel_lobbyList.HorizontalScroll.Maximum = 0;
                panel_lobbyList.AutoScroll = true;


                Thread.Sleep(2000);


                ctThread = new Thread(getMessage);
                ctThread.Start();


                txtBox_Tchat.ScrollBars = ScrollBars.Vertical;
                txtBox_Tchat.WordWrap = false;

         

        }

  


        private void btn_backMenu_Click(object sender, EventArgs e)
        {
            
            ShutDown();
            this.Hide();
            Form_Menu fm = new Form_Menu(this.Location);
            fm.ShowDialog();
            this.Close();

        }

    

        private void btn_addLobby_Click(object sender, EventArgs e)
        {
            Form_AddGame fm = new Form_AddGame(this.Location,client, player,this);
            fm.ShowDialog();
        }

        private void btn_tchat_Click(object sender, EventArgs e)
        {
            /*
            form_Tchat = new Form_Tchat(this.Location,pseudo);
            form_Tchat.Show();
            */
            
        }

  
          
        private List<Network.Lobby> getLobbyList()
        {
            List<Network.Lobby> lobbyList = new List<Network.Lobby>();

            String data = client.ClientAskLobby();

            
            CreatePanelList(data);
           

            return lobbyList;
        }
        

        private void ClearPanelList()
        {
            foreach(Panel p in panelList)
            {
                p.Controls.Clear();
            }
            panel_lobbyList.Controls.Clear();
            panelList.Clear();
        }
        private void CreatePanelList(String data)
        {
            
            
            String[] dataArray = data.Split('|');
            int lobby = Int32.Parse(dataArray[0]);

            for(int i = 0; i<lobby;i++)
            {
                Panel panel = new Panel();
                panel.MouseDown += new MouseEventHandler(moveForm);
                panel.BackColor = Color.FromArgb(90, Color.Black);
                panel.Width = panel_lobbyList.Width - 30;
                panel.Height = 60;


                if (i == 0)
                    panel.Location = new Point(15,10);

                else
                    panel.Location = new Point(15, 70 * i+10);

                // panel.Margin = new Padding(50, 50, 50, 50);

                panel_lobbyList.Controls.Add(panel);

                //Gestion des sous panel
                String[] lobbyArray;
                lobbyArray = dataArray[i+1].Split('&');


                Button btnJoin = new Button();
                btnJoin.Text = String.Format(Properties._string.Join);
                btnJoin.Location = new Point(panel.Width - 109, 10);
                btnJoin.Size = new Size(99, 42);
                btnJoin.BackColor = Color.BurlyWood;
                btnJoin.FlatStyle = FlatStyle.Flat;
                btnJoin.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
               

                Label lobbyName = new Label();
                lobbyName.Font= new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                lobbyName.Text = "Lobby: " + lobbyArray[0];
                lobbyName.AutoSize = true;
                lobbyName.Location = new Point(5, 19);
                lobbyName.BackColor = Color.Transparent;
                panel.Controls.Add(lobbyName);
                lobbyName.MouseDown += new MouseEventHandler(moveForm);


                Label nbPlayer = new Label();
                nbPlayer.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                //nbPlayer.Text = data;
                String text= "Joueurs:" + "" + lobbyArray[1] + "/" + lobbyArray[2];
                Console.WriteLine(text);
                nbPlayer.AutoSize = true;
                nbPlayer.Text = "";
                nbPlayer.Text = text;
                nbPlayer.Location = new Point(panel.Width-btnJoin.Width - nbPlayer.Width - 10, 19);
                nbPlayer.BackColor = Color.Transparent;
                nbPlayer.MouseDown += new MouseEventHandler(moveForm);
                panel.Controls.Add(nbPlayer);
                

                if (lobbyArray[1]==lobbyArray[2])
                {
                    btnJoin.Enabled = false;
                }

                btnJoin.Click += new EventHandler(JoinLobby);
                btnJoin.Tag = lobbyArray[0]+"|"+lobbyArray[3]; //Tag Button = lobbyName
                
                panel.Controls.Add(btnJoin);

                panelList.Add(panel);
                
            }          
        }

        private void JoinLobby(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Console.WriteLine(button.Tag.ToString());
            String[] dataArray = button.Tag.ToString().Split('|');


            KillTheThread();

            client.ClientJoinLobby(dataArray[0]);
            client.ClientUnsubscribe();

        
            this.Hide();
            this.Close();   
            Form_BeforeGame fm = new Form_BeforeGame(this.Location, new Player(pseudo), button.Tag.ToString(),client,int.Parse(dataArray[1]));
            fm.ShowDialog();
            
           
        }


        private void getMessage()
        {
            
            while (!freeThread)
            {
           
                Network.Message m = client.ClientReceiveMsgText();
                
                if(m.pseudo=="server")
                {
                    if(m.commande==Network.Commande.FREETHREAD)
                    {
                        Console.WriteLine("Free thread true");
                        freeThread = true;
                    }

                    else if(m.commande==Network.Commande.REFRESH)
                    {
                        this.Invoke((Action)delegate {
                            Thread.Sleep(250);
                            ClearPanelList();
                            CreatePanelList(m.data);
                         
                        });
                    }
                  

                }
                else
                {
                    
                    if (readText != "")
                    {
                        readText = "" + m.data; readPseudo = "" + m.pseudo;
                        this.Invoke((Action)delegate {
                            txtBox_Tchat.AppendText(Environment.NewLine + readPseudo + " >> " + readText);
                        });

                    }


                   

                }
                
            }
            Console.WriteLine("FIN THREAD LOBBY");


        }
        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                txtBox_Tchat.Text = txtBox_Tchat.Text + Environment.NewLine + readPseudo + " >> " + readText;
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if(textBox_Send.Text!="")
            {
                String msg = textBox_Send.Text;
                client.ClientSendMsgText(msg);
                textBox_Send.Clear();
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


        private void label3_MouseLeave(object sender, EventArgs e)
        {
              label3.ForeColor = Color.FromArgb(64, 0, 0);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ShutDown();
            System.Windows.Forms.Application.Exit();
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.Focus();
            label3.ForeColor = Color.Red;
        }

        private void panel_lobbyList_Paint(object sender, PaintEventArgs e)
        {

        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void KillTheThread()
        {
            client.ClientFreeThread();
        }

        private void ShutDown()
        {
            client.ClientFreeName(pseudo);
            KillTheThread();
            client.ClientUnsubscribe();
            Thread.Sleep(1000);
            client.CloseSocket();
            client = null;
        }
    }
}
