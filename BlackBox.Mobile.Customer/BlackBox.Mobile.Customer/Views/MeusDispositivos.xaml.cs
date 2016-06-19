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
    public partial class MeusDispositivos : ContentPage
    {
        private HomeViewModel ViewModel;
        private ApiService Service;

        public MeusDispositivos(ApiHackaton.Entities.Customer customer)
        {
            InitializeComponent();

            ViewModel = new HomeViewModel();
            Service = ApiService.GetInstance();

            ViewModel.Customer = customer;
            Load();

            DevicesListView.ItemTapped += DevicesListView_ItemTapped;
        }

        private async void DevicesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DevicesListView.BackgroundColor = Color.Default;

            var deviceItem = (ApiHackaton.Entities.Device)e.Item;
            await Navigation.PushAsync(new OffersPage(deviceItem));
        }

        async void Load()
        {
            ViewModel.Devices = await Service.GetDevicesByCustomerId(ViewModel.Customer.Id);
            DevicesListView.ItemsSource = ViewModel.GetGroupDevice(ViewModel.Devices);
            if (ViewModel.Devices.Count() == 0)
                LabelX.Text= "Nenhum dispositivo foi registrado";
            else
                LabelX.Text = "Seus dispositivos";

        }
        void Handle_FabClicked(object sender, System.EventArgs e)
        {
            this.DisplayAlert("Floating Action Button", "You clicked the FAB!", "Awesome!");
        }
    }
}
