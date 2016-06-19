using BlackBox.Mobile.Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiHackaton.Entities;

using Xamarin.Forms;
using BlackBox.Mobile.Customer.ViewModel;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class Login : ContentPage
    {

        private ApiService Service;


        public Login()
        {
            InitializeComponent();
            Service = ApiService.GetInstance();
            Entrar.Clicked += Entrar_Clicked;
            BindingContext = this;
        }


        private async void Entrar_Clicked(object sender, EventArgs e)
        {
            ProgressEntrando.IsVisible = true;
            await this.FadeTo(0.1);
            var id = int.Parse(CustomerId.Text);

            var customer = await Service.GetCustomerById(id);
            if (customer != null)
            {
                ProgressEntrando.IsVisible = false;
                Service.PessoaCorrente = customer;
                //verificar customerId
                // navegar para outra pagina
                await Navigation.PushAsync(new HomePage(customer));
            }
            else
            {
                await DisplayAlert("Erro ao entrar", "Verifique seu id", "Ok");
                ProgressEntrando.IsVisible = false;
            }
            await this.FadeTo(1);
        }
    }
}
