using Avalonia;
using ReactiveUI.Fody.Helpers;
using RedPixel.Ui.Views.Utilities;

namespace RedPixel.Ui.ViewModels.UtilitiesViewModels;

public class SelectionViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly Selection _view;

    public SelectionViewModel(Selection view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }

    public Point RealFirstPoint { get; set; }
    public Point RealSecondPoint { get; set; }
    public Point RealThirdPoint { get; set; }
    public Point RealFourthPoint { get; set; }
    [Reactive] public Point SelectionFirstPoint { get; set; }
    [Reactive] public Point SelectionSecondPoint { get; set; }
    [Reactive] public Point SelectionThirdPoint { get; set; }
    [Reactive] public Point SelectionFourthPoint { get; set; }
    [Reactive] public bool IsSelecting { get; set; } = false;
    [Reactive] public bool IsSelected { get; set; } = false;

    public void ImagePointerPressed(int x, int y, Point previewPosition)
    {
        if (IsSelected)
        {
            IsSelected = false;
            IsSelecting = false;
        }
        else if (IsSelecting)
        {
            SelectionSecondPoint = new Point(SelectionFirstPoint.X, previewPosition.Y);
            SelectionThirdPoint = previewPosition;
            SelectionFourthPoint = new Point(previewPosition.X, SelectionFirstPoint.Y);

            RealSecondPoint = new Point(RealFirstPoint.X, y);
            RealThirdPoint = new Point(x, y);
            RealFourthPoint = new Point(x, RealFirstPoint.Y);
            
            IsSelected = true;
        }
        else
        {
            SelectionFirstPoint = previewPosition;
            SelectionSecondPoint = previewPosition;
            SelectionThirdPoint = previewPosition;
            SelectionFourthPoint = previewPosition;
            
            RealFirstPoint = new Point(x, y);
            RealSecondPoint = new Point(x, y);
            RealThirdPoint = new Point(x, y);
            RealFourthPoint = new Point(x, y);
            
            IsSelecting = true;
        }
    }

    public void ImagePointerMoved(Point previewPosition)
    {
        if (IsSelecting && !IsSelected)
        {
            SelectionSecondPoint = new Point(SelectionFirstPoint.X, previewPosition.Y);
            SelectionThirdPoint = previewPosition;
            SelectionFourthPoint = new Point(previewPosition.X, SelectionFirstPoint.Y);
        }
    }

    public void ImageSelectionCanceled()
    {
        IsSelecting = false;
        IsSelected = false;
    }
}