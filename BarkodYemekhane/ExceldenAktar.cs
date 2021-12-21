using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class ExceldenAktar : Form
    {
        public ExceldenAktar()
        {
            InitializeComponent();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public static string DosyaAdi, DosyaYolu;
        public string DosyaaYolu;

        OleDbConnection xlsxbaglanti = new OleDbConnection(@"Provider=Microsoft.JET.OLEDB.4.0;Data Source=excel.xlsx; Extended Properties='Excel 8.0;HDR=YES'");
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            exceldenListele();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            ExcelIlerleme_ excelIlerleme = new ExcelIlerleme_();
            excelIlerleme.Show();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Dispose();
        }

        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void exceldenListele()
        {
            ListViewItem ekle = new ListViewItem();
            if (xlsxbaglanti.State == ConnectionState.Closed)
            {
                xlsxbaglanti.Open();
            }
            OleDbCommand komut = new OleDbCommand("SELECT * FROM [Sayfa1$]", xlsxbaglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                try
                {
                    ekle.Text = (oku["Barkod"].ToString());
                    ekle.SubItems.Add(oku["Ad"].ToString());
                    ekle.SubItems.Add(oku["Soyad"].ToString());
                    ekle.SubItems.Add(oku["Sınıf"].ToString());
                    ekle.SubItems.Add(oku["Köy"].ToString());
                    ekle.SubItems.Add(oku["Numara"].ToString());
                    MessageBox.Show(oku["Barkod"].ToString() +
                    (oku["Ad"].ToString()) +
                    (oku["Soyad"].ToString()) +
                    (oku["Sınıf"].ToString()) +
                    (oku["Köy"].ToString()) +
                    (oku["Numara"].ToString()));
                }
                catch
                {

                }
                finally
                {

                }
            }
            if (xlsxbaglanti.State == ConnectionState.Open)
            {
                xlsxbaglanti.Close();
            }
        }
    }
}
