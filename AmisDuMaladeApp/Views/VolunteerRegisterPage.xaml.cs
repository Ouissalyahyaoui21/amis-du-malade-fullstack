using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class VolunteerRegisterPage : ContentPage
{
    public VolunteerRegisterPage(VolunteerRegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
