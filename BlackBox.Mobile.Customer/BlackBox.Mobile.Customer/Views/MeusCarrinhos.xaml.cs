using BlackBox.Mobile.Customer.Services;
using BlackBox.Mobile.Customer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class MeusCarrinhos : ContentPage
    {
        private ApiService Service;
        private ApiHackaton.Entities.Customer Customer;
        private CarrinhosViewModel ViewModel;

        public MeusCarrinhos(ApiHackaton.Entities.Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            Service = ApiService.GetInstance();
            ViewModel = new CarrinhosViewModel();
            LoadCarrinhos();

            MeusCarrinhosListView.ItemTapped += MeusCarrinhosListView_ItemTapped;
            NovoCarrinhoBtn.Clicked += NovoCarrinhoBtn_Clicked;
        }

        private async void NovoCarrinhoBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Service.CarrinhoCorrente.Label))
            {
                NovoCarrinho(false);
            }
            else
            {
                var result = await DisplayAlert("Carrinho em aberto", "Você tem um carrinho em aberto, deseja cancelar e começar um novo?", "Sim", "Não");
                if (result)
                {
                    NovoCarrinho(true);
                }
            }
        }

        public async void NovoCarrinho(bool novoMesmo)
        {
            var name = await InputBox(this.Navigation);
            if (!string.IsNullOrEmpty(name))
            {
                if (novoMesmo)
                {
                    Service.CarrinhoCorrente = new ApiHackaton.Entities.AuthorizedModel
                    {
                        DeviceOffers = new List<ApiHackaton.Entities.DeviceOffer>()
                    };
                }
                Service.CarrinhoCorrente.Label = name;
                await Navigation.PushAsync(new OffersPage());
            }
        }

        private async void MeusCarrinhosListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ApiHackaton.Entities.AuthorizedModel;
            await Navigation.PushAsync(new MeuCarrinhoPage(item));
        }

        async void LoadCarrinhos()
        {
            var models = await Service.GetCarrinhos(Customer.Id);

            if (Service.CarrinhoCorrente.DeviceOffers.Count() > 0)
                models.Add(Service.CarrinhoCorrente);

            ViewModel.Carrinhos = models;
            MeusCarrinhosListView.ItemsSource = ViewModel.Carrinhos;

            if (models.Count() == 0)
                ViewModel.EmptyCarrinho = true;
            BindingContext = ViewModel;

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
