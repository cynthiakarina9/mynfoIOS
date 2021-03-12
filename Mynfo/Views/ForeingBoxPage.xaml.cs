﻿namespace Mynfo.Views
{
    using Mynfo.Helpers;
    using Mynfo.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.OpenWhatsApp;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForeingBoxPage 
    {
        #region Properties
        public string data = Data_ntc.data_value;
        public ProfileLocal selectedItem { get; set; }
        #endregion

        #region Constructor
        public ForeingBoxPage(ForeingBox _foreingBox, bool isAfterReceiving = false)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            BackG.CloseWhenBackgroundIsClicked = true;

            #region DataFill
            ForeingBox foreing = _foreingBox;

            ForeignUserImage.Source = foreing.ImageFullPath;
            ForeignUserName.Text = foreing.FullName;
            if(foreing.Edad != 0)
                ForeignAge.Text = foreing.Edad + Languages.Anios;
            if (foreing.Ubicacion != "")
                ForeignLocation.Text = foreing.Ubicacion;
            if (foreing.Ubicacion != "")
                ForeignJob.Text = foreing.Ocupacion;
            ForeignConnection.Text = Languages.ViewsLabel + foreing.Conexiones;
            #endregion
        }
        #endregion

        #region Command
        private void Back_Clicked(object sender, EventArgs e, bool isAfterReceiving)
        {
            if (isAfterReceiving == true)
            {

                Application.Current.MainPage = new MasterPage();
            }
            else
            {
                Navigation.PopAsync();
            }
        }
        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = e.CurrentSelection.FirstOrDefault() as ProfileLocal;
            if (selectedItem == null)
                return;
            switch (selectedItem.ProfileType)
            {
                case "Email":
                    await Launcher.OpenAsync(new Uri("mailto:" + selectedItem.value));
                    break;

                case "Instagram":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Facebook":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "LinkedIn":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Phone":
                    PhoneDialer.Open(selectedItem.value);
                    break;
                case "Snapchat":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Spotify":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "TikTok":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Twitch":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Twitter":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "WebPage":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                case "Whatsapp":
                    try
                    {
                        string language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                        switch (language)
                        {
                            case "en":
                                Chat.Open("52" + selectedItem.value, "Hi%20from%20mynfo!");
                                break;
                            case "es":
                                Chat.Open("52" + selectedItem.value, "Hola%20desde%20mynfo!");
                                break;
                            case "pt":
                                Chat.Open("52" + selectedItem.value, "Olá%20de%20mynfo!");
                                break;
                            default:
                                Chat.Open("52" + selectedItem.value, "Hi%20from%20mynfo!");
                                break;
                        }
                        //Chat.Open("52" + selectedItem.value, Languages.MessageWhatsApp + " " + MainViewModel.GetInstance().User.FirstName);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert(Languages.Error, ex.Message, Languages.Accept);
                    }
                    break;
                case "Youtube":
                    await Launcher.OpenAsync(new Uri(selectedItem.value));
                    break;
                default:
                    break;
            }
            ((CollectionView)sender).SelectedItem = null;
        }        
        #endregion
    }
}