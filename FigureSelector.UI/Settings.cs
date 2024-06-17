namespace FigureSelector.UI
{
    internal class Settings
    {
        public System.Drawing.Color RectangleColor { get; set; } = System.Drawing.Color.White;
        public List<System.Drawing.Color> IncludedColors { get; set; } = new();
        public List<System.Drawing.Color> IgnoredColors { get; set; } = new();
    }
}
