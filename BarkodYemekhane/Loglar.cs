using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class Loglar : Form
    {
        public Loglar()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void kontrolEt()
        {
            string k1 = maskedTextBox1.Text.Replace(",", ".");
            if (k1 == "")
            {
                MessageBox.Show("Bir tarih girmediniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                string kayit = "SELECT * from Kayıtlar where Tarih=@Tarih";
                OleDbCommand komut = new OleDbCommand(kayit, baglan);
                komut.Parameters.AddWithValue("@Tarih", k1);
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["Toplam"].ToString();
                    textBox2.Text = dr["İznikli"].ToString();
                    textBox3.Text = dr["Köylü"].ToString();
                }
                else
                {
                    MessageBox.Show("Tarih bulunamadı!", "Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
        }

        private void verileriGoster()
        {
            listView1.Items.Clear();

            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select *From Kayıtlar", baglan);
            OleDbDataReader oku = komut.ExecuteReader();
            int kactanevar = 0;
            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["Toplam"].ToString();
                ekle.SubItems.Add(oku["İznikli"].ToString());
                ekle.SubItems.Add(oku["Köylü"].ToString());
                ekle.SubItems.Add(oku["Tarih"].ToString());

                kactanevar++;
                listView1.Items.Add(ekle);
            }
            if (baglan.State == ConnectionState.Open)
            {
                baglan.Close();
            }
            label4.Text = "Kayıtlı tarih: " + kactanevar.ToString();
            label4.Visible = true;
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                kontrolEt();
            }
        }

        private void Loglar_Load(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        Point offset;
        bool dragging;

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            { dragging = true; offset = e.Location; }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }
    }
}
