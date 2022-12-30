using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Core.ImageParsers;
using RedPixel.Core.Models;
using RedPixel.Ui.Utility;
using RedPixel.Ui.ViewModels.ToolViewModels;
using RedPixel.Ui.ViewModels.UtilitiesViewModels;
using RedPixel.Ui.Views;
using RedPixel.Ui.Views.Tools;
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
        [Reactive] public double ImageWidth { get; set; }
        [Reactive] public double ImageHeight { get; set; }
        [Reactive] public double LeftImageMargin { get; set; }
        [Reactive] public double TopImageMargin { get; set; }

        public ColorSpaceToolViewModel ColorSpaceToolViewModel { get; set; }
        public GammaCorrectionToolViewModel GammaConversionToolViewModel { get; set; }
        public DitheringToolViewModel DitheringToolViewModel { get; set; }
        public UtilitiesToolViewModel UtilitiesToolViewModel { get; set; }
        public LineDrawingToolViewModel LineDrawingToolViewModel { get; set; }
        public FilteringToolViewModel FilteringToolViewModel { get; set; }
        public SelectionViewModel SelectionViewModel { get; set; }
        public HistogramToolViewModel HistogramToolViewModel { get; set; }

        private async Task<Unit> OpenImageAsync()
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.AddRange(ImageFormat.AllFormats.Value.Select(x => new FileDialogFilter()
            {
                Name = $"Image",
                Extensions = new[] { x.Value }.Concat(x.Alternatives).ToList()
            }));

            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "All",
                Extensions = new List<string>() { "*" }
            });

            dialog.AllowMultiple = false;
            var result = await dialog.ShowAsync(_view);

            if (result is null) return Unit.Default;
            var filePath = result.First();
            await using var fileStream = File.OpenRead(filePath);
            var format = ImageFormat.Parse(fileStream);

            var sw = new Stopwatch();
            sw.Start();
            var img = ImageParserFactory.CreateParser(format)
                .Parse(fileStream, ColorSpaceToolViewModel.SelectedColorSpace);
            img.Gamma = GammaConversionToolViewModel.GammaValue;

            sw.Stop();
            File.AppendAllText("log.txt", $"Parse: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
            Image = img;

            var coefficient = Image.Width > Image.Height
                ? (_view.Width - 360) / Image.Width
                : (_view.Height - 90) / Image.Height;

            ImageWidth = Image.Width * coefficient;
            ImageHeight = Image.Height * coefficient;
            LeftImageMargin = (_view.Width - 340 - ImageWidth) / 2;
            TopImageMargin = (_view.Height - 70 - ImageHeight) / 2 + 15;

            return Unit.Default;
        }

        private async Task<Unit> SaveImageAsync()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "Images",
                Extensions = ImageFormat.AllFormats.Value.Select(x => x.Value).ToList()
            });

            var result = await dialog.ShowAsync(_view);

            if (result is null) return Unit.Default;
            var extension = Path.GetExtension(result).Replace(".", "");

            if (extension is null)
                throw new ArgumentException("File path has no extension");

            var format = ImageFormat.Parse(extension);
            await using var fileStream = File.OpenWrite(result);
            ImageParserFactory.CreateParser(format)
                .SerializeToStream(Image, fileStream, ColorSpaceToolViewModel.SelectedColorSpace,
                    ColorSpaceToolViewModel.ColorComponents);

            return Unit.Default;
        }
    }
}