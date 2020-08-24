using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Data;


namespace Watch_Men
{
    class Connect
    {

        private MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=watch_men_db"); 
        public MySqlConnection Kapcsolat()
        {
            return connection;
        }

        public void Csatlakozas()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("A kapcsolódás sikertelen!",ex.Message);
            }

        }

        public void KapcsolatBontasa()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("A kapcsolat bontása!", ex.Message);
            }

        }
    }
}
