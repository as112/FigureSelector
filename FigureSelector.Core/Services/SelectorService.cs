using System.Drawing;

namespace FigureSelector.Core.Services
{
    public class SelectorService : ISelectorService
    {
        public Models.Rectangle? Select(Models.Rectangle mainRectangle, IEnumerable<Models.Rectangle> rectangles, IEnumerable<Color> includedColors, bool includeExternalPoints)
        {
            var r = new Models.Rectangle(mainRectangle.Color,
                new Models.Point(int.MaxValue, int.MinValue), new Models.Point(int.MinValue, int.MinValue),
                new Models.Point(int.MaxValue, int.MaxValue), new Models.Point(int.MinValue, int.MaxValue));

            var result = new Models.Rectangle(mainRectangle.Color,
                new Models.Point(int.MaxValue, int.MinValue), new Models.Point(int.MinValue, int.MinValue),
                new Models.Point(int.MaxValue, int.MaxValue), new Models.Point(int.MinValue, int.MaxValue));

            var outOfMain = 0;

            foreach (var rect in rectangles)
            {
                if(!includedColors.Contains(rect.Color)) continue;

                if(!mainRectangle.Contains(rect) && !includeExternalPoints && !mainRectangle.ContainsAnyPoint(rect))
                {
                    outOfMain++;
                    continue;
                }
                else if(!mainRectangle.ContainsAnyPoint(rect) && includeExternalPoints)
                {
                    outOfMain++;
                    continue;
                }
                if(includeExternalPoints)
                {
                    if(!mainRectangle.ContainsAnyPoint(rect)) continue;

                    if(rect.TopLeft.X <= result.TopLeft.X) result.TopLeft.X = rect.TopLeft.X;
                    if(rect.TopLeft.Y <= result.TopLeft.Y) result.TopLeft.Y = rect.TopLeft.Y;

                    if(rect.TopRight.X >= result.TopRight.X) result.TopRight.X = rect.TopRight.X;
                    if(rect.TopRight.Y <= result.TopRight.Y) result.TopRight.Y = rect.TopRight.Y;

                    if(rect.BotLeft.X <= result.BotLeft.X) result.BotLeft.X = rect.BotLeft.X;
                    if(rect.BotLeft.Y >= result.BotLeft.Y) result.BotLeft.Y = rect.BotLeft.Y;

                    if(rect.BotRight.X >= result.BotRight.X) result.BotRight.X = rect.BotRight.X;
                    if(rect.BotRight.Y >= result.BotRight.Y) result.BotRight.Y = rect.BotRight.Y;
                }
                else
                {                   
                    if(rect.TopLeft.X >= mainRectangle.TopLeft.X && rect.TopLeft.X <= result.TopLeft.X) result.TopLeft.X = rect.TopLeft.X;
                    if(rect.TopLeft.Y >= mainRectangle.TopLeft.Y && rect.TopLeft.Y <= result.TopLeft.Y) result.TopLeft.Y = rect.TopLeft.Y;

                    if(rect.TopRight.X <= mainRectangle.TopRight.X && rect.TopRight.X >= result.TopRight.X) result.TopRight.X = rect.TopRight.X;
                    if(rect.TopRight.Y >= mainRectangle.TopRight.Y && rect.TopRight.Y <= result.TopRight.Y) result.TopRight.Y = rect.TopRight.Y;

                    if(rect.BotLeft.X >= mainRectangle.BotLeft.X && rect.BotLeft.X <= result.BotLeft.X) result.BotLeft.X = rect.BotLeft.X;
                    if(rect.BotLeft.Y <= mainRectangle.BotLeft.Y && rect.BotLeft.Y >= result.BotLeft.Y) result.BotLeft.Y = rect.BotLeft.Y;

                    if(rect.BotRight.X <= mainRectangle.BotRight.X && rect.BotRight.X >= result.BotRight.X) result.BotRight.X = rect.BotRight.X;
                    if(rect.BotRight.Y <= mainRectangle.BotRight.Y && rect.BotRight.Y >= result.BotRight.Y) result.BotRight.Y = rect.BotRight.Y;
                }
            }

            return outOfMain == rectangles.Count() || result.Equals(r) ? null : result;
        }
    }
}
