using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(VolunteerRegisterPage), typeof(VolunteerRegisterPage));
        Routing.RegisterRoute(nameof(CareRequestPage),       typeof(CareRequestPage));
    }
}
