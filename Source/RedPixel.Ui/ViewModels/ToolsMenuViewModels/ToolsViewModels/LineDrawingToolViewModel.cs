using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.ToolsMenu.Tools;
using RedPixelColor = RedPixel.Core.Colors.ValueObjects.Color;
using AvaloniaColor = Avalonia.Media.Color;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class LineDrawingToolViewModel : BaseViewModel
{
    private readonly ToolsMenuViewModel _parentViewModel;
    private readonly LineDrawingTool _view;
    private Point _startPoint;

    public LineDrawingToolViewModel(LineDrawingTool view, ToolsMenuViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;

        this.WhenAnyValue(x => x.SelectedColor)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                File.AppendAllText("log.txt", $"Color is changing{Environment.NewLine}");
                AvaloniaColor = SelectedColor.ToString();
                File.AppendAllText(
                    "log.txt",
                    $"Color changed: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    [Reactive] public AvaloniaColor SelectedColor { get; set; } = Colors.Red;
    [Reactive] public string AvaloniaColor { get; set; } = "Red";
    [Reactive] public bool DrawingInProgress { get; set; } = false;
    [Reactive] public int Thickness { get; set; } = 1;
    [Reactive] public Point StartPoint { get; set; }
    [Reactive] public Point EndPoint { get; set; }

    public void ImageClicked(int x, int y, int clickCount, Point previewPosition)
    {
        if (DrawingInProgress)
        {
            var color = SelectedColor;
            var colorInCurrentColorSpace =
                _parentViewModel.ColorSpaceToolViewModel.SelectedColorSpace.ColorFromRgb(
                    new RedPixelColor(color.R, color.G, color.B));


            _parentViewModel.ParentViewModel.Image.DrawLine(
                (int)_startPoint.X,
                (int)_startPoint.Y,
                x, y,
                color.A / 255f,
                colorInCurrentColorSpace,
                Thickness);
            _parentViewModel.ParentViewModel.Bitmap = _parentViewModel.ParentViewModel.Image.ConvertToAvaloniaBitmap();
            DrawingInProgress = false;
        }
        else if (clickCount == 2)
        {
            DrawingInProgress = true;
            _startPoint = new Point(x, y);

            StartPoint = previewPosition;
            EndPoint = previewPosition;
        }
    }

    public void PointerMoved(Point previewPosition)
    {
        if (DrawingInProgress)
        {
            EndPoint = previewPosition;
        }
    }

    public void DrawingCanceled()
    {
        DrawingInProgress = false;
    }

    public void ColorChanged(AvaloniaColor color)
    {
        SelectedColor = color;
    }
}