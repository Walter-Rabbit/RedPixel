using Avalonia.Controls;
using RedPixel.Ui.ViewModels.UtilitiesViewModels;
using RedPixel.Ui.Views.StatusBar;
using RedPixel.Ui.Views.Utilities;

namespace RedPixel.Ui.ViewModels.StatusBarViewModels;

public class StatusBarViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly StatusBar _view;

    public StatusBarViewModel(StatusBar view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
        
        CoordinatesViewModel = new CoordinatesViewModel(_view.Get<Coordinates>("Coordinates"), this);

    }
    
    public CoordinatesViewModel CoordinatesViewModel { get; set; }
}