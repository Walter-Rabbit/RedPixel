using Avalonia.Controls;
using RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;
using RedPixel.Ui.Views.ToolsMenu;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels;

public class ToolsMenuViewModel : BaseViewModel
{
    private readonly ToolsMenu _view;

    public ToolsMenuViewModel(ToolsMenu view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        ParentViewModel = parentViewModel;

        ColorSpaceToolViewModel = new ColorSpaceToolViewModel(_view.Get<ColorSpaceTool>("ColorSpace"), this);
        GammaConversionToolViewModel =
            new GammaCorrectionToolViewModel(_view.Get<GammaCorrectionTool>("GammaCorrection"), this);
        LineDrawingToolViewModel = new LineDrawingToolViewModel(_view.Get<LineDrawingTool>("LineDrawing"), this);
        DitheringToolViewModel = new DitheringToolViewModel(_view.Get<DitheringTool>("Dithering"), this);
        UtilitiesToolViewModel = new UtilitiesToolViewModel(_view.Get<UtilitiesTool>("Utilities"), this);
        HistogramToolViewModel = new HistogramToolViewModel(_view.Get<HistogramTool>("Histogram"), this);
        FilteringToolViewModel = new FilteringToolViewModel(_view.Get<FilteringTool>("Filtering"), this);
        ScalingToolViewModel = new ScalingToolViewModel(_view.Get<ScalingTool>("Scaling"), this);
    }

    public ColorSpaceToolViewModel ColorSpaceToolViewModel { get; set; }
    public GammaCorrectionToolViewModel GammaConversionToolViewModel { get; set; }
    public DitheringToolViewModel DitheringToolViewModel { get; set; }
    public UtilitiesToolViewModel UtilitiesToolViewModel { get; set; }
    public ScalingToolViewModel ScalingToolViewModel { get; set; }
    public LineDrawingToolViewModel LineDrawingToolViewModel { get; set; }
    public FilteringToolViewModel FilteringToolViewModel { get; set; }
    public HistogramToolViewModel HistogramToolViewModel { get; set; }

    public MainWindowViewModel ParentViewModel { get; }
}