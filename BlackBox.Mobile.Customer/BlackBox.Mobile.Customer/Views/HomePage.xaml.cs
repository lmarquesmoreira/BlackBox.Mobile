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
        private HomeViewModel ViewModel;
        private ApiService Service ;

        public HomePage(ApiHackaton.Entities.Customer customer)
        {
            InitializeComponent();
            
            ViewModel = new HomeViewModel();
            Service = new ApiService();

            ViewModel.Customer = customer;
            Load();

            DevicesListView.ItemTapped += DevicesListView_ItemTapped;
            AllOffersBtn.Clicked += HistoryBtn_Clicked;
           
        }

        private async void HistoryBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new OffersPage());
        }

        private void DevicesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DevicesListView.BackgroundColor = Color.Default;
            
            // send to another page
            DisplayAlert("Item tapped", e.Item.ToString(), "Ok");
        }

        async void Load()
        {
            ViewModel.Devices = await Service.GetDevicesByCustomerId(ViewModel.Customer.Id);
            DevicesListView.ItemsSource = ViewModel.GetGroupDevice(ViewModel.Devices);
        }
        void Handle_FabClicked(object sender, System.EventArgs e)
        {
            this.DisplayAlert("Floating Action Button", "You clicked the FAB!", "Awesome!");
        }
    }
}
