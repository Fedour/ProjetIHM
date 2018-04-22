using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter.View
{
    public partial class comboBoxDialog : Form
    {
        //targeted player
        private Player target = Game.Instance.players.ElementAt(0); 

        //value of choosed card
        private int cardType = Card.cardsType.ElementAt(0).value;

        //choosen card
        private Card card;

        //type of object (0 player (object), 1 cardType (int), 2 Card (object) )
        int type;

        public comboBoxDialog()
        {
            InitializeComponent();
        }


        //combobox constructor for player choice
        public comboBoxDialog(List<Player> values, int type)
        {
            InitializeComponent();

            this.type = type;

            foreach (Player p in values)
            {
                ComboboxItemPlayer i = new ComboboxItemPlayer();
                i.name = p.name;
                i.value = p;
                this.target_cmbBox.Items.Add(i);
            }
            this.Text = "Choose a player !";

            //default value for combobox 
            this.target_cmbBox.SelectedIndex = 0;
        }

        //combobox constructor for card choice in a player hand , list of cards and a boolena who indicate if the player choice a TYPE of card of a REFERENCE to a player card
        public comboBoxDialog(List<Card> values, int type)
        {
            InitializeComponent();

            this.type = type;

            if (type == 1) // we need the TYPE of the cards stored
            {
                foreach (Card p in values)
                {
                    ComboboxItemCardType i = new ComboboxItemCardType();
                    i.name = Card.getCardNameByValue(p.value);
                    i.value = p.value;
                    this.target_cmbBox.Items.Add(i);
                }

                this.Text = "Guess the type !";

                //default value for combobox 
                this.target_cmbBox.SelectedIndex = 0;
            }
            else // we need to store the card itself
            {
                foreach (Card p in values)
                {
                    ComboboxItemCard i = new ComboboxItemCard();
                    i.name = Card.getCardNameByValue(p.value);
                    i.value = p;
                    this.target_cmbBox.Items.Add(i);
                }
                this.Text = "Choose a card !";

                //default value
                //default value for combobox 
                this.target_cmbBox.SelectedIndex = 0;
            }

        }

        //accessors
        public Player getTarget()
        {
            return this.target;
        }

        public int getCardType()
        {
            return this.cardType;
        }

        public Card getCard()
        {
            return this.card;
        }
        private void comboBoxDialog_Load(object sender, EventArgs e)
        {

        }

        private void validateButtonTarget_btn_Click(object sender, EventArgs e)
        {
            if (type == 0)
            {
                ComboboxItemPlayer p = (ComboboxItemPlayer)this.target_cmbBox.SelectedItem;
                this.target = (Player)p.value;
            }
            else if (type == 1)
            {
                ComboboxItemCardType ct = (ComboboxItemCardType)this.target_cmbBox.SelectedItem;
                this.cardType = (int)ct.value;
            }
            else if (type == 2)
            {
                ComboboxItemCard c = (ComboboxItemCard)this.target_cmbBox.SelectedItem;
                this.card = (Card)c.value;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void target_cmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(type == 0)
            {
                if(this.target_cmbBox.SelectedIndex>=0)
                {
                    ComboboxItemPlayer cp = (ComboboxItemPlayer)this.target_cmbBox.Items[this.target_cmbBox.SelectedIndex];
                    Player p = (Player)cp.value;
                    if (p.HMProtected)
                    {
                        this.target_cmbBox.SelectedIndex = -1;
                    }
                }
            }
        }

        private void target_cmbBox_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            Font myFont = new Font("Aerial", 10, FontStyle.Regular);
            if (this.target_cmbBox.SelectedIndex >= 0)
            {
                if (type == 0)
                {

                    ComboboxItemPlayer cp = (ComboboxItemPlayer)this.target_cmbBox.Items[this.target_cmbBox.SelectedIndex];
                    Player p = (Player)cp.value;
                    if (p.HMProtected)
                    {
                        e.Graphics.DrawString(target_cmbBox.Items[e.Index].ToString(), myFont, Brushes.LightGray, e.Bounds);
                    }
                    else
                    {
                        e.DrawBackground();
                        e.Graphics.DrawString(target_cmbBox.Items[e.Index].ToString(), myFont, Brushes.Black, e.Bounds);
                        e.DrawFocusRectangle();
                    }
                }
                else
                {
                    e.DrawBackground();
                    e.Graphics.DrawString(target_cmbBox.Items[e.Index].ToString(), myFont, Brushes.Black, e.Bounds);
                    e.DrawFocusRectangle();
                }
            }
           
        }
    }


    /*
     * All data structure for combobox object 
     */
    public class ComboboxItemPlayer
    {
        public string name { get; set; }
        public Player value { get; set; }

        public override String ToString()
        {
            String n = this.name;

            if(value.HMProtected)
            {
                n += " (protected)";
            }
            return n;
        }
    }



    public class ComboboxItemCard
    {
        public string name { get; set; }
        public Card value { get; set; }

        public override String ToString()
        {
            return name;
        }
    }

    public class ComboboxItemCardType
    {
        public string name { get; set; }
        public int value { get; set; }

        public override String ToString()
        {
            return name;
        }
    }
}