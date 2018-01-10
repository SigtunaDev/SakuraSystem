using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    class Account
    {
        private MySqlConnection connection = MainPage.connection;
        public String password;
        public String admin;
        public String group;
        public String name;
        public int level;
        public int exp;

        public Account(String account)
        {
            String pre_command = String.Format("Select * from `account` where 帳號='{0}'", account);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            command.ExecuteNonQuery();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                name = reader.GetString(2);
                password = reader.GetString(1);
                admin = reader.GetString(3);
                group = reader.GetString(4);
                level = reader.GetInt32(6);
                exp = reader.GetInt32(7);
            }
            reader.Dispose();
        }
    }
}
