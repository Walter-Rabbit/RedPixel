using Avalonia;
using Avalonia.Media;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.Tools;
using RedPixelColor = RedPixel.Core.Colors.ValueObjects.Color;
using AvaloniaColor = Avalonia.Media.Color;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class LineDrawingToolViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly LineDrawingTool _view;
    private Point _startPoint;

    public LineDrawingToolViewModel(LineDrawingTool view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }

    [Reactive] public AvaloniaColor SelectedColor { get; set; } = Colors.Red;
    [Reactive] public bool DrawingInProgress { get; set; } = false;
    [Reactive] public int Thickness { get; set; } = 1;
    [Reactive] public Point StartPoint1 { get; set; }
    [Reactive] public Point StartPoint2 { get; set; }
    [Reactive] public Point EndPoint1 { get; set; }
    [Reactive] public Point EndPoint2 { get; set; }

    public void ImageClicked(int x, int y, int clickCount, Point previewPosition)
    {
        if (DrawingInProgress)
        {
            var color = SelectedColor;
            var colorInCurrentColorSpace =
                _parentViewModel.ColorSpaceToolViewModel.SelectedColorSpace.ColorFromRgb(
                    new RedPixelColor(color.R, color.G, color.B));


            _parentViewModel.Image.DrawLine(
                (int)_startPoint.X,
                (int)_startPoint.Y,
                x, y,
                color.A / 255f,
                colorInCurrentColorSpace,
                Thickness);
            _parentViewModel.Bitmap = _parentViewModel.Image.ConvertToAvaloniaBitmap();
            DrawingInProgress = false;
        }
        else if (clickCount == 2)
        {
            DrawingInProgress = true;
            _startPoint = new Point(x, y);

            StartPoint1 = new Point(previewPosition.X - 5, previewPosition.Y - 5);
            EndPoint1 = new Point(previewPosition.X + 5, previewPosition.Y + 5);
            StartPoint2 = new Point(previewPosition.X - 5, previewPosition.Y + 5);
            EndPoint2 = new Point(previewPosition.X + 5, previewPosition.Y - 5);
        }
    }

    public void ColorChanged(AvaloniaColor color)
    {
        SelectedColor = color;
    }
}