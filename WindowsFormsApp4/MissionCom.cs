using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    class MissionCom
    {
        private String orderNumber;
        public String ID;
        public String name;
        public String MissionNumber;
        public String CompleteTime;
        public String HandleFileURL;
        public String Post_Script;
        public String MissionInfo;
        public String judge;
        public String judger;
        public String judge_message;

        public MissionCom(String orderNumber)
        {
            this.orderNumber = orderNumber;
            MySQL mySQL = new MySQL("missioncom");
            MySqlDataReader reader = mySQL.RunReader(String.Format("select * from `missioncom` where ID = {0}", orderNumber));
            while (reader.Read())
            {
                ID = reader.GetString(0);
                name = reader.GetString(1);
                MissionNumber = reader.GetString(2);
                CompleteTime = reader.GetString(3);
                HandleFileURL = reader.GetString(4);
                Post_Script = reader.GetString(5);
                judge = reader.GetString(6);
                judge_message = reader.GetString(7);
                judger = reader.GetString(8);
                MissionInfo = reader.GetString(9);
            }
            reader.Dispose();
        }
    }
}
