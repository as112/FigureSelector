using FigureSelector.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigureSelector.Tests
{
    public class RectangleTests
    {
        private Core.Models.Rectangle _rectangle;
        public RectangleTests()
        {
            _rectangle = new Core.Models.Rectangle(Color.AliceBlue,
                    new Core.Models.Point(5, 10),
                    new Core.Models.Point(10, 10),
                    new Core.Models.Point(5, 5),
                    new Core.Models.Point(10, 5));
        }

        [Test]
        public void When_NotContains_ShouldReturnFalse()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 2),
                    new Core.Models.Point(2, 2),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(2, 0));
            var result = mainRectangle.Contains(_rectangle);
            Assert.False(result);
        }
        [Test]
        public void When_NotContainsAnyPoint_ShouldReturnFalse()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 2),
                    new Core.Models.Point(2, 2),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(2, 0));
            var result = mainRectangle.ContainsAnyPoint(_rectangle);
            Assert.False(result);
        }
        [Test]
        public void When_Contains_ShouldReturnTrue()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 12),
                    new Core.Models.Point(12, 12),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(12, 0));
            var result = mainRectangle.Contains(_rectangle);
            Assert.True(result);
        }
        [Test]
        public void When_ContainsBotLeft_ShouldReturnTrue()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 12),
                    new Core.Models.Point(7, 12),
                    new Core.Models.Point(0, 7),
                    new Core.Models.Point(7, 7));
            var result = mainRectangle.ContainsAnyPoint(_rectangle);
            Assert.True(result);
        }
        [Test]
        public void When_ContainsBotRight_ShouldReturnTrue()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(7, 12),
                    new Core.Models.Point(12, 12),
                    new Core.Models.Point(7, 7),
                    new Core.Models.Point(12, 7));
            var result = mainRectangle.ContainsAnyPoint(_rectangle);
            Assert.True(result);
        }
        [Test]
        public void When_ContainsTopLeft_ShouldReturnTrue()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(0, 7),
                    new Core.Models.Point(7, 7),
                    new Core.Models.Point(0, 0),
                    new Core.Models.Point(7, 0));
            var result = mainRectangle.ContainsAnyPoint(_rectangle);
            Assert.True(result);
        }
        [Test]
        public void When_ContainsTopRight_ShouldReturnTrue()
        {
            var mainRectangle = new Core.Models.Rectangle(Color.Transparent,
                    new Core.Models.Point(7, 7),
                    new Core.Models.Point(12, 7),
                    new Core.Models.Point(7, 0),
                    new Core.Models.Point(12, 0));
            var result = mainRectangle.ContainsAnyPoint(_rectangle);
            Assert.True(result);
        }
    }
}
