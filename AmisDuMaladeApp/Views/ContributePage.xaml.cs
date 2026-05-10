using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class ContributePage : ContentPage
{
    public ContributePage(ContributeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
