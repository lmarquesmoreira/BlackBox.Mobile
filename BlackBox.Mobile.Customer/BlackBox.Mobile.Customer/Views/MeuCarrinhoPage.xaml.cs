using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class MeuCarrinhoPage : ContentPage
    {
        private ApiService Service;
        private AuthorizedModel Model;
        public MeuCarrinhoPage(AuthorizedModel model)
        {
            InitializeComponent();
            Service = ApiService.GetInstance();
            Model = model;
            this.BindingContext = model;
            MeuCarrinhoListView.ItemsSource = model.DeviceOffers;
            ComprarAgoraBtn.Clicked += ComprarAgoraBtn_Clicked;
            ProgressEntrando.IsVisible = false;

        }

        private async void ComprarAgoraBtn_Clicked(object sender, EventArgs e)
        {
            ProgressEntrando.IsVisible = true;
            var result = await Service.ComprarMuitos(Model);
            if (result)
            {
                await DisplayAlert("Compra", "Compra Efetuada com Sucesso!", "Ok");
                Service.CarrinhoCorrente = new AuthorizedModel() { DeviceOffers = new List<DeviceOffer>() };
                await Navigation.PushAsync(new HomePage(Service.PessoaCorrente));
            }
            else
                await DisplayAlert("Compra", "Ocorreu um erro, tente novamente!", "Ok");

            ProgressEntrando.IsVisible = false;
        }
    }
}
