using Avalonia.Controls;
using RedPixel.Ui.ViewModels;
using RedPixel.Ui.Views.Tools;

namespace RedPixel.Ui.Views
{
    public partial class MainWindow : Window
    {
        public ColorSpaceTool ColorSpaceTool { get; }
        public GammaCorrectionTool GammaCorrectionTool { get; }
        public DitheringTool DitheringTool { get; }

        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);
            DitheringTool = new DitheringTool();
            ColorSpaceTool = new ColorSpaceTool();
            GammaCorrectionTool = new GammaCorrectionTool();
            InitializeComponent();
        }
    }
}