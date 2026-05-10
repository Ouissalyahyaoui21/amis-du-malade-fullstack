using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class AdminLoginPage : ContentPage
{
    public AdminLoginPage(AdminLoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
