using System;
using System.Drawing;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using RedPixel.Core.Colors.ValueObjects;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Styles;
using Color = System.Drawing.Color;

namespace RedPixel.Ui.Views.Tools;

public partial class HistogramTool : UserControl
{
    private AvaPlot _histogram1;
    private AvaPlot _histogram2;
    private AvaPlot _histogram3;

    public HistogramTool()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _histogram1 = this.Find<AvaPlot>("Histogram1");
        _histogram2 = this.Find<AvaPlot>("Histogram2");
        _histogram3 = this.Find<AvaPlot>("Histogram3");

        _histogram1.Configuration.Zoom = false;
        _histogram1.Configuration.LockHorizontalAxis = true;
        _histogram1.Configuration.LockVerticalAxis = true;

        _histogram2.Configuration.Zoom = false;
        _histogram2.Configuration.LockHorizontalAxis = true;
        _histogram2.Configuration.LockVerticalAxis = true;

        _histogram3.Configuration.Zoom = false;
        _histogram3.Configuration.LockHorizontalAxis = true;
        _histogram3.Configuration.LockVerticalAxis = true;

        _histogram1.Plot.Style(new PlotStyle());
        _histogram2.Plot.Style(new PlotStyle());
        _histogram3.Plot.Style(new PlotStyle());

        _histogram1.Configuration.Zoom = false;
        _histogram1.Configuration.LockHorizontalAxis = true;
        _histogram1.Configuration.LockVerticalAxis = true;

        _histogram2.Configuration.Zoom = false;
        _histogram2.Configuration.LockHorizontalAxis = true;
        _histogram2.Configuration.LockVerticalAxis = true;

        _histogram3.Configuration.Zoom = false;
        _histogram3.Configuration.LockHorizontalAxis = true;
        _histogram3.Configuration.LockVerticalAxis = true;

        _histogram1.Refresh();
        _histogram2.Refresh();
        _histogram3.Refresh();
    }

    public void UpdateHistograms(double[][] histograms)
    {
        _histogram1.Plot.Clear();
        _histogram2.Plot.Clear();
        _histogram3.Plot.Clear();

        _histogram1.Plot.AddBar(histograms[0], Color.Red).BorderColor = Color.Red;
        _histogram2.Plot.AddBar(histograms[1], Color.Green).BorderColor = Color.Green;
        _histogram3.Plot.AddBar(histograms[2], Color.Blue).BorderColor = Color.Blue;

        _histogram1.Plot.SetAxisLimits(yMin: 0, xMax: histograms[0].Length, xMin:0);
        _histogram2.Plot.SetAxisLimits(yMin: 0, xMax: histograms[1].Length, xMin:0);
        _histogram3.Plot.SetAxisLimits(yMin: 0, xMax: histograms[2].Length, xMin:0);

        _histogram1.Plot.Style(new PlotStyle());
        _histogram2.Plot.Style(new PlotStyle());
        _histogram3.Plot.Style(new PlotStyle());

        _histogram1.Plot.Frameless();
        _histogram2.Plot.Frameless();
        _histogram3.Plot.Frameless();

        _histogram1.Refresh();
        _histogram2.Refresh();
        _histogram3.Refresh();
    }
}

public class PlotStyle : Default
{
    public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#2b2d30");
    public override Color DataBackgroundColor => ColorTranslator.FromHtml("#2b2d30");
    public override Color FrameColor => ColorTranslator.FromHtml("#757a80");
    public override Color GridLineColor => ColorTranslator.FromHtml("#444b52");
    public override Color AxisLabelColor => ColorTranslator.FromHtml("#d6d7d8");
    public override Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
    public override Color TickLabelColor => ColorTranslator.FromHtml("#757a80");
    public override Color TickMajorColor => ColorTranslator.FromHtml("#757a80");
    public override Color TickMinorColor => ColorTranslator.FromHtml("#757a80");
}