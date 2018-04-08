using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views
{
    public class KeystoreAccountLoaderUserControl : UserControl
    {
        public KeystoreAccountLoaderUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("btnOpenFile").Click += async delegate 
            {
                var openfileDialog = new OpenFileDialog()
                {
                    Title = "Open file"
                };

                var files = await openfileDialog.ShowAsync(GetWindow());
                if (files != null && files.Length > 0)
                {
                    var vm = (KeyStoreLoaderViewModel) DataContext;
                    if (vm != null)
                    {
                        vm.FileName = files[0];
                    }
                }
            };
        }

        Window GetWindow() => (Window)this.VisualRoot;

    }
}
