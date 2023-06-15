using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace PDF_Text_Extractor.Services
{
    public class FileAccessService
    {
        /// <summary>
        /// Loads a PDF document.
        /// </summary>
        /// <param name="fileName">PDF file to load.</param>
        /// <returns>PDF document.</returns>
        public static async Task<PdfDocument> LoadPDFDocumentAsync(string fileName)
        {
            return await Task.Run(() => PdfDocument.Open(File.OpenRead(fileName)));
        }

        /// <summary>
        /// Gets the name of a file.
        /// </summary>
        /// <param name="fileName">The file.</param>
        /// <returns>The file name.</returns>
        public static async Task<string> GetFileNameAsync(string fileName)
        {
            return await Task.Run(() => Path.GetFileNameWithoutExtension(fileName));
        }

        /// <summary>
        /// Saves PDF document text into a CSV file.
        /// </summary>
        /// <param name="textOfDocuments">Text of the PDF document.</param>
        /// <param name="fileName">File name to save into.</param>
        /// <returns>If the operaton succeeded.</returns>
        public static async Task<bool> SaveTextInCSVAsync(List<Dictionary<string, string>> textOfDocuments, string fileName)
        {
            using (CsvFileWriter theWriter = new CsvFileWriter(fileName))
            {
                List<string> columnData = new List<string>();
                string cellData = string.Empty;
                await Task.Run(() =>
                {
                    for (int i = 0; i < textOfDocuments.Count; i++)
                    {
                        columnData.Clear();
                        cellData = string.Empty;

                        foreach (string currentKey in textOfDocuments[i].Keys)
                        {
                            cellData += textOfDocuments[i][currentKey];
                        }
                        
                        columnData.Add(cellData);

                        theWriter.WriteRow(columnData);
                    }
                });
            }

            return true;
        }

        /// <summary>
        /// Saves PDF document text into a text file.
        /// </summary>
        /// <param name="textOfDocuments">Text of the PDF document.</param>
        /// <param name="fileName">File name to save into.</param>
        /// <returns>If the operaton succeeded.</returns>
        public static async Task<bool> SaveTextInTXTAsync(List<Dictionary<string, string>> textOfDocuments, string fileName)
        {
            List<string> currentKeys = new List<string>();
            string currentLine = string.Empty;
            using (TextWriter theWriter = new StreamWriter(File.OpenWrite(fileName)))
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < textOfDocuments.Count; i++)
                    {
                        currentKeys.Clear();
                        currentLine = string.Empty;

                        foreach (string currentKey in textOfDocuments[i].Keys)
                        {
                            await theWriter.WriteLineAsync(currentKey + " - " + textOfDocuments[i][currentKey]);
                        }
                    }
                });
            }

            return true;
        }

        /// <summary>
        /// Gets all files in a directory, including subdirectories.
        /// </summary>
        /// <param name="sourceFolder">Source directory.</param>
        /// <returns>String array of file names.</returns>
        public static async Task<string[]> GetFilesAsync(string sourceFolder)
        {
            string[] fileNames = await Task.Run(() => Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories));

            return fileNames;
        }

    }
}
