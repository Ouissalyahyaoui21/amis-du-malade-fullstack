using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class VolunteerRegisterViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── Navigation state ────────────────────────────────────────────────────
    [ObservableProperty] private int currentStep = 1;

    // ── Step 1 — Personal info ───────────────────────────────────────────────
    [ObservableProperty] private string fullName = "";
    [ObservableProperty] private string phone = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string municipality = "";
    [ObservableProperty] private string volunteerCategory = "";

    // ── Step 2 — Availability ────────────────────────────────────────────────
    [ObservableProperty] private bool canHomeVisit = true;
    [ObservableProperty] private bool canHospitalVisit;
    [ObservableProperty] private bool canNightPresence;
    [ObservableProperty] private bool hasTransportation;

    // ── Step 4 — Charter ────────────────────────────────────────────────────
    [ObservableProperty] private bool charterAccepted;

    // ── Localized labels ─────────────────────────────────────────────────────
    public string TitleLabel           => Loc.Get("volunteer_title");
    public string Step1Label           => Loc.Get("volunteer_step1");
    public string Step2Label           => Loc.Get("volunteer_step2");
    public string Step3Label           => Loc.Get("volunteer_step3");
    public string Step4Label           => Loc.Get("volunteer_step4");
    public string FullNameLabel        => Loc.Get("full_name");
    public string PhoneLabel           => Loc.Get("phone");
    public string EmailLabel           => Loc.Get("email");
    public string MunicipalityLabel    => Loc.Get("municipality");
    public string CategoryLabel        => Loc.Get("volunteer_category");
    public string CanHomeVisitLabel    => Loc.Get("can_home_visit");
    public string CanHospitalVisitLabel=> Loc.Get("can_hospital_visit");
    public string CanNightPresenceLabel=> Loc.Get("can_night_presence");
    public string HasTransportLabel    => Loc.Get("has_transportation");
    public string CharterLabel         => Loc.Get("charter_accept");
    public string NextLabel            => Loc.Get("next");
    public string PrevLabel            => Loc.Get("previous");
    public string SubmitLabel          => Loc.Get("submit");

    // ── Step visibility ──────────────────────────────────────────────────────
    public bool IsStep1   => CurrentStep == 1;
    public bool IsStep2   => CurrentStep == 2;
    public bool IsStep3   => CurrentStep == 3;
    public bool IsStep4   => CurrentStep == 4;
    public bool CanGoNext => CurrentStep < 4;
    public bool CanGoPrev => CurrentStep > 1;
    public bool CanSubmit => CurrentStep == 4;

    public string StepIndicator =>
        $"{Loc.Get("step")} {CurrentStep} {Loc.Get("of")} 4";

    public VolunteerRegisterViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
        Loc.LanguageChanged += (_, _) => RefreshAllLabels();
    }

    [RelayCommand]
    private void NextStep()
    {
        if (!ValidateCurrentStep()) return;
        if (CurrentStep < 4) { CurrentStep++; NotifyStepChange(); }
    }

    [RelayCommand]
    private void PrevStep()
    {
        if (CurrentStep > 1) { CurrentStep--; NotifyStepChange(); }
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (!CharterAccepted)
        {
            await ShowErrorAsync(Loc.Get("charter_accept"));
            return;
        }

        IsBusy = true;
        try
        {
            var request = new VolunteerRegisterRequest
            {
                FullName = FullName,
                Phone = Phone,
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email,
                Municipality = Municipality,
                VolunteerCategory = VolunteerCategory,
                CanHomeVisit = CanHomeVisit,
                CanHospitalVisit = CanHospitalVisit,
                CanNightPresence = CanNightPresence,
                HasTransportation = HasTransportation
            };

            var (success, _) = await _api.RegisterVolunteerAsync(request);
            if (success)
            {
                await ShowSuccessAsync(Loc.Get("success"));
                await Shell.Current.GoToAsync("..");
            }
            else
                await ShowErrorAsync(Loc.Get("error"));
        }
        finally { IsBusy = false; }
    }

    private bool ValidateCurrentStep()
    {
        if (CurrentStep == 1 && (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Phone)))
        {
            ShowErrorAsync(Loc.Get("required_field"));
            return false;
        }
        return true;
    }

    private void NotifyStepChange()
    {
        OnPropertyChanged(nameof(IsStep1));
        OnPropertyChanged(nameof(IsStep2));
        OnPropertyChanged(nameof(IsStep3));
        OnPropertyChanged(nameof(IsStep4));
        OnPropertyChanged(nameof(CanGoNext));
        OnPropertyChanged(nameof(CanGoPrev));
        OnPropertyChanged(nameof(CanSubmit));
        OnPropertyChanged(nameof(StepIndicator));
    }

    private void RefreshAllLabels()
    {
        OnPropertyChanged(nameof(TitleLabel));
        OnPropertyChanged(nameof(Step1Label));
        OnPropertyChanged(nameof(Step2Label));
        OnPropertyChanged(nameof(Step3Label));
        OnPropertyChanged(nameof(Step4Label));
        OnPropertyChanged(nameof(FullNameLabel));
        OnPropertyChanged(nameof(PhoneLabel));
        OnPropertyChanged(nameof(EmailLabel));
        OnPropertyChanged(nameof(MunicipalityLabel));
        OnPropertyChanged(nameof(NextLabel));
        OnPropertyChanged(nameof(PrevLabel));
        OnPropertyChanged(nameof(SubmitLabel));
        OnPropertyChanged(nameof(StepIndicator));
        OnPropertyChanged(nameof(FlowDirection));
    }
}
