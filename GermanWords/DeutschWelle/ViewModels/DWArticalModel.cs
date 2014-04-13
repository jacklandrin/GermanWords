using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows;

namespace GermanWords.DeutschWelle.ViewModels
{
    public class DWArticalModel : ViewModelBase
    {

        private double _progressvalue;

        public double ProgressValue
        {
            get { return _progressvalue; }
            set
            {
                _progressvalue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }


        private StatePlay _stateplay = StatePlay.NOPLAY;

        public StatePlay StatePlay
        {
            get { return _stateplay; }
            set 
            {
                if (_stateplay != value)
                {
                    _stateplay = value;
                    RaisePropertyChanged("StatePlay");
                }
                
            }
        }


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


        private string _playimage;

        public string PlayImage
        {
            get { return _playimage; }
            set 
            {
                if (_playimage != value)
                {
                    _playimage = value;
                    RaisePropertyChanged("PlayImage");
                }
            }
        }


        private string _speedimage;

        public string SpeedImage
        {
            get { return _speedimage; }
            set 
            {
                if (_speedimage != value)
                {
                    _speedimage = value;
                    RaisePropertyChanged("SpeedImage");
                }
            }
        }


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

        private bool _iscanplay;

        public bool IsCanPlay
        {
            get { return _iscanplay; }
            set 
            {
                if (_iscanplay != value)
                {
                    _iscanplay = value;
                    RaisePropertyChanged("IsCanPlay");
                }

            }
        }


        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
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

        private string _artical;

        public string Artical
        {
            get { return _artical; }
            set 
            {
                if (_abstruct != value)
                {
                    _artical = value;
                    RaisePropertyChanged("Artical");
                }
            }
        }

        private string _slowPath;

        public string SlowPath
        {
            get { return _slowPath; }
            set 
            {
                if (_slowPath != value)
                {
                    _slowPath = value;
                    RaisePropertyChanged("SlowPath");
                }
            }
        }

        private string _normalPath;

        public string NormalPath
        {
            get { return _normalPath; }
            set 
            {
                if (_normalPath != value)
                {
                    _normalPath = value;
                    RaisePropertyChanged("NormalPath");
                }
            }
        }


    }
}
