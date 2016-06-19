using ApiHackaton.Entities;
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
            MeuCarrinho.Clicked += MeuCarrinho_Clicked;
            MeuHome.Clicked += MeuHome_Clicked;
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
                LabelX.Text = "Nenhum dispositivo foi registrado";
            else
                LabelX.Text = "Seus dispositivos";

        }
        void Handle_FabClicked(object sender, System.EventArgs e)
        {
            this.DisplayAlert("Floating Action Button", "You clicked the FAB!", "Awesome!");
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
