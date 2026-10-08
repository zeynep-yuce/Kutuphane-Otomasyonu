using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace K.Otomasyon
{
    public partial class FormAdminGiris : Form
    {


        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["KutuphaneDB"].ConnectionString);
        public FormAdminGiris()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text.Trim();
            string sifre = textBox2.Text;

            SqlConnection baglanti = new SqlConnection("Server=.;Database=KutuphaneDB;Integrated Security=True");
            baglanti.Open();

            SqlCommand komut = new SqlCommand("SELECT * FROM Kullanicilar WHERE KullaniciAdi=@kullanici AND Sifre=@sifre", baglanti);
            komut.Parameters.AddWithValue("@kullanici", kullaniciAdi);
            komut.Parameters.AddWithValue("@sifre", sifre);

            SqlDataReader reader = komut.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show("Giriş başarılı!");

                // Ana sayfa formuna geçiş
                this.Hide();
                Form1 frm = new Form1(); // varsa yetkiyi parametre olarak da gönderebilirsin
                frm.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
            }

            baglanti.Close();
        }

        private void FormAdminGiris_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
    
}
