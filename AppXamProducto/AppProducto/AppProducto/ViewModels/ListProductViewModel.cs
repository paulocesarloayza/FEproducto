namespace AppProducto.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using AppProducto.Models;
    using AppProducto.Services;

    public class ListProductViewModel :BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<ProductItemViewModel> listProducts;
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<ProductItemViewModel> ListProducts 
        {
            get { return this.listProducts; }
            set { SetValue(ref this.listProducts, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public ListProductViewModel()
        {
            this.ListProducts = new ObservableCollection<ProductItemViewModel>();
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var main=MainViewModel.GetInstance();
            var lista = await this.apiService.Get<Product>(
                "https://webapiproductsecuritysi220.azurewebsites.net",
                "api/",
                "Products",
                main.TokenType,
                main.Token );
            ReloadProducts(lista);
            this.IsRefreshing = false;
        }

        private void ReloadProducts(List<Product> products)
        {
            ListProducts.Clear();
            foreach(var product in products)
            {
                ListProducts.Add(new ProductItemViewModel
                {
                    Name = product.Name,
                    ProductId = product.ProductId,
                    Value=product.Value,
                }); 
            }
        }

        #endregion
        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(
                    LoadProducts);
            }
        }
        #endregion
    }
}
