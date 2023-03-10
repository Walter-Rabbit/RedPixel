using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using RedPixel.Ui.ViewModels;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var previewPosition = e.GetPosition((IVisual) e.Source.InteractiveParent);

            var position = e.GetPosition((IVisual) e.Source);
            var imageBounds = (sender as Image).Bounds;
            var sourceSize = (sender as Image).Source.Size;
            var x = (int) (position.X / (imageBounds.Width / sourceSize.Width));
            var y = (int) (position.Y / (imageBounds.Height / sourceSize.Height));

            if (e.ClickCount == 1 && e.KeyModifiers == KeyModifiers.Shift)
            {
                (DataContext as MainWindowViewModel)?.SelectionViewModel.ImagePointerPressed(x, y, previewPosition);
            }
            else
            {
                (DataContext as MainWindowViewModel)?.ToolsMenuViewModel.LineDrawingToolViewModel.ImageClicked(x, y,
                    e.ClickCount,
                    previewPosition);
            }
        }

        private void InputElement_OnPointerMoved(object sender, PointerEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.ToolsMenuViewModel.LineDrawingToolViewModel.PointerMoved(
                e.GetPosition((IVisual) e.Source.InteractiveParent));
            (DataContext as MainWindowViewModel)?.SelectionViewModel.ImagePointerMoved(
                e.GetPosition((IVisual) e.Source.InteractiveParent));

            var position = e.GetPosition((IVisual) e.Source);
            var imageBounds = (sender as Image).Bounds;
            var sourceSize = (sender as Image).Source.Size;
            var x = (int) (position.X / (imageBounds.Width / sourceSize.Width)) + 1;
            var y = (int) (position.Y / (imageBounds.Height / sourceSize.Height)) + 1;

            x = Math.Max(0, Math.Min((int) sourceSize.Width, x));
            y = Math.Max(0, Math.Min((int) sourceSize.Height, y));
            (DataContext as MainWindowViewModel)?.StatusBarViewModel.CoordinatesViewModel.PointerMoved(x, y);
        }

        private void InputElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                (DataContext as MainWindowViewModel)?.ToolsMenuViewModel.LineDrawingToolViewModel.DrawingCanceled();
                (DataContext as MainWindowViewModel)?.SelectionViewModel.ImageSelectionCanceled();
            }
        }
    }
}