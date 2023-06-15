using Avalonia.Controls;
using PDF_Text_Extractor.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDF_Text_Extractor.Commands
{
    public class ChooseFileCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Window _currentWindow;

        public ChooseFileCommand(MainWindowViewModel mainWindowViewModel, Window currentWindow)
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

        public override async void Execute(object? parameter)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            FileDialogFilter filters = new FileDialogFilter();
            filters.Name = "TXT Files (.txt)";
            filters.Extensions.Add("txt");

            sfd.Filters = new List<FileDialogFilter>();
            sfd.Filters.Add(filters);

            string theFile = await sfd.ShowAsync(_currentWindow) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(theFile))
            {
                _mainWindowViewModel.OutputFile = theFile;
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
