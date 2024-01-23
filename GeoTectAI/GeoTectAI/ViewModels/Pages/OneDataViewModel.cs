using GeoTectAI.Models;
using GeoTectAI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace GeoTectAI.ViewModels.Pages
{
    public partial class OneDataViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private float _SiO2;

        [ObservableProperty]
        private float _TiO2;

        [ObservableProperty]
        private float _Al2O3;

        [ObservableProperty]
        private float _CaO;

        [ObservableProperty]
        private float _MgO;

        [ObservableProperty]
        private float _MnO;

        [ObservableProperty]
        private float _K2O;

        [ObservableProperty]
        private float _Na2O;

        [ObservableProperty]
        private float _P2O5;

        [ObservableProperty]
        private float _La;

        [ObservableProperty]
        private float _Ce;

        [ObservableProperty]
        private float _Pr;

        [ObservableProperty]
        private float _Nd;

        [ObservableProperty]
        private float _Sm;

        [ObservableProperty]
        private float _Eu;

        [ObservableProperty]
        private float _Gd;

        [ObservableProperty]
        private float _Tb;

        [ObservableProperty]
        private float _Dy;

        [ObservableProperty]
        private float _Ho;

        [ObservableProperty]
        private float _Er;

        [ObservableProperty]
        private float _Tm;

        [ObservableProperty]
        private float _Yb;

        [ObservableProperty]
        private float _Lu;

        private bool _isInitialized = false;

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

        [RelayCommand]
        private async void OnPredict()
        {
            var predictorService = new PredictorService("D:\\Work Space\\Project\\GeoTectAI\\GeoTectAI\\GeoTectAI\\Executables\\predict.exe");
            var modelPath = "D:\\Work Space\\Project\\GeoTectAI\\GeoTectAI\\GeoTectAI\\Executables\\RandomForest_model.onnx";
            //var features = new float[] { 0.5846253f, 0.8544842f, 0.40910038f, 0.45354682f, 0.84390426f,
            //    0.44641337f, 0.25145847f, 0.4332255f, 0.533847f, 0.6043557f, 0.6473962f, 0.58364123f, 
            //    0.6483353f, 0.60697925f, 0.5437231f, 0.2620916f, 0.23379433f, 0.84173906f, 0.84291583f,
            //    0.7229303f, 0.31760728f, 0.8344309f, 0.47476304f };
            var features = new float[] { SiO2, TiO2, Al2O3, CaO, MgO, MnO, K2O, Na2O, P2O5, La, Ce, Pr, Nd, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu };

            var (predictedClass, probabilities) = await predictorService.PredictAsync(modelPath, features);
            Console.WriteLine($"Predicted class: {predictedClass}");
            Console.WriteLine($"Probabilities: {string.Join(", ", probabilities)}");

        }
    }
}
