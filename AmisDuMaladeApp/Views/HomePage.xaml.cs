using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
