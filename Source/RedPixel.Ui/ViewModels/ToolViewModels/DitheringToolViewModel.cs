using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Dithering;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class DitheringToolViewModel : BaseViewModel
{
    private readonly DitheringTool _view;
    
    [Reactive] public bool IsVisible { get; set; } = false;
    [Reactive] public DitheringAlgorithms SelectedDitheringAlgorithm { get; set; }
    [Reactive] public string RString { get; set; } = "8";
    [Reactive] public string GString { get; set; } = "8";
    [Reactive] public string BString { get; set; } = "8";
    
    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;
    private readonly MainWindowViewModel _parentViewModel;
    
    public ReactiveCommand<Unit, Unit> ApplyDitheringCommand { get; }
    
    public IEnumerable<DitheringAlgorithms> AllDitheringAlgorithms { get; set; } = 
        DitheringAlgorithms.AllAlgorithms.Value;
    
    public DitheringToolViewModel(DitheringTool view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
        
        SelectedDitheringAlgorithm = DitheringAlgorithms.RawConversion;
        ApplyDitheringCommand = ReactiveCommand.Create(ApplyDithering);
        
        this.WhenAnyValue(x => x.SelectedDitheringAlgorithm)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                File.AppendAllText("log.txt", $"ApplyDithering started{Environment.NewLine}");
                SelectedDitheringAlgorithm = x;
                File.AppendAllText(
                    "log.txt",
                    $"ConvertToAvaloniaBitmap (ApplyDithering finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    public Unit ApplyDithering()
    {
        var depth = new ColorDepth(int.Parse(RString), int.Parse(GString), int.Parse(BString));
        
        SelectedDitheringAlgorithm.ApplyDithering(_parentViewModel.Image);
        _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        return Unit.Default;
    }
}