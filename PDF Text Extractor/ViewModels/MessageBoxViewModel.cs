using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Windows.Input;
using PDF_Text_Extractor.Commands;
using System.ComponentModel;

namespace PDF_Text_Extractor.ViewModels
{
    public class MessageBoxViewModel : ReactiveObject
    {
        private readonly Window _currentWindow;

        private string _messageText = string.Empty;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _messageText, value);
            }
        }

        public ICommand OKCommand { get; }

        public MessageBoxViewModel(Window currentWindow, string messageText)
        {
            MessageText = messageText;
            _currentWindow = currentWindow;

            OKCommand = new OKCommand(_currentWindow);

            _currentWindow.Opened += OnWindowOpened;
            _currentWindow.Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            _currentWindow.Opened -= OnWindowOpened;
            _currentWindow.Closing -= OnWindowClosing;
        }

        private void OnWindowOpened(object? sender, EventArgs e)
        {
            _currentWindow.FindControl<Button>("btnOk")?.Focus();
        }
    }
}
