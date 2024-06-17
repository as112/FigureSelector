using System.Drawing;

namespace FigureSelector.Core.Services
{
    public interface ISelectorService
    {
        Models.Rectangle? Select(Models.Rectangle mainRectangle, IEnumerable<Models.Rectangle> rectangles, IEnumerable<Color> includedColors, bool includeExternalPoints);
    }
}