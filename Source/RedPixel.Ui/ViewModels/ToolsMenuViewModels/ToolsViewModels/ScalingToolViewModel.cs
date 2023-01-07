using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools.Scaler;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class ScalingToolViewModel : BaseViewModel
{
    private ToolsMenuViewModel _parentViewModel;
    private ScalingTool _view;

    public ScalingToolViewModel(ScalingTool view, ToolsMenuViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
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
            _parentViewModel.ParentViewModel.Image =
                SelectedScaler.Scaler.Invoke(_parentViewModel.ParentViewModel.Image, Width, Height, null);
        }
        else
        {
            _parentViewModel.ParentViewModel.Image =
                SelectedScaler.Scaler.Invoke(_parentViewModel.ParentViewModel.Image, Width, Height,
                    new float[] { B, C });
        }
    }
}