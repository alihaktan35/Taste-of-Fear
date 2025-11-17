# Table01 Kurulum Durum Raporu

## ✅ Tamamlanan Kurulumlar

### 1. GameManager - TableSceneManager ✅
- **Durum:** TAMAM
- **Lokasyon:** Hierarchy → GameManager
- **Referanslar:**
  - ✅ Recipe Database: Atanmış
  - ✅ Instruction Text: Atanmış (fileID: 1252008523)
  - ✅ Plate Controller: Atanmış (fileID: 1126321801)
  - ⚠️ Instruction Template: "THE ORDER IS XXXX" (Türkçe yapılabilir)

### 2. PlateImage - UIPlateController ✅
- **Durum:** TAMAM
- **Lokasyon:** Hierarchy → Canvas → PlateImage
- **Referanslar:**
  - ✅ Plate Image: Atanmış (kendi Image component'i)
  - ✅ Current Recipe: Boş (normal, runtime'da doldurulacak)

### 3. Canvas - Graphic Raycaster ✅
- **Durum:** TAMAM
- **Lokasyon:** Hierarchy → Canvas
- ✅ Graphic Raycaster component ekli ve aktif

### 4. Ingredients - UIDraggableItem ⚠️
- **Durum:** NEREDEYSE TAMAM
- **35/40 ingredient'ta component var**

#### ✅ UIDraggableItem olan ingredientlar (35 adet):
1. asit ✓
2. bağırsak ✓
3. baharat ✓
4. balçık ✓
5. beyin ✓
6. bilinmeyen yağ ✓
7. böcek yumurtası ✓
8. çürük ekmek ✓
9. çürümüş dişler ✓
10. diş cipsleri ✓
11. dokunaç ✓
12. et (pişmiş) ✓
13. et ✓
14. göz ✓
15. irin tozu ✓
16. kafatası ✓
17. kalp ✓
18. kanayan patates ✓
19. kara spagetti ✓
20. kara yosun ✓
21. kemik çubuğu ✓
22. kertenkele kuyruğu ✓
23. kırmızı şeker ✓
24. koyu zehir ✓
25. kurabiye ✓
26. kurtçuklar ✓
27. mantar ✓
28. örümcek ✓
29. parlak kan şişe ✓
30. parmak ✓
31. pıhtılanmış kan şişe ✓
32. şeker kasesi ✓
33. şırınga ✓
34. sivri kemik ✓
35. siyah zeytin ✓

#### ❌ UIDraggableItem OLMAYAN ingredientlar (5 adet):
1. **bardak** ❌
2. **knife** ❌
3. **metal kap** ❌
4. **parlak şişe** ❌
5. **tabak** ❌

---

## 🔧 Yapılması Gerekenler

### Adım 1: Eksik 5 Ingredient'a UIDraggableItem Ekle

Unity Editor'de:

1. **Table01 sahnesini aç**

2. **Hierarchy'de şu ingredient'ları BUL ve SEÇ:**
   - bardak
   - knife
   - metal kap
   - parlak şişe
   - tabak

3. **Her biri için:**
   - GameObject'i seç
   - Inspector → **Add Component**
   - **UIDraggableItem** yaz ve ekle
   - **Ingredient Name** alanına GameObject'in adını yaz:
     - bardak → Ingredient Name: "bardak"
     - knife → Ingredient Name: "knife"
     - metal kap → Ingredient Name: "metal kap"
     - parlak şişe → Ingredient Name: "parlak şişe"
     - tabak → Ingredient Name: "tabak"
   - Image component'inde **Raycast Target** işaretli olmalı ✓

4. **Sahneyi kaydet** (Ctrl+S / Cmd+S)

### Adım 2: Instruction Template'i Türkçeleştir (Opsiyonel)

1. **GameManager** GameObject'ini seç
2. **TableSceneManager** component'inde
3. **Instruction Template** alanını şu şekilde değiştir:
   - Şu an: "THE ORDER IS XXXX"
   - Önerilen: "XXXX YAPINIZ"

### Adım 3: Test Et!

1. **Play moduna gir** ▶️
2. **Console'u aç** (Window → General → Console)
3. **Görmek istediğin mesajlar:**
   ```
   No recipe requested! Using default for testing.
   Setting up recipe: Göz Küresi Çorbası
   Required ingredients for Göz Küresi Çorbası:
     - 2x göz
     - 1x kara yosun
     - 3x pıhtılanmış kan şişe
   ```

4. **Drag & Drop Test:**
   - Bir ingredient'a tıkla ve tut
   - PlateImage'a sürükle
   - Bırak
   - Console'da "Dropped ingredient: [ad]" görmeli
   - Malzeme PlateImage üzerinde küçülmüş halde görünmeli
   - Orijinal malzeme hala yerinde olmalı (sınırsız malzeme!)

5. **Tarif Tamamlama Test:**
   - 2x göz ekle
   - 1x kara yosun ekle
   - 3x pıhtılanmış kan şişe ekle
   - Console: "Recipe complete: Göz Küresi Çorbası"
   - 2 saniye sonra "Scene complete!" mesajı

---

## 📊 Kurulum Özeti

| Bileşen | Durum | Tamamlanma |
|---------|-------|------------|
| GameManager + TableSceneManager | ✅ Tamam | 100% |
| PlateImage + UIPlateController | ✅ Tamam | 100% |
| Canvas + Graphic Raycaster | ✅ Tamam | 100% |
| Ingredients + UIDraggableItem | ⚠️ Eksik | 87.5% (35/40) |
| **GENEL DURUM** | **⚠️ Neredeyse Hazır** | **96.9%** |

---

## ⚡ Hızlı Düzeltme Listesi

Sadece şunları yap ve %100 hazır olacak:

- [ ] Hierarchy'de "bardak" GameObject'ini bul → Add Component → UIDraggableItem → Ingredient Name: "bardak"
- [ ] Hierarchy'de "knife" GameObject'ini bul → Add Component → UIDraggableItem → Ingredient Name: "knife"
- [ ] Hierarchy'de "metal kap" GameObject'ini bul → Add Component → UIDraggableItem → Ingredient Name: "metal kap"
- [ ] Hierarchy'de "parlak şişe" GameObject'ini bul → Add Component → UIDraggableItem → Ingredient Name: "parlak şişe"
- [ ] Hierarchy'de "tabak" GameObject'ini bul → Add Component → UIDraggableItem → Ingredient Name: "tabak"
- [ ] Sahneyi kaydet (Ctrl+S)
- [ ] Test et (Play butonuna bas)

---

## 🎮 Sistem Özellikleri (Hazır!)

✅ **17 Farklı Tarif Sistemi**
✅ **Sınırsız Malzeme** (klon sistemi)
✅ **Dinamik Tarif Seçimi** (scene parametresi ile)
✅ **Otomatik Tarif Doğrulama**
✅ **UI Güncellemesi** (instruction text otomatik)

---

## 📝 Notlar

- RecipeDatabase ve 17 tarif zaten oluşturulmuş ve atanmış
- Tüm script'ler hazır ve çalışır durumda
- Sadece 5 ingredient'a component eklemek kaldı
- Test için sahneyi direkt Play modunda açabilirsin
