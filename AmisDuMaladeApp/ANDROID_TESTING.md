# دليل تجربة التطبيق على Android

## المتطلبات

- **Visual Studio 2022** (v17.8+) مع workload **".NET Multi-platform App UI development"**
- أو **.NET 10 SDK** + **Android SDK** يدوياً
- الكمبيوتر والجهاز على نفس شبكة Wi-Fi (للجهاز الحقيقي فقط)

---

## الطريقة 1 — محاكي Android Emulator (الأسهل)

### الخطوات:

1. **شغّل الـ Backend** على الجهاز:
   ```
   cd <مجلد المشروع>
   dotnet run --project AmisduMalade.csproj
   ```
   الـ API يكون شغال على `http://localhost:5113`

2. **افتح Visual Studio 2022** وافتح ملف `AmisDuMaladeApp.csproj`

3. **اختر Android Emulator** من القائمة المنسدلة في شريط الأدوات:
   - اضغط على **Tools → Android → Android Device Manager**
   - أنشئ جهازاً افتراضياً (AVD) لو ما عندكش واحد
   - اختر الجهاز الافتراضي من القائمة المنسدلة

4. **اختر Framework**: من القائمة المنسدلة للـ framework اختر `net10.0-android`

5. اضغط **F5** أو زر التشغيل الأخضر

> ✅ `AppConstants.cs` فيه بالفعل `http://10.0.2.2:5113/` للأندرويد — هذا هو الـ IP الصحيح للوصول لـ localhost من داخل المحاكي.

---

## الطريقة 2 — جهاز Android حقيقي عبر USB

### الخطوات:

1. **فعّل Developer Mode** على الهاتف:
   - اذهب إلى **الإعدادات → حول الهاتف**
   - اضغط على **رقم البناء (Build Number)** 7 مرات
   - ارجع للإعدادات → **خيارات المطور**
   - فعّل **USB Debugging**

2. **وصّل الهاتف بالـ USB** وقبل طلب التصريح على الهاتف

3. **غيّر الـ IP في `Constants/AppConstants.cs`**:
   ```csharp
   // ابحث عن IP الـ PC بتاعك: ipconfig في CMD
   public const string BaseUrl = "http://192.168.1.XXX:5113/";
   //                                         ^^^^^^^^^^^
   //                               ضع IP الـ PC الخاص بك هنا
   ```

4. **تأكد أن الـ Backend يسمع على كل الـ interfaces**:
   في `appsettings.json` أو عند تشغيله:
   ```
   dotnet run --urls "http://0.0.0.0:5113"
   ```

5. في Visual Studio اختر الهاتف من القائمة المنسدلة واضغط **F5**

---

## الطريقة 3 — جهاز حقيقي عبر Wi-Fi (بدون USB)

### الخطوات:

1. وصّل الهاتف بالـ USB أولاً وفعّل USB Debugging
2. شغّل في CMD:
   ```
   adb tcpip 5555
   adb connect 192.168.1.YYY:5555
   ```
   (استبدل `192.168.1.YYY` بـ IP الهاتف — تجده في **الإعدادات → Wi-Fi → تفاصيل الشبكة**)
3. افصل الـ USB
4. Visual Studio سيرى الهاتف عبر Wi-Fi
5. غيّر الـ IP في `AppConstants.cs` كما في الطريقة 2

---

## بناء APK بدون تشغيل مباشر

لو تحب تبني ملف APK وتركّبه يدوياً:

```bash
cd AmisDuMaladeApp
dotnet build -f net10.0-android -c Release /p:AndroidPackageFormats=apk
```

الـ APK يكون في:
```
AmisDuMaladeApp/bin/Release/net10.0-android/com.amisdumalade.app-Signed.apk
```

---

## مشاكل شائعة

| المشكلة | الحل |
|---------|------|
| `Connection refused` على الجهاز | تحقق من IP الـ PC في `AppConstants.cs` وأن الـ firewall يسمح بالمنفذ 5113 |
| `CLEARTEXT not permitted` | `AndroidManifest.xml` فيه `android:usesCleartextTraffic="true"` ✅ |
| الهاتف ما يظهرش في VS | تأكد من USB Debugging وتقبل الـ authorization popup على الهاتف |
| `SDK not found` | Visual Studio Installer → Modify → تأكد تثبيت Android SDK |
