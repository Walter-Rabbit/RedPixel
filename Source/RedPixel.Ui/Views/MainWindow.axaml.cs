using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using RedPixel.Ui.ViewModels;
using RedPixel.Ui.Views.Tools;

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
            var position = e.GetPosition((IVisual)e.Source);
            var imageBounds = (sender as Image).Bounds;
            var sourceSize = (sender as Image).Source.Size;
            var x = (int)(position.X / (imageBounds.Width / sourceSize.Width));
            var y = (int)(position.Y / (imageBounds.Height / sourceSize.Height));

            var previewPosition = e.GetPosition((IVisual)e.Source.InteractiveParent);

            (DataContext as MainWindowViewModel)?.LineDrawingToolViewModel.ImageClicked(x, y, e.ClickCount, previewPosition);
        }
    }
}