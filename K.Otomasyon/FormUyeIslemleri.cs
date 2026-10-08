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
    public partial class FormUyeIslemleri : Form
    {
        int secilenUyeID = 0;


        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["KutuphaneDB"].ConnectionString);


        public FormUyeIslemleri()
        {
            InitializeComponent();
        }
        void UyeleriListele()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Uyeler", connection);
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
            secilenUyeID = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }
            if (textBox2.Text.Length != 11 || !textBox2.Text.All(char.IsDigit))
            {
                MessageBox.Show("TC kimlik numarası 11 haneli ve sadece rakamlardan oluşmalıdır.");
                return;
            }
            if ((textBox3.Text.Length != 10 && textBox3.Text.Length != 11) || !textBox3.Text.All(char.IsDigit))
            {
                MessageBox.Show("Telefon numarası 10 veya 11 haneli ve sadece rakamlardan oluşmalıdır.");
                return;
            }

            connection.Open();


            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Uyeler (AdSoyad, TC, Telefon, Eposta, Adres) " +
                "VALUES (@AdSoyad, @TC, @Telefon, @Eposta, @Adres)",
                connection);

            cmd.Parameters.AddWithValue("@AdSoyad", textBox1.Text);
            cmd.Parameters.AddWithValue("@TC", textBox2.Text);
            cmd.Parameters.AddWithValue("@Telefon", textBox3.Text);
            cmd.Parameters.AddWithValue("@Eposta", textBox4.Text);
            cmd.Parameters.AddWithValue("@Adres", textBox5.Text);

            cmd.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Üye başarıyla eklendi.");
            UyeleriListele();
        }

        private void FormUyeIslemleri_Load(object sender, EventArgs e)
        {
            UyeleriListele();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                secilenUyeID = Convert.ToInt32(row.Cells["UyeID"].Value);

                textBox1.Text = row.Cells["AdSoyad"].Value.ToString();
                textBox2.Text = row.Cells["TC"].Value.ToString();
                textBox3.Text = row.Cells["Telefon"].Value.ToString();
                textBox4.Text = row.Cells["Eposta"].Value.ToString();
                textBox5.Text = row.Cells["Adres"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (secilenUyeID != 0)
            {
                if(string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text) ||
                    string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                    return;
                }
                if (textBox2.Text.Length != 11 || !textBox2.Text.All(char.IsDigit))
                {
                    MessageBox.Show("TC kimlik numarası 11 haneli ve sadece rakamlardan oluşmalıdır.");
                    return;
                }
                if ((textBox3.Text.Length != 10 && textBox3.Text.Length != 11) ||!textBox3.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Telefon numarası 10 veya 11 haneli ve sadece rakamlardan oluşmalıdır.");
                    return;
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Uyeler SET AdSoyad=@AdSoyad, TC=@TC, Telefon=@Telefon, Eposta=@Eposta, Adres=@Adres WHERE UyeID=@UyeID", connection);
                cmd.Parameters.AddWithValue("@AdSoyad", textBox1.Text);
                cmd.Parameters.AddWithValue("@TC", textBox2.Text);
                cmd.Parameters.AddWithValue("@Telefon", textBox3.Text);
                cmd.Parameters.AddWithValue("@Eposta", textBox4.Text);
                cmd.Parameters.AddWithValue("@Adres", textBox5.Text);
                cmd.Parameters.AddWithValue("@UyeID", secilenUyeID);
                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Üye bilgileri güncellendi.");
                UyeleriListele();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek bir üye seçin.");
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (secilenUyeID != 0)
            {
                DialogResult cevap = MessageBox.Show("Bu üyeyi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cevap == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Uyeler WHERE UyeID=@UyeID", connection);
                    cmd.Parameters.AddWithValue("@UyeID", secilenUyeID);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Üye silindi.");
                    UyeleriListele();
                    Temizle(); // TextBox'ları temizleyelim
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir üye seçin.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}

