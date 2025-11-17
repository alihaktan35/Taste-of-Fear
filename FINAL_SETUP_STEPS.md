# Table01 Final Setup Steps

## ✅ Yapılan Değişiklikler

### 1. Malzemelerin Tabakta Görünmesi Sorunu ✅
- **Düzeltildi:** Ingredient'lar artık tabağın üstünde görünecek
- `SetAsLastSibling()` eklendi (z-order düzeltmesi)
- Scale 0.25'e küçültüldü (daha iyi görünüm)

### 2. Instruction Text Template ✅
- **Eski:** "THE ORDER IS XXXX"
- **Yeni:** "The order is a XXXX.\nDrag the 4 ingredients on the table to the plate to make it."
- **Dinamik:** "4 ingredients" kısmı otomatik olarak gerçek malzeme sayısıyla değiştirilecek

### 3. Yemek Görsellerinin Otomatik Atanması ✅
- Editor script oluşturuldu: `AssignDishSprites.cs`
- Assets/Images/Orders klasöründeki görselleri otomatik olarak tariflere atar

---

## 🎯 Unity Editor'de Yapılması Gerekenler

### Adım 1: Yemek Görsellerini Tariflere Ata

1. **Unity Editor'ü aç**
2. Menü çubuğundan **Tools → Assign Dish Sprites to Recipes** seç
3. Script otomatik olarak tüm yemek görsellerini tariflere atayacak
4. Dialog kutusu kaç tarifin başarıyla güncellendiğini gösterecek

**Not:** Eğer bazı yemekler eşleşmezse, Console'da hangi yemeklerin başarısız olduğunu göreceksin. Manuel olarak:
- Project → Assets/Resources/Recipes → ilgili tarifi seç
- Inspector'da **Dish Sprite** alanına Assets/Images/Orders'dan ilgili görseli sürükle

### Adım 2: GameManager'daki Instruction Template'i Kontrol Et

1. **Table01 sahnesini aç**
2. **Hierarchy → GameManager** seç
3. **Inspector → TableSceneManager** component'ine bak
4. **Instruction Template** alanında şu yazmalı:
   ```
   The order is a XXXX.
   Drag the 4 ingredients on the table to the plate to make it.
   ```
5. Eğer farklı bir yazı varsa, yukarıdaki metni kopyala yapıştır
6. **Sahneyi kaydet** (Ctrl+S / Cmd+S)

### Adım 3: PlateImage Ayarlarını Kontrol Et

1. **Hierarchy → Canvas → PlateImage** seç
2. **Inspector** penceresinde kontrol et:
   - ✅ **Image** component var
   - ✅ **UIPlateController** component var
   - ✅ UIPlateController'da **Plate Image** referansı atanmış (kendi Image component'i)

---

## 🧪 Test Adımları

### Test 1: Temel Drag & Drop
1. **Play** butonuna bas ▶️
2. Console'da şunu görmeli:
   ```
   Setting up recipe: Göz Küresi Çorbası
   Required ingredients for Göz Küresi Çorbası:
     - 2x göz
     - 1x kara yosun
     - 3x pıhtılanmış kan şişe
   ```
3. Instruction text'te şöyle yazmalı:
   ```
   The order is a GÖZ KÜRESİ ÇORBASI.
   Drag the 6 ingredients on the table to the plate to make it.
   ```
   (6 = 2 göz + 1 kara yosun + 3 pıhtılanmış kan şişe)

### Test 2: Malzemelerin Tabakta Görünmesi
1. **"göz"** malzemesini PlateImage'a sürükle
2. **Beklenen sonuç:**
   - Göz malzemesi **PlateImage'ın ÜZERİNDE** küçülmüş halde görünmeli
   - Orijinal göz hala masada olmalı (sınırsız malzeme!)
   - Console: `Dropped ingredient: göz`
   - Console: `Ingredients on plate: göz:1`

### Test 3: Tarif Tamamlama
1. Doğru malzemeleri ekle:
   - 2x göz
   - 1x kara yosun
   - 3x pıhtılanmış kan şişe
2. **Beklenen sonuç:**
   - Console: `Recipe complete: Göz Küresi Çorbası`
   - Console: `Dish completed: Göz Küresi Çorbası`
   - Tabaktaki tüm malzemeler kaybolmalı
   - PlateImage'da **"Göz küresi çorbası.png"** görseli görünmeli
   - 2 saniye sonra: `Scene complete! Returning to previous scene...`

