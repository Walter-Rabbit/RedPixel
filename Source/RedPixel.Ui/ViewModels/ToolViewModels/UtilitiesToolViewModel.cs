using System.Globalization;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Dithering.Utilities;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class UtilitiesToolViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private UtilitiesTool _view;

    public UtilitiesToolViewModel(UtilitiesTool view, MainWindowViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
        _view = view;

        GenerateGradientCommand = ReactiveCommand.Create(GenerateGradient);
        ConvertToBlackWhiteCommand = ReactiveCommand.Create(ConvertToBlackWhite);
    }

    [Reactive] public string WidthString { get; set; } = "1920";
    [Reactive] public string HeightString { get; set; } = "1080";
    [Reactive] public bool IsVisible { get; set; } = false;

    public ReactiveCommand<Unit, Unit> GenerateGradientCommand { get; }
    public ReactiveCommand<Unit, Unit> ConvertToBlackWhiteCommand { get; }

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