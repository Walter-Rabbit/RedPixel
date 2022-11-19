using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Ui.Views;

namespace RedPixel.Ui.ViewModels.ToolViewModels;

public class ColorSpaceToolViewModel : BaseToolViewModel
{
    private readonly ColorSpaceTool _view;

    public ReactiveCommand<Unit, Unit> ChangeColorLayersCommand { get; }

    [Reactive] public ColorComponents ColorComponents { get; set; } = ColorComponents.All;
    [Reactive] public ColorSpaces SelectedColorSpace { get; set; }
    [Reactive] public bool[] EnabledComponents { get; set; }

    public IEnumerable<ColorSpaces> AllColorSpaces { get; set; } = ColorSpaces.AllSpaces.Value;

    public ColorSpaceToolViewModel(ColorSpaceTool view)
    {
        EnabledComponents = new bool[] { true, true, true };
        SelectedColorSpace = ColorSpaces.Rgb;
        ChangeColorLayersCommand = ReactiveCommand.CreateFromTask(ChangeColorLayersAsync);
        _view = view;
    }

    public async Task<Unit> ChangeColorLayersAsync()
    {
        ColorComponents = (EnabledComponents[0] ? ColorComponents.First : ColorComponents.None)
                          | (EnabledComponents[1] ? ColorComponents.Second : ColorComponents.None)
                          | (EnabledComponents[2] ? ColorComponents.Third : ColorComponents.None);

        return Unit.Default;
    }
}