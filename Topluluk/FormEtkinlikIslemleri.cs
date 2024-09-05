using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace vize_1
{
    public partial class FormEtkinlikIslemleri : Form
    {
        Veritabani baglanti = new Veritabani();
        public FormEtkinlikIslemleri()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEtkinlikID.Text = row.Cells["etkinlik_ID"].Value.ToString();
                textBoxEtkinlikAd.Text = row.Cells["etkinlik_ad"].Value.ToString();
                textBoxEtkinlikAciklama.Text = row.Cells["etkinlik_aciklama"].Value.ToString();
                comboBoxTopluluk.Text = row.Cells["topluluk_ad"].Value.ToString();

                //Tarih hücresinin değerinin kontrolü için null kontrolü de ekledik çünkü veritabanında null değer alabiliyor.
                if (row.Cells["etkinlik_tarih"].Value != null && row.Cells["etkinlik_tarih"].Value != DBNull.Value)
                {
                    DateTime etkinlikTarih = (DateTime)row.Cells["etkinlik_tarih"].Value;
                    if (etkinlikTarih >= dateTimePicker1.MinDate && etkinlikTarih <= dateTimePicker1.MaxDate)
                    {
                        dateTimePicker1.Value = etkinlikTarih;
                    }
                    else
                    {
                        // Eğer etkinlik tarihi, dateTimePicker1'ın MinDate ve MaxDate aralığı dışındaysa,
                        // dateTimePicker1 ın  MinDate veya MaxDate değerine yakın bir tarih ata.
                        dateTimePicker1.Value = etkinlikTarih < dateTimePicker1.MinDate ? dateTimePicker1.MinDate : dateTimePicker1.MaxDate;
                    }
                }


            }
        }

        public void Temizle()
        {
            labelEtkinlikID.Text = "";
            textBoxEtkinlikAd.Text = "";
            textBoxEtkinlikAciklama.Text = "";
            comboBoxTopluluk.Text = "";
            dateTimePicker1.Value = DateTime.Now;   
        }

        public void verileriGoster()
        {
            try
            {
                string sql = "SELECT t.topluluk_ID, t.topluluk_ad, e.etkinlik_ad, e.etkinlik_ID , e.etkinlik_tarih, e.etkinlik_aciklama " +
                    "FROM ot_topluluklar AS t  " +
                    "INNER JOIN ot_etkinlik AS e ON t.topluluk_ID = e.topluluk_ID";

                var sonuc = baglanti.sql_sonuc_al(sql);
                DataTable dt = new DataTable();
                dt.Load(sonuc);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
        
    }

        private void FormEtkinlikIslemleri_Load(object sender, EventArgs e)
        {
            verileriGoster();
            string sql2 = "select * from ot_topluluklar";
            var sonuc2 = baglanti.sql_sonuc_al(sql2);
            DataTable dt2 = new DataTable();
            dt2.Load(sonuc2);
            comboBoxTopluluk.ValueMember = "topluluk_ID";
            comboBoxTopluluk.DisplayMember = "topluluk_ad";
            comboBoxTopluluk.DataSource = dt2;
            comboBoxTopluluk.SelectedIndex = -1;
        }

        private void buttonToplulukİslemGit_Click(object sender, EventArgs e)
        {
            FormToplulukIslemleri kullaniciIslemleri = new FormToplulukIslemleri();
            kullaniciIslemleri.Show();
            this.Hide();
        }

        public int toplulukIDGetir(string toplulukAdi)
        {
            int toplulukID = -1;
            toplulukAdi = comboBoxTopluluk.Text;

            string sql = "Select topluluk_ID from ot_topluluklar where topluluk_ad='" + toplulukAdi + "'";
            var sonuc = baglanti.sql_gonder(sql);
            if (sonuc != null)
            {
                toplulukID = Convert.ToInt32(comboBoxTopluluk.SelectedValue);
            }
            return toplulukID;
        }

        public void InsertEtkinlik(string etkinlikAd, int toplulukID, DateTime secilenTarih, string etkinlikAciklama)
        {
            string sql = "INSERT INTO ot_etkinlik (etkinlik_ad, etkinlik_tarih, etkinlik_aciklama, topluluk_ID) " +
                  "VALUES (@etkinlikAd, @secilenTarih, @etkinlikAciklama, @toplulukID)";

            using (SqlConnection connection = new SqlConnection("Data Source=MONSTER\\SQLEXPRESS;Initial Catalog=22420210003_ogrenci_topluluk;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@etkinlikAd", etkinlikAd);
                command.Parameters.AddWithValue("@secilenTarih", secilenTarih);
                command.Parameters.AddWithValue("@etkinlikAciklama", etkinlikAciklama);
                command.Parameters.AddWithValue("@toplulukID", toplulukID);
                command.ExecuteNonQuery();
            }
        }

        private void buttonEtkinlikEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string etkinlikAd = textBoxEtkinlikAd.Text;
                DateTime secilenTarih = dateTimePicker1.Value;
                string etkinlikAciklama = textBoxEtkinlikAciklama.Text;
                string toplulukAdi = comboBoxTopluluk.Text;
                int toplulukID = toplulukIDGetir(toplulukAdi);

                if (toplulukID != -1)
                {
                    InsertEtkinlik(etkinlikAd, toplulukID, secilenTarih, etkinlikAciklama);
                    MessageBox.Show("Etkinlik başarıyla eklendi");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            verileriGoster();
            Temizle();
        }

        private void buttonEtkinlikSil_Click(object sender, EventArgs e)
        {
            if (labelEtkinlikID.Text == "-" || labelEtkinlikID.Text == "")
            {
                MessageBox.Show("Lütfen silinecek etkinliği listeden seçin");
            }
            try
            {
                string sql = "delete from ot_etkinlik where etkinlik_ID='" + labelEtkinlikID.Text + "'";
                var sonuc = baglanti.sql_gonder(sql);
                MessageBox.Show("Etkinlik başarıyla silindi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            verileriGoster();
            Temizle();
        }

        private void buttonEtkinlikGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "UPDATE ot_etkinlik " +
             "SET ot_etkinlik.etkinlik_ad = '" + textBoxEtkinlikAd.Text + "', " +
             "ot_etkinlik.etkinlik_aciklama = '" + textBoxEtkinlikAciklama.Text + "', " +
             "ot_etkinlik.topluluk_ID = (SELECT topluluk_ID FROM ot_topluluklar WHERE topluluk_ad = '" + comboBoxTopluluk.Text + "') " +
             "WHERE ot_etkinlik.etkinlik_ID = '" + labelEtkinlikID.Text + "'";
                baglanti.sql_gonder(sql);
                MessageBox.Show("Etkinlik başarıyla güncellendi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            verileriGoster();
            Temizle();
        }

        private void buttonEtkinlikSorgula_Click(object sender, EventArgs e)
        {
            string sql_where = "1=1";
            try
            {
                if (textBoxEtkinlikAd.Text.Length > 0)
                {
                    sql_where += " AND e.etkinlik_ad LIKE '" + textBoxEtkinlikAd.Text + "%'";
                }
                if (comboBoxTopluluk.Text.Length > 0)
                {
                    sql_where += " AND t.topluluk_ad LIKE '" + comboBoxTopluluk.Text + "%'";
                }

                string sql = "SELECT e.etkinlik_ID, e.etkinlik_ad, e.etkinlik_tarih, e.etkinlik_aciklama, t.topluluk_ad " +
                             "FROM ot_etkinlik AS e " +
                             "INNER JOIN ot_topluluklar AS t ON e.topluluk_ID = t.topluluk_ID " +
                             "WHERE " + sql_where;

                var sonuc = baglanti.sql_sonuc_al(sql);
                DataTable dt = new DataTable();
                dt.Load(sonuc);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            Temizle();
        }


        private void buttonKisiİslem_Click(object sender, EventArgs e)
        {
           FormKisiİslemleri formKisiİslemleri = new FormKisiİslemleri();
            formKisiİslemleri.Show();
            this.Hide();    
        }

        private void buttonGoster_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public static readonly string DEST = "pdf/etkinlik_table.pdf";
        private void buttonPdfEtkinlik_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FormEtkinlikIslemleri().ManipulatePdfEtkinlik(DEST);
            MessageBox.Show("PDF başarıyla oluşturuldu");
        }

        private void ManipulatePdfEtkinlik(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Etkinlik Listesi"));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 20, 20, 20, 10, 10, 10 }));
            table.SetMarginTop(5);
            table.AddCell("Etkinlik ID");
            table.AddCell("Topluluk Adı");
            table.AddCell("Fakülte Adı");
            table.AddCell("Etkinlik Adı");
            table.AddCell("Etkinlik Tarihi");
            table.AddCell("Etkinlik Saati");
            table.AddCell("Açıklama");

            string sql = "SELECT e.etkinlik_ID, t.topluluk_ad, f.fakulte_ad, e.etkinlik_ad, e.etkinlik_tarih, e.etkinlik_saat, e.etkinlik_aciklama " +
                         "FROM ot_etkinlik AS e " +
                         "INNER JOIN ot_topluluklar AS t ON e.topluluk_ID = t.topluluk_ID " +
                         "INNER JOIN ot_fakulte AS f ON t.fakulte_ID = f.fakulte_ID";

            var sonuc = baglanti.sql_sonuc_al(sql);

            while (sonuc.Read())
            {
                table.AddCell(sonuc["etkinlik_ID"].ToString());         
                table.AddCell(sonuc["topluluk_ad"].ToString());        
                table.AddCell(sonuc["fakulte_ad"].ToString());         
                table.AddCell(sonuc["etkinlik_ad"].ToString());        
                table.AddCell(sonuc["etkinlik_tarih"].ToString());     
                table.AddCell(sonuc["etkinlik_saat"].ToString());      
                table.AddCell(sonuc["etkinlik_aciklama"].ToString());  
            }

            doc.Add(table);
            doc.Close();
        }

        private void buttonTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}
