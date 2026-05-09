using AmisDuMaladeApp.Constants;

namespace AmisDuMaladeApp.Services;

public class LocalizationService
{
    public string CurrentLanguage { get; private set; }
    public bool IsRtl => CurrentLanguage == "ar";
    public FlowDirection FlowDirection => IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

    public event EventHandler<string>? LanguageChanged;

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        ["ar"] = new()
        {
            // App
            ["app_title"]           = "أصدقاء المريض – سكيكدة",
            // Home
            ["home_welcome"]        = "مرحباً بكم",
            ["home_subtitle"]       = "جمعية أصدقاء المريض – سكيكدة",
            ["home_volunteer_btn"]  = "تسجيل كمتطوع",
            ["home_care_btn"]       = "طلب مرافق",
            ["home_contribute_btn"] = "المساهمة في الجمعية",
            ["home_about"]          = "من نحن؟",
            ["home_about_text"]     = "جمعية أصدقاء المريض بسكيكدة تهدف إلى مرافقة المرضى وتقديم الدعم لهم ولذويهم.",
            ["admin_login"]         = "دخول الإدارة",
            // Volunteer
            ["volunteer_title"]     = "تسجيل متطوع جديد",
            ["volunteer_step1"]     = "المعلومات الشخصية",
            ["volunteer_step2"]     = "أوقات التوفر",
            ["volunteer_step3"]     = "المهارات",
            ["volunteer_step4"]     = "الوثائق والميثاق",
            ["charter_accept"]      = "أقبل ميثاق الجمعية",
            // Care Request
            ["care_request_title"]  = "طلب مرافق",
            ["care_step1"]          = "بيانات الطالب",
            ["care_step2"]          = "بيانات المريض",
            ["care_step3"]          = "تفاصيل الطلب",
            ["care_step4"]          = "نوع الخدمة",
            // Fields
            ["full_name"]           = "الاسم الكامل",
            ["phone"]               = "رقم الهاتف",
            ["email"]               = "البريد الإلكتروني",
            ["municipality"]        = "البلدية",
            ["address"]             = "العنوان",
            ["relation"]            = "صلة القرابة بالمريض",
            ["patient_name"]        = "اسم المريض",
            ["patient_age"]         = "عمر المريض",
            ["medical_condition"]   = "الحالة الطبية",
            ["medical_summary"]     = "ملخص طبي",
            ["support_summary"]     = "نوع الدعم المطلوب",
            ["priority_level"]      = "مستوى الأولوية",
            ["requested_date"]      = "تاريخ البدء المطلوب",
            ["location_details"]    = "تفاصيل الموقع",
            ["can_home_visit"]      = "زيارة منزلية",
            ["can_hospital_visit"]  = "زيارة مستشفى",
            ["can_night_presence"]  = "حضور ليلي",
            ["has_transportation"]  = "لديه وسيلة نقل",
            ["needs_transport"]     = "يحتاج دعم النقل",
            ["volunteer_category"]  = "فئة التطوع",
            // Login
            ["login_title"]         = "تسجيل دخول الإدارة",
            ["login_email"]         = "البريد الإلكتروني",
            ["login_password"]      = "كلمة المرور",
            ["login_btn"]           = "دخول",
            // Dashboard
            ["dashboard_title"]     = "لوحة التحكم",
            ["dashboard_volunteers"]= "المتطوعون",
            ["dashboard_requests"]  = "الطلبات",
            ["dashboard_patients"]  = "المرضى",
            ["dashboard_assignments"]= "التكليفات",
            ["dashboard_alerts"]    = "التنبيهات",
            ["dashboard_active"]    = "نشط",
            ["dashboard_pending"]   = "معلّق",
            ["recent_activity"]     = "آخر الأنشطة",
            ["logout"]              = "تسجيل الخروج",
            // Common
            ["next"]                = "التالي",
            ["previous"]            = "السابق",
            ["submit"]              = "إرسال",
            ["cancel"]              = "إلغاء",
            ["language"]            = "اللغة",
            ["loading"]             = "جاري التحميل...",
            ["error"]               = "حدث خطأ",
            ["success"]             = "تم بنجاح",
            ["required_field"]      = "هذا الحقل مطلوب",
            ["step"]                = "الخطوة",
            ["of"]                  = "من",
            ["confirmation_title"]  = "تأكيد الطلب",
            ["suggestions_title"]   = "المتطوعون المقترحون",
            ["refresh"]             = "تحديث",
        },
        ["fr"] = new()
        {
            ["app_title"]           = "Amis du Malade – Skikda",
            ["home_welcome"]        = "Bienvenue",
            ["home_subtitle"]       = "Association Amis du Malade – Skikda",
            ["home_volunteer_btn"]  = "Devenir bénévole",
            ["home_care_btn"]       = "Demander un accompagnant",
            ["home_contribute_btn"] = "Contribuer à l'association",
            ["home_about"]          = "Qui sommes-nous ?",
            ["home_about_text"]     = "L'association Amis du Malade de Skikda accompagne les malades et soutient leurs familles.",
            ["admin_login"]         = "Connexion Admin",
            ["volunteer_title"]     = "Inscription bénévole",
            ["volunteer_step1"]     = "Informations personnelles",
            ["volunteer_step2"]     = "Disponibilités",
            ["volunteer_step3"]     = "Compétences",
            ["volunteer_step4"]     = "Documents et charte",
            ["charter_accept"]      = "J'accepte la charte de l'association",
            ["care_request_title"]  = "Demande d'accompagnement",
            ["care_step1"]          = "Données du demandeur",
            ["care_step2"]          = "Données du patient",
            ["care_step3"]          = "Détails de la demande",
            ["care_step4"]          = "Type de service",
            ["full_name"]           = "Nom complet",
            ["phone"]               = "Téléphone",
            ["email"]               = "Email",
            ["municipality"]        = "Commune",
            ["address"]             = "Adresse",
            ["relation"]            = "Lien avec le patient",
            ["patient_name"]        = "Nom du patient",
            ["patient_age"]         = "Âge du patient",
            ["medical_condition"]   = "État médical",
            ["medical_summary"]     = "Résumé médical",
            ["support_summary"]     = "Type de soutien requis",
            ["priority_level"]      = "Niveau de priorité",
            ["requested_date"]      = "Date de début souhaitée",
            ["location_details"]    = "Détails du lieu",
            ["can_home_visit"]      = "Visite à domicile",
            ["can_hospital_visit"]  = "Visite à l'hôpital",
            ["can_night_presence"]  = "Présence nocturne",
            ["has_transportation"]  = "A un moyen de transport",
            ["needs_transport"]     = "Besoin de transport",
            ["volunteer_category"]  = "Catégorie de bénévolat",
            ["login_title"]         = "Connexion Administration",
            ["login_email"]         = "Email",
            ["login_password"]      = "Mot de passe",
            ["login_btn"]           = "Connexion",
            ["dashboard_title"]     = "Tableau de bord",
            ["dashboard_volunteers"]= "Bénévoles",
            ["dashboard_requests"]  = "Demandes",
            ["dashboard_patients"]  = "Patients",
            ["dashboard_assignments"]= "Affectations",
            ["dashboard_alerts"]    = "Alertes",
            ["dashboard_active"]    = "Actif",
            ["dashboard_pending"]   = "En attente",
            ["recent_activity"]     = "Activités récentes",
            ["logout"]              = "Déconnexion",
            ["next"]                = "Suivant",
            ["previous"]            = "Précédent",
            ["submit"]              = "Envoyer",
            ["cancel"]              = "Annuler",
            ["language"]            = "Langue",
            ["loading"]             = "Chargement...",
            ["error"]               = "Une erreur s'est produite",
            ["success"]             = "Opération réussie",
            ["required_field"]      = "Ce champ est obligatoire",
            ["step"]                = "Étape",
            ["of"]                  = "sur",
            ["confirmation_title"]  = "Confirmation",
            ["suggestions_title"]   = "Bénévoles suggérés",
            ["refresh"]             = "Actualiser",
        },
        ["en"] = new()
        {
            ["app_title"]           = "Friends of the Patient – Skikda",
            ["home_welcome"]        = "Welcome",
            ["home_subtitle"]       = "Association Amis du Malade – Skikda",
            ["home_volunteer_btn"]  = "Register as Volunteer",
            ["home_care_btn"]       = "Request a Companion",
            ["home_contribute_btn"] = "Contribute to the Association",
            ["home_about"]          = "About Us",
            ["home_about_text"]     = "The Friends of the Patient Association in Skikda accompanies patients and supports their families.",
            ["admin_login"]         = "Admin Login",
            ["volunteer_title"]     = "New Volunteer Registration",
            ["volunteer_step1"]     = "Personal Information",
            ["volunteer_step2"]     = "Availability",
            ["volunteer_step3"]     = "Skills",
            ["volunteer_step4"]     = "Documents & Charter",
            ["charter_accept"]      = "I accept the association charter",
            ["care_request_title"]  = "Companion Request",
            ["care_step1"]          = "Requester Data",
            ["care_step2"]          = "Patient Data",
            ["care_step3"]          = "Request Details",
            ["care_step4"]          = "Service Type",
            ["full_name"]           = "Full Name",
            ["phone"]               = "Phone Number",
            ["email"]               = "Email Address",
            ["municipality"]        = "Municipality",
            ["address"]             = "Address",
            ["relation"]            = "Relation to patient",
            ["patient_name"]        = "Patient Name",
            ["patient_age"]         = "Patient Age",
            ["medical_condition"]   = "Medical Condition",
            ["medical_summary"]     = "Medical Summary",
            ["support_summary"]     = "Type of Support Needed",
            ["priority_level"]      = "Priority Level",
            ["requested_date"]      = "Requested Start Date",
            ["location_details"]    = "Location Details",
            ["can_home_visit"]      = "Home visit",
            ["can_hospital_visit"]  = "Hospital visit",
            ["can_night_presence"]  = "Night presence",
            ["has_transportation"]  = "Has transportation",
            ["needs_transport"]     = "Needs transport support",
            ["volunteer_category"]  = "Volunteer Category",
            ["login_title"]         = "Admin Login",
            ["login_email"]         = "Email",
            ["login_password"]      = "Password",
            ["login_btn"]           = "Login",
            ["dashboard_title"]     = "Dashboard",
            ["dashboard_volunteers"]= "Volunteers",
            ["dashboard_requests"]  = "Requests",
            ["dashboard_patients"]  = "Patients",
            ["dashboard_assignments"]= "Assignments",
            ["dashboard_alerts"]    = "Alerts",
            ["dashboard_active"]    = "Active",
            ["dashboard_pending"]   = "Pending",
            ["recent_activity"]     = "Recent Activities",
            ["logout"]              = "Logout",
            ["next"]                = "Next",
            ["previous"]            = "Previous",
            ["submit"]              = "Submit",
            ["cancel"]              = "Cancel",
            ["language"]            = "Language",
            ["loading"]             = "Loading...",
            ["error"]               = "An error occurred",
            ["success"]             = "Operation successful",
            ["required_field"]      = "This field is required",
            ["step"]                = "Step",
            ["of"]                  = "of",
            ["confirmation_title"]  = "Confirmation",
            ["suggestions_title"]   = "Suggested Volunteers",
            ["refresh"]             = "Refresh",
        }
    };

    public LocalizationService()
    {
        CurrentLanguage = Preferences.Get(AppConstants.LanguageKey, AppConstants.DefaultLanguage);
    }

    public string Get(string key)
    {
        if (Strings.TryGetValue(CurrentLanguage, out var dict) && dict.TryGetValue(key, out var val))
            return val;
        // Fallback to Arabic
        if (Strings["ar"].TryGetValue(key, out var arVal))
            return arVal;
        return key;
    }

    public void SetLanguage(string language)
    {
        if (!Strings.ContainsKey(language)) return;
        CurrentLanguage = language;
        Preferences.Set(AppConstants.LanguageKey, language);
        LanguageChanged?.Invoke(this, language);
    }

    public List<(string Code, string Label)> AvailableLanguages => new()
    {
        ("ar", "العربية"),
        ("fr", "Français"),
        ("en", "English"),
    };
}
