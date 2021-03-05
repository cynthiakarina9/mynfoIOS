﻿namespace Mynfo.Views
{
    using Mynfo.Domain;
    using Mynfo.Models;
    using Mynfo.Services;
    using Mynfo.ViewModels;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Data.SqlClient;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsBoxPage : ContentPage
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Properties
        public Entry BxNameEntry = new Entry();
        public String BoxName;
        public Box Box { get; set; }
        #endregion

        #region Constructor
        public DetailsBoxPage(Box _Box)
        {
            InitializeComponent();
            apiService = new ApiService();
            NavigationPage.SetHasNavigationBar(this, false);
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if (currentTheme == OSAppTheme.Dark)
            {
                EdithButton.Source = "edit1";
                BoxProfiles.Source = "plusb";
            }
            else
            {
                EdithButton.Source = "edit2";
                BoxProfiles.Source = "plus";
            }
        
            int BoxId = _Box.BoxId;
            var boxLocal = new BoxLocal();
            int UserID = MainViewModel.GetInstance().User.UserId;
            string consultaDefault;
            string queryLastBoxCreated = "SELECT TOP 1 * FROM dbo.Boxes where dbo.Boxes.UserId = " + UserID + " ORDER BY BoxId DESC";
            string queryUpdatesetDefault;
            string queryUpdateTakeOffDefault;
            string queryGetPhones;
            string queryGetEmails;
            string queryGetSMProfiles;
            string queryGetWhatsapp;
            string cadenaConexion = @"data source=serverappmynfo.database.windows.net;initial catalog=mynfo;user id=adminmynfo;password=4dmiNFC*Atx2020;Connect Timeout=60";
            //string cadenaConexion = @"data source=serverappmynfo.database.windows.net;initial catalog=mynfo;user id=adminmynfo;password=4dmiNFC*Atx2020;Connect Timeout=60";
            System.Text.StringBuilder sb;
            BoxName = "";
            bool BoxDefault = false;
            int UserId = 0;
            DateTime boxcreation = DateTime.Now;
            var BxSaveName = new ImageButton();
            var BxBtnDelete = new ImageButton();
            var bxBtnHome = new ImageButton();
            var BxDefaultCheckBox = new CheckBox();
            int listProfileNum = 0;

            //Llenar BoxId si es 0
            //if (BoxId == 0)
            //{
            //    using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //    {
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(queryLastBoxCreated);

            //        string sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            using (SqlDataReader reader = command.ExecuteReader())
            //            {
            //                while (reader.Read())
            //                {
            //                    BoxId = (int)reader["BoxId"];
            //                }
            //            }

            //            connection.Close();
            //        }
            //    }
            //}

            //Asignación de querys
            consultaDefault = "select * from dbo.Boxes where dbo.Boxes.BoxId = " + BoxId;
            queryUpdatesetDefault = "update dbo.Boxes set BoxDefault = 1 where dbo.Boxes.UserId =" + UserID + " and dbo.Boxes.BoxId =" + BoxId;
            queryUpdateTakeOffDefault = "update dbo.Boxes set BoxDefault = 0 where dbo.Boxes.UserId =" + UserID + " and dbo.Boxes.BoxDefault = 1 and dbo.Boxes.BoxId !=" + BoxId;
            queryGetPhones = "select dbo.Boxes.BoxId, dbo.ProfilePhones.ProfilePhoneId, dbo.ProfilePhones.Name, " +
                             "dbo.ProfilePhones.Number from dbo.Box_ProfilePhone Join dbo.Boxes " +
                             "on(dbo.Boxes.BoxId = dbo.Box_ProfilePhone.BoxId) " +
                             "Join dbo.ProfilePhones on(dbo.ProfilePhones.ProfilePhoneId = dbo.Box_ProfilePhone.ProfilePhoneId) " +
                             "where dbo.Boxes.BoxId = " + BoxId;
            queryGetEmails = "select dbo.Boxes.BoxId, dbo.ProfileEmails.ProfileEmailId, dbo.ProfileEmails.Name, " +
                              "dbo.ProfileEmails.Email from dbo.Box_ProfileEmail " +
                              "Join dbo.Boxes on(dbo.Boxes.BoxId = dbo.Box_ProfileEmail.BoxId) " +
                              "Join dbo.ProfileEmails on(dbo.ProfileEmails.ProfileEmailId = dbo.Box_ProfileEmail.ProfileEmailId) " +
                              "where dbo.Boxes.BoxId = " + BoxId;
            queryGetSMProfiles = "select * from dbo.Box_ProfileSM " +
                                    "join dbo.ProfileSMs on(dbo.ProfileSMs.ProfileMSId = dbo.Box_ProfileSM.ProfileMSId) " +
                                    "join dbo.RedSocials on(dbo.ProfileSMs.RedSocialId = dbo.RedSocials.RedSocialId) " +
                                    "where dbo.Box_ProfileSM.BoxId = " + _Box.BoxId;
            queryGetWhatsapp = "select dbo.Boxes.BoxId, dbo.ProfileWhatsapps.ProfileWhatsappId, dbo.ProfileWhatsapps.Name, " +
                                        "dbo.ProfileWhatsapps.Number from dbo.Box_ProfileWhatsapp Join dbo.Boxes " +
                                        "on(dbo.Boxes.BoxId = dbo.Box_ProfileWhatsapp.BoxId) " +
                                        "Join dbo.ProfileWhatsapps on(dbo.ProfileWhatsapps.ProfileWhatsappId = dbo.Box_ProfileWhatsapp.ProfileWhatsappId) " +
                                        "where dbo.Boxes.BoxId =" + _Box.BoxId;

            //Consulta para obtener Box
            //using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //{
            //    sb = new System.Text.StringBuilder();
            //    sb.Append(consultaDefault);

            //    string sql = sb.ToString();

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                BoxName = (string)reader["Name"];
            //                BoxDefault = (bool)reader["BoxDefault"];
            //                UserId = (int)reader["UserId"];
            //                boxcreation = (DateTime)reader["Time"];
            //            }
            //        }

            //        connection.Close();
            //    }
            //}

            boxLocal.BoxId = _Box.BoxId;
            boxLocal.Name = BoxName;
            boxLocal.BoxDefault = BoxDefault;
            boxLocal.UserId = UserId;
            boxLocal.Time = boxcreation;
            boxLocal.FirstName = MainViewModel.GetInstance().User.FirstName;
            boxLocal.LastName = MainViewModel.GetInstance().User.LastName;
            boxLocal.ImagePath = MainViewModel.GetInstance().User.ImagePath;
            boxLocal.UserTypeId = MainViewModel.GetInstance().User.UserTypeId;

            //Definir color de fondo con respecto a si la box es predeterminada
            if (_Box.BoxDefault == true)
            {
                if(currentTheme == OSAppTheme.Light)
                {
                    FullBackGround.BackgroundColor = Color.FromHex("#ff6c45");
                    bxBtnHome.BackgroundColor = Color.FromHex("#ff6c45");
                    BxSaveName.BackgroundColor = Color.FromHex("#ff6c45");
                    BxBtnDelete.BackgroundColor = Color.FromHex("#ff6c45");
                }
                else
                {
                    FullBackGround.BackgroundColor = Color.FromHex("#FF5521");
                    bxBtnHome.BackgroundColor = Color.FromHex("#FF5521");
                    BxSaveName.BackgroundColor = Color.FromHex("#FF5521");
                    BxBtnDelete.BackgroundColor = Color.FromHex("#FF5521");
                }
                
            }
            else
            {
                if (currentTheme == OSAppTheme.Light)
                {
                    FullBackGround.BackgroundColor = Color.FromHex("#FFFFFF");
                    bxBtnHome.BackgroundColor = Color.FromHex("#FFFFFF");
                    BxSaveName.BackgroundColor = Color.FromHex("#FFFFFF");
                    BxBtnDelete.BackgroundColor = Color.FromHex("#FFFFFF");
                }
                else
                {
                    FullBackGround.BackgroundColor = Color.FromHex("#d1d1d1");
                    bxBtnHome.BackgroundColor = Color.FromHex("#d1d1d1");
                    BxSaveName.BackgroundColor = Color.FromHex("#d1d1d1");
                    BxBtnDelete.BackgroundColor = Color.FromHex("#d1d1d1");
                }
            }


            //Navegación a ventana de perfiles
            BoxProfiles.Clicked += new EventHandler((sender, e) => BoxDetails_Clicked(sender, e, BoxId, BoxDefault, BoxName));

            //Creación del botón para volver a home
            bxBtnHome.BackgroundColor = Color.Transparent;
            bxBtnHome.Source = "back.png";
            bxBtnHome.WidthRequest = 50;
            bxBtnHome.HeightRequest = 50;
            bxBtnHome.Clicked += ToolbarItem_Clicked;

            HomeButton.Children.Add(bxBtnHome);

            //Creación de botón para borrar box
            //BxBtnDelete.BackgroundColor = Color.Transparent;
            //BxBtnDelete.Source = "edit2.png";
            //BxBtnDelete.WidthRequest = 50;
            //BxBtnDelete.HeightRequest = 50;
            EdithButton.Clicked += new EventHandler((sender, e) => edithBox(sender, e, _Box, UserID, BoxDefault));

            //EdithButton.Children.Add(BxBtnDelete);

            //Creación del checkbox de box predeterminada
            BxDefaultCheckBox.IsChecked = BoxDefault;
            if (BoxDefault == true)
            {
                BxDefaultCheckBox.IsEnabled = false;
                if(currentTheme == OSAppTheme.Dark)
                {
                    BxDefaultCheckBox.BackgroundColor = Color.White;
                }
                else
                {
                    BxDefaultCheckBox.BackgroundColor = Color.Black;
                }
            }
            else
            {
                BxDefaultCheckBox.IsEnabled = true;
                BxDefaultCheckBox.BackgroundColor = Color.FromHex("#FF5521");
            }
            BxDefaultCheckBox.CheckedChanged += CheckDefaultBox;

            BoxDefaultCheckBox.Children.Add(BxDefaultCheckBox);

            #region LastCode

            //**********************************************************
            //AGREGACIÓN DE PERFILES A BOX
            //**********************************************************

            ////Consulta para obtener Teléfonos
            //using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //{
            //    sb = new System.Text.StringBuilder();
            //    sb.Append(queryGetPhones);
            //    string sql = sb.ToString();

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var phoneIcon = new ImageButton();
            //                var phoneName = new Label();
            //                //var deleteProfile = new ImageButton();
            //                var Line = new BoxView();
            //                int PhoneId = (int)reader["ProfilePhoneId"];
            //                var space = new BoxView();

            //                phoneIcon.Source = "tel2.png";
            //                phoneIcon.WidthRequest = 50;
            //                phoneIcon.HeightRequest = 50;
            //                phoneIcon.HorizontalOptions = LayoutOptions.Center;
            //                phoneIcon.IsEnabled = true;
            //                phoneIcon.Clicked += new EventHandler((sender, e) => DeleteBoxPhone(sender, e, BoxId, PhoneId));

            //                phoneName.Text = (string)reader["Name"];
            //                phoneName.FontSize = 15;
            //                phoneName.HorizontalTextAlignment = TextAlignment.Center;
            //                phoneName.FontAttributes = FontAttributes.Bold;
            //                phoneName.TextColor = Color.Black;

            //                space.HeightRequest = 30;

            //                /*deleteProfile.Source = "trash2.png";
            //                deleteProfile.BackgroundColor = Color.FromHex("#f9a589");
            //                deleteProfile.CornerRadius = 15;
            //                deleteProfile.HeightRequest = 30;
            //                deleteProfile.WidthRequest = 30;
            //                deleteProfile.HorizontalOptions = LayoutOptions.End;
            //                deleteProfile.Clicked += new EventHandler((sender, e) => DeleteBoxPhone(sender, e, BoxId, PhoneId));*/

            //                //Definir color de fondo de ícono de basura con respecto a si la box es predeterminada
            //                if (BoxDefault == true)
            //                {
            //                    // deleteProfile.BackgroundColor = Color.FromHex("#FFAB8F");
            //                    phoneIcon.BackgroundColor = Color.FromHex("#FEBDA8");
            //                }
            //                else
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#AAAAAA");
            //                    phoneIcon.BackgroundColor = Color.FromHex("#FFFFFF");
            //                }

            //                //Asignación de caja en columnas
            //                //switch (listProfileNum)
            //                //{
            //                //    case 0:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(phoneIcon);
            //                //        ProfilesList1.Children.Add(phoneName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 1:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(phoneIcon);
            //                //        ProfilesList1.Children.Add(phoneName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        // ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 2:
            //                //        listProfileNum = 3;

            //                //        ProfilesList2.Children.Add(phoneIcon);
            //                //        ProfilesList2.Children.Add(phoneName);
            //                //        ProfilesList2.Children.Add(space);
            //                //        //ProfilesList2.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 3:
            //                //        listProfileNum = 1;

            //                //        ProfilesList3.Children.Add(phoneIcon);
            //                //        ProfilesList3.Children.Add(phoneName);
            //                //        ProfilesList3.Children.Add(space);
            //                //        //ProfilesList3.Children.Add(deleteProfile);
            //                //        break;

            //                //    default:
            //                //        break;
            //                //}
            //            }
            //        }
            //        connection.Close();
            //    }
            //}

            ////Consulta para obtener Emails
            //using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //{
            //    sb = new System.Text.StringBuilder();
            //    sb.Append(queryGetEmails);
            //    string sql = sb.ToString();

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var emailIcon = new ImageButton();
            //                var emailProfile = new Label();
            //                //var deleteProfile = new ImageButton();
            //                var Line = new BoxView();
            //                int EmailId = (int)reader["ProfileEmailId"];
            //                var space = new BoxView();

            //                emailIcon.Source = "mail2.png";
            //                emailIcon.WidthRequest = 50;
            //                emailIcon.HeightRequest = 50;
            //                emailIcon.HorizontalOptions = LayoutOptions.Center;
            //                emailIcon.IsEnabled = true;
            //                emailIcon.Clicked += new EventHandler((sender, e) => DeleteBoxEmail(sender, e, BoxId, EmailId));

            //                emailProfile.Text = (string)reader["Name"];
            //                emailProfile.FontSize = 15;
            //                emailProfile.HorizontalTextAlignment = TextAlignment.Center;
            //                emailProfile.FontAttributes = FontAttributes.Bold;
            //                emailProfile.TextColor = Color.Black;

            //                space.HeightRequest = 30;

            //                /*deleteProfile.Source = "trash2.png";
            //                deleteProfile.BackgroundColor = Color.FromHex("#f9a589");
            //                deleteProfile.CornerRadius = 15;
            //                deleteProfile.HeightRequest = 30;
            //                deleteProfile.WidthRequest = 30;
            //                deleteProfile.HorizontalOptions = LayoutOptions.End;
            //                deleteProfile.Clicked += new EventHandler((sender, e) => DeleteBoxEmail(sender, e, BoxId, EmailId));*/

            //                //Definir color de fondo de ícono de basura con respecto a si la box es predeterminada
            //                if (BoxDefault == true)
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#FFAB8F");
            //                    emailIcon.BackgroundColor = Color.FromHex("#FEBDA8");
            //                }
            //                else
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#AAAAAA");
            //                    emailIcon.BackgroundColor = Color.FromHex("#FFFFFF");
            //                }

            //                //Asignación de caja en columnas
            //                //switch (listProfileNum)
            //                //{
            //                //    case 0:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(emailIcon);
            //                //        ProfilesList1.Children.Add(emailProfile);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 1:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(emailIcon);
            //                //        ProfilesList1.Children.Add(emailProfile);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 2:
            //                //        listProfileNum = 3;

            //                //        ProfilesList2.Children.Add(emailIcon);
            //                //        ProfilesList2.Children.Add(emailProfile);
            //                //        ProfilesList2.Children.Add(space);
            //                //        //ProfilesList2.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 3:
            //                //        listProfileNum = 1;

            //                //        ProfilesList3.Children.Add(emailIcon);
            //                //        ProfilesList3.Children.Add(emailProfile);
            //                //        ProfilesList3.Children.Add(space);
            //                //        //ProfilesList3.Children.Add(deleteProfile);
            //                //        break;

            //                //    default:
            //                //        break;
            //                //}
            //            }
            //        }
            //        connection.Close();
            //    }
            //}

            ////Consulta para obtener Perfiles de redes sociales
            //using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //{
            //    sb = new System.Text.StringBuilder();
            //    sb.Append(queryGetSMProfiles);
            //    string sql = sb.ToString();

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var SMIcon = new ImageButton();
            //                var SMProfileName = new Label();
            //                //var deleteProfile = new ImageButton();
            //                var Line = new BoxView();
            //                int SMId = (int)reader["ProfileMSId"];
            //                string link = (string)reader["link"];
            //                string SMType = (string)reader["Name"];
            //                var space = new BoxView();

            //                //Aquí se deben agregar las diferentes variables de redes sociales que se agreguen en el futúro
            //                switch (SMType)
            //                {
            //                    case "Facebook":
            //                        SMIcon.Source = "facebook2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Twitter":
            //                        SMIcon.Source = "twitter2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Instagram":
            //                        SMIcon.Source = "instagramlogo2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Snapchat":
            //                        SMIcon.Source = "snapchat2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "LinkedIn":
            //                        SMIcon.Source = "linkedin2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "TikTok":
            //                        SMIcon.Source = "tiktok2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Youtube":
            //                        SMIcon.Source = "youtube2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Spotify":
            //                        SMIcon.Source = "spotify2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "Twitch":
            //                        SMIcon.Source = "twitch2.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    case "WebPage":
            //                        SMIcon.Source = "networks_icon.png";
            //                        SMIcon.WidthRequest = 50;
            //                        SMIcon.HeightRequest = 50;
            //                        SMIcon.HorizontalOptions = LayoutOptions.Center;
            //                        SMIcon.IsEnabled = true;
            //                        SMIcon.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));

            //                        SMProfileName.Text = (string)reader["ProfileName"];
            //                        SMProfileName.FontSize = 15;
            //                        SMProfileName.HorizontalTextAlignment = TextAlignment.Center;
            //                        SMProfileName.FontAttributes = FontAttributes.Bold;
            //                        SMProfileName.TextColor = Color.Black;
            //                        break;
            //                    default:
            //                        break;
            //                }

            //                space.HeightRequest = 30;

            //                /*deleteProfile.Source = "trash2.png";
            //                deleteProfile.BackgroundColor = Color.FromHex("#f9a589");
            //                deleteProfile.CornerRadius = 15;
            //                deleteProfile.HeightRequest = 30;
            //                deleteProfile.WidthRequest = 30;
            //                deleteProfile.HorizontalOptions = LayoutOptions.End;
            //                deleteProfile.Clicked += new EventHandler((sender, e) => DeleteBoxSM(sender, e, BoxId, SMId));*/

            //                //Definir color de fondo de ícono de basura con respecto a si la box es predeterminada
            //                if (BoxDefault == true)
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#FFAB8F");
            //                    SMIcon.BackgroundColor = Color.FromHex("#FEBDA8");
            //                }
            //                else
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#AAAAAA");
            //                    SMIcon.BackgroundColor = Color.FromHex("#FFFFFF");
            //                }

            //                //Asignación de caja en columnas
            //                //switch (listProfileNum)
            //                //{
            //                //    case 0:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(SMIcon);
            //                //        ProfilesList1.Children.Add(SMProfileName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 1:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(SMIcon);
            //                //        ProfilesList1.Children.Add(SMProfileName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 2:
            //                //        listProfileNum = 3;

            //                //        ProfilesList2.Children.Add(SMIcon);
            //                //        ProfilesList2.Children.Add(SMProfileName);
            //                //        ProfilesList2.Children.Add(space);
            //                //        //ProfilesList2.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 3:
            //                //        listProfileNum = 1;

            //                //        ProfilesList3.Children.Add(SMIcon);
            //                //        ProfilesList3.Children.Add(SMProfileName);
            //                //        ProfilesList3.Children.Add(space);
            //                //        //ProfilesList3.Children.Add(deleteProfile);
            //                //        break;

            //                //    default:
            //                //        break;
            //                //}
            //            }
            //        }
            //        connection.Close();
            //    }
            //}

            ////Consulta para obtener Whatsapp
            //using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //{
            //    sb = new System.Text.StringBuilder();
            //    sb.Append(queryGetWhatsapp);
            //    string sql = sb.ToString();

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var whatsappIcon = new ImageButton();
            //                var whatsappName = new Label();
            //                //var deleteProfile = new ImageButton();
            //                var Line = new BoxView();
            //                int WhatsappId = (int)reader["ProfileWhatsappId"];
            //                var space = new BoxView();

            //                whatsappIcon.Source = "whatsapp2.png";
            //                whatsappIcon.WidthRequest = 50;
            //                whatsappIcon.HeightRequest = 50;
            //                whatsappIcon.HorizontalOptions = LayoutOptions.Center;
            //                whatsappIcon.IsEnabled = true;
            //                whatsappIcon.Clicked += new EventHandler((sender, e) => DeleteBoxWhatsapp(sender, e, BoxId, WhatsappId));

            //                whatsappName.Text = (string)reader["Name"];
            //                whatsappName.FontSize = 15;
            //                whatsappName.HorizontalTextAlignment = TextAlignment.Center;
            //                whatsappName.FontAttributes = FontAttributes.Bold;
            //                whatsappName.TextColor = Color.Black;

            //                space.HeightRequest = 30;

            //                /*deleteProfile.Source = "trash2.png";
            //                deleteProfile.BackgroundColor = Color.FromHex("#f9a589");
            //                deleteProfile.CornerRadius = 15;
            //                deleteProfile.HeightRequest = 30;
            //                deleteProfile.WidthRequest = 30;
            //                deleteProfile.HorizontalOptions = LayoutOptions.End;
            //                deleteProfile.Clicked += new EventHandler((sender, e) => DeleteBoxPhone(sender, e, BoxId, PhoneId));*/

            //                //Definir color de fondo de ícono de basura con respecto a si la box es predeterminada
            //                if (BoxDefault == true)
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#FFAB8F");
            //                    whatsappIcon.BackgroundColor = Color.FromHex("#FEBDA8");
            //                }
            //                else
            //                {
            //                    //deleteProfile.BackgroundColor = Color.FromHex("#AAAAAA");
            //                    whatsappIcon.BackgroundColor = Color.FromHex("#FFFFFF" +
            //                        "");
            //                }

            //                //Asignación de caja en columnas
            //                //switch (listProfileNum)
            //                //{
            //                //    case 0:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(whatsappIcon);
            //                //        ProfilesList1.Children.Add(whatsappName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        //ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 1:
            //                //        listProfileNum = 2;

            //                //        ProfilesList1.Children.Add(whatsappIcon);
            //                //        ProfilesList1.Children.Add(whatsappName);
            //                //        ProfilesList1.Children.Add(space);
            //                //        // ProfilesList1.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 2:
            //                //        listProfileNum = 3;

            //                //        ProfilesList2.Children.Add(whatsappIcon);
            //                //        ProfilesList2.Children.Add(whatsappName);
            //                //        ProfilesList2.Children.Add(space);
            //                //        //ProfilesList2.Children.Add(deleteProfile);
            //                //        break;

            //                //    case 3:
            //                //        listProfileNum = 1;

            //                //        ProfilesList3.Children.Add(whatsappIcon);
            //                //        ProfilesList3.Children.Add(whatsappName);
            //                //        ProfilesList3.Children.Add(space);
            //                //        //ProfilesList3.Children.Add(deleteProfile);
            //                //        break;

            //                //    default:
            //                //        break;
            //                //}
            //            }
            //        }
            //        connection.Close();
            //    }
            //}
            #endregion
            //Marcar o desmarcar la box predeterminada
            void CheckDefaultBox(object sender, EventArgs e)
            {
                string queryGetBoxEmail = "select * from dbo.ProfileEmails " +
                                            "join dbo.Box_ProfileEmail on" +
                                            "(dbo.ProfileEmails.ProfileEmailId = dbo.Box_ProfileEmail.ProfileEmailId) " +
                                            "where dbo.Box_ProfileEmail.BoxId = " + _Box.BoxId;
                string queryGetBoxPhone = "select * from dbo.ProfilePhones " +
                                            "join dbo.Box_ProfilePhone on" +
                                            "(dbo.ProfilePhones.ProfilePhoneId = dbo.Box_ProfilePhone.ProfilePhoneId) " +
                                            "where dbo.Box_ProfilePhone.BoxId = " + _Box.BoxId;
                string queryGetBoxSMProfiles = "select * from dbo.ProfileSMs " +
                                                "join dbo.Box_ProfileSM on" +
                                                "(dbo.ProfileSMs.ProfileMSId = dbo.Box_ProfileSM.ProfileMSId) " +
                                                "join dbo.RedSocials on(dbo.ProfileSMs.RedSocialId = dbo.RedSocials.RedSocialId) " +
                                                "where dbo.Box_ProfileSM.BoxId = " + _Box.BoxId;
                string queryGetBoxWhatsapp = "select * from dbo.ProfileWhatsapps join dbo.Box_ProfileWhatsapp on " +
                                                "(dbo.ProfileWhatsapps.ProfileWhatsappId = dbo.Box_ProfileWhatsapp.ProfileWhatsappId) " +
                                                "where dbo.Box_ProfileWhatsapp.BoxId = " + _Box.BoxId;

                //Borrar box predeterminada anterior
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.DeleteAll<BoxLocal>();
                }
                //Borrar perfiles de box predeterminada anteriores
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.DeleteAll<ProfileLocal>();
                }

                //Crear box local predeterminada
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<BoxLocal>();
                    conn.Insert(boxLocal);
                }

                //Crear tabla de perfiles de box local predeterminada
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<ProfileLocal>();
                }


                //Consulta para obtener perfiles email
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryGetBoxEmail);

                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProfileLocal emailProfile = new ProfileLocal
                                {
                                    IdBox = _Box.BoxId,
                                    UserId = (int)reader["UserId"],
                                    ProfileName = (string)reader["Name"],
                                    value = (string)reader["Email"],
                                    ProfileType = "Email"
                                };
                                //Crear perfil de correo de box local predeterminada
                                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                                {
                                    conn.Insert(emailProfile);
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                //Consulta para obtener perfiles teléfono
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryGetBoxPhone);

                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProfileLocal phoneProfile = new ProfileLocal
                                {
                                    IdBox = _Box.BoxId,
                                    UserId = (int)reader["UserId"],
                                    ProfileName = (string)reader["Name"],
                                    value = (string)reader["Number"],
                                    ProfileType = "Phone"
                                };
                                //Crear perfil de teléfono de box local predeterminada
                                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                                {
                                    conn.Insert(phoneProfile);
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                //Consulta para obtener perfiles de redes sociales
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryGetBoxSMProfiles);

                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProfileLocal smProfile = new ProfileLocal
                                {
                                    IdBox = _Box.BoxId,
                                    UserId = (int)reader["UserId"],
                                    ProfileName = (string)reader["ProfileName"],
                                    value = (string)reader["link"],
                                    ProfileType = (string)reader["Name"]
                                };
                                //Crear perfil de teléfono de box local predeterminada
                                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                                {
                                    conn.Insert(smProfile);
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                //Consulta para obtener perfiles de whatsapp
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryGetBoxWhatsapp);

                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProfileLocal whatsappProfile = new ProfileLocal
                                {
                                    IdBox = _Box.BoxId,
                                    UserId = (int)reader["UserId"],
                                    ProfileName = (string)reader["Name"],
                                    value = (string)reader["Number"],
                                    ProfileType = "Whatsapp"
                                };
                                //Crear perfil de teléfono de box local predeterminada
                                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                                {
                                    conn.Insert(whatsappProfile);
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                //Consulta para predeterminar la box actual
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryUpdatesetDefault);
                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                //Consulta para quitar predeterminado de la box default anterior
                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(queryUpdateTakeOffDefault);
                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                MainViewModel.GetInstance().Home.GetBoxDefault();
                BxDefaultCheckBox.IsEnabled = false;
                MainViewModel.GetInstance().Home.GetBoxNoDefault();
            }
        }
        #endregion

        #region Methods
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        public Box GetBoxe(Box _Box)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            Box = new Box();
            var Box2 = this.apiService.GetBox(
                apiSecurity,
                "/api",
                "/Boxes",
                _Box.BoxId);

            return Box;
        }
        async void edithBox(object sender, EventArgs e, Box Box, int _UserId, bool _BoxDefault)
        {
            #region LastCode
            //string sqlDeleteEmails = "delete from dbo.Box_ProfileEmail where dbo.Box_ProfileEmail.BoxId = " + _BoxId,
            //        sqlDeletePhones = "delete from dbo.Box_ProfilePhone where dbo.Box_ProfilePhone.BoxId = " + _BoxId,
            //        sqlDeleteSMProfiles = "delete from dbo.Box_ProfileSM where dbo.Box_ProfileSM.BoxId = " + _BoxId,
            //        sqlDeleteWhatsappProfiles = "delete from dbo.Box_ProfileWhatsapp where dbo.Box_ProfileWhatsapp.BoxId = " + _BoxId,
            //        sqlDeleteBox = "delete from dbo.Boxes where dbo.boxes.BoxId = " + _BoxId;
            //string cadenaConexion = @"data source=serverappmynfo.database.windows.net;initial catalog=mynfo;user id=adminmynfo;password=4dmiNFC*Atx2020;Connect Timeout=60";
            ////string cadenaConexion = @"data source=serverappmynfo.database.windows.net;initial catalog=mynfo;user id=adminmynfo;password=4dmiNFC*Atx2020;Connect Timeout=60";
            //StringBuilder sb;
            //string sql;

            //bool answer = await DisplayAlert(Resource.Warning, Resource.DeleteBoxNotification, Resource.Yes, Resource.No);

            //if (answer == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(cadenaConexion))
            //    {
            //        //Borrar emails de la box
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(sqlDeleteEmails);
            //        sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }

            //        //Borrar teléfonos de la box
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(sqlDeletePhones);
            //        sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }

            //        //Borrar perfiles de redes sociales de la box
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(sqlDeleteSMProfiles);
            //        sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }

            //        //Borrar perfiles de whatsapp de la box
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(sqlDeleteWhatsappProfiles);
            //        sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }

            //        //Borrar box
            //        sb = new System.Text.StringBuilder();
            //        sb.Append(sqlDeleteBox);
            //        sql = sb.ToString();

            //        using (SqlCommand command = new SqlCommand(sql, connection))
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }

            //        //Si la box era predeterminada
            //        if (_BoxDefault == true)
            //        {
            //            //Borrar box predeterminada anterior
            //            using (var conn = new SQLite.SQLiteConnection(App.root_db))
            //            {
            //                conn.DeleteAll<BoxLocal>();
            //            }
            //            //Borrar perfiles de box predeterminada anteriores
            //            using (var conn = new SQLite.SQLiteConnection(App.root_db))
            //            {
            //                conn.DeleteAll<ProfileLocal>();
            //            }

            //            string sqlUpdateBoxDefault = "update top (1) dbo.Boxes set BoxDefault = 1 where dbo.Boxes.UserId = " + _UserId;
            //            BoxLocal boxLocal;
            //            bool boxLocalExists = false;
            //            int boxIdLocal = 0;

            //            //Definir nueva box default
            //            sb = new System.Text.StringBuilder();
            //            sb.Append(sqlUpdateBoxDefault);
            //            sql = sb.ToString();

            //            using (SqlCommand command = new SqlCommand(sql, connection))
            //            {
            //                connection.Open();
            //                command.ExecuteNonQuery();
            //                connection.Close();
            //            }


            //            string sqlGetNewDefault = "select * from dbo.Boxes " +
            //                                        "where dbo.Boxes.UserId = " + _UserId +
            //                                        "and dbo.Boxes.BoxDefault = 1";

            //            //Definir nueva box default
            //            sb = new System.Text.StringBuilder();
            //            sb.Append(sqlGetNewDefault);
            //            sql = sb.ToString();
            //            //Creación de nueva box local
            //            using (SqlCommand command = new SqlCommand(sql, connection))
            //            {
            //                connection.Open();
            //                using (SqlDataReader reader = command.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        boxLocal = new BoxLocal
            //                        {
            //                            BoxId = (int)reader["BoxId"],
            //                            BoxDefault = true,
            //                            Name = (String)reader["Name"],
            //                            UserId = MainViewModel.GetInstance().User.UserId,
            //                            Time = (DateTime)reader["Time"],
            //                            FirstName = MainViewModel.GetInstance().User.FirstName,
            //                            LastName = MainViewModel.GetInstance().User.LastName,
            //                            ImagePath = MainViewModel.GetInstance().User.ImagePath,
            //                            UserTypeId = MainViewModel.GetInstance().User.UserTypeId
            //                        };
            //                        //Crear box local predeterminada
            //                        using (var conn = new SQLite.SQLiteConnection(App.root_db))
            //                        {
            //                            conn.CreateTable<BoxLocal>();
            //                            conn.Insert(boxLocal);
            //                        }
            //                        //Crear tabla de perfiles de box local predeterminada
            //                        using (var conn = new SQLite.SQLiteConnection(App.root_db))
            //                        {
            //                            conn.CreateTable<ProfileLocal>();
            //                        }
            //                        boxLocalExists = true;
            //                        boxIdLocal = (int)reader["BoxId"];
            //                    }
            //                }

            //                connection.Close();
            //            }

            //            //Si se creo la box local, procedo a crear los perfiles locales
            //            if (boxLocalExists == true)
            //            {
            //                //Creación de perfiles locales de box local
            //                string queryGetBoxEmail = "select * from dbo.ProfileEmails " +
            //                                "join dbo.Box_ProfileEmail on" +
            //                                "(dbo.ProfileEmails.ProfileEmailId = dbo.Box_ProfileEmail.ProfileEmailId) " +
            //                                "where dbo.Box_ProfileEmail.BoxId = " + boxIdLocal;
            //                string queryGetBoxPhone = "select * from dbo.ProfilePhones " +
            //                                            "join dbo.Box_ProfilePhone on" +
            //                                            "(dbo.ProfilePhones.ProfilePhoneId = dbo.Box_ProfilePhone.ProfilePhoneId) " +
            //                                            "where dbo.Box_ProfilePhone.BoxId = " + boxIdLocal;
            //                string queryGetBoxSMProfiles = "select * from dbo.ProfileSMs " +
            //                                                "join dbo.Box_ProfileSM on" +
            //                                                "(dbo.ProfileSMs.ProfileMSId = dbo.Box_ProfileSM.ProfileMSId) " +
            //                                                "join dbo.RedSocials on(dbo.ProfileSMs.RedSocialId = dbo.RedSocials.RedSocialId) " +
            //                                                "where dbo.Box_ProfileSM.BoxId = " + boxIdLocal;
            //                string queryGetBoxWhatsappProfiles = "select * from dbo.ProfileWhatsapps join dbo.Box_ProfileWhatsapp on " +
            //                                    "(dbo.ProfileWhatsapps.ProfileWhatsappId = dbo.Box_ProfileWhatsapp.ProfileWhatsappId) " +
            //                                    "where dbo.Box_ProfileWhatsapp.BoxId = " + boxIdLocal;

            //                //Consulta para obtener perfiles email
            //                using (SqlConnection conn = new SqlConnection(cadenaConexion))
            //                {
            //                    sb = new System.Text.StringBuilder();
            //                    sb.Append(queryGetBoxEmail);

            //                    sql = sb.ToString();

            //                    using (SqlCommand command = new SqlCommand(sql, conn))
            //                    {
            //                        conn.Open();
            //                        using (SqlDataReader reader = command.ExecuteReader())
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                ProfileLocal emailProfile = new ProfileLocal
            //                                {
            //                                    IdBox = boxIdLocal,
            //                                    UserId = (int)reader["UserId"],
            //                                    ProfileName = (string)reader["Name"],
            //                                    value = (string)reader["Email"],
            //                                    ProfileType = "Email"
            //                                };
            //                                //Crear perfil de correo de box local predeterminada
            //                                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
            //                                {
            //                                    connSQLite.Insert(emailProfile);
            //                                }
            //                            }
            //                        }

            //                        conn.Close();
            //                    }
            //                }

            //                //Consulta para obtener perfiles teléfono
            //                using (SqlConnection conn = new SqlConnection(cadenaConexion))
            //                {
            //                    sb = new System.Text.StringBuilder();
            //                    sb.Append(queryGetBoxPhone);

            //                    sql = sb.ToString();

            //                    using (SqlCommand command = new SqlCommand(sql, conn))
            //                    {
            //                        conn.Open();
            //                        using (SqlDataReader reader = command.ExecuteReader())
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                ProfileLocal phoneProfile = new ProfileLocal
            //                                {
            //                                    IdBox = boxIdLocal,
            //                                    UserId = (int)reader["UserId"],
            //                                    ProfileName = (string)reader["Name"],
            //                                    value = (string)reader["Number"],
            //                                    ProfileType = "Phone"
            //                                };
            //                                //Crear perfil de teléfono de box local predeterminada
            //                                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
            //                                {
            //                                    connSQLite.Insert(phoneProfile);
            //                                }
            //                            }
            //                        }

            //                        conn.Close();
            //                    }
            //                }

            //                //Consulta para obtener perfiles de redes sociales
            //                using (SqlConnection conn = new SqlConnection(cadenaConexion))
            //                {
            //                    sb = new System.Text.StringBuilder();
            //                    sb.Append(queryGetBoxSMProfiles);

            //                    sql = sb.ToString();

            //                    using (SqlCommand command = new SqlCommand(sql, conn))
            //                    {
            //                        conn.Open();
            //                        using (SqlDataReader reader = command.ExecuteReader())
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                ProfileLocal smProfile = new ProfileLocal
            //                                {
            //                                    IdBox = boxIdLocal,
            //                                    UserId = (int)reader["UserId"],
            //                                    ProfileName = (string)reader["ProfileName"],
            //                                    value = (string)reader["link"],
            //                                    ProfileType = (string)reader["Name"]
            //                                };
            //                                //Crear perfil de teléfono de box local predeterminada
            //                                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
            //                                {
            //                                    connSQLite.Insert(smProfile);
            //                                }
            //                            }
            //                        }

            //                        conn.Close();
            //                    }
            //                }

            //                //Consulta para obtener perfiles whatsapp
            //                using (SqlConnection conn = new SqlConnection(cadenaConexion))
            //                {
            //                    sb = new System.Text.StringBuilder();
            //                    sb.Append(queryGetBoxWhatsappProfiles);

            //                    sql = sb.ToString();

            //                    using (SqlCommand command = new SqlCommand(sql, conn))
            //                    {
            //                        conn.Open();
            //                        using (SqlDataReader reader = command.ExecuteReader())
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                ProfileLocal whatsappProfile = new ProfileLocal
            //                                {
            //                                    IdBox = boxIdLocal,
            //                                    UserId = (int)reader["UserId"],
            //                                    ProfileName = (string)reader["Name"],
            //                                    value = (string)reader["Number"],
            //                                    ProfileType = "Whatsapp"
            //                                };
            //                                //Crear perfil de whatsapp de box local predeterminada
            //                                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
            //                                {
            //                                    connSQLite.Insert(whatsappProfile);
            //                                }
            //                            }
            //                        }

            //                        conn.Close();
            //                    }
            //                }
            //            }

            //        }
            //    }

            ////Regresar a home
            //var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Home = new HomeViewModel();
            //Application.Current.MainPage = new MasterPage();
            //}
            #endregion
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.DetailsBoxEdith = new DetailsBoxEdithViewModel(Box);
            await Navigation.PushAsync(new DetailsBoxEdithPage(Box));
        }

        private void BoxDetails_Clicked(object sender, EventArgs e, int _BoxId, bool _boxDefault, string _boxName)
        {
            //App.Navigator.PushAsync(new TabbedListOfNetworksPage(_BoxId, _boxDefault, _boxName), false);
            PopupNavigation.Instance.PushAsync(new ListOfNetworksPage(_BoxId));
        }

        private void BackHome (object sender, EventArgs e)
        {
            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}