using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K.Otomasyon
{
    public partial class FormOdunc : Form
    {
        int secilenIslemID = 0;
        int secilenKitapID = 0;


        SqlConnection connection = new SqlConnection( ConfigurationManager.ConnectionStrings["KutuphaneDB"].ConnectionString);
        public FormOdunc()
        {
            InitializeComponent();
        }
        private void UyeleriYukle()
        {
            SqlCommand cmd = new SqlCommand("SELECT UyeID, AdSoyad FROM Uyeler", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DisplayMember = "AdSoyad";  // Görünen
            comboBox1.ValueMember = "UyeID";      // Arkada tutulan
            comboBox1.DataSource = dt;
        }

        private void KitaplariYukle()
        {
            SqlCommand cmd = new SqlCommand("SELECT KitapID, KitapAdi FROM Kitaplar WHERE Stok > 0", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox2.DisplayMember = "KitapAdi";  // Görünen
            comboBox2.ValueMember = "KitapID";     // Arkada tutulan
            comboBox2.DataSource = dt;
        }
        private void ListeleOduncIslemleri()
        {
            string query = @"
        SELECT 
            O.IslemID,
            U.AdSoyad AS UyeAdi,
            K.KitapAdi,
            O.VerilisTarihi,
            O.IadeTarihi,
            CASE WHEN O.TeslimEdildi = 1 THEN 'Evet' ELSE 'Hayır' END AS TeslimDurumu
        FROM 
            OduncIslemler O
        INNER JOIN Uyeler U ON O.UyeID = U.UyeID
        INNER JOIN Kitaplar K ON O.KitapID = K.KitapID
        ORDER BY O.VerilisTarihi DESC";

            SqlDataAdapter da = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private int GetirKitapID(int islemID)
        {
            int kitapID = 0;
            string query = "SELECT KitapID FROM OduncIslemler WHERE IslemID = @IslemID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@IslemID", islemID);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                kitapID = Convert.ToInt32(reader["KitapID"]);
            }
            connection.Close();

            return kitapID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && comboBox2.SelectedValue != null)
            {
                int uyeID = Convert.ToInt32(comboBox1.SelectedValue);
                int kitapID = Convert.ToInt32(comboBox2.SelectedValue);
                DateTime verilisTarihi = dateTimePicker1.Value.Date;
                DateTime iadeTarihi = dateTimePicker2.Value.Date;

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                // 1. OduncIslemler tablosuna kayıt ekle
                SqlCommand cmd1 = new SqlCommand("INSERT INTO OduncIslemler (UyeID, KitapID, VerilisTarihi, IadeTarihi, TeslimEdildi) VALUES (@UyeID, @KitapID, @VerilisTarihi, @IadeTarihi, 0)",connection,
                transaction);
                cmd1.Parameters.AddWithValue("@UyeID", uyeID);
                cmd1.Parameters.AddWithValue("@KitapID", kitapID);
                cmd1.Parameters.AddWithValue("@VerilisTarihi", verilisTarihi);
                cmd1.Parameters.AddWithValue("@IadeTarihi", iadeTarihi);
                cmd1.ExecuteNonQuery();


                // 2. Kitap stok azalt
                SqlCommand cmd2 = new SqlCommand(
                    "UPDATE Kitaplar SET Stok = Stok - 1 WHERE KitapID = @KitapID AND Stok > 0",
                    connection,transaction);

                cmd2.Parameters.AddWithValue("@KitapID", kitapID);

                int etkilenenSatir = cmd2.ExecuteNonQuery();

                if (etkilenenSatir == 0)
                {
                    transaction.Rollback();
                    MessageBox.Show("Bu kitabın stoğu bulunmuyor.");
                    connection.Close();
                    return;
                }
                transaction.Commit();
                connection.Close();
                MessageBox.Show("Kitap ödünç verildi.");
                ListeleOduncIslemleri(); // Varsayılan listeleme metodu varsa çağır
                KitaplariYukle(); // ComboBox’ı yeniden doldur stok azaldığı için
            }
            else
            {
                MessageBox.Show("Lütfen bir üye ve kitap seçiniz.");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormOdunc_Load(object sender, EventArgs e)
        {
            UyeleriYukle();
            KitaplariYukle();
            ListeleOduncIslemleri();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                secilenIslemID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["IslemID"].Value);
                secilenKitapID = GetirKitapID(secilenIslemID);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (secilenIslemID != 0)
            {
                connection.Open();

                // Sadece henüz teslim edilmemiş işlemi teslim edildi olarak güncelle
                SqlCommand cmd1 = new SqlCommand(
                    "UPDATE OduncIslemler SET TeslimEdildi = 1 WHERE IslemID = @IslemID AND TeslimEdildi = 0",
                    connection);

                cmd1.Parameters.AddWithValue("@IslemID", secilenIslemID);

                int etkilenenSatir = cmd1.ExecuteNonQuery();

                if (etkilenenSatir > 0)
                {
                    // Kitap stoğunu artır
                    SqlCommand cmd2 = new SqlCommand(
                        "UPDATE Kitaplar SET Stok = Stok + 1 WHERE KitapID = @KitapID",
                        connection);

                    cmd2.Parameters.AddWithValue("@KitapID", secilenKitapID);
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Kitap iade edildi.");
                }
                else
                {
                    MessageBox.Show("Bu kitap zaten iade edilmiş.");
                }

                connection.Close();

                ListeleOduncIslemleri();
                KitaplariYukle();
            }
            else
            {
                MessageBox.Show("Lütfen iade edilecek işlemi seçin.");
            }
        }
    }
}
