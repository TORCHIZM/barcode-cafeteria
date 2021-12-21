using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu frm1 = new Menu(); 
            this.Hide();
            frm1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ogrenciBul();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            verilerigöster();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void verilerigöster()
        {
            listView1.Items.Clear();

            if(baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select *From OgrenciBilgileri", baglan);
            OleDbDataReader oku = komut.ExecuteReader();
            int kactanevar = 0;

            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["Ad"].ToString();
                ekle.SubItems.Add(oku["Soyad"].ToString());
                ekle.SubItems.Add(oku["Sınıf"].ToString());
                ekle.SubItems.Add(oku["Numara"].ToString());
                ekle.SubItems.Add(oku["Köy"].ToString());
                ekle.SubItems.Add(oku["Barkod"].ToString());
                ekle.SubItems.Add(oku["Kaçış"].ToString());
                ekle.SubItems.Add(oku["NormalGiriş"].ToString());
                ekle.SubItems.Add(oku["KöylüGiriş"].ToString());
                kactanevar++;
                listView1.Items.Add(ekle);
            }
            baglan.Close();
            label7.Text = "Kayıtlı öğrenci: " + kactanevar.ToString();
            label7.Visible = true;
        }

        private void şifreniziDeğiştirinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void bilgileriGuncelle()
        {
            try
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                //OleDbCommand komut = new OleDbCommand("Select *From OgrenciBilgileri where Ad='" + textBox1.Text + "' and Soyad ='" + textBox2.Text + "' and Sınıf='" + textBox3.Text + "' and Numara='" + textBox4.Text + "' and Köy='" + comboBox1.SelectedItem.ToString() + "' and Barkod='" + maskedTextBox1.Text + "'", baglan);
                //OleDbDataReader dr = komut.ExecuteReader();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE OgrenciBilgileri SET Ad='" + textBox1.Text + "', Soyad='" + textBox2.Text + "', Sınıf='" + textBox3.Text + "', Numara='" + textBox4.Text + "', Köy='" + comboBox1.Text + "', Köylü=@Köylü WHERE Barkod=" + maskedTextBox1.Text + " ";

                if (comboBox1.Text == "İznik")
                {
                    cmd.Parameters.AddWithValue("@Köylü", "0");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Köylü", "1");
                }

                cmd.ExecuteNonQuery();
                baglan.Close();

                if (Directory.Exists(Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg"))
                {
                    File.Delete(Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg");
                }
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = maskedTextBox1.Text
                };
                pictureBox2.Image.Save(Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg");
                }
            catch (Exception hata)
            {
                int sonuc;
                string girilen;

                girilen = "Must descalar";
                sonuc = hata.Message.IndexOf(girilen);

                if (sonuc > 0)
                {
                    MessageBox.Show(hata.Message, "Hata Oluştu!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if(baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                    verilerigöster();
                }
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ogrenciBul();
            }
        }

        private void ogrenciBul()
        {
            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            string kayit = "SELECT * from OgrenciBilgileri where Barkod=@barkod";
            OleDbCommand komut = new OleDbCommand(kayit, baglan);
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
            if (baglan.State == ConnectionState.Open)
            {
                baglan.Close();
            }
        }

        private void veritabanıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = baglan;
            cmd.CommandText = "UPDATE OgrenciBilgileri SET BugunGirmis=@BugunGirmis";
            cmd.Parameters.AddWithValue("@BugunGirmis", 0);
            cmd.ExecuteNonQuery();
            if (baglan.State == ConnectionState.Open)
            {
                baglan.Close();
            }
            MessageBox.Show("Bütün öğrenciler bugün tekrar girebilir.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void şifreDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SifreDegistir sifreDegistir = new SifreDegistir();
            sifreDegistir.Show();
            this.Hide();
        }

        private void öğretmenİstatistikleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgretmenIstatistikleri ogretmenIstatistikleri = new OgretmenIstatistikleri();
            ogretmenIstatistikleri.Show();
            this.Hide();
        }

        private void öğretmenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loglar loglar = new Loglar();
            loglar.Show();
            this.Hide();
        }

        private void öğretmenEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgretmenEkle ogretmenEkle = new OgretmenEkle();
            ogretmenEkle.Show();
            this.Hide();
        }

        private void yedekAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kopyalanacakDosya = Application.StartupPath + "\\Veritabani.mdb";

            File.Copy(kopyalanacakDosya, Application.StartupPath + "\\Veritabanı Yedek\\Veritabanı" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            FileInfo info = new FileInfo(Application.StartupPath + "\\Veritabanı Yedek\\Veritabanı" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            info.MoveTo(Application.StartupPath + "\\Veritabanı Yedek\\Veritabanı" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".mdb");
            File.Delete(Application.StartupPath + "\\Veritabanı Yedek\\Veritabanı" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            MessageBox.Show("Dosya Kopyalama İşlemi Başarılı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void öğrenciEkleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OgrenciEkle ogrenciEkle = new OgrenciEkle();
            this.Dispose();
            ogrenciEkle.Show();
        }

        private void öğrenciSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrenciSil ogrenciSil = new OgrenciSil();
            this.Dispose();
            ogrenciSil.Show();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox4.Text = listView1.SelectedItems[0].SubItems[3].Text;
            comboBox1.Text = listView1.SelectedItems[0].SubItems[4].Text;
            maskedTextBox1.Text = listView1.SelectedItems[0].SubItems[5].Text;
            pictureBox2.ImageLocation = Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg";
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            verilerigöster();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            ogrenciBul();
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg";
            dosya.ShowDialog();
            pictureBox2.ImageLocation = dosya.FileName;
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Öğrenci bilgileri değiştirilecek, onaylıyor musunuz?", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bilgileriGuncelle();
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ogrenciBul();
            }
        }

        Point offset;
        bool dragging;

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            { dragging = true; offset = e.Location; }
        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }
    }
}
