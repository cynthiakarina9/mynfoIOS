﻿namespace Mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Mynfo.Domain;
    using Mynfo.Helpers;
    using Mynfo.Models;
    using Mynfo.Services;
    using Mynfo.Views;
    using Rg.Plugins.Popup.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class ListOfNetworksViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private bool isEmpty;
        private bool isRunning;
        private Box box;
        private ObservableCollection<ProfileEmail> profileEmail;
        private ObservableCollection<ProfilePhone> profilePhone;
        private ObservableCollection<ProfileSM> profileSM;
        private ObservableCollection<ProfileWhatsapp> profileWhatsapp;
        private ObservableCollection<ProfileLocal> profilePerfiles;
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get { return this.isEmpty; }
            set { SetValue(ref this.isEmpty, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public Box Box
        {
            get { return this.box; }
            set { SetValue(ref this.box, value); }
        }
        public ObservableCollection<ProfileEmail> ProfileEmail
        {
            get { return profileEmail; }
            private set
            {
                SetValue(ref profileEmail, value);
            }
        }
        public ObservableCollection<ProfilePhone> ProfilePhone
        {
            get { return profilePhone; }
            private set
            {
                SetValue(ref profilePhone, value);
            }
        }
        public ObservableCollection<ProfileSM> ProfileSM
        {
            get { return profileSM; }
            private set
            {
                SetValue(ref profileSM, value);
            }
        }
        public ObservableCollection<ProfileWhatsapp> ProfileWhatsapp
        {
            get { return profileWhatsapp; }
            private set
            {
                SetValue(ref profileWhatsapp, value);
            }
        }
        public ObservableCollection<ProfileLocal> ProfilePerfiles
        {
            get { return profilePerfiles; }
            private set
            {
                SetValue(ref profilePerfiles, value);
            }
        }
        public ProfileLocal selectedProfileProfiles 
        { 
            get; 
            set; 
        }
        #endregion

        #region Constructor
        public ListOfNetworksViewModel(int _BoxId)
        {
            apiService = new ApiService();
            IsEmpty = false;
            ProfilePerfiles = new ObservableCollection<ProfileLocal>();
            GetBox(_BoxId);
            GetListEmail(_BoxId);
            GetListPhone(_BoxId);
            GetListSM(_BoxId);
            GetListWhatsapp(_BoxId);
        }
        #endregion

        #region Methods

        #region Box
        public async Task<Box> GetBox(int _BoxId)
        {
            Box = new Box();
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var BoxApi = await this.apiService.GetBox(
               apiSecurity,
               "/api",
               "/Boxes",
               _BoxId);
            Box = BoxApi;
            return Box;
        }
        #endregion

        #region Email
        private async Task<ObservableCollection<ProfileEmail>> GetListEmail(int _BoxId)
        {
            this.IsRunning = true;
            List<ProfileEmail> listEmail;
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            ProfileEmail = new ObservableCollection<ProfileEmail>();
            listEmail = await this.apiService.GetListByUser<ProfileEmail>(
                apiSecurity,
                "/api",
                "/ProfileEmails",
                MainViewModel.GetInstance().User.UserId);

            var ListOrderBy = listEmail.OrderBy(x => x.Name).ToList();
            foreach (ProfileEmail ItemEmail in ListOrderBy)
            {
                Box_ProfileEmail RelationEmail;
                RelationEmail = new Box_ProfileEmail
                {
                    BoxId = _BoxId,
                    ProfileEmailId = ItemEmail.ProfileEmailId
                };
                
                var response = await this.apiService.Get(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileEmail/GetBox_ProfileEmail",
                    RelationEmail);

                ItemEmail.Exist = response.IsSuccess;

                //if (ItemEmail.Exist == true)
                //{
                    var Email = Converter.ToProfileLocalE(ItemEmail);
                    ProfilePerfiles.Add(Email);
                //}
            }
            this.IsRunning = false;
            return ProfileEmail;
        }

        #region ListasEmail
        public void addProfileEmail(ProfileEmail _profileEmail)
        {
            var E = Converter.ToProfileLocalE(_profileEmail);
            ProfilePerfiles.Add(E);
            IsEmpty = false;
        }
        public void removeProfileEmail(ProfileEmail _profileEmail)
        {
            ProfileLocal E = Converter.ToProfileLocalE(_profileEmail);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (E.ProfileName == PLocal.ProfileName && E.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            if(ProfilePerfiles.Count == 0)
            {
                IsEmpty = true;
            }
        }
        public void updateProfileEmail(ProfileEmail _profileEmail)
        {
            ProfileLocal E = Converter.ToProfileLocalE(_profileEmail);
            ProfileLocal Aux = new ProfileLocal();
            int newIndex = 0;
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (E.ProfileName == PLocal.ProfileName && E.value == PLocal.value)
                {
                    Aux = PLocal;
                    newIndex = ProfilePerfiles.IndexOf(PLocal);
                }
            }

            ProfilePerfiles.Remove(Aux);

            ProfilePerfiles.Insert(newIndex, E);
        }
        #endregion

        #endregion

        #region Phone
        private async Task<ObservableCollection<ProfilePhone>> GetListPhone(int _BoxId)
        {
            this.IsRunning = true;
            List<ProfilePhone> listPhone;
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            ProfilePhone = new ObservableCollection<ProfilePhone>();
            listPhone = await this.apiService.GetListByUser<ProfilePhone>(
                apiSecurity,
                "/api",
                "/ProfilePhones",
                MainViewModel.GetInstance().User.UserId);

            var ListOrderBy = listPhone.OrderBy(x => x.Name).ToList();
            foreach (ProfilePhone ItemPhone in ListOrderBy)
            {
                Box_ProfilePhone RelationPhone;
                RelationPhone = new Box_ProfilePhone
                {
                    BoxId = _BoxId,
                    ProfilePhoneId = ItemPhone.ProfilePhoneId
                };

                var response = await this.apiService.Get(
                    apiSecurity,
                    "/api",
                    "/Box_ProfilePhone/GetBox_ProfilePhone",
                    RelationPhone);

                ItemPhone.Exist = response.IsSuccess;
                //if (ItemPhone.Exist == true)
                //{
                    var Phone = Converter.ToProfileLocalP(ItemPhone);
                    ProfilePerfiles.Add(Phone);
                //}
            }
            this.IsRunning = false;
            return ProfilePhone;
        }

        #region ListasPhone
        public void addProfilePhone(ProfilePhone _profilePhone)
        {
            var P = Converter.ToProfileLocalP(_profilePhone);
            ProfilePerfiles.Add(P);
            IsEmpty = false;
        }
        public void removeProfilePhone(ProfilePhone _profilePhone)
        {
            ProfileLocal P = Converter.ToProfileLocalP(_profilePhone);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (P.ProfileName == PLocal.ProfileName && P.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            if (ProfilePerfiles.Count == 0)
            {
                IsEmpty = true;
            }
        }
        public void updateProfilePhone(ProfilePhone _profileP)
        {
            ProfileLocal P = Converter.ToProfileLocalP(_profileP);
            ProfileLocal Aux = new ProfileLocal();
            int newIndex = 0;
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (P.ProfileName == PLocal.ProfileName && P.value == PLocal.value)
                {
                    Aux = PLocal;
                    newIndex = ProfilePerfiles.IndexOf(PLocal);
                }
            }

            ProfilePerfiles.Remove(Aux);

            ProfilePerfiles.Insert(newIndex, P);
        }
        #endregion

        #endregion

        #region SM
        private async Task<ObservableCollection<ProfileSM>> GetListSM(int _BoxId)
        {
            this.IsRunning = true;
            List<ProfileSM> listSM;

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();

            ProfileSM = new ObservableCollection<ProfileSM>();
            listSM = await this.apiService.GetListByUser<ProfileSM>(
                apiSecurity,
                "/api",
                "/ProfileSMs",
                MainViewModel.GetInstance().User.UserId);

            var ListOrderBy = listSM.OrderBy(x => x.ProfileName).ToList();
            foreach (ProfileSM ItemSM in ListOrderBy)
            {
                Box_ProfileSM RelationSM;
                RelationSM = new Box_ProfileSM
                {
                    BoxId = _BoxId,
                    ProfileMSId = ItemSM.ProfileMSId
                };
 
                var response = await this.apiService.Get(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileSM/GetBox_ProfileSM",
                    RelationSM);
                ItemSM.Exist = response.IsSuccess;
                var SM = Converter.ToProfileLocalSM(ItemSM);
                ProfilePerfiles.Add(SM);
            }
            this.IsRunning = false;
            return ProfileSM;
        }

        #region ListasSM
        public void addProfileSM(ProfileSM _profileSM)
        {
            var SM = Converter.ToProfileLocalSM(_profileSM);
            ProfilePerfiles.Add(SM);
            IsEmpty = false;
        }
        public void removeProfileSM(ProfileSM _profileSM)
        {
            ProfileLocal SM = Converter.ToProfileLocalSM(_profileSM);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (SM.ProfileName == PLocal.ProfileName && SM.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            if (ProfilePerfiles.Count == 0)
            {
                IsEmpty = true;
            }
        }
        public void updateProfileSM(ProfileSM _profileSM)
        {
            ProfileLocal SM = Converter.ToProfileLocalSM(_profileSM);
            ProfileLocal Aux = new ProfileLocal();
            int newIndex = 0;
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (SM.ProfileName == PLocal.ProfileName && SM.value == PLocal.value)
                {
                    Aux = PLocal;
                    newIndex = ProfilePerfiles.IndexOf(PLocal);
                }
            }

            ProfilePerfiles.Remove(Aux);

            ProfilePerfiles.Insert(newIndex, SM);
        }
        #endregion
        #endregion

        #region Whatsapp
        private async Task<ObservableCollection<ProfileWhatsapp>> GetListWhatsapp(int _BoxId)
        {
            this.IsRunning = true;
            List<ProfileWhatsapp> listWhatsapp;

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            ProfileWhatsapp = new ObservableCollection<ProfileWhatsapp>();
            listWhatsapp = await this.apiService.GetListByUser<ProfileWhatsapp>(
                apiSecurity,
                "/api",
                "/ProfileWhatsapps",
                MainViewModel.GetInstance().User.UserId);

            var ListOrderBy = listWhatsapp.OrderBy(x => x.Name).ToList();
            foreach (ProfileWhatsapp ItemWhatsapp in ListOrderBy)
            {
                Box_ProfileWhatsapp RelationWhatsapp;
                RelationWhatsapp = new Box_ProfileWhatsapp
                {
                    BoxId = _BoxId,
                    ProfileWhatsappId = ItemWhatsapp.ProfileWhatsappId
                };
                
                var response = await this.apiService.Get(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileWhatsapp/GetBox_ProfileWhatsapp",
                    RelationWhatsapp);

                ItemWhatsapp.Exist = response.IsSuccess;
                
                var W = Converter.ToProfileLocalW(ItemWhatsapp);
                ProfilePerfiles.Add(W);
            }
            this.IsRunning = false;
            return ProfileWhatsapp;
        }

        #region ListaWhatsapp
        public void addProfileWhatsapp(ProfileWhatsapp _profileW)
        {
            var W = Converter.ToProfileLocalW(_profileW);
            ProfilePerfiles.Add(W);
            IsEmpty = false;
        }
        public void removeProfileWhatsapp(ProfileWhatsapp _profileW)
        {
            ProfileLocal W = Converter.ToProfileLocalW(_profileW);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (W.ProfileName == PLocal.ProfileName && W.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            if (ProfilePerfiles.Count == 0)
            {
                IsEmpty = true;
            }
        }
        public void updateProfileWhatsapp(ProfileWhatsapp _profileW)
        {
            ProfileLocal W = Converter.ToProfileLocalW(_profileW);
            ProfileLocal Aux = new ProfileLocal();
            int newIndex = 0;
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (W.ProfileName == PLocal.ProfileName && W.value == PLocal.value)
                {
                    Aux = PLocal;
                    newIndex = ProfilePerfiles.IndexOf(PLocal);
                }
            }

            ProfilePerfiles.Remove(Aux);

            ProfilePerfiles.Insert(newIndex, W);
        }
        #endregion

        #endregion

        #endregion

        #region Commads
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

        public ICommand GotoAddCommand
        {
            get
            {
                return new RelayCommand(GotoAdd);
            }
        }
        private void GotoAdd()
        {
            PopupNavigation.Instance.PopAsync();
            MainViewModel.GetInstance().ProfileTypeSelection = new ProfileTypeSelectionViewModel(Box.BoxId);
            PopupNavigation.Instance.PushAsync(new ProfileTypeSelection(Box.BoxId, Box.BoxDefault, Box.Name));
        }
        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand(GoBack);
            }
        }
        private void GoBack()
        {
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}
