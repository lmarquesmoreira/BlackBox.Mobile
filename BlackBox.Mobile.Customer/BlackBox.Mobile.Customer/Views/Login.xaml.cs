using BlackBox.Mobile.Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiHackaton.Entities;

using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class Login : ContentPage
    {

        private ApiService Service;

        public Login()
        {
            InitializeComponent();
            Service = new ApiService();
            Entrar.Clicked += Entrar_Clicked;

            BindingContext = this;
            IsBusy = false;
        }

        private bool _isBusy;
        public bool LoadingBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }


        private async void Entrar_Clicked(object sender, EventArgs e)
        {
            LoadingBusy = true;
            var id = int.Parse(CustomerId.Text);

            var customer = await Service.GetCustomerById(id);
            if (customer != null)
            {
                //verificar customerId
                // navegar para outra pagina
                await Navigation.PushAsync(new HomePage(customer));
                LoadingBusy = false;
            }
            else
                LoadingBusy = false;
        }
    }
}
