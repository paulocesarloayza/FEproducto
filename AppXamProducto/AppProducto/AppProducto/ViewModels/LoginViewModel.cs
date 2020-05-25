
namespace AppProducto.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using AppProducto.Services;
    using AppProducto.Views;
    using Xamarin.Forms;
    using System;
    using AppProducto.Models;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes  
        private string email;
        private string password;
        private bool isenabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsEnabled
        {
            get { return this.isenabled; }
            set { SetValue(ref this.isenabled, value); }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        private async void Login()
        {         
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "ERROR",
                    "Usted debe ingresar un correo",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "ERROR",
                    "Usted debe ingresar el passwor",
                    "Accept");
                return;
            }
            this.IsEnabled = false;

            ApiService apiService = new ApiService();
            TokenResponse token = await apiService.GetToken(
                "https://webapiproductsecuritysi220.azurewebsites.net/",
                this.Email,
                this.Password);
            if (token == null)
            {
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "ERROR",
                    "SIN ACCESO",
                    "Accept");
                return;
            }
         
            if (string.IsNullOrEmpty(token.AccessToken))
            {              
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   token.ErrorDescription,
                   "Accept");
                this.Password = String.Empty;

                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;

            mainViewModel.Products = new ListProductViewModel();
            Application.Current.MainPage = new ListProductView();
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.Email = "pcloayza@upsa.edu.bo";
            this.Password = "Passw0rd";
        }
        #endregion
    }

}
