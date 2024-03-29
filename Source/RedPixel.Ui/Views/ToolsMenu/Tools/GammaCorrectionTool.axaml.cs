﻿using Avalonia.Controls;
using RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

namespace RedPixel.Ui.Views.ToolsMenu.Tools;

public partial class GammaCorrectionTool : UserControl
{
    public GammaCorrectionTool()
    {
        InitializeComponent();
    }
    
    private void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
    {
        (DataContext as GammaCorrectionToolViewModel)?.NumericUpDown_OnValueChanged(sender, e);
    }
}