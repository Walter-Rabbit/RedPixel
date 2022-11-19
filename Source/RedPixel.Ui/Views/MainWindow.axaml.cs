using Avalonia.Controls;
using RedPixel.Ui.ViewModels;
using RedPixel.Ui.ViewModels.ToolViewModels;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public ColorSpaceTool ColorSpaceTool { get; }
        
        public MainWindow()
        {
            DataContext = new MainWindowBaseViewModel(this);
            ColorSpaceTool = new ColorSpaceTool();
            InitializeComponent();
        }

        private void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            (DataContext as MainWindowBaseViewModel)?.NumericUpDown_OnValueChanged(sender, e);
        }
    }
}