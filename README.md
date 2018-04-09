# Nethereum UI Desktop Simple Wallet

Functional sample of a Reactive Cross platform desktop wallet connecting to Ethereum using Ethereum. 
The sample provides a cross platform Desktop port using Avalonia and ReactiveUI. 

The sample provides an overview of loading accounts from different sources (KeyStore, HdWallet backup words, Private Key) and overall interaction with Ethereum to send transactions and interact with a standard token contract.

## Why this sample?

The aim of the sample is to provide another learning exercise of Nethereum, but at the same time work as the MVVM spike towards the full Reactive UI solution, which will be also based on Xamarin.Forms as per the other sample, include hybrid dapps, multi signature, uport, etc, etc.

This sample uses the core MVVM components already leveraged on the Windows Forms [Nethereum Simple Wallet Windows Forms](https://github.com/Nethereum/Nethereum.SimpleWindowsWallet)

NOTE: The sample is not the a hardened solution so there is no error handling, there is not generic validation binded to view models, context handling (everything is done through messaging), decoupling of signing transactions, etc.

## Screenshots

![Ubuntu Load From KeyStore](Screenshots/ubuntu2.PNG)
![Ubuntu Transfer Sample](screenshots/ubuntu1.PNG)
![Mac Load From KeyStore](Screenshots/mac1.PNG)
![Mac Transfer Sample](screenshots/mac2.PNG)
