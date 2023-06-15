using Avalonia.Controls;
using PDF_Text_Extractor.Services;
using PDF_Text_Extractor.ViewModels;
using PDF_Text_Extractor.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace PDF_Text_Extractor.Commands
{
    public class ExtractCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Window _currentWindow;
        public ExtractCommand(Window currentWindow, MainWindowViewModel informationViewModel)
        {
            _currentWindow = currentWindow;
            _mainWindowViewModel = informationViewModel;

            _mainWindowViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_mainWindowViewModel.OutputFile) &&
                !string.IsNullOrEmpty(_mainWindowViewModel.SourceFolder) &&
                !_mainWindowViewModel.Busy &&
                base.CanExecute(parameter);
        }
        public async override void Execute(object? parameter)
        {
            _mainWindowViewModel.Busy = true;
            _mainWindowViewModel.ExtractingText = "Loading Files...";

            await SaveTextAsync();

            _mainWindowViewModel.ExtractingText = "Extract";

            MessageBoxView mboxView = new MessageBoxView();
            mboxView.DataContext = new MessageBoxViewModel(mboxView, "Finished Extracting");
            await mboxView.ShowDialog(_currentWindow);

            _mainWindowViewModel.Busy = false;

        }

        private async Task<bool> SaveTextAsync()
        {
            string[] fileNames = await FileAccessService.GetFilesAsync(_mainWindowViewModel.SourceFolder);
            fileNames = await Task.Run(() => fileNames.Where(x => x.ToLower().EndsWith("pdf")).ToArray());

            _mainWindowViewModel.ExtractingText = "Extracting Text...";
            List<Dictionary<string, string>> textOfDocuments = await GetTextOfDocumentsAsync(fileNames);

            _mainWindowViewModel.ExtractingText = "Saving CSV...";
            return await FileAccessService.SaveTextInTXTAsync(textOfDocuments, _mainWindowViewModel.OutputFile);
        }

        /// <summary>
        /// This method gets the text of all the pages in a PDF document.
        /// </summary>
        /// <param name="fileNames">PDF file name.</param>
        /// <returns>Text of the document organized by filename and page in a dictionary</returns>
        private static async Task<List<Dictionary<string, string>>> GetTextOfDocumentsAsync(string[] fileNames)
        {
            List<Dictionary<string, string>> textOfDocuments = new List<Dictionary<string, string>>();
            for (int i = 0; i < fileNames.Length; i++)
            {
                textOfDocuments.Add(await GetDocumentTextAsync(fileNames[i]));
            }

            return textOfDocuments;
        }

        /// <summary>
        /// Helper method to get all the text in a PDF document.
        /// </summary>
        /// <param name="fileName">PDF document to get the text from</param>
        /// <returns>Dictionary of the text organized by page</returns>
        private static async Task<Dictionary<string, string>> GetDocumentTextAsync(string fileName)
        {
            string title = string.Empty;
            Dictionary<string, string> documentText = new Dictionary<string, string>();

            using (PdfDocument currentDocument = await FileAccessService.LoadPDFDocumentAsync(fileName))
            {
                for (int i = 1; i < currentDocument.NumberOfPages; i++)
                {
                    title = await FileAccessService.GetFileNameAsync(fileName) + " Page " + i;
                    documentText.Add(title, await ExtractPageTextAsync(currentDocument.GetPage(i)));
                }
            }

            return documentText;
        }

        /// <summary>
        /// Returns all the the text on a PDF page as a string seperated by a space asynchronously.
        /// </summary>
        /// <param name="thePage">The PDF page to extract text from.</param>
        /// <returns>The text of the page combined into one spring.</returns>
        private static async Task<string> ExtractPageTextAsync(Page thePage)
        {
            string text = string.Empty;
            List<Word> pageWords = await Task.Run(() => thePage.GetWords().ToList());
            await Task.Run(() =>
            {
                for (int i = 0; i < pageWords.Count; i++)
                {
                    text += pageWords[i].Text + " ";
                }
            });
            return text;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.OutputFile) ||
                e.PropertyName == nameof(MainWindowViewModel.SourceFolder) ||
                e.PropertyName == nameof(MainWindowViewModel.Busy))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
