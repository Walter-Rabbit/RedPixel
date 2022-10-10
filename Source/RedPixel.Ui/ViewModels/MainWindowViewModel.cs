using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Core.Bitmap;
using RedPixel.Core.ImageParsers;
using RedPixel.Ui.Views;
using Image = System.Drawing.Image;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow _view;

        private ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }

        [Reactive] private Core.Bitmap.Bitmap Image { get; set; }
        public MainWindowViewModel(MainWindow view)
        {
            _view = view;
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
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

            Image = ImageParserFactory.CreateParser(format).Parse(fileStream);

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
            ImageParserFactory.CreateParser(format).SerializeToStream(Image, fileStream);

            return Unit.Default;

        }
    }
}