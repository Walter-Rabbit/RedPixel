using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using CircularBuffer;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Models;
using RedPixel.Ui.Utility;
using RedPixel.Ui.ViewModels.StatusBarViewModels;
using RedPixel.Ui.ViewModels.ToolsMenuViewModels;
using RedPixel.Ui.ViewModels.TopMenuViewModels;
using RedPixel.Ui.ViewModels.UtilitiesViewModels;
using RedPixel.Ui.Views;
using RedPixel.Ui.Views.StatusBar;
using RedPixel.Ui.Views.ToolsMenu;
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
            SelectionViewModel = new SelectionViewModel(_view.Get<Selection>("Selection"), this);
            TopMenuViewModel = new TopMenuViewModel(_view.Get<TopMenu>("TopMenu"), this);
            StatusBarViewModel = new StatusBarViewModel(_view.Get<StatusBar>("StatusBar"), this);
            ToolsMenuViewModel = new ToolsMenuViewModel(_view.Get<ToolsMenu>("ToolsMenu"), this);

            this.WhenAnyValue(x => x.Image)
                .Subscribe(x =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    Bitmap = Image?.ConvertToAvaloniaBitmap(ToolsMenuViewModel.ColorSpaceToolViewModel.ColorComponents);
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

                    ToolsMenuViewModel.HistogramToolViewModel.HistogramValues =
                        Image.GetHistogram(0, Image.Width, 0, Image.Height);
                });

            ImagesHistory = new CircularBuffer<Bitmap>(10);

            ExtendClientAreaToDecorationsHint = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        [Reactive] public Bitmap Image { get; set; }
        [Reactive] public Avalonia.Media.Imaging.Bitmap Bitmap { get; set; }
        [Reactive] public bool ExtendClientAreaToDecorationsHint { get; set; }

        public CircularBuffer<Bitmap> ImagesHistory { get; set; }

        public double Height => _view.Height;
        public double Width => _view.Width;

        public ToolsMenuViewModel ToolsMenuViewModel { get; set; }
        public SelectionViewModel SelectionViewModel { get; set; }
        public StatusBarViewModel StatusBarViewModel { get; set; }
        public TopMenuViewModel TopMenuViewModel { get; set; }

        public void SaveImageToHistory()
        {
            ImagesHistory.PushFront((Bitmap) Image.Clone());
        }

        public void RollBack()
        {
            if (ImagesHistory.IsEmpty)
            {
                return;
            }

            Image = ImagesHistory.Front();
            Bitmap = Image?.ConvertToAvaloniaBitmap(ToolsMenuViewModel.ColorSpaceToolViewModel.ColorComponents);

            ImagesHistory.PopFront();
        }

        public T GetFromView<T>(string name) where T : class
        {
            return _view.Get<T>(name);
        }
    }
}