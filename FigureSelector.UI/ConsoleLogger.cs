namespace FigureSelector.UI
{
    public class ConsoleLogger
    {
        private readonly Action<string> _writeAction;

        public ConsoleLogger(Action<string> writeAction)
        {
            _writeAction = writeAction;
        }

        public void Log(string message)
        {
            _writeAction?.Invoke(message);
        }
    }
}
