﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GermanWords.Page.GermanRelated
{
    public partial class AdjektiveTable : PhoneApplicationPage
    {
        public AdjektiveTable()
        {
            InitializeComponent();
            
            webbrower.Navigate(new Uri("/Resource/tables.html",UriKind.Relative));
        }
    }
}