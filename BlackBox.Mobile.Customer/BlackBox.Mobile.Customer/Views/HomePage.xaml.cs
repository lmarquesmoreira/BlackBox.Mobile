using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using BlackBox.Mobile.Customer.ViewModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class HomePage : ContentPage
    {

        private ApiHackaton.Entities.Customer Customer;

        public HomePage(ApiHackaton.Entities.Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            DispositivosBtn.Clicked += DispositivosBtn_Clicked;
            CarrinhoBtn.Clicked += CarrinhoBtn_Clicked;
            OfertasBtn.Clicked += OfertasBtn_Clicked;
        }

        private async void OfertasBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new OffersPage());
        }

        private async void CarrinhoBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MeusCarrinhos(Customer));
        }

        private async void DispositivosBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MeusDispositivos(Customer));
        }
    }
}
