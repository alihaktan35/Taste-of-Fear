# Table01 Sahne Kurulum Talimatları

## Genel Bakış
Table01 sahnesi artık dinamik bir yemek yapma sistemi kullanıyor. Oyuncu malzemeleri tabağa sürükleyerek yemek yapabilir.

## Oluşturulan Script'ler

### 1. Core Scripts
- **RecipeData.cs**: Tarif verilerini tutan ScriptableObject
- **RecipeDatabase.cs**: Tüm tarifleri içeren veritabanı
- **TableSceneManager.cs**: Sahne yöneticisi, tarif parametresini alır ve UI'ı günceller
- **UIDraggableItem.cs**: Güncellenmiş - Sınırsız malzeme için klon oluşturur
- **UIPlateController.cs**: Yeniden yazıldı - Yeni tarif sistemi ile çalışır

### 2. Editor Tools
- **RecipeCreator.cs**: Unity Editor'de menü ekler, 17 tarifi otomatik oluşturur

## Unity Editor'de Kurulum Adımları

### Adım 1: Tarifleri Oluştur
1. Unity Editor'ü aç
2. Menü çubuğundan **Tools > Create All Recipes** seçeneğine tıkla
3. Bu işlem `Assets/Resources/Recipes/` klasörüne 17 tarif oluşturacak

### Adım 2: RecipeDatabase Oluştur
1. Project penceresinde `Assets/Resources` klasörüne sağ tıkla
2. **Create > Cooking > Recipe Database** seç
3. Dosyayı `RecipeDatabase` olarak adlandır
4. RecipeDatabase'i Inspector'da aç
5. **All Recipes** listesine `Assets/Resources/Recipes/` klasöründeki tüm tarifleri ekle (Drag & drop)

### Adım 3: Table01 Sahnesini Düzenle

#### 3.1 TableSceneManager Ekle
1. Table01 sahnesinde yeni bir Empty GameObject oluştur, adını `GameManager` yap
2. `TableSceneManager` component'ini ekle
3. Inspector'da şunları ata:
   - **Recipe Database**: Adım 2'de oluşturduğun RecipeDatabase
   - **Instruction Text**: Sahnedeki instruction text UI elementi (TextMeshPro veya Text)
   - **Plate Controller**: PlateImage GameObject'inin UIPlateController component'i
   - **Instruction Template**: "XXXX YAPINIZ" (veya istediğin format)

#### 3.2 Plate Image Ayarları
1. Sahnede PlateImage GameObject'ini bul (veya oluştur)
2. Şu component'lerin olduğundan emin ol:
   - `Image` component
   - `UIPlateController` component
3. UIPlateController Inspector'da:
   - **Plate Image**: PlateImage'in kendi Image component'i

#### 3.3 Ingredient'ları Ayarla
1. Sahnedeki TÜM ingredient GameObjects'leri seç (asit, göz, beyin, vb.)
2. Her birine `UIDraggableItem` component'i ekle (eğer yoksa)
3. Inspector'da **Ingredient Name** alanını GameObject'in adıyla aynı yap
   - Örnek: GameObject adı "göz" ise, Ingredient Name = "göz"
   - Örnek: GameObject adı "asit" ise, Ingredient Name = "asit"
4. Her ingredient'ın `Image` component'inde **Raycast Target** işaretli olmalı
5. Her ingredient parent Canvas altında olmalı

#### 3.4 Canvas Ayarları
1. Canvas GameObject'inin şu component'leri olmalı:
   - `Canvas`
   - `Canvas Scaler`
   - `Graphic Raycaster` (ZORUNLU - drag & drop için)

## Kullanım

### Başka Sahneden Table01'i Başlatma
```csharp
// Örnek: Order sahnesinden yemek yapmak için table01'e git
TableSceneManager.LoadTableSceneWithRecipe("Göz Küresi Çorbası");
```

### Test İçin
1. Table01 sahnesini direkt Play modunda aç
2. RequestedRecipeName boş olduğundan, ilk tarif kullanılacak (test için)
3. Malzemeleri PlateImage'a sürükle
4. Doğru malzemeleri doğru sayıda sürüklediğinde yemek tamamlanacak

## Tarif İsimleri (Kodda kullanmak için)
1. Göz Küresi Çorbası
2. Kan Gölü Sosu
3. Kertenkele Kuyrukları
4. Kurtçuk Kanepesi
5. Canavar Kalbi Izgara
6. Zombi Beyin Keki
7. Yaratık Kaburgaları
8. İğrenç Pizza
9. Dokunaç Güveci
10. Kesik Damar Spagetti
11. Zehirli Mantar Sepeti
12. Beyin Salatası
13. Örümcek Yumurtaları
14. Kanlı Şırıngalar
15. Çürümüş Diş Pastası
16. Parmak Sucukları (Tatlı)
17. Kusmuk Şekerlemesi

## Ingredient İsimleri (GameObject adları)
- asit
- bağırsak
- baharat
- balçık
- bardak
- beyin
- bilinmeyen yağ
- böcek yumurtası
- çürük ekmek
- çürümüş dişler
- diş cipsleri
- dokunaç
- et (pişmiş)
- et
- göz
- irin tozu
- kafatası
- kalp
- kanayan patates
- kara spagetti
- kara yosun
- kemik çubuğu
- kertenkele kuyruğu
- kırmızı şeker
- knife
- koyu zehir
- kurabiye
- kurtçuklar
- mantar
- metal kap
- örümcek
- parlak kan şişe
- parlak şişe
- parmak
- pıhtılanmış kan şişe
- şeker kasesei
- şırınga
- sivri kemik
- siyah zeytin
- tabak

## Yemek Görsellerini Ekleme
1. Her tarif için tamamlanmış yemek görseli hazırla
2. Unity'ye import et
3. Her RecipeData asset'ini aç
4. **Dish Sprite** alanına ilgili sprite'ı ata

## Sorun Giderme

### Malzemeler sürüklenmiyor
- Canvas'ta `Graphic Raycaster` component'i var mı?
- Ingredient'larda `Image` component'inin `Raycast Target` açık mı?
- `UIDraggableItem` component'i ekli mi?

### Tarif tamamlanmıyor
- Console'da debug mesajlarını kontrol et
- Ingredient Name'ler doğru yazılmış mı? (tam eşleşme gerekli)
- Doğru sayıda malzeme sürükledin mi?

### Instruction text güncellenmiyor
- TableSceneManager'da Instruction Text atandı mı?
- TextMeshPro kullanıyorsan `using TMPro;` import edilmiş mi?

## Özellikler
✅ Sınırsız malzeme (her sürüklemede yeni klon oluşur)
✅ 17 farklı tarif
✅ Dinamik tarif sistemi
✅ Scene parametresi ile yemek seçimi
✅ Otomatik doğrulama
✅ Yemek tamamlandığında sahne sonlanır
