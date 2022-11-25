using System.Globalization;
using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class UtilitiesToolViewModel
{
    private UtilitiesTool _view;
    [Reactive] public bool IsVisible { get; set; } = false;
    
    [Reactive] public string WidthString { get; set; } = "1";
    [Reactive] public string HeightString { get; set; } = "1";
    
    
    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;
    public UtilitiesToolViewModel(UtilitiesTool view, MainWindowViewModel parentViewModel)
    {
        _view = view;
    }
}