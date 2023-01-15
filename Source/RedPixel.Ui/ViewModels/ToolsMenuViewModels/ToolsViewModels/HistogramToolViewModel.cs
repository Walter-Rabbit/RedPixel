using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class HistogramToolViewModel : BaseViewModel
{
    [Reactive] public bool FirstHistogramVisible { get; set; } = true;
    [Reactive] public bool SecondHistogramVisible { get; set; } = true;
    [Reactive] public bool ThirdHistogramVisible { get; set; } = true;
    [Reactive] public bool IsVisible { get; set; } = false;

    [Reactive] public ToolsMenuViewModel ToolsMenuViewModel { get; set; }
    [Reactive] public double[][] HistogramValues { get; set; }
    public HistogramToolViewModel(HistogramTool view, ToolsMenuViewModel parentViewModel)
    {
        ToolsMenuViewModel = parentViewModel;
        this.WhenAnyValue(x => x.HistogramValues)
            .Subscribe(x =>
            {
                if (x is not null)
                    view.UpdateHistograms(x);
            });

        this.WhenAnyValue(x => x.ToolsMenuViewModel.ColorSpaceToolViewModel.ColorComponents)
            .Subscribe(x =>
            {
                FirstHistogramVisible = (x & ColorComponents.First) != 0;
                SecondHistogramVisible = (x & ColorComponents.Second) != 0;
                ThirdHistogramVisible = (x & ColorComponents.Third) != 0;
            });
    }
}