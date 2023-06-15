using Avalonia.Controls;
using PDF_Text_Extractor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Text_Extractor.Commands
{
    public class ChooseFolderCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Window _currentWindow;
        public ChooseFolderCommand(MainWindowViewModel mainWindowViewModel, Window currentWindow)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _currentWindow = currentWindow;

            _mainWindowViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !_mainWindowViewModel.Busy &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();

            string theDirectory = ofd.ShowAsync(_currentWindow)?.Result ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(theDirectory))
            {
                _mainWindowViewModel.SourceFolder = theDirectory;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Busy))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
