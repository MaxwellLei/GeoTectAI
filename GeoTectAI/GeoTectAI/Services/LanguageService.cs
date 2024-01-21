﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GeoTectAI.Services
{
    public class LanguageService : INotifyPropertyChanged
    {
        private readonly ResourceManager _resourceManager;

        private static readonly Lazy<LanguageService> _lazy = new Lazy<LanguageService>(() => new LanguageService());
        public static LanguageService Instance => _lazy.Value;
        public event PropertyChangedEventHandler PropertyChanged;

        public LanguageService()
        {
            //获取此命名空间下Resources的Lang的资源
            _resourceManager = new ResourceManager("GeoTectAI.Resources.Language", typeof(LanguageService).Assembly);
        }
        
        public string this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                return _resourceManager.GetString(name);
            }
        }

        public void ChangeLanguage(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("item[]"));  //字符串集合，对应资源的值
        }
    }
}
