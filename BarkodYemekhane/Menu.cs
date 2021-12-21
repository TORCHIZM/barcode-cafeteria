using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net.Mail;

namespace BarkodYemekhane
{
    public partial class Menu : Form
    {
        private static string girmisMi;
        private static bool oncelik = true;
        private static string koylumu = "0";
        private static bool alindi = false;

        public Menu()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

        private void kontrolet(string barkod)
        {
            string k1 = maskedTextBox1.Text;
            if (k1 == "")
            {
                MessageBox.Show("Bir barkod girmediniz!","Hata");
            }
            else
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
                    textBox6.Text = dr["Köy"].ToString();
                    girmisMi = dr["BugunGirmis"].ToString();
                    koylumu = dr["Köylü"].ToString();
                    pictureBox3.ImageLocation = Application.StartupPath + "\\Resimler\\"+ maskedTextBox1.Text + ".jpg";
                    alindi = true;
                }
                else
                {
                    MessageBox.Show("Öğrenci bulunamadı!", "Hata");
                }
                if(alindi == true)
                {
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                    string rtb = richTextBox1.Text;
                    if (girmisMi == "1")
                    {
                        if (rtb == "")
                        {
                            richTextBox1.Text = "Öğrenci bugün zaten girmiş.";
                        }
                        else
                        {
                            richTextBox1.Text = "Öğrenci bugün zaten girmiş.\r\n" + rtb;
                        }
                    }
                    else
                    {
                        if (oncelik == true)
                        {
                            if (koylumu == "1")
                            {
                                if (rtb == "")
                                {
                                    richTextBox1.Text = "Öğrenci yemekhaneye girebilir.";
                                    bugunGirdi();
                                    koyluGirdi();
                                }
                                else
                                {
                                    richTextBox1.Text = "Öğrenci yemekhaneye girebilir.\r\n" + rtb;
                                    bugunGirdi();
                                    koyluGirdi();
                                }
                            }
                            else
                            {
                                if (rtb == "")
                                {
                                    richTextBox1.Text = "Öğrenci yemekhaneye giremez: Şuanda sadece taşımalı öğrenciler girebilir.";
                                    KaçmayıDenedi();
                                }
                                else
                                {
                                    richTextBox1.Text = "Öğrenci yemekhaneye giremez: Şuanda sadece taşımalı öğrenciler girebilir.\r\n" + rtb;
                                    KaçmayıDenedi();
                                }
                            }
                        }
                        else
                        {
                            if (rtb == "")
                            {
                                richTextBox1.Text = "Öğrenci yemekhaneye girebilir.";
                                bugunGirdi();
                                normalGirdi();
                            }
                            else
                            {
                                richTextBox1.Text = "Öğrenci yemekhaneye girebilir.\r\n" + rtb;
                                bugunGirdi();
                                normalGirdi();
                            }
                        }
                    }
                }
            }
            maskedTextBox1.Clear();
        }

        private void KaçmayıDenedi()
        {
            int kactigiSuree = 0;
            try
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
                    kactigiSuree = Convert.ToInt32(dr["Kaçış"]);
                }

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE OgrenciBilgileri SET Kaçış=@kacis WHERE Barkod=" + maskedTextBox1.Text + " ";
                kactigiSuree++;
                cmd.Parameters.AddWithValue("@kacis", kactigiSuree);
                cmd.ExecuteNonQuery();

                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
                string rtb = richTextBox1.Text;
                if (rtb == "")
                {
                    richTextBox1.Text = textBox1.Text = textBox1.Text + textBox2.Text + " Taşımalı öğrencilerin arasına karışmaya çalıştı!";
                }
                else
                {
                    richTextBox1.Text = textBox1.Text + textBox2.Text + " Taşımalı öğrencilerin arasına karışmaya çalıştı!\r\n" + rtb;
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
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ogretmenGirisi();
        }

        private void bugunGirdi()
        {
            try
            {
                if(baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE OgrenciBilgileri SET BugunGirmis=@bugungirmis WHERE Barkod=" + maskedTextBox1.Text + " ";
                cmd.Parameters.AddWithValue("@bugungirmis", "1");
                cmd.ExecuteNonQuery();
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message,"Hata");
            }
        }

        private void Menu_Load_1(object sender, EventArgs e)
        {
            bunifuDatepicker1.Value = DateTime.Now;
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                kontrolet(maskedTextBox1.Text);
            }
        }

        private void öğretmenGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogretmenGirisi();
        }

        private void ogretmenGirisi()
        {
            AdminPanelGiris giris = new AdminPanelGiris();
            giris.Show();
        }

        private void koyluGirdi()
        {
            int koyluGiris = 0;
            try
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
                    koyluGiris = Convert.ToInt32(dr["KöylüGiriş"]);
                }
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE OgrenciBilgileri SET KöylüGiriş=@koylugiris WHERE Barkod=" + maskedTextBox1.Text + " ";
                koyluGiris++;
                cmd.Parameters.AddWithValue("@koylugiris", koyluGiris);
                cmd.ExecuteNonQuery();
                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }

                kayitlaraKoyluEkle();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata Oluştu!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglan.State == ConnectionState.Open)
                {
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                }
            }
        }

        private void kayitlaraKoyluEkle()
        {
            int toplam = 0;
            int iznikli = 0;
            int köylü = 0;
            try
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                string kayit = "SELECT * from Kayıtlar where Tarih=@Tarih";
                OleDbCommand komut = new OleDbCommand(kayit, baglan);
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToShortDateString());
                OleDbDataAdapter daa = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    toplam = Convert.ToInt32(dr["Toplam"]);
                    iznikli = Convert.ToInt32(dr["İznikli"]);
                    köylü = Convert.ToInt32(dr["Köylü"]);
                }
                baglan.Close();

                OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Veritabani.mdb");

                baglanti.Open();
                OleDbCommand cmd1 = new OleDbCommand();
                cmd1.Connection = baglanti;
                cmd1.CommandText = "UPDATE Kayıtlar SET Köylü=@Köylü,Toplam=@Toplam WHERE Tarih='" + DateTime.Now.ToShortDateString() + "' ";
                köylü++;
                toplam = iznikli + köylü;
                cmd1.Parameters.AddWithValue("@Köylü", köylü);
                cmd1.Parameters.AddWithValue("@Toplam", toplam);

                cmd1.ExecuteNonQuery();

                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
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
                }
            }
        }

        private void normalGirdi()
        {
            int normalGiris = 0;
            try
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
                    normalGiris = Convert.ToInt32(dr["NormalGiriş"]);
                }
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE OgrenciBilgileri SET NormalGiriş=@NormalGiriş WHERE Barkod=" + maskedTextBox1.Text + " ";
                normalGiris++;
                cmd.Parameters.AddWithValue("@NormalGiriş", normalGiris);
                cmd.ExecuteNonQuery();

                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }

                kayitlaraIznikliEkle();
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
                }
            }
        }

        private void kayitlaraIznikliEkle()
        {
            int toplam = 0;
            int iznikli = 0;
            int köylü = 0;
            try
            {
                if (baglan.State == ConnectionState.Closed)
                {
                    baglan.Open();
                }
                string kayit = "SELECT * from Kayıtlar where Tarih=@Tarih";
                OleDbCommand komut = new OleDbCommand(kayit, baglan);
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToShortDateString());
                OleDbDataAdapter daa = new OleDbDataAdapter(komut);
                OleDbDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    toplam = Convert.ToInt32(dr["Toplam"]);
                    iznikli = Convert.ToInt32(dr["İznikli"]);
                    köylü = Convert.ToInt32(dr["Köylü"]);
                }
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = baglan;
                cmd.CommandText = "UPDATE Kayıtlar SET İznikli=@İznikli,Toplam=@Toplam WHERE Tarih='" + DateTime.Now.ToShortDateString() + "' ";
                iznikli++;
                toplam = iznikli + köylü;
                cmd.Parameters.AddWithValue("@İznikli", iznikli);
                cmd.Parameters.AddWithValue("@Toplam", toplam);
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
                }
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                kontrolet(maskedTextBox1.Text);
            }
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            kontrolet(maskedTextBox1.Text);
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        string rtb;
        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox2.Checked == false)
            {
                oncelik = false;
                rtb = richTextBox1.Text;
                if (rtb == "")
                {
                    richTextBox1.Text = "Yemekhaneye herkes girebilir.";
                }
                else
                {
                    richTextBox1.Text = "Yemekhaneye herkes girebilir.\r\n" + rtb;
                }
            }
            else if (bunifuCheckbox2.Checked == true)
            {
                oncelik = true;
                rtb = richTextBox1.Text;
                if (rtb == "")
                {
                    richTextBox1.Text = "Yemekhaneye sadece taşımalı öğrenciler girebilir.";
                }
                else
                {
                    richTextBox1.Text = "Yemekhaneye sadece taşımalı öğrenciler girebilir.\r\n" + rtb;
                }
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://iznikmtal.meb.k12.tr/");
        }

        static int timer;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;
            if(timer == 20)
            {
                GC.Collect(GC.MaxGeneration);
                GC.WaitForPendingFinalizers();
                timer = 0;
            }
        }

        Point offset;
        bool dragging;

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

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}