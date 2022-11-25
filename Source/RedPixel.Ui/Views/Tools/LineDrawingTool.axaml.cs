using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Egorozh.ColorPicker.Dialog;
using Color = Avalonia.Media.Color;

namespace RedPixel.Ui.Views.Tools;

public partial class LineDrawingTool : UserControl
{
    public static Color SelectedColor { get; set; }
    
    public LineDrawingTool()
    {
        InitializeComponent();
        SelectedColor = this.FindControl<ColorPickerButton>("ColorPicker").Color;
    }
    
    private void ColorPicker_OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Color")
        {
            SelectedColor = (Color)e.NewValue;
        }
    }
}