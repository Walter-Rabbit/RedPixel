using System;
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
    public class MainWindowBaseViewModel : BaseViewModel
    {
        private readonly MainWindow _view;
        [Reactive] private Bitmap Image { get; set; }

        private ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> AssignGammaCommand { get; }
        private ReactiveCommand<Unit, Unit> ConvertToGammaCommand { get; }
        
        [Reactive] private Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] private string GammaValueString { get; set; } = "1";
        [Reactive] private float GammaValue { get; set; } = 1;
        [Reactive] private string ConvertGammaMessage { get; set; } = "Convert γ";

        private ColorSpaceToolViewModel ColorSpaceToolViewModel { get; set; }
        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public MainWindowBaseViewModel(MainWindow view)
        {
            _view = view;
            ColorSpaceToolViewModel = new ColorSpaceToolViewModel(_view.ColorSpaceTool);
            
            this.WhenAnyValue(
                    x => x.Image,
                    x => x.ColorSpaceToolViewModel.ColorComponents)
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

            this.WhenAnyValue(x => x.ColorSpaceToolViewModel.SelectedColorSpace)
                .Subscribe(x =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    File.AppendAllText("log.txt", $"ChangeColorSpace started{Environment.NewLine}");
                    Image?.ToColorSpace(x);
                    Bitmap = Image?.ConvertToAvaloniaBitmap(ColorSpaceToolViewModel.ColorComponents);
                    File.AppendAllText(
                        "log.txt",
                        $"ConvertToAvaloniaBitmap (change color space finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                    sw.Stop();
                });
            
            AssignGammaCommand = ReactiveCommand.Create(AssignGamma);
            ConvertToGammaCommand = ReactiveCommand.Create(ConvertToGamma);
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
        }

        public void NumericUpDown_OnValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            ConvertGammaMessage = $"Convert γ to {e.NewValue}";
        }

        private Unit AssignGamma()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
                Image.Gamma = GammaValue;
                Bitmap = Image?.ConvertToAvaloniaBitmap(ColorSpaceToolViewModel.ColorComponents);
                sw.Stop();
                File.AppendAllText(
                    "log.txt",
                    $"AssignGamma: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
            }
            catch (Exception e)
            {
                File.AppendAllText("log.txt", $"{e.Message}");
            }

            return Unit.Default;
        }

        private Unit ConvertToGamma()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
                Bitmap = Image?.ConvertToGamma(GammaValue).ConvertToAvaloniaBitmap(ColorSpaceToolViewModel.ColorComponents);
                sw.Stop();
                File.AppendAllText(
                    "log.txt",
                    $"ConvertToGamma: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
            }
            catch (Exception e)
            {
                File.AppendAllText("log.txt", $"{e.Message}");
            }

            return Unit.Default;
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
            var img = ImageParserFactory.CreateParser(format).Parse(fileStream, ColorSpaceToolViewModel.SelectedColorSpace);
            sw.Stop();
            File.AppendAllText("log.txt", $"Parse: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
            GammaValue = 1;
            GammaValueString = "1";
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
                .SerializeToStream(Image, fileStream, ColorSpaceToolViewModel.SelectedColorSpace, ColorSpaceToolViewModel.ColorComponents);

            return Unit.Default;
        }
    }
}