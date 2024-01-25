using GeoTectAI.Helpers;
using GeoTectAI.Models;
using GeoTectAI.Services;
using HarfBuzzSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using static OfficeOpenXml.ExcelErrorValue;

namespace GeoTectAI.ViewModels.Pages
{
    public partial class MultipleDataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private string tempFile = string.Empty;
        private bool isPre = false;

        [ObservableProperty]
        private DataTable _data;    //读取的excel数据

        [ObservableProperty]
        private ObservableCollection<string> _columnNames;      //列名称

        [ObservableProperty]
        private int _modelIndex;        //选择的模型

        [ObservableProperty]
        private ObservableCollection<ISeries> _myNightingaleRoseChartSeries;

        [ObservableProperty]
        private Visibility isLoadVisible = Visibility.Hidden;

        [ObservableProperty]
        private ObservableCollection<Axis> _xAxes;

        [ObservableProperty]
        private ObservableCollection<Axis> _yAxes;

        [ObservableProperty]
        private int _siO2Index;

        [ObservableProperty]
        private int _tiO2Index;

        [ObservableProperty]
        private int _al2O3Index;

        [ObservableProperty]
        private int _caOIndex;

        [ObservableProperty]
        private int _mgOIndex;

        [ObservableProperty]
        private int _mnOIndex;

        [ObservableProperty]
        private int _k2OIndex;

        [ObservableProperty]
        private int _na2OIndex;

        [ObservableProperty]
        private int _p2O5Index;

        [ObservableProperty]
        private int _laIndex;

        [ObservableProperty]
        private int _ceIndex;

        [ObservableProperty]
        private int _prIndex;

        [ObservableProperty]
        private int _ndIndex;

        [ObservableProperty]
        private int _smIndex;

        [ObservableProperty]
        private int _euIndex;

        [ObservableProperty]
        private int _gdIndex;

        [ObservableProperty]
        private int _tbIndex;

        [ObservableProperty]
        private int _dyIndex;

        [ObservableProperty]
        private int _hoIndex;

        [ObservableProperty]
        private int _erIndex;

        [ObservableProperty]
        private int _tmIndex;

        [ObservableProperty]
        private int _ybIndex;

