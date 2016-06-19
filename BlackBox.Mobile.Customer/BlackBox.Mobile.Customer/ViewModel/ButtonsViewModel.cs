using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.ViewModel
{
    public class OffersViewModel
    {

        public ObservableCollection<Group<ApiHackaton.Entities.Offer>> Offers { get; set; }

        public ObservableCollection<Group<ApiHackaton.Entities.Offer>> GetGroupOffers(Dictionary<string, List<ApiHackaton.Entities.Offer>> offers)
        {
            var Offers = new ObservableCollection<Group<ApiHackaton.Entities.Offer>>();

            foreach (var o in offers)
            {
                var page = new Group<ApiHackaton.Entities.Offer>(o.Key, o.Key);
                foreach (var offer in o.Value)
                    page.Add(offer);
                Offers.Add(page);
            }
            return Offers;
        }
    }

    public class HomeViewModel
    {
        public ApiHackaton.Entities.Customer Customer { get; set; }

        public List<ApiHackaton.Entities.Device> Devices { get; set; }

        public ObservableCollection<Group<ApiHackaton.Entities.Device>> GetGroupDevice(List<ApiHackaton.Entities.Device> devices)
        {
            var Devices = new ObservableCollection<Group<ApiHackaton.Entities.Device>>();

            var query = from device in devices
                        group device by device.Type into newGroup
                        orderby newGroup.Key
                        select newGroup;

            foreach (var q in query)
            {
                var page = new Group<ApiHackaton.Entities.Device>(q.Key, q.Key);
                foreach (var v in q)
                    page.Add(v);
                Devices.Add(page);
            }

            return Devices;
        }

    }


    public class Group<T> : ObservableCollection<T>
    {
        public string Name { get; private set; }
        public string ShortName { get; private set; }

        public Group(string Name, string ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }

    }
}
