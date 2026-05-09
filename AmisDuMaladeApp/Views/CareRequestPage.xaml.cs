using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class CareRequestPage : ContentPage
{
    public CareRequestPage(CareRequestViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
