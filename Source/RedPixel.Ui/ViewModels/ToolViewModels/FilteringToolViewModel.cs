using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        MaxParameter = FilteringAlgorithms.Threshold.MaxParameter;

        this.WhenAnyValue(x => x.SelectedFilteringAlgorithm)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                File.AppendAllText("log.txt", $"ApplyFiltering started{Environment.NewLine}");
                SelectedFilteringAlgorithm = x;
                ParameterName = x.ParameterName + ":";
                IsParameterVisible = x.ParameterName != "";
                MaxParameter = x.MaxParameter;

                File.AppendAllText(
                    "log.txt",
                    $"ConvertToAvaloniaBitmap (ApplyFiltering finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    [Reactive] public FilteringAlgorithms SelectedFilteringAlgorithm { get; set; }
    [Reactive] public string Parameter { get; set; } = "0";
    [Reactive] public string ParameterName { get; set; }
    [Reactive] public bool IsParameterVisible { get; set; }
    [Reactive] public string MaxParameter { get; set; }

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public IEnumerable<FilteringAlgorithms> AllFilteringAlgorithms { get; set; } =
        FilteringAlgorithms.AllAlgorithms.Value;

    public Unit ApplyFiltering()
    {
        var parameter = Convert.ToSingle(Parameter, CultureInfo.InvariantCulture);

        if (_parentViewModel.SelectionViewModel.IsSelected)
        {
            var (leftTopPoint, rightBottomPoint) = GetCornerPoints();

            _parentViewModel.Image = SelectedFilteringAlgorithm.ApplyFiltering(
                _parentViewModel.Image,
                parameter,
                leftTopPoint,
                rightBottomPoint);
            _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
                _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        }
        else
        {
            _parentViewModel.Image = SelectedFilteringAlgorithm.ApplyFiltering(
                _parentViewModel.Image,
                parameter,
                new Point(0, 0),
                new Point(_parentViewModel.Image.Width - 1, _parentViewModel.Image.Height - 1));
            _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap(
                _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        }

        return Unit.Default;
    }

    private (Point, Point) GetCornerPoints()
    {
        var points = new[]
        {
            _parentViewModel.SelectionViewModel.RealFirstPoint,
            _parentViewModel.SelectionViewModel.RealSecondPoint,
            _parentViewModel.SelectionViewModel.RealThirdPoint,
            _parentViewModel.SelectionViewModel.RealFourthPoint
        };

        var leftTopPoint = new Point((int)points[0].X, (int)points[0].Y);
        var rightBottomPoint = new Point((int)points[0].X, (int)points[0].Y);

        for (var i = 0; i < 4; i++)
        {
            if (points[i].X < leftTopPoint.X && points[i].Y < leftTopPoint.Y)
            {
                leftTopPoint = new Point((int)points[i].X, (int)points[i].Y);
            }

            if (points[i].X > rightBottomPoint.X && points[i].Y > rightBottomPoint.Y)
            {
                rightBottomPoint = new Point((int)points[i].X, (int)points[i].Y);
            }
        }

        return (leftTopPoint, rightBottomPoint);
    }
}