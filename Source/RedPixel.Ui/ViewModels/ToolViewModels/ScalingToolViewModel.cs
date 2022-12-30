using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools.Scaler;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class ScalingToolViewModel : BaseViewModel
{
    private MainWindowViewModel _mainWindowViewModel;
    private ScalingTool _view;

    public ScalingToolViewModel(ScalingTool view, MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _view = view;

        this.WhenAnyValue(x => x.SelectedScaler)
            .Subscribe(x => { IsBcSplines = x.Name == "BC Splines"; });
    }

    public List<ImageScaler> AllImageScalers { get; set; } = ImageScaler.All.Value.ToList();
    [Reactive] public ImageScaler SelectedScaler { get; set; } = ImageScaler.All.Value.First();
    public int Width { get; set; }
    public int Height { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    [Reactive] public bool IsBcSplines { get; set; } = false;

    public void Scale()
    {
        if (SelectedScaler.Name != "BC Splines")
        {
            _mainWindowViewModel.Image = SelectedScaler.Scaler.Invoke(_mainWindowViewModel.Image, Width, Height, null);
        }
        else
        {
            _mainWindowViewModel.Image = SelectedScaler.Scaler.Invoke(_mainWindowViewModel.Image, Width, Height, new float[]{ B, C});
        }
    }
}