﻿#pragma checksum "D:\Workspace\Visual_Studio_for_WP\GermanWords\ScheduledTaskAgent1\UserControl\WordCard\TwoSideWordCard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "53AEFA2F28DBA69703BFCFCD755D6FA5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace GermanWords.UserControl.WordCard {
    
    
    public partial class TwoSideWordCard : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid Grid_Front;
        
        internal System.Windows.Controls.TextBlock TextBlock_FrontLine0;
        
        internal System.Windows.Controls.TextBlock TextBlock_FrontLine1;
        
        internal System.Windows.Controls.Grid Grid_Back;
        
        internal System.Windows.Controls.TextBlock TextBlock_BackLine1;
        
        internal System.Windows.Media.Animation.Storyboard StoryBoard_TurnBack;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/ScheduledTaskAgent1;component/UserControl/WordCard/TwoSideWordCard.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Grid_Front = ((System.Windows.Controls.Grid)(this.FindName("Grid_Front")));
            this.TextBlock_FrontLine0 = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_FrontLine0")));
            this.TextBlock_FrontLine1 = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_FrontLine1")));
            this.Grid_Back = ((System.Windows.Controls.Grid)(this.FindName("Grid_Back")));
            this.TextBlock_BackLine1 = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_BackLine1")));
            this.StoryBoard_TurnBack = ((System.Windows.Media.Animation.Storyboard)(this.FindName("StoryBoard_TurnBack")));
        }
    }
}
