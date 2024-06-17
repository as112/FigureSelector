using FigureSelector.Core.Models;
using FigureSelector.Core.Services;
using System.Drawing;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FigureSelector.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Point _startPoint;
        private System.Windows.Shapes.Rectangle _drawingRectangle;
        private bool isDrawing = false;
        private Settings _settings;
        private ConsoleLogger _consoleLogger;
        private FileLogger _fileLogger;
        private ISelectorService _selectorService;
        private List<Core.Models.Rectangle> _rectangleList;

        public MainWindow()
        {
            InitializeComponent();
            _selectorService = new SelectorService();
            _settings = new Settings();
            _rectangleList = new List<Core.Models.Rectangle>();
            _consoleLogger = new ConsoleLogger(LogToTextBox);
            _fileLogger = new FileLogger("log.txt");
            ConsoleLogCheckBox.IsChecked = true;
            FileLogCheckBox.IsChecked = true;
            IncludeExternalPointsCheckBox.IsChecked = false;
            MainButton.IsEnabled = true;
            SecondaryButton.IsEnabled = false;
            PopulateColorListBox();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing) return;

            _startPoint = e.GetPosition(DrawCanvas);
            _drawingRectangle = new System.Windows.Shapes.Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(_drawingRectangle, _startPoint.X);
            Canvas.SetTop(_drawingRectangle, _startPoint.Y);
            DrawCanvas.Children.Add(_drawingRectangle);
            isDrawing = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            var pos = e.GetPosition(DrawCanvas);
            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);
            var width = Math.Abs(pos.X - _startPoint.X);
            var height = Math.Abs(pos.Y - _startPoint.Y);

            _drawingRectangle.Width = width;
            _drawingRectangle.Height = height;

            Canvas.SetLeft(_drawingRectangle, x);
            Canvas.SetTop(_drawingRectangle, y);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDrawing) return;

            var endPoint = e.GetPosition(DrawCanvas);

            var botLeft = new Core.Models.Point(Math.Min(_startPoint.X, endPoint.X), Math.Max(_startPoint.Y, endPoint.Y));
            var botRight = new Core.Models.Point(Math.Max(_startPoint.X, endPoint.X), Math.Max(_startPoint.Y, endPoint.Y));
            var topLeft = new Core.Models.Point(Math.Min(_startPoint.X, endPoint.X), Math.Min(_startPoint.Y, endPoint.Y));
            var topRight = new Core.Models.Point(Math.Max(_startPoint.X, endPoint.X), Math.Min(_startPoint.Y, endPoint.Y));

            if (botLeft == topRight)
            {
                isDrawing = false;
                return;
            }

            var rectangleData = new Core.Models.Rectangle(_settings.RectangleColor, botLeft, botRight, topLeft, topRight);

            DrawCanvas.Children.Remove(_drawingRectangle);
            DrawRectangle(DrawCanvas, rectangleData, !MainButton.IsEnabled);
            isDrawing = false;
        }

        public void DrawRectangle(Canvas canvas, Core.Models.Rectangle rectangle, bool isMainRectangle)
        {
            var color = isMainRectangle ? Colors.Transparent : ConvertToMediaColor(rectangle.Color);
            var strokeThickness = isMainRectangle ? 3 : 1;
            var stroke = isMainRectangle ? Colors.Brown : Colors.Black;
            var rect = isMainRectangle ? _selectorService.Select(rectangle, _rectangleList, _settings.IncludedColors, (bool)IncludeExternalPointsCheckBox.IsChecked!) : rectangle;
            if (rect == null) return;
            Polygon polygon = new Polygon
            {
                Stroke = new SolidColorBrush(stroke),
                Fill = new SolidColorBrush(color),
                StrokeThickness = strokeThickness,
                Points = new PointCollection
                {
                    new System.Windows.Point(rect.BotLeft.X, rect.BotLeft.Y),
                    new System.Windows.Point(rect.BotRight.X, rect.BotRight.Y),
                    new System.Windows.Point(rect.TopRight.X, rect.TopRight.Y),
                    new System.Windows.Point(rect.TopLeft.X, rect.TopLeft.Y)
                }
            };

            canvas.Children.Add(polygon);
            if (!_settings.IncludedColors.Contains(rectangle.Color) && !isMainRectangle)
            {
                _settings.IncludedColors.Add(rectangle.Color);
                IncludedColorsListBox.Items.Add(_settings.RectangleColor);
            }
            if(!isMainRectangle) _rectangleList.Add(rectangle);
            WriteLog($"A rectangle was drawn at {rectangle.ToString()}.");
        }
        private void WriteLog(string message)
        {
            var log = $"{DateTimeOffset.Now}: {message}";
            if ((bool)ConsoleLogCheckBox.IsChecked!) _consoleLogger.Log(log);
            if ((bool)FileLogCheckBox.IsChecked!) _fileLogger.Log(log);
        }
        private void LogToTextBox(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogTextBox.AppendText(message + Environment.NewLine);
                LogTextBox.ScrollToEnd();
            });
        }
        private void PopulateColorListBox()
        {
            ColorComboBox.ItemsSource = typeof(Colors).GetProperties()
                .Select(p => p.Name)
                .ToList();
            ColorComboBox.SelectedIndex = 0;
        }
        public static System.Windows.Media.Color ConvertToMediaColor(System.Drawing.Color drawingColor)
        {
            return System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorComboBox.SelectedItem != null)
            {
                var colorName = ColorComboBox.SelectedItem.ToString();
                _settings.RectangleColor = System.Drawing.Color.FromName(colorName);

                WriteLog($"Selected color changed to {_settings.RectangleColor}.");
            }
        }
        private void IncludedColorsListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement((ListBox)sender, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                var color = (System.Drawing.Color)item.Content;
                IncludedColorsListBox.Items.Remove(color);
                _settings.IncludedColors.Remove(color);

                _settings.IgnoredColors.Add(color);
                IgnoredColorsListBox.Items.Add(color);
                WriteLog($"Removed color {color}.");
            }
        }
        private void IgnoredColorsListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement((ListBox)sender, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                var color = (System.Drawing.Color)item.Content;

                IgnoredColorsListBox.Items.Remove(color);
                _settings.IgnoredColors.Remove(color);

                IncludedColorsListBox.Items.Add(color);
                _settings.IncludedColors.Add(color);
                WriteLog($"Added color {color}.");
            }
        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            SecondaryButton.IsEnabled = !SecondaryButton.IsEnabled;
            MainButton.IsEnabled = !MainButton.IsEnabled;
            WriteLog($"Main rectangle mode is activated");
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            SecondaryButton.IsEnabled = !SecondaryButton.IsEnabled;
            MainButton.IsEnabled = !MainButton.IsEnabled;
            WriteLog($"Secondary rectangle mode is activated");

        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _rectangleList.Clear();
            DrawCanvas.Children.Clear();
            _settings.IncludedColors.Clear();
            _settings.IgnoredColors.Clear();
            IncludedColorsListBox.Items.Clear();
        }
    }
}