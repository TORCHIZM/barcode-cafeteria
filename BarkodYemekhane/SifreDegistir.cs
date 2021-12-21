using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class SifreDegistir : Form
    {
        public SifreDegistir()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb"); //; Integrated Security=True

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Dispose();
        }

        private void degistir()
        {
            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select *From Adminler where k_adi='" + textBox1.Text + "' and sifre ='" + textBox2.Text + "'", baglan);
            OleDbDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                if (textBox3.Text == textBox4.Text)
                {
                    if (MessageBox.Show("Şifreniz değiştirilecek, onaylıyor musunuz?", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        sifreyiDegistir();
                    }
                }
                else
                {
                    MessageBox.Show("Yeni şifreler birbiriyle uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
        }
        private void sifreyiDegistir()
        {
            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

            try
            {
                baglanti.Open();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglanti;
                cmd.CommandText = "UPDATE Adminler SET sifre='" + textBox4.Text + "' WHERE k_adi='" + textBox1.Text + "' ";
                cmd.ExecuteNonQuery();

                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(baglan.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void SifreDegistir_Load(object sender, EventArgs e)
        {
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            degistir();
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
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
