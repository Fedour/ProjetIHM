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

namespace LoveLetter
{
    public partial class AskIPFormcs : Form
    {
        String pseudo;
        public AskIPFormcs(Point p, String pseudo)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            this.Location = p;
            this.StartPosition = FormStartPosition.Manual;
            this.pseudo = pseudo;
       



        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
           
            
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }

        private void AskIPFormcs_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
