using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Models;
using RedPixel.Ui.Utility;
using RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;
using RedPixel.Ui.ViewModels.TopMenuViewModels;
using RedPixel.Ui.ViewModels.UtilitiesViewModels;
using RedPixel.Ui.Views;
using RedPixel.Ui.Views.ToolsMenu.Tools;
using RedPixel.Ui.Views.TopMenu;
using RedPixel.Ui.Views.Utilities;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly MainWindow _view;

        public MainWindowViewModel(MainWindow view)
        {
            _view = view;
            ColorSpaceToolViewModel = new ColorSpaceToolViewModel(_view.Get<ColorSpaceTool>("ColorSpace"), this);
            GammaConversionToolViewModel =
                new GammaCorrectionToolViewModel(_view.Get<GammaCorrectionTool>("GammaCorrection"), this);
            LineDrawingToolViewModel = new LineDrawingToolViewModel(_view.Get<LineDrawingTool>("LineDrawing"), this);
            DitheringToolViewModel = new DitheringToolViewModel(_view.Get<DitheringTool>("Dithering"), this);
            UtilitiesToolViewModel = new UtilitiesToolViewModel(_view.Get<UtilitiesTool>("Utilities"), this);
            HistogramToolViewModel = new HistogramToolViewModel(_view.Get<HistogramTool>("Histogram"), this);
            SelectionViewModel = new SelectionViewModel(_view.Get<Selection>("Selection"), this);
            FilteringToolViewModel = new FilteringToolViewModel(_view.Get<FilteringTool>("Filtering"), this);
            ScalingToolViewModel = new ScalingToolViewModel(_view.Get<ScalingTool>("Scaling"), this);
            CoordinatesViewModel = new CoordinatesViewModel(_view.Get<Coordinates>("Coordinates"), this);
            TopMenuViewModel = new TopMenuViewModel(_view.Get<TopMenu>("TopMenu"), this);

            this.WhenAnyValue(x => x.Image)
                .Subscribe(x =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    Bitmap = Image?.ConvertToAvaloniaBitmap(ColorSpaceToolViewModel.ColorComponents);
                    sw.Stop();
                    File.AppendAllText(
                        "log.txt",
                        $"ConvertToAvaloniaBitmap: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                });

            this.WhenAnyValue(x => x.Bitmap)
                .Subscribe(x =>
                {
                    if (x is null)
                        return;

                    HistogramToolViewModel.HistogramValues = Image.GetHistogram(0, Image.Width, 0, Image.Height);
                });

            ExtendClientAreaToDecorationsHint = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        [Reactive] public Bitmap Image { get; set; }
        [Reactive] public Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] public bool ExtendClientAreaToDecorationsHint { get; set; }

        public double Height => _view.Height;
        public double Width => _view.Width;

        public ColorSpaceToolViewModel ColorSpaceToolViewModel { get; set; }
        public GammaCorrectionToolViewModel GammaConversionToolViewModel { get; set; }
        public DitheringToolViewModel DitheringToolViewModel { get; set; }
        public UtilitiesToolViewModel UtilitiesToolViewModel { get; set; }
        public ScalingToolViewModel ScalingToolViewModel { get; set; }
        public LineDrawingToolViewModel LineDrawingToolViewModel { get; set; }
        public FilteringToolViewModel FilteringToolViewModel { get; set; }
        public SelectionViewModel SelectionViewModel { get; set; }
        public HistogramToolViewModel HistogramToolViewModel { get; set; }
        public CoordinatesViewModel CoordinatesViewModel { get; set; }
        public TopMenuViewModel TopMenuViewModel { get; set; }

        public T GetFromView<T>(string name) where T : class
        {
            return _view.Get<T>(name);
        }
    }
}