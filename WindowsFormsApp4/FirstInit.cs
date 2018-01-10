using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;

namespace WindowsFormsApp4
{
    public partial class FirstInit : Form
    {
        public FirstInit()
        {
            InitializeComponent();
        }

        String path = "";
        String verification = "";
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = true;
            button2.Visible = true;
            textBox1.Visible = true;
            button1.Visible = false;
            button3.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBox1.Text = folderBrowserDialog.SelectedPath;
            path = folderBrowserDialog.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Text = "步驟2.設定妳的電子信箱";
            textBox3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            textBox1.Visible = false;
            button2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox3.Text != null)
            {
                textBox3.Enabled = true;
                Random random = new Random();
                verification = random.Next(100000).ToString().PadLeft(6, '0');
                List<String> list = new List<string>();
                list.Add(textBox3.Text);
                SendMailBase(list, "夜月緋櫻驗證系統", "您的驗證碼: " + verification);
                label6.Visible = true;
                textBox2.Visible = true;
                button3.Visible = false;
            }
        }

        public static void SendMailBase(List<string> ReceivingMails, string MailSubject, string MailBody)
        {
            // 建立類別
            MailMessage Mail = new MailMessage();
            // 設定發件人
            Mail.From = new MailAddress("SakuraMail@google.com", "Sigtuna's Secretary");
            // 加入收件人
            foreach (string ReceivingMail in ReceivingMails)
            { Mail.Bcc.Add(ReceivingMail); }
            // 等級
            Mail.Priority = MailPriority.High;
            // 標題
            Mail.Subject = MailSubject;
            // 內文
            Mail.Body = MailBody;
            Mail.IsBodyHtml = true;
            Mail.BodyEncoding = System.Text.Encoding.UTF8;
            // 發信
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new System.Net.NetworkCredential(
                "han20020625@gmail.com",
                "han910625");
            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(Mail);
            // 釋放
            Mail.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == verification)
            {
                label4.Text = "完成! 歡迎使用!";
                button6.Visible = true;
            }
            else
            {
                MessageBox.Show("驗證失敗!", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
