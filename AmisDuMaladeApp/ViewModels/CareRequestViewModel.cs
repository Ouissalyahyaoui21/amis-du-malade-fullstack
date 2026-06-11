using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class CareRequestViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── Wizard state ─────────────────────────────────────────────────────────
    [ObservableProperty] private int currentStep = 1;
    [ObservableProperty] private bool isSuccess;
    [ObservableProperty] private string referenceNumber = "";

    // ── Step 1 — Requester ───────────────────────────────────────────────────
    [ObservableProperty] private string requesterName = "";
    [ObservableProperty] private string requesterPhone = "";
    [ObservableProperty] private string cityDistrict = "";

    public ObservableCollection<SelectableItem> Relations { get; } = new()
    {
        new() { Key = "spouse",   Label = "الزوج / الزوجة" },
        new() { Key = "parent",   Label = "الأب / الأم" },
        new() { Key = "child",    Label = "الابن / البنت" },
        new() { Key = "sibling",  Label = "الأخ / الأخت" },
        new() { Key = "grandp",   Label = "الجد / الجدة" },
        new() { Key = "uncle",    Label = "عم / خال" },
        new() { Key = "friend",   Label = "صديق مقرب" },
        new() { Key = "neighbor", Label = "جار" },
        new() { Key = "other",    Label = "أخرى" },
    };

    public string SelectedRelationLabel =>
        Relations.FirstOrDefault(r => r.IsSelected)?.Label ?? "";

    // ── Step 2 — Patient ─────────────────────────────────────────────────────
    [ObservableProperty] private string patientName = "";
    [ObservableProperty] private string patientAge = "";
    [ObservableProperty] private string patientGender = "ذكر";
    [ObservableProperty] private string detailedAddress = "";
    [ObservableProperty] private string patientDescription = "";

    public List<string> Genders { get; } = new() { "ذكر", "أنثى" };

    public ObservableCollection<SelectableItem> InsuranceTypes { get; } = new()
    {
        new() { Key = "cnas",   Label = "وظيف عمومي (CNAS)" },
        new() { Key = "casnos", Label = "عمال مؤسسة خاصة (CASNOS)" },
        new() { Key = "cnr",    Label = "متقاعد (CNR)" },
        new() { Key = "mesrs",  Label = "طالب (MESRS)" },
        new() { Key = "cnmss",  Label = "عسكري / أمن (CNMSS)" },
        new() { Key = "cnma",   Label = "فلاح (CNMA)" },
        new() { Key = "none",   Label = "بدون تأمين" },
    };

    public ObservableCollection<SelectableItem> HealthConditions { get; } = new()
    {
        new() { Key = "diabetes",  Label = "مريض بالسكري" },
        new() { Key = "heart",     Label = "أمراض القلب" },
        new() { Key = "paralysis", Label = "شلل / إعاقة حركية" },
        new() { Key = "cancer",    Label = "مريض بالسرطان" },
        new() { Key = "post_op",   Label = "بعد عملية جراحية" },
        new() { Key = "elderly",   Label = "مسنّ يحتاج رعاية" },
        new() { Key = "mental",    Label = "مريض نفسي" },
        new() { Key = "kidney",    Label = "أمراض الكلى" },
        new() { Key = "accident",  Label = "حوادث وإصابات" },
        new() { Key = "other",     Label = "أخرى" },
    };

    public string SelectedInsuranceLabel =>
        InsuranceTypes.FirstOrDefault(i => i.IsSelected)?.Label ?? "";
    public bool HasInsurance =>
        InsuranceTypes.Any(i => i.IsSelected && i.Key != "none");
    public string SelectedInsuranceKey =>
        InsuranceTypes.FirstOrDefault(i => i.IsSelected)?.Key ?? "";

    // ── Step 3 — Details ─────────────────────────────────────────────────────
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private bool needsNightPresence;

    public ObservableCollection<SelectableItem> Locations { get; } = new()
    {
        new() { Key = "home",         Label = "المنزل" },
        new() { Key = "hospital",     Label = "المستشفى العمومي" },
        new() { Key = "clinic",       Label = "العيادة الخاصة" },
        new() { Key = "elderly_home", Label = "دار المسنين" },
        new() { Key = "other",        Label = "مكان آخر" },
    };

    public ObservableCollection<SelectableItem> Durations { get; } = new()
    {
        new() { Key = "1day",    Label = "يوم واحد" },
        new() { Key = "2_3days", Label = "2-3 أيام" },
        new() { Key = "1week",   Label = "أسبوع" },
        new() { Key = "2weeks",  Label = "أسبوعان" },
        new() { Key = "1month",  Label = "شهر" },
        new() { Key = "more",    Label = "أكثر من شهر" },
    };

    public string SelectedLocationLabel =>
        Locations.FirstOrDefault(l => l.IsSelected)?.Label ?? "—";
    public string SelectedDurationLabel =>
        Durations.FirstOrDefault(d => d.IsSelected)?.Label ?? "—";

    // ── Step 4 — Qualifications (optional) ───────────────────────────────────
    [ObservableProperty] private string companionNotes = "";

    public ObservableCollection<SelectableItem> RequiredQualifications { get; } = new()
    {
        new() { Key = "nursing",     Label = "مهارات تمريض اساسية",     Icon = "💉" },
        new() { Key = "hygiene",     Label = "العناية الشخصية والنظافة", Icon = "🧴" },
        new() { Key = "mobility",    Label = "مساعدة على الحركة",        Icon = "♿" },
        new() { Key = "psych",       Label = "الدعم النفسي والتواصل",    Icon = "💬" },
        new() { Key = "meds",        Label = "متابعة الأدوية",           Icon = "💊" },
        new() { Key = "female_only", Label = "مرافقة أنثى فقط",          Icon = "👩" },
        new() { Key = "male_only",   Label = "مرافق ذكر فقط",            Icon = "👨" },
        new() { Key = "food",        Label = "إعداد الطعام",             Icon = "🍜" },
        new() { Key = "transport",   Label = "نقل وتنقّل",               Icon = "🚗" },
        new() { Key = "dialect",     Label = "يتحدث اللهجة المحلية",     Icon = "🗣️" },
        new() { Key = "teaching",    Label = "تدريس",                    Icon = "📚" },
    };

    // ── Summary (shown in step 4) ─────────────────────────────────────────────
    public string SummaryRequester =>
        string.IsNullOrWhiteSpace(RequesterName) ? "—" :
        $"{RequesterName}{(string.IsNullOrWhiteSpace(SelectedRelationLabel) ? "" : $" ({SelectedRelationLabel})")}";
    public string SummaryPatient =>
        string.IsNullOrWhiteSpace(PatientName) ? "—" : $"{PatientName}، {PatientAge} سنة";
    public string SummaryInsurance =>
        string.IsNullOrWhiteSpace(SelectedInsuranceLabel) ? "—" : SelectedInsuranceLabel;
    public string SummaryLocation => SelectedLocationLabel;
    public string SummaryDuration => SelectedDurationLabel;

    // ── Step visibility ──────────────────────────────────────────────────────
    public bool IsStep1       => CurrentStep == 1 && !IsSuccess;
    public bool IsStep2       => CurrentStep == 2 && !IsSuccess;
    public bool IsStep3       => CurrentStep == 3 && !IsSuccess;
    public bool IsStep4       => CurrentStep == 4 && !IsSuccess;
    public bool ShowSingleNav => CurrentStep == 1 && !IsSuccess;
    public bool ShowDoubleNav => (CurrentStep == 2 || CurrentStep == 3) && !IsSuccess;
    public bool ShowSubmitNav => CurrentStep == 4 && !IsSuccess;

    // ── Step indicator ───────────────────────────────────────────────────────
    public string Step1Icon => CurrentStep > 1 || IsSuccess ? "✓" : "👤";
    public string Step2Icon => CurrentStep > 2 || IsSuccess ? "✓" : "🏥";
    public string Step3Icon => CurrentStep > 3 || IsSuccess ? "✓" : "📋";
    public string Step4Icon => IsSuccess ? "✓" : "⭐";

    public Color Step1Bg => IsSuccess || CurrentStep > 1 ? Color.FromArgb("#16a34a") :
                            CurrentStep == 1 ? Colors.White : Color.FromArgb("#6b7280");
    public Color Step2Bg => IsSuccess || CurrentStep > 2 ? Color.FromArgb("#16a34a") :
                            CurrentStep == 2 ? Colors.White : Color.FromArgb("#6b7280");
    public Color Step3Bg => IsSuccess || CurrentStep > 3 ? Color.FromArgb("#16a34a") :
                            CurrentStep == 3 ? Colors.White : Color.FromArgb("#6b7280");
    public Color Step4Bg => IsSuccess ? Color.FromArgb("#16a34a") :
                            CurrentStep == 4 ? Colors.White : Color.FromArgb("#6b7280");

    public Color Step1TextColor => CurrentStep == 1 && !IsSuccess ? Color.FromArgb("#1a3a5c") : Colors.White;
    public Color Step2TextColor => CurrentStep == 2 && !IsSuccess ? Color.FromArgb("#1a3a5c") : Colors.White;
    public Color Step3TextColor => CurrentStep == 3 && !IsSuccess ? Color.FromArgb("#1a3a5c") : Colors.White;
    public Color Step4TextColor => CurrentStep == 4 && !IsSuccess ? Color.FromArgb("#1a3a5c") : Colors.White;

    public Color Line12Color => CurrentStep > 1 || IsSuccess ? Colors.White : Color.FromArgb("#ffffff50");
    public Color Line23Color => CurrentStep > 2 || IsSuccess ? Colors.White : Color.FromArgb("#ffffff50");
    public Color Line34Color => CurrentStep > 3 || IsSuccess ? Colors.White : Color.FromArgb("#ffffff50");

    public CareRequestViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
    }

    // ── Navigation ───────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task GoHome() =>
        await Shell.Current.GoToAsync("//HomePage");

    [RelayCommand]
    private void NextStep()
    {
        if (!ValidateStep()) return;
        if (CurrentStep < 4) { CurrentStep++; NotifyStep(); }
    }

    [RelayCommand]
    private void PrevStep()
    {
        if (CurrentStep > 1) { CurrentStep--; NotifyStep(); }
    }

    // ── Single-select commands ────────────────────────────────────────────────
    [RelayCommand]
    private void SelectRelation(SelectableItem item)
    {
        SelectSingle(Relations, item);
        OnPropertyChanged(nameof(SelectedRelationLabel));
        OnPropertyChanged(nameof(SummaryRequester));
    }

    [RelayCommand]
    private void SelectInsurance(SelectableItem item)
    {
        SelectSingle(InsuranceTypes, item);
        OnPropertyChanged(nameof(SelectedInsuranceLabel));
        OnPropertyChanged(nameof(HasInsurance));
        OnPropertyChanged(nameof(SelectedInsuranceKey));
        OnPropertyChanged(nameof(SummaryInsurance));
    }

    [RelayCommand]
    private void SelectLocation(SelectableItem item)
    {
        SelectSingle(Locations, item);
        OnPropertyChanged(nameof(SelectedLocationLabel));
        OnPropertyChanged(nameof(SummaryLocation));
    }

    [RelayCommand]
    private void SelectDuration(SelectableItem item)
    {
        SelectSingle(Durations, item);
        OnPropertyChanged(nameof(SelectedDurationLabel));
        OnPropertyChanged(nameof(SummaryDuration));
    }

    // ── Multi-select commands ─────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleHealth(SelectableItem item) => item.IsSelected = !item.IsSelected;

    [RelayCommand]
    private void ToggleQualification(SelectableItem item) => item.IsSelected = !item.IsSelected;

    // ── Submit ───────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task SubmitAsync()
    {
        IsBusy = true;
        try
        {
            // تحويل مفاتيح الموقع إلى نص
            var locationKey  = Locations.FirstOrDefault(l => l.IsSelected)?.Key ?? "home";
            var relationKey  = Relations.FirstOrDefault(r => r.IsSelected)?.Key ?? "";
            var healthKeys   = HealthConditions.Where(h => h.IsSelected).Select(h => h.Key).ToList();
            var qualKeys     = RequiredQualifications.Where(q => q.IsSelected).Select(q => q.Key).ToList();

            // تجميع الملاحظات الطبية
            var healthLabels = HealthConditions.Where(h => h.IsSelected)
                                               .Select(h => h.Label).ToList();
            var medicalSummary = healthLabels.Any()
                ? string.Join("، ", healthLabels)
                : null;

            var payload = new CareRequestPublicPayload
            {
                PatientName           = PatientName,
                PatientAge            = int.TryParse(PatientAge, out var age) ? age : null,
                PatientGender         = PatientGender,
                PatientMunicipality   = CityDistrict,
                RequesterName         = RequesterName,
                RequesterPhone        = RequesterPhone,
                RequesterRelation     = relationKey,
                CareLocationType      = locationKey,
                Municipality          = CityDistrict,
                RequestedStartDate    = StartDate,
                NeedsNightPresence    = NeedsNightPresence,
                NeedsTransportSupport = qualKeys.Contains("transport"),
                PriorityLevel         = "Normal",
                MedicalSummary        = medicalSummary,
                SupportSummary        = string.IsNullOrWhiteSpace(CompanionNotes) ? null : CompanionNotes,
                RequiredSkillNames    = qualKeys,
            };

            var (success, refNum, error) = await _api.SubmitCareRequestAsync(payload);

            if (success)
            {
                ReferenceNumber = refNum ?? "—";
                IsSuccess = true;
                NotifyStep();
                OnPropertyChanged(nameof(IsSuccess));
                OnPropertyChanged(nameof(ShowSingleNav));
                OnPropertyChanged(nameof(ShowDoubleNav));
                OnPropertyChanged(nameof(ShowSubmitNav));
                OnPropertyChanged(nameof(HasInsurance));
                OnPropertyChanged(nameof(SummaryRequester));
                OnPropertyChanged(nameof(SummaryPatient));
                OnPropertyChanged(nameof(SummaryInsurance));
                OnPropertyChanged(nameof(SummaryLocation));
                OnPropertyChanged(nameof(SummaryDuration));
            }
            else
            {
                await Shell.Current.DisplayAlertAsync(
                    "تعذّر إرسال الطلب",
                    "تأكد من اتصال الجهاز بالإنترنت وأن خادم التطبيق يعمل، ثم أعد المحاولة.\n\nإذا استمرت المشكلة، تواصل معنا مباشرة عبر الواتساب.",
                    "حسناً");
            }
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void NewRequest()
    {
        CurrentStep = 1;
        IsSuccess = false;
        RequesterName = ""; RequesterPhone = ""; CityDistrict = "";
        PatientName = ""; PatientAge = ""; DetailedAddress = ""; PatientDescription = "";
        StartDate = DateTime.Today; NeedsNightPresence = false; CompanionNotes = "";
        ReferenceNumber = "";
        foreach (var item in Relations.Concat(InsuranceTypes).Concat(HealthConditions)
                                      .Concat(Locations).Concat(Durations)
                                      .Concat(RequiredQualifications))
            item.IsSelected = false;
        NotifyStep();
        OnPropertyChanged(nameof(IsSuccess));
        OnPropertyChanged(nameof(ShowSingleNav));
        OnPropertyChanged(nameof(ShowDoubleNav));
        OnPropertyChanged(nameof(ShowSubmitNav));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────
    private static void SelectSingle(ObservableCollection<SelectableItem> col, SelectableItem item)
    {
        foreach (var i in col) i.IsSelected = false;
        item.IsSelected = true;
    }

    private bool ValidateStep()
    {
        if (CurrentStep == 1 &&
            (string.IsNullOrWhiteSpace(RequesterName) || string.IsNullOrWhiteSpace(RequesterPhone)))
        {
            _ = ShowErrorAsync(Loc.Get("required_field"));
            return false;
        }
        if (CurrentStep == 2 && string.IsNullOrWhiteSpace(PatientName))
        {
            _ = ShowErrorAsync(Loc.Get("required_field"));
            return false;
        }
        return true;
    }

    private void NotifyStep()
    {
        OnPropertyChanged(nameof(IsStep1));   OnPropertyChanged(nameof(IsStep2));
        OnPropertyChanged(nameof(IsStep3));   OnPropertyChanged(nameof(IsStep4));
        OnPropertyChanged(nameof(ShowSingleNav)); OnPropertyChanged(nameof(ShowDoubleNav));
        OnPropertyChanged(nameof(ShowSubmitNav));
        OnPropertyChanged(nameof(Step1Bg));   OnPropertyChanged(nameof(Step2Bg));
        OnPropertyChanged(nameof(Step3Bg));   OnPropertyChanged(nameof(Step4Bg));
        OnPropertyChanged(nameof(Step1Icon)); OnPropertyChanged(nameof(Step2Icon));
        OnPropertyChanged(nameof(Step3Icon)); OnPropertyChanged(nameof(Step4Icon));
        OnPropertyChanged(nameof(Step1TextColor)); OnPropertyChanged(nameof(Step2TextColor));
        OnPropertyChanged(nameof(Step3TextColor)); OnPropertyChanged(nameof(Step4TextColor));
        OnPropertyChanged(nameof(Line12Color)); OnPropertyChanged(nameof(Line23Color));
        OnPropertyChanged(nameof(Line34Color));
    }
}
