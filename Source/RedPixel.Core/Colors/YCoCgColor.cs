﻿using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class YCoCgColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }
    public int BytesForColor { get; }

    public YCoCgColor(float luma, float cOrange, float cGreen, int bytesForColor)
    {
        BytesForColor = bytesForColor;
        FirstComponent = new ColorComponent(luma);
        SecondComponent = new ColorComponent(cOrange);
        ThirdComponent = new ColorComponent(cGreen);
    }

    public RgbColor ToRgb()
    {
        var luma = FirstComponent.Visible ? FirstComponent.Value : 0;
        var cOrange = SecondComponent.Visible ? SecondComponent.Value : -255;
        var cGreen = ThirdComponent.Visible ? ThirdComponent.Value : -255;

        var y = luma / 510;
        var cO = cOrange / 510;
        var cG = cGreen / 510;

        var r = y + cO - cG;
        var g = y + cG;
        var b = y - cO - cG;

        return new RgbColor(r * 255, g * 255, b * 255, BytesForColor - 1);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var r = rgb.FirstComponent.Value / 255;
        var g = rgb.SecondComponent.Value / 255;
        var b = rgb.ThirdComponent.Value / 255;

        var y = 1f / 4f * r + 1f / 2f * g + 1f / 4f * b;
        var cO = 1f / 2f * r - 1f / 2f * b;
        var cG = -1f / 4f * r + 1f / 2f * g - 1f / 4f * b;

        return new YCoCgColor(y * 510, cO * 510, cG * 510, rgb.BytesForColor + 1);
    }
}