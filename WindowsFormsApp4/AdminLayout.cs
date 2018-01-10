using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class AdminLayout : Form
    {
        String account;
        public AdminLayout(String account)
        {
            this.account = account;
            InitializeComponent();
        }

        private void AdminLayout_Load(object sender, EventArgs e)
        {
            RadioButton[] radioButton = new RadioButton[2] { radioButton1, radioButton2 };
            for(int i = 0; i < radioButton.Length; i++)
            {
                radioButton[i].Click += AdminLayout_Click;
            }
            timer1.Interval = 10000;
            timer1.Tick += UpdateLoginList;
            timer1.Start();
            UpdateLoginList(sender, e);
        }

        String admin = "";

        private void AdminLayout_Click(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if(radioButton == radioButton1)
            {
                admin = "system";
            }
            else
            {
                admin = "member";
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label8.Text = String.Format("經驗值: {0}",trackBar1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String account = textBox1.Text;
            String password = textBox2.Text;
            String name = textBox5.Text;
            MySQL mySql = new MySQL("account");
            String Group = comboBox2.Text;
            String adminp = admin;
            String info = "帳號新增成功" + Environment.NewLine +
                "帳號: " + account + Environment.NewLine +
                "密碼: " + password + Environment.NewLine +
                "暱稱: " + name + Environment.NewLine + 
                "身分組: " + adminp + Environment.NewLine +
                "組別: " + Group + Environment.NewLine;
            mySql.RunCommand(String.Format("insert into `account`(帳號,密碼,暱稱,身分組,組別,經驗值,等級) values ('{0}','{1}','{2}','{3}','{4}',{5},{6})", account, password,name,adminp,Group,0,0));
            MessageBox.Show(info, "ACCOUNT ADD SUCCESSFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == null || comboBox3.Text == "")
            {
                MessageBox.Show("發布群組類別沒有選擇", "Group Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("名稱不能為白", "Name Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(comboBox1.Text == null || comboBox1.Text == "")
            {
                MessageBox.Show("期限沒有選擇", "DateTime Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(textBox3.Text.Length > 10)
            {
                MessageBox.Show("名稱超過了限制的數量", "Name Length Over", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Account acc = new Account(account);
            String mission_name = textBox3.Text;
            int exp = trackBar1.Value;
            String express = comboBox1.Text;
            String time = "unknown";
            String group = comboBox3.Text;
            Random random = new Random();
            String id = random.Next(10000).ToString().PadLeft(5,'0');
            String info = "";
            String name = acc.name;
            for(int i = 0; i < textBox4.Lines.Count(); i++)
            {
                info += textBox4.Lines.ElementAt(i).Replace("'", "┘") + "*";
            }
            if (express == "今天")
            {
                time = GetDate();

            }else if(express == "本週")
            {
                time = "week " + GetIso8601WeekOfYear(DateTime.Now);

            }else if(express == "不限")
            {
                time = "none";
            }
            MySQL mySQL = new MySQL("mission");
            mySQL.RunCommand(String.Format("insert into `mission`(組別,敘述,時間,編號,經驗值,任務發布者,詳細敘述) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", group, mission_name, time, id, exp, name, info));
            MessageBox.Show("任務登入完成!", "mission register successful!",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private String GetDate()
        {
            String date = "";
            DateTime dateTime = DateTime.Now;
            String year = dateTime.Year.ToString();
            String month = dateTime.Month.ToString().PadLeft(2, '0');
            String day = dateTime.Day.ToString().PadLeft(2, '0');
            date = String.Format("{0}/{1}/{2}", year, month, day);
            return date;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public void UpdateLoginList(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            MySQL mySQL = new MySQL("account");
            MySqlDataReader reader = mySQL.RunReader("select 暱稱,最後簽到時間 from `account`");
            while (reader.Read())
            {
                if (reader.GetString(1) != "" || reader.GetString(1) != null)
                {
                    listBox1.Items.Add(reader.GetString(0) + " " + reader.GetString(1));
                }
                else
                {
                    listBox1.Items.Add(reader.GetString(0) + " " + "從未上線過");
                }
            }
            reader.Dispose();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
