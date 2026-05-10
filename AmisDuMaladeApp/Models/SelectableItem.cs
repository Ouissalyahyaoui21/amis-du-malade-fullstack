using CommunityToolkit.Mvvm.ComponentModel;

namespace AmisDuMaladeApp.Models;

public partial class SelectableItem : ObservableObject
{
    [ObservableProperty] private bool isSelected;
    public string Key   { get; init; } = "";
    public string Label { get; init; } = "";
    public string Icon  { get; init; } = "";
}
