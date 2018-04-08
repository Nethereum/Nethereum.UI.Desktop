using System;
using System.IO;
using System.Reactive;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class KeyStoreLoaderViewModel : ReactiveObject
    {
        private string _fileName;
        private string _password;

        private readonly ReactiveCommand<Unit, string> _loadPrivateKeyCommand;
        public ReactiveCommand<Unit, string> LoadPrivateKeyCommand => this._loadPrivateKeyCommand;

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
            var canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.FileName,
                x => x.Password,
                (fileName, password) => !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(password));

            this._loadPrivateKeyCommand = ReactiveCommand.Create(this.LoadPrivateKey, canExecuteLoadPrivateKey);
        } 

        public string LoadPrivateKey()
        {
            if (String.IsNullOrEmpty(Password)) throw new NullReferenceException("Password cannot be null or empty");
            if (String.IsNullOrEmpty(FileName)) throw new NullReferenceException("File name cannot be null or empty");
            if (!File.Exists(FileName)) throw new Exception("File cannot be found");
            using(var file = System.IO.File.OpenText(FileName))
            {
                var json = file.ReadToEnd();
                //create a new Nethereum key store service
                var service = new KeyStoreService();
                //decrypt the json using the password and get the private key
                var privateKey = service.DecryptKeyStoreFromJson(Password, json).ToHex();

                var account = new Account(privateKey);

                MessageBus.Current.SendMessage(new AccountLoaded(account));

                return privateKey;
            }
        }
    }
}