using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using BlackBox.Mobile.Customer.ViewModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;

using System.Linq;
using System.Collections.Generic;

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
    }
}
