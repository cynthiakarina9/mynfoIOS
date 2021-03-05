﻿namespace Mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Mynfo.Services;
    using Mynfo.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class TAGViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Atributes

        public class CarouselModel
        {
            public string Title { get; set; }
            public string Detail { get; set; }
        }

        private ObservableCollection<CarouselModel> carouselInstructions;

        #endregion

        #region Properties
        public ObservableCollection<CarouselModel> CarouselInstructions
        {
            get { return carouselInstructions; }
            private set 
            {
                SetValue(ref carouselInstructions, value);
            }
        }
        #endregion

        #region Constructor
        public TAGViewModel()
        {
            apiService = new ApiService();
            GetSteps();
        }
        #endregion

        #region Methods
        public void GetSteps()
        {
            CarouselInstructions = new ObservableCollection<CarouselModel>();

            CarouselInstructions.Add(new CarouselModel 
            { 
                Title = "Paso 1", 
                Detail = "Ten a la mano tu myTAG, si aun no lo tienes, compralo en tienda" 
            });
            CarouselInstructions.Add(new CarouselModel 
            {   
                Title = "Paso 2", 
                Detail = "Al presionar el botón configurar TAG, tu teléfono vibrará, una vez que termine de vibrar acerca tu sticker a la parte TRASERA SUPERIOR de tu teléfono" 
            });
            CarouselInstructions.Add(new CarouselModel
            {
                Title = "Paso 3",
                Detail = "Manten tu myTAG pegado al teléfono hasta que vibre nuevamente y veas la pantalla de confirmación"
            });
            CarouselInstructions.Add(new CarouselModel
            {
                Title = "Paso 4",
                Detail = "Manten tu myTAG pegado al teléfono hasta que vibre nuevamente y veas la pantalla de confirmación"
            });
        }
        #endregion

        #region Commands
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

        public ICommand GoToStoreCommand
        {
            get
            {
                return new RelayCommand(GoToStore);
            }
        }
        private async void GoToStore()
        {
            await Launcher.OpenAsync(new Uri("https://mynfo.mx/index.php/tienda/"));
        }
        #endregion
    }
}
