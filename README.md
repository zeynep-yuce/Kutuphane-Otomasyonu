#  Kütüphane Otomasyonu

C# Windows Forms ve SQL Server kullanılarak geliştirilmiş, kütüphanelerdeki **üye, kitap ve ödünç/iade işlemlerinin** yönetilmesini sağlayan masaüstü kütüphane otomasyon uygulamasıdır.

Bu proje, C# ile masaüstü uygulama geliştirme, SQL Server ile veri yönetimi ve ADO.NET kullanarak veritabanı işlemleri konusunda pratik kazanmak amacıyla geliştirilmiştir.

---

##   Özellikler

###  Üye Yönetimi

* Yeni üye ekleme
* Üye bilgilerini güncelleme
* Üye silme
* Üyeleri listeleme
* TC kimlik numarası doğrulama
* Telefon numarası doğrulama
* Zorunlu alan kontrolü

###  Kitap Yönetimi

* Yeni kitap ekleme
* Kitap bilgilerini güncelleme
* Kitap silme
* Kitapları listeleme
* Sayfa sayısı için sayısal değer kontrolü
* Stok miktarı için sayısal değer kontrolü
* Zorunlu alan kontrolü

###  Ödünç ve İade İşlemleri

* Üyeye kitap ödünç verme
* Kitap iade işlemi
* Ödünç verilen kitapların listelenmesi
* Ödünç verme sırasında stok azaltma
* İade sırasında stok artırma
* Stokta bulunmayan kitabın ödünç verilmesini engelleme
* Aynı kitabın ikinci kez iade edilmesini engelleme
* Ödünç verme işleminde SQL Transaction kullanımı

###  Kullanıcı Girişi

* Kullanıcı adı ve şifre ile giriş
* Yetkiye dayalı kullanıcı yapısı

###  Veritabanı Bağlantısı

* SQL Server kullanımı
* ADO.NET ile veritabanı işlemleri
* Connection String'in `App.config` üzerinden yönetilmesi

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji                | Kullanım Alanı                      |
| ------------------------ | ----------------------------------- |
| **C#**                   | Uygulama geliştirme                 |
| **.NET Framework 4.7.2** | Uygulama altyapısı                  |
| **Windows Forms**        | Masaüstü kullanıcı arayüzü          |
| **SQL Server**           | Veritabanı                          |
| **ADO.NET**              | Veritabanı işlemleri                |
| **SQL Transaction**      | İşlem bütünlüğünün sağlanması       |
| **Git / GitHub**         | Versiyon kontrolü ve proje yönetimi |

---

##  Veritabanı

Uygulama **SQL Server** üzerinde `KutuphaneDB` isimli veritabanını kullanmaktadır.

Temel tablolar:

* `Kullanicilar`
* `Uyeler`
* `Kitaplar`
* `OduncIslemler`

### Kitap Stok Yönetimi

Ödünç verme işleminde:

```text
Kitap Stok = Kitap Stok - 1
```

İade işleminde:

```text
Kitap Stok = Kitap Stok + 1
```

Stok miktarı `0` olan kitaplar ödünç verme listesinde gösterilmez.

Ödünç verme sırasında kitap kaydı oluşturulması ve stok azaltılması aynı transaction içerisinde gerçekleştirilerek işlemlerin birlikte tamamlanması sağlanmıştır.

---

##  Proje Yapısı

```text
K.Otomasyon
│
├── Form1.cs
├── FormAdminGiris.cs
├── FormUyeIslemleri.cs
├── FormKitapIslemleri.cs
├── FormOdunc.cs
│
├── App.config
├── Program.cs
└── K.Otomasyon.csproj
```

### Formlar

**Form1**
Uygulamanın ana menüsünü içerir.

**FormAdminGiris**
Kullanıcı giriş işlemlerini gerçekleştirir.

**FormUyeIslemleri**
Üye ekleme, güncelleme, silme ve listeleme işlemlerini yönetir.

**FormKitapIslemleri**
Kitap ekleme, güncelleme, silme ve listeleme işlemlerini yönetir.

**FormOdunc**
Kitap ödünç verme, iade etme ve stok takibi işlemlerini yönetir.

---

##  Veri Doğrulama

Uygulamada kullanıcı girişlerinin daha kontrollü alınması için çeşitli doğrulamalar bulunmaktadır.

Örneğin:

* Boş alan kontrolü
* TC kimlik numarası için 11 hane kontrolü
* Telefon numarası için uzunluk ve rakam kontrolü
* Sayfa sayısı için sayısal değer kontrolü
* Stok miktarı için sayısal değer kontrolü
* Stokta kitap bulunup bulunmadığının kontrolü
* Seçili kayıt olmadan güncelleme/silme işlemlerinin engellenmesi

---

##  Connection String

Veritabanı bağlantısı `App.config` içerisinde tanımlanmıştır:

```xml
<connectionStrings>
    <add name="KutuphaneDB"
         connectionString="Data Source=ZEYNEP;Initial Catalog=KutuphaneDB;Integrated Security=True" />
</connectionStrings>
```

Projeyi farklı bir bilgisayarda çalıştırırken `Data Source` değerinin ilgili SQL Server sunucusuna göre değiştirilmesi gerekir.

---

##  Kurulum

### 1. Projeyi klonlayın

```bash
git clone https://github.com/zeynep-yuce/Kutuphane-Otomasyonu.git
```

### 2. Projeyi Visual Studio ile açın

`K.Otomasyon.sln` dosyasını Visual Studio ile açın.

### 3. SQL Server veritabanını oluşturun

SQL Server üzerinde:

```text
KutuphaneDB
```

isimli veritabanını oluşturun ve gerekli tabloları ekleyin.

### 4. Connection String'i düzenleyin

`App.config` içerisindeki `Data Source` değerini kendi SQL Server sunucunuza göre değiştirin.

Örneğin:

```xml
Data Source=SUNUCU_ADI;
Initial Catalog=KutuphaneDB;
Integrated Security=True
```

### 5. Projeyi çalıştırın

Visual Studio üzerinden:

```text
Build → Build Solution
```

ardından uygulamayı çalıştırabilirsiniz.

---

##  Projenin Amacı

Bu proje ile aşağıdaki konularda pratik yapılmıştır:

* C# ile masaüstü uygulama geliştirme
* Windows Forms kullanımı
* SQL Server veritabanı yönetimi
* ADO.NET ile CRUD işlemleri
* SQL sorguları
* Parametreli SQL sorguları
* Formlar arası geçiş
* Kullanıcı girişi
* Veri doğrulama
* Stok yönetimi
* SQL Transaction kullanımı
* Git ve GitHub ile versiyon kontrolü

---

##  Proje Durumu

**Tamamlandı.**

Proje, C# ve SQL Server kullanarak gerçek bir iş sürecini simüle eden bir masaüstü uygulaması olarak geliştirilmiştir.

Geliştirme sürecinde özellikle **CRUD işlemleri, veri doğrulama, stok takibi, ödünç/iade yönetimi ve transaction kullanımı** üzerine çalışılmıştır.

---

##  Geliştirici

**Zeynep Yüce**

* GitHub: [zeynep-yuce](https://github.com/zeynep-yuce)

---

⭐ Projeyi faydalı bulduysanız repository'ye yıldız bırakabilirsiniz.
