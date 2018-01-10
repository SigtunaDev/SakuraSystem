using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp4
{
    class MySQL
    {
        String table;
        MySqlConnection connection = MainPage.connection;

        public MySQL(String table)
        {
            this.table = table;
        }

        public void RunCommand(String command)
        {
            MySqlCommand commands = new MySqlCommand(command, connection);
            commands.ExecuteNonQuery();
        }

        public int TableCount()
        {
            int count = -1;
            String pre_command = String.Format("select count(*) from `{0}`", table);
            MySqlCommand command = new MySqlCommand(pre_command, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = reader.GetInt16(0);
            }
            reader.Dispose();
            return count;
        }

        public MySqlDataReader RunReader(String command)
        {
            MySqlCommand commands = new MySqlCommand(command, connection);
            MySqlDataReader reader = commands.ExecuteReader();
            return reader;
        }
    }
}
