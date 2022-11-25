﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Core.ImageParsers;
using RedPixel.Ui.Utility;
using RedPixel.Ui.ViewModels.ToolViewModels;
using RedPixel.Ui.Views;
using Bitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly MainWindow _view;
        [Reactive] public Bitmap Image { get; set; }

        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> SwitchColorSpacesCommand { get; }
        public ReactiveCommand<Unit, Unit> SwitchGammaCorrectionCommand { get; }
        public ReactiveCommand<Unit, Unit> LineDrawingCommand { get; }

        [Reactive] public Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] public bool ToolPanelIsVisible { get; set; } = false;

        public ColorSpaceToolViewModel ColorSpaceToolViewModel { get; set; }
        public GammaCorrectionToolViewModel GammaConversionToolViewModel { get; set; }
        public DitheringToolViewModel DitheringToolViewModel { get; set; }
        public UtilitiesToolViewModel UtilitiesToolViewModel { get; set; }
        
        public LineDrawingToolViewModel LineDrawingToolViewModel { get; set; }

        public MainWindowViewModel(MainWindow view)
        {
            _view = view;
            ColorSpaceToolViewModel = new ColorSpaceToolViewModel(_view.ColorSpaceTool, this);
            GammaConversionToolViewModel = new GammaCorrectionToolViewModel(_view.GammaCorrectionTool, this);
            LineDrawingToolViewModel = new LineDrawingToolViewModel(_view.LineDrawingTool, this);
            DitheringToolViewModel = new DitheringToolViewModel(_view.DitheringTool, this);
            UtilitiesToolViewModel = new UtilitiesToolViewModel(_view.UtilitiesTool, this);

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
            
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
            SwitchColorSpacesCommand = ReactiveCommand.Create(SwitchColorSpaces);
            SwitchGammaCorrectionCommand = ReactiveCommand.Create(SwitchGammaCorrection);
            
            LineDrawingCommand = ReactiveCommand.Create(SwitchLineDrawing);
        }

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

        private Unit SwitchColorSpaces()
        {
            ColorSpaceToolViewModel.IsVisible = !ColorSpaceToolViewModel.IsVisible;
            SwitchToolPanel();
            
            return Unit.Default;
        }
        
        private Unit SwitchGammaCorrection()
        {
            GammaConversionToolViewModel.IsVisible = !GammaConversionToolViewModel.IsVisible;
            SwitchToolPanel();
            
            return Unit.Default;
        }

        private Unit SwitchLineDrawing()
        {
            LineDrawingToolViewModel.IsVisible = !LineDrawingToolViewModel.IsVisible;
            SwitchToolPanel();
            
            return Unit.Default;
        }
        
        private Unit SwitchDithering()
        {
            DitheringToolViewModel.IsVisible = !DitheringToolViewModel.IsVisible;
            SwitchToolPanel();
            
            return Unit.Default;
        }

        private void SwitchToolPanel()
        {
            ToolPanelIsVisible = 
                ColorSpaceToolViewModel.IsVisible || 
                GammaConversionToolViewModel.IsVisible ||
                UtilitiesToolViewModel.IsVisible ||
                DitheringToolViewModel.IsVisible;
        }
    }
}