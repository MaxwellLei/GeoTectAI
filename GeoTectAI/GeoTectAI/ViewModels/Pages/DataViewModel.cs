// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using GeoTectAI.Helpers;
using GeoTectAI.Models;
using GeoTectAI.Services;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace GeoTectAI.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private readonly INavigationService _navigationService;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        public DataViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void OnCardClick(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
            {
                MessageService.AutoShowDialog("提示", "功能开发中...", Wpf.Ui.Controls.ControlAppearance.Info);
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                MessageService.AutoShowDialog("错误", "未找到页面", Wpf.Ui.Controls.ControlAppearance.Danger);
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }
    }
}
