using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class GammaCorrectionToolViewModel : BaseViewModel
{
    private readonly GammaCorrectionTool _view;
    private readonly MainWindowViewModel _parentViewModel;

    public ReactiveCommand<Unit, Unit> AssignGammaCommand { get; }
    public ReactiveCommand<Unit, Unit> ConvertToGammaCommand { get; }

    [Reactive] public string GammaValueString { get; set; } = "1";
    [Reactive] public float GammaValue { get; set; } = 1;
    [Reactive] public string ConvertGammaMessage { get; set; } = "Convert γ";
    [Reactive] public bool IsVisible { get; set; } = false;

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public GammaCorrectionToolViewModel(GammaCorrectionTool view, MainWindowViewModel parentViewModel)
    {
        AssignGammaCommand = ReactiveCommand.Create(AssignGamma);
        ConvertToGammaCommand = ReactiveCommand.Create(ConvertToGamma);
        _parentViewModel = parentViewModel;

        _view = view;
    }

    public Unit AssignGamma()
    {
        try
        {
            var sw = new Stopwatch();
            sw.Start();
            GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
            _parentViewModel.Image.Gamma = GammaValue;
            _parentViewModel.Bitmap =
                _parentViewModel.Image?.ConvertToAvaloniaBitmap(
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
            _parentViewModel.Bitmap = _parentViewModel.Image?.ConvertToGamma(GammaValue)
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

    public void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
    {
        ConvertGammaMessage = $"Convert γ to {e.NewValue}";
    }
}