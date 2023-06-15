using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PDF_Text_Extractor.Views
{
    public partial class MessageBoxView : Window
    {
        public MessageBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
