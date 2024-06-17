using System.IO;
using System.Text;
using System.Windows;

namespace FigureSelector.UI
{
    public class FileLogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
