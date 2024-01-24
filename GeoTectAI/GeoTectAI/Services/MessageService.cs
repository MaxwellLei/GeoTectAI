using GeoTectAI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace GeoTectAI.Services
{
    internal class MessageService
    {
        //全局静默通知
        public static SnackbarPresenter SnackbarPresenter { get; set; }
        //全局弹窗通知
        public static ContentPresenter ContentPresenter { get; set; }
        //数据项目窗体静默通知
        public static SnackbarPresenter SnackbarPresenter_ProjectDataWindow { get; set; }
        //数据项目窗体弹窗通知
        public static ContentPresenter ContentPresenter_ProjectDataWindow { get; set; }

        //常规通知自动适配设置
        public static void AutoShowDialog(string title, string message, ControlAppearance controlAppearance)
        {
            int time;
            switch (ConfigHelper.ReadConfig("NotificationMode"))
            {
                case "0":
                    //静默通知
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time),null);
                    break;
                case "1":
                    //展示系统弹窗
                    ShowContentDialog(title, message);
                    break;
                case "2":
                    break;
                default:
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time),null);
                    break;
            }
        }

        //常规通知自动适配设置
        public static void AutoShowDialog(string title, string message, ControlAppearance controlAppearance
                                        ,SnackbarPresenter? snackbarPresenter)
        {
            int time;
            switch (ConfigHelper.ReadConfig("NotificationMode"))
            {
                case "0":
                    //静默通知
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time), snackbarPresenter);
                    break;
                case "1":
                    //展示系统弹窗
                    ShowContentDialog(title, message);
                    break;
                case "2":
                    break;
                default:
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time), snackbarPresenter);
                    break;
            }
        }

        //常规通知自动适配设置(重载)
        public static void AutoShowDialog(string title, string message,
                                ControlAppearance controlAppearance, SymbolRegular symbolRegular
                                , SnackbarPresenter? snackbarPresenter)
        {
            int time;
            switch (ConfigHelper.ReadConfig("NotificationMode"))
            {
                case "0":
                    //静默通知
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time), symbolRegular
                        , snackbarPresenter);
                    break;
                case "1":
                    //展示系统弹窗
                    ShowContentDialog(title, message);
                    break;
                case "2":
                    break;
                default:
                    time = Convert.ToInt32(ConfigHelper.ReadConfig("NotificationTime"));
                    ShowSnackbar(title, message, controlAppearance, TimeSpan.FromSeconds(time), symbolRegular
                        , snackbarPresenter);
                    break;
            }
        }

        //静默通知方法
        public static void ShowSnackbar(string title, string message)
        {
            Snackbar snackbar = new Snackbar(SnackbarPresenter);
            snackbar.Appearance = ControlAppearance.Info;
            snackbar.Title = title;
            snackbar.Content = message;
            snackbar.Show();
        }

        //静默通知方法,重载
        public static void ShowSnackbar(string title, string message, ControlAppearance controlAppearance, 
            TimeSpan time, SnackbarPresenter? snackbarPresenter)
        {
            Snackbar snackbar;
            if (snackbarPresenter != null)
            {
                snackbar = new Snackbar(snackbarPresenter);
            }
            else
            {
                snackbar = new Snackbar(SnackbarPresenter);
            }
            snackbar.Title = title;
            snackbar.Icon = new SymbolIcon { Symbol = SymbolRegular.Bot24 };
            snackbar.Appearance = controlAppearance;
            snackbar.Timeout = time;
            snackbar.Content = message;
            snackbar.Show();
        }

        //静默通知方法,重载
        public static void ShowSnackbar(string title, string message,
            ControlAppearance controlAppearance, TimeSpan time, SymbolRegular symbolRegular
            , SnackbarPresenter? snackbarPresenter)
        {
            Snackbar snackbar = new Snackbar(SnackbarPresenter);
            snackbar.Title = title;
            snackbar.Icon = new SymbolIcon { Symbol = symbolRegular };
            snackbar.Appearance = controlAppearance;
            snackbar.Timeout = time;
            snackbar.Content = message;
            snackbar.Show();
        }

        //全局弹窗通知
        public static Task<ContentDialogResult> ShowContentDialog(string title, string content)
        {
            ContentDialog contentDialog = new ContentDialog(ContentPresenter);
            contentDialog.Title = title;
            contentDialog.Content = content;
            contentDialog.PrimaryButtonText = "确定";
            contentDialog.CloseButtonText = "取消";
            return contentDialog.ShowAsync();
        }

        //全局弹窗通知,重载
        public static async void ShowContentDialog(string title, string content, string firstButtonText
                                                , string secondButtonText, string cancelButtonText)
        {
            var contentDialogService = new ContentDialogService();
            contentDialogService.SetContentPresenter(ContentPresenter);

            await contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = title,
                    Content = content,
                    PrimaryButtonText = firstButtonText,
                    SecondaryButtonText = secondButtonText,
                    CloseButtonText = cancelButtonText
                }
                );
        }

        //常用弹窗，带标题
        public static async void ShowDialogAsync(string title, string content)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Title = title;
            messageBox.Content = content;
            await messageBox.ShowDialogAsync();
        }
        //常用弹窗，不带标题
        public static async void ShowDialogAsync(string content)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Content = content;
            await messageBox.ShowDialogAsync();
        }


    }
}
