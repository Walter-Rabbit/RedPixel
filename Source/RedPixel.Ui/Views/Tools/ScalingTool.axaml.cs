using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RedPixel.Ui.Views.Tools;

public partial class ScalingTool : UserControl
{
    public ScalingTool()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}