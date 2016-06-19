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
    public partial class OfertaPage : ContentPage
    {
        private ApiService Service;
        private Offer Offer { get; set; }
        public OfertaPage(Offer oferta)
        {
            InitializeComponent();
            BindingContext = oferta;
            Offer = oferta;
            Service = ApiService.GetInstance();
            ComprarBtn.Clicked += ComprarBtn_Clicked;
            AddCarrinhoBtn.Clicked += AddCarrinhoBtn_Clicked;
        }

        private async void AddCarrinhoBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Service.CarrinhoCorrente.Label))
                NovoCarrinho();
            else
            {
                Service.CarrinhoCorrente.DeviceOffers.Add(new DeviceOffer
                {
                    Offer = Offer
                });
                await DisplayAlert("Carrinho", "Item adicionado com sucesso", "Ok");
                await Navigation.PushAsync(new OffersPage());
            }

        }
        public async void NovoCarrinho
            ()
        {
            var name = await InputBox(this.Navigation);
            if (!string.IsNullOrEmpty(name))
            {
                // checar carrinho
                Service.CarrinhoCorrente.Label = name;
                Service.CarrinhoCorrente.DeviceOffers.Add(new DeviceOffer
                {
                    Offer = Offer
                });

                await DisplayAlert("Carrinho", "Item adicionado com sucesso", "Ok");
                await Navigation.PushAsync(new OffersPage());
            }
        }

        private async void ComprarBtn_Clicked(object sender, EventArgs e)
        {
            var single = new SingleAuthorizedModel
            {
                CustomerId = Service.PessoaCorrente.Id,
                Offer = Offer
            };
            var result = await Service.CompraUnica(single);
            if (result)
                await DisplayAlert("Transação", "Pedido Enviado com sucesso!", "Ok");
            else
                await DisplayAlert("Transação", "Ocorreu um erro, tente novamente :/", "Ok");
        }
        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Novo carrinho", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Diga-nos o nome do carrinho:" };
            var txtInput = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancelar",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
    }
}
