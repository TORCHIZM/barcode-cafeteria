using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace BarkodYemekhane
{
    public partial class OgrenciSil : Form
    {
        public OgrenciSil()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPanel menu = new AdminPanel();
            this.Dispose();
            menu.Show();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void sil()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            OleDbCommand komut = new OleDbCommand("Delete From OgrenciBilgileri where barkod =(" + maskedTextBox1.Text + ")", baglanti);
            System.IO.File.Delete(Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg");
            komut.ExecuteNonQuery();
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            verilerigöster();
        }

        private void yardımAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bir öğrenciyi silmek için barkodunu yazdıktan sonra Öğrenciyi Sil Butonu'na tıklayabilir veya öğrencileri listeledikten sonra öğrenciye çift tıklayıp Öğrenciyi Sil' butonuna tıklayabilirsiniz.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void verilerigöster()
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
                ekle.SubItems.Add(oku["Kaçış"].ToString());
                ekle.SubItems.Add(oku["NormalGiriş"].ToString());
                ekle.SubItems.Add(oku["KöylüGiriş"].ToString());
                kactanevar++;
                listView1.Items.Add(ekle);
            }
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            label7.Text = "Toplam Kayıt: " + kactanevar.ToString();
            label7.Visible = true;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBox4.Text = listView1.SelectedItems[0].SubItems[0].Text;
            comboBox1.Text = listView1.SelectedItems[0].SubItems[4].Text;
            pictureBox2.ImageLocation = Application.StartupPath + "\\Resimler\\" + maskedTextBox1.Text + ".jpg";
        }

        private void ogrenciGoster()
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
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Öğrenci bulunamadı!", "Hata");
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ogrenciGoster();
            }
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            sil();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            verilerigöster();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            ogrenciGoster();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
