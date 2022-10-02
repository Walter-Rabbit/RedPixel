using Avalonia.Controls;
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
    }
}