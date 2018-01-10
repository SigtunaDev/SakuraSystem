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
    public partial class info : Form
    {
        String mission_number;
        MySqlConnection connection = MainPage.connection;
        public info(String mission_number)
        {
            this.mission_number = mission_number;
            InitializeComponent();
        }

        private void info_Load(object sender, EventArgs e)
        {
            String pre_command = String.Format("select * from `mission` where 編號='{0}'", mission_number);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                label2.Text = String.Format("任務名稱: {0}", reader.GetString(1));
                label3.Text = String.Format("任務編號: {0}", reader.GetString(3));
                label6.Text = String.Format("任務發布人: {0}", reader.GetString(5));
                label5.Text = String.Format("任務期限: {0}", DateChange(reader.GetString(2)));
                String info = "";
                String[] info_split = reader.GetString(6).Split('*');
                for (int i = 0; i < info_split.Count(); i++)
                {
                    info += info_split[i].Replace("┘","'") + Environment.NewLine;
                }
                label4.Text = String.Format("任務內容: {0} \n", info);
                label7.Text = String.Format("任務經驗值: {0}", reader.GetString(4));
            }
            reader.Dispose();
        }

        private String DateChange(String str)
        {
            if (str.Contains("week"))
            {
                return "這週之前";
            }else if (str.Contains("none"))
            {
                return "不限";
            }
            else
            {
                return "今天";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
