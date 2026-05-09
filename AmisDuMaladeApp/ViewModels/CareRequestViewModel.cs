using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class CareRequestViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── Navigation ───────────────────────────────────────────────────────────
    [ObservableProperty] private int currentStep = 1;

    // ── Step 1 — Requester ───────────────────────────────────────────────────
    [ObservableProperty] private string requesterName = "";
    [ObservableProperty] private string requesterPhone = "";
    [ObservableProperty] private string requesterRelation = "";

    // ── Step 2 — Patient ────────────────────────────────────────────────────
    [ObservableProperty] private string patientName = "";
    [ObservableProperty] private string patientAge = "";
    [ObservableProperty] private string medicalCondition = "";

    // ── Step 3 — Details ────────────────────────────────────────────────────
    [ObservableProperty] private string municipality = "";
    [ObservableProperty] private string locationDetails = "";
    [ObservableProperty] private DateTime requestedDate = DateTime.Today.AddDays(1);
    [ObservableProperty] private string priorityLevel = "Normal";
    [ObservableProperty] private string medicalSummary = "";

    // ── Step 4 — Service type ────────────────────────────────────────────────
    [ObservableProperty] private bool needsHomeVisit = true;
    [ObservableProperty] private bool needsHospitalVisit;
    [ObservableProperty] private bool needsNightPresence;
    [ObservableProperty] private bool needsTransportSupport;
    [ObservableProperty] private string supportSummary = "";

    // Submitted request ID for showing suggestions
    [ObservableProperty] private Guid submittedRequestId;
    [ObservableProperty] private bool showSuggestions;
    [ObservableProperty] private List<VolunteerSuggestion> suggestions = new();

    // ── Labels ───────────────────────────────────────────────────────────────
    public string TitleLabel        => Loc.Get("care_request_title");
    public string Step1Label        => Loc.Get("care_step1");
    public string Step2Label        => Loc.Get("care_step2");
    public string Step3Label        => Loc.Get("care_step3");
    public string Step4Label        => Loc.Get("care_step4");
    public string NextLabel         => Loc.Get("next");
    public string PrevLabel         => Loc.Get("previous");
    public string SubmitLabel       => Loc.Get("submit");
    public string SuggestionsTitle  => Loc.Get("suggestions_title");

    public bool IsStep1   => CurrentStep == 1;
    public bool IsStep2   => CurrentStep == 2;
    public bool IsStep3   => CurrentStep == 3;
    public bool IsStep4   => CurrentStep == 4;
    public bool CanGoNext => CurrentStep < 4;
    public bool CanGoPrev => CurrentStep > 1;
    public bool CanSubmit => CurrentStep == 4;

    public string StepIndicator =>
        $"{Loc.Get("step")} {CurrentStep} {Loc.Get("of")} 4";

    public CareRequestViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
        Loc.LanguageChanged += (_, _) => RefreshLabels();
    }

    [RelayCommand]
    private void NextStep()
    {
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
        if (string.IsNullOrWhiteSpace(RequesterName) || string.IsNullOrWhiteSpace(RequesterPhone))
        {
            await ShowErrorAsync(Loc.Get("required_field"));
            return;
        }

        IsBusy = true;
        try
        {
            // First create the patient record
            var patientRequest = new CreatePatientModel
            {
                FullName = PatientName,
                Municipality = Municipality,
                MedicalNotes = MedicalCondition
            };
            var (patientCreated, patientId) = await _api.CreatePatientAsync(patientRequest);

            if (!patientCreated)
            {
                await ShowErrorAsync(Loc.Get("error"));
                return;
            }

            // Then submit the care request
            var request = new CreateCareRequestModel
            {
                PatientId = patientId,
                Municipality = Municipality,
                LocationDetails = LocationDetails,
                RequestedStartDate = RequestedDate,
                PriorityLevel = PriorityLevel,
                NeedsNightPresence = NeedsNightPresence,
                NeedsTransportSupport = NeedsTransportSupport,
                MedicalSummary = MedicalSummary,
                SupportSummary = SupportSummary,
                CareLocationType = NeedsHospitalVisit ? "Hospital" : "Home"
            };

            var (success, requestId) = await _api.CreateCareRequestAsync(request);
            if (success)
            {
                SubmittedRequestId = requestId;
                // Load volunteer suggestions
                Suggestions = await _api.GetSuggestionsAsync(requestId);
                ShowSuggestions = true;
            }
            else
                await ShowErrorAsync(Loc.Get("error"));
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

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

    private void RefreshLabels()
    {
        OnPropertyChanged(nameof(TitleLabel));
        OnPropertyChanged(nameof(Step1Label));
        OnPropertyChanged(nameof(Step2Label));
        OnPropertyChanged(nameof(Step3Label));
        OnPropertyChanged(nameof(Step4Label));
        OnPropertyChanged(nameof(NextLabel));
        OnPropertyChanged(nameof(PrevLabel));
        OnPropertyChanged(nameof(SubmitLabel));
        OnPropertyChanged(nameof(StepIndicator));
        OnPropertyChanged(nameof(FlowDirection));
    }
}
