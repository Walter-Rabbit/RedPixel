using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools.Filtering;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class FilteringToolViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly FilteringTool _view;

    public FilteringToolViewModel(FilteringTool view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;

        SelectedFilteringAlgorithm = FilteringAlgorithms.Threshold;
        ParameterName = FilteringAlgorithms.Threshold.ParameterName;
        IsParameterVisible = true;

        this.WhenAnyValue(x => x.SelectedFilteringAlgorithm)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                File.AppendAllText("log.txt", $"ApplyFiltering started{Environment.NewLine}");
                SelectedFilteringAlgorithm = x;
                ParameterName = x.ParameterName + ":";
                IsParameterVisible = x.ParameterName != "";

                File.AppendAllText(
                    "log.txt",
                    $"ConvertToAvaloniaBitmap (ApplyFiltering finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    [Reactive] public FilteringAlgorithms SelectedFilteringAlgorithm { get; set; }
    [Reactive] public float Parameter { get; set; } = 0;
    [Reactive] public string ParameterName { get; set; }
    [Reactive] public bool IsParameterVisible { get; set; }

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public IEnumerable<FilteringAlgorithms> AllFilteringAlgorithms { get; set; } =
        FilteringAlgorithms.AllAlgorithms.Value;

    public Unit ApplyFiltering()
    {
        SelectedFilteringAlgorithm.ApplyFiltering(_parentViewModel.Image, Parameter);
        _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
            _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        return Unit.Default;
    }
}