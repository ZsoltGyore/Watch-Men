using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Watch_Men
{
    class Felhasznalok
    {
        Connect conn = new Connect();
        //Felhasználó beillesztés
        public bool FelhasznaloBeilleszt(string felhasznaloNev, string jelszo)
        {
            MySqlCommand command = new MySqlCommand();
            string beillesztes = "INSERT INTO `felhasznalok`(`felhasznalonev`,`jelszo`) VALUES (@felh,@jel)";
            command.CommandText = beillesztes;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@felh", MySqlDbType.VarChar).Value = felhasznaloNev;
            command.Parameters.Add("@jel", MySqlDbType.VarChar).Value = jelszo;


            conn.Csatlakozas();

            if (command.ExecuteNonQuery() == 1)
            {
                conn.KapcsolatBontasa();
                return true;
            }
            else
            {
                conn.KapcsolatBontasa();
                return false;
            }
        }
    }
}
