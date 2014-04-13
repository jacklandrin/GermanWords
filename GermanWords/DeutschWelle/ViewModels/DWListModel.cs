using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows;

namespace GermanWords.DeutschWelle.ViewModels
{
    public class DWModel : ViewModelBase
    {
        private Visibility _visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    RaisePropertyChanged("Visibility");
                }
            }
        }

        private bool _isactivated = false;

        public bool IsActivated
        {
            get { return _isactivated; }
            set
            {
                _isactivated = value;
                RaisePropertyChanged("IsActivated");
            }
        }

        private ObservableCollection<DWListItemModel> _dwTilelist = new ObservableCollection<DWListItemModel>();

        public ObservableCollection<DWListItemModel> DWtileList
        {
            get { return _dwTilelist; }
            set 
            {
                if (_dwTilelist != value)
                {
                    _dwTilelist = value;
                    RaisePropertyChanged("DWtileList");
                }
            }
        }

        private DWArticalModel _dwartical = new DWArticalModel();

        public DWArticalModel DWArtical
        {
            get { return _dwartical; }
            set 
            {
                if (_dwartical != value)
                {
                    _dwartical = value;
                    RaisePropertyChanged("DWArtical");
                }
            }
        }


        public bool IsDataLoaded
        {
            get;
            private set;
        }

        //public void LoadData()
        //{
        //    HttpGet httpget = new HttpGet();
        //    _dwTilelist = httpget.GetTitle();
        //    this.IsDataLoaded = true;
        //}
    }
}
