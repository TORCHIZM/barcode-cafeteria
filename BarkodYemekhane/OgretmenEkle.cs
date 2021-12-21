using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class OgretmenEkle : Form
    {
        public OgretmenEkle()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void kontrolEt()
        {
            string k1 = textBox5.Text;
            if (k1 == "")
            {
                MessageBox.Show("Bir kullanıcı adı girmediniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                string kayit = "SELECT * from Adminler where k_adi=@k_adi";
                OleDbCommand komut = new OleDbCommand(kayit, baglan);
                komut.Parameters.AddWithValue("@k_adi", textBox5.Text);
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["k_adi"].ToString();
                    textBox2.Text = dr["yapilangirisler"].ToString();
                }
                else
                {
                    MessageBox.Show("Öğretmen bulunamadı!", "Hata");
                }
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kontrolEt();
            }
        }

        private void Ekle()
        {
            try
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }

                string kontrolkayit = "SELECT * from Adminler where k_adi=@k_adi";
                OleDbCommand kontrolkomut = new OleDbCommand(kontrolkayit, baglan);
                kontrolkomut.Parameters.AddWithValue("@k_adi", textBox1.Text);
                OleDbDataAdapter da = new OleDbDataAdapter(kontrolkomut);
                OleDbDataReader dr = kontrolkomut.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor! Lütfen başka bir kullanıcı adı seçiniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string kayit = "insert into Adminler(k_adi,sifre) values (@k_adi,@sifre)";
                    OleDbCommand komut = new OleDbCommand(kayit, baglan);
                    komut.Parameters.AddWithValue("@k_adi", textBox1.Text);
                    komut.Parameters.AddWithValue("@sifre", textBox2.Text);
                    komut.Parameters.AddWithValue("@yapilangirisler", 0);
                    komut.ExecuteNonQuery();
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                    MessageBox.Show(textBox1.Text + " sisteme kayıt edildi.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Öğretmen eklenemedi!" + hata.Message, "HATA");
            }
        }

        private void OgretmenEkle_Load(object sender, EventArgs e)
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

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            Ekle();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            kontrolEt();
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
