using ApiHackaton.Entities;
using BlackBox.Mobile.Customer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlackBox.Mobile.Customer.ViewModel
{
    public class OffersViewModel
    {

        public Dictionary<string, List<Offer>> OfertasLegais;
        public ObservableCollection<Group<ApiHackaton.Entities.Offer>> Offers
        {
            get; set;
        }

        public ObservableCollection<Group<ApiHackaton.Entities.Offer>> GetGroupOffers(Dictionary<string, List<ApiHackaton.Entities.Offer>> offers)
        {
            var Offers = new ObservableCollection<Group<ApiHackaton.Entities.Offer>>();

            foreach (var o in offers)
            {
                var page = new Group<ApiHackaton.Entities.Offer>(o.Key, o.Key);
                var byo = o.Value.OrderBy(x => x.Price);
                foreach (var offer in byo)
                    page.Add(offer);
                Offers.Add(page);
            }
            return Offers;
        }


        #region Command and associated methods for SearchCommand
        private Xamarin.Forms.Command _searchCommand;
        public System.Windows.Input.ICommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new Xamarin.Forms.Command(DoSearchCommand, CanExecuteSearchCommand);
                return _searchCommand;
            }
        }
        private void DoSearchCommand()
        {
            // Refresh the list, which will automatically apply the search text
            //RaisePropertyChanged(() => YourList);
        }
        private bool CanExecuteSearchCommand()
        {
            return true;
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    //RaisePropertyChanged(() => SearchText);

                    // Perform the search
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }
            }
        }
        #endregion
    }

    public class HomeViewModel
    {
        public ApiHackaton.Entities.Customer Customer { get; set; }

        public List<ApiHackaton.Entities.Device> Devices { get; set; }

        public bool NaoTemDispositivos { get; set; }


        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

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

    public class CarrinhosViewModel
    {
        public List<ApiHackaton.Entities.AuthorizedModel> Carrinhos { get; set; }

        private bool _emptyCarrinho = false;
        public bool EmptyCarrinho { get { return _emptyCarrinho; } set { _emptyCarrinho = value; } }
    }

    public class MeuCarrinhoViewModel
    {
        public List<AuthorizedModel> Carrinhos { get; set; }
        public AuthorizedModel CarrinhoCorrente { get; set; }
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


    public class ProgressBarViewModel : INotifyPropertyChanged
    {
        public ProgressBarViewModel()
        {
        }

        private bool isIndeterminate;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indeterminate.
        /// </summary>
        /// <value><c>true</c> if this instance is indeterminate; otherwise, <c>false</c>.</value>
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set { isIndeterminate = value; OnPropertyChanged("IsIndeterminate"); }
        }

        private float progress = 0.0f;


        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public float Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        private double speed = 100;



        /// <summary>
        /// Gets or sets the speed f.
        /// I could use an IValueConverter here to convert from double to int if I needed to
        /// </summary>
        /// <value>The speed f.</value>
        public double SpeedF
        {
            get { return speed; }
            set
            {
                if (Math.Abs(speed - value) < double.Epsilon)
                    return;

                speed = value;
                OnPropertyChanged("Speed");
            }
        }
        /// <summary>
        /// Get to beind to the actual speed
        /// </summary>
        /// <value>The speed.</value>
        public int Speed
        {
            get { return (int)speed; }
        }


        private Command toggleIndeterminateCommand;
        /// <summary>
        /// When triggered InDeterminate flag will be flipped
        /// </summary>
        /// <value>The toggle indeterminate command.</value>
        public Command ToggleIndeterminateCommand
        {
            get
            {
                return toggleIndeterminateCommand ??
              (toggleIndeterminateCommand = new Command(ExecuteToggleIndeterminateCommand));
            }
        }


        private void ExecuteToggleIndeterminateCommand()
        {
            IsIndeterminate = !IsIndeterminate;
        }

        private Command<string> addProgressCommand;
        /// <summary>
        /// Based on the "float" that is passed in via string progress will be added or subtracted
        /// </summary>
        /// <value>The add progress command.</value>
        public Command AddProgressCommand
        {
            get { return addProgressCommand ?? (addProgressCommand = new Command<string>(ExecuteAddProgressCommand)); }
        }


        private void ExecuteAddProgressCommand(string toAdd)
        {
            float addThis = 0.0F;
            if (float.TryParse(toAdd, out addThis))
                Progress += addThis;
        }

        /// <summary>
        /// Gets the default color of the progress bar
        /// </summary>
        /// <value>The color of the progress.</value>
        public Color ProgressColor
        {
            get { return Color.FromHex("3498DB"); }
        }

        /// <summary>
        /// Gets the default background color of the progress bar
        /// </summary>
        /// <value>The color of the progress background.</value>
        public Color ProgressBackgroundColor
        {
            get { return Color.FromHex("B4BCBC"); }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
