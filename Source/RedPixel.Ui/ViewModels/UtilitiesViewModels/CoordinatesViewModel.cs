using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.ViewModels.StatusBarViewModels;
using RedPixel.Ui.Views.Utilities;

namespace RedPixel.Ui.ViewModels.UtilitiesViewModels;

public class CoordinatesViewModel : BaseViewModel
{
    private readonly StatusBarViewModel _parentViewModel;
    private readonly Coordinates _view;

    public CoordinatesViewModel(Coordinates view, StatusBarViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }

    [Reactive] public string Coordinates { get; set; }
    [Reactive] public bool IsVisible { get; set; } = true;

    public void PointerMoved(int x, int y)
    {
        Coordinates = $"🖰 {x}:{y}";
    }
}