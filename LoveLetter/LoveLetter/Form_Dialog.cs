using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter.View
{
    
    public partial class Fm_Dialog : Form
    {
        String texte;
        Boolean showButton;

        public Fm_Dialog()
        {
            InitializeComponent();
        }


        public Fm_Dialog(Point p, String texte,Boolean showButton)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;

            Point p2 = new Point(p.X + 100, p.Y + 100);
            this.Location = p2;
            this.StartPosition = FormStartPosition.Manual;
            this.texte = texte;
            this.showButton = showButton;
            
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            lbl_text.AutoSize = true;
            lbl_text.Text = texte;
            if(!showButton)
            {
                btn_Ok.Hide();
            }
            this.Width = lbl_text.Width + 20;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
