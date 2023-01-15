using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class GammaCorrectionToolViewModel : BaseViewModel
{
    private readonly ToolsMenuViewModel _parentViewModel;
    private readonly GammaCorrectionTool _view;
    private readonly MainWindowViewModel _imageViewModel;

    public GammaCorrectionToolViewModel(GammaCorrectionTool view, ToolsMenuViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
        _view = view;
        _imageViewModel = _parentViewModel.ParentViewModel;
    }

    [Reactive] public string GammaValueString { get; set; } = "1";
    [Reactive] public float GammaValue { get; set; } = 1;
    [Reactive] public string ConvertGammaMessage { get; set; } = "Convert γ";

    [Reactive] public float IgnorePixelsPart { get; set; } = 0;

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public Unit AssignGamma()
    {
        try
        {
            var sw = new Stopwatch();
            sw.Start();
            GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
            _imageViewModel.Image.Gamma = GammaValue;
            _imageViewModel.Bitmap =
                _imageViewModel.Image?.ConvertToAvaloniaBitmap(
                    _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
            sw.Stop();
            File.AppendAllText(
                "log.txt",
                $"AssignGamma: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
        }
        catch (Exception e)
        {
            File.AppendAllText("log.txt", $"{e.Message}");
        }

        return Unit.Default;
    }

    private Unit ConvertToGamma()
    {
        try
        {
            var sw = new Stopwatch();
            sw.Start();
            GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
            _imageViewModel.Bitmap = _imageViewModel.Image?.ConvertToGamma(GammaValue)
                .ConvertToAvaloniaBitmap(_parentViewModel.ColorSpaceToolViewModel.ColorComponents);
            sw.Stop();
            File.AppendAllText(
                "log.txt",
                $"ConvertToGamma: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
        }
        catch (Exception e)
        {
            File.AppendAllText("log.txt", $"{e.Message}");
        }

        return Unit.Default;
    }

    private Unit AdjustContrast()
    {
        _imageViewModel.Image.ApplyContrastAdjustment(IgnorePixelsPart);
        _imageViewModel.Bitmap = _imageViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);

        return Unit.Default;
    }

    public void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
    {
        ConvertGammaMessage = $"Convert γ to {e.NewValue}";
    }
}