﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Core.Bitmap;
using RedPixel.Core.Colors;
using RedPixel.Core.ImageParsers;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow _view;
        [Reactive] private Bitmap Image { get; set; }

        private ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> ChangeColorLayers { get; }

        [Reactive] private bool[] EnabledComponents { get; set; }
        [Reactive] private Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] private ColorComponents ColorComponents { get; set; } = ColorComponents.All;
        [Reactive] private ColorSpace SelectedColorSpace { get; set; }
        private IEnumerable<ColorSpace> ColorSpaces { get; set; } = ColorSpace.AllSpaces.Value;

        public MainWindowViewModel(MainWindow view)
        {
            this.WhenAnyValue(
                x => x.Image,
                x => x.ColorComponents).Subscribe(x => Bitmap = Image?.ConvertToAvaloniaBitmap(ColorComponents));

            this.WhenAnyValue(x => x.SelectedColorSpace).Subscribe(x => Image = Image?.ChangeColorSpace(x));

            _view = view;
            EnabledComponents = new bool[] { true, true, true };
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
            ChangeColorLayers = ReactiveCommand.CreateFromTask(ChangeColorLayersAsync);
        }

        private async Task<Unit> OpenImageAsync()
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.AddRange(ImageFormat.AllFormats.Value.Select(x => new FileDialogFilter()
            {
                Name = $"Image",
                Extensions = new [] {x.Value}.Concat(x.Alternatives).ToList()
            }));

            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "All",
                Extensions = new List<string>(){"*"}
            });

            dialog.AllowMultiple = false;
            var result = await dialog.ShowAsync(_view);

            if (result is null) return Unit.Default;
            var filePath = result.First();
            await using var fileStream = File.OpenRead(filePath);
            var format = ImageFormat.Parse(fileStream);

            Image = ImageParserFactory.CreateParser(format).Parse(fileStream, SelectedColorSpace);

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
            ImageParserFactory.CreateParser(format).SerializeToStream(Image.SelectColorComponents(ColorComponents), fileStream, SelectedColorSpace);

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