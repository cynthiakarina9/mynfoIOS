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
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class DetailsBoxEdithViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private Box box;
        private string color;
        private ObservableCollection<ProfileEmail> profileEmail;
        private ObservableCollection<ProfilePhone> profilePhone;
        private ObservableCollection<ProfileSM> profileSM;
        private ObservableCollection<ProfileWhatsapp> profileWhatsapp;
        private ObservableCollection<ProfileLocal> profilePerfiles;
        private ObservableCollection<ProfileLocal> profilesSelected;
        #endregion

        #region Properties
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
        public string Color
        {
            get { return this.color; }
            set { SetValue(ref this.color, value); }
        }
        private ObservableCollection<ProfileLocal> ProfilesSelected
        {
            get { return this.profilesSelected; }
            set { SetValue(ref this.profilesSelected, value); }
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
        #endregion

        #region Constructor
        public DetailsBoxEdithViewModel(Box _Box)
        {
            apiService = new ApiService();
            Box = new Box();
            GetBox(_Box.BoxId);
            ProfilesSelected = new ObservableCollection<ProfileLocal>();
            ProfilePerfiles = new ObservableCollection<ProfileLocal>();
            GetListEmail(_Box.BoxId);
            GetListPhone(_Box.BoxId);
            GetListSM(_Box.BoxId);
            GetListWhatsapp(_Box.BoxId);
            GetBoxColorName(_Box);
        }
        #endregion

        #region Methods

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
            foreach (ProfileEmail ItemEmail in listEmail)
            {
                Box_ProfileEmail RelationEmail;
                RelationEmail = new Box_ProfileEmail
                {
                    BoxId = _BoxId,
                    ProfileEmailId = ItemEmail.ProfileEmailId
                };
                //apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                var response = await this.apiService.Get(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileEmail/GetBox_ProfileEmail",
                    RelationEmail);

                ItemEmail.Exist = response.IsSuccess;

                if (ItemEmail.Exist == true)
                {
                    var Email = Converter.ToProfileLocalE1(ItemEmail);
                    ProfilePerfiles.Add(Email);
                }
            }
            this.IsRunning = false;
            return ProfileEmail;
        }
        public void addProfileEmail(ProfileEmail _profileEmail)
        {
            var E = Converter.ToProfileLocalE1(_profileEmail);
            ProfilePerfiles.Add(E);
        }
        public void removeProfileEmail(ProfileEmail _profileEmail)
        {
            ProfileLocal E = Converter.ToProfileLocalE1(_profileEmail);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (E.ProfileName == PLocal.ProfileName && E.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            var A = ProfilePerfiles.Count;
        }
        public async void DeleteProfileEmail(int _box, int _profileEmailId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfileEmail box_ProfileEmail = new Box_ProfileEmail
            {
                BoxId = _box,
                ProfileEmailId = _profileEmailId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Email = await this.apiService.GetIdRelation(
               apiSecurity,
               "/api",
               "/Box_ProfileEmail/GetBox_ProfileEmail",
               box_ProfileEmail);
            var profileEmail = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileEmail",
                idBox_Email.Box_ProfileEmailId);
        }
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
            foreach (ProfilePhone ItemPhone in listPhone)
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
                if (ItemPhone.Exist == true)
                {
                    var Phone = Converter.ToProfileLocalP1(ItemPhone);
                    ProfilePerfiles.Add(Phone);
                }
            }

            this.IsRunning = false;
            return ProfilePhone;
        }
        public void addProfilePhone(ProfilePhone _profilePhone)
        {
            var P = Converter.ToProfileLocalP1(_profilePhone);
            ProfilePerfiles.Add(P);
        }
        public void removeProfilePhone(ProfilePhone _profilePhone)
        {
            ProfileLocal P = Converter.ToProfileLocalP1(_profilePhone);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (P.ProfileName == PLocal.ProfileName && P.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            var A = ProfilePerfiles.Count;
        }
        public async void DeleteProfilePhone(int _box, int _profilePhoneId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfilePhone box_ProfilePhone = new Box_ProfilePhone
            {
                BoxId = _box,
                ProfilePhoneId = _profilePhoneId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Phone = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfilePhone/GetBox_ProfilePhone",
                box_ProfilePhone);

            var profilePhone = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfilePhone",
                idBox_Phone.Box_ProfilePhoneId);
        }
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
            foreach (ProfileSM ItemSM in listSM)
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
                if (ItemSM.Exist == true)
                {
                    var SM = Converter.ToProfileLocalSM1(ItemSM);
                    ProfilePerfiles.Add(SM);
                }
            }
            this.IsRunning = false;
            return ProfileSM;
        }
        public void addProfileSM(ProfileSM _profileSM)
        {
            var SM = Converter.ToProfileLocalSM1(_profileSM);
            ProfilePerfiles.Add(SM);
        }
        public void removeProfileSM(ProfileSM _profileSM)
        {
            ProfileLocal SM = Converter.ToProfileLocalSM1(_profileSM);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (SM.ProfileName == PLocal.ProfileName && SM.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            var A = ProfilePerfiles.Count;
        }
        public async void DeleteProfileSM(int _box, int _profileSMId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfileSM box_ProfileSM = new Box_ProfileSM
            {
                BoxId = _box,
                ProfileMSId = _profileSMId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_SM = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfileSM/GetBox_ProfileSM",
                box_ProfileSM);

            var profileSM = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileSM",
                idBox_SM.Box_ProfileSMId);
        }
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
            foreach (ProfileWhatsapp ItemWhatsapp in listWhatsapp)
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
                if (ItemWhatsapp.Exist == true)
                {
                    var W = Converter.ToProfileLocalW1(ItemWhatsapp);
                    ProfilePerfiles.Add(W);
                }
            }
            this.IsRunning = false;
            return ProfileWhatsapp;
        }
        public void addProfileW(ProfileWhatsapp _profileW)
        {
            var W = Converter.ToProfileLocalW1(_profileW);
            ProfilePerfiles.Add(W);
        }
        public void removeProfileW(ProfileWhatsapp _profileW)
        {
            ProfileLocal W = Converter.ToProfileLocalW1(_profileW);
            ProfileLocal Aux = new ProfileLocal();
            foreach (ProfileLocal PLocal in ProfilePerfiles)
            {
                if (W.ProfileName == PLocal.ProfileName && W.value == PLocal.value)
                {
                    Aux = PLocal;
                }
            }
            ProfilePerfiles.Remove(Aux);
            var A = ProfilePerfiles.Count;
        }
        public async void DeleteProfileWhatsapp(int _box, int _profileWhatsappId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfileWhatsapp box_ProfileWhatsapp = new Box_ProfileWhatsapp
            {
                BoxId = _box,
                ProfileWhatsappId = _profileWhatsappId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Whatsapp = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfileWhatsapp/GetBox_ProfileWhatsapp",
                box_ProfileWhatsapp);

            var profileWhatsapp = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileWhatsapp",
                idBox_Whatsapp.Box_ProfileWhatsappId);
        }
        #endregion

        #region Box
        public async Task<Box> GetBox(int _BoxId)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            Box = await this.apiService.GetBox(
                apiSecurity,
                "/api",
                "/Boxes",
                _BoxId);
            return Box;        
        }
        public async Task<Box> EdithBox(Box Box)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var profile = await this.apiService.PutBox(
                apiSecurity,
                "/api", 
                "/Boxes",
                Box,
                Box.BoxId);
            profile = Box;
            await MainViewModel.GetInstance().DetailsBox.GetBoxe(Box.BoxId);
            MainViewModel.GetInstance().Home.UpdateList(Box);
            return Box;
        }
        public void addProfile(ProfileLocal _profileSelected)
        {
            ProfilesSelected.Add(_profileSelected);
        }

        public void GetBoxColorName(Box _Box)
        {
            string colorHex = _Box.ColorBox;

            switch (colorHex)
            {
                case "#12947f":
                    Color = Languages.Green;// "Verde";
                    break;
                case "#2fc4b2":
                    Color = Languages.Cyan;// "Verde Agua";
                    break;
                case "#404a7f":
                    Color = Languages.DarkBlue;// "Azul oscuro";
                    break;
                case "#FF5521":
                    Color = Languages.Orange;// "Anaranjado";
                    break;
                case "#508ed8":
                    Color = Languages.LightBlue;// "Azul claro";
                    break;
                case "#d89a00":
                    Color = Languages.Yellow;// "Amarillo";
                    break;
                case "#ff0033":
                    Color = Languages.Fuchsia;// "Fuchsia";
                    break;
                case "#008445":
                    Color = Languages.DarkGreen;// "Verde Oscuro";
                    break;
                case "#7f416a":
                    Color = Languages.Purple;// "Morado";
                    break;
                case "#6f50ff":
                    Color = Languages.Lilac;// "Lila";
                    break;
                case "#c1271f":
                    Color = Languages.Red;// "Rojo";
                    break;
                case "#ce7d7d":
                    Color = Languages.Pink;// "Rosa";
                    break;
                default:
                    Color = Languages.Colorless;// "Sin color";
                    break;
            }
        }
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
            PopupNavigation.Instance.PopAllAsync();
            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }

        public async void DeleteBox(int _BoxId)
        {
            this.IsRunning = true;
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();

            #region Revisión de perfiles de box

            #region Email
            var EmailProfiles = await this.apiService.GetRelationBox_ProfilesEmail(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileEmail/GetBox_ProfileEmailBoxId",
                    _BoxId);
            if (EmailProfiles.Count != 0)
            {
                var DeleteEmail = await this.apiService.DeleteProfilesEmail_Box(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileEmail/DeleteBox_ProfileEmailBoxRelation",
                    _BoxId);
            }
            #endregion

            #region Phone
            var PhoneProfiles = await this.apiService.GetRelationBox_ProfilesPhone(
                    apiSecurity,
                    "/api",
                    "/Box_ProfilePhone/GetBox_ProfilePhoneBoxId",
                    _BoxId);
            if (PhoneProfiles.Count != 0)
            {
                var DeletePhone = await this.apiService.DeleteProfilesPhone_Box(
                    apiSecurity,
                    "/api",
                    "/Box_ProfilePhone/DeleteBox_ProfilePhoneBoxRelations",
                    _BoxId);
            }
            #endregion

            #region Whatsapp
            var WhatsappProfiles = await this.apiService.GetRelationBox_ProfilesWhatsapp(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileWhatsapp/GetBox_ProfileWhatsappBoxId",
                    _BoxId);
            if (WhatsappProfiles.Count != 0)
            {
                var DeleteWhatsapp = await this.apiService.DeleteProfilesWhatsapp_Box(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileWhatsapp/DeleteBox_ProfileWhatsappBoxRelations",
                    _BoxId);
            }
            #endregion

            #region SM
            var SMProfiles = await this.apiService.GetRelationBox_ProfilesSM(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileSM/GetBox_ProfileSMBoxId",
                    _BoxId);
            if (SMProfiles.Count != 0)
            {
                var DeleteSM = await this.apiService.DeleteProfilesPhone_Box(
                    apiSecurity,
                    "/api",
                    "/Box_ProfileSM/DeleteBox_ProfileSMBoxRelations",
                    _BoxId);
            }
            #endregion

            #endregion

            var box = await this.apiService.GetBox(
                apiSecurity,
                    "/api",
                    "/Boxes",
                    _BoxId);
            if(box.BoxDefault == true)
            {
                MainViewModel.GetInstance().Home.RemoveList(box);
                var LastBox = await this.apiService.GetLastBox(
                    apiSecurity,
                    "/api",
                    "/Boxes/GetLastBox",
                    _BoxId);
                var NewDefault = new Box 
                {
                    BoxId = LastBox.BoxId,
                    UserId = LastBox.UserId,
                    Name = LastBox.Name,
                    Time = LastBox.Time,
                    BoxDefault = true
                };
                await EdithBox(NewDefault);
                var response = await this.apiService.Delete(
                    apiSecurity,
                    "/api",
                    "/Boxes",
                    _BoxId);
            }
            else
            {
                MainViewModel.GetInstance().Home.RemoveList(box);
                var response = await this.apiService.Delete(
                    apiSecurity,
                    "/api",
                    "/Boxes",
                    _BoxId);
            }
            await PopupNavigation.Instance.PopAllAsync();
            //MainViewModel.GetInstance().Home = new HomeViewModel();
            //Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}
