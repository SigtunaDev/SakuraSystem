using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
namespace WindowsFormsApp4
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public static MySqlConnection connection;
        static String account;
        int level, exp,exp_range;
        String name,group;

        private void MainPage_Load(object sender, EventArgs e)
        {
            ConnectionOpen();
            Form1 f1 = new Form1();
            f1.ShowDialog();
            account = f1.account;
            InfoSet();
            timer1.Interval = 1000;
            timer1.Tick += expSet;
            timer1.Tick += TimeSet;
            timer1.Start();
            timer2.Interval = 2000;
            timer2.Tick += MissionCatch;
            timer2.Tick += MissionFinish;
            timer2.Start();
            listBox1.DoubleClick += ListBox1_DoubleClick;
            MissionCatch(sender, e);
            MissionFinish(sender, e);
            expSet(sender, e);
            TimeSet(sender, e);
            Account acc = new Account(account);
            if(acc.admin == "member")
            {
                button3.Visible = false;
                button4.Visible = false;
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                String MissionNumber = listBox1.SelectedItem.ToString().Split(' ')[1];
                info info = new info(MissionNumber);
                info.Show();
            }
        }

        private void ConnectionOpen()
        {
            String dbHost = "forloop.ddns.net";
            String dbUser = "SQL";
            String dbPass = "8741";
            String dbName = "sakura";
            String connStr = "Server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";charset=utf8";
            connection = new MySqlConnection(connStr);
            connection.Open();
        }

        private void InfoSet()
        {
            String pre_command = String.Format("select * from `account` where 帳號='{0}'",account);
            MySqlCommand command = new MySqlCommand(pre_command,connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                label5.Text += reader.GetString(3);
                label6.Text += reader.GetString(4);
                name = reader.GetString(2);
                group = reader.GetString(4);
            }
            reader.Close();
            label1.Text = String.Format("歡迎回來，{0}醬!", name);
        }

        private void expSet(object sender, EventArgs e)
        {
            String pre_command = String.Format("select * from `account` where 帳號='{0}'", account);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                level = reader.GetInt32(6);
                exp = reader.GetInt32(7);
            }
            reader.Dispose();

            String pre_command_1 = String.Format("select Exp from `expline` where Level={0}", level);
            MySqlCommand command_1 = new MySqlCommand(pre_command_1, connection);
            MySqlDataReader reader_1 = command_1.ExecuteReader();
            while (reader_1.Read())
            {
                exp_range = reader_1.GetInt32(0);
            }
            reader_1.Dispose();

            if (exp < exp_range)
            {
                progressBar1.Value = exp;
                progressBar1.Maximum = exp_range;

            }
            else
            {
                SetLevel(level + 1);
                SetExp(exp - exp_range);
            }
            label3.Text = String.Format("目前等級: {0} ({1}/{2})", level,exp,exp_range);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String pre_command = String.Format("select 最後簽到時間 from `account` where 帳號='{0}'", account);
            MySqlCommand command = new MySqlCommand(pre_command,connection);
            MySqlDataReader reader = command.ExecuteReader();
            String dateTime = "";
            while (reader.Read())
            {
                dateTime = reader.GetString(0);
            }
            reader.Dispose();
            DateTime date_time = DateTime.Now;
            String year = date_time.Year.ToString();
            String month = date_time.Month.ToString();
            String day = date_time.Day.ToString();
            String hour = date_time.Hour.ToString().PadLeft(2,'0');
            String minutes = date_time.Minute.ToString().PadLeft(2, '0');
            String seconds = date_time.Second.ToString().PadLeft(2, '0');
            if (year + "/" + month + "/" + day != dateTime.Split(' ')[0])
            {
                button2.Enabled = false;
                button2.Text = "簽到成功! 經驗值+4";
                String date = String.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, minutes, seconds);
                RunCommand(String.Format("update `account` set 最後簽到時間='{0}' where 帳號='{1}'", date, account));
                AddExp(4);
            }
            else
            {
                button2.Enabled = false;
                button2.Text = "簽到時間更新!";
                String date = String.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, minutes, seconds);
                RunCommand(String.Format("update `account` set 最後簽到時間='{0}' where 帳號='{1}'", date, account));
            }
        }

        private void TimeSet(object sender,EventArgs e)
        {
            DateTime dt = DateTime.Now;
            String hour = dt.Hour.ToString().PadLeft(2,'0');
            String minutes = dt.Minute.ToString().PadLeft(2, '0');
            String seconds = dt.Second.ToString().PadLeft(2, '0');
            label4.Text = String.Format("現在時間: {0}:{1}:{2} 第 {3} 週", hour, minutes, seconds, GetIso8601WeekOfYear(dt));
        }

        private void MissionCatch(object sender, EventArgs e)
        {
            String pre_command = "select * from `mission`";
            MySqlCommand command = new MySqlCommand(pre_command,connection);
            Account acc = new Account(account);
            String group = acc.group;
            MySqlDataReader reader = command.ExecuteReader();
            listBox1.Items.Clear();
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            if (group != "主管部門")
            {
                while (reader.Read())
                {
                    if (reader.GetString(0) == "全體" || reader.GetString(0).Equals(group))
                    {
                        String mission_id = reader.GetString(3);
                        String mission_name = reader.GetString(1);
                        String mission_group = reader.GetString(0);
                        String mission_date = reader.GetString(2);
                        dictionary.Add(mission_id, mission_group + "_" + mission_name + "_" + mission_date);
                    }
                }
                reader.Dispose();
            }
            else
            {
                while (reader.Read())
                {
                    String mission_id = reader.GetString(3);
                    String mission_name = reader.GetString(1);
                    String mission_group = reader.GetString(0);
                    String mission_date = reader.GetString(2);
                    dictionary.Add(mission_id, mission_group + "_" + mission_name + "_" + mission_date);
                }
                reader.Dispose();
            }
            for(int i = 0; i < dictionary.Count(); i++)
            {
                String key = dictionary.ElementAt(i).Key;
                if (MissionFinished(key) == false && MissionTimeUp(key) == true)
                {
                    String mission_pack = dictionary.ElementAt(i).Value;
                    String mission_group = mission_pack.Split('_')[0];
                    String mission_name = mission_pack.Split('_')[1];
                    String mission_id = dictionary.ElementAt(i).Key;
                    listBox1.Items.Add(String.Format("[{0}] {1} {2}", mission_group, mission_id, mission_name));
                }
            }
        }

        private Boolean MissionFinished(String MissionNumber)
        {
            String pre_command = String.Format("select * from `missioncom` where 任務編號='{0}' and 暱稱='{1}' and 審核='Checked'",MissionNumber,name);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            if(reader.Read() == true)
            {
                reader.Dispose();
                return true;
            }
            else
            {
                reader.Dispose();
                return false;
            }
        }

        private Boolean MissionTimeUp(String MissionNumber)
        {
            String pre_command = String.Format("select 時間 from `mission` where 編號='{0}'", MissionNumber);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            String time = "";
            while (reader.Read())
            {
                time = reader.GetString(0);
            }
            reader.Dispose();
            DateTime date_time = DateTime.Now;
            String year = date_time.Year.ToString();
            String month = date_time.Month.ToString();
            String day = date_time.Day.ToString();
            String today = String.Format("{0}/{1}/{2}", year, month.ToString().PadLeft(2,'0'), day.ToString().PadLeft(2, '0'));
            String week = "week " + GetIso8601WeekOfYear(date_time);
            if(time == today || time == week || time == "none")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RunCommand(String command)
        {
            MySqlCommand commands = new MySqlCommand(command, connection);
            commands.ExecuteNonQuery();
        }

        private void AddExp(int amount)
        {
            String pre_command = String.Format("update `account` set 經驗值={0} where 帳號='{1}'", exp + amount, account);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            command.ExecuteNonQuery();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResponseMission response = new ResponseMission(listBox1.Items,account);
            response.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminLayout al = new AdminLayout(account);
            al.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            JudgeSystem judge = new JudgeSystem(account);
            judge.Show();
        }

        private void SetLevel(int level)
        {
            String pre_command = String.Format("update `account` set 等級={0} where 帳號='{1}'", level, account);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            command.ExecuteNonQuery();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            FirstInit fi = new FirstInit();
            fi.Show();
        }

        private void SetExp(int exp)
        {
            String pre_command = String.Format("update `account` set 經驗值={0} where 帳號='{1}'", exp, account);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            command.ExecuteNonQuery();
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

        public void MissionFinish(object sender,EventArgs e)
        {
            for(int i = 0; i < listBox1.Items.Count; i++)
            {
                Account acc = new Account(account);
                String name = acc.name;
                String Mission_Number = listBox1.Items[i].ToString().Split(' ')[1];
                String pre_command = String.Format("select ID,審核 from `missioncom` where 暱稱='{0}' and 任務編號='{1}'",name,Mission_Number);
                MySQL mySQL = new MySQL("missioncom");
                MySqlDataReader reader = mySQL.RunReader(pre_command);
                Dictionary<String,String> dictionary = new Dictionary<string,string>();
                while (reader.Read())
                {
                    dictionary.Add(reader.GetString(0), reader.GetString(1));
                }
                reader.Dispose();
                for(int r = 0; r < dictionary.Count; r++)
                {
                    String ID = dictionary.ElementAt(r).Key;
                    String Judge = dictionary.ElementAt(r).Value;
                    if(Judge == "PASS")
                    {
                        Form2 f2 = new Form2(ID);
                        f2.Show();
                    }else if(Judge == "DENIED")
                    {
                        Form2 f2 = new Form2(ID);
                        f2.Show();
                    }else if(Judge == "Denied_Checked" || Judge == "Checked" || Judge == "SKIP")
                    {
                        continue;
                    }
                }
            }
        }
    }
}
