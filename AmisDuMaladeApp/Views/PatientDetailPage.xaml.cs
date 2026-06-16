using AmisDuMaladeApp.ViewModels;

namespace AmisDuMaladeApp.Views;

public partial class PatientDetailPage : ContentPage
{
    public PatientDetailPage(PatientDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
