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
    public partial class ExcelIlerleme_ : Form
    {
        public ExcelIlerleme_()
        {
            InitializeComponent();
        }

        private void ExcelIlerleme__Load(object sender, EventArgs e)
        {
            Say();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        string Barkod, Ad, Soyad, Sınıf, Köy, Numara;

        Point offset;
        bool dragging;

        private void ExcelIlerleme__MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void ExcelIlerleme__MouseDown(object sender, MouseEventArgs e)
        {
            { dragging = true; offset = e.Location; }
        }

        private void ExcelIlerleme__MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        int ilerleme, eklenemeyen;

        private void Say()
        {
            ExceldenAktar exel = new ExceldenAktar();

            OleDbConnection xlsxbaglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=excel.xlsx; Extended Properties='Excel 8.0;HDR=YES'");

            ListViewItem ekle = new ListViewItem();
            if (xlsxbaglanti.State == ConnectionState.Closed)
            {
                xlsxbaglanti.Open();
            }
            OleDbCommand komut = new OleDbCommand("SELECT * FROM [Sayfa1$]", xlsxbaglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                Barkod = oku["Barkod"].ToString();
                Ad = (oku["Ad"].ToString());
                Soyad = (oku["Soyad"].ToString());
                Sınıf = (oku["Sınıf"].ToString());
                Köy = (oku["Köy"].ToString());
                Numara = (oku["Numara"].ToString());
                bunifuMaterialTextbox1.Text = "Alınıyor: " + Barkod + " " + Ad + " " + Soyad;
                ilerleme++;
                bunifuMaterialTextbox1.Text = ilerleme.ToString();
                bunifuCircleProgressbar1.Value = (100 * ilerleme) / ilerleme;
                Aktar();
            }
            if (xlsxbaglanti.State == ConnectionState.Open)
            {
                xlsxbaglanti.Close();
            }
        }

        private void Aktar()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                string kontrolkayit = "SELECT * from OgrenciBilgileri where Barkod=@barkod";
                OleDbCommand kontrolkomut = new OleDbCommand(kontrolkayit, baglanti);
                kontrolkomut.Parameters.AddWithValue("@barkod", Barkod);
                OleDbDataAdapter da = new OleDbDataAdapter(kontrolkomut);
                OleDbDataReader dr = kontrolkomut.ExecuteReader();
                if (dr.Read())
                {
                    eklenemeyen++;
                    richTextBox1.Text = Barkod + " Barkodlu öğrenci eklenemedi!\r\n" + richTextBox1.Text;
                    bunifuMaterialTextbox2.Text = "Eklenemeyen: " + eklenemeyen;
                }
                else
                {
                    string eklekayit = "insert into OgrenciBilgileri(Ad,Soyad,Sınıf,Numara,Köy,Barkod,BugunGirmis,Kaçış,KöylüGiriş,NormalGiriş,Köylü) values (@ad,@soyad,@sınıf,@numara,@köy,@barkod,@bugungirmis,@kaçış,@köylügiriş,@normalgiriş,@köylü)";
                    OleDbCommand eklekomut = new OleDbCommand(eklekayit, baglanti);
                    eklekomut.Parameters.AddWithValue("@ad", Ad);
                    eklekomut.Parameters.AddWithValue("@soyad", Soyad);
                    eklekomut.Parameters.AddWithValue("@sınıf", Sınıf);
                    eklekomut.Parameters.AddWithValue("@numara", Numara);
                    eklekomut.Parameters.AddWithValue("@köy", Köy);
                    eklekomut.Parameters.AddWithValue("@barkod", Barkod);
                    eklekomut.Parameters.AddWithValue("@bugungirmis", "0");
                    eklekomut.Parameters.AddWithValue("@kaçış", 0);
                    eklekomut.Parameters.AddWithValue("@köylügiriş", 0);
                    eklekomut.Parameters.AddWithValue("@normalgiriş", 0);

                    if (Köy == "İznik")
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
                }
                if(bunifuCircleProgressbar1.Value == 100)
                {
                    bunifuImageButton1.Visible = true;
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Öğrenci eklenemedi!" + hata.Message, "HATA");
                eklenemeyen++;
            }
            finally
            {
                //MessageBox.Show(ilerleme + " Öğrenci veri tabanına aktarıldı.", "İşlem Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return;
        }
    }
}