        [ObservableProperty]
        private int _luIndex;


        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            // 设置 EPPlus 的 LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            _isInitialized = true;
        }

        //返回选择列对应的索引
        private int GetColumnIndex(string columnName)
        {
            return Data.Columns.IndexOf(columnName);
        }

        //夜莺玫瑰绘图
        private void PaintNightingaleRoseChart(ObservableCollection<ISeries> basicBarsSeries)
        {

            MyNightingaleRoseChartSeries = basicBarsSeries;

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
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

        //读取数据
        public List<float[]> ExtractData()
        {
            var extractedData = new List<float[]>();

            // 获取选择的列的索引
            var indexes = new int[]
            {
                SiO2Index, TiO2Index, Al2O3Index, CaOIndex, MgOIndex, MnOIndex,
                K2OIndex, Na2OIndex, P2O5Index, LaIndex, CeIndex, PrIndex,
                NdIndex, SmIndex, EuIndex, GdIndex, TbIndex, DyIndex,
                HoIndex, ErIndex, TmIndex, YbIndex, LuIndex
            };

            // 遍历每一行
            foreach (DataRow row in Data.Rows)
            {
                var rowData = new List<float>();
                foreach (var index in indexes)
                {
                    if (index >= 0 && index < Data.Columns.Count)
                    {
                        if (float.TryParse(row[index].ToString(), out float value))
                        {
                            rowData.Add(value);
                        }
                        else
                        {
                            rowData.Add(0f);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MessageService.AutoShowDialog(LanguageService.Instance["Error"], LanguageService.Instance["Error_1"], ControlAppearance.Danger);
                            });
                        }
                    }
                }
                extractedData.Add(rowData.ToArray());
            }

            return extractedData;
        }

        //加载列名称
        private void LoadColumnNames()
        {
            ColumnNames = new ObservableCollection<string>();
            foreach (DataColumn column in Data.Columns)
            {
                ColumnNames.Add(column.ColumnName);
            }
        }

        //读取excel文件返回DataTable
        public DataTable ReadExcelFile(string filePath)
        {
            var dataTable = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                // 添加列到DataTable
                for (int col = 1; col <= colCount; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Text);
                }

                // 添加行到DataTable
                for (int row = 2; row <= rowCount; row++)
                {
                    var newRow = dataTable.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        newRow[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dataTable.Rows.Add(newRow);
                }
            }

            return dataTable;
        }

        //添加列
        public void AddColumnToDataTable(DataTable dataTable, string columnName, List<string> data)
        {
            // 添加新列
            if (!dataTable.Columns.Contains(columnName))
            {
                dataTable.Columns.Add(columnName, typeof(string));
            }

            // 填充新列的数据
            for (int i = 0; i < data.Count; i++)
            {
                // 如果行数不足，添加新行
                if (i >= dataTable.Rows.Count)
                {
                    var newRow = dataTable.NewRow();
                    newRow[columnName] = data[i];
                    dataTable.Rows.Add(newRow);
                }
                else
                {
                    dataTable.Rows[i][columnName] = data[i];
                }
            }
        }

        //返回指定匹配字符的索引
        private int FindIndexIgnoreCase(ObservableCollection<string> collection, string value)
        {
            // 检索集合中指定字符串的索引
            var index = collection
                .Select((s, i) => new { String = s, Index = i })
                .FirstOrDefault(x => x.String.Equals(value, StringComparison.OrdinalIgnoreCase))
                ?.Index;

            // 如果找到了匹配项，返回索引；否则返回 -1
            return index ?? -1;
        }

        //自动匹配
        private void autoCheck()
        {
            //主量元素
            SiO2Index = FindIndexIgnoreCase(ColumnNames, "SIO2");
            TiO2Index = FindIndexIgnoreCase(ColumnNames, "TiO2");
            Al2O3Index = FindIndexIgnoreCase(ColumnNames, "Al2O3");
            CaOIndex = FindIndexIgnoreCase(ColumnNames, "CaO");
            MgOIndex = FindIndexIgnoreCase(ColumnNames, "MgO");
            MnOIndex = FindIndexIgnoreCase(ColumnNames, "MnO");
            K2OIndex = FindIndexIgnoreCase(ColumnNames, "K2O");
            Na2OIndex = FindIndexIgnoreCase(ColumnNames, "Na2O");
            P2O5Index = FindIndexIgnoreCase(ColumnNames, "P2O5");

            //微量元素
            LaIndex = FindIndexIgnoreCase(ColumnNames, "La");
            CeIndex = FindIndexIgnoreCase(ColumnNames, "Ce");
            PrIndex = FindIndexIgnoreCase(ColumnNames, "pr");
            NdIndex = FindIndexIgnoreCase(ColumnNames, "nd");
            SmIndex = FindIndexIgnoreCase(ColumnNames, "sm");
            EuIndex = FindIndexIgnoreCase(ColumnNames, "eu");
            GdIndex = FindIndexIgnoreCase(ColumnNames, "gd");
            TbIndex = FindIndexIgnoreCase(ColumnNames, "tb");
            DyIndex = FindIndexIgnoreCase(ColumnNames, "dy");
            HoIndex = FindIndexIgnoreCase(ColumnNames, "ho");
            ErIndex = FindIndexIgnoreCase(ColumnNames, "er");
            TmIndex = FindIndexIgnoreCase(ColumnNames, "tm");
            YbIndex = FindIndexIgnoreCase(ColumnNames, "yb");
            LuIndex = FindIndexIgnoreCase(ColumnNames, "lu");
        }

        //导出为excel文件
        private void ExportDataTableToExcel(DataTable dataTable, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // 添加列标题
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // 添加行数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataTable.Rows[i][j];
                    }
                }

                // 保存到文件
                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }
        }

        //随机生成文件名称
        private string GetFileNameByTime()
        {
            DateTime currentTime = DateTime.Now;
            string timeString = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            byte[] timeBytes = Encoding.UTF8.GetBytes(timeString);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(timeBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        //导入文件
        [RelayCommand]
        private void OnImportFile()
        {
            Task task = new Task(() =>
            {
                var temp = FileHelper.GetExcelFiles(false);
                if (temp != null)
                {
                    Data = ReadExcelFile(temp[0]);  //读取数据
                    LoadColumnNames();      //加载列名称
                    autoCheck();    //自动匹配
                    isPre = false;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageService.AutoShowDialog(LanguageService.Instance["Success"], LanguageService.Instance["Success_1"], ControlAppearance.Success);
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageService.AutoShowDialog(LanguageService.Instance["Info"], LanguageService.Instance["Info_1"], ControlAppearance.Info);
                    });
                }
            });
            task.Start();
            
        }

        //清除数据
        [RelayCommand]
        private void OnDeleteData()
        {
            Data = null;
            ColumnNames = null;
            isPre = false;
            MyNightingaleRoseChartSeries = null;
            MessageService.AutoShowDialog(LanguageService.Instance["Success"], LanguageService.Instance["Success_2"], ControlAppearance.Success);
        }

        //批量预测
        [RelayCommand]
        private async void OnMultiplePredict()
        {
            IsLoadVisible = Visibility.Visible;

            ObservableCollection<ISeries> tempSeries = new ObservableCollection<ISeries>(); 
            // 获取当前程序路径
            string appPath = System.IO.Directory.GetCurrentDirectory();

            // 路径字典，方便根据ModelIndex索引模型
            var models = new Dictionary<int, (string path, string name)>
            {
                { 3, (appPath + "\\Resources\\ML_Model\\ANN_model.onnx", "人工神经网络") },
                { 1, (appPath + "\\Resources\\ML_Model\\RandomForest_model.onnx", "随机森林") },
                { 2, (appPath + "\\Resources\\ML_Model\\XGBoosting_model.onnx", "XGBooting") },
            };

            // 预测服务
            string predictExePath = appPath + "\\Executables\\predict.exe";
            PredictorService predictorService = new PredictorService(predictExePath);

            //预测结果
            List<string> predictRes = new List<string>();

            var multiple_Data = ExtractData();
            if(models.ContainsKey(ModelIndex+1))
            {
                var model = models[ModelIndex+1];
                foreach (var data in multiple_Data)
                {
                    var (predictedClass, probabilities) = await predictorService.PredictAsync(model.path, data);
                    predictRes.Add(CategoryMapper.GetCategoryName(predictedClass));
                    tempSeries.Add(new ColumnSeries<int>
                    {
                        Values = Enumerable.Repeat(predictedClass + 1, 1),
                    });
                }
            }
            DataTable temptable = Data.Copy();
            AddColumnToDataTable(temptable, "pre_name", predictRes);
            Data = temptable;
            PaintNightingaleRoseChart(tempSeries);
            IsLoadVisible = Visibility.Hidden;
            MessageService.AutoShowDialog(LanguageService.Instance["Success"], LanguageService.Instance["Success_3"], ControlAppearance.Success);
        }

        //导出Excel
        [RelayCommand]
        private void OnExportAsExcel()
        {
            if(Data == null)
            {
                MessageService.AutoShowDialog(LanguageService.Instance["Error"], LanguageService.Instance["Error_2"], ControlAppearance.Danger);
            }
            else
            {
                if (isPre)
                {
                    Task task = new Task(() =>
                    {
                        string appPath = System.IO.Directory.GetCurrentDirectory() + "\\Data_OutPut";
                        FileHelper.CreateFolder(appPath);
                        tempFile = appPath + "\\" + GetFileNameByTime() + ".xlsx";
                        ExportDataTableToExcel(Data, tempFile);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageService.AutoShowDialog(LanguageService.Instance["Success"], LanguageService.Instance["Success_4"], ControlAppearance.Success);
                        });
                    });
                    task.Start();
                }
                else
                {
                    MessageService.AutoShowDialog(LanguageService.Instance["Error"], LanguageService.Instance["Error_3"], ControlAppearance.Danger);
                }

            }
        }

        //打开文件位置
        [RelayCommand]
        private void OnOpenFileLocation()
        {
            string appPath = System.IO.Directory.GetCurrentDirectory() + "\\Data_OutPut";
            FileHelper.CreateFolder(appPath);
            if (tempFile == string.Empty)
            {
                FileHelper.OpenFolder(System.IO.Directory.GetCurrentDirectory() + "\\Data_OutPut");
            }
            else
            {
                FileHelper.OpenFolder(tempFile);
            }
            MessageService.AutoShowDialog(LanguageService.Instance["Success"], LanguageService.Instance["Success_5"], ControlAppearance.Success);
        }
    }
}
