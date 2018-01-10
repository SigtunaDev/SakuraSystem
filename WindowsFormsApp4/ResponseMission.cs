using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class ResponseMission : Form
    {
        ListBox.ObjectCollection mission;
        String account;

        public ResponseMission(ListBox.ObjectCollection mission,String account)
        {
            this.account = account;
            this.mission = mission;

            InitializeComponent();
        }

        private void ResponseMission_Load(object sender, EventArgs e)
        {
            textBox1.KeyDown += KeyEnterEvent;
            Init_Combo_List();
        }

        private void Init_Combo_List()
        {
            for (int i = 0; i < mission.Count; i++) {
                comboBox1.Items.Add(mission[i]);
            }
        }

        private void Upload()
        {

            String mission_number = comboBox1.Text.Split(' ')[1];
            String mission_post_script = "";
            for(int i = 0; i < textBox1.Lines.Count(); i++)
            {
                mission_post_script += textBox1.Lines.ElementAt(i).Replace("'","┘") + "*";
            }

            Mission mission = new Mission(mission_number);
            MySQL mySQL = new MySQL("missioncom");
            Account acc = new Account(account);

            String ID = mySQL.TableCount().ToString().PadLeft(5,'0');
            String MissionNumber = mission_number;
            String MissionName = mission.MissionName;
            String Time = GetTime();
            String Name = acc.name;
            String File = textBox2.Text;

            mySQL.RunCommand(String.Format("insert into `missioncom`(ID,暱稱,任務編號,完成時間,提交檔案,備註,審核訊息,任務敘述) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", ID, Name, MissionNumber, Time, File, mission_post_script, "", MissionName));
        }

        private String GetTime()
        {
            String time = "";
            DateTime dateTime = DateTime.Now;
            String Year = dateTime.Year.ToString().PadLeft(2, '0');
            String Month = dateTime.Month.ToString().PadLeft(2, '0');
            String Day = dateTime.Day.ToString().PadLeft(2, '0');
            String Hour = dateTime.Hour.ToString().PadLeft(2, '0');
            String Minutes = dateTime.Minute.ToString().PadLeft(2, '0');
            String Seconds = dateTime.Day.ToString().PadLeft(2, '0');
            time = String.Format("{0}/{1}/{2} {3}:{4}:{5}",Year,Month,Day,Hour,Minutes,Seconds);
            return time;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Upload();
            MessageBox.Show("Handle完成! 請等待回覆! 0.<","MISSION HANDLE SUCCESSFUL",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }

        private void KeyEnterEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                if(textBox1.Lines.Length > 4)
                e.Handled = true;
            }
        }

    }
}
