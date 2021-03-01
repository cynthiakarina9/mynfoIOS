﻿namespace Mynfo.ViewModels
{
    using Domain;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Mynfo.Models;
    using Mynfo.Views;
    using Rg.Plugins.Popup.Services;
    using Services;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class CreateProfileFacebookViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Atributtes
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public string Name
        {
            get;
            set;
        }
        public string Link
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public CreateProfileFacebookViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand SaveProfileFacebookCommand
        {
            get
            {
                return new RelayCommand(SaveProfileFacebook);
            }
        }
        private async void SaveProfileFacebook()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameValidation,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Link))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LinkValidation,
                    Languages.Accept);
                return;
            }
            
            if (!RegexUtilities.IsValidURL(this.Link))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LinkValidation,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var profileFB = new ProfileSM
            {
                ProfileName = this.Name,
                link = this.Link,
                UserId = mainViewModel.User.UserId,
                Exist = false,
                RedSocialId = 1
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var profileSM = await this.apiService.Post(
                apiSecurity,
                "/api",
                "/ProfileSMs",
                profileFB);

            if (profileSM == default)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorAddProfile,
                    Languages.Accept);
                return;
            }
            var ProfileLocal = new Profile
            {
                UserId = mainViewModel.User.UserId,
                ProfileName = profileSM.ProfileName,
                value = profileSM.link,
                ProfileType = "Facebook",
                Logo = "facebook2",
                ProfileId = profileSM.ProfileMSId,
            };
            using (var conn = new SQLite.SQLiteConnection(App.root_db))
            {
                conn.CreateTable<Profile>();
                conn.Insert(ProfileLocal);
            }
            this.IsRunning = false;
            this.IsEnabled = true;

            //Agregar a la lista
            if(mainViewModel.ProfilesBYPESM != null)
            {
                mainViewModel.ProfilesBYPESM.addProfileSM(profileSM);
                mainViewModel.ListOfNetworks.addProfileSM(profileSM);
            }
            else
            {
                mainViewModel.ProfilesByFacebook.addProfile(profileSM);
            }
            
            
            this.Name = string.Empty;
            this.Link = string.Empty;

            if (mainViewModel.ProfilesBYPESM != null)
            {
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await App.Navigator.PopAsync();
            }
        }

        public ICommand GotoGIFCommand
        {
            get
            {
                return new RelayCommand(GotoGIF);
            }
        }
        private async void GotoGIF()
        {
            MainViewModel.GetInstance().GIF = new GifViewModel("Facebook");
            await PopupNavigation.Instance.PushAsync(new GifPage());
        }
        public ICommand BackHomeCommand
        {
            get
            {
                return new RelayCommand(BackHome);
            }
        }
        private void BackHome()
        {
            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}
