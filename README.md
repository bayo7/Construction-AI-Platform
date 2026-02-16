# Construction & Real Estate AI Platform

Bu proje, modern web teknolojileri ve **N-Tier (Çok Katmanlı)** mimari kullanılarak geliştirilmiş, dinamik bir inşaat ve gayrimenkul yönetim sistemidir. Klasik CMS özelliklerinin yanı sıra, proje girişlerinde **OpenAI (Yapay Zeka)** desteği ile içerik zenginleştirme özelliklerini barındırmayı hedefler.

## 🛠 Teknoloji Yığını (Tech Stack)

Proje geliştirme sürecinde aşağıdaki teknolojiler ve kütüphaneler kullanılmıştır:

* **Backend:** ASP.NET Core 8.0 / 9.0
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core (Code First Yaklaşımı)
* **Authentication:** ASP.NET Core Identity
* **Mimari Desenler:** N-Tier Architecture, Generic Repository Pattern, Dependency Injection (DI)
* **Frontend (Admin):** Bootstrap 5, Sneat Admin Template
* **Frontend (UI):** UpConstruction Template (Entegrasyon aşamasında)

## 📂 Mimari Yapı

Proje, S.O.L.I.D. prensiplerine uygun olarak 4 ana katmana ayrılmıştır:

1.  **Entity Layer:** Veritabanı tabloları ve Identity sınıfları (AppUser).
2.  **DataAccess Layer:** Veri erişim kodları, Generic Repository ve Context yapılandırması.
3.  **Business Layer:** İş kuralları, Validasyonlar ve AI servis entegrasyonları.
4.  **Web (UI) Layer:** MVC yapısı, Controller'lar, View'lar ve Admin Paneli (Areas).

## ✅ Tamamlanan Özellikler (v1.0 - Pre-Alpha)

Şu ana kadar projenin altyapısı ve temel yönetim paneli tamamlanmıştır:

* [x] **Kurumsal Mimari:** Entity, DataAccess ve Business katmanları arasındaki soyutlama (Interface) ve somutlama (Concrete) yapıları kuruldu.
* [x] **Veritabanı Tasarımı:** Projeler, Kategoriler ve Kullanıcılar için Code-First tabloları oluşturuldu.
* [x] **Identity Entegrasyonu:** Kullanıcı kayıt, giriş ve rol yönetimi için altyapı hazırlandı.
* [x] **Admin Paneli:** "Sneat" teması projeye entegre edildi, "Areas" yapısı ile UI'dan izole edildi.
* [x] **Kategori Yönetimi:** Kategorilerin listelenmesi, eklenmesi ve düzenlenmesi için Backend kodları yazıldı.

## 🚀 Gelecek Planları (Roadmap)

Projenin bir sonraki fazında aşağıdaki özellikler eklenecektir:

* [ ] **AI Entegrasyonu:** Proje detayları girildiğinde OpenAI API kullanılarak otomatik açıklama metni ve SEO uyumlu içerik oluşturulması.
* [ ] **Frontend (Müşteri Yüzü):** Müşterilerin projeleri inceleyebileceği arayüzün (UpConstruction) giydirilmesi.
* [ ] **Dashboard Widget'ları:** Admin panelinde aktif proje sayısı, son eklenenler gibi istatistiklerin grafiklerle sunulması.
* [ ] **Medya Yönetimi:** Projeler için çoklu fotoğraf yükleme altyapısı.
* [ ] **Role Based Authorization:** Admin ve Editör yetkilerinin ayrıştırılması.

## ⚙️ Kurulum (Local Development)

1.  Repoyu klonlayın.
2.  `appsettings.json` dosyasındaki Connection String'i kendi SQL Server bilginize göre düzenleyin.
3.  Package Manager Console üzerinden `Update-Database` komutunu çalıştırarak veritabanını oluşturun.
4.  Projeyi `Construction.Web` katmanından başlatın.

---
*Geliştirme süreci devam etmektedir.*