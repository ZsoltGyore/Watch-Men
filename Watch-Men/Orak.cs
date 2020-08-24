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
    public partial class Orak : Form
    {
        enum OraNem
        {
            Férfi,
            Nöi,
            Gyerek,
            Unisex
        }
        enum OraSzerkezet
        {
            Quartz,
            Automata,
            Napelemes,
            Kinetik,
            Egyéb
        }
        enum Kijelzes
        {
            Analóg,
            AnalógDigitális,
            Digitális,
            Egyéb
        }
        enum TokSzine
        {
            Ezüst,
            Arany,
            Fekete,
            Fehér,
            Piros,
            Zöld,
            Kék,
            Sárga,
            Barna,
            Lila,
            Rózsaszín,
            Többszínű,
            Egyéb
        }
        enum SzijAnyaga
        {
            Bör,
            Nemesacél,
            Szilikon,
            Textil,
            Kerámia,
            Titán,
            Karbon
        }

        Ora orak = new Ora();
        public Orak()
        {
            InitializeComponent();
            comboBoxNem.DataSource = Enum.GetValues(typeof(OraNem));
            comboBoxSzerkezet.DataSource = Enum.GetValues(typeof(OraSzerkezet));
            comboBoxKijelzes.DataSource = Enum.GetValues(typeof(Kijelzes));
            comboBoxSzin.DataSource = Enum.GetValues(typeof(TokSzine));
            comboBoxSzijAnyag.DataSource = Enum.GetValues(typeof(SzijAnyaga));
        }

        private void Orak_Load(object sender, EventArgs e)
        {
            dataGridViewOra.DataSource = orak.OraLista();
            comboBoxAfa.SelectedIndex = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBoxID.Text = dataGridViewOra.CurrentRow.Cells[0].Value.ToString();
                textBoxOraCikk.Text = dataGridViewOra.CurrentRow.Cells[1].Value.ToString();
                textBoxMarka.Text = dataGridViewOra.CurrentRow.Cells[2].Value.ToString();
                textBoxTipus.Text = dataGridViewOra.CurrentRow.Cells[3].Value.ToString();
                numericUpDownNetto.Value = (int)dataGridViewOra.CurrentRow.Cells[4].Value;
                comboBoxAfa.Text = dataGridViewOra.CurrentRow.Cells[5].Value.ToString();
                numericUpDownBrutto.Value = (int)dataGridViewOra.CurrentRow.Cells[6].Value;
                numericUpDownDarab.Value = (int)dataGridViewOra.CurrentRow.Cells[7].Value;
                comboBoxNem.Text = dataGridViewOra.CurrentRow.Cells[8].Value.ToString();
                comboBoxSzerkezet.Text = dataGridViewOra.CurrentRow.Cells[9].Value.ToString();
                comboBoxKijelzes.Text = dataGridViewOra.CurrentRow.Cells[10].Value.ToString();
                comboBoxSzin.Text = dataGridViewOra.CurrentRow.Cells[11].Value.ToString();
                comboBoxSzijAnyag.Text = dataGridViewOra.CurrentRow.Cells[12].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonOraMezoTorol_Click(object sender, EventArgs e)
        {
            textBoxOraCikk.Text = "";
            textBoxMarka.Text = "";
            textBoxTipus.Text = "";
            numericUpDownNetto.Value = 1;
            comboBoxAfa.SelectedIndex = 0;
            numericUpDownBrutto.Value = 1;
            numericUpDownDarab.Value = 1;
            comboBoxSzerkezet.SelectedIndex = 0;
            comboBoxKijelzes.SelectedIndex = 0;
            comboBoxNem.SelectedIndex = 0;
            comboBoxSzijAnyag.SelectedIndex = 0;
            comboBoxSzin.SelectedIndex = 0;
        }

        private void buttonOraUj_Click(object sender, EventArgs e)
        {
            string cikk_szam = textBoxOraCikk.Text;
            string marka = textBoxMarka.Text;
            string tipus = textBoxTipus.Text;
            int netto_ar = (int)numericUpDownNetto.Value;
            string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
            int brutto_ar = (int)numericUpDownBrutto.Value;
            int darabszam = (int)numericUpDownDarab.Value;
            string nem = comboBoxNem.SelectedValue.ToString();
            string szerkezet = comboBoxSzerkezet.SelectedItem.ToString();
            string kijezles = comboBoxKijelzes.SelectedItem.ToString();
            string tok_szine = comboBoxSzin.SelectedItem.ToString();
            string szij_anyaga = comboBoxSzijAnyag.SelectedItem.ToString();

            if (cikk_szam.Trim().Equals("") || marka.Trim().Equals("") || tipus.Trim().Equals(""))
            {
                MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Boolean oraBeilleszt = orak.oraBeilleszt(cikk_szam, marka, tipus, netto_ar,afa_kulcs,brutto_ar,darabszam, nem, szerkezet, kijezles, tok_szine, szij_anyaga);
                if (oraBeilleszt)
                {
                    dataGridViewOra.DataSource = orak.OraLista();
                    MessageBox.Show("Termék sikeresen hozzáadva a listához.", "Sikeres",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Hiba - Termék felvitele sikertelen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void buttonOraModosit_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                string cikk_szam = textBoxOraCikk.Text;
                string marka = textBoxMarka.Text;
                string tipus = textBoxTipus.Text;
                int netto_ar = (int)numericUpDownNetto.Value;
                string afa_kulcs = comboBoxAfa.SelectedItem.ToString();
                int brutto_ar = (int)numericUpDownBrutto.Value;
                int darabszam = (int)numericUpDownDarab.Value;
                string nem = comboBoxNem.SelectedValue.ToString();
                string szerkezet = comboBoxSzerkezet.SelectedItem.ToString();
                string kijezles = comboBoxKijelzes.SelectedItem.ToString();
                string tok_szine = comboBoxSzin.SelectedItem.ToString();
                string szij_anyaga = comboBoxSzijAnyag.SelectedItem.ToString();
                try
                {
                    id = Convert.ToInt32(textBoxID.Text);
                    if (cikk_szam.Trim().Equals("") || marka.Trim().Equals("") || tipus.Trim().Equals(""))
                    {
                        MessageBox.Show("Hiba- Minden mező kitöltése kötelező!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Boolean oraModosit = orak.oraModosit(id, cikk_szam, marka, tipus, netto_ar, afa_kulcs, brutto_ar, darabszam, nem, szerkezet, kijezles, tok_szine, szij_anyaga);
                        if (oraModosit)
                        {
                            dataGridViewOra.DataSource = orak.OraLista();
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void buttonOraTorol_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Valóban törli a kijelölt elemet?","Törlés",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(textBoxID.Text);
                    if (orak.oraTorol(id))
                    {
                        dataGridViewOra.DataSource = orak.OraLista();
                        MessageBox.Show("Termék sikeresen törölve!", "Termék törölve", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonOraMezoTorol.PerformClick();
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

        private void buttonOraKilep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy kilép?", "Kilépés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
            
        }


        private void numericUpDownNetto_ValueChanged(object sender, EventArgs e)
        {
            
            if(comboBoxAfa.Text == "27%")
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
        private void copyAlltoClipboard()
        {
            dataGridViewOra.RowHeadersVisible = false;
            dataGridViewOra.SelectAll();
            DataObject dataObj = dataGridViewOra.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void buttonExcelKiment_Click(object sender, EventArgs e)
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
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE ID LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Cikkszám")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE cikk_szam LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Márka")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE marka LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Típus")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE tipus LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Nettóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE Netto_ar LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Bruttóár")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE Brutto_ar LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Nem")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE nem LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Szerkezet")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE szerkezet LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Kijelzés")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE kijelzes LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Tokszíne")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE tok_szine LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
            else if (comboBoxKategoria.Text == "Szíjanyaga")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `ID`, `cikk_szam`, `marka`, `tipus`, `Netto_ar`, `Afa_kulcs`, `Brutto_ar`,`Darabszam`,`nem`,`szerkezet`,`kijelzes`,`tok_szine`,`szij_anyaga` FROM `orak` WHERE szij_anyaga LIKE '" + textBoxKeres.Text + "%'", connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dataGridViewOra.DataSource = table;
            }
        }
    }
}
