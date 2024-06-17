using System.Drawing;

namespace FigureSelector.Core.Models
{
    public class Rectangle
    {
        public Color Color { get; set; }
        public Point BotLeft { get; set; }
        public Point BotRight { get; set; }
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }

        public Rectangle(Color color, Point botLeft, Point botRight, Point topLeft, Point topRight)
        {
            Color = color;
            BotLeft = botLeft;
            BotRight = botRight;
            TopLeft = topLeft;
            TopRight = topRight;
        }

        public bool Contains(Rectangle other)
        {
            return other.TopLeft.X >= this.TopLeft.X &&
                other.TopLeft.Y >= this.TopLeft.Y &&
                other.TopRight.X <= this.TopRight.X &&
                other.TopRight.Y >= this.TopRight.Y &&
                other.BotLeft.X >= this.BotLeft.X &&
                other.BotLeft.Y <= this.BotLeft.Y &&
                other.BotRight.X <= this.BotRight.X &&
                other.BotRight.Y <= this.BotRight.Y;
        }
        public bool ContainsAnyPoint(Rectangle other)
        {
            return IsPointInside(other.TopLeft) ||
                   IsPointInside(other.TopRight) ||
                   IsPointInside(other.BotLeft) ||
                   IsPointInside(other.BotRight);
        }

        private bool IsPointInside(Point point)
        {
            return point.X >= this.TopLeft.X &&
                   point.X <= this.TopRight.X &&
                   point.Y >= this.TopLeft.Y &&
                   point.Y <= this.BotLeft.Y;
        }
        public override bool Equals(object? obj)
        {
            return obj is Rectangle rectangle &&
                   Color.Equals(rectangle.Color) &&
                   EqualityComparer<Point>.Default.Equals(BotLeft, rectangle.BotLeft) &&
                   EqualityComparer<Point>.Default.Equals(BotRight, rectangle.BotRight) &&
                   EqualityComparer<Point>.Default.Equals(TopLeft, rectangle.TopLeft) &&
                   EqualityComparer<Point>.Default.Equals(TopRight, rectangle.TopRight);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, BotLeft, BotRight, TopLeft, TopRight);
        }
        public override string ToString()
        {
            return $"'{TopLeft}{TopRight}{BotLeft}{BotRight} {Color}'";
        }
    }
    
}
