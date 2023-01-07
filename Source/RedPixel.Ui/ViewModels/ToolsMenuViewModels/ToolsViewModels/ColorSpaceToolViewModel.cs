using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Ui.Utility;
using RedPixel.Ui.Views.ToolsMenu.Tools;

namespace RedPixel.Ui.ViewModels.ToolsMenuViewModels.ToolsViewModels;

public class ColorSpaceToolViewModel : BaseViewModel
{
    private readonly ToolsMenuViewModel _parentViewModel;
    private readonly ColorSpaceTool _view;

    public ColorSpaceToolViewModel(ColorSpaceTool view, ToolsMenuViewModel parentViewModel)
    {
        EnabledComponents = new bool[] { true, true, true };
        SelectedColorSpace = ColorSpaces.Rgb;
        _view = view;
        _parentViewModel = parentViewModel;

        this.WhenAnyValue(
                x => x.ColorComponents)
            .Subscribe(x =>
            {
                var sw = new Stopwatch();
                sw.Start();
                _parentViewModel.ParentViewModel.Bitmap =
                    _parentViewModel.ParentViewModel.Image?.ConvertToAvaloniaBitmap(ColorComponents);
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
                File.AppendAllText("log.txt", $"ChangeColorSpace started{Environment.NewLine}");
                _parentViewModel.ParentViewModel.Image?.ToColorSpace(x);
                _parentViewModel.ParentViewModel.Bitmap =
                    _parentViewModel.ParentViewModel.Image?.ConvertToAvaloniaBitmap(ColorComponents);
                File.AppendAllText(
                    "log.txt",
                    $"ConvertToAvaloniaBitmap (change color space finished): {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
                sw.Stop();
            });
    }

    [Reactive] public ColorComponents ColorComponents { get; set; } = ColorComponents.All;
    [Reactive] public ColorSpaces SelectedColorSpace { get; set; }
    [Reactive] public bool[] EnabledComponents { get; set; }

    public IEnumerable<ColorSpaces> AllColorSpaces { get; set; } = ColorSpaces.AllSpaces.Value;

    public Unit ChangeColorLayers()
    {
        ColorComponents = (EnabledComponents[0] ? ColorComponents.First : ColorComponents.None)
                          | (EnabledComponents[1] ? ColorComponents.Second : ColorComponents.None)
                          | (EnabledComponents[2] ? ColorComponents.Third : ColorComponents.None);

        return Unit.Default;
    }
}