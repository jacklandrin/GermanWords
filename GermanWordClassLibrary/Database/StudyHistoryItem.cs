using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace GermanWordsClassLibrary.Database
{
    [Table]
    public class StudyHistoryItem : INotifyPropertyChanged, INotifyPropertyChanging
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

        private DateTime _date;

        [Column(DbType = "datetime")]
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date.CompareTo(value) != 0)
                {
                    NotifyPropertyChanging("Date");
                    _date = value;
                    NotifyPropertyChanged("Date");
                }
            }
        }

        private double _durationMinute;

        [Column]
        public double DurationMinute
        {
            get
            {
                return _durationMinute;
            }
            set
            {
                if (_durationMinute != value)
                {
                    NotifyPropertyChanging("DurationMinute");
                    _durationMinute = value;
                    NotifyPropertyChanged("DurationMinute");
                }
            }
        }

        private string _contentTitle;

        [Column]
        public string ContentTitle
        {
            get
            {
                return _contentTitle;
            }
            set
            {
                NotifyPropertyChanging("ContentTitle");
                _contentTitle = value;
                NotifyPropertyChanged("ContentTitle");
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
