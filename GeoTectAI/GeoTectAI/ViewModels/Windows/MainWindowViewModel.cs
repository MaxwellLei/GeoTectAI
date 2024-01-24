// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using GeoTectAI.Services;
using GeoTectAI.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace GeoTectAI.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "WPF UI - GeoTectAI";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem("数据预测", SymbolRegular.DataHistogram24, typeof(DataPage))
            {
                MenuItems = new object[]
                {
                    new NavigationViewItem()
                    {
                        Content = "单项预测",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                        TargetPageType = typeof(Views.Pages.OneDataPage)
                    },
                    new NavigationViewItem()
                    {
                        Content = "批量预测",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                        TargetPageType = typeof(Views.Pages.MultipleDataPage)
                    }

                }
                
            } 
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
