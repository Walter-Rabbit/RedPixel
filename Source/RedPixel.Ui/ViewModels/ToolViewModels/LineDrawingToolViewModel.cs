using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;
using RedPixel.Core.Tools;
using RedPixel.Core;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;
using RedPixel.Ui.Views.Tools;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;
using Point = Avalonia.Point;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class LineDrawingToolViewModel : BaseViewModel
{
    private readonly LineDrawingTool _view;
    private readonly MainWindowViewModel _parentViewModel;

    [Reactive] public bool DrawingInProgress { get; set; } = false;
    [Reactive] public int Thickness { get; set; } = 1;
    [Reactive] public bool IsVisible { get; set; } = false;
    private Point _startPoint;

    public LineDrawingToolViewModel(LineDrawingTool view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }

    public void ImageClicked(int x, int y, int clickCount, Point previewPosition)
    {

        if (DrawingInProgress)
        {
            var color = LineDrawingTool.SelectedColor;
            var colorInCurrentColorSpace =
                _parentViewModel.ColorSpaceToolViewModel.SelectedColorSpace.ColorFromRgb(new Color(color.R, color.G, color.B));


            _parentViewModel.Image.DrawLine(
                (int)_startPoint.X,
                (int)_startPoint.Y,
                x, y,
                (float)color.A / 255,
                colorInCurrentColorSpace,
                Thickness);
            _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap();
            DrawingInProgress = false;
        } else if (clickCount == 2)
        {
            DrawingInProgress = true;
            _startPoint = new Point(x, y);

            _parentViewModel.StartPoint1 = new Point(previewPosition.X - 5, previewPosition.Y - 5);
            _parentViewModel.EndPoint1 = new Point(previewPosition.X + 5, previewPosition.Y + 5);
            _parentViewModel.StartPoint2 = new Point(previewPosition.X - 5, previewPosition.Y + 5);
            _parentViewModel.EndPoint2 = new Point(previewPosition.X + 5, previewPosition.Y - 5);
        }
    }
}