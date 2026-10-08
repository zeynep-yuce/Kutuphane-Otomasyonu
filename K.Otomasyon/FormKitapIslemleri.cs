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
using System.Configuration;

namespace K.Otomasyon
{
    public partial class FormKitapIslemleri : Form
    {
        int secilenKitapID = 0;

        SqlConnection connection = new SqlConnection( ConfigurationManager.ConnectionStrings["KutuphaneDB"].ConnectionString);

        public FormKitapIslemleri()
        {
            InitializeComponent();
        }

        void KitaplariListele()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kitaplar", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void Temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        // KİTAP EKLE
        private void button1_Click(object sender, EventArgs e)
        {
            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            // Sayfa kontrolü
            if (!int.TryParse(textBox4.Text, out int sayfa))
            {
                MessageBox.Show("Sayfa sayısı sadece sayı olmalıdır.");
                return;
            }

            // Stok kontrolü
            if (!int.TryParse(textBox6.Text, out int stok))
            {
                MessageBox.Show("Stok miktarı sadece sayı olmalıdır.");
                return;
            }

            connection.Open();

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Kitaplar (KitapAdi, Yazar, Yayinevi, Sayfa, Tur, Stok) " +
                "VALUES (@KitapAdi, @Yazar, @Yayinevi, @Sayfa, @Tur, @Stok)",
                connection);

            cmd.Parameters.AddWithValue("@KitapAdi", textBox1.Text);
            cmd.Parameters.AddWithValue("@Yazar", textBox2.Text);
            cmd.Parameters.AddWithValue("@Yayinevi", textBox3.Text);
            cmd.Parameters.AddWithValue("@Sayfa", sayfa);
            cmd.Parameters.AddWithValue("@Tur", textBox5.Text);
            cmd.Parameters.AddWithValue("@Stok", stok);

            cmd.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Kitap başarıyla eklendi.");
            KitaplariListele();
            Temizle();
        }

        // KİTAP SEÇ
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                secilenKitapID = Convert.ToInt32(row.Cells["KitapID"].Value);

                textBox1.Text = row.Cells["KitapAdi"].Value.ToString();
                textBox2.Text = row.Cells["Yazar"].Value.ToString();
                textBox3.Text = row.Cells["Yayinevi"].Value.ToString();
                textBox4.Text = row.Cells["Sayfa"].Value.ToString();
                textBox5.Text = row.Cells["Tur"].Value.ToString();
                textBox6.Text = row.Cells["Stok"].Value.ToString();
            }
        }

        // KİTAP GÜNCELLE
        private void button2_Click(object sender, EventArgs e)
        {
            if (secilenKitapID != 0)
            {
                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text) ||
                    string.IsNullOrWhiteSpace(textBox5.Text) ||
                    string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                    return;
                }

                // Sayfa kontrolü
                if (!int.TryParse(textBox4.Text, out int sayfa))
                {
                    MessageBox.Show("Sayfa sayısı sadece sayı olmalıdır.");
                    return;
                }

                // Stok kontrolü
                if (!int.TryParse(textBox6.Text, out int stok))
                {
                    MessageBox.Show("Stok miktarı sadece sayı olmalıdır.");
                    return;
                }

                connection.Open();

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Kitaplar SET KitapAdi=@KitapAdi, Yazar=@Yazar, Yayinevi=@Yayinevi, Sayfa=@Sayfa, Tur=@Tur, Stok=@Stok WHERE KitapID=@KitapID",
                    connection);

                cmd.Parameters.AddWithValue("@KitapAdi", textBox1.Text);
                cmd.Parameters.AddWithValue("@Yazar", textBox2.Text);
                cmd.Parameters.AddWithValue("@Yayinevi", textBox3.Text);
                cmd.Parameters.AddWithValue("@Sayfa", sayfa);
                cmd.Parameters.AddWithValue("@Tur", textBox5.Text);
                cmd.Parameters.AddWithValue("@Stok", stok);
                cmd.Parameters.AddWithValue("@KitapID", secilenKitapID);

                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Kitap bilgileri güncellendi.");
                KitaplariListele();
                Temizle();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek bir kitap seçin.");
            }
        }

        // KİTAP SİL
        private void button3_Click(object sender, EventArgs e)
        {
            if (secilenKitapID != 0)
            {
                DialogResult result = MessageBox.Show(
                    "Bu kitabı silmek istediğinize emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Kitaplar WHERE KitapID=@KitapID",
                        connection);

                    cmd.Parameters.AddWithValue("@KitapID", secilenKitapID);
                    cmd.ExecuteNonQuery();

                    connection.Close();

                    MessageBox.Show("Kitap silindi.");
                    KitaplariListele();
                    Temizle();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir kitap seçin.");
            }
        }

        private void FormKitapIslemleri_Load(object sender, EventArgs e)
        {
            KitaplariListele();
        }
    }
}