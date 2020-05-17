using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views
{
    public class KeystoreAccountLoaderUserControl : UserControl
    {
        public KeystoreAccountLoaderUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("btnOpenFile").Click += async delegate
            {
                OpenFileDialog openfileDialog = new OpenFileDialog()
                {
                    Title = "Open file"
                };

                string[] files = await openfileDialog.ShowAsync(GetWindow());
                if (files != null && files.Length > 0)
                {
                    KeyStoreLoaderViewModel vm = (KeyStoreLoaderViewModel)DataContext;
                    if (vm != null)
                    {
                        vm.FileName = files[0];
                    }
                }
            };
        }

        private Window GetWindow()
        {
            return (Window)VisualRoot;
        }
    }
}
