using Avalonia.Controls;
using PDF_Text_Extractor.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace PDF_Text_Extractor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private readonly Window _currentWindow;
        #endregion

        #region Properties
        private string outputFile = string.Empty;
        public string OutputFile
        {
            get
            {
                return outputFile;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref outputFile, value);
            }
        }

        private string sourceFolder = string.Empty;
        public string SourceFolder
        {
            get
            {
                return sourceFolder;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref sourceFolder, value);
            }

        }

        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _busy, value);
            }
        }

        private string extractingText = "Extract";
        public string ExtractingText
        {
            get
            {
                return extractingText;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref extractingText, value);
            }
        }
        #endregion

        #region Commands
        public ICommand ChooseFileCommand { get; }
        public ICommand ChooseFolderCommand { get; }
        public ICommand ExtractCommand { get; }
        #endregion

        public MainWindowViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;

            ChooseFileCommand = new ChooseFileCommand(this, _currentWindow);
            ChooseFolderCommand = new ChooseFolderCommand(this, _currentWindow);
            ExtractCommand = new ExtractCommand(currentWindow, this);
        }
    }
}
