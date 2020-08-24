using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;


namespace Watch_Men
{
    public partial class Ugyfelek : Form
    {

        Ugyfel ugyfel = new Ugyfel();
        public Ugyfelek()
        {
            InitializeComponent();
        }

        private void Ugyfelek_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ugyfel.UgyfelLista();
        }

        private void buttonUjUgyfel_Click(object sender, EventArgs e)
        {
            
            String nev = textBox1.Text;
            String cegnev = textBox2.Text;
            String szekhely = textBox3.Text;
            String telefon = maskedTextBox1.Text;
            String fax = maskedTextBox2.Text;
            String email = textBox4.Text;

            if (nev.Trim().Equals("") || cegnev.Trim().Equals("") || szekhely.Trim().Equals("") || telefon.Trim().Equals("") || fax.Trim().Equals("") || email.Trim().Equals(""))
            {
                MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Boolean ugyfelBeilleszt = ugyfel.ugyfelBeilleszt(nev, cegnev, szekhely, telefon, fax, email);
                if (ugyfelBeilleszt)
                {
                    dataGridView1.DataSource = ugyfel.UgyfelLista();
                    MessageBox.Show("Ügyfél hozzáadva a listához.", "Sikeres");
                }
                else
                {
                    MessageBox.Show("Hiba - Ügyfél felvitele sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonUgyfelModosit_Click(object sender, EventArgs e)
        {
            int id;
            String nev = textBox1.Text;
            String cegnev = textBox2.Text;
            String szekhely = textBox3.Text;
            String telefon = maskedTextBox1.Text;
            String fax = maskedTextBox2.Text;
            String email = textBox4.Text;
            try
            {
                id = Convert.ToInt32(textBoxID.Text);
                if (nev.Trim().Equals("") || cegnev.Trim().Equals("") || szekhely.Trim().Equals("") || telefon.Trim().Equals("") || fax.Trim().Equals("") || email.Trim().Equals(""))
                {
                    MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Boolean ugyfelBeilleszt = ugyfel.ugyfelModosit(id,nev, cegnev, szekhely, telefon, fax, email);
                    if (ugyfelBeilleszt)
                    {
                        dataGridView1.DataSource = ugyfel.UgyfelLista();
                        MessageBox.Show("Ügyfél módosítás sikeres!", "Ügyfél módosítás", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("HIBA - Ügyfél módosítás sikertelen!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            maskedTextBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void buttonUgyfelTorol_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(textBoxID.Text);
                if (ugyfel.ugyfelTorol(id))
                {
                    dataGridView1.DataSource = ugyfel.UgyfelLista();
                    MessageBox.Show("Ügyfél sikeresen törölve!", "Ügyfél törölve", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonUgyfelMezoTorol.PerformClick();
                }
                else
                {
                    MessageBox.Show("HIBA - Ügyfél törlése sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxID.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
            textBox4.Text = "";
        }

        private void buttonUgyfelKilep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan bezárja az ügyfelek formot?", "Kilép?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void buttonKimentes_Click(object sender, EventArgs e)
        {
            try
            {
                copyAlltoClipboard();
                Microsoft.Office.Interop.Excel.Application xlexcel;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Microsoft.Office.Interop.Excel.Application();
                xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Range range = (Range)xlWorkSheet.Cells[1, 1];
                range.Select();
                xlWorkSheet.PasteSpecial(range, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private void textBoxKeres_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=watch_men_db");
            if (comboBoxKategoria.Text == "ID")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE ID LIKE '" + textBoxKeres.Text + "%'",connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Nev")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE Nev LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Ceg_Neve")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE Ceg_Neve LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Szekhely")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE Szekhely LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Telefon")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE Telefon LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Fax")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE Fax LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "E_mail")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `Nev`, `Ceg_Neve`, `Szekhely`, `Telefon`, `Fax`, `E_mail` FROM `ugyfel` WHERE E_mail LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
    }
}
