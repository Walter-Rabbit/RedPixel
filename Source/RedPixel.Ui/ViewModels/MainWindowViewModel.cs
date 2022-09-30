using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Ui.Views;
using Image = System.Drawing.Image;
using Path = Avalonia.Controls.Shapes.Path;

namespace RedPixel.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow _view;

        private ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveFileDialogCommand { get; }

        [Reactive] private Image Image { get; set; }
        public MainWindowViewModel(MainWindow view)
        {
            _view = view;
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(OpenImageAsync);
            SaveFileDialogCommand = ReactiveCommand.CreateFromTask(SaveImageAsync);
        }

        private async Task<Unit> OpenImageAsync()
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter
            {
                Name = "Images",
                Extensions = ImageFormat.AllFormats.Value.Select(x => new string(x.Value.Skip(1).ToArray())).ToList()
            });
            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "All",
                Extensions = new List<string>(){"*"}
            });

            dialog.AllowMultiple = false;
            var result = await dialog.ShowAsync(_view);

            if (result is not null)
            {
                var filePath = result.First();
                using var fileStream = File.OpenRead(filePath);
                var format = ImageFormat.Parse(fileStream);
                Image = ImageParserFactory.CreateParser(format).Parse(fileStream);
            }

            return Unit.Default;
        }

        private async Task<Unit> SaveImageAsync()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter()
            {
                Name = "Images",
                Extensions = ImageFormat.AllFormats.Value.Select(x => new string(x.Value.Skip(1).ToArray())).ToList()
            });

            var result = await dialog.ShowAsync(_view);

            if (result is not null)
            {
                var filePath = result;
                var extension = System.IO.Path.GetExtension(filePath);

                if (extension is null)
                    throw new ArgumentException("File path has no extension");

                var format = ImageFormat.Parse(extension);
                using var fileStream = File.OpenWrite(filePath);
                ImageParserFactory.CreateParser(format).SerializeToStream(Image, fileStream);
            }

            return Unit.Default;
        }
    }
}