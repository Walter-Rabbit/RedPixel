using Avalonia.Controls;
using RedPixel.Ui.ViewModels.ToolViewModels;

namespace RedPixel.Ui.Views.Tools;

public partial class GammaConversionTool : UserControl
{
    public GammaConversionTool()
    {
        InitializeComponent();
    }
    
    private void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
    {
        (DataContext as GammaConversionToolViewModel)?.NumericUpDown_OnValueChanged(sender, e);
    }
}