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
    public partial class Form2 : Form
    {
        String Mission_Number;
        public Form2(String Mission_Number)
        {
            this.Mission_Number = Mission_Number;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MissionCom mission = new MissionCom(Mission_Number);
            label2.Text = String.Format("ID: {0}", mission.ID);
            label3.Text = String.Format("Mission_Name: {0}", mission.MissionInfo);
            if (mission.judge == "PASS")
            {
                label4.ForeColor = Color.Green;
                label4.Text = String.Format("Response Result: 通過");
                Checked(true);
            }
            else if (mission.judge == "DENIED")
            {
                label4.ForeColor = Color.Red;
                if (mission.judge_message != null && mission.judge_message != "")
                {
                    label4.Text = String.Format("Response Result: 不通過-{0}", mission.judge_message);
                }
                else
                {
                    label4.Text = "Response Result: 不通過";
                }
                Checked(false);
            }
        }

        private void Checked(Boolean b)
        {
            if(b == true)
            {
                MySQL mySQL = new MySQL("missioncom");
                mySQL.RunCommand(String.Format("update `missioncom` set 審核='Checked' where ID={0}", Mission_Number));
            }
            else
            {
                MySQL mySQL = new MySQL("missioncom");
                mySQL.RunCommand(String.Format("update `missioncom` set 審核='Denied_Checked' where ID={0}", Mission_Number));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
