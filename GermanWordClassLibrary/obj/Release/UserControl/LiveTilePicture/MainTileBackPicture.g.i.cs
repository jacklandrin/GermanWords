﻿#pragma checksum "D:\Projects\GermanWords\GermanWordClassLibrary\UserControl\LiveTilePicture\MainTileBackPicture.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3DC9A5C0B806E86DD1CD18A920875E18"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34003
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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


namespace GermanWordsClassLibrary.UserControl.LiveTilePicture {
    
    
    public partial class MainTileBackPicture : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.TextBlock TextBlock_Head;
        
        internal System.Windows.Controls.Canvas Canvas_Content;
        
        internal System.Windows.Controls.TextBlock TextBlock_Value;
        
        internal System.Windows.Controls.TextBlock TextBlock_Unit;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GermanWordsClassLibrary;component/UserControl/LiveTilePicture/MainTileBackPictur" +
                        "e.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.TextBlock_Head = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_Head")));
            this.Canvas_Content = ((System.Windows.Controls.Canvas)(this.FindName("Canvas_Content")));
            this.TextBlock_Value = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_Value")));
            this.TextBlock_Unit = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_Unit")));
        }
    }
}

