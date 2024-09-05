using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace vize_1
{
    internal class Veritabani
    {
        SqlConnection baglanti;


        public void baglan()
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = "Data Source=MONSTER\\SQLEXPRESS;Initial Catalog=22420210003_ogrenci_topluluk;Integrated Security=True";
            baglanti.Open();
        }

        public void kapat()
        {
            baglanti.Close();
        }

        public bool sql_gonder(string sql) 
        {
            baglan();
            SqlCommand komut = new SqlCommand(sql, baglanti);
            komut.ExecuteNonQuery();
            kapat();
            return true;
        }

        public SqlDataReader sql_sonuc_al(string sql)
        {
            baglan();
            SqlCommand komut = new SqlCommand(sql, baglanti);
            var sonuc = komut.ExecuteReader();
            return sonuc;
        }
    }
}