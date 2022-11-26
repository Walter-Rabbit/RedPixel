using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using RedPixel.Ui.ViewModels.ToolViewModels;

namespace RedPixel.Ui.Views.Tools;

public partial class LineDrawingTool : UserControl
{
    public LineDrawingTool()
    {
        InitializeComponent();
    }

    private void ColorPicker_OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Color")
        {
            (DataContext as LineDrawingToolViewModel)?.ColorChanged((Color)e.NewValue);
        }
    }
}