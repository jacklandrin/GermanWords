using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace GermanWordsClassLibrary.Database
{
    [Table]
    public class StarredWordItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _entryId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int EntryId
        {
            get
            {
                return _entryId;
            }
            set
            {
                if (_entryId != value)
                {
                    NotifyPropertyChanging("EntryId");
                    _entryId = value;
                    NotifyPropertyChanged("EntryId");
                }
            }
        }

        private int _wordSourceId;

        [Column]
        public int WordSourceId
        {
            get
            {
                return _wordSourceId;
            }
            set
            {
                if (_wordSourceId.Equals(value) == false)
                {
                    NotifyPropertyChanging("WordSourceId");
                    _wordSourceId = value;
                    NotifyPropertyChanged("WordSourceId");
                }
            }
        }

        private int _wordIdOfWordSource;

        [Column]
        public int WordIdOfWordSource
        {
            get
            {
                return _wordIdOfWordSource;
            }
            set
            {
                if (_wordIdOfWordSource != value)
                {
                    NotifyPropertyChanging("WordIdOfWordSource");
                    _wordIdOfWordSource = value;
                    NotifyPropertyChanged("WordIdOfWordSource");
                }
            }
        }

        private DateTime _addTime;

        [Column]
        public DateTime AddTime
        {
            get
            {
                return _addTime;
            }
            set
            {
                if (_addTime.Equals(value) == false)
                {
                    NotifyPropertyChanging("AddTime");
                    _addTime = value;
                    NotifyPropertyChanged("AddTime");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
