using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class VolunteerRegisterPage : ContentPage
{
    public VolunteerRegisterPage(VolunteerRegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Content.Opacity = 0;
        await Task.Delay(1);
        await Content.FadeTo(1, 280, Easing.CubicOut);
    }
}
