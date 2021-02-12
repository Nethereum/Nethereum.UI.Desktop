using System;
using System.IO;
using System.Reactive;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.UI.HostProvider.Services;
using Nethereum.Web3.Accounts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nethereum.UI.ViewModels
{
    public class KeyStoreLoaderViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> _loadPrivateKeyCommand;
        private readonly AccountsService accountsService;

        public ReactiveCommand<Unit, Unit> LoadPrivateKeyCommand => this._loadPrivateKeyCommand;

        [Reactive] public string FileName { get; set; }
        [Reactive] public string Password { get; set; }


        protected KeyStoreLoaderViewModel()
        {
            //Designer only..
        }

        public KeyStoreLoaderViewModel(AccountsService accountsService)
        {
            var canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.FileName,
                x => x.Password,
                (fileName, password) => !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(password));

            this._loadPrivateKeyCommand = ReactiveCommand.Create(this.LoadPrivateKey, canExecuteLoadPrivateKey);
            this.accountsService = accountsService;
        } 

        public void LoadPrivateKey()
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
                accountsService.AddAccount(account);
            }
        }
    }
}