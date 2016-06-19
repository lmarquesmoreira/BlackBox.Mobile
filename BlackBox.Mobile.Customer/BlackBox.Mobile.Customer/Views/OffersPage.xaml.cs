using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using BlackBox.Mobile.Customer.ViewModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackBox.Mobile.Customer.Views
{
    public partial class OffersPage : ContentPage
    {
        private OffersViewModel OfertasViewModel;
        private ApiService Service;

        public OffersPage()
        {
            InitializeComponent();
            Init();
            LoadOffers();
        }

        public OffersPage(ApiHackaton.Entities.Device device)
        {
            InitializeComponent();
            Init();
            LoadOffersByDeviceId(device);
        }

        public void Init()
        {
            OfertasViewModel = new OffersViewModel();
            Service = ApiService.GetInstance();

            OffersListView.ItemTapped += OffersListView_ItemTapped;
            OffersResultListView.ItemTapped += OffersListView_ItemTapped;
            OffersResultListView.IsVisible = false;
            MeuSearchBar.TextChanged += (sender, e) => Filter(OfertasViewModel.SearchText);
            MeuSearchBar.SearchButtonPressed += (sender, e) => Filter(OfertasViewModel.SearchText);
            BindingContext = OfertasViewModel;
            MeuCarrinho.Clicked += MeuCarrinho_Clicked;
            MeuHome.Clicked += MeuHome_Clicked;
        }

        private async void MeuHome_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(Service.PessoaCorrente));
        }

        private async void MeuCarrinho_Clicked(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(Service.CarrinhoCorrente.Label))
            {
                var name = await InputBox(this.Navigation);
                if (!string.IsNullOrEmpty(name))
                {
                    // checar carrinho
                    Service.CarrinhoCorrente.Label = name;
                    Service.CarrinhoCorrente.DeviceOffers = new List<DeviceOffer>();
                    await Navigation.PushAsync(new OffersPage());
                }
            }
            else
                await Navigation.PushAsync(new MeuCarrinhoPage(Service.CarrinhoCorrente));
        }

        void Filter(string _searchText)
        {

            if (string.IsNullOrEmpty(_searchText))
            {
                OffersListView.ItemsSource = OfertasViewModel.Offers;

                OffersResultListView.IsVisible = false;
                OffersListView.IsVisible = true;
            }
            else
            {
                OffersResultListView.IsVisible = true;
                OffersListView.IsVisible = false;

                var ofertas = OfertasViewModel.OfertasLegais.Select(x => x.Value.ToList().Where(xx => xx.Label.ToLower().Contains(_searchText.ToLower())));

                var ddd = new List<Offer>();
                foreach (var o in ofertas)
                    foreach (var x in o)
                        ddd.Add(x);

                OffersResultListView.ItemsSource = ddd.OrderBy(x => x.Price);
            }
        }
        private async void OffersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var offer = e.Item as ApiHackaton.Entities.Offer;
            await Navigation.PushAsync(new OfertaPage(offer));
        }

        private async void LoadOffers()
        {
            ProgressEntrando.IsVisible = true;
            var offers = await Service.GetAllOffers();
            Apply(offers);
            ProgressEntrando.IsVisible = false;
        }

        private async void LoadOffersByDeviceId(ApiHackaton.Entities.Device device)
        {
            ProgressEntrando.IsVisible = true;

            var merchants = await Service.GetMerchants();
            var offers = await Service.GetOfferByDeviceId(device.Id.ToString());
            var group = offers.GroupBy(x => x.MerchantId);
            var items = new Dictionary<string, List<Offer>>();
            foreach (var item in group)
            {
                var title = merchants.FirstOrDefault(x => x.MerchantId.ToString() == item.Key).Name;
                items.Add(title, item.ToList());
            }
            Apply(items);
            ProgressEntrando.IsVisible = false;
            
        }
        private void Apply(Dictionary<string, List<Offer>> offers)
        {
            OfertasViewModel.OfertasLegais = offers;
            OfertasViewModel.Offers = OfertasViewModel.GetGroupOffers(offers);
            OffersListView.ItemsSource = OfertasViewModel.Offers;
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
