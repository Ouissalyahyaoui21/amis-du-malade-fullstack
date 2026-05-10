using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage(AdminDashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AdminDashboardViewModel vm)
            await vm.LoadDataCommand.ExecuteAsync(null);
    }
}
