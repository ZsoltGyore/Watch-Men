using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Watch_Men
{
    class Ugyfel
    {
        Connect conn = new Connect();

        //Ügyfelek listája
        public DataTable UgyfelLista()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `ugyfel`", conn.Kapcsolat());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }

        //Ügyfél beillesztése
        public bool ugyfelBeilleszt(string nev,string cegnev,string szekhely, string telefon, string fax, string email)
        {
            MySqlCommand command = new MySqlCommand();
            string beillesztes = "INSERT INTO `ugyfel`(`Nev`,`Ceg_Neve`,`Szekhely`,`Telefon`,`Fax`,`E_mail`) VALUES (@ne,@ceg,@szek,@tel,@fa,@em)";
            command.CommandText = beillesztes;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@ne", MySqlDbType.VarChar).Value = nev;
            command.Parameters.Add("@ceg", MySqlDbType.VarChar).Value = cegnev;
            command.Parameters.Add("@szek", MySqlDbType.VarChar).Value = szekhely;
            command.Parameters.Add("@tel", MySqlDbType.VarChar).Value = telefon;
            command.Parameters.Add("@fa", MySqlDbType.VarChar).Value = fax;
            command.Parameters.Add("@em", MySqlDbType.VarChar).Value = email;

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

        //Ügyfél módosítása
        public bool ugyfelModosit(int id,string nev, string cegnev, string szekhely, string telefon, string fax, string email)
        {
            MySqlCommand command = new MySqlCommand();
            string modosit = "UPDATE `ugyfel` SET `Nev`= @ne,`Ceg_Neve`= @ceg,`Szekhely`= @szek,`Telefon`= @tel,`Fax`=@fa,`E_mail`=@em WHERE `ID`=@i";
            command.CommandText = modosit;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@i", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@ne", MySqlDbType.VarChar).Value = nev;
            command.Parameters.Add("@ceg", MySqlDbType.VarChar).Value = cegnev;
            command.Parameters.Add("@szek", MySqlDbType.VarChar).Value = szekhely;
            command.Parameters.Add("@tel", MySqlDbType.VarChar).Value = telefon;
            command.Parameters.Add("@fa", MySqlDbType.VarChar).Value = fax;
            command.Parameters.Add("@em", MySqlDbType.VarChar).Value = email;

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


        //Ügyfél törlése
        public bool ugyfelTorol(int id)
        {
            MySqlCommand command = new MySqlCommand();
            String torles = "DELETE FROM `ugyfel` WHERE `ID`=@i";
            command.CommandText = torles;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@i", MySqlDbType.VarChar).Value = id;

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
