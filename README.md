# أصدقاء المريض — سكيكدة | Amis du Malade

> **مشروع تخرج — ليسانس L3**  
> جامعة سكيكدة · قسم الإعلام الآلي · 2025/2026  
> تحت إشراف: توييل غسان

---

## نظرة عامة

منصة رقمية متكاملة لإدارة طوعية مرافقة المرضى، تتكون من جزأين مترابطين:

| الجزء | التقنية | الوصف |
|---|---|---|
| **الخلفية (Backend)** | ASP.NET Core 10 + PostgreSQL | واجهة برمجية REST مع مصادقة JWT |
| **التطبيق (Frontend)** | .NET MAUI — Windows Desktop | تطبيق سطح مكتب Windows بواجهة عربية RTL |

---

## معمارية النظام

```
┌──────────────────────────────────────────┐
│     تطبيق Windows — .NET MAUI           │
│     8 صفحات · واجهة عربية RTL           │
│     لوحة تحكم + واجهات عامة             │
└─────────────────┬────────────────────────┘
                  │  REST API (JSON + JWT)
┌─────────────────▼────────────────────────┐
│     Backend API — ASP.NET Core 10        │
│     10 Controllers · 10 Services         │
└─────────────────┬────────────────────────┘
                  │
┌─────────────────▼────────────────────────┐
│     PostgreSQL 14+                       │
│     29 جدول · EF Core Migrations        │
└──────────────────────────────────────────┘
```

---

## الجزء الأول — Backend API

### قاعدة البيانات (29 جدول)

| الفئة | الجداول |
|---|---|
| الإدارة | `AssociationBranches`, `Users`, `Hospitals` |
| المرضى | `Patients`, `PatientContacts`, `MedicalConditions`, `PatientMedicalConditions` |
| المتطوعون | `Volunteers`, `Skills`, `VolunteerSkills`, `VolunteerAvailabilities`, `VolunteerCoverageAreas`, `VolunteerDocuments`, `VolunteerInterviews` |
| التدريب | `Trainings`, `TrainingEnrollments` |
| الطلبات | `CareRequests`, `CareRequestRequiredSkills`, `Assignments` |
| الزيارات | `VisitSessions`, `VisitNotes`, `VisitRatings` |
| التنبيهات والمساهمات | `Alerts`, `Contributions` |
| النظام | `Notifications`, `AuditLogs`, `SyncLogs` |

### نقاط النهاية API

#### عامة (بدون مصادقة)
```
POST   /api/auth/login                    — تسجيل دخول المشرف
POST   /api/volunteer/register            — تسجيل متطوع جديد
POST   /api/carerequest/public            — إرسال طلب مرافقة
POST   /api/contribution                  — إرسال مساهمة
GET    /api/carerequest/suggestions/{id}  — اقتراحات المتطوعين (خوارزمية ذكية)
```

#### محمية بـ JWT (للمشرف)
```
GET/PUT /api/volunteer/{id}/status        — إدارة حالة المتطوع
POST    /api/interview                    — جدولة مقابلة
PUT     /api/interview/{id}/result        — تسجيل نتيجة المقابلة
GET/PUT /api/carerequest/{id}/status      — إدارة الطلبات
POST    /api/assignment                   — تعيين متطوع لطلب
GET     /api/dashboard/stats             — إحصائيات لوحة التحكم
GET     /api/alert                        — التنبيهات
GET     /api/training                     — التدريبات
GET     /api/contribution                 — المساهمات
```

### الخوارزمية الذكية لاقتراح المتطوعين

تعتمد على نظام نقاط (130 نقطة كحد أقصى):

| المعيار | النقاط |
|---|---|
| تطابق المهارات المطلوبة | حتى 40 نقطة |
| تغطية المنطقة الجغرافية | حتى 30 نقطة |
| التوفر في اليوم والوقت | حتى 30 نقطة |
| نوع الرعاية (منزل/مستشفى/ليلي) | حتى 10 نقاط |
| وسيلة نقل | 10 نقاط إضافية |

---

## الجزء الثاني — تطبيق Windows (.NET MAUI)

### الصفحات (8 صفحات)

| الصفحة | الوصف |
|---|---|
| `HomePage` | الصفحة الرئيسية مع التنقل |
| `CareRequestPage` | طلب مرافق — معالج 4 خطوات |
| `VolunteerRegisterPage` | تسجيل متطوع — معالج 4 خطوات |
| `ContributePage` | المساهمة (مال / عينية / وقت) |
| `AdminLoginPage` | تسجيل دخول المشرف |
| `AdminDashboardPage` | لوحة تحكم كاملة (إحصائيات + متطوعون + طلبات + مساهمات) |
| `VolunteerDetailPage` | تفاصيل المتطوع |
| `AboutPage` | معلومات الجمعية |

### لوحة تحكم المشرف

- **تبويبة الإحصائيات:** عدد المتطوعين، الطلبات، المساهمات
- **تبويبة المتطوعون:** قبول/رفض الطلبات + عرض التفاصيل
- **تبويبة الطلبات:** عرض الطلبات الجديدة + اقتراحات المتطوعين المثلى
- **تبويبة المساهمات:** متابعة المساهمات
- **نظام التعيين:** تأكيد ونجاح مع إشعارات

### الميزات التقنية

- واجهة عربية RTL كاملة مع دعم Windows
- Handler مخصص للـ Picker (RTL ComboBox fix)
- مصادقة JWT محفوظة محلياً
- تعدد لغات (عربي / فرنسي)
- تصميم متجاوب بألوان الجمعية

---

## تثبيت وتشغيل المشروع

### المتطلبات

```
- .NET 10 SDK
- PostgreSQL 14+
- Visual Studio 2022+ (مع workload MAUI)
```

### تشغيل الـ Backend

```bash
# نسخ المستودع
git clone https://github.com/Ouissalyahyaoui21/amis-du-malade-fullstack.git
cd amis-du-malade-fullstack

# إعداد قاعدة البيانات في appsettings.json
# "DefaultConnection": "Host=localhost;Database=amisdumalade;Username=...;Password=..."

# تطبيق الـ migrations وتشغيل الخادم
dotnet ef database update
dotnet run
# الخادم يعمل على: http://localhost:5000
```

### تشغيل تطبيق Windows

```bash
cd AmisDuMaladeApp
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0
```

أو من Visual Studio: فتح الحل ← اختيار `AmisDuMaladeApp` ← تشغيل على Windows.

---

## بيانات الدخول الافتراضية

```
Email:    admin@amisdumalade.dz
Password: Admin@123
```

---

## هيكل المشروع

```
amis-du-malade-fullstack/
├── Controllers/          # 10 Controllers (REST API)
├── Services/             # Business logic + خوارزمية الاقتراح
├── Models/               # 29 EF Core entity
├── Data/                 # AppDbContext + Migrations
├── ViewModels/           # Request/Response DTOs
├── AmisDuMaladeApp/      # تطبيق .NET MAUI Windows
│   ├── Views/            # 8 صفحات XAML
│   ├── ViewModels/       # MVVM ViewModels
│   ├── Services/         # ApiService + LocalizationService
│   ├── Models/           # Client-side models
│   └── Platforms/Windows/# Windows-specific handlers
├── appsettings.json
└── Program.cs
```

---

## المستودع

**GitHub:** https://github.com/Ouissalyahyaoui21/amis-du-malade-fullstack
