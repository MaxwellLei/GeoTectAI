using GeoTectAI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace GeoTectAI.Views.Pages
{
    /// <summary>
    /// MultipleDataPage.xaml 的交互逻辑
    /// </summary>
    public partial class MultipleDataPage : INavigableView<MultipleDataViewModel>
    {
        public MultipleDataViewModel ViewModel { get; }

        public MultipleDataPage(MultipleDataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
