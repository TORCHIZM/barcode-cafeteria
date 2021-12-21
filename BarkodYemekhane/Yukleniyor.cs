using System;
using System.Data;
using System.Windows.Forms;
//using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using ADOX;
using System.Drawing;

namespace BarkodYemekhane
{
    public partial class Yukleniyor : Form
    {
        public Yukleniyor()
        {
            InitializeComponent();
        }

        static int timer = 0;

        private void Yukleniyor_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;
            if(timer == 1)
            {
                Kontrol();
                timer1.Stop();
                timer1.Dispose();
                timer = 0;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer++;
            if (timer == 1)
            {
                timer2.Stop();
                timer2.Dispose();
                programiBaslat();
            }
        }

        static string VeritabaniAdi = "Veritabani";
        static bool BaglantiKuruldu = true;

        OleDbConnection con;
        //OleDbDataAdapter da;
        OleDbCommand cmd;
        //DataSet ds;

        public void Kontrol()
        {
            label1.Text = "Veritabanı bilgisi alınıyor..";
            /*SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=BarkodYemekhane;Integrated Security=True;MultipleActiveResultSets=True;");
            SqlCommand komut = new SqlCommand("SELECT Count(name) FROM master.mdb.sysdatabases WHERE name=@prmVeritabani", baglanti);*/

            con = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");
            cmd = new OleDbCommand();
            cmd.Parameters.AddWithValue("@prmVeriTabani", VeritabaniAdi);
            if (!(Directory.Exists(Application.StartupPath + "\\Resimler")))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Resimler");
            }
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    con.Open();
                }
                catch(Exception)
                {
                    //MessageBox.Show(hata.Message, "Veritabanı ile bağlantı kurulamıyor!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if(BaglantiKuruldu == true)
                    {
                        BaglantiKuruldu = false;
                    }
                }
            }
            if (con.State == ConnectionState.Open) {
                label1.Text = "Veritabanı bulundu, tarih kontrol ediliyor..";
                tarihKontrol();
                label1.Text = "Herşey hazır! Program başlatılıyor..";
                timer2.Start();
            }
            else
            {
                label1.Text = "Veritabanı bulunamadı, oluşturuluyor..";

                cmd = new OleDbCommand();

                //cmd.CommandText = "Create Database " + VeritabaniAdi;
                //cmd.ExecuteNonQuery();

                ADOX.Catalog cat = new ADOX.Catalog();

                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
                   "Data Source=" + Application.StartupPath + "\\Veritabani.mdb;" +
                   "Jet OLEDB:Engine Type=5");

                cat = null;

                label1.Text = "Veritabanı oluşturuldu..";
                TabloOlustur();
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        private void TabloOlustur()
        {
            label1.Text = "Tablolar oluşturuluyor..";
            //string baglantiCumlesi = "server=.\\SQLEXPRESS; database=BarkodYemekhane; integrated security=SSPI";
            string baglantiCumlesi = "Provider = Microsoft.JET.OLEDB.4.0; Data Source = " + Application.StartupPath + "\\Veritabani.mdb";
            using (OleDbConnection baglanti = new OleDbConnection(baglantiCumlesi))
            {
                try
                {

                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand("CREATE TABLE OgrenciBilgileri (Barkod integer not null default 0,Ad TEXT(50),Soyad TEXT(50),Sınıf TEXT(5),Numara integer,Köy TEXT(50),Köylü TEXT(1),BugunGirmis varchar(1),Kaçış integer not null default 0,KöylüGiriş integer not null default 0,NormalGiriş integer not null default 0)", baglanti))
                    {
                        command.ExecuteNonQuery();
                    }
                    if (baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }

                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand("CREATE TABLE Kayıtlar (Tarih text(10),Toplam integer not null default 0,İznikli integer not null default 0,Köylü integer not null default 0);", baglanti))
                    {
                        command.ExecuteNonQuery();
                    }
                    if (baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }

                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand("CREATE TABLE Adminler (k_adi text(20),sifre text(20),yapilangirisler integer not null default 0);", baglanti))
                    {
                        command.ExecuteNonQuery();
                    }
                    if (baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }

                    label1.Text = "Tablolara veriler yükleniyor..";
                    try
                    {
                        if (baglanti.State == ConnectionState.Closed)
                        {
                            baglanti.Open();
                        }

                        string kayit = "insert into Adminler(k_adi,sifre,yapilangirisler) values (@k_adi,@sifre,@yapilangirisler)";
                        cmd = new OleDbCommand(kayit, baglanti);
                        cmd.Parameters.AddWithValue("@k_adi", "IMTAL");
                        cmd.Parameters.AddWithValue("@sifre", "suleymanmutlu");
                        cmd.Parameters.AddWithValue("@yapilangirisler", 0);
                        cmd.ExecuteNonQuery();

                        if (baglanti.State == ConnectionState.Open)
                        {
                            baglanti.Close();
                        }
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show(hata.Message, "Tablo eklenemedi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (baglanti.State == ConnectionState.Open)
                        {
                            baglanti.Close();
                        }
                    }

                    try
                    {
                        if (baglanti.State == ConnectionState.Closed)
                        {
                            baglanti.Open();
                        }
                        string kayit = "insert into Kayıtlar(Toplam, İznikli, Köylü, Tarih) values (@Toplam, @İznikli, @Köylü, @Tarih)";
                        cmd = new OleDbCommand(kayit, baglanti);

                        cmd.Parameters.AddWithValue("@Toplam", 0);
                        cmd.Parameters.AddWithValue("@İznikli", 0);
                        cmd.Parameters.AddWithValue("@Köylü", 0);
                        cmd.Parameters.AddWithValue("@Tarih", DateTime.Now.ToShortDateString());
                        cmd.ExecuteNonQuery();

                        if (baglanti.State == ConnectionState.Open)
                        {
                            baglanti.Close();
                        }
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show(hata.Message, "Tablo eklenemedi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (baglanti.State == ConnectionState.Open)
                        {
                            baglanti.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if(baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }
                }
                label1.Text = "Herşey hazır! Program başlatılıyor..";
                timer2.Start();
            }
        }

        //SqlConnection baglan = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=BarkodYemekhane;Integrated Security=True;MultipleActiveResultSets=True");
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void tarihKontrol()
        {
            string tarih = DateTime.Now.ToShortDateString();
            string tarihkontrol1 = "";

            if (baglan.State == ConnectionState.Closed)
            {
                baglan.Open();
            }
            OleDbCommand komut = new OleDbCommand("Select *From Kayıtlar where Tarih='" + tarih + "'", baglan);
            OleDbDataReader dr = komut.ExecuteReader();

            /*OleDbCommand cmd = new OleDbCommand("Select id, adi, soyadi from tablo where id=1", con);
            OleDbDataReader dr = cmd.ExecuteReader();*/

            if (dr.Read())
            {
                tarihkontrol1 = dr["Tarih"].ToString();
            }
            else
            {
                try
                {
                    if (baglan.State == ConnectionState.Closed)
                    {
                        baglan.Open();
                    }
                        string kayit = "insert into Kayıtlar(Toplam,İznikli,Köylü,Tarih) values (@Toplam,@İznikli,@Köylü,@Tarih)";
                        OleDbCommand cmd = new OleDbCommand(kayit, baglan);
                        cmd = new OleDbCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "insert into Kayıtlar(Toplam, İznikli, Köylü, Tarih) values(@Toplam, @İznikli, @Köylü, @Tarih)";
                        cmd.Parameters.AddWithValue("@Toplam", 0);
                        cmd.Parameters.AddWithValue("@İznikli", 0);
                        cmd.Parameters.AddWithValue("@Köylü", 0);
                        cmd.Parameters.AddWithValue("@Tarih", DateTime.Now.ToShortDateString());
                        cmd.ExecuteNonQuery();
                        if (baglan.State == ConnectionState.Open)
                        {
                            baglan.Close();
                        }
                        girisleriSifirla();
                    }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                }
                label1.Text = "Herşey hazır! Program başlatılıyor..";
                timer2.Start();
            }
        }

        private void girisleriSifirla()
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("UPDATE OgrenciBilgileri SET BugunGirmis=@BugunGirmis", baglan);
                cmd.Parameters.AddWithValue("@BugunGirmis", 0);
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                cmd.ExecuteNonQuery();
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
                label1.Text = "Herşey hazır! Program başlatılıyor..";
                timer2.Start();
            }
        }

        private void programiBaslat()
        {
            this.Hide();
            Menu menu = new Menu();
            menu.timer1.Start();
            menu.Show();
            if(BaglantiKuruldu == false)
            {
                baglan.Dispose();
                menu.label9.Text = ("VERİTABANI BAĞLANTISI KURULAMADI! PROGRAM PASİF DURUMDA.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
