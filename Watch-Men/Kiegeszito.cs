using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Watch_Men
{
    class Kiegeszito
    {
        Connect conn = new Connect();

        //Kiegészítők lista
        public DataTable KiegeszitoLista()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `kiegeszitok`", conn.Kapcsolat());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }
        //Ékszer beillesztés
        public bool KiegeszitoBeilleszt(string cikkszam, string anyag, string tipus, int netto_ar, string afa_kulcs, int brutto_ar, int darabszam)
        {
            MySqlCommand command = new MySqlCommand();
            string beillesztes = "INSERT INTO `kiegeszitok`(`cikk_szam`,`anyag`,`tipus`,`Netto_ar`,`Afa_kulcs`,`Brutto_ar`,`Darabszam`) VALUES (@cik,@any,@tip,@net,@afa,@brt,@db)";
            command.CommandText = beillesztes;
            command.Connection = conn.Kapcsolat();

            //@cik,@any,@tip,@net,@afa,@brt
            command.Parameters.Add("@cik", MySqlDbType.VarChar).Value = cikkszam;
            command.Parameters.Add("@any", MySqlDbType.VarChar).Value = anyag;
            command.Parameters.Add("@tip", MySqlDbType.VarChar).Value = tipus;
            command.Parameters.Add("@net", MySqlDbType.Int32).Value = netto_ar;
            command.Parameters.Add("@afa", MySqlDbType.VarChar).Value = afa_kulcs;
            command.Parameters.Add("@brt", MySqlDbType.Int32).Value = brutto_ar;
            command.Parameters.Add("@db", MySqlDbType.Int32).Value = darabszam;

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

        public bool KiegeszitoModosit(int id, string cikkszam, string anyag, string tipus, int netto_ar, string afa_kulcs, int brutto_ar, int darabszam)
        {
            MySqlCommand command = new MySqlCommand();
            string modosit = "UPDATE `kiegeszitok` SET `cikk_szam`=@cik,`anyag`=@any,`tipus`=@tip,`Netto_ar`=@net,`Afa_kulcs`=@afa,`Brutto_ar`=@brt,`Darabszam`=@db WHERE `ID`=@i";
            command.CommandText = modosit;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@i", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@cik", MySqlDbType.VarChar).Value = cikkszam;
            command.Parameters.Add("@any", MySqlDbType.VarChar).Value = anyag;
            command.Parameters.Add("@tip", MySqlDbType.Int32).Value = tipus;
            command.Parameters.Add("@net", MySqlDbType.Int32).Value = netto_ar;
            command.Parameters.Add("@afa", MySqlDbType.VarChar).Value = afa_kulcs;
            command.Parameters.Add("@brt", MySqlDbType.Int32).Value = brutto_ar;
            command.Parameters.Add("@db", MySqlDbType.Int32).Value = darabszam;

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

        public bool KiegisztoTorol(int id)
        {
            MySqlCommand command = new MySqlCommand();
            String kiegeszito_torles = "DELETE FROM `kiegeszitok` WHERE `ID`=@i";
            command.CommandText = kiegeszito_torles;
            command.Connection = conn.Kapcsolat();

            command.Parameters.Add("@i", MySqlDbType.Int32).Value = id;

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
