using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
