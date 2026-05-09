using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Constants;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class ContributeViewModel : BaseViewModel
{
    private readonly ApiService _api;

    // ── نوع المساهمة ────────────────────────────────────────────────────────
    [ObservableProperty] private string contributionType = "money";

    public bool IsMoney   => ContributionType == "money";
    public bool IsInKind  => ContributionType == "inkind";
    public bool IsSupport => ContributionType == "support";

    // ── بيانات المتبرع ──────────────────────────────────────────────────────
    [ObservableProperty] private string donorName  = "";
    [ObservableProperty] private string donorPhone = "";

    // ── تبرع مالي ───────────────────────────────────────────────────────────
    [ObservableProperty] private string amount = "";

    public ObservableCollection<SelectableItem> AmountPresets { get; } = new()
    {
        new() { Key = "500",   Label = "500 دج" },
        new() { Key = "1000",  Label = "1,000 دج" },
        new() { Key = "2000",  Label = "2,000 دج" },
        new() { Key = "5000",  Label = "5,000 دج" },
        new() { Key = "10000", Label = "10,000 دج" },
        new() { Key = "other", Label = "مبلغ آخر" },
    };

    // ── هدية عينية ──────────────────────────────────────────────────────────
    public ObservableCollection<SelectableItem> InKindItems { get; } = new()
    {
        new() { Key = "medicines",  Label = "أدوية",            Icon = "💊" },
        new() { Key = "food",       Label = "مواد غذائية",       Icon = "🥫" },
        new() { Key = "hygiene",    Label = "مستلزمات النظافة",  Icon = "🧴" },
        new() { Key = "clothes",    Label = "ملابس",             Icon = "👕" },
        new() { Key = "equipment",  Label = "معدات طبية",        Icon = "🩺" },
        new() { Key = "other",      Label = "أخرى",              Icon = "📦" },
    };

    [ObservableProperty] private string inKindDescription = "";

    // ── دعم متواصل ──────────────────────────────────────────────────────────
    public ObservableCollection<SelectableItem> SupportTypes { get; } = new()
    {
        new() { Key = "monthly",    Label = "تبرع شهري منتظم",   Icon = "🔄" },
        new() { Key = "sponsorship",Label = "كفالة مريض",        Icon = "🤲" },
        new() { Key = "project",    Label = "دعم مشروع",         Icon = "🏗️" },
        new() { Key = "company",    Label = "شراكة مؤسسية",      Icon = "🏢" },
    };

    [ObservableProperty] private string supportNotes = "";

    // ── نتيجة الإرسال ───────────────────────────────────────────────────────
    [ObservableProperty] private bool isSuccess;

    public ContributeViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
    }

    // ── اختيار النوع ────────────────────────────────────────────────────────
    [RelayCommand]
    private void SetType(string type)
    {
        ContributionType = type;
        OnPropertyChanged(nameof(IsMoney));
        OnPropertyChanged(nameof(IsInKind));
        OnPropertyChanged(nameof(IsSupport));
    }

    // ── مبالغ مسبقة ─────────────────────────────────────────────────────────
    [RelayCommand]
    private void SelectPreset(SelectableItem item)
    {
        foreach (var i in AmountPresets) i.IsSelected = false;
        item.IsSelected = true;
        if (item.Key != "other") Amount = item.Key;
        else Amount = "";
    }

    // ── تبديل عناصر الهدية ──────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleInKind(SelectableItem item) => item.IsSelected = !item.IsSelected;

    // ── تبديل نوع الدعم ─────────────────────────────────────────────────────
    [RelayCommand]
    private void SelectSupport(SelectableItem item)
    {
        foreach (var i in SupportTypes) i.IsSelected = false;
        item.IsSelected = true;
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
            // بناء رسالة الواتساب بدلًا من API مباشرة (مناسب للجمعيات الصغيرة)
            var msg = BuildWhatsAppMessage();
            var url = $"https://wa.me/{AppConstants.WhatsAppNumber}?text={Uri.EscapeDataString(msg)}";
            await Launcher.OpenAsync(url);
            IsSuccess = true;
            OnPropertyChanged(nameof(IsSuccess));
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GoHome() =>
        await Shell.Current.GoToAsync("//HomePage");

    // ── مساعد بناء رسالة الواتساب ────────────────────────────────────────────
    private string BuildWhatsAppMessage()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("📋 طلب مساهمة جديد — جمعية أصدقاء المريض");
        sb.AppendLine($"👤 الاسم: {DonorName}");
        sb.AppendLine($"📞 الهاتف: {DonorPhone}");

        switch (ContributionType)
        {
            case "money":
                var preset = AmountPresets.FirstOrDefault(a => a.IsSelected);
                var displayAmount = preset?.Key == "other" || preset == null ? Amount : preset.Label;
                sb.AppendLine($"💰 نوع المساهمة: تبرع مالي");
                sb.AppendLine($"💵 المبلغ: {displayAmount}");
                break;

            case "inkind":
                var items = InKindItems.Where(i => i.IsSelected).Select(i => i.Label);
                sb.AppendLine($"🎁 نوع المساهمة: هدية عينية");
                sb.AppendLine($"📦 العناصر: {string.Join("، ", items)}");
                if (!string.IsNullOrWhiteSpace(InKindDescription))
                    sb.AppendLine($"📝 تفاصيل: {InKindDescription}");
                break;

            case "support":
                var support = SupportTypes.FirstOrDefault(s => s.IsSelected);
                sb.AppendLine($"🤝 نوع المساهمة: دعم متواصل");
                if (support != null) sb.AppendLine($"🔖 الفئة: {support.Label}");
                if (!string.IsNullOrWhiteSpace(SupportNotes))
                    sb.AppendLine($"📝 ملاحظات: {SupportNotes}");
                break;
        }

        return sb.ToString();
    }
}
