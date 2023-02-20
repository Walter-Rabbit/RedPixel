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
    private readonly ToolsMenuViewModel _parentViewModel;
    private UtilitiesTool _view;
    private readonly MainWindowViewModel _imageViewModel;

    public UtilitiesToolViewModel(UtilitiesTool view, ToolsMenuViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
        _view = view;
        _imageViewModel = _parentViewModel.ParentViewModel;
    }

    [Reactive] public string WidthString { get; set; } = "1920";
    [Reactive] public string HeightString { get; set; } = "1080";

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public Unit GenerateGradient()
    {
        _parentViewModel.ParentViewModel.SaveImageToHistory();

        var bytesForColor = _imageViewModel.Image?.BytesForColor ?? 1;
        var colorSpace = _imageViewModel.Image?.ColorSpace ?? ColorSpaces.Rgb;

        if (_parentViewModel == null) return Unit.Default;
        _imageViewModel.Image = GradientGenerator.Generate(
            int.Parse(HeightString),
            int.Parse(WidthString),
            bytesForColor,
            colorSpace);

        _imageViewModel.Bitmap = _imageViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);

        return Unit.Default;
    }

    public Unit ConvertToBlackWhite()
    {
        _parentViewModel.ParentViewModel.SaveImageToHistory();

        BwConverter.ConvertToBlackAndWhite(_imageViewModel.Image);
        _imageViewModel.Bitmap = _imageViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        return Unit.Default;
    }
}