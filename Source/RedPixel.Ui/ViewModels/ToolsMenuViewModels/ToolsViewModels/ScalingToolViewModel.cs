using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Tools.Scaler;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class ScalingToolViewModel : BaseViewModel
{
    private ToolsMenuViewModel _parentViewModel;
    private ScalingTool _view;
    private readonly MainWindowViewModel _imageViewModel;

    public ScalingToolViewModel(ScalingTool view, ToolsMenuViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
        _view = view;
        _imageViewModel = _parentViewModel.ParentViewModel;

        this.WhenAnyValue(x => x.SelectedScaler)
            .Subscribe(x => { IsBcSplines = x.Name == "BC Splines"; });
    }

    public List<ImageScaler> AllImageScalers { get; set; } = ImageScaler.All.Value.ToList();
    [Reactive] public ImageScaler SelectedScaler { get; set; } = ImageScaler.All.Value.First();
    [Reactive] public bool IsBcSplines { get; set; } = false;

    public string WidthString { get; set; }
    public string HeightString { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string BString { get; set; }
    public string CString { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    public CultureInfo CultureInfo => CultureInfo.InvariantCulture;

    public void Scale()
    {
        try
        {
            _parentViewModel.ParentViewModel.SaveImageToHistory();

            var sw = new Stopwatch();
            sw.Start();
            Width = Convert.ToInt32(WidthString);
            Height = Convert.ToInt32(HeightString);

            if (SelectedScaler.Name != "BC Splines")
            {
                _imageViewModel.Image = SelectedScaler.Scaler.Invoke(_imageViewModel.Image, Width, Height, null);
            }
            else
            {
                B = Convert.ToSingle(BString, CultureInfo.InvariantCulture);
                C = Convert.ToSingle(CString, CultureInfo.InvariantCulture);
                _imageViewModel.Image =
                    SelectedScaler.Scaler.Invoke(_imageViewModel.Image, Width, Height, new float[] { B, C });
            }

            sw.Stop();
            File.AppendAllText(
                "log.txt",
                $"AssignGamma: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
        }
        catch (Exception e)
        {
            File.AppendAllText("log.txt", $"{e.Message}");
        }
    }
}