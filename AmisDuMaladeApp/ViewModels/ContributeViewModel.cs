using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class ContributeViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── Wizard state ─────────────────────────────────────────────────────────
    [ObservableProperty] private int    currentStep = 2;
    [ObservableProperty] private bool   isSuccess;
    [ObservableProperty] private string referenceNumber = "";

    public bool IsStep2      => CurrentStep == 2 && !IsSuccess;
    public bool IsStep3      => CurrentStep == 3 && !IsSuccess;
    public bool ShowStep2Nav => CurrentStep == 2 && !IsSuccess;
    public bool ShowStep3Nav => CurrentStep == 3 && !IsSuccess;

    // ── Step indicator ───────────────────────────────────────────────────────
    public string Sc2Text  => CurrentStep > 2 || IsSuccess ? "✓" : "2";
    public string Sc3Text  => IsSuccess ? "✓" : "3";
    public Color  Sc1Bg    => Color.FromArgb("#1a3a5c");
    public Color  Sc2Bg    => IsSuccess || CurrentStep > 2 ? Color.FromArgb("#1a3a5c") :
                              CurrentStep == 2             ? Color.FromArgb("#c8a53a") :
                                                             Color.FromArgb("#d1d5db");
    public Color  Sc3Bg    => IsSuccess ? Color.FromArgb("#1a3a5c") :
                              CurrentStep == 3 ? Color.FromArgb("#c8a53a") :
                                                 Color.FromArgb("#d1d5db");
    public Color  Line12   => Color.FromArgb("#1a3a5c");
    public Color  Line23   => CurrentStep > 2 || IsSuccess ? Color.FromArgb("#1a3a5c") : Color.FromArgb("#d1d5db");
    public Color  Sc2TextColor => CurrentStep == 2 && !IsSuccess ? Colors.White : Colors.White;
    public Color  Sc3TextColor => Colors.White;

    // ── Section 1 — نوع المساهمة ─────────────────────────────────────────────
    public ObservableCollection<SelectableItem> ContribTypes { get; } = new()
    {
        new() { Key = "inkind",   Label = "هدية عينية",    Icon = "🎁" },
        new() { Key = "money",    Label = "مساهمة مالية",   Icon = "💳" },
        new() { Key = "office",   Label = "زيارة المكتب",   Icon = "🏛️" },
        new() { Key = "patient",  Label = "لمريض محدد",     Icon = "🤲" },
    };

    public string SelectedTypeLabel =>
        ContribTypes.FirstOrDefault(t => t.IsSelected)?.Label ?? "—";
    public string SelectedTypeKey =>
        ContribTypes.FirstOrDefault(t => t.IsSelected)?.Key ?? "";

    // ── Section 2 — فئة النشاط ──────────────────────────────────────────────
    public ObservableCollection<SelectableItem> ActivityCategories { get; } = new()
    {
        new() { Key = "equipment", Label = "التجهيزات الطبية" },
        new() { Key = "care",      Label = "رعاية المرضى",    IsSelected = true },
        new() { Key = "general",   Label = "عام للجمعية" },
        new() { Key = "training",  Label = "تدريب المتطوعين" },
    };

    public string SelectedCategoryLabel =>
        string.Join("، ", ActivityCategories.Where(c => c.IsSelected).Select(c => c.Label));

    // ── Section 3 — المبلغ ───────────────────────────────────────────────────
    public ObservableCollection<SelectableItem> AmountPresets { get; } = new()
    {
        new() { Key = "500",   Label = "500 دج" },
        new() { Key = "1000",  Label = "1000 دج", IsSelected = true },
        new() { Key = "2000",  Label = "2000 دج" },
        new() { Key = "5000",  Label = "5000 دج" },
        new() { Key = "10000", Label = "10000 دج" },
        new() { Key = "other", Label = "آخر..." },
    };

    [ObservableProperty] private string amountOrDescription = "1000";

    // ── Section 4 — طريقة المساهمة ──────────────────────────────────────────
    public ObservableCollection<SelectableItem> PaymentMethods { get; } = new()
    {
        new() { Key = "ccp",   Label = "تحويل بريدي (Virement CCP)",    IsSelected = true },
        new() { Key = "post",  Label = "دفع في مكتب البريد (Versement)" },
        new() { Key = "assoc", Label = "زيارة مكتب الجمعية مباشرة" },
    };

    public string SelectedMethodLabel =>
        PaymentMethods.FirstOrDefault(m => m.IsSelected)?.Label ?? "—";
    public bool IsCcpSelected   => PaymentMethods.FirstOrDefault(m => m.IsSelected)?.Key == "ccp";
    public bool IsAssocSelected => PaymentMethods.FirstOrDefault(m => m.IsSelected)?.Key == "assoc";

    // ── Section 5 — رفع الوصل ───────────────────────────────────────────────
    [ObservableProperty] private string receiptFileName = "";
    public bool HasReceipt => !string.IsNullOrEmpty(ReceiptFileName);

    // ── Step 3 — بيانات المتبرع ──────────────────────────────────────────────
    [ObservableProperty] private string donorName  = "";
    [ObservableProperty] private string donorPhone = "";
    [ObservableProperty] private string notes      = "";

    // ── Summary ──────────────────────────────────────────────────────────────
    public string SummaryType     => SelectedTypeLabel;
    public string SummaryCategory => SelectedCategoryLabel;
    public string SummaryAmount   => string.IsNullOrWhiteSpace(AmountOrDescription)
                                     ? "—" : AmountOrDescription + " دج";
    public string SummaryMethod   => SelectedMethodLabel;

    public ContributeViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
        // اختيار "مساهمة مالية" افتراضياً
        ContribTypes[1].IsSelected = true;
    }

    // ── اختيار النوع ────────────────────────────────────────────────────────
    [RelayCommand]
    private void SelectType(SelectableItem item)
    {
        foreach (var t in ContribTypes) t.IsSelected = false;
        item.IsSelected = true;
        OnPropertyChanged(nameof(SelectedTypeLabel));
        OnPropertyChanged(nameof(SelectedTypeKey));
        OnPropertyChanged(nameof(SummaryType));
    }

    // ── تبديل فئة النشاط ────────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleCategory(SelectableItem item)
    {
        item.IsSelected = !item.IsSelected;
        OnPropertyChanged(nameof(SelectedCategoryLabel));
        OnPropertyChanged(nameof(SummaryCategory));
    }

    // ── اختيار مبلغ مسبق ────────────────────────────────────────────────────
    [RelayCommand]
    private void SelectPreset(SelectableItem item)
    {
        foreach (var a in AmountPresets) a.IsSelected = false;
        item.IsSelected = true;
        if (item.Key != "other") AmountOrDescription = item.Key;
        else AmountOrDescription = "";
        OnPropertyChanged(nameof(SummaryAmount));
    }

    // ── اختيار طريقة الدفع ──────────────────────────────────────────────────
    [RelayCommand]
    private void SelectPayment(SelectableItem item)
    {
        foreach (var m in PaymentMethods) m.IsSelected = false;
        item.IsSelected = true;
        OnPropertyChanged(nameof(SelectedMethodLabel));
        OnPropertyChanged(nameof(IsCcpSelected));
        OnPropertyChanged(nameof(IsAssocSelected));
        OnPropertyChanged(nameof(SummaryMethod));
    }

    // ── رفع الوصل ────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task PickReceipt()
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
                PickerTitle = "رفع وصل الاستلام"
            });
            if (result != null)
            {
                ReceiptFileName = result.FileName;
                OnPropertyChanged(nameof(HasReceipt));
            }
        }
        catch { /* cancelled */ }
    }

    // ── التنقل ───────────────────────────────────────────────────────────────
    [RelayCommand]
    private void NextStep()
    {
        if (CurrentStep == 2)
        {
            CurrentStep = 3;
            NotifyStep();
        }
    }

    [RelayCommand]
    private void PrevStep()
    {
        if (CurrentStep == 3)
        {
            CurrentStep = 2;
            NotifyStep();
        }
    }

    // ── إرسال ────────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(DonorName) || string.IsNullOrWhiteSpace(DonorPhone))
        {
            await ShowErrorAsync(Loc.Get("required_field"));
            return;
        }
        IsBusy = true;
        try
        {
            var typeMap = SelectedTypeKey switch
            {
                "inkind"  => "Goods",
                "office"  => "Time",
                "patient" => "Goods",
                _         => "Money"
            };

            decimal? amount = null;
            if (typeMap == "Money" && decimal.TryParse(AmountOrDescription, out var parsed))
                amount = parsed;

            var payload = new AmisDuMaladeApp.Models.ContributionPayload
            {
                ContributorName = DonorName.Trim(),
                Phone           = DonorPhone.Trim(),
                Type            = typeMap,
                Amount          = amount,
                Description     = typeMap != "Money" ? AmountOrDescription : null,
                Message         = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
            };

            var (success, id, _) = await _api.SubmitContributionAsync(payload);
            if (success)
            {
                ReferenceNumber = id != null ? $"#{id.Substring(0, 8).ToUpper()}" : "#----";
                IsSuccess = true;
                NotifyStep();
                OnPropertyChanged(nameof(IsSuccess));
                OnPropertyChanged(nameof(ShowStep3Nav));
            }
            else
            {
                await ShowErrorAsync("حدث خطأ أثناء الإرسال. تحقق من اتصالك.");
            }
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GoHome() =>
        await Shell.Current.GoToAsync("//HomePage");

    // ── مساعد ────────────────────────────────────────────────────────────────
    private void NotifyStep()
    {
        OnPropertyChanged(nameof(IsStep2));   OnPropertyChanged(nameof(IsStep3));
        OnPropertyChanged(nameof(ShowStep2Nav)); OnPropertyChanged(nameof(ShowStep3Nav));
        OnPropertyChanged(nameof(Sc2Text));   OnPropertyChanged(nameof(Sc3Text));
        OnPropertyChanged(nameof(Sc2Bg));     OnPropertyChanged(nameof(Sc3Bg));
        OnPropertyChanged(nameof(Line23));
    }
}
