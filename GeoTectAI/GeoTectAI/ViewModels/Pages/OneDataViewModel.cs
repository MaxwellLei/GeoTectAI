using GeoTectAI.Models;
using GeoTectAI.Services;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace GeoTectAI.ViewModels.Pages
{
    public partial class OneDataViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private Visibility isLoadVisible = Visibility.Hidden;

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
        private ObservableCollection<ISeries> _myNightingaleRoseChartSeries;

        [ObservableProperty]
        private ObservableCollection<Axis> _xAxes;

        [ObservableProperty]
        private ObservableCollection<Axis> _yAxes;

        [ObservableProperty]
        private SolidColorPaint _legendTextPaint = new SolidColorPaint(SKColors.DeepSkyBlue);

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

        //堆折线图绘图
        private void PaintStackedLineChart(ObservableCollection<ISeries> tempSeries)
        {
            // 构造环境名称
            ObservableCollection<string> Categories = new ObservableCollection<string>
            { "1", "2", "3", "4", "5", "6", "7", "8" };

            MyChartSeries = tempSeries;

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Labels = Categories,
                    LabelsPaint = new SolidColorPaint(SKColors.DeepSkyBlue)
                }
            };

            YAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DeepSkyBlue)
                }
            };
        }

        //夜莺玫瑰绘图
        private void PaintNightingaleRoseChart(ObservableCollection<ISeries> basicBarsSeries)
        {

            MyNightingaleRoseChartSeries = basicBarsSeries;
        }

        private void AddToSeries(ObservableCollection<ISeries> seriesCollection, float[] probabilities, string modelName)
        {
            seriesCollection.Add(new LineSeries<double>
            {
                Values = probabilities.Select(f => (double)f).ToList(),
                Name = modelName
            });
        }

        private void AddToSeries(ObservableCollection<ISeries> basicBarsSeries, int preClass, string modelName)
        {
            basicBarsSeries.Add(new ColumnSeries<int>
            {
                Values = Enumerable.Repeat(preClass + 1, 1),
                Name = modelName
            }) ;
        }

        //预测
        [RelayCommand]
        private async void OnPredict()
        {
            //加载进度条
            IsLoadVisible = Visibility.Visible;
            // 获取当前程序路径
            string appPath = System.IO.Directory.GetCurrentDirectory();

            // 路径字典，方便根据ModelIndex索引模型
            var models = new Dictionary<int, (string path, string name)>
            {
                { 3, (appPath + "\\Resources\\ML_Model\\ANN_model.onnx", "人工神经网络") },
                { 1, (appPath + "\\Resources\\ML_Model\\RandomForest_model.onnx", "随机森林") },
                { 2, (appPath + "\\Resources\\ML_Model\\XGBoosting_model.onnx", "XGBooting") },
            };

            // 预测数据
            var features = new float[] { SiO2, TiO2, Al2O3, CaO, MgO, MnO, K2O, Na2O, P2O5, La, Ce, Pr, Nd, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu };

            // 预测服务
            string predictExePath = appPath + "\\Executables\\predict.exe";
            PredictorService predictorService = new PredictorService(predictExePath);

            // 生成图表数据
            ObservableCollection<ISeries> tempSeries = new ObservableCollection<ISeries>();
            ObservableCollection<ISeries> preClassSeries = new ObservableCollection<ISeries>();

            if (ModelIndex == 0)
            {
                foreach (var model in models.Values)
                {
                    var (predictedClass, probabilities) = await predictorService.PredictAsync(model.path, features);
                    AddToSeries(preClassSeries, predictedClass, model.name);
                    AddToSeries(tempSeries, probabilities, model.name);
                }
            }
            else if (models.ContainsKey(ModelIndex))
            {
                var model = models[ModelIndex];
                var (predictedClass, probabilities) = await predictorService.PredictAsync(model.path, features);
                AddToSeries(preClassSeries, predictedClass, model.name);
                AddToSeries(tempSeries, probabilities, model.name);
            }
            else
            {
                Console.WriteLine("警告：超出索引");
            }

            // 绘制图表
            PaintStackedLineChart(tempSeries);
            PaintNightingaleRoseChart(preClassSeries);

            IsLoadVisible = Visibility.Hidden;
            MessageService.AutoShowDialog("成功", "预测完成", ControlAppearance.Success);
        }
    }
}
