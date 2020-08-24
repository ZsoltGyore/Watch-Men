using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Watch_Men
{
    class Ora
    {
        Connect conn = new Connect();

        //Óra lista
        public DataTable OraLista()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `orak`", conn.Kapcsolat());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table;
        }
        //Óra beillesztés
        public bool oraBeilleszt(string cikkszam, string marka, string tipus, int netto_ar,string afa_kulcs,int brutto_ar,int darabszam, string nem, string szerkezet, string kijelzes, string tok_szine, string szij_anyaga)
        {
            MySqlCommand command = new MySqlCommand();
            string beillesztes = "INSERT INTO `orak`(`cikk_szam`,`marka`,`tipus`,`Netto_ar`,`Afa_kulcs`,`Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga`) VALUES (@cik,@mar,@tip,@net,@afa,@brt,@db,@ne,@szer,@kij,@tok,@szij)";
            command.CommandText = beillesztes;
            command.Connection = conn.Kapcsolat();

            //@cik,@mar,@tip,@net,@afa,@brt,@db,@ne,@szer,@kij,@tok,@szij
            command.Parameters.Add("@cik", MySqlDbType.VarChar).Value = cikkszam;
            command.Parameters.Add("@mar", MySqlDbType.VarChar).Value = marka;
            command.Parameters.Add("@tip", MySqlDbType.VarChar).Value = tipus;
            command.Parameters.Add("@net", MySqlDbType.Int32).Value = netto_ar;
            command.Parameters.Add("@afa", MySqlDbType.VarChar).Value = afa_kulcs;
            command.Parameters.Add("@brt", MySqlDbType.Int32).Value = brutto_ar;
            command.Parameters.Add("@db", MySqlDbType.Int32).Value = darabszam;
            command.Parameters.Add("@ne", MySqlDbType.VarChar).Value = nem;
            command.Parameters.Add("@szer", MySqlDbType.VarChar).Value = szerkezet;
            command.Parameters.Add("@kij", MySqlDbType.VarChar).Value = kijelzes;
            command.Parameters.Add("@tok", MySqlDbType.VarChar).Value = tok_szine;
            command.Parameters.Add("@szij", MySqlDbType.VarChar).Value = szij_anyaga;

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
        //Óra módosítása
        public bool oraModosit(int id, string cikkszam, string marka, string tipus, int netto_ar, string afa_kulcs, int brutto_ar, int darabszam, string nem, string szerkezet, string kijelzes, string tok_szine, string szij_anyaga)
        {
            MySqlCommand command = new MySqlCommand();
            string modosit = "UPDATE `orak` SET `cikk_szam`=@cik,`marka`=@mar,`tipus`=@tip,`Netto_ar`=@net,`Afa_kulcs`=@afa,`Brutto_ar`=@brt,`Darabszam`=@db,`nem`=@ne,`szerkezet`=@szer,`kijelzes`=@kij,`tok_szine`=@tok,`szij_anyaga`=@szij WHERE `ID`=@i";
            command.CommandText = modosit;
            command.Connection = conn.Kapcsolat();

            //@i,@cik,@mar,@tip,@net,@afa,@brt,@db,@ne,@szer,@kij,@tok,@szij
            command.Parameters.Add("@i", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@cik", MySqlDbType.VarChar).Value = cikkszam;
            command.Parameters.Add("@mar", MySqlDbType.VarChar).Value = marka;
            command.Parameters.Add("@tip", MySqlDbType.VarChar).Value = tipus;
            command.Parameters.Add("@net", MySqlDbType.Int32).Value = netto_ar;
            command.Parameters.Add("@afa", MySqlDbType.VarChar).Value = afa_kulcs;
            command.Parameters.Add("@brt", MySqlDbType.Int32).Value = brutto_ar;
            command.Parameters.Add("@db", MySqlDbType.Int32).Value = darabszam;
            command.Parameters.Add("@ne", MySqlDbType.VarChar).Value = nem;
            command.Parameters.Add("@szer", MySqlDbType.VarChar).Value = szerkezet;
            command.Parameters.Add("@kij", MySqlDbType.VarChar).Value = kijelzes;
            command.Parameters.Add("@tok", MySqlDbType.VarChar).Value = tok_szine;
            command.Parameters.Add("@szij", MySqlDbType.VarChar).Value = szij_anyaga;
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
        //Óra töröl
        public bool oraTorol(int id)
        {
            MySqlCommand command = new MySqlCommand();
            String ora_torles = "DELETE FROM `orak` WHERE `ID`=@i";
            command.CommandText = ora_torles;
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
