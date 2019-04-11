using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoreboardControl
{
    public partial class JoinLeagueSuccess : Form
    {
        public JoinLeagueSuccess()
        {
            CenterToScreen();
            InitializeComponent();
        }

        private void JoinLeagueSuccess_Load(object sender, EventArgs e)
        {
            pbLeagueLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            Image image = Image.FromFile("Images/OCAA.jpg");
            pbLeagueLogo.Image = image;

            lblLeagueName.Text = "Now Viewing Page for: " + Properties.Settings.Default.LeagueName;
            lblLoginKey.Text = "Unique Login Key: " + Properties.Settings.Default.LoginKey;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 forn1 = new Form1();
            forn1.Show();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
