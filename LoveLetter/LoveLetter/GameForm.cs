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
    public partial class game_window : Form
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

        PictureBox deck;
        PictureBox bufferedPreviousCard;//copy of the former card when mouse enter and makes the card pop
        int deck_width = (int)(Screen.PrimaryScreen.Bounds.Width / 13.3);
        int deck_height = (int)(Screen.PrimaryScreen.Bounds.Height / 5.4);

        Point deck_location;

        Boolean tuto = false;

        System.Threading.ManualResetEventSlim mre = new ManualResetEventSlim(false);

        int selectedPlayerNum = 0;

        String selectedTypeCard = "";

        String[] resourcesPath = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

        public Boolean muted = false;

        public double musicVolume = 0.5;
        public double effectVolume = 0.5;
        public double mainVolume = 0.5;

        Boolean pause = false;

        //tooltip that show card effect
        ToolTip effect = new ToolTip();

        public System.Windows.Media.MediaPlayer cardSound = new System.Windows.Media.MediaPlayer();
        public System.Windows.Media.MediaPlayer ambianceSound = new System.Windows.Media.MediaPlayer();
        public System.Windows.Media.MediaPlayer cardPick = new System.Windows.Media.MediaPlayer();

        //removed cards at the beggining of the game
        PictureBox[] removedCards;
        int indexRemovedCard = 0;
        int offsetRemovedCard = 0;

        //hands of player
        PictureBox[][] handPlayer = new PictureBox[][]
        {
            new PictureBox[2] { null, null },
            new PictureBox[2] { null, null },
            new PictureBox[2] { null, null },
            new PictureBox[2] { null, null }
        };

        //array of player removed cards offset
        int[] offsetPLayerRemovedCards;

        //player information
        Point[] locations = new Point[4];
        Label[] playerNames;
        Label[] playerPoints;

        //log box
        TextBox logBox;

        public game_window()
        {
            this.Visible = false;

            this.DoubleBuffered = true; //test

            // this.TopMost = true;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //waiting = new Thread(new ThreadStart(showLoading));
            //waiting.Start();
            //this.Opacity = 0;           

            InitializeComponent();
        }

        public game_window(Boolean mute, double main, double music, double effect)
        {
            this.Visible = false;

            this.DoubleBuffered = true; //test

            // this.TopMost = true;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            muted = mute;
            mainVolume = main;
            musicVolume = music;
            effectVolume = effect;
            

            //waiting = new Thread(new ThreadStart(showLoading));
            //waiting.Start();
            //this.Opacity = 0;           

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            //deck , Care the deck width and height is a reference size for all the other components 
            deck = new PictureBox();
            deck.SizeMode = PictureBoxSizeMode.StretchImage;
            deck.Size = new Size(deck_width, deck_height);
            deck.Image = Properties.Resources.backcard2;
            deck.Location = new Point(this.Width / 2 - (deck.Width / 2), this.Height / 2 - (deck.Height / 2));
            deck.MouseEnter += new EventHandler(card_MouseEnter);
            deck.MouseLeave += new EventHandler(card_MouseLeave);
            deck.Click += new EventHandler(deck_Click);
            this.deck_location = new Point(this.Width / 2 - (deck.Width / 2), this.Height / 2 - (deck.Height / 2));

            //initialize offset for removed card game deck
            this.offsetRemovedCard = deck_width / 4;
            this.Controls.Add(deck);

            int nbPlayers = Properties.Settings.Default.nbrPlayers;

            playerNames = new Label[nbPlayers];
            playerPoints = new Label[nbPlayers];

            int size = deck.Height;


            double tmpWidth = (double)this.Width;
            double offset = tmpWidth * 0.1;

            int xPositionLabels = this.Width - (this.Width / 3);
            int yPositionLabels = this.Height - (this.Height / 3);

            locations[0] = new Point(xPositionLabels, this.Height - size / 2);
            locations[1] = new Point(0, yPositionLabels); // let the space for a card in the hand and some extra space (offset) / left side player
            locations[2] = new Point(xPositionLabels, 0);
            locations[3] = new Point(this.Width - size / 2, yPositionLabels);

            //update offset array
            offsetPLayerRemovedCards = new int[] { 0, 0, 0, 0 };

            if (nbPlayers == 2)
            {
                this.removedCards = new PictureBox[3];
            }

            for (int i = 0; i < nbPlayers; i++)
            {

                //label for players name
                playerNames[i] = new Label();
                Label label = playerNames[i];
                label.Location = locations[i];
                label.Text = Game.Instance.players[i].name + "\n" + String.Format(Properties._string.Score, Game.Instance.players[i].nbMarker);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Dock = DockStyle.Fill;
                label.Font = new Font("Times New Roman", (this.Width / this.Height) * 12, FontStyle.Bold);
                label.Tag = i; //playerIndex
                //label.BackColor = Color.Transparent;

                Panel panel = new Panel();
                panel.Location = locations[i];
                panel.Controls.Add(label);
                panel.Anchor = AnchorStyles.None;
                panel.Size = new Size(100, 100);
                panel.BackColor = Color.Transparent;

                this.Controls.Add(panel);

                //allocate picture box for players hands
            }

            //Control box
            int cbOffsetX = this.Width / 300;
            int cbOffsetY = this.Width / 300;
            btn_pause.Location = new Point(this.Width - cbOffsetX - btn_pause.Size.Width, cbOffsetY);

            //pause menu init
            back_btn.Visible = false;
            btn_exit.Visible = false;
            btn_settings.Visible = false;
            btn_tuto.Visible = false;
            btn_visualSettings.Visible = false;

            btn_settings.Location = new Point(btn_pause.Location.X, 2*cbOffsetY + btn_pause.Size.Height);
            btn_visualSettings.Location = new Point(btn_pause.Location.X, 3 * cbOffsetY + 2 * btn_pause.Size.Height);
            btn_tuto.Location = new Point(btn_pause.Location.X, 4*cbOffsetY + 3*btn_pause.Size.Height);
            back_btn.Location = new Point(btn_pause.Location.X, 5*cbOffsetY + 4*btn_pause.Size.Height);
            btn_exit.Location = new Point(btn_pause.Location.X, 6*cbOffsetY + 5*btn_pause.Size.Height);

            btn_visualSettings.Size = btn_exit.Size;

            //get app Path and read sound that we saved before in the menu
            String appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            cardSound.Open(new System.Uri(appPath+@"\cardSoundv2.wav"));
            ambianceSound.Open(new System.Uri(appPath + @"\ambianceSound.wav"));
            cardPick.Open(new System.Uri(appPath + @"\cardPick.wav"));

            if (muted)//if former sound param were muted
            {
                ambianceSound.Volume = 0;
                cardSound.Volume = 0;
                cardPick.Volume = 0;
            }
            else
            {
                ambianceSound.Volume = musicVolume;
                cardSound.Volume = effectVolume;
                cardPick.Volume = effectVolume;
            }
                
            cardSound.MediaEnded += new EventHandler(cardSound_Ended);
            cardPick.MediaEnded += new EventHandler(cardSound_Ended);
            ambianceSound.MediaEnded += new EventHandler(ambiance_Ended);
            ambianceSound.Play();
            this.ResumeLayout();
            this.Visible = true;

            //log box
            logBox= new TextBox();
            
            logBox.Size = new Size(this.Width / 5, this.Height / 5);
            logBox.Location = new Point(0, this.Height - (this.Height / 5));
            logBox.Multiline = true;
            logBox.ScrollBars = ScrollBars.Vertical;
            logBox.ReadOnly = true;
            logBox.Font = new Font("Times new Roman", 16, FontStyle.Regular);
            logBox.BackColor = Color.FromArgb(160, 110, 60);
            logBox.BorderStyle = BorderStyle.None;
            logBox.Visible = Properties.Settings.Default.showMessageBox;
            this.Controls.Add(logBox);

            StartGame();
        }

        public void addLog(String txt)
        {
            Invoke(new Action(() =>
            {
                this.logBox.AppendText(Environment.NewLine + txt);
                this.logBox.Refresh();
            }));
            
        }

        //update the removed and the moved picture box 
        public void updatePictureBox(PictureBox removedCard, PictureBox movedCard, Card c, RotateFlipType rotate, Player p)
        {
            //update the picture by adding tooltip effect and picture for the visible side of the card
            removedCard.Image = c.picture;
            removedCard.Tag = c.texte;
            removedCard.MouseHover += new EventHandler(displayCardEffect); // add the tooltip effect now that the card is visible
            removedCard.Image.RotateFlip(rotate);

            if (p.indexPlayer % 2 == 0) // player up and down, update the offset
            {
                this.offsetPLayerRemovedCards[p.indexPlayer] += removedCard.Width / 4;

            }
            else
            {
                this.offsetPLayerRemovedCards[p.indexPlayer] += removedCard.Height / 4;
            }

            removedCard.Refresh();
            movedCard.Refresh();
            this.Update();

            //remove all the interaction from the removed card
            removedCard.MouseEnter -= new EventHandler(card_MouseEnter);
            removedCard.MouseLeave -= new EventHandler(card_MouseLeave);
            removedCard.Click -= new EventHandler(card_Click);
        }

        //show the hand of the player and add tooltip to it
        public void showCardsHandOnBoard(Player player)
        {
            Invoke(new Action(() =>
            {
                int i = 0;
                PictureBox pb = this.handPlayer[player.indexPlayer][i];
                foreach (Card c in player.player_Deck)
                {
                    Bitmap b = new Bitmap(c.picture);
                    pb.Image = b;
                    pb.Tag = c.texte;
                    pb.MouseHover += new EventHandler(displayCardEffect); // add the tooltip effect now that the card is visible
                    pb.Image.RotateFlip((RotateFlipType)player.indexPlayer);
                    this.Refresh();
                    i++;
                }
            }));
        }

        //moves the card C in the front of the player P with an offset depending on the played cards
        public void PlayCard(Card c, Player p)
        {
            int indexCard = p.player_Deck.IndexOf(c);
            int indexOtherCard = 1 - indexCard;

            PictureBox card = handPlayer[p.indexPlayer][indexCard];
            PictureBox otherCard = handPlayer[p.indexPlayer][indexCard];

            if (p.indexPlayer == 0) downSizeCard(card);

            Invoke(new Action(() =>
            {
                cardSound.Play();                      
                int offset = this.offsetPLayerRemovedCards[p.indexPlayer];
                switch (p.indexPlayer)
                {
                    case 0:
                        if (indexCard == 0)
                        {
                            handPlayer[0][0].Location = new Point(card.Location.X + deck_width / 2 + offset, (deck_location.Y + handPlayer[0][0].Location.Y) / 2);
                            handPlayer[0][0].BringToFront();
                        }
                        else
                        {
                            handPlayer[0][1].Location = new Point(card.Location.X - deck_width / 2 + offset, (deck_location.Y + handPlayer[0][1].Location.Y) / 2);
                            handPlayer[0][1].BringToFront();
                        }
                        break;
                    case 1:
                        if (indexCard == 0)
                        {
                            handPlayer[1][0].Location = new Point((handPlayer[1][0].Location.X + deck_location.X) / 2, card.Location.Y + handPlayer[1][0].Height / 2 + offset);
                            handPlayer[1][0].BringToFront();
                        }
                        else
                        {
                            handPlayer[1][1].Location = new Point((handPlayer[1][0].Location.X + deck_location.X) / 2, card.Location.Y - handPlayer[1][1].Height / 2 + offset);
                            handPlayer[1][1].BringToFront();
                        }
                        break;
                    case 2:
                        if (indexCard == 0)
                        {
                            handPlayer[2][0].Location = new Point(card.Location.X + deck_width / 2 + offset, (deck_location.Y - handPlayer[2][0].Location.Y) / 2);
                            handPlayer[2][0].BringToFront();

                        }
                        else
                        {
                            handPlayer[2][1].Location = new Point(card.Location.X - deck_width / 2 + offset, (deck_location.Y + handPlayer[2][1].Location.Y) / 2);
                            handPlayer[2][1].BringToFront();
                        }
                        break;
                    case 3:
                        if (indexCard == 0)
                        {
                            handPlayer[3][0].Location = new Point((handPlayer[3][0].Location.X + deck_location.X) / 2, card.Location.Y + handPlayer[3][0].Height / 2 + offset);
                            handPlayer[3][0].BringToFront();
                        }
                        else
                        {
                            handPlayer[3][1].Location = new Point((handPlayer[3][0].Location.X + deck_location.X) / 2, card.Location.Y - handPlayer[3][1].Height / 2 + offset);
                            handPlayer[3][1].BringToFront();
                        }
                        break;
                }
                this.updatePictureBox(card, otherCard, c, (RotateFlipType)p.indexPlayer, p);
                if (indexCard == 0) handPlayer[p.indexPlayer][0] = handPlayer[p.indexPlayer][1];
                PlaceCard(p.indexPlayer, 0, 1);
            }));

        }

        private void downSizeCard(PictureBox pb)
        {
            Invoke(new Action(() =>
            {
                if (pb.Size.Width > deck_width && pb.Size.Height > deck_height)
                {
                    this.Controls.Add(bufferedPreviousCard);
                    pb.Hide();
                    pb.Size = new Size((int)(pb.Size.Width / 1.2), (int)(pb.Size.Height / 1.2));
                    int replacementX = (int)(0.2 * pb.Size.Width) / 2;//to compensate the one vector grow with relocation
                    int replacementY = (int)(0.2 * pb.Size.Height) / 2;
                    pb.Location = new Point(pb.Location.X + replacementX, pb.Location.Y + replacementY);
                    pb.Refresh();
                    pb.Show();
                    this.Controls.Remove(bufferedPreviousCard);
                }
            }));
        }

        //remove 3 cards and show them on the board (only used for 2 player game)
        public void removeCardFromGameDeck2Players(Card c)
        {

            offsetRemovedCard = +this.indexRemovedCard * (deck_width / 2); // offset so the cards are not stack in the view

            this.removedCards[this.indexRemovedCard] = new PictureBox();
            PictureBox pb = this.removedCards[this.indexRemovedCard];

            pb.Location = new Point(this.deck_location.X + deck_width + 50 + offsetRemovedCard, this.deck_location.Y);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Size = new Size(deck_width, deck_height);
            pb.Image = c.picture;
            pb.Tag = c.texte;
            pb.MouseHover += new EventHandler(displayCardEffect);
            this.indexRemovedCard++;

            this.Controls.Add(pb);
        }



        //show a tool tip text containing the card effect on a mouseHover event
        public void displayCardEffect(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            effect.RemoveAll();
            effect = new ToolTip();
            effect.SetToolTip(pb, (String)pb.Tag);
        }

        //remove 1 card from the top of the deck
        public void remove1CardFromGameDeck(Card c)
        {
            PictureBox pb = new PictureBox();

            pb.Location = new Point(this.deck_location.X + deck_width + 50, this.deck_location.Y);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Size = new Size(deck_width, deck_height);
            pb.Image = c.picture;
            pb.Tag = c.texte;
            pb.MouseHover += new EventHandler(displayCardEffect);
            this.indexRemovedCard++;

            this.Controls.Add(pb);
        }

        //Place a card when there are 1 or 2 cards
        public void PlaceCard(int indexPlayer, int indexCard, int numberOfCards)
        {
            Invoke(new Action(() =>
            {
                PictureBox card = handPlayer[indexPlayer][indexCard];

                if (indexPlayer == 0) downSizeCard(card);

                int offset = (deck_width / 2) * ((((indexCard == 0) ? -1 : 1) + ((numberOfCards == 1) ? 0 : -1)));

                if (indexPlayer == 0)
                    card.Location = new Point(this.Width / 2 + offset, this.Height - deck_height);
                else if (indexPlayer == 1)
                    card.Location = new Point(0, this.Height / 2 + offset);
                else if (indexPlayer == 2)
                    card.Location = new Point(this.Width / 2 + offset, 0);
                else if (indexPlayer == 3)
                    card.Location = new Point(this.Width - deck_height, this.Height / 2 + offset);
            }));
        }

        //move a card from the deck to the indexPlayer in the game players list
        public void PickCard(int indexPlayer)
        {
            
            int indexCard = (handPlayer[indexPlayer][0] == null) ? 0 : 1;

            handPlayer[indexPlayer][indexCard] = new PictureBox();
            PictureBox card = handPlayer[indexPlayer][indexCard];
            card.Size = new Size(((indexPlayer % 2) == 0) ? deck_width : deck_height, ((indexPlayer % 2) == 0) ? deck_height : deck_width);

            if (indexPlayer == 0)
            {
                card.MouseHover += new EventHandler(displayCardEffect);
                card.MouseEnter += new EventHandler(card_MouseEnter);
                card.MouseLeave += new EventHandler(card_MouseLeave);
                card.Click += new EventHandler(card_Click);

                card.Image = Game.Instance.players[0].player_Deck[indexCard].picture;
                card.Tag = Game.Instance.players[0].player_Deck[indexCard].texte;
            }
            else
            {
                card.Image = Properties.Resources.backcard2;
            }

            card.Image.RotateFlip((System.Drawing.RotateFlipType)indexPlayer);
            card.SizeMode = PictureBoxSizeMode.StretchImage;

            if (indexCard == 0)
            {
                PlaceCard(indexPlayer, 0, 1);
            }
            else if (indexCard == 1)
            {
                PlaceCard(indexPlayer, 0, 2);
                PlaceCard(indexPlayer, 1, 2);
            }
            Invoke(new Action(() =>
            {
                this.Controls.Add(card);
                this.Update();
                cardSound.Play();
            }));
            
        }

        //show the hand of the player and add tooltip to it
        public void showCardsHand(String txt, Player target)
        {
            this.ResumeLayout();
            Label lbl = new Label();
            lbl.Text = txt;
            lbl.AutoSize = true;
            lbl.Font = new Font("Times new Roman", (this.Width / this.Height) * 75, FontStyle.Italic);
            lbl.ForeColor = Color.FromArgb(255, Color.White);
            lbl.BackColor = Color.Transparent;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            Panel trick = new Panel();
            trick.BackColor = Color.FromArgb(125, Color.Black);
            trick.AutoSize = true;
            trick.Controls.Add(lbl);

            lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height * 3);

            trick.Dock = DockStyle.Fill;

            Invoke(new Action(() =>
            {
            this.Controls.Add(trick);
            trick.BringToFront();

            //button to close the panel
            Button btn = new Button();
            btn.Text = "Ok !";
            btn.Font = new Font("Times new Roman", (btn.Height / btn.Height) * 20, FontStyle.Italic);
            btn.BackColor = Color.FromArgb(160, 110, 60);
            btn.ForeColor = Color.Black;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.Size = this.back_btn.Size;
            btn.Location = new Point(this.Width / 2 - btn.Width / 2, this.Height - btn.Height * 3);
            btn.Click += new EventHandler(button_Click_sentSignal);
            trick.Controls.Add(btn);

            int space = this.Width - target.player_Deck.Count * deck_width;
            space = space / target.player_Deck.Count;
            int xPosition = space / 2;

                foreach (Card c in target.player_Deck) // create a picture box for each type of cartes
                {
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Size = new Size(deck_width, deck_height);
                    pb.Image = c.picture;
                    pb.Name = Card.getCardNameByValue(c.value); 
                    pb.Location = new Point(xPosition, this.Height / 2 - (deck.Height / 2));
                    xPosition += deck_width + space;
                    pb.Click += new EventHandler(card_Click_SelectType);
                    trick.Controls.Add(pb);
                    pb.Update();
                }
                //add btn              

                //anim
                lbl.ForeColor = Color.FromArgb(0, Color.White);
                trick.Refresh();
            }));
            mre.Reset();//erase former data
            mre.Wait();//wait for the event to be fired

            Invoke(new Action(() =>
            {
                trick.Hide();
                this.Controls.Remove(trick);
            }));
        }

        //compare the hand of 2 player
        public void compareCardsHand(String txt, Player source, Player target)
        {
            this.ResumeLayout();
            Label lbl = indicationLabel(txt);

            Panel trick = new Panel();
            trick.BackColor = Color.FromArgb(125, Color.Black);
            trick.AutoSize = true;
            trick.Controls.Add(lbl);

            lbl.Location = new Point(this.Width / 2 - lbl.Width / 2,  lbl.Height/2 );

            trick.Dock = DockStyle.Fill;

            Invoke(new Action(() =>
            {
                this.Controls.Add(trick);
                trick.BringToFront();

                int xPosition = this.Width/2 - deck_width*2;

                foreach (Card c in target.player_Deck) // create a picture box each target cards
                {
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Size = new Size(deck_width, deck_height);
                    pb.Image = c.picture;
                    pb.Name = Card.getCardNameByValue(c.value);
                    pb.Location = new Point(xPosition, this.Height / 2 - (deck.Height / 2));
                    xPosition += deck_width *3;
                    pb.Click += new EventHandler(card_Click_SelectType);
                    trick.Controls.Add(pb);
                    pb.Update();
                }

                foreach (Card c in source.player_Deck) // create a picture box for source cards
                {
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Size = new Size(deck_width, deck_height);
                    pb.Image = c.picture;
                    pb.Name = Card.getCardNameByValue(c.value); 
                    pb.Location = new Point(xPosition, this.Height / 2 - (deck.Height / 2));
                    xPosition += deck_width;
                    pb.Click += new EventHandler(card_Click_SelectType);
                    trick.Controls.Add(pb);
                    pb.Update();
                }


                //anim
                lbl.ForeColor = Color.FromArgb(0, Color.White);
                trick.Refresh();
            }));
            Thread.Sleep(2000);//wait 2 seconds
            Invoke(new Action(() =>
            {
                trick.Hide();
                this.Controls.Remove(trick);
            }));
        }

        //hide the hand of the player and remove tooltip to it
        public void hideCardsHand(Player player)
        {
            int i = 0;
            PictureBox pb = this.handPlayer[player.indexPlayer][i];
            foreach (Card c in player.player_Deck)
            {
                pb.Image = Properties.Resources.backcard2;
                pb.Tag = c.texte;
                pb.MouseEnter -= new EventHandler(card_MouseEnter);
                pb.MouseLeave -= new EventHandler(card_MouseLeave);
                pb.Image.RotateFlip((RotateFlipType)player.indexPlayer);
                this.Refresh();
                i++;
            }
        }


        //update hand of a human player
        public void updateHand(Player player)
        {
            Invoke(new Action(() =>
            {
                int i = 0;
                foreach (Card c in player.player_Deck)
                {
                    Bitmap tmpBmp = new Bitmap(c.picture);
                    handPlayer[player.indexPlayer][i].Image = tmpBmp;
                    handPlayer[player.indexPlayer][i].Image.RotateFlip((RotateFlipType)player.indexPlayer);
                    handPlayer[player.indexPlayer][i].Refresh();
                }
            }));
        }

        //show big white indications on the center of the screen for a couple of seconds
        //type = show deck or show player hand
        public void showIndications(String txt,String type)
        {
            if (tuto)
            {
                this.ResumeLayout();
                Label lbl = indicationLabel(txt);

                Panel trick = new Panel();
                trick.BackColor = Color.FromArgb(125, Color.Black);
                trick.AutoSize = true;
                trick.Controls.Add(lbl);

                if(type=="deck")
                {
                    lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height * 3 );//center of the screen + offset
                    trick.Controls.Add(deck);
                }
                else if(type=="player")
                {
                    lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height / 2);//center of the screen
                    trick.Controls.Add(handPlayer[0][0]);
                    trick.Controls.Add(handPlayer[0][1]);
                }
                else if(type=="end of game")
                {
                    lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height * 3);//center of the screen + offset
                    trick.Controls.Add(deck);
                    trick.Controls.Add(handPlayer[0][0]);
                    trick.Controls.Add(handPlayer[1][0]);
                    trick.Controls.Add(handPlayer[2][0]);
                    trick.Controls.Add(handPlayer[3][0]);
                    trick.Controls.Add(back_btn);
                    trick.Controls.Add(btn_pause);
                    for(int i =0; i<Properties.Settings.Default.nbrPlayers;i++)
                    {
                        handPlayer[i][0].Click -= new EventHandler(card_Click);
                        handPlayer[i][0].MouseEnter -= new EventHandler(card_MouseEnter);
                        handPlayer[i][0].MouseLeave -= new EventHandler(card_MouseLeave);
                        handPlayer[i][0].MouseHover -= new EventHandler(displayCardEffect);
                    }
                    deck.MouseEnter -= new EventHandler(card_MouseEnter);
                    deck.MouseLeave -= new EventHandler(card_MouseLeave);
                    deck.Click -= new EventHandler(deck_Click);
                }
                

                trick.Dock = DockStyle.Fill;
                this.Controls.Add(trick);
                trick.BringToFront();

                //anim
                lbl.ForeColor = Color.FromArgb(0, Color.White);
                trick.Refresh();
                if(type!="end of game")
                {
                    Thread.Sleep(1000);//wait 1 seconds
                    trick.Hide();
                    this.Controls.Add(handPlayer[0][0]);
                    this.Controls.Add(handPlayer[0][1]);
                    this.Controls.Add(deck);
                    this.Controls.Remove(trick);
                }        
            }
        }

        //show a message in the middle of the screen
        public void showMessage(String txt, Boolean mustShow)
        {
            this.addLog(txt);
            if(mustShow)
            {
                Label lbl = indicationLabel(txt);
                Panel trick = new Panel();
                trick.BackColor = Color.FromArgb(125, Color.Black);

                trick.AutoSize = true;
                trick.Controls.Add(lbl);
                lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height / 2);//center of the screen
                                                                                                           //care label is outside the panel
                trick.Dock = DockStyle.Fill;

                Invoke(new Action(() =>
                {
                    this.Controls.Add(trick);
                    trick.BringToFront();
                    trick.Refresh();
                }));

                Thread.Sleep(2000);//wait 2 seconds

                Invoke(new Action(() =>
                {
                    trick.Hide();
                    this.Controls.Remove(trick);
                }));
            }            
        }

        public int selectPlayer(String txt,Boolean itself, List<Player> players)
        {
                this.ResumeLayout();
                Label lbl  = indicationLabel(txt);

                Panel trick = new Panel();
                trick.BackColor = Color.FromArgb(125, Color.Black);

                trick.AutoSize = true;
                trick.Controls.Add(lbl);

                lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height / 2);//center of the screen

                trick.Dock = DockStyle.Fill;

            Invoke(new Action(() =>
            {
                this.Controls.Add(trick);
                trick.BringToFront();

                foreach (Player p in players)
                {
                    if (itself && p.indexPlayer==0)
                    {
                        trick.Controls.Add(handPlayer[0][0]);
                        handPlayer[p.indexPlayer][0].MouseHover -= new EventHandler(displayCardEffect); //remove old event on the player card
                        handPlayer[p.indexPlayer][0].MouseEnter -= new EventHandler(card_MouseEnter);
                        handPlayer[p.indexPlayer][0].MouseLeave -= new EventHandler(card_MouseLeave);
                        handPlayer[p.indexPlayer][0].Click -= new EventHandler(card_Click);
                        handPlayer[p.indexPlayer][0].Click += new EventHandler(card_Click_Select);

                        //the control has been resized so we need to down size it
                        if(handPlayer[p.indexPlayer][0].Width>deck_width)
                        {
                            downSizeCard(handPlayer[p.indexPlayer][0]);
                        }
                    }
                    trick.Controls.Add(handPlayer[p.indexPlayer][0]);
                    handPlayer[p.indexPlayer][0].Click += new EventHandler(card_Click_Select);
                }

                //anim
                lbl.ForeColor = Color.FromArgb(0, Color.White);
                trick.Refresh();
            }));
            mre.Reset();//erase former data
            mre.Wait();//wait for the event to be fired
            Invoke(new Action(() =>
            {
                trick.Hide();
                foreach (Player p in players)
                {
                    if (itself && p.indexPlayer == 0)
                    {
                        trick.Controls.Add(handPlayer[0][0]);
                        handPlayer[p.indexPlayer][0].MouseHover += new EventHandler(displayCardEffect); //add old event on the player card
                        handPlayer[p.indexPlayer][0].MouseEnter += new EventHandler(card_MouseEnter);
                        handPlayer[p.indexPlayer][0].MouseLeave += new EventHandler(card_MouseLeave);
                        handPlayer[p.indexPlayer][0].Click += new EventHandler(card_Click);                       
                    }
                    handPlayer[p.indexPlayer][0].Click -= new EventHandler(card_Click_Select);
                    this.Controls.Add(handPlayer[p.indexPlayer][0]);
                }
                
                this.Controls.Remove(trick);

            }));
            return selectedPlayerNum;
        }


        public String selectedTypeOfCards(String txt,List<Card> cards)
        {
            this.ResumeLayout();
            Label lbl = indicationLabel(txt);

            Panel trick = new Panel();
            trick.BackColor = Color.FromArgb(125, Color.Black);
            trick.AutoSize = true;
            trick.Controls.Add(lbl);

            lbl.Location = new Point(this.Width / 2 - lbl.Width / 2, this.Height / 2 - lbl.Height * 3);

            trick.Dock = DockStyle.Fill;
            Invoke(new Action(() =>
            {
                this.Controls.Add(trick);
                trick.BringToFront();

                int space = this.Width - cards.Count * deck_width;
                space = space / cards.Count;
                int xPosition = space/2;

                foreach (Card c in cards) // create a picture box for each type of cartes
                {
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Size = new Size(deck_width, deck_height);
                    pb.Image = c.picture;
                    pb.Name = c.value.ToString();
                    pb.Location = new Point(xPosition, this.Height / 2 - (deck.Height / 2));
                    xPosition += deck_width + space;
                    pb.Click += new EventHandler(card_Click_SelectType);
                    trick.Controls.Add(pb);
                    pb.Update();
                }

                //anim
                lbl.ForeColor = Color.FromArgb(0, Color.White);
                trick.Refresh();
            }));
            mre.Reset();//erase former data
            mre.Wait();//wait for the event to be fired

            Invoke(new Action(() =>
            {
                trick.Hide();
                this.Controls.Remove(trick);
            }));         

            return selectedTypeCard;
        }

        private void StartGame()
        {
            //start the game
            GameController.gameView = this;          
        }

        //event controlbox

        private void x_Click(object sender, EventArgs e)
        {
            cardPick.Stop();
            cardPick.Close();
            cardSound.Stop();
            cardSound.Close();
            ambianceSound.Stop();
            ambianceSound.Close();
            System.Windows.Forms.Application.Exit();
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            backToMenu();
        }

        public void backToMenu()
        {
            //stop music to avoid thread collision
            cardPick.Stop();
            cardPick.Close();
            cardSound.Stop();
            cardSound.Close();
            ambianceSound.Stop();
            ambianceSound.Close();
            this.Hide();
            Game.Instance.eraseGame();
            Form_Menu fm = new Form_Menu();
            fm.ShowDialog();
            this.Close();
        }

        private void deck_Click(object sender, EventArgs e)
        {
            //downSizeCard(deck);
            GameController.PickCard(GameController.currentPlayer);
            if (GameController.currentPlayer.isHuman)
            {
                showIndications(Properties._string.Play1Card, "player");
            }
        }

        //event for player 0 which is human player
        private void card_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            int index = 0;
            if (pb.Equals(handPlayer[0][0]))
            {
                index = 0;
            }
            else if (pb.Equals(handPlayer[0][1]))
            {
                index = 1;
            }
            this.SuspendLayout();
            GameController.PlayCard(GameController.currentPlayer.player_Deck[index], GameController.currentPlayer);
            this.ResumeLayout();
        }

        //event for player 0 to choose opponent
        private void card_Click_Select(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            for (int i = 0; i < 4; i++)
            {
                if (pb.Equals(handPlayer[i][0]))
                {
                    selectedPlayerNum = i;
                }
            }
            cardPick.Play();
            mre.Set();//sig event is fired
        }


        //just send sig event
        private void button_Click_sentSignal(object sender, EventArgs e)
        {
            cardPick.Play();
            mre.Set();
        }

        //event for player 0 to choose opponent
        private void card_Click_SelectType(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            this.selectedTypeCard = pb.Name;
            cardPick.Play();
            mre.Set();//sig event is fired
        }
        
        private void card_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if(pb.Size.Width-deck_width<2 || pb.Size.Width - deck_width > -2)//to avoid specific conditions in pause menu
            {
                bufferedPreviousCard = new PictureBox { Image = pb.Image, Size = pb.Size, SizeMode = pb.SizeMode, Location = pb.Location, Tag = pb.Tag };
                int replacementX = (int)(0.2 * pb.Size.Width) / 2;//to compensate the one vector grow with relocation
                int replacementY = (int)(0.2 * pb.Size.Height) / 2;
                pb.Size = new Size((int)(pb.Size.Width * 1.2), (int)(pb.Size.Height * 1.2));
                pb.Location = new Point(pb.Location.X - replacementX, pb.Location.Y - replacementY);
                pb.BringToFront();
                pb.Refresh();
            }
        }

        private void card_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            downSizeCard(pb);
        }

        private void ambiance_Ended(object sender, EventArgs e)
        {
            System.Windows.Media.MediaPlayer md = (System.Windows.Media.MediaPlayer)sender;
            md.Stop();
            md.Position = TimeSpan.Zero;
            md.Play();
        }

        private void cardSound_Ended(object sender, EventArgs e)
        {
            System.Windows.Media.MediaPlayer md = (System.Windows.Media.MediaPlayer)sender;
            md.Stop();
            md.Position = TimeSpan.Zero;
        }

        //initialize a title label for screen indications
        public Label indicationLabel(String txt)
        {
            double height = (this.Height*0.05)* (this.Height *0.05);
            double width = (this.Width * 0.05) * (this.Width * 0.05);
            double hyp = Math.Sqrt(height + width)*0.5;

            Label lbl = new Label();

            lbl.Text = txt;
            lbl.AutoSize = true;
            lbl.Font = new Font("Times new Roman", (int) hyp, FontStyle.Italic);
            lbl.MaximumSize = new Size(this.Width, this.Height);                        
            lbl.ForeColor = Color.FromArgb(255, Color.White);
            lbl.BackColor = Color.Transparent;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            return lbl;
        }

        //show the pause menu in game
        private void showMenu()
        {
            this.ResumeLayout();

            Invoke(new Action(() =>
            {
                cardPick.Play();
            }));
                
            if (pause == false)
            {
                pause = true;

                Panel trick = new Panel();
                trick.BackColor = Color.FromArgb(125, Color.Black);

                trick.AutoSize = true;
                trick.Click += new EventHandler(button_Click_sentSignal);
                trick.Dock = DockStyle.Fill;
                Invoke(new Action(() =>
                {
                    btn_settings.Visible = true;
                    btn_tuto.Visible = true;
                    btn_exit.Visible = true;
                    back_btn.Visible = true;
                    btn_visualSettings.Visible = true;
                    trick.Controls.Add(btn_pause);
                    trick.Controls.Add(btn_settings);
                    trick.Controls.Add(btn_tuto);
                    trick.Controls.Add(back_btn);
                    trick.Controls.Add(btn_exit);
                    trick.Controls.Add(btn_visualSettings);
                    //deleting events on deck
                    deck.MouseEnter -= new EventHandler(card_MouseEnter);
                    deck.MouseLeave -= new EventHandler(card_MouseLeave);
                    deck.Click -= new EventHandler(deck_Click);
                    deck.Click += new EventHandler(button_Click_sentSignal);
                    trick.Controls.Add(deck);
                    this.Controls.Add(trick);
                    trick.BringToFront();
                    trick.Refresh();
                }));
                mre.Reset();
                mre.Wait();//wait for the event to be fired
                Invoke(new Action(() =>
                {
                    pause = false;
                    btn_settings.Visible = false;
                    btn_tuto.Visible = false;
                    btn_exit.Visible = false;
                    back_btn.Visible = false;
                    btn_visualSettings.Visible = false;
                    this.Controls.Add(btn_pause);
                    this.Controls.Add(btn_settings);
                    this.Controls.Add(btn_tuto);
                    this.Controls.Add(back_btn);
                    this.Controls.Add(btn_exit);
                    this.Controls.Add(btn_visualSettings);
                    //putting back events on deck
                    deck.Click -= new EventHandler(button_Click_sentSignal);
                    deck.Click += new EventHandler(deck_Click);
                    deck.MouseEnter += new EventHandler(card_MouseEnter);
                    deck.MouseLeave += new EventHandler(card_MouseLeave);
                    this.Controls.Add(deck);
                    trick.Hide();
                    this.Controls.Remove(trick);
                }));
            }
            else
            {
                
                mre.Set();
            }
        }


        //show sound settings
        private void showSoundSettings()
        {
            this.ResumeLayout();
            mre.Set();
                Panel trick = new Panel();
                trick.BackColor = Color.FromArgb(125, Color.Black);

                trick.AutoSize = true;
                trick.Dock = DockStyle.Fill;
                Invoke(new Action(() =>
                {
                    //button to close the panel
                    Button btn = new Button();
                    btn.Text = "Ok !";
                    btn.Font = new Font("Times new Roman", (btn.Height / btn.Height) * 20, FontStyle.Italic);
                    btn.BackColor = Color.FromArgb(160, 110, 60);
                    btn.ForeColor = Color.Black;
                    btn.FlatAppearance.BorderColor = Color.Black;
                    btn.Size = this.back_btn.Size;
                    
                    btn.Click += new EventHandler(button_Click_sentSignal);

                    //offset
                    int offset = this.Height / 5;

                    //main sound track bar
                    TrackBar main_tb = new TrackBar();
                    main_tb.SmallChange = 2;
                    main_tb.LargeChange = 10;
                    main_tb.BackColor = Color.FromArgb(160, 110, 60);
                    main_tb.TickStyle = TickStyle.None;
                    main_tb.Maximum = 100;
                    main_tb.Value = (int)(mainVolume * 100);
                    main_tb.Size = new Size((int)(this.Width / 4), (int)(this.Height / 32));
                    main_tb.Location = new Point(this.Width / 2 - main_tb.Size.Width / 2, this.Height/4);
                    main_tb.Scroll += new EventHandler(tb_main_Changed);
                    //main sound label
                    Label main_lbl = new Label();
                    main_lbl.Text = Properties._string.masterVolume;
                    main_lbl.AutoSize = true;
                    main_lbl.Font= new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                    main_lbl.BackColor = Color.Transparent;
                    main_lbl.ForeColor = Color.FromArgb(255, Color.White);
                    main_lbl.Location = new Point(this.Width / 2 - (int)(main_lbl.Width *1.5), main_tb.Location.Y - main_lbl.Size.Height *3);

                    //Music sound track bar
                    TrackBar music_tb = new TrackBar();
                    music_tb.SmallChange = 2;
                    music_tb.LargeChange = 10;
                    music_tb.BackColor = Color.FromArgb(160, 110, 60);
                    music_tb.TickStyle = TickStyle.None;
                    music_tb.Maximum = 100;
                    music_tb.Value = (int)(musicVolume*100);
                    music_tb.Size = new Size((int)(this.Width / 4), (int)(this.Height / 32));
                    music_tb.Location = new Point(this.Width / 2 - music_tb.Size.Width / 2, main_tb.Location.Y+offset);
                    music_tb.Scroll += new EventHandler(tb_music_Changed);
                    //music sound label
                    Label music_lbl = new Label();
                    music_lbl.Text = Properties._string.musicVolume;
                    music_lbl.AutoSize = true;
                    music_lbl.Font = new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                    music_lbl.BackColor = Color.Transparent;
                    music_lbl.ForeColor = Color.FromArgb(255, Color.White);
                    music_lbl.Location = new Point(this.Width / 2 - (int)(music_lbl.Width*1.5), music_tb.Location.Y - music_lbl.Size.Height * 3);

                    //Effect sound track bar
                    TrackBar effect_tb = new TrackBar();
                    music_tb.SmallChange = 2;
                    music_tb.LargeChange = 10;
                    effect_tb.BackColor = Color.FromArgb(160, 110, 60);
                    effect_tb.TickStyle = TickStyle.None;
                    effect_tb.Maximum = 100;
                    effect_tb.Value = (int)(effectVolume * 100);
                    effect_tb.Size = new Size((int)(this.Width / 4), (int)(this.Height / 32));
                    effect_tb.Location = new Point(this.Width / 2 - effect_tb.Size.Width / 2, music_tb.Location.Y + offset);
                    effect_tb.MouseUp += new MouseEventHandler(tb_effect_MouseUp);
                    //effect sound label
                    Label effect_lbl = new Label();
                    effect_lbl.Text = Properties._string.effectVolume;
                    effect_lbl.AutoSize = true;
                    effect_lbl.Font = new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                    effect_lbl.BackColor = Color.Transparent;
                    effect_lbl.ForeColor = Color.FromArgb(255, Color.White);
                    effect_lbl.Location = new Point(this.Width / 2 - (int)(effect_lbl.Width*1.5), effect_tb.Location.Y - effect_lbl.Size.Height * 3);

                    CheckBox mute_cb = new CheckBox();
                    mute_cb.Text = Properties._string.mute;
                    mute_cb.AutoSize = true;
                    mute_cb.Font = new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                    mute_cb.BackColor = Color.Transparent;
                    mute_cb.ForeColor = Color.FromArgb(255, Color.White);
                    mute_cb.Location = new Point(this.Width / 2 - mute_cb.Width / 2, effect_tb.Location.Y + offset / 2);
                    mute_cb.Checked = muted;
                    mute_cb.CheckedChanged += new EventHandler(mute_cb_Click);

                    //btn location
                    btn.Location = new Point(this.Width / 2 - btn.Width / 2, mute_cb.Location.Y + offset / 2);
                    //adding components
                    trick.Controls.Add(main_tb);
                    trick.Controls.Add(main_lbl);
                    trick.Controls.Add(music_tb);
                    trick.Controls.Add(music_lbl);
                    trick.Controls.Add(effect_tb);
                    trick.Controls.Add(effect_lbl);
                    trick.Controls.Add(mute_cb);


                    trick.Controls.Add(btn);
                    this.Controls.Add(trick);
                    trick.BringToFront();
                    trick.Refresh();
                }));
                mre.Reset();
                mre.Wait();//wait for the event to be fired
                Invoke(new Action(() =>
                {
                    trick.Hide();
                    this.Controls.Remove(trick);
                }));
        }

        private void showVisualSettings()
        {
            this.ResumeLayout();
            mre.Set();
            Panel trick = new Panel();
            trick.BackColor = Color.FromArgb(125, Color.Black);

            trick.AutoSize = true;
            trick.Dock = DockStyle.Fill;
            Invoke(new Action(() =>
            {

                int offset = this.Height / 5;

                //button to close the panel
                Button btn = new Button();
                btn.Text = "Ok !";
                btn.Font = new Font("Times new Roman", (btn.Height / btn.Height) * 20, FontStyle.Italic);
                btn.BackColor = Color.FromArgb(160, 110, 60);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.Size = this.back_btn.Size;
                btn.Click += new EventHandler(button_Click_sentSignal);


                //message box checkbox
                CheckBox logBox_cb = new CheckBox();
                logBox_cb.Text = Properties._string.showMessageBox;
                logBox_cb.AutoSize = true;
                logBox_cb.Font = new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                logBox_cb.BackColor = Color.Transparent;
                logBox_cb.ForeColor = Color.FromArgb(255, Color.White);
                logBox_cb.Location = new Point(this.Width / 2 - logBox_cb.Width / 2, this.Height/3);
                logBox_cb.Checked = this.logBox.Visible;
                logBox_cb.CheckStateChanged += new EventHandler(logBox_cb_checked);

                //message box checkbox
                CheckBox message_cb = new CheckBox();
                message_cb.Text = Properties._string.showMessageFullscreen;
                message_cb.AutoSize = true;
                message_cb.Checked = Properties.Settings.Default.showMessage;
                message_cb.Font = new Font("Times new Roman", (this.Width / this.Height) * 30, FontStyle.Italic);
                message_cb.BackColor = Color.Transparent;
                message_cb.ForeColor = Color.FromArgb(255, Color.White);
                message_cb.Location = new Point(this.Width / 2 - message_cb.Width / 2, logBox_cb.Location.Y + offset);
                message_cb.CheckStateChanged += new EventHandler(message_cb_checked);


                //btn location
                btn.Location = new Point(this.Width / 2 - btn.Width / 2, message_cb.Location.Y + offset);

                trick.Controls.Add(btn);
                trick.Controls.Add(logBox_cb);
                trick.Controls.Add(message_cb);

                this.Controls.Add(trick);
                trick.BringToFront();
                trick.Refresh();
            }));
            mre.Reset();
            mre.Wait();//wait for the event to be fired
            Invoke(new Action(() =>
            {
                trick.Hide();
                this.Controls.Remove(trick);
            }));
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            cardPick.Play();
            Task t = new Task(() => showMenu());
            t.Start();
        }

        private void mute_cb_Click(object sender, EventArgs e)
        {
            cardPick.Play();
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked)
            {
                muted = true;
                ambianceSound.Volume = 0;
                cardPick.Volume = 0;
                cardSound.Volume = 0;
            }
            else
            {
                muted = false;
                ambianceSound.Volume = musicVolume;
                cardPick.Volume = effectVolume;
                cardSound.Volume = effectVolume;
            }
        }

        private void tb_effect_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            cardPick.Volume = (double)tb.Value / 100;
            cardSound.Volume = (double)tb.Value / 100;
            effectVolume = (double)tb.Value / 100;
            cardPick.Play();
        }

        private void tb_main_Changed(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            ambianceSound.Volume = (double)tb.Value / 100;
            musicVolume = (double)tb.Value / 100;
            cardPick.Volume = (double)tb.Value / 100;
            cardSound.Volume = (double)tb.Value / 100;
            effectVolume = (double)tb.Value / 100;
            mainVolume = (double)tb.Value / 100; ;
        }

        private void tb_music_Changed(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            ambianceSound.Volume = (double)tb.Value / 100;
            musicVolume = (double)tb.Value / 100;
        }

        private void btn_tuto_Click(object sender, EventArgs e)
        {
            cardPick.Play();
            if (tuto)
            {
                tuto = false;
                btn_tuto.Text = Properties._string.ActivateTutorial;
            }
            else
            {
                tuto = true;
                btn_tuto.Text = Properties._string.DesactivateTutorial;
            }
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            cardPick.Play();
            Task t = new Task(() => showSoundSettings());
            t.Start();
        }

        private void btn_visualSettings_Click(object sender, EventArgs e)
        {
            cardPick.Play();
            Task t = new Task(() => showVisualSettings());
            t.Start();
        }

        private void logBox_cb_checked(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked)
            {
                Properties.Settings.Default.showMessageBox = true;
                this.logBox.Show();
            }
            else
            {
                Properties.Settings.Default.showMessageBox = false;
                this.logBox.Hide();
            }
        }

        private void message_cb_checked(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                Properties.Settings.Default.showMessage = true;
            }
            else
            {
                Properties.Settings.Default.showMessage = false;
            }
        }
    }
}