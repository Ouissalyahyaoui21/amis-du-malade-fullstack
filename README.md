# Amis du Malade — Skikda | Patient Companion Platform

> **Final Year Project (Licence L3)** — Full-stack volunteer coordination platform  
> University of Skikda · Department of Computer Science · 2025/2026  
> Supervisor: Touil Ghassen

---

## What Was Built

This is a **complete full-stack project** consisting of two parts:

| Part | Technology | Description |
|---|---|---|
| **Backend** | ASP.NET Core **10** + PostgreSQL | REST API with JWT authentication |
| **Mobile App** | .NET MAUI (Android / iOS) | Cross-platform mobile application |

---

## System Architecture

```
┌─────────────────────────────────────┐
│      Mobile App — .NET MAUI         │
│  Android / iOS · 8 complete pages   │
└──────────────┬──────────────────────┘
               │  REST API (JSON + JWT)
┌──────────────▼──────────────────────┐
│   Backend API — ASP.NET Core 8      │
│   10 controllers · 9 services       │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│        PostgreSQL 14+               │
│        20+ tables, auto-created     │
└─────────────────────────────────────┘
```

---

## Backend — ASP.NET Core API

### Database Entities (Models)

| Entity | Description |
|---|---|
| `User` | Admin accounts with BCrypt password hashing |
| `Volunteer` | Volunteer profiles with skills & availability |
| `Patient` | Patient records |
| `CareRequest` | Companion requests with priority system |
| `Assignment` | Volunteer-to-request assignments |
| `VisitSession` | Visit sessions with notes and ratings |
| `Alert` | Real-time operational alerts |
| `Contribution` ✨ | **Donor contributions** (money / goods / time) — *added this branch* |
| `Training` | Volunteer training sessions |
| `VolunteerInterview` | Interview scheduling |

### API Controllers & Endpoints

#### Public Endpoints (no authentication required)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/login` | Admin login → returns JWT token |
| `POST` | `/api/auth/register` | Create admin account |
| `POST` | `/api/volunteer/register` | Public volunteer registration |
| `POST` | `/api/carerequest` | Submit a companion request |
| `POST` | `/api/contribution` ✨ | Submit a donation/contribution |

#### Protected Endpoints (JWT Bearer token required)

**Volunteers**
```
GET  /api/volunteer                    → List all volunteers
GET  /api/volunteer/{id}               → Get volunteer by ID
PUT  /api/volunteer/{id}/status        → Update status (Active/Rejected)
GET  /api/carerequest/{id}/suggestions → Smart volunteer suggestions ⭐
```

**Care Requests**
```
GET  /api/carerequest                  → List all requests
GET  /api/carerequest/{id}             → Request details
PUT  /api/carerequest/{id}/status      → Update status
```

**Patients**
```
POST /api/patient                      → Add patient
GET  /api/patient                      → List patients
GET  /api/patient/{id}                 → Patient details
```

**Assignments**
```
POST /api/assignment                   → Assign volunteer to request
GET  /api/assignment                   → List assignments
PUT  /api/assignment/{id}/status       → Update assignment status
```

**Visit Sessions**
```
POST /api/visit/session                → Create session
GET  /api/visit/assignment/{id}        → Sessions by assignment
GET  /api/visit/session/{id}           → Session details
POST /api/visit/session/{id}/note      → Add visit note
POST /api/visit/session/{id}/rating    → Rate a session
```

**Alerts**
```
POST /api/alert                        → Create alert
GET  /api/alert                        → All alerts
GET  /api/alert/open                   → Open alerts only
PUT  /api/alert/{id}/resolve           → Resolve an alert
```

**Contributions** ✨ *(added this branch)*
```
GET  /api/contribution                 → List all contributions
GET  /api/contribution/{id}            → Contribution details
PUT  /api/contribution/{id}/status     → Update status (Confirmed/Distributed)
```

**Dashboard & Sync**
```
GET  /api/dashboard                    → Global statistics (including contributions) ✨
POST /api/sync/pull                    → Pull data for offline use
POST /api/sync/push                    → Push offline operations
```

### Smart Suggestion Algorithm ⭐

When assigning a volunteer to a care request, the system calculates a compatibility score:

```
Matching skills            = 40 points
Geographic coverage        = 30 points
Confirmed availability     = 30 points
Has transportation (bonus) = 10 points
─────────────────────────────────────
Maximum score              = 110 points
```
Returns the **top 5 candidates** sorted by score descending.

### What Was Added to the Backend in This Branch ✨

| Addition | File | Description |
|---|---|---|
| Contribution entity | `Models/Contribution.cs` | New model: Money / Goods / Time types |
| Contribution service | `Services/ContributionService.cs` | Full CRUD with status management |
| Contribution controller | `Controllers/ContributionController.cs` | Public POST + admin GET/PUT |
| Dashboard stats update | `Services/DashboardService.cs` | Added contribution counts |
| Dashboard VM update | `ViewModels/DashboardVM.cs` | `TotalContributions` + `PendingContributions` |
| DB auto-creation | `Program.cs` | `EnsureCreated()` — no manual migrations needed |
| Admin seed | `Program.cs` | Default admin user created on first run |

---

## Mobile App — .NET MAUI

### Pages (8 complete pages)

| Page | Description |
|---|---|
| `HomePage` | Landing page with 3 languages (Arabic / French / English) |
| `VolunteerRegisterPage` | 4-step registration wizard |
| `CareRequestPage` | 4-step companion request wizard |
| `ContributePage` | Donation form — connected to real API ✨ |
| `AdminLoginPage` | JWT login with lockout after 5 failed attempts |
| `AdminDashboardPage` | **11-tab admin dashboard** ✨ |
| `VolunteerDetailPage` | Full volunteer profile with actions ✨ |
| `AboutPage` | Association information |

### Admin Dashboard — 11 Tabs ✨

| Tab | Content |
|---|---|
| 🏠 Overview | 6 color-coded stat cards + recent activities + alert summary |
| 🤝 Volunteers | Filter (All/Active/Pending) + full cards + approve/reject/WhatsApp/details |
| 🗣️ Interviews | Placeholder (ready for development) |
| 🎓 Training | Placeholder (ready for development) |
| ⭐ Evaluations | Placeholder (ready for development) |
| ⏳ New Requests | Pending list + **assign volunteer popup** with smart suggestions |
| 📋 All Requests | Full list with status badges |
| 👥 Patients | List with avatar + details (lazy loaded) |
| 🚨 Alerts | List + inline resolve button |
| 📊 Reports | Numeric summary + activity bars |
| 💰 Contributions | List + confirm + distribute buttons (lazy loaded) ✨ |

### Key Mobile Features Added This Branch ✨

| Feature | Description |
|---|---|
| **Pull-to-refresh** | Swipe down to reload data on any tab |
| **Error state UI** | Red banner with "Retry" button when connection fails |
| **Assign volunteer popup** | Modal with scored suggestions when tapping "Assign" |
| **Volunteer detail page** | Full profile view with capability tags + action buttons |
| **Real contribution submit** | `ContributePage` now POSTs to `/api/contribution` |
| **Dashboard contribution stats** | Counts pulled from API on load |
| **WhatsApp integration** | One-tap opens WhatsApp conversation (`wa.me/213...`) |
| **Lazy loading** | Patients and contributions load only when their tab is opened |

### Value Converters

| Converter | Usage |
|---|---|
| `IntToBoolConverter` | Show alert banner only when count > 0 |
| `FirstCharConverter` | Extract first letter for avatar circles |
| `StringNotEmptyConverter` | Show optional fields only when not empty |
| `InverseBoolConverter` | Hide content while loading |

---

## Default Admin Credentials

> Automatically created on first backend run — no manual setup needed.

| Field | Value |
|---|---|
| **Email** | `admin@amisdumalade.dz` |
| **Password** | `Admin2024!` |
| **Role** | Admin |

---

## How to Run the Project

### Prerequisites

| Tool | Version | Link |
|---|---|---|
| .NET SDK | 8.0+ | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| MAUI workload | latest | `dotnet workload install maui` |
| PostgreSQL | 14+ | [postgresql.org](https://www.postgresql.org/download/) |
| Android Emulator | API 21+ | Via Android Studio or Visual Studio |

---

### Step 1 — Set Up the Database

```bash
# Create the database in PostgreSQL:
psql -U postgres -c "CREATE DATABASE amis_du_malade;"
```

Edit `appsettings.json` if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=amis_du_malade;Username=postgres;Password=postgres123"
}
```

---

### Step 2 — Run the Backend

```bash
cd amis-du-malade-backend
dotnet run
```

On first run, the backend automatically:
- Creates all database tables (`EnsureCreated`)
- Seeds the default admin user

The API runs on: **`http://localhost:5113`**

---

### Step 3 — Test the API

```bash
# Login and get a JWT token:
curl -X POST http://localhost:5113/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"Email":"admin@amisdumalade.dz","Password":"Admin2024!"}'
```

Use the returned token in subsequent requests:
```
Authorization: Bearer <token>
```

Token validity: **7 days**

---

### Step 4 — Configure the Mobile App URL

Open `AmisDuMaladeApp/Constants/AppConstants.cs` and set `BaseUrl`:

```csharp
// Android Emulator (maps localhost to host machine):
public const string BaseUrl = "http://10.0.2.2:5113/";

// Real Android device on same Wi-Fi (replace X with your PC's IP):
public const string BaseUrl = "http://192.168.1.X:5113/";

// Windows/Mac local development with hot reload:
public const string BaseUrl = "http://localhost:5113/";
```

---

### Step 5 — Run the Mobile App

**Via Visual Studio 2022:**
1. Open the solution folder in Visual Studio
2. Set `AmisDuMaladeApp` as the startup project
3. Select an Android emulator or connected device
4. Press **F5**

**Via command line:**
```bash
cd AmisDuMaladeApp
dotnet build -t:Run -f net10.0-android
```

---

### Step 6 — Test the App

1. Open the app → Home page appears
2. Tap **"دخول الإدارة"** (Admin Login) at the bottom
3. Enter: `admin@amisdumalade.dz` / `Admin2024!`
4. The 11-tab admin dashboard opens
5. Try the full flow:
   - Register a volunteer (from home page)
   - Approve/reject the volunteer from the Volunteers tab
   - Submit a care request and assign a volunteer to it
   - Record a contribution and confirm it from the dashboard

---

## Project Structure

```
amis-du-malade-backend/
│
├── Controllers/               ← API entry points (10 controllers)
│   ├── AuthController.cs
│   ├── VolunteerController.cs
│   ├── CareRequestController.cs
│   ├── PatientController.cs
│   ├── AssignmentController.cs
│   ├── VisitController.cs
│   ├── AlertController.cs
│   ├── DashboardController.cs
│   ├── ContributionController.cs  ✨ added this branch
│   └── SyncController.cs
│
├── Models/                    ← Database entities (EF Core)
├── Services/                  ← Business logic layer
├── ViewModels/                ← API request/response DTOs
├── Data/
│   └── AppDbContext.cs        ← EF Core DbContext + seed data
├── Program.cs                 ← App startup + DI + DB init
└── appsettings.json           ← DB connection + JWT config
│
└── AmisDuMaladeApp/           ← .NET MAUI mobile app
    ├── Constants/             ← BaseUrl + app keys
    ├── Converters/            ← XAML value converters  ✨ 4 converters
    ├── Models/                ← API response models
    ├── Services/              ← ApiService + AuthTokenService
    ├── ViewModels/            ← MVVM ViewModels (8 VMs)
    ├── Views/                 ← XAML pages (8 pages)
    └── Resources/             ← Images + fonts
```

---

## Technologies

| Technology | Version | Usage |
|---|---|---|
| ASP.NET Core | **10.0** | REST API framework |
| Entity Framework Core | **10.0** | ORM + PostgreSQL driver |
| BCrypt.Net-Next | 4.1 | Secure password hashing |
| JWT Bearer | **10.0** | Stateless authentication |
| .NET MAUI | 10.0 | Cross-platform mobile |
| CommunityToolkit.Mvvm | 8.3 | MVVM + source generators |
| PostgreSQL | 14+ | Relational database |

---

*Final Year Project — University of Skikda, Computer Science Department, 2025/2026*  
*Supervisor: Touil Ghassen*
