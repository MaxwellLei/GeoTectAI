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
        private string _applicationTitle = "GeoTectAI";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = LanguageService.Instance["Home"],
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem(LanguageService.Instance["DataPredict"], SymbolRegular.DataHistogram24, typeof(DataPage))
            {
                MenuItems = new object[]
                {
                    new NavigationViewItem()
                    {
                        Content = LanguageService.Instance["UPrediction"],
                        Icon = new SymbolIcon { Symbol = SymbolRegular.DataPie24 },
                        TargetPageType = typeof(Views.Pages.OneDataPage)
                    },
                    new NavigationViewItem()
                    {
                        Content = LanguageService.Instance["MPrediction"],
                        Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentLandscapeData24 },
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
                Content = LanguageService.Instance["Settings"],
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
