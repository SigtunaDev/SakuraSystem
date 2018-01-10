using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    class Mission
    {

        public String MissionName;
        public String MissionGroup;
        public int MissionExp;
        public String MissionInfo;
        public String MissionMaker;
        public String MissionTime;

        private MySqlConnection connection = MainPage.connection;
        
        public Mission(String MissionNumber)
        {
            String pre_command = String.Format("select * from `mission` where 編號='{0}'", MissionNumber);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                MissionGroup = reader.GetString(0);
                MissionName = reader.GetString(1);
                MissionTime = reader.GetString(2);
                MissionExp = reader.GetInt32(4);
                MissionMaker = reader.GetString(5);
                MissionInfo = reader.GetString(6);
            }
            reader.Dispose();
        }
    }
}
