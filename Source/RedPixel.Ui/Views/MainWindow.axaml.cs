using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using RedPixel.Ui.ViewModels;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);
            InitializeComponent();
        }

        private void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.NumericUpDown_OnValueChanged(sender, e);
        }
    }
}