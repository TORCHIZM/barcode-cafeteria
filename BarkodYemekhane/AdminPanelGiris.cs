using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class AdminPanelGiris : Form
    {
        public AdminPanelGiris()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb"); //; Integrated Security=True

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void GirisYap()
        {
            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select * From Adminler where k_adi='" + textBox1.Text + "' and sifre ='" + textBox2.Text + "'", baglan);
            OleDbDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                this.Dispose();
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
                adminPanel.label6.Text = (textBox1.Text + " olarak giriş yapıldı.");
                girisYapti();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata");
            }
            if (baglan.State == ConnectionState.Open)
            {
                baglan.Close();
            }
        }

        private void girisYapti()
        {
            int yapılangiris = 0;
            try
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                string kayit = "SELECT * from Adminler where k_adi=@k_adi";
                OleDbCommand komut = new OleDbCommand(kayit, baglan);
                komut.Parameters.AddWithValue("@k_adi", textBox1.Text);
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    yapılangiris = Convert.ToInt32(dr["yapilangirisler"]);
                }
                kayit = "UPDATE Adminler SET yapilangirisler=@yapilangirisler WHERE k_adi='" + textBox1.Text + "' ";
                OleDbCommand cmd = new OleDbCommand(kayit, baglan);
                yapılangiris++;
                cmd.Parameters.AddWithValue("@yapilangirisler", yapılangiris);
                cmd.ExecuteNonQuery();

                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata Oluştu!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                    this.Hide();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                GirisYap();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                GirisYap();
            }
        }

        private void AdminPanelGiris_Load(object sender, EventArgs e)
        {
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            GirisYap();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
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
