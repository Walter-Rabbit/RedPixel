using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core;
using RedPixel.Core.ImageParsers;
using RedPixel.Ui.Views.TopMenu;

namespace RedPixel.Ui.ViewModels.TopMenuViewModels;

public class TopMenuViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _parentViewModel;
    private readonly TopMenu _view;

    public TopMenuViewModel(TopMenu view, MainWindowViewModel parentViewModel)
    {
        _view = view;
        _parentViewModel = parentViewModel;
    }

    [Reactive] public string HistogramsVisibilityString { get; set; } = "Histograms";
    [Reactive] public string CoordinatesVisibilityString { get; set; } = "Cursor Coordinates ✓";

    private async Task<Unit> OpenImageAsync()
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.AddRange(ImageFormat.AllFormats.Value.Select(x => new FileDialogFilter()
        {
            Name = $"{x.Value}",
            Extensions = new[] { x.Value }.Concat(x.Alternatives).ToList()
        }));

        dialog.Filters.Add(new FileDialogFilter()
        {
            Name = "All",
            Extensions = new List<string>() { "*" }
        });

        dialog.AllowMultiple = false;
        var result = await dialog.ShowAsync(new Window());

        if (result is null) return Unit.Default;
        var filePath = result.First();
        await using var fileStream = File.OpenRead(filePath);
        var format = ImageFormat.Parse(fileStream);

        var sw = new Stopwatch();
        sw.Start();
        var img = ImageParserFactory.CreateParser(format)
            .Parse(fileStream, _parentViewModel.ColorSpaceToolViewModel.SelectedColorSpace);
        img.Gamma = _parentViewModel.GammaConversionToolViewModel.GammaValue;

        sw.Stop();
        File.AppendAllText("log.txt", $"Parse: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
        _parentViewModel.Image = img;

        ApplyDefaultZoom();

        return Unit.Default;
    }

    private async Task<Unit> SaveImageAsync()
    {
        var dialog = new SaveFileDialog();

        dialog.Filters.AddRange(ImageFormat.AllFormats.Value.Select(x => new FileDialogFilter()
        {
            Name = $"{x.Value}",
            Extensions = new[] { x.Value }.Concat(x.Alternatives).ToList()
        }));

        var result = await dialog.ShowAsync(new Window());

        if (result is null) return Unit.Default;
        var extension = Path.GetExtension(result).Replace(".", "");

        if (extension is null)
            throw new ArgumentException("File path has no extension");

        var format = ImageFormat.Parse(extension);
        await using var fileStream = File.OpenWrite(result);
        ImageParserFactory.CreateParser(format)
            .SerializeToStream(_parentViewModel.Image, fileStream,
                _parentViewModel.ColorSpaceToolViewModel.SelectedColorSpace,
                _parentViewModel.ColorSpaceToolViewModel.ColorComponents);

        return Unit.Default;
    }

    private void ApplyDefaultZoom()
    {
        if (_parentViewModel.Image is null)
        {
            return;
        }

        var coefficient = _parentViewModel.Image.Width > _parentViewModel.Image.Height
            ? (_parentViewModel.Width - 360) / _parentViewModel.Image.Width
            : (_parentViewModel.Height - 90) / _parentViewModel.Image.Height;

        var imageWidth = _parentViewModel.Image.Width * coefficient;
        var imageHeight = _parentViewModel.Image.Height * coefficient;
        var leftMargin = (_parentViewModel.Width - 340 - imageWidth) / 2;
        var topMargin = (_parentViewModel.Height - 70 - imageHeight) / 2 + 15;

        var zoomBorder = _parentViewModel.GetFromView<ZoomBorder>("ZoomBorder");
        zoomBorder.Zoom(coefficient, _parentViewModel.Width / 2, _parentViewModel.Height / 2);
        zoomBorder.Pan(leftMargin, topMargin);
    }

    private Unit ChangeHistogramsVisibility()
    {
        _parentViewModel.HistogramToolViewModel.IsVisible = !_parentViewModel.HistogramToolViewModel.IsVisible;
        HistogramsVisibilityString =
            _parentViewModel.HistogramToolViewModel.IsVisible ? "Histograms ✓" : "Histograms  ";

        return Unit.Default;
    }

    private Unit ChangeCoordinatesVisibility()
    {
        _parentViewModel.CoordinatesViewModel.IsVisible = !_parentViewModel.CoordinatesViewModel.IsVisible;
        CoordinatesVisibilityString =
            _parentViewModel.CoordinatesViewModel.IsVisible ? "Cursor Coordinates ✓" : "Cursor Coordinates  ";

        return Unit.Default;
    }
}