using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GermanWords.DeutschWelle.ViewModels
{
    public class DWListItemModel : ViewModelBase
    {
        private SpeedMode _speedmode = SpeedMode.TWOMODE;

        public SpeedMode SpeedMode
        {
            get { return _speedmode; }
            set
            {
                if (_speedmode != value)
                {
                    _speedmode = value;
                    RaisePropertyChanged("SpeedMode");
                }

            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set 
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _abstruct;

        public string Abstruct
        {
            get { return _abstruct; }
            set 
            {
                if (_abstruct != value)
                {
                    _abstruct = value;
                    RaisePropertyChanged("Abstruct");
                }
            }
        }

        private string _urlPath;

        public string UrlPath
        {
            get { return _urlPath; }
            set 
            {
                if (_abstruct != value)
                {
                    _urlPath = value;
                    RaisePropertyChanged("UrlPath");
                }
            }
        }

    }
}
