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
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers;
using RedPixel.Core.Models;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views;
using Bitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow _view;
        [Reactive] private Bitmap Image { get; set; }

        private ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> ChangeColorLayersCommand { get; }
        private ReactiveCommand<Unit, Unit> AssignGammaCommand { get; }
        private ReactiveCommand<Unit, Unit> ConvertToGammaCommand { get; }

        [Reactive] private bool[] EnabledComponents { get; set; }
        [Reactive] private Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] private ColorComponents ColorComponents { get; set; } = ColorComponents.All;
        [Reactive] private ColorSpaces SelectedColorSpace { get; set; }
        [Reactive] private string GammaValueString { get; set; }
        [Reactive] private float GammaValue { get; set; } = 0;
        private CultureInfo CultureInfo => CultureInfo.InvariantCulture;
        private IEnumerable<ColorSpaces> AllColorSpaces { get; set; } = ColorSpaces.AllSpaces.Value;

        public MainWindowViewModel(MainWindow view)
        {
            this.WhenAnyValue(
                    x => x.Image,
                    x => x.ColorComponents)
                .Subscribe(x =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    Bitmap = Image?.ConvertToAvaloniaBitmap(ColorComponents);
                    sw.Stop();
                    File.AppendAllText(
                        "log.txt",
                        $"ConvertToAvaloniaBitmap: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                });

            this.WhenAnyValue(x => x.SelectedColorSpace)
                .Subscribe(x =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    File.AppendAllText("log.txt", $"ChangeColorSpace: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                    sw.Reset();
                    sw.Start();
                    Image?.ToColorSpace(x);
                    Bitmap = Image?.ConvertToAvaloniaBitmap(ColorComponents);
                    File.AppendAllText(
                        "log.txt",
                        $"ConvertToAvaloniaBitmap: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                    sw.Stop();
                });

            _view = view;
            EnabledComponents = new bool[] { true, true, true };
            AssignGammaCommand = ReactiveCommand.Create(AssignGamma);
            ConvertToGammaCommand = ReactiveCommand.Create(ConvertToGamma);
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
            ChangeColorLayersCommand = ReactiveCommand.CreateFromTask(ChangeColorLayersAsync);
        }

        private Unit AssignGamma()
        {
            try
            {
                GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
                Bitmap = Image?.AssignGamma(GammaValue).ConvertToAvaloniaBitmap(ColorComponents);
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
                GammaValue = Convert.ToSingle(GammaValueString, CultureInfo.InvariantCulture);
                Bitmap = Image?.ConvertToGamma(GammaValue).ConvertToAvaloniaBitmap();
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
            var img = ImageParserFactory.CreateParser(format).Parse(fileStream, SelectedColorSpace);
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
                .SerializeToStream(Image, fileStream, SelectedColorSpace, ColorComponents);

            return Unit.Default;
        }

        private async Task<Unit> ChangeColorLayersAsync()
        {
            ColorComponents = (EnabledComponents[0] ? ColorComponents.First : ColorComponents.None)
                              | (EnabledComponents[1] ? ColorComponents.Second : ColorComponents.None)
                              | (EnabledComponents[2] ? ColorComponents.Third : ColorComponents.None);

            return Unit.Default;
        }
    }
}