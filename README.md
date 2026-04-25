# 🏥 Association Amis du Malade - Backend API

Plateforme de coordination bénévole  
Association Amis du Malade - Skikda, Algérie  
Année académique 2025/2026

---

## 📋 Description

Backend API REST pour la plateforme de coordination entre bénévoles et patients.  
Développé dans le cadre du projet de fin de licence.

---

## 🏗️ Architecture générale
---

## ⚙️ Technologies utilisées

| Couche | Technologie |
|--------|------------|
| Langage | C# |
| Framework | ASP.NET Core 10 |
| Base de données | PostgreSQL 13 |
| ORM | Entity Framework Core |
| Authentification | JWT Bearer Token |
| Architecture | MVVM + REST API |

---

## 🗄️ Base de données - 28 tables

### Administration
- `association_branches` - Centres de l'association par commune
- `users` - Comptes administrateurs et coordinateurs
- `hospitals` - Hôpitaux partenaires et référents

### Patients
- `patients` - Dossiers patients (table indépendante)
- `patient_contacts` - Contacts familiaux par patient
- `medical_conditions` - Référentiel des conditions médicales
- `patient_medical_conditions` - Liaison patient / conditions

### Bénévoles
- `volunteers` - Profils bénévoles
- `skills` - Référentiel des compétences
- `volunteer_skills` - Compétences par bénévole
- `volunteer_availability` - Disponibilités structurées
- `volunteer_coverage_areas` - Zones géographiques couvertes
- `volunteer_documents` - Documents de vérification
- `volunteer_interviews` - Entretiens de validation

### Formation
- `trainings` - Sessions de formation
- `training_enrollments` - Inscriptions aux formations

### Opérations
- `care_requests` - Demandes de prise en charge
- `care_request_required_skills` - Compétences requises par demande
- `care_request_schedules` - Plannings des demandes
- `assignments` - Affectations bénévole / demande
- `visit_sessions` - Sessions de visite
- `visit_notes` - Notes de visite
- `visit_ratings` - Évaluations post-visite
- `alerts` - Alertes opérationnelles

### Système
- `notifications` - Historique des notifications
- `devices` - Appareils mobiles enregistrés
- `sync_log` - Journal de synchronisation
- `audit_log` - Journal d'audit des actions

---

## 🌐 API Endpoints

### 🔓 Publics (sans authentification)

### 🔒 Protégés (TOKEN JWT requis)

---

## ⭐ Algorithme de suggestion intelligente

Pour chaque demande, l'algorithme calcule un score de compatibilité:

Retourne les **5 meilleurs candidats** triés par score décroissant.

---

## 🔐 Authentification JWT

---

## 🚀 Installation et lancement

### Prérequis
- .NET 10 SDK
- PostgreSQL 13+
- Postman (pour les tests)

### Étapes

**1. Cloner le projet**
```bash
git clone https://github.com/Ouissalyahyaoui21/amis-du-malade-backend.git
cd amis-du-malade-backend
```

**2. Créer la base de données**
```bash
psql -U postgres -c "CREATE DATABASE amis_du_malade;"
psql -U postgres -d amis_du_malade -f backend_postgresql.sql
```

**3. Configurer appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=amis_du_malade;Username=postgres;Password=VOTRE_MOT_DE_PASSE"
  },
  "Jwt": {
    "Key": "AmisduMalade_SuperSecretKey_2024_Skikda!",
    "Issuer": "AmisduMalade",
    "Audience": "AmisduMaladeApp"
  }
}
```

**4. Lancer l'API**
```bash
dotnet restore
dotnet run
```

**5. Tester**

---

## 👩‍💻 Équipe projet

| Rôle | Responsabilité |
|------|---------------|
| **Student A** - Backend | API REST, PostgreSQL, Auth, Sync endpoints |
| **Student B** - Mobile | .NET MAUI, SQLite local, Offline behavior |
| **Student C** - Dashboard | Admin Blazor, Notifications, Tests |

---

## 📅 Informations académiques

- **Établissement:** Université de Skikda
- **Département:** Informatique
- **Niveau:** Licence (L3)
- **Année:** 2025 - 2026
- **Encadreur:** DepMI2020