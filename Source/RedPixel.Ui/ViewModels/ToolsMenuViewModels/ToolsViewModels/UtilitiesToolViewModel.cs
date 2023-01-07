using System.Globalization;
using System.Reactive;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Tools.Utilities;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class UtilitiesToolViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private UtilitiesTool _view;

    public UtilitiesToolViewModel(UtilitiesTool view, MainWindowViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
        _view = view;
    }

    [Reactive] public string WidthString { get; set; } = "1920";
    [Reactive] public string HeightString { get; set; } = "1080";

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public Unit GenerateGradient()
    {
        var bytesForColor = _parentViewModel.Image?.BytesForColor ?? 1;
        var colorSpace = _parentViewModel.Image?.ColorSpace ?? ColorSpaces.Rgb;

        if (_parentViewModel == null) return Unit.Default;
        _parentViewModel.Image = GradientGenerator.Generate(
            int.Parse(HeightString),
            int.Parse(WidthString),
            bytesForColor,
            colorSpace);

        _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);

        return Unit.Default;
    }

    public Unit ConvertToBlackWhite()
    {
        BwConverter.ConvertToBlackAndWhite(_parentViewModel.Image);
        _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        return Unit.Default;
    }
}