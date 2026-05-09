using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp;

public partial class App : Application
{
    public App(LocalizationService localization)
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(VolunteerRegisterPage), typeof(VolunteerRegisterPage));
        Routing.RegisterRoute(nameof(CareRequestPage), typeof(CareRequestPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

        MainPage = new AppShell();
    }
}
