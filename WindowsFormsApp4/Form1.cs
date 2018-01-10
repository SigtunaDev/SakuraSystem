using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public String account;
        MySqlConnection connection = MainPage.connection;
        public Boolean closed = false;

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.KeyDown += Login;
            textBox2.KeyDown += Login;
            this.FormClosing += new_Closing;
            VersionDetect();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        public void new_Closing(object sender,EventArgs e)
        {
            if (closed == false)
            {
                Environment.Exit(0);
            }
        }

        private void Login(object sender,KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String pre_command = String.Format("select 密碼 from `account` where 帳號= '{0}'", textBox1.Text);
                MySqlCommand msc = new MySqlCommand(pre_command, connection);
                MySqlDataReader reader = msc.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == textBox2.Text)
                    {
                        account = textBox1.Text;
                        closed = true;
                        this.Close();
                        MessageBox.Show("登入成功!", "LoginSuccessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("登入失敗! 帳號與密碼錯誤!","LoginFailed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                reader.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void VersionDetect()
        {
            WebClient wc = new WebClient();
            String version = wc.DownloadString("https://pastebin.com/raw/PuYuVVFb").Split('\n')[0].Split('=')[1];
            String now_version = "0.2";
            if (!version.Equals(now_version))
            {

            }
        }
    }
}
