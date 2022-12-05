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
            DataContext = new MainWindowViewModel(this);
            DitheringTool = new DitheringTool();
            UtilitiesTool = new UtilitiesTool();
            ColorSpaceTool = new ColorSpaceTool();
            GammaCorrectionTool = new GammaCorrectionTool();
            LineDrawingTool = new LineDrawingTool();
            FilteringTool = new FilteringTool();
            DataContext = new MainWindowViewModel(this);
            InitializeComponent();
        }

        public ColorSpaceTool ColorSpaceTool { get; }
        public GammaCorrectionTool GammaCorrectionTool { get; }
        public DitheringTool DitheringTool { get; }
        public UtilitiesTool UtilitiesTool { get; }
        public LineDrawingTool LineDrawingTool { get; }
        public FilteringTool FilteringTool { get; }

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition((IVisual)e.Source);
            var imageBounds = (sender as Image).Bounds;
            var sourceSize = (sender as Image).Source.Size;
            var x = (int)(position.X / (imageBounds.Width / sourceSize.Width));
            var y = (int)(position.Y / (imageBounds.Height / sourceSize.Height));

            var previewPosition = e.GetPosition((IVisual)e.Source.InteractiveParent);

            (DataContext as MainWindowViewModel)?.LineDrawingToolViewModel.ImageClicked(x, y, e.ClickCount,
                previewPosition);
        }

        private void InputElement_OnPointerMoved(object sender, PointerEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.LineDrawingToolViewModel.PointerMoved(
                e.GetPosition((IVisual)e.Source.InteractiveParent));
        }

        private void InputElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                (DataContext as MainWindowViewModel)?.LineDrawingToolViewModel.DrawingCanceled();
            }
        }
    }
}