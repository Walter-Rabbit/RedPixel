using System;
using Avalonia;
using Avalonia.Controls;
using Egorozh.ColorPicker.Dialog;
using RedPixel.Core.Colors.ValueObjects;
using Color = Avalonia.Media.Color;

namespace RedPixel.Ui.Views.Tools;

public partial class ColorSpaceTool : UserControl
{
    // TODO: GIGA HACK
    public static Color SelectedColor { get; set; }

    public ColorSpaceTool()
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