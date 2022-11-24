using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools;
using RedPixel.Ui.Utility;
using RedPixel.Ui.ViewModels;
using RedPixel.Ui.Views.Tools;
using Splat;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public ColorSpaceTool ColorSpaceTool { get; }
        public GammaCorrectionTool GammaCorrectionTool { get; }

        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);
            ColorSpaceTool = new ColorSpaceTool();
            GammaCorrectionTool = new GammaCorrectionTool();
            InitializeComponent();
        }

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition((IVisual)e.Source);
            var imageBounds = (sender as Image).Bounds;
            var sourceSize = (sender as Image).Source.Size;
            var x = (int)(position.X / (imageBounds.Width / sourceSize.Width));
            var y = (int)(position.Y / (imageBounds.Height / sourceSize.Height));

            var previewPosition = e.GetPosition((IVisual)e.Source.InteractiveParent);

            (DataContext as MainWindowViewModel)?.ImageClicked(x, y, e.ClickCount);
        }
    }
}