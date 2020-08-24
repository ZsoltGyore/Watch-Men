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
    public partial class Ekszerek : Form
    {
        Ekszer ekszerek = new Ekszer();
        enum Anyag
        {
            Arany,
            Ezüst,
            Gyémánt,
            Titán,
            Nemesacél,
            Porcelán,
            Bizsu,
            Egyéb
        }
        enum Tipus
        {
            Nyaklánc,
            Gyürü,
            Karkötö,
            Fülbevaló,
            Piercing,
            Medál

        }
        public Ekszerek()
        {
            InitializeComponent();
            comboBoxAnyag.DataSource = Enum.GetValues(typeof(Anyag));
            comboBoxTipus.DataSource = Enum.GetValues(typeof(Tipus));
        }

        private void Ekszerek_Load(object sender, EventArgs e)
        {
            dataGridViewEkszer.DataSource = ekszerek.EkszerLista();
            comboBoxAfa.SelectedIndex = 0;
        }

        private void buttonUjekszer_Click(object sender, EventArgs e)
        {
            string cikkszam = textBoxCikk.Text;
            string anyag = comboBoxAnyag.SelectedItem.ToString();
            string tipus = comboBoxTipus.SelectedItem.ToString();
            int netto_ar = (int)numericUpDownNetto.Value;
            string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
            int brutto_ar = (int)numericUpDownBrutto.Value;
            int darabszam = (int)numericUpDownDarabszam.Value;
            if (cikkszam.Trim().Equals(""))
            {
                MessageBox.Show("Hiba! Cikkszám kitöltése kötelező!","Hiba",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                Boolean ekszerbeilleszt = ekszerek.EkszerBeilleszt(cikkszam, anyag, tipus, netto_ar, afa_kulcs, brutto_ar, darabszam);
                if (ekszerbeilleszt)
                {
                    dataGridViewEkszer.DataSource = ekszerek.EkszerLista();
                    MessageBox.Show("Ékszer sikeresen hozzáadva a listához","Felvitel sikeres",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Hiba - Ékszer felvitele sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonEkszerModosit_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                string cikkszam = textBoxCikk.Text;
                string anyag = comboBoxAnyag.SelectedItem.ToString();
                string tipus = comboBoxTipus.SelectedItem.ToString();
                int netto_ar = (int)numericUpDownNetto.Value;
                string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
                int brutto_ar = (int)numericUpDownBrutto.Value;
                int darabszam = (int)numericUpDownDarabszam.Value;
                try
                {
                    id = Convert.ToInt32(textBoxID.Text);
                    if (cikkszam.Trim().Equals(""))
                    {
                        MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Boolean oraModosit = ekszerek.EkszerModosit(id, cikkszam, anyag, tipus, netto_ar, afa_kulcs, brutto_ar, darabszam);
                        if (oraModosit)
                        {
                            dataGridViewEkszer.DataSource = ekszerek.EkszerLista();
                            MessageBox.Show("Termék módosítás sikeres!", "Termék módosítás", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("HIBA - Termék módosítás sikertelen!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void buttonEkszerTorol_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Valóban törli a kijelölt elemet?", "Törlés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(textBoxID.Text);
                    if (ekszerek.EkszerTorol(id))
                    {
                        dataGridViewEkszer.DataSource = ekszerek.EkszerLista();
                        MessageBox.Show("Termék sikeresen törölve!", "Termék törölve", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonEkszerMezoTorol.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("HIBA - Termék törlése sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonEkszerMezoTorol_Click(object sender, EventArgs e)
        {
            textBoxCikk.Text = "";
            comboBoxAnyag.SelectedIndex = 0;
            comboBoxTipus.SelectedIndex = 0;
            numericUpDownNetto.Value = 1;
            comboBoxAfa.SelectedIndex = 0;
            numericUpDownBrutto.Value = 1;
            numericUpDownDarabszam.Value = 1;
        }

        private void buttonEkszerKilep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztos bezárja az ékszerek form-ot?","Bezárja?",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridViewEkszer.RowHeadersVisible = false;
            dataGridViewEkszer.SelectAll();
            DataObject dataObj = dataGridViewEkszer.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void button1_Click(object sender, EventArgs e)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBoxID.Text = dataGridViewEkszer.CurrentRow.Cells[0].Value.ToString();
                textBoxCikk.Text = dataGridViewEkszer.CurrentRow.Cells[1].Value.ToString();
                comboBoxAnyag.Text = dataGridViewEkszer.CurrentRow.Cells[2].Value.ToString();
                comboBoxTipus.Text = dataGridViewEkszer.CurrentRow.Cells[3].Value.ToString();
                numericUpDownNetto.Value = (int)dataGridViewEkszer.CurrentRow.Cells[4].Value;
                comboBoxAfa.Text = dataGridViewEkszer.CurrentRow.Cells[5].Value.ToString();
                numericUpDownBrutto.Value = (int)dataGridViewEkszer.CurrentRow.Cells[6].Value;
                numericUpDownDarabszam.Value = (int)dataGridViewEkszer.CurrentRow.Cells[7].Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDownNetto_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxAfa.Text == "27%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 127;
            }
            else if (comboBoxAfa.Text == "18%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 118;
            }
            else if (comboBoxAfa.Text == "5%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 105;
            }
            else if (comboBoxAfa.Text == "Adómentes")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = numericUpDownNetto.Value;
            }
        }

        private void comboBoxAfa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAfa.Text == "27%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 127;
            }
            else if (comboBoxAfa.Text == "18%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 118;
            }
            else if (comboBoxAfa.Text == "5%")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = (numericUpDownNetto.Value / 100) * 105;
            }
            else if (comboBoxAfa.Text == "Adómentes")
            {
                numericUpDownBrutto.Enabled = false;
                numericUpDownBrutto.Value = numericUpDownNetto.Value;
            }
        }

        private void textBoxKeres_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=watch_men_db");
            if (comboBoxKategoria.Text == "ID")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE ID LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Cikkszám")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE cikk_szam LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Anyag")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE anyag LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Tipus")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE tipus LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Nettóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE Netto_ar LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Bruttóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE Brutto_ar LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Darabszám")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `ekszerek` WHERE nem LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewEkszer.DataSource = table;
            }
            
        }
    }
}
