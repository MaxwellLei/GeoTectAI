using GeoTectAI.Models;
using GeoTectAI.Services;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private int _modelIndex;

        //图表内容
        [ObservableProperty]
        private ObservableCollection<ISeries> _myChartSeries;

        [ObservableProperty]
        private ObservableCollection<Axis> _xAxes;

        [ObservableProperty]
        private ObservableCollection<ISeries> _yAxes;

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
            //设置图表中文字体支持
            LiveCharts.Configure(config => config.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')));
        }

        //绘图
        private void PintChart(ObservableCollection<ISeries> tempSeries)
        {
            // 构造环境名称
            ObservableCollection<string> Categories = new ObservableCollection<string>
            { "类别1", "类别2", "类别3", "类别4", "类别5", "类别6", "类别7", "类别8" };

            MyChartSeries = tempSeries;
            // 创建堆积线图的数据系列
            //MyChartSeries = new ObservableCollection<ISeries>
            //{
            //    new LineSeries<double>
            //    {
            //        Values = new List<double> { 10, 20, 30, 40, 50, 60, 70, 80 },
            //        Name = "系列1"
            //    },
            //    new LineSeries<double>
            //    {
            //        Values = new List<double> { 15, 25, 35, 45, 55, 65, 75, 85 },
            //        Name = "系列2"
            //    }
            //    // 可以根据需要添加更多系列
            //};

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Labels = Categories,
                    
                }
            };
        }

        //预测
        [RelayCommand]
        private async void OnPredict()
        {
            //当前程序路径
            string appPath = System.IO.Directory.GetCurrentDirectory();
            //预测程序路径
            string predictExePath = appPath + "\\Executables\\predict.exe"; ; 
            //人工神经网络模型路径
            string ann_modelPath = appPath + "\\Resources\\ML_Model\\ANN_model.onnx";
            //随机模型路径
            string randomForest_modelPath = appPath + "\\Resources\\ML_Model\\RandomForest_model.onnx";
            //XGB模型路径
            string xGBoosting_modelPath = appPath + "\\Resources\\ML_Model\\XGBoosting_model.onnx";
            //预测数据
            var features = new float[] { SiO2, TiO2, Al2O3, CaO, MgO, MnO, K2O, Na2O, P2O5, La, Ce, Pr, Nd, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu };
            PredictorService predictorService = new PredictorService(predictExePath);
            if (ModelIndex == 0)
            {
                
                var (predictedClass1, probabilities1) = await predictorService.PredictAsync(ann_modelPath, features);
                var (predictedClass2, probabilities2) = await predictorService.PredictAsync(randomForest_modelPath, features);
                var (predictedClass3, probabilities3) = await predictorService.PredictAsync(xGBoosting_modelPath, features);

                ObservableCollection <ISeries> tempSeries = new ObservableCollection<ISeries>
                {
                    new LineSeries<double>
                    {
                        Values = (probabilities1.ToList()).Select(f => (double)f).ToList(),
                        Name = "系列1"
                    },
                    new LineSeries<double>
                    {
                        Values = (probabilities2.ToList()).Select(f => (double)f).ToList(),
                        Name = "系列2"
                    },
                    new LineSeries<double>
                    {
                        Values = (probabilities3.ToList()).Select(f => (double)f).ToList(),
                        Name = "系列3"
                    }
                };
                PintChart(tempSeries);
            }

        }
    }
}
