using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class HistogramToolViewModel : BaseViewModel
{
    [Reactive] public MainWindowViewModel MainWindowViewModel { get; set; }
    [Reactive] public double[][] HistogramValues { get; set; }

    public HistogramToolViewModel(HistogramTool view, MainWindowViewModel parentViewModel)
    {
        MainWindowViewModel = parentViewModel;
        this.WhenAnyValue(x => x.HistogramValues)
            .Subscribe(x =>
            {
                if (x is not null)
                    view.UpdateHistograms(x, parentViewModel.ColorSpaceToolViewModel.ColorComponents);
            });
    }
}