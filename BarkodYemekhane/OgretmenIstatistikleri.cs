using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class OgretmenIstatistikleri : Form
    {
        public OgretmenIstatistikleri()
        {
            InitializeComponent();
        }


        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void kontrolEt()
        {
            string k1 = textBox3.Text;
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
                komut.Parameters.AddWithValue("@k_adi", textBox3.Text);
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["k_adi"].ToString();
                    textBox2.Text = dr["yapilangirisler"].ToString();
                }
                else
                {
                    MessageBox.Show("Öğretmen bulunamadı!", "Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

            baglan.Open();
            OleDbCommand komut = new OleDbCommand("Select *From Adminler", baglan);
            OleDbDataReader oku = komut.ExecuteReader();
            int kactanevar = 0;

            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["k_adi"].ToString();
                ekle.SubItems.Add(oku["yapilangirisler"].ToString());
                kactanevar++;
                listView1.Items.Add(ekle);
            }
            if (baglan.State == ConnectionState.Open)
            {
                baglan.Close();
            }
            label3.Text = "Kayıtlı öğretmen: " + kactanevar.ToString();
            label3.Visible = true;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kontrolEt();
            }
        }

        private void OgretmenIstatistikleri_Load(object sender, EventArgs e)
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

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            kontrolEt();
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
