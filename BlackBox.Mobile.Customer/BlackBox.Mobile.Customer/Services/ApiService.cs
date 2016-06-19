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

        public ApiService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
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
    }

}
