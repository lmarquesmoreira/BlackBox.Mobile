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
    public partial class OffersPage : ContentPage
    {
        private OffersViewModel ViewModel;
        private ApiService Service;

        public OffersPage()
        {
            InitializeComponent();

            ViewModel = new OffersViewModel();
            Service = new ApiService();

            LoadOffers();
        }

        private async void LoadOffers()
        {
            var offers = await Service.GetAllOffers();
            ViewModel.Offers = ViewModel.GetGroupOffers(offers);
            OffersListView.ItemsSource = ViewModel.Offers;
        }
    }
}
