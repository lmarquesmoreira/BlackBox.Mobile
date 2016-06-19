using ApiHackaton.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlackBox.Mobile.Customer.Services
{
    public class ApiService
    {
        HttpClient client;

        private ApiService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            CarrinhoCorrente = new AuthorizedModel { DeviceOffers = new List<DeviceOffer>() };
        }

        public AuthorizedModel CarrinhoCorrente { get; set; }

        private ApiHackaton.Entities.Customer _customer;
        public ApiHackaton.Entities.Customer PessoaCorrente
        {
            get
            {
                return _customer;
            }
            set {
                _customer = value;
                CarrinhoCorrente.CustomerId = _customer.Id;
            }
        }

        private static ApiService _apiService = new ApiService();

        public static ApiService GetInstance()
        {
            if (_apiService == null)
                _apiService = new ApiService();
            return _apiService;
        }


        private static string Prefix = "https://uberblackbox.azurewebsites.net";

        private Func<string, string> RequestUrl = (v) => { return string.Format("{0}/{1}", Prefix, v); };


        public async Task<List<Merchant>> GetMerchants()
        {
            var url = new Uri(RequestUrl("BlackBox/Merchants"));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Merchant>>(content);
            }
            return null;
        }

        public async Task<ApiHackaton.Entities.Customer> GetCustomerById(int id)
        {
            var url = new Uri(RequestUrl(string.Format("BlackBox/Customer?id={0}", id)));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiHackaton.Entities.Customer>(content);
            }
            return null;
        }

        public async Task<List<Device>> GetDevicesByCustomerId(int id)
        {
            var url = new Uri(RequestUrl(string.Format("BlackBox/Device/DevicesByCustomerId?customerId={0}", id)));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Device>>(content);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> String = MerchantId; Offer => Offer</returns>
        public async Task<Dictionary<string, List<Offer>>> GetAllOffers()
        {
            var url = new Uri(RequestUrl("BlackBox/Offer/AllOffers"));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, List<Offer>>>(content);
            }
            return null;
        }

        public async Task<List<Offer>> GetOfferByDeviceId(string deviceId)
        {
            var url = new Uri(RequestUrl(string.Format("BlackBox/Offer/OfferByDevice?deviceId={0}", deviceId)));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Offer>>(content);
            }
            return null;
        }

        public async Task<List<DeviceOffer>> GetOfferByToken(Guid token)
        {
            var url = new Uri(RequestUrl("BlackBox/Device/DeviceOfferByOrderId"));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DeviceOffer>>(content);
            }
            return null;
        }

        public async Task<List<AuthorizedModel>> GetCarrinhos(int id)
        {
            //BlackBox/Device/AuthorizedModelsByCustomerId?customerId={customerId}
            var url = new Uri(RequestUrl(string.Format("BlackBox/Device/AuthorizedModelsByCustomerId?customerId={0}", id)));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AuthorizedModel>>(content);
            }
            return null;
        }

        public async Task<AuthorizedModel> GetCarrinho(int id)
        {
            //BlackBox/Device/AuthorizedModelsByCustomerId?customerId={customerId}
            var url = new Uri(RequestUrl(string.Format("BlackBox/Device/DeviceOfferByCustomerId?customerId={0}", id)));
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthorizedModel>(content);
            }
            return null;
        }

        public async Task<bool> ComprarMuitos(AuthorizedModel model)
        {
            var url = new Uri(RequestUrl(string.Format("BlackBox/Shop/AuthorizeCart")));
            var response = await client.PostAsJsonAsync<AuthorizedModel>(url, model);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> CompraUnica(SingleAuthorizedModel model)
        {
            var url = new Uri(RequestUrl(string.Format("BlackBox/Shop/Authorize")));
            var response = await client.PostAsJsonAsync<SingleAuthorizedModel>(url, model);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }

}
