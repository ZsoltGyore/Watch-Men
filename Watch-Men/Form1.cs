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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan kilép a programból?", "Kilép?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                Connect con = new Connect();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();
                String query = "SELECT * FROM `felhasznalok` WHERE `felhasznalonev`=@felh AND `jelszo`=@jel";

                command.CommandText = query;
                command.Connection = con.Kapcsolat();
                command.Parameters.Add("@felh", MySqlDbType.VarChar).Value = textBoxFelhasznalo.Text;
                byte[] jelszoHashelve = Encoding.UTF8.GetBytes(textBoxJelszo.Text.ToString());
                command.Parameters.Add("@jel", MySqlDbType.VarChar).Value = MD5Hash(jelszoHashelve);

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    this.Hide();
                    MainForm form = new MainForm();
                    form.Show();

                }
                else
                {
                    if (textBoxFelhasznalo.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Kérem adja meg a felhasználónevet", "Hibás felhasználónév", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (textBoxJelszo.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Kérem adja meg a jelszavát", "Hibás jelszó", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Hibás felhasználónév vagy jelszó", "Hibás adat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            RegForm form = new RegForm();
            form.Show();
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
