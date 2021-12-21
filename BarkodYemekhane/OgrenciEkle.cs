using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class OgrenciEkle : Form
    {
        public OgrenciEkle()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void ekle()
        {
            try
            {
                if (maskedTextBox1 == null)
                {
                    MessageBox.Show("Bir barkod girmediniz!", "Hata");
                }
                else
                {
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    string kontrolkayit = "SELECT * from OgrenciBilgileri where Barkod=" + maskedTextBox1.Text;
                    OleDbCommand kontrolkomut = new OleDbCommand(kontrolkayit, baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(kontrolkomut);
                    OleDbDataReader dr = kontrolkomut.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu barkodla bir öğrenci zaten var! Lütfen başka bir barkod seçiniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string eklekayit = "insert into OgrenciBilgileri(Ad,Soyad,Sınıf,Numara,Köy,Barkod,BugunGirmis,Kaçış,KöylüGiriş,NormalGiriş,Köylü) values (@ad,@soyad,@sınıf,@numara,@köy,@barkod,@bugungirmis,@kaçış,@köylügiriş,@normalgiriş,@köylü)";
                        OleDbCommand eklekomut = new OleDbCommand(eklekayit, baglanti);
                        eklekomut.Parameters.AddWithValue("@ad", textBox1.Text);
                        eklekomut.Parameters.AddWithValue("@soyad", textBox2.Text);
                        eklekomut.Parameters.AddWithValue("@sınıf", textBox3.Text);
                        int numara = Convert.ToInt16(textBox4.Text);
                        eklekomut.Parameters.AddWithValue("@numara", numara);
                        eklekomut.Parameters.AddWithValue("@köy", comboBox1.Text);
                        eklekomut.Parameters.AddWithValue("@barkod", maskedTextBox1.Text);
                        eklekomut.Parameters.AddWithValue("@bugungirmis", "0");
                        eklekomut.Parameters.AddWithValue("@kaçış", 0);
                        eklekomut.Parameters.AddWithValue("@köylügiriş", 0);
                        eklekomut.Parameters.AddWithValue("@normalgiriş", 0);

                        if (pictureBox2.Image != null)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.FileName = maskedTextBox1.Text;
                            pictureBox2.Image.Save(Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg");
                        }

                        if (comboBox1.Text == "İznik")
                        {
                            eklekomut.Parameters.AddWithValue("@köylü", "0");
                        }
                        else
                        {
                            eklekomut.Parameters.AddWithValue("@köylü", "1");
                        }
                        eklekomut.ExecuteNonQuery();
                        if (baglanti.State == ConnectionState.Open)
                        {
                            baglanti.Close();
                        }
                        if (checkBox1.Checked == true)
                        {
                            MessageBox.Show(textBox1.Text + " " + textBox2.Text + "(" + textBox4.Text + " " + comboBox1.Text + ")" + " sisteme kayıt edildi.");
                        }
                    }
                    maskedTextBox1.Clear();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Öğrenci eklenemedi!" + hata.Message, "HATA");
            }
            verileriGoster();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ekle();
            }
        }

        private void OgrenciEkle_Load(object sender, EventArgs e)
        {

        }   

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg";
            dosya.ShowDialog();
            pictureBox2.ImageLocation = dosya.FileName;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            ekle();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1 == null)
            {
                MessageBox.Show("Bir barkod girmediniz!", "Hata");
            }
            else
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                string kayit = "SELECT * from OgrenciBilgileri where Barkod=@barkod";
                OleDbCommand komut = new OleDbCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@barkod", maskedTextBox1.Text);
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["Ad"].ToString();
                    textBox2.Text = dr["Soyad"].ToString();
                    textBox3.Text = dr["Sınıf"].ToString();
                    textBox4.Text = dr["Numara"].ToString();
                    comboBox1.Text = dr["Köy"].ToString();
                    pictureBox2.ImageLocation = Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg";
                }
                else
                {
                    MessageBox.Show("Öğrenci bulunamadı!", "Hata");
                }
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ekle();
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void verileriGoster()
        {
            listView1.Items.Clear();

            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select *From OgrenciBilgileri", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            int kactanevar = 0;

            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["Barkod"].ToString();
                ekle.SubItems.Add(oku["Ad"].ToString());
                ekle.SubItems.Add(oku["Soyad"].ToString());
                ekle.SubItems.Add(oku["Sınıf"].ToString());
                ekle.SubItems.Add(oku["Köy"].ToString());
                ekle.SubItems.Add(oku["Numara"].ToString());
                kactanevar++;
                listView1.Items.Add(ekle);
            }
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            kayitliogrencilabel.Text = "Kayıtlı Öğrenci:" +  kactanevar.ToString();
            kayitliogrencilabel.Visible = true;
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            ExceldenAktar exceldenAktar = new ExceldenAktar();
            exceldenAktar.Show();
            this.Dispose();
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
