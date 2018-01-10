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

namespace WindowsFormsApp4
{
    public partial class JudgeSystem : Form
    {

        String judge_result = "";
        String Mission_ID = "";
        String message = "";
        String account = "";
        public JudgeSystem(String acc)
        {
            this.account = acc;
            InitializeComponent();
        }

        private void JudgeSystem_Load(object sender, EventArgs e)
        {
            RadioButton[] radioButton = new RadioButton[2] { radioButton1, radioButton2 };
            foreach(RadioButton rb in radioButton)
            {
                rb.Click += SelectRadioButton;
            }
            Upload(sender, e);
            timer1.Interval = 60000;
            timer1.Tick += Upload;
            timer1.Start();
            listBox1.Click += ClickListBox;
        }

        private void Upload(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            MySQL mySQL = new MySQL("missioncom");
            MySqlDataReader reader = mySQL.RunReader("select * from `missioncom`");
            while (reader.Read())
            {
                if (reader.GetString(6) == null || reader.GetString(6) == "")
                {
                    listBox1.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(9));
                }
            }
            reader.Dispose();
        }
        String selected = "";
        private void ClickListBox(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                SelectBox(listBox1.SelectedItem.ToString());
                selected = listBox1.SelectedItem.ToString();
            }
        }

        private void SelectBox(String text)
        {
            textBox3.Text = null;
            String ID = text.Split(' ')[0];
            Mission_ID = ID;
            MissionCom missionCom = new MissionCom(ID);
            label2.Text = "任務: " + missionCom.MissionInfo;
            label3.Text = "任務繳交者: " + missionCom.name;
            String[] message = missionCom.Post_Script.Split('*');
            for (int i = 0; i < message.Length; i++)
            {
                textBox3.Text += message[i].Replace("┘", "'") + Environment.NewLine;
            }
            textBox1.Text = missionCom.HandleFileURL;
        }

        private void SelectRadioButton(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton == radioButton1)
            {
                judge_result = "PASS";
                textBox2.Enabled = false;
            }
            else
            {
                judge_result = "DENIED";
                textBox2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            message = textBox2.Text;
            if (judge_result != "")
            {
                if (judge_result == "PASS")
                {
                    String pre_command = String.Format("update `missioncom` set 審核='PASS',Judger='{1}' where ID='{0}'",Mission_ID,account);
                    MySQL mySQL = new MySQL("missioncom");
                    mySQL.RunCommand(pre_command);
                    listBox1.Items.Remove(selected);
                    MissionCom missionCom = new MissionCom(Mission_ID);
                    List<String> list = new List<string>();
                    for(int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if(listBox1.Items[i].ToString().Contains(missionCom.name + " " + missionCom.MissionInfo))
                        {
                            list.Add(listBox1.Items[i].ToString());
                        }
                    }
                    Mission_Dispose(list);
                }
                else
                {
                    String pre_command = String.Format("update `missioncom` set 審核='DENIED',Judger='{1}',審核訊息='{2}' where ID='{0}'", Mission_ID, account, message);
                    MySQL mySQL = new MySQL("missioncom");
                    mySQL.RunCommand(pre_command);
                    listBox1.Items.Remove(selected);
                }
            }
            MessageBox.Show("審核成功送出!", "Judge Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Mission_Dispose(List<String> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                String missionID = list[i].Split(' ')[0];
                String pre_command = String.Format("update `missioncom` set 審核='SKIP',Judger='{1}' where ID='{0}'", missionID, account);
                MySQL mySQL = new MySQL("missioncom");
                mySQL.RunCommand(pre_command);
                listBox1.Items.Remove(list[i]);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
