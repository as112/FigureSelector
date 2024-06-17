using FigureSelector.Core.Models;
using FigureSelector.Core.Services;
using System.ComponentModel.Design;
using System.Drawing;

namespace FigureSelector.Tests
{
    public class SelectorServiceTest
    {
        private List<Core.Models.Rectangle> _rects;
        private List<Color> _includedColors;
        private ISelectorService _selectionService; 

        public SelectorServiceTest()
        {
            _rects = new List<Core.Models.Rectangle>();
            _rects.Add(new Core.Models.Rectangle(Color.AliceBlue,
                    new Core.Models.Point(2, 8),
                    new Core.Models.Point(6, 8),
                    new Core.Models.Point(2, 3),
                    new Core.Models.Point(6, 3)));
            _rects.Add(new Core.Models.Rectangle(Color.AliceBlue,
                    new Core.Models.Point(3, 6),
                    new Core.Models.Point(8, 6),
                    new Core.Models.Point(3, 1),
                    new Core.Models.Point(8, 1)));
            _rects.Add(new Core.Models.Rectangle(Color.AliceBlue,
                    new Core.Models.Point(4, 9),
                    new Core.Models.Point(9, 9),
                    new Core.Models.Point(4, 2),
                    new Core.Models.Point(9, 2)));
            
            _includedColors = new List<Color>();
            _includedColors.Add(Color.AliceBlue);

            _selectionService = new SelectorService();
        }

        [Test]
        public void When_NoCrossing_ShouldReturnNull()
        {
            var includeExternalPoints = false;
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 10),
                    new Core.Models.Point(1, 10),
                    new Core.Models.Point(0, 9),
                    new Core.Models.Point(1, 9));
            var result = _selectionService.Select(mainRectangle, _rects, _includedColors, includeExternalPoints);
            Assert.Null(result);
        }
        [Test]
        public void When_SelectAll_ShouldReturnAll()
        {
            var includeExternalPoints = false;
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 10),
                    new Core.Models.Point(10, 10),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(10, 0));
            var expected = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(2, 9),
                    new Core.Models.Point(9, 9),
                    new Core.Models.Point(2, 1),
                    new Core.Models.Point(9, 1));
            var result = _selectionService.Select(mainRectangle, _rects, _includedColors, includeExternalPoints);
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test]
        public void When_SelectWithoutSomePoints_IncludeExt()
        {
            var includeExternalPoints = true;
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 10),
                    new Core.Models.Point(5, 10),
                    new Core.Models.Point(0, 4),
                    new Core.Models.Point(5, 4));
            var expected = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(2, 9),
                    new Core.Models.Point(9, 9),
                    new Core.Models.Point(2, 1),
                    new Core.Models.Point(9, 1));
            var result = _selectionService.Select(mainRectangle, _rects, _includedColors, includeExternalPoints);
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test]
        public void When_SelectWithoutSomePoints_IgnoreExt()
        {
            var includeExternalPoints = false;
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 10),
                    new Core.Models.Point(7, 10),
                    new Core.Models.Point(0, 2),
                    new Core.Models.Point(7, 2));
            var expected = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(2, 9),
                    new Core.Models.Point(6, 9),
                    new Core.Models.Point(2, 2),
                    new Core.Models.Point(6, 2));
            var result = _selectionService.Select(mainRectangle, _rects, _includedColors, includeExternalPoints);
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test]
        public void When_SelectAll_DifferentColors()
        {
            var includeExternalPoints = false;
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 10),
                    new Core.Models.Point(10, 10),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(10, 0));

            var diffColorRect = new Core.Models.Rectangle(Color.Red,
                    new Core.Models.Point(1, 9),
                    new Core.Models.Point(8, 9),
                    new Core.Models.Point(1, 1),
                    new Core.Models.Point(8, 1));
            _rects.Add(diffColorRect);

            var expected = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(2, 9),
                    new Core.Models.Point(9, 9),
                    new Core.Models.Point(2, 1),
                    new Core.Models.Point(9, 1));
            var result = _selectionService.Select(mainRectangle, _rects, _includedColors, includeExternalPoints);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}