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
using System.Xml.Linq;

namespace vize_1
{
    public partial class FormKisiİslemleri : Form
    {
        Veritabani baglanti = new Veritabani();
        public FormKisiİslemleri()
        {
            InitializeComponent();
        }

        private void verileriGoster()
        {
            try
            {
                string sql = "select k.kisi_ID, k.kisi_ad, k.kisi_soyad, k.kisi_email, r.rol_ad, kr.baslangic_tarih, kr.bitis_tarih, t.topluluk_ad from ot_kisiler as k " +
                             "inner join ot_kisi_rol as kr ON k.kisi_ID=kr.kisi_ID inner join ot_roller as r ON r.rol_ID=kr.rol_ID" +
                             " inner join ot_topluluklar as t ON kr.topluluk_ID=t.topluluk_ID";

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

        public void Temizle()
        {
            labelKisiID.Text = "";
            textBoxAd.Text = "";
            textBoxEmail.Text = "";
            textBoxSoyad.Text = "";
            comboBoxKisiTopluluk.Text = "";
            comboBoxRolGetir.Text = "";
            dateTimePickerBaslangic.Value = DateTime.Now;
            dateTimePickerBitis.Value = DateTime.Now;
        }

        private void FormKisiİslemleri_Load(object sender, EventArgs e)
        {
            verileriGoster();
            string sql = "select * from ot_roller";
            var sonuc = baglanti.sql_sonuc_al(sql);

            DataTable dt = new DataTable();
            dt.Load(sonuc);
            comboBoxRolGetir.ValueMember = "rol_ID";
            comboBoxRolGetir.DisplayMember = "rol_ad";
            comboBoxRolGetir.DataSource = dt;

            string sql2 = "select * from ot_topluluklar";
            var sonuc2 = baglanti.sql_sonuc_al(sql2);
            DataTable dt2 = new DataTable();
            dt2.Load(sonuc2);
            comboBoxKisiTopluluk.ValueMember = "topluluk_ID";
            comboBoxKisiTopluluk.DisplayMember = "topluluk_ad";
            comboBoxKisiTopluluk.DataSource = dt2;

            comboBoxKisiTopluluk.SelectedIndex = -1;
            comboBoxRolGetir.SelectedIndex = -1;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    row = dataGridView1.Rows[e.RowIndex];
                    labelKisiID.Text = row.Cells["kisi_ID"].Value.ToString();
                    textBoxAd.Text = row.Cells["kisi_ad"].Value.ToString();
                    textBoxSoyad.Text = row.Cells["kisi_soyad"].Value.ToString();
                    textBoxEmail.Text = row.Cells["kisi_email"].Value.ToString();
                    comboBoxKisiTopluluk.Text = row.Cells["topluluk_ad"].Value.ToString();
                    comboBoxRolGetir.Text = row.Cells["rol_ad"].Value.ToString();

                }

                // Başlangıç tarihi
                if (row.Cells["baslangic_tarih"].Value != null && row.Cells["baslangic_tarih"].Value != DBNull.Value)
                {
                    DateTime baslangicTarih = (DateTime)row.Cells["baslangic_tarih"].Value;
                    if (baslangicTarih >= dateTimePickerBaslangic.MinDate && baslangicTarih <= dateTimePickerBaslangic.MaxDate)
                    {
                        dateTimePickerBaslangic.Value = baslangicTarih;
                    }
                    else
                    {
                        dateTimePickerBaslangic.Value = baslangicTarih < dateTimePickerBaslangic.MinDate ? dateTimePickerBaslangic.MinDate : dateTimePickerBaslangic.MaxDate;
                    }
                }

                // Bitiş tarihi
                if (row.Cells["bitis_tarih"].Value != null && row.Cells["bitis_tarih"].Value != DBNull.Value)
                {
                    DateTime bitisTarih = (DateTime)row.Cells["bitis_tarih"].Value;
                    if (bitisTarih >= dateTimePickerBitis.MinDate && bitisTarih <= dateTimePickerBitis.MaxDate)
                    {
                        dateTimePickerBitis.Value = bitisTarih;
                    }
                    else
                    {
                        dateTimePickerBitis.Value = bitisTarih < dateTimePickerBitis.MinDate ? dateTimePickerBitis.MinDate : dateTimePickerBitis.MaxDate;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void buttonKisiEkle_Click(object sender, EventArgs e)
        {
            string kisiAdi = textBoxAd.Text;
            string soyadi = textBoxSoyad.Text;
            string email = textBoxEmail.Text;
            string rolAdi = comboBoxRolGetir.Text;
            string toplulukAdi = comboBoxKisiTopluluk.Text;
            DateTime baslangicTarih = dateTimePickerBaslangic.Value;
            DateTime bitisTarih = dateTimePickerBitis.Value;

            InsertData(kisiAdi, soyadi, email, rolAdi, toplulukAdi, baslangicTarih, bitisTarih);
        }

        private void InsertData(string kisiAdi, string soyadi, string email, string rolAdi, string toplulukAdi, DateTime baslangicTarih, DateTime bitisTarih)
        {
            SqlConnection connection = new SqlConnection("Data Source=MONSTER\\SQLEXPRESS;Initial Catalog=22420210003_ogrenci_topluluk;Integrated Security=True");
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // geçici tablo
                    string createTempTableQuery = "CREATE TABLE #TempKisiID (kisi_ID INT)";
                    SqlCommand createTempTableCmd = new SqlCommand(createTempTableQuery, connection, transaction);
                    createTempTableCmd.ExecuteNonQuery();

                    // ot_kisiler tablosuna veri ekleme ve yeni kisi_ID yi geçici tabloya yazma
                    string insertKisiQuery = @"
                        INSERT INTO ot_kisiler (kisi_ad, kisi_soyad, kisi_email)
                        OUTPUT INSERTED.kisi_ID INTO #TempKisiID
                        VALUES (@kisi_adi, @soyadi, @Email)";
                    SqlCommand insertKisiCmd = new SqlCommand(insertKisiQuery, connection, transaction);
                    insertKisiCmd.Parameters.AddWithValue("@kisi_adi", kisiAdi);
                    insertKisiCmd.Parameters.AddWithValue("@soyadi", soyadi);
                    insertKisiCmd.Parameters.AddWithValue("@Email", email);
                    insertKisiCmd.ExecuteNonQuery();

                    // Geçici tablodan yeni kisi_ID yi alma
                    string selectKisiIdQuery = "SELECT kisi_ID FROM #TempKisiID";
                    SqlCommand selectKisiIdCmd = new SqlCommand(selectKisiIdQuery, connection, transaction);
                    int kisiId = (int)selectKisiIdCmd.ExecuteScalar();

                    // rol_id yi ot_roller tablosundan alma
                    string selectRolQuery = "SELECT rol_ID FROM ot_roller WHERE rol_ad = @rol_adi";
                    SqlCommand selectRolCmd = new SqlCommand(selectRolQuery, connection, transaction);
                    selectRolCmd.Parameters.AddWithValue("@rol_adi", rolAdi);
                    object rolIdObj = selectRolCmd.ExecuteScalar();
                    if (rolIdObj == null)
                    {
                        throw new Exception("Belirtilen rol adına sahip bir kayıt bulunamadı.");
                    }
                    int rolId = (int)rolIdObj;

                    // topluluk_id'yi ot_topluluklar tablosundan alma
                    string selectToplulukQuery = "SELECT topluluk_ID FROM ot_topluluklar WHERE topluluk_ad = @topluluk_adi";
                    SqlCommand selectToplulukCmd = new SqlCommand(selectToplulukQuery, connection, transaction);
                    selectToplulukCmd.Parameters.AddWithValue("@topluluk_adi", toplulukAdi);
                    object toplulukIdObj = selectToplulukCmd.ExecuteScalar();
                    if (toplulukIdObj == null)
                    {
                        throw new Exception("Belirtilen topluluk adına sahip bir kayıt bulunamadı.");
                    }
                    int toplulukId = (int)toplulukIdObj;

                    //  ot_kisi_rol tablosu veri ekleme
                    string insertKisiRolQuery = @"
                        INSERT INTO ot_kisi_rol (kisi_ID, rol_ID, topluluk_ID, baslangic_tarih, bitis_tarih)
                        VALUES (@kisi_ID, @rol_ID, @topluluk_ID, @baslangic_tarih, @bitis_tarih)";
                    SqlCommand insertKisiRolCmd = new SqlCommand(insertKisiRolQuery, connection, transaction);
                    insertKisiRolCmd.Parameters.AddWithValue("@kisi_ID", kisiId);
                    insertKisiRolCmd.Parameters.AddWithValue("@rol_ID", rolId);
                    insertKisiRolCmd.Parameters.AddWithValue("@topluluk_ID", toplulukId);
                    insertKisiRolCmd.Parameters.AddWithValue("@baslangic_tarih", baslangicTarih);
                    insertKisiRolCmd.Parameters.AddWithValue("@bitis_tarih", bitisTarih);
                    insertKisiRolCmd.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show("Kişi başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Hata durumunda değişiklikleri geri alın
                    transaction.Rollback();
                    MessageBox.Show("Veritabanı işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                verileriGoster();
                Temizle();
            }
        }

        private void buttonKisiSil_Click(object sender, EventArgs e)
        {
            if (labelKisiID.Text == "-" || labelKisiID.Text == "")
            {
                MessageBox.Show("Lütfen silinecek kişiyi listeden seçin");
            }
            SqlConnection connection = new SqlConnection("Data Source=MONSTER\\SQLEXPRESS;Initial Catalog=22420210003_ogrenci_topluluk;Integrated Security=True");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // ot_kisi_rol tablosundan  kişinin rolleri silme
                string deleteKisiRolQuery = "DELETE FROM ot_kisi_rol WHERE kisi_ID = @kisi_ID";
                SqlCommand deleteKisiRolCmd = new SqlCommand(deleteKisiRolQuery, connection, transaction);
                deleteKisiRolCmd.Parameters.AddWithValue("@kisi_ID", Convert.ToInt32(labelKisiID.Text));
                deleteKisiRolCmd.ExecuteNonQuery();

                // ot_kisiler tablosundan ilgili kişi silme
                string deleteKisiQuery = "DELETE FROM ot_kisiler WHERE kisi_ID = @kisi_ID";
                SqlCommand deleteKisiCmd = new SqlCommand(deleteKisiQuery, connection, transaction);
                deleteKisiCmd.Parameters.AddWithValue("@kisi_ID", Convert.ToInt32(labelKisiID.Text));
                deleteKisiCmd.ExecuteNonQuery();
                transaction.Commit();
                MessageBox.Show("Kişi başarıyla silindi");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Veritabanı işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            verileriGoster();
            Temizle();
        }

        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(labelKisiID.Text))
            {
                MessageBox.Show("Lütfen güncellemek için bir kişi seçin.");
                return;
            }

            try
            {
                string sql = "UPDATE ot_kisiler " +
                    "SET kisi_ad = '" + textBoxAd.Text + "', " +
                    "kisi_soyad = '" + textBoxSoyad.Text + "'," +
                    "kisi_email = '" + textBoxEmail.Text + "' " +
                    " WHERE kisi_ID = '" + labelKisiID.Text + "';" +
                    "" +
                    "UPDATE ot_kisi_rol " +
                    "SET rol_ID = (SELECT rol_ID FROM ot_roller WHERE rol_ad = '" + comboBoxRolGetir.Text + "'), " +
                    "   topluluk_ID = (SELECT topluluk_ID FROM ot_topluluklar WHERE topluluk_ad = '" + comboBoxKisiTopluluk.Text + "')," +
                    "   baslangic_tarih='"+dateTimePickerBaslangic.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "   bitis_tarih='"+dateTimePickerBitis.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    "WHERE kisi_ID = '" + labelKisiID.Text + "' ";

                baglanti.sql_gonder(sql);
                MessageBox.Show("Kişi başarıyla güncellendi!");
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

        private void buttonFormToplulukGit_Click(object sender, EventArgs e)
        {
            FormToplulukIslemleri formKullaniciIslemleri = new FormToplulukIslemleri();
            formKullaniciIslemleri.Show();
            this.Hide();
        }

        private void buttonbuttonFormEtkinlikGit_Click(object sender, EventArgs e)
        {
            FormEtkinlikIslemleri formEtkinlikIslemleri = new FormEtkinlikIslemleri();
            formEtkinlikIslemleri.Show();
            this.Hide();
        }

        private void buttonKisiSorgula_Click(object sender, EventArgs e)
        {
            string sql_where = "1=1"; // Başlangıçta tüm satırları seçmek için geçerli bir koşul
            try
            {
                // SQL sorgusunu oluştur
                string sql = "SELECT k.kisi_ID, k.kisi_email, k.kisi_ad, k.kisi_soyad, kr.baslangic_tarih, kr.bitis_tarih, r.rol_ad, t.topluluk_ad " +
                             "FROM ot_kisiler AS k " +
                             "INNER JOIN ot_kisi_rol AS kr ON k.kisi_ID = kr.kisi_ID " +
                             "INNER JOIN ot_roller AS r ON kr.rol_ID = r.rol_ID " +
                             "INNER JOIN ot_topluluklar AS t ON kr.topluluk_ID = t.topluluk_ID " +
                             "WHERE " + sql_where;

                // Koşulları ekle
                if (textBoxAd.Text.Length > 0)
                {
                    sql_where += " AND k.kisi_ad LIKE @kisiAd";
                }
                if (textBoxSoyad.Text.Length > 0)
                {
                    sql_where += " AND k.kisi_soyad LIKE @kisiSoyad";
                }
                if (textBoxEmail.Text.Length > 0)
                {
                    sql_where += " AND k.kisi_email LIKE @kisiEmail";
                }
                if (comboBoxRolGetir.Text.Length > 0)
                {
                    sql_where += " AND r.rol_ad LIKE @rolAd";
                }
                if (comboBoxKisiTopluluk.Text.Length > 0)
                {
                    sql_where += " AND t.topluluk_ad LIKE @toplulukAd";
                }

                // Tarihleri kontrol et ve sorguya ekle
                bool hasBaslangicTarih = dateTimePickerBaslangic.Value.Date != DateTime.Now.Date;
                bool hasBitisTarih = dateTimePickerBitis.Value.Date != DateTime.Now.Date;

                if (hasBaslangicTarih)
                {
                    sql_where += " AND kr.baslangic_tarih >= @baslangicTarih";
                }

                if (hasBitisTarih)
                {
                    sql_where += " AND kr.bitis_tarih <= @bitisTarih";
                }

                // Sorguyu güncellenmiş sql_where ile tamamla
                sql = sql.Replace("1=1", sql_where);

                using (SqlConnection connection = new SqlConnection("Data Source=MONSTER\\SQLEXPRESS;Initial Catalog=22420210003_ogrenci_topluluk;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Parametreleri ekle
                        if (textBoxAd.Text.Length > 0)
                        {
                            command.Parameters.AddWithValue("@kisiAd", textBoxAd.Text + "%");
                        }
                        if (textBoxSoyad.Text.Length > 0)
                        {
                            command.Parameters.AddWithValue("@kisiSoyad", textBoxSoyad.Text + "%");
                        }
                        if (textBoxEmail.Text.Length > 0)
                        {
                            command.Parameters.AddWithValue("@kisiEmail", textBoxEmail.Text + "%");
                        }
                        if (comboBoxRolGetir.Text.Length > 0)
                        {
                            command.Parameters.AddWithValue("@rolAd", comboBoxRolGetir.Text + "%");
                        }
                        if (comboBoxKisiTopluluk.Text.Length > 0)
                        {
                            command.Parameters.AddWithValue("@toplulukAd", comboBoxKisiTopluluk.Text + "%");
                        }
                        if (hasBaslangicTarih)
                        {
                            command.Parameters.Add("@baslangicTarih", SqlDbType.Date).Value = dateTimePickerBaslangic.Value.Date;
                        }
                        if (hasBitisTarih)
                        {
                            command.Parameters.Add("@bitisTarih", SqlDbType.Date).Value = dateTimePickerBitis.Value.Date;
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dt;
                        }
                    }
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

        public static readonly string DEST = "pdf/kisi_table.pdf";
        private void buttonPdfKisi_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FormKisiİslemleri().ManipulatePdfKisi(DEST);
            MessageBox.Show("PDF başarıyla oluşturuldu");
        }

        private void ManipulatePdfKisi(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Kişi Listesi"));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 20, 20, 20, 10, 10, 10 }));
            table.SetMarginTop(5);
            table.AddCell("Kişi ID");
            table.AddCell("Ad");
            table.AddCell("Soyad");
            table.AddCell("E-mail");
            table.AddCell("Rol");
            table.AddCell("Başlangıç Tarihi");
            table.AddCell("Bitiş Tarihi");

            string sql = "select k.kisi_ID, k.kisi_ad, k.kisi_soyad, k.kisi_email, r.rol_ad, kr.baslangic_tarih, kr.bitis_tarih " +
                         "from ot_kisiler as k " +
                         "inner join ot_kisi_rol as kr ON k.kisi_ID = kr.kisi_ID " +
                         "inner join ot_roller as r ON r.rol_ID = kr.rol_ID";

            var sonuc = baglanti.sql_sonuc_al(sql);

            while (sonuc.Read())
            {
                table.AddCell(sonuc["kisi_ID"].ToString());         
                table.AddCell(sonuc["kisi_ad"].ToString());         
                table.AddCell(sonuc["kisi_soyad"].ToString());      
                table.AddCell(sonuc["kisi_email"].ToString());      
                table.AddCell(sonuc["rol_ad"].ToString());          
                table.AddCell(sonuc["baslangic_tarih"].ToString()); 
                table.AddCell(sonuc["bitis_tarih"].ToString());     
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
