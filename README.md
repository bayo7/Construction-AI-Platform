# Construction & Real Estate AI Platform

Bu proje, modern web teknolojileri ve **N-Tier (Çok Katmanlı)** mimari kullanılarak geliştirilmiş, dinamik bir inşaat ve gayrimenkul yönetim sistemidir. Klasik CMS özelliklerinin yanı sıra, **Yapay Zeka (AI)** destekli akıllı tavsiye motoru ile kullanıcıların arama niyetlerini anlamsal olarak analiz edip (örn: "doğa ile iç içe" ev arayan birine manzaralı projeleri eşleştirmek) en doğru gayrimenkul önerilerini sunmayı hedefler.

## 🛠 Teknoloji Yığını (Tech Stack)

Proje geliştirme sürecinde aşağıdaki teknolojiler ve kütüphaneler kullanılmıştır:

* **Backend:** ASP.NET Core 8.0 / 9.0
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core (Code First Yaklaşımı)
* **Authentication & Authorization:** ASP.NET Core Identity
* **Mimari Desenler:** N-Tier Architecture, Generic Repository Pattern, Dependency Injection (DI), Data Transfer Objects (DTO/ViewModel)
* **Frontend (Admin):** Bootstrap 5, Sneat Admin Template
* **Frontend (UI):** UpConstruction Template (Entegrasyon aşamasında)

## 📂 Mimari Yapı

Proje, S.O.L.I.D. prensiplerine uygun olarak 4 ana katmana ayrılmıştır:

1.  **Entity Layer:** Veritabanı tabloları ve özelleştirilmiş Identity sınıfları (`AppUser`, `AppRole`).
2.  **DataAccess Layer:** Veri erişim kodları, Generic Repository, Context yapılandırması (`IdentityDbContext`) ve Migration'lar.
3.  **Business Layer:** İş kuralları, Validasyonlar ve AI servis entegrasyonları.
4.  **Web (UI) Layer:** MVC yapısı, Controller'lar, View'lar, ViewModel'ler ve Admin Paneli (Areas).

## ✅ Tamamlanan Özellikler (v1.1 - Pre-Alpha)

Şu ana kadar projenin altyapısı, temel yönetim paneli ve kimlik doğrulama sistemleri tamamlanmıştır:

* [x] **Kurumsal Mimari:** Entity, DataAccess ve Business katmanları arasındaki soyutlama (Interface) ve somutlama (Concrete) yapıları kuruldu.
* [x] **Veritabanı Tasarımı:** Projeler, Kategoriler ve Müşteri Yorumları (Testimonials) için ilişkisel Code-First tabloları oluşturuldu.
* [x] **Admin Paneli Altyapısı:** "Sneat" teması projeye entegre edildi, "Areas" yapısı ile UI'dan izole edildi.
* [x] **Kategori & Referans Yönetimi:** Kategori ve Müşteri Yorumlarının (Testimonials) listelenmesi, eklenmesi ve silinmesi için Backend/UI kodları yazıldı (Eager Loading / Include işlemleri entegre edildi).
* [x] **Özelleştirilmiş Identity Entegrasyonu:** ASP.NET Core Identity altyapısı kurularak `AppUser` ve `AppRole` sınıfları sisteme entegre edildi.
* [x] **Kullanıcı Yönetimi (User Management):** Admin panelinden sisteme yeni kullanıcı ekleme, listeleme ve silme (ViewModel ve UserManager kullanılarak) işlemleri tamamlandı.
* [x] **Rol Yönetimi ve Atama (Role Management & Assign):** Dinamik rol oluşturma/silme ve çoklu Checkbox (List Model Binding) yapısı ile kullanıcılara rol atama ekranları kodlandı.
* [x] **Güvenlik ve Oturum (Authorization):** Controller seviyesinde `[Authorize]` attribute'u ile rol bazlı erişim kısıtlamaları getirildi. Cookie (Çerez) yapılandırması ile yetkisiz erişimler engellendi.
* [x] **Login (Giriş Yap) Modülü:** Güvenlik duvarından geçen yetkili kullanıcılar için kullanıcı adı/şifre ile sisteme giriş ekranının tasarlandı ve kodlandı.
* [x] **AI Destekli Akıllı Öneri Sistemi:** Kullanıcıların incelediği ilanları ve arama terimlerini doğal dil işleme (NLP) ile analiz ederek, kullanıcının gerçek niyetini anlayan (semantic search) ve ona en uygun, benzer veya alternatif projeleri/ilanları sunan yapay zeka tavsiye motoru entegrasyonu yapıldı.
* [x] **Frontend (Müşteri Yüzü):** Müşterilerin projeleri inceleyebileceği arayüzün (UpConstruction) Backend ile bağlanması ve dinamikleştirilmesi.
* [x] **Dashboard Widget'ları:** Admin panelinde aktif proje sayısı, son eklenenler, sistemdeki kullanıcı sayısı gibi istatistiklerin grafiklerle sunulması.
* [x] **Medya Yönetimi:** Projeler için çoklu fotoğraf yükleme altyapısı.
* [x] **Gelişmiş Yetkilendirme:** Sadece sayfa bazlı değil, buton/menü bazlı (View seviyesinde) Editör/Admin yetki ayrımlarının yapılması.
## 🚀 Gelecek Planları (Roadmap)

Projenin bir sonraki fazında aşağıdaki özellikler eklenecektir:




## ⚙️ Kurulum (Local Development)

1.  Repoyu klonlayın.
2.  `appsettings.json` dosyasındaki Connection String'i kendi SQL Server bilginize göre düzenleyin.
3.  Package Manager Console üzerinden `Update-Database` komutunu çalıştırarak veritabanını (Identity tabloları dahil) oluşturun.
4.  Projeyi `Construction.Web` katmanından başlatın.

---
*Geliştirme süreci devam etmektedir.*
