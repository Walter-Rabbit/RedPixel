using Avalonia;
using Avalonia.Controls;
using RedPixel.Ui.ViewModels;
using RedPixel.Ui.ViewModels.ToolViewModels;
using Color = Avalonia.Media.Color;

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