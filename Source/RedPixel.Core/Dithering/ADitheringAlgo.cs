using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Dithering;

public abstract class ADitheringAlgo
{
        protected static Color FindClosestPaletteColor(Color clr)
        {
                var bwPixel = (float)(
                        0.0722 * clr.FirstComponent +
                        0.7152 * clr.SecondComponent +
                        0.2126 * clr.ThirdComponent
                );

                bwPixel = bwPixel > 128 ? 255 : 0;

                return new Color(bwPixel, bwPixel, bwPixel);
        }
        
        protected static Color GetError(Color lhs, Color rhs)
        {
                return new Color(
                        lhs.FirstComponent - rhs.FirstComponent,
                        lhs.SecondComponent - rhs.SecondComponent,
                        lhs.ThirdComponent - rhs.ThirdComponent
                );
        }
        
        protected static Color GetPixelWithError(Color clr, Color error, float weight)
        {
                var first = clr.FirstComponent + error.FirstComponent * weight;
                first = first > 255f ? 255 : first;
                first = first < 0f ? 0 : first;
        
                var second = clr.SecondComponent + error.SecondComponent * weight;
                second = second > 255f ? 255 : second;
                second = second < 0f ? 0 : second;
        
                var third = clr.ThirdComponent + error.ThirdComponent * weight;
                third = third > 255f ? 255 : third;
                third = third < 0f ? 0 : third;
        
                return new Color(
                        first,
                        second,
                        third
                );
        }
}