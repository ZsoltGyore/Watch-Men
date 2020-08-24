using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Watch_Men
{
    public partial class RegForm : Form
    {
        Felhasznalok felhasznalo = new Felhasznalok();
        public RegForm()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (textBoxJelszo1.UseSystemPasswordChar)
            {
                textBoxJelszo1.UseSystemPasswordChar = false;
            }
            else
            {
                textBoxJelszo1.UseSystemPasswordChar = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (textBoxJelszo2.UseSystemPasswordChar)
            {
                textBoxJelszo2.UseSystemPasswordChar = false;
            }
            else
            {
                textBoxJelszo2.UseSystemPasswordChar = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] jelszoHash = Encoding.UTF8.GetBytes(textBoxJelszo1.Text.ToString());
            byte[] jelszoHash2 = Encoding.UTF8.GetBytes(textBoxJelszo2.Text.ToString());
            string felhasznaloNev = textBox1.Text;
            string jelszo = MD5Hash(jelszoHash);
            string jelszo2 = MD5Hash(jelszoHash2);
            if (jelszo == jelszo2)
            {
                if (felhasznaloNev.Trim().Equals("") && jelszo.Trim().Equals("") && jelszo2.Trim().Equals(""))
                {
                    MessageBox.Show("Hiba! Minden kitöltése kötelező!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Boolean felhasznaloBeilleszt = felhasznalo.FelhasznaloBeilleszt(felhasznaloNev, jelszo);
                    if (felhasznaloBeilleszt)
                    {
                        MessageBox.Show("A regisztráció sikeres!", "Felvitel sikeres", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hiba - Felhasználó felvitele sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("HIBA - A két jelszó nem egyezik!","Hiba",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string MD5Hash(byte[] ertek)
        {
            using (MD5 md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(ertek);
                return Convert.ToBase64String(hash);
            }
        }

    }
}
