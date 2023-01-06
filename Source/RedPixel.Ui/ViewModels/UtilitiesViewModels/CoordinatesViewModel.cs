using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Views.Utilities;

namespace RedPixel.Ui.ViewModels.UtilitiesViewModels;

public class CoordinatesViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly Coordinates _view;

    public CoordinatesViewModel(Coordinates view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }
    
    [Reactive] public string Coordinates { get; set; }

    public void PointerMoved(int x, int y)
    {
        Coordinates = $"🖰 {x}:{y}";
    }
}