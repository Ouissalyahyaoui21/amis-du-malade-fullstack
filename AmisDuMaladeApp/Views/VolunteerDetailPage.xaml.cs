using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class VolunteerDetailPage : ContentPage
{
    public VolunteerDetailPage(VolunteerDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
