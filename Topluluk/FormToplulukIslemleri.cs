using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Data.Sql;
using System.Web;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;

namespace vize_1
{
    public partial class FormToplulukIslemleri : Form
    {
        Veritabani baglanti = new Veritabani();
        public FormToplulukIslemleri()
        {
            InitializeComponent();
        }

        private void FormKullaniciIslemleri_Load(object sender, EventArgs e)
        {
            verileriGoster();

            string sql = "select * from ot_fakulte";
            var sonuc = baglanti.sql_sonuc_al(sql);

            DataTable dt = new DataTable();
            dt.Load(sonuc);
            comboBoxFakulte.ValueMember = "fakulte_ID";
            comboBoxFakulte.DisplayMember = "fakulte_ad";
            comboBoxFakulte.DataSource = dt;

        }

        public void Temizle()
        {
            labelID.Text = "";
            textBoxToplulukAd.Text = "";
            comboBoxFakulte.Text = "";
            pictureBox1.Image = null;
        }

        private void verileriGoster()
        {
            try
            {
                string sql = "select t.topluluk_ID, t.topluluk_ad, f.fakulte_ad, t.topluluk_resim from ot_topluluklar as t " +
                    "inner join ot_fakulte as f ON t.fakulte_ID=f.fakulte_ID";
                
                var sonuc = baglanti.sql_sonuc_al(sql);
                DataTable dt = new DataTable();
                dt.Load(sonuc);

                if(dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            
        }

        public int fakulteIDGetir(string fakulteAdi)
        {
            int fakulteID = -1;
            fakulteAdi = comboBoxFakulte.Text; 

            string sql = "Select fakulte_ID from ot_fakulte where fakulte_ad='" + fakulteAdi + "'";
            var sonuc = baglanti.sql_gonder(sql);
            if (sonuc != null)
            {
                fakulteID = Convert.ToInt32(comboBoxFakulte.SelectedValue);
            }
            return fakulteID;
        }

        private void buttonToplulukEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string toplulukAdi = textBoxToplulukAd.Text;
                string fakulteAdi = comboBoxFakulte.Text;
                int fakulteID = fakulteIDGetir(fakulteAdi);

                if (string.IsNullOrWhiteSpace(toplulukAdi))
                {
                    MessageBox.Show("Lütfen topluluk adını giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(fakulteAdi) || fakulteID == -1)
                {
                    MessageBox.Show("Lütfen geçerli bir fakülte seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql;

                if (!string.IsNullOrEmpty(resimYolu))
                {
                    sql = "INSERT INTO ot_topluluklar (topluluk_ad, fakulte_ID, topluluk_resim) VALUES ('" + toplulukAdi + "', " + fakulteID + ", '" + resimYolu.Replace("'", "''") + "')";
                }
                else
                {
                    sql = "INSERT INTO ot_topluluklar (topluluk_ad, fakulte_ID) VALUES ('" + toplulukAdi + "', " + fakulteID + ")";
                }

                baglanti.sql_gonder(sql);

                MessageBox.Show("Topluluk başarıyla eklendi!");
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
        }


        private void buttonToplulukSil_Click(object sender, EventArgs e)
        {
            if(labelID.Text =="-" || labelID.Text == "")
            {
                MessageBox.Show("Lütfen silinecek topluluğu listeden seçin");
            }
            try
            {
                string sql = "delete from ot_topluluklar where topluluk_ID='" + labelID.Text + "'";
                var sonuc = baglanti.sql_gonder(sql);
                MessageBox.Show("Topluluk başarıyla silindi");
            }
            catch(Exception ex)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelID.Text = row.Cells["topluluk_ID"].Value.ToString();
                textBoxToplulukAd.Text = row.Cells["topluluk_ad"].Value.ToString();
                comboBoxFakulte.Text = row.Cells["fakulte_ad"].Value.ToString();

                try
                {
                    string imagePath = row.Cells["topluluk_resim"].Value.ToString();
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenemedi: " + ex.Message);
                }
            }
        }

        private void buttonEtkinlikSayfasi_Click(object sender, EventArgs e)
        {
            FormEtkinlikIslemleri etkinlikIslemleri = new FormEtkinlikIslemleri();
            etkinlikIslemleri.Show();
            this.Hide();
        }

        private void buttonToplulukGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                string sql;

                if (!string.IsNullOrEmpty(resimYolu))
                {
                    sql = "UPDATE ot_topluluklar SET ot_topluluklar.topluluk_ad = '" + textBoxToplulukAd.Text + "', ot_topluluklar.topluluk_resim = '" + resimYolu.Replace("'", "''") + "', ot_topluluklar.fakulte_ID = (SELECT fakulte_ID FROM ot_fakulte WHERE fakulte_ad = '" + comboBoxFakulte.Text + "') WHERE ot_topluluklar.topluluk_ID = '" + labelID.Text + "'";
                }
                else
                {
                    sql = "UPDATE ot_topluluklar SET ot_topluluklar.topluluk_ad = '" + textBoxToplulukAd.Text + "', ot_topluluklar.fakulte_ID = (SELECT fakulte_ID FROM ot_fakulte WHERE fakulte_ad = '" + comboBoxFakulte.Text + "') WHERE ot_topluluklar.topluluk_ID = '" + labelID.Text + "'";
                }

                baglanti.sql_gonder(sql);

                MessageBox.Show("Topluluk başarıyla güncellendi!");
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.kapat();
            }
            Temizle();
            verileriGoster();
        }

        private void buttonToplulukAra_Click(object sender, EventArgs e)
        {
            string toplulukAd = textBoxToplulukAd.Text;
            string fakulteAd = comboBoxFakulte.Text;

            try
            {
                string sql = "select t.topluluk_ad, t.topluluk_ID, f.fakulte_ad from ot_topluluklar as t inner join ot_fakulte as f " +
                    " on f.fakulte_ID=t.fakulte_ID  where t.topluluk_ad like '" + toplulukAd + "%' AND  f.fakulte_ad like '"+ fakulteAd +"%'";
                var sonuc = baglanti.sql_sonuc_al(sql);
                baglanti.baglan();
                DataTable dt = new DataTable();
                dt.Load(sonuc);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Aranan topluluk veya fakülte bulunamadı.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
            finally { baglanti.kapat(); }
            Temizle();
        }

        private void buttonKisiGit_Click(object sender, EventArgs e)
        {
            FormKisiİslemleri kisiİslemleri = new FormKisiİslemleri();
            kisiİslemleri.Show();
            this.Hide();
        }

        private void buttonGoster_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        string resimYolu = "";
        private void buttonResimYukle_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Resim Seçme Ekranı";
            ofd.Filter = "PNG (*.png)|*.png|JPG-JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files (*.*)|*.*";
            ofd.FilterIndex = 3;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                    resimYolu = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void buttonKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        public static readonly string DEST = "pdf/topluluk_table.pdf";
        private void buttonPdfTopluluk_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FormToplulukIslemleri().ManipulatePdfTopluluk(DEST);
            MessageBox.Show("PDF başarıyla oluşturuldu");
        }
        private void ManipulatePdfTopluluk(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Topluluk Listesi"));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 30, 30, 40 }));
            table.SetMarginTop(5);
            table.AddCell("Topluluk ID");
            table.AddCell("Topluluk Adı");
            table.AddCell("Fakülte Adı");

            string sql = "select t.topluluk_ID, t.topluluk_ad, f.fakulte_ad " +
                         "from ot_topluluklar as t " +
                         "inner join ot_fakulte as f ON t.fakulte_ID = f.fakulte_ID";

            var sonuc = baglanti.sql_sonuc_al(sql);

            while (sonuc.Read())
            {
                table.AddCell(sonuc["topluluk_ID"].ToString());   
                table.AddCell(sonuc["topluluk_ad"].ToString());  
                table.AddCell(sonuc["fakulte_ad"].ToString());   
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
