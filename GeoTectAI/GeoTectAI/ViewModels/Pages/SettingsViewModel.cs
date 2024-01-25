// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using GeoTectAI.Helpers;
using GeoTectAI.Services;
using System.Globalization;
using Wpf.Ui.Controls;

namespace GeoTectAI.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        //软件主题【0，浅色；1，深色】
        [ObservableProperty]
        private int? _currentTheme;

        //软件语言【0，中文；1，英语(美国)】
        [ObservableProperty]
        private int? _currentLanguage;

        //弹窗模式【0，静默通知；1，弹窗通知】
        [ObservableProperty]
        private int? _currentPopUp;

        //静默通知时间
        [ObservableProperty]
        private int? _currentPopTime;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            //CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            AppVersion = $"{GetAssemblyVersion()}";
            ConfigInit();   //初始化设置
            _isInitialized = true;
        }

        //初始化设置——读取配置文件
        private void ConfigInit()
        {
            CurrentTheme = Convert.ToInt32(ConfigHelper.ReadConfig("Theme"));
            CurrentLanguage = Convert.ToInt32(ConfigHelper.ReadConfig("Language"));
            CurrentPopUp = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationMode"));
            CurrentPopTime = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
        }


        //获取软件版本号
        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        //改变主题
        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            if (CurrentTheme != null)
            {
                if (CurrentTheme == 0)
                {
                    //改变主题
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                }
                else
                {
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                }
                //写入配置文件
                ConfigHelper.WriteConfig("Theme", CurrentTheme.ToString());
            }
        }

        //改变语言
        [RelayCommand]
        private void OnChangeLanguage()
        {
            if (CurrentLanguage != null)
            {
                if (CurrentLanguage == 0)
                {
                    //改变语言
                    LanguageService.Instance.ChangeLanguage(new CultureInfo("zh-CN"));
                }
                else
                {
                    LanguageService.Instance.ChangeLanguage(new CultureInfo("en-US"));
                }
                //写入配置文件
                ConfigHelper.WriteConfig("Language", CurrentLanguage.ToString());
            }
        }

        //弹窗模式
        [RelayCommand]
        private void OnChangePopUpMode()
        {
            if (CurrentPopUp != null)
            {
                //写入配置文件
                ConfigHelper.WriteConfig("NotificationMode", CurrentPopUp.ToString());
            }
        }

        //静默通知自动关闭时间
        [RelayCommand]
        private void OnChangePopUpTime()
        {
            if (CurrentPopTime != null)
            {
                //写入配置文件
                ConfigHelper.WriteConfig("NotificationTime", CurrentPopTime.ToString());
            }
        }
    }
}
