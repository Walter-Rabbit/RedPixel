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
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class FilteringToolViewModel : BaseViewModel
{
    private readonly ToolsMenuViewModel _parentViewModel;
    private readonly FilteringTool _view;
    private readonly MainWindowViewModel _imageViewModel;

    public FilteringToolViewModel(FilteringTool view, ToolsMenuViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
        _imageViewModel = _parentViewModel.ParentViewModel;

        SelectedFilteringAlgorithm = FilteringAlgorithms.Threshold;
        ParameterName = FilteringAlgorithms.Threshold.ParameterName;
        IsParameterVisible = true;
        Parameter = "1";
        MaxParameter = FilteringAlgorithms.Threshold.MaxParameter;

        this.WhenAnyValue(x => x.SelectedFilteringAlgorithm)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                File.AppendAllText("log.txt", $"Filtering algorithm changing {Environment.NewLine}");
                SelectedFilteringAlgorithm = x;
                ParameterName = x.ParameterName + ":";
                IsParameterVisible = x.ParameterName != "";
                MaxParameter = x.MaxParameter;
                Parameter = "1";

                File.AppendAllText(
                    "log.txt",
                    $"ConvertToAvaloniaBitmap (Filtering algorithm changing finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    [Reactive] public FilteringAlgorithms SelectedFilteringAlgorithm { get; set; }
    [Reactive] public string Parameter { get; set; }
    [Reactive] public string ParameterName { get; set; }
    [Reactive] public bool IsParameterVisible { get; set; }
    [Reactive] public string MaxParameter { get; set; }

    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public IEnumerable<FilteringAlgorithms> AllFilteringAlgorithms { get; set; } =
        FilteringAlgorithms.AllAlgorithms.Value;

    public Unit ApplyFiltering()
    {
        var parameter = Convert.ToSingle(Parameter, CultureInfo.InvariantCulture);

        if (_imageViewModel.SelectionViewModel.IsSelected)
        {
            var (leftTopPoint, rightBottomPoint) = GetCornerPoints();

            _imageViewModel.Image = SelectedFilteringAlgorithm.ApplyFiltering(
                _imageViewModel.Image,
                parameter,
                leftTopPoint,
                rightBottomPoint);
            _imageViewModel.Bitmap = _imageViewModel.Image.ConvertToAvaloniaBitmap(
                _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        }
        else
        {
            _imageViewModel.Image = SelectedFilteringAlgorithm.ApplyFiltering(
                _imageViewModel.Image,
                parameter,
                new Point(0, 0),
                new Point(_imageViewModel.Image.Width - 1,
                    _imageViewModel.Image.Height - 1));
            _imageViewModel.Bitmap = _imageViewModel.Image.ConvertToAvaloniaBitmap(
                _parentViewModel.ColorSpaceToolViewModel.ColorComponents);
        }

        return Unit.Default;
    }

    private (Point, Point) GetCornerPoints()
    {
        var points = new[]
        {
            _imageViewModel.SelectionViewModel.RealFirstPoint,
            _imageViewModel.SelectionViewModel.RealSecondPoint,
            _imageViewModel.SelectionViewModel.RealThirdPoint,
            _imageViewModel.SelectionViewModel.RealFourthPoint
        };

        var leftTopPoint = new Point((int) points[0].X, (int) points[0].Y);
        var rightBottomPoint = new Point((int) points[0].X, (int) points[0].Y);

        for (var i = 0; i < 4; i++)
        {
            if (points[i].X < leftTopPoint.X && points[i].Y < leftTopPoint.Y)
            {
                leftTopPoint = new Point((int) points[i].X, (int) points[i].Y);
            }

            if (points[i].X > rightBottomPoint.X && points[i].Y > rightBottomPoint.Y)
            {
                rightBottomPoint = new Point((int) points[i].X, (int) points[i].Y);
            }
        }

        return (leftTopPoint, rightBottomPoint);
    }
}