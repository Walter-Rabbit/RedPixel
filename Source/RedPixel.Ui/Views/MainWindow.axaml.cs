using Avalonia.Controls;
using RedPixel.Ui.ViewModels;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public ColorSpaceTool ColorSpaceTool { get; }
        public GammaConversionTool GammaConversionTool { get; }
        
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);
            ColorSpaceTool = new ColorSpaceTool();
            GammaConversionTool = new GammaConversionTool();
            InitializeComponent();
        }
    }
}