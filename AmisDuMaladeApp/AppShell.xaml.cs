using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(VolunteerRegisterPage), typeof(VolunteerRegisterPage));
        Routing.RegisterRoute(nameof(CareRequestPage),        typeof(CareRequestPage));
        Routing.RegisterRoute(nameof(ContributePage),         typeof(ContributePage));
        Routing.RegisterRoute(nameof(AdminLoginPage),         typeof(AdminLoginPage));
        Routing.RegisterRoute(nameof(AdminDashboardPage),     typeof(AdminDashboardPage));
        Routing.RegisterRoute(nameof(VolunteerDetailPage),    typeof(VolunteerDetailPage));
        Routing.RegisterRoute(nameof(AboutPage),              typeof(AboutPage));
    }
}
