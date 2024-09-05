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

namespace vize_1
{
    public partial class FormGiris : Form
    {
        Veritabani baglanti = new Veritabani();
        public FormGiris()
        {
            InitializeComponent();
        }

        public void Temizle()
        {
            textBoxEposta.Text = "";
            textBoxSifre.Text = "";
        }
        private void buttonKullaniciGiris_Click(object sender, EventArgs e)
        {
            try
            {
                string sifre = "";
                baglanti.baglan();
                string sql = "SELECT kisi_email, sifre, rol_ID FROM " +
                    "ot_kisiler k INNER JOIN ot_kisi_rol kr ON k.kisi_ID = kr.kisi_ID " +
                    "WHERE k.kisi_email = '"+textBoxEposta.Text+"' AND kr.sifre ='"+textBoxSifre.Text+"' AND kr.rol_ID IN(3, 4)";
                var sonuc = baglanti.sql_sonuc_al(sql);

                if(sonuc.HasRows)
                {
                    FormToplulukIslemleri formKullaniciIslemleri = new FormToplulukIslemleri();
                    formKullaniciIslemleri.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
                    Temizle();
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

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
