using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class VolunteerRegisterViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── Wizard state ─────────────────────────────────────────────────────────
    [ObservableProperty] private int currentStep = 1;
    [ObservableProperty] private bool isSuccess;

    // ── Step 1 — Personal info ───────────────────────────────────────────────
    [ObservableProperty] private string fullName = "";
    [ObservableProperty] private DateTime dateOfBirth = DateTime.Today.AddYears(-25);
    [ObservableProperty] private string phone = "";
    [ObservableProperty] private string municipality = "";
    [ObservableProperty] private string profession = "";
    [ObservableProperty] private string maritalStatus = "";

    // ── Step 2 — Days ────────────────────────────────────────────────────────
    [ObservableProperty] private bool daySat;
    [ObservableProperty] private bool daySun;
    [ObservableProperty] private bool dayMon;
    [ObservableProperty] private bool dayTue;
    [ObservableProperty] private bool dayWed;
    [ObservableProperty] private bool dayThu;

    // ── Step 2 — Time slots ──────────────────────────────────────────────────
    [ObservableProperty] private bool slot0810;
    [ObservableProperty] private bool slot1012;
    [ObservableProperty] private bool slot1214;
    [ObservableProperty] private bool slot1416;
    [ObservableProperty] private bool slot1618;
    [ObservableProperty] private bool slot1820;

    // ── Step 3 — Skills ──────────────────────────────────────────────────────
    public ObservableCollection<SelectableItem> Skills { get; } = new()
    {
        new() { Key = "patient_care",  Label = "رعاية المريض",      Icon = "🩺" },
        new() { Key = "hygiene",       Label = "نظافة المريض",      Icon = "🧴" },
        new() { Key = "medical_help",  Label = "مساعدة طبية",       Icon = "💊" },
        new() { Key = "transport",     Label = "نقل المرضى",        Icon = "🚗" },
        new() { Key = "food_prep",     Label = "تحضير الطعام",      Icon = "🍽️" },
        new() { Key = "housework",     Label = "أعمال منزلية",      Icon = "🧹" },
        new() { Key = "communication", Label = "التواصل والمتابعة", Icon = "📞" },
        new() { Key = "psych_support", Label = "الدعم النفسي",      Icon = "🤗" },
        new() { Key = "translation",   Label = "الترجمة",           Icon = "🌐" },
        new() { Key = "elder_care",    Label = "رعاية المسنين",     Icon = "👴" },
    };

    // ── Step 3 — Training level ──────────────────────────────────────────────
    [ObservableProperty] private string trainingLevel = "";
    public bool IsLevelBeginner     => TrainingLevel == "beginner";
    public bool IsLevelTrainable    => TrainingLevel == "trainable";
    public bool IsLevelProficient   => TrainingLevel == "proficient";
    public bool IsLevelProfessional => TrainingLevel == "professional";

    // ── Step 3 — Previous volunteer ──────────────────────────────────────────
    [ObservableProperty] private bool? hasVolunteeredBefore;
    public bool IsYesPrev => HasVolunteeredBefore == true;
    public bool IsNoPrev  => HasVolunteeredBefore == false;

    // ── Step 4 — Documents ───────────────────────────────────────────────────
    [ObservableProperty] private string nationalIdFileName = "";
    [ObservableProperty] private string profilePhotoFileName = "";
    [ObservableProperty] private string notes = "";
    public bool HasNationalId    => !string.IsNullOrEmpty(NationalIdFileName);
    public bool HasProfilePhoto  => !string.IsNullOrEmpty(ProfilePhotoFileName);

    // ── Step visibility ──────────────────────────────────────────────────────
    public bool IsStep1       => CurrentStep == 1 && !IsSuccess;
    public bool IsStep2       => CurrentStep == 2 && !IsSuccess;
    public bool IsStep3       => CurrentStep == 3 && !IsSuccess;
    public bool IsStep4       => CurrentStep == 4 && !IsSuccess;
    public bool ShowSingleNav => CurrentStep == 1 && !IsSuccess;
    public bool ShowDoubleNav => (CurrentStep == 2 || CurrentStep == 3) && !IsSuccess;
    public bool ShowSubmitNav => CurrentStep == 4 && !IsSuccess;

    // ── Step indicator ───────────────────────────────────────────────────────
    public Color Step1Bg        => BgFor(1);
    public Color Step2Bg        => BgFor(2);
    public Color Step3Bg        => BgFor(3);
    public Color Step4Bg        => BgFor(4);
    public string Step1Text     => CurrentStep > 1 ? "✓" : "1";
    public string Step2Text     => CurrentStep > 2 ? "✓" : "2";
    public string Step3Text     => CurrentStep > 3 ? "✓" : "3";
    public string Step4Text     => IsSuccess ? "✓" : "4";
    public Color Line12Color    => CurrentStep > 1 ? Color.FromArgb("#16a34a") : Color.FromArgb("#d1d5db");
    public Color Line23Color    => CurrentStep > 2 ? Color.FromArgb("#16a34a") : Color.FromArgb("#d1d5db");
    public Color Line34Color    => CurrentStep > 3 ? Color.FromArgb("#16a34a") : Color.FromArgb("#d1d5db");

    // ── Marital status ───────────────────────────────────────────────────────
    public bool IsMaritalSingle   => MaritalStatus == "single";
    public bool IsMaritalMarried  => MaritalStatus == "married";
    public bool IsMaritalDivorced => MaritalStatus == "divorced";
    public bool IsMaritalWidowed  => MaritalStatus == "widowed";

    // ── Pickers data ─────────────────────────────────────────────────────────
    public List<string> Municipalities { get; } = new()
    {
        "سكيكدة", "عزابة", "القل", "الحروش", "أم الطوب", "زيت الدوير",
        "بن عزوز", "بوشطاطة", "جندل", "مجاز عمار", "رمضان جمال",
        "فلفلة", "قنواع", "سطورة", "سلمة", "تمالوس", "عين بوزيان",
        "بئر الدرادج", "كركرة", "خنشلة الوسط",
    };

    public List<string> Professions { get; } = new()
    {
        "طالب / طالبة", "موظف إداري", "طبيب / ممرض", "مهندس",
        "معلم / أستاذ", "تاجر", "حرفي", "متقاعد", "ربة بيت", "أخرى",
    };

    // ── Localized labels ─────────────────────────────────────────────────────
    public string TitleLabel      => Loc.Get("volunteer_title");
    public string Step1Label      => Loc.Get("volunteer_step1");
    public string Step2Label      => Loc.Get("volunteer_step2");
    public string Step3Label      => Loc.Get("volunteer_step3");
    public string Step4Label      => Loc.Get("volunteer_step4");
    public string NextLabel       => Loc.Get("next");
    public string PrevLabel       => Loc.Get("previous");
    public string SubmitLabel     => Loc.Get("vol_send_request");
    public string SuccessTitle    => Loc.Get("vol_success_title");
    public string SuccessMsg      => Loc.Get("vol_success_msg");
    public string BackHomeLabel   => Loc.Get("vol_back_home");

    public VolunteerRegisterViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
        Loc.LanguageChanged += (_, _) => RefreshLabels();
    }

    private Color BgFor(int step) =>
        IsSuccess && step <= 4 ? Color.FromArgb("#16a34a") :
        CurrentStep == step    ? Color.FromArgb("#dc2626") :
        CurrentStep > step     ? Color.FromArgb("#16a34a") :
                                 Color.FromArgb("#9ca3af");

    // ── Navigation ───────────────────────────────────────────────────────────
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

    // ── Marital status ───────────────────────────────────────────────────────
    [RelayCommand]
    private void SetMaritalStatus(string status)
    {
        MaritalStatus = status;
        OnPropertyChanged(nameof(IsMaritalSingle));
        OnPropertyChanged(nameof(IsMaritalMarried));
        OnPropertyChanged(nameof(IsMaritalDivorced));
        OnPropertyChanged(nameof(IsMaritalWidowed));
    }

    // ── Days ─────────────────────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleDay(string day)
    {
        switch (day)
        {
            case "sat": DaySat = !DaySat; break;
            case "sun": DaySun = !DaySun; break;
            case "mon": DayMon = !DayMon; break;
            case "tue": DayTue = !DayTue; break;
            case "wed": DayWed = !DayWed; break;
            case "thu": DayThu = !DayThu; break;
        }
    }

    // ── Time slots ───────────────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleSlot(string slot)
    {
        switch (slot)
        {
            case "0810": Slot0810 = !Slot0810; break;
            case "1012": Slot1012 = !Slot1012; break;
            case "1214": Slot1214 = !Slot1214; break;
            case "1416": Slot1416 = !Slot1416; break;
            case "1618": Slot1618 = !Slot1618; break;
            case "1820": Slot1820 = !Slot1820; break;
        }
    }

    // ── Skills ───────────────────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleSkill(SelectableItem item) => item.IsSelected = !item.IsSelected;

    // ── Training level ───────────────────────────────────────────────────────
    [RelayCommand]
    private void SetTrainingLevel(string level)
    {
        TrainingLevel = level;
        OnPropertyChanged(nameof(IsLevelBeginner));
        OnPropertyChanged(nameof(IsLevelTrainable));
        OnPropertyChanged(nameof(IsLevelProficient));
        OnPropertyChanged(nameof(IsLevelProfessional));
    }

    // ── Previous volunteer ───────────────────────────────────────────────────
    [RelayCommand]
    private void SetVolunteeredBefore(string val)
    {
        HasVolunteeredBefore = val == "yes";
        OnPropertyChanged(nameof(IsYesPrev));
        OnPropertyChanged(nameof(IsNoPrev));
    }

    // ── File picking ─────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task PickNationalId()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "image/*", "application/pdf" } },
                    { DevicePlatform.iOS,     new[] { "public.image", "com.adobe.pdf" } },
                }),
                PickerTitle = "بطاقة التعريف الوطنية"
            });
            if (result != null)
            {
                NationalIdFileName = result.FileName;
                OnPropertyChanged(nameof(HasNationalId));
            }
        }
        catch { /* user cancelled */ }
    }

    [RelayCommand]
    private async Task PickProfilePhoto()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "صورة شخصية"
            });
            if (result != null)
            {
                ProfilePhotoFileName = result.FileName;
                OnPropertyChanged(nameof(HasProfilePhoto));
            }
        }
        catch { /* user cancelled */ }
    }

    // ── Submit ───────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(NationalIdFileName))
        {
            await ShowErrorAsync(Loc.Get("required_field"));
            return;
        }

        IsBusy = true;
        try
        {
            var availabilities = BuildAvailabilities();
            var skills = Skills
                .Where(s => s.IsSelected)
                .Select(s => new VolunteerSkillRequest { SkillName = s.Key })
                .ToList();

            // استنتاج القدرات من المهارات المختارة والوقت
            bool hasTransport   = Skills.FirstOrDefault(s => s.Key == "transport")?.IsSelected ?? false;
            bool canNight       = Slot1820;  // متاح مساءً = يقدر حضور ليلي

            var request = new VolunteerRegisterRequest
            {
                FullName           = FullName,
                Phone              = Phone,
                Municipality       = Municipality,
                VolunteerCategory  = Profession,
                CanHomeVisit       = true,
                CanHospitalVisit   = Skills.FirstOrDefault(s => s.Key == "patient_care")?.IsSelected ?? false,
                CanNightPresence   = canNight,
                HasTransportation  = hasTransport,
                Skills             = skills,
                Availabilities     = availabilities,
                CoverageAreas      = string.IsNullOrWhiteSpace(Municipality)
                                         ? new()
                                         : new List<string> { Municipality },
            };

            var (success, _) = await _api.RegisterVolunteerAsync(request);
            if (success)
            {
                IsSuccess = true;
                NotifyStep();
                OnPropertyChanged(nameof(IsSuccess));
                OnPropertyChanged(nameof(ShowSingleNav));
                OnPropertyChanged(nameof(ShowDoubleNav));
                OnPropertyChanged(nameof(ShowSubmitNav));
            }
            else
                await ShowErrorAsync(Loc.Get("error"));
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GoHome() =>
        await Shell.Current.GoToAsync("//HomePage");

    // ── Helpers ──────────────────────────────────────────────────────────────
    private List<VolunteerAvailabilityRequest> BuildAvailabilities()
    {
        var days = new List<(bool on, string name)>
        {
            (DaySat, "Saturday"), (DaySun, "Sunday"),  (DayMon, "Monday"),
            (DayTue, "Tuesday"),  (DayWed, "Wednesday"), (DayThu, "Thursday"),
        };
        var slots = new List<(bool on, string s, string e)>
        {
            (Slot0810, "08:00", "10:00"), (Slot1012, "10:00", "12:00"),
            (Slot1214, "12:00", "14:00"), (Slot1416, "14:00", "16:00"),
            (Slot1618, "16:00", "18:00"), (Slot1820, "18:00", "20:00"),
        };

        return (from d  in days  where d.on
                from sl in slots where sl.on
                select new VolunteerAvailabilityRequest
                {
                    DayOfWeek = d.name,
                    StartTime = sl.s,
                    EndTime   = sl.e,
                }).ToList();
    }

    private bool ValidateStep()
    {
        if (CurrentStep == 1 &&
            (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Phone)))
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
        OnPropertyChanged(nameof(Step1Text)); OnPropertyChanged(nameof(Step2Text));
        OnPropertyChanged(nameof(Step3Text)); OnPropertyChanged(nameof(Step4Text));
        OnPropertyChanged(nameof(Line12Color)); OnPropertyChanged(nameof(Line23Color));
        OnPropertyChanged(nameof(Line34Color));
    }

    private void RefreshLabels()
    {
        OnPropertyChanged(nameof(TitleLabel));  OnPropertyChanged(nameof(Step1Label));
        OnPropertyChanged(nameof(Step2Label));  OnPropertyChanged(nameof(Step3Label));
        OnPropertyChanged(nameof(Step4Label));  OnPropertyChanged(nameof(NextLabel));
        OnPropertyChanged(nameof(PrevLabel));   OnPropertyChanged(nameof(SubmitLabel));
        OnPropertyChanged(nameof(SuccessTitle)); OnPropertyChanged(nameof(SuccessMsg));
        OnPropertyChanged(nameof(BackHomeLabel)); OnPropertyChanged(nameof(FlowDirection));
    }
}
