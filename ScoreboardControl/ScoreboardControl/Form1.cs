using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapstoneTest.Models;
using Newtonsoft.Json;

namespace ScoreboardControl
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        public Form1()
        {
            CenterToScreen();
            InitializeComponent();
            client.BaseAddress = new Uri("http://142.55.32.86:50291/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void btnJoin_Click(object sender, EventArgs e)
        {
            string leagueKey = tbLeagueKey.Text;
            string password = GetPasswordHash(tbPassword.Text);

            LoginObject login = null;
            var path = "http://142.55.32.86:50291/api/Login?leagueKey=" + leagueKey + "&hashedPassword=" + password;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string loginObjectString = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(loginObjectString))
                {
                    MessageBox.Show("Unable to log in, please check your credentials and internet connection.", "Unable to join League", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    login = JsonConvert.DeserializeObject<LoginObject>(loginObjectString);
                    Properties.Settings.Default.LeagueId = login.LeagueId;
                    Properties.Settings.Default.LeagueKey = login.LeagueKey;
                    Properties.Settings.Default.LeagueLogo = login.Logo;
                    Properties.Settings.Default.LeagueName = login.LeagueName;
                    Properties.Settings.Default.LoginKey = login.Login.LoginKey;
                }

                this.Hide();
                JoinLeagueSuccess form2 = new JoinLeagueSuccess();
                form2.FormClosed += (s, args) => this.Close();
                form2.Show();
            }
            else
            {
                MessageBox.Show("Unable to log in, please check your credentials and internet connection.", "Unable to join League", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private string GetPasswordHash(string password)
        {
            string sSourceData = password;
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);


            return ByteArrayToString(tmpHash);
        }

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
