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
    public partial class Kiegeszitok : Form
    {
        Kiegeszito kiegeszitok = new Kiegeszito();
        enum Anyag
        {
            Fém,
            Bör,
            Vászon,
            Müanyag,
            Gumirozott,
            Egyéb

        }

        enum Tipus
        {
            Öv,
            Táska,
            Napszemüveg,
            Pénztárca,
            Esernyö,
            Toll,
            Egyéb

        }
        public Kiegeszitok()
        {
            InitializeComponent();
            comboBoxKiegAnyag.DataSource = Enum.GetValues(typeof(Anyag));
            comboBoxKiegTipus.DataSource = Enum.GetValues(typeof(Tipus));
        }

        private void Kiegeszitok_Load(object sender, EventArgs e)
        {
            dataGridViewKieg.DataSource = kiegeszitok.KiegeszitoLista();
            comboBoxAfa.SelectedIndex = 0;
        }

        private void buttonUjKieg_Click(object sender, EventArgs e)
        {
            string cikkszam = textBoxKiegCikk.Text;
            string anyag = comboBoxKiegAnyag.SelectedItem.ToString();
            string tipus = comboBoxKiegTipus.SelectedItem.ToString();
            int netto_ar = (int)numericUpDownKiegNetto.Value;
            string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
            int brutto_ar = (int)numericUpDownKiegBrutto.Value;
            int darabszam = (int)numericUpDownKiegDarabszam.Value;
            if (cikkszam.Trim().Equals(""))
            {
                MessageBox.Show("Hiba! Cikkszám kitöltése kötelező!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Boolean kiegbeilleszt = kiegeszitok.KiegeszitoBeilleszt(cikkszam, anyag, tipus, netto_ar, afa_kulcs, brutto_ar, darabszam);
                if (kiegbeilleszt)
                {
                    dataGridViewKieg.DataSource = kiegeszitok.KiegeszitoLista();
                    MessageBox.Show("Kiegészítő sikeresen hozzáadva a listához", "Felvitel sikeres", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Hiba - Kiegészítő felvitele sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonKiegModosit_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                string cikkszam = textBoxKiegCikk.Text;
                string anyag = comboBoxKiegAnyag.SelectedItem.ToString();
                string tipus = comboBoxKiegTipus.SelectedItem.ToString();
                int netto_ar = (int)numericUpDownKiegNetto.Value;
                string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
                int brutto_ar = (int)numericUpDownKiegBrutto.Value;
                int darabszam = (int)numericUpDownKiegDarabszam.Value;
                try
                {
                    id = Convert.ToInt32(textBoxKiegID.Text);
                    if (cikkszam.Trim().Equals(""))
                    {
                        MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Boolean kiegmodosit = kiegeszitok.KiegeszitoModosit(id, cikkszam, anyag, tipus, netto_ar, afa_kulcs, brutto_ar, darabszam);
                        if (kiegmodosit)
                        {
                            dataGridViewKieg.DataSource = kiegeszitok.KiegeszitoLista();
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

        private void buttonKiegTorol_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Valóban törli a kijelölt elemet?", "Törlés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(textBoxKiegID.Text);
                    if (kiegeszitok.KiegisztoTorol(id))
                    {
                        dataGridViewKieg.DataSource = kiegeszitok.KiegeszitoLista();
                        MessageBox.Show("Termék sikeresen törölve!", "Termék törölve", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonKiegMezokTorol.PerformClick();
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

        private void buttonKiegMezokTorol_Click(object sender, EventArgs e)
        {
            textBoxKiegCikk.Text = "";
            comboBoxKiegAnyag.SelectedIndex = 0;
            comboBoxKiegTipus.SelectedIndex = 0;
            numericUpDownKiegNetto.Value = 1;
            comboBoxAfa.SelectedIndex = 0;
            numericUpDownKiegBrutto.Value = 1;
            numericUpDownKiegBrutto.Value = 1;
        }
        private void copyAlltoClipboard()
        {
            dataGridViewKieg.RowHeadersVisible = true;
            dataGridViewKieg.SelectAll();
            DataObject dataObj = dataGridViewKieg.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void buttonKiegExcelKiment_Click(object sender, EventArgs e)
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

        private void buttonKiegKilep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan kilép a Kiegészítők fromból?","Kilép?",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void textBoxKeres_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=watch_men_db");
            if (comboBoxKategoria.Text == "ID")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE ID LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Cikkszám")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE cikk_szam LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Anyag")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE anyag LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Tipus")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE tipus LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Nettóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE Netto_ar LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Bruttóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE Brutto_ar LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Darabszám")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `anyag`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam` FROM `kiegeszitok` WHERE darabszam LIKE '" + textBoxKiegKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewKieg.DataSource = table;
            }

        }

        private void comboBoxAfa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAfa.Text == "27%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 127;
            }
            else if (comboBoxAfa.Text == "18%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 118;
            }
            else if (comboBoxAfa.Text == "5%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 105;
            }
            else if (comboBoxAfa.Text == "Adómentes")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = numericUpDownKiegNetto.Value;
            }
        }

        private void dataGridViewKieg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBoxKiegID.Text = dataGridViewKieg.CurrentRow.Cells[0].Value.ToString();
                textBoxKiegCikk.Text = dataGridViewKieg.CurrentRow.Cells[1].Value.ToString();
                comboBoxKiegAnyag.Text = dataGridViewKieg.CurrentRow.Cells[2].Value.ToString();
                comboBoxKiegTipus.Text = dataGridViewKieg.CurrentRow.Cells[3].Value.ToString();
                numericUpDownKiegNetto.Value = (int)dataGridViewKieg.CurrentRow.Cells[4].Value;
                comboBoxAfa.Text = dataGridViewKieg.CurrentRow.Cells[5].Value.ToString();
                numericUpDownKiegBrutto.Value = (int)dataGridViewKieg.CurrentRow.Cells[6].Value;
                numericUpDownKiegDarabszam.Value = (int)dataGridViewKieg.CurrentRow.Cells[7].Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDownKiegNetto_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxAfa.Text == "27%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 127;
            }
            else if (comboBoxAfa.Text == "18%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 118;
            }
            else if (comboBoxAfa.Text == "5%")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = (numericUpDownKiegNetto.Value / 100) * 105;
            }
            else if (comboBoxAfa.Text == "Adómentes")
            {
                numericUpDownKiegBrutto.Enabled = false;
                numericUpDownKiegBrutto.Value = numericUpDownKiegNetto.Value;
            }
        }
    }
}