### Test 4: Yanlış Malzeme Testi
1. Play'e bas
2. Rastgele malzemeler ekle (yanlış tarif)
3. **Beklenen sonuç:**
   - Malzemeler tabağa eklenecek
   - Ama tarif tamamlanmayacak
   - Console: `Ingredients don't match recipe yet.`

---

## 🎮 Tarif Listesi ve Yemek Görselleri

### Eşleşme Tablosu

| Tarif Adı | Yemek Görseli (Assets/Images/Orders) | Durum |
|-----------|--------------------------------------|-------|
| Göz Küresi Çorbası | Göz küresi çorbası.png | ✓ |
| Kan Gölü Sosu | kan gölü sosu.png | ✓ |
| Kertenkele Kuyrukları | kertenkele kuyrukları(tabak).png | ✓ |
| Kurtçuk Kanepesi | Kurtçuk Kanepesi.png | ✓ |
| Canavar Kalbi Izgara | Canavar Kalbi Izgara.png | ✓ |
| Zombi Beyin Keki | Zombi Beyin Keki.png | ✓ |
| Yaratık Kaburgaları | Yaratık Kaburgaları.png | ✓ |
| İğrenç Pizza | iğrenç pizza.png | ✓ |
| Dokunaç Güveci | Dokunaç Güveci.png | ✓ |
| Kesik Damar Spagetti | Kesik Damar Spagetti.png | ✓ |
| Zehirli Mantar Sepeti | Zehirli Mantar Sepeti.png | ✓ |
| Beyin Salatası | beyin salatası.png | ✓ |
| Örümcek Yumurtaları | Örümcek Yumurtaları.png | ✓ |
| Kanlı Şırıngalar | Kanlı Şırıngalar.png | ✓ |
| Çürümüş Diş Pastası | Çürümüş Diş Pastası.png | ✓ |
| Parmak Sucukları (Tatlı) | Parmak Sucukları (Tatlı).png | ✓ |
| Kusmuk Şekerlemesi | Kusmuk Şekerlemesi.png | ✓ |

**Toplam:** 17 tarif, 17 görsel

---

## 🔧 Sorun Giderme

### Malzemeler tabakta görünmüyor
**Çözüm:**
- PlateImage'ın Canvas'ta doğru sırada olduğundan emin ol
- Ingredient'ların Image component'inde Color alpha değeri 1 (opak) olmalı
- Script güncellemesi yapıldıysa, Unity'yi yeniden başlat

### Yemek görseli gösterilmiyor
**Çözüm:**
1. RecipeData asset'ini aç (Assets/Resources/Recipes)
2. **Dish Sprite** alanını kontrol et
3. Boşsa, Assets/Images/Orders'dan ilgili görseli sürükle
4. Veya Tools → Assign Dish Sprites to Recipes'i tekrar çalıştır

### Instruction text malzeme sayısını göstermiyor
**Çözüm:**
- GameManager → TableSceneManager → Instruction Template'de "4 ingredients" ifadesi olmalı
- Script dinamik olarak bunu gerçek sayıyla değiştirecek

### Tarif tamamlanmıyor
**Çözüm:**
1. Console'da hangi malzemelerin gerekli olduğunu kontrol et
2. **Tam olarak** o malzemeleri **tam olarak** o sayılarda ekle
3. Ingredient Name'lerin GameObject adlarıyla eşleştiğinden emin ol
4. Console'da debug mesajlarına bak: `Ingredients on plate: ...`

---

## ✅ Final Checklist

Tamamlanması gerekenler:

- [ ] Unity Editor açıldı
- [ ] Tools → Assign Dish Sprites to Recipes çalıştırıldı
- [ ] 17 tarifin hepsi sprite aldı (Console'da kontrol et)
- [ ] Table01 sahnesi açıldı
- [ ] GameManager → Instruction Template doğru metin var
- [ ] PlateImage → UIPlateController → Plate Image referansı atanmış
- [ ] Test edildi: Malzemeler tabakta görünüyor
- [ ] Test edildi: Tarif tamamlanınca yemek görseli gösteriliyor
- [ ] Sahne kaydedildi

**Hepsi tamamsa:** Sistem %100 hazır! 🎉

---

## 📞 Başka Sahneden Table01'e Geçiş

```csharp
// Örnek: Order sahnesinden
TableSceneManager.LoadTableSceneWithRecipe("Göz Küresi Çorbası");
```

Tarif isimleri büyük/küçük harf duyarlı!
