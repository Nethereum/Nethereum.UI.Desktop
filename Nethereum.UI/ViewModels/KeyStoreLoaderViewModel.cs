using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;

namespace Nethereum.UI.ViewModels
{
    public class KeyStoreLoaderViewModel : ReactiveObject
    {
        private string _fileName;
        private string _password;

        private readonly ReactiveCommand<Unit, string> _loadPrivateKeyCommand;
        public ReactiveCommand<Unit, string> LoadPrivateKeyCommand => _loadPrivateKeyCommand;

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public KeyStoreLoaderViewModel()
        {
            IObservable<bool> canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.FileName,
                x => x.Password,
                (fileName, password) => !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(password));

            _loadPrivateKeyCommand = ReactiveCommand.Create(LoadPrivateKey, canExecuteLoadPrivateKey);
        }

        public string LoadPrivateKey()
        {
            if (string.IsNullOrEmpty(Password))
            {
                throw new NullReferenceException("Password cannot be null or empty");
            }

            if (string.IsNullOrEmpty(FileName))
            {
                throw new NullReferenceException("File name cannot be null or empty");
            }

            if (!File.Exists(FileName))
            {
                throw new Exception("File cannot be found");
            }

            using (StreamReader file = System.IO.File.OpenText(FileName))
            {
                string json = file.ReadToEnd();
                //create a new Nethereum key store service
                KeyStoreService service = new KeyStoreService();
                //decrypt the json using the password and get the private key
                string privateKey = service.DecryptKeyStoreFromJson(Password, json).ToHex();

                Account account = new Account(privateKey);

                MessageBus.Current.SendMessage(new AccountLoaded(account));

                return privateKey;
            }
        }
    }
}