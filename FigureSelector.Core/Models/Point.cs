using Microsoft.Win32.SafeHandles;

namespace FigureSelector.Core.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public override string ToString() 
        {
            return $"[{X};{Y}]";
        }
    }
}