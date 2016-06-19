using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.Services
{
    public interface ISqlLite
    {
        SQLiteConnection GetConnection();
    }

    public class LocalDBService
    {
        private static LocalDBService Service = new LocalDBService();

        private SQLiteConnection _db;
        public SQLiteConnection Db
        {
            get
            {
                if (_db == null)
                    _db = DependencyService.Get<ISqlLite>().GetConnection();
                return _db;
            }
        }
        public static LocalDBService GetInstance()
        {
            if (Service == null)
                Service = new LocalDBService();
            return Service;
        }

        private LocalDBService()
        {
            InitDb();
        }

        public void InitDb()
        {
            Db.CreateTable<ApiHackaton.Entities.AuthorizedModel>();
            Db.CreateTable<ApiHackaton.Entities.Customer>();
            Db.CreateTable<ApiHackaton.Entities.Device>();
            Db.CreateTable<ApiHackaton.Entities.DeviceOffer>();
            Db.CreateTable<ApiHackaton.Entities.Merchant>();
            Db.CreateTable<ApiHackaton.Entities.Offer>();
            Db.CreateTable<ApiHackaton.Entities.Order>();
        }
    }

    public class DAO<T>
    {
        private SQLiteConnection _coon;
        private SQLiteConnection Conn {
        get
            {
                if(_coon == null)
                {
                    var service = LocalDBService.GetInstance();
                    _coon = service.Db;
                }
                return _coon;
            }
        }
    }
}
