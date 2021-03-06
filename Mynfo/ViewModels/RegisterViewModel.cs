﻿namespace Mynfo.ViewModels
{
    using Domain;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class RegisterViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

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

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Confirm
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.ImageSource = "no_image";
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }
        private async void Register()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }
            if (!RegexUtilities.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Confirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation,
                    Languages.Accept);
                return;
            }
            if (this.Password != this.Confirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation2,
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var user = new User
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                ImageArray = imageArray,
                UserTypeId = 1,
                Password = this.Password,
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();

            var exists = await this.apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                this.Email);

            if(exists != null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.WrongEmail,
                    Languages.Accept);
                return;
            }

            var response = await this.apiService.Post2(
                apiSecurity,
                "/api",
                "/Users",
                user);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                Languages.ConfirmLabel,
                Languages.UserRegisteredMessage,
                Languages.Accept);
            var mainViewModel = MainViewModel.GetInstance().User;
            await Application.Current.MainPage.Navigation.PopAsync();

            this.Email = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Password = string.Empty;
            this.Confirm = string.Empty;
            this.ImageSource = "no_image";
        }

        public ICommand NextCommand
        {
            get
            {
                return new RelayCommand(Next);
            }
        }
        private async void Next()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }
            //char[] A = FirstName.ToCharArray();
            //foreach(char B in A)
            //{

            //    if (!Char.IsLetter(B) || !Char.IsWhiteSpace(B))
            //    {
            //        await Application.Current.MainPage.DisplayAlert(
            //            Languages.Error,
            //            Languages.FirstNameValidation,
            //            Languages.Accept);
            //        return;
            //    }
            //}
            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }

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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var user = new User
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                ImageArray = imageArray,
                UserTypeId = 1,
                Password = this.Password,
            };

            MainViewModel.GetInstance().Register2 = new Register2ViewModel(user);
            await Application.Current.MainPage.Navigation.PushAsync(new Register2Page());
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();
            try
            {

                if (CrossMedia.Current.IsCameraAvailable &&
                    CrossMedia.Current.IsTakePhotoSupported)
                {
                    var source = await Application.Current.MainPage.DisplayActionSheet(
                        Languages.SourceImageQuestion,
                        Languages.Cancel,
                        null,
                        Languages.FromGallery,
                        Languages.FromCamera);

                    if (source == Languages.Cancel)
                    {
                        this.file = null;
                        return;
                    }

                    if (source == Languages.FromCamera)
                    {
                        this.file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                SaveToAlbum = true,
                                Directory = "mynfo",
                                Name = "ProfileImage.jpg",
                                PhotoSize = PhotoSize.Small,
                                CompressionQuality = 50
                            }
                        );
                    }
                    else
                    {
                        //this.file = await CrossMedia.Current.PickPhotoAsync();
                        this.file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {
                            CompressionQuality = 50
                        });
                    }
                }

                else
                {
                    //this.file = await CrossMedia.Current.PickPhotoAsync();
                    this.file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CompressionQuality = 50
                    });
                }

                if (this.file != null)
                {
                    this.ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                }

                return;
            }
            catch(Exception e)
            {
                return;
            }
            
        }
        #endregion
    }
}