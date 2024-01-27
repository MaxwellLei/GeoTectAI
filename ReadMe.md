[⭐简体中文](https://github.com/MaxwellLei/GeoTectAI/tree/main/ReadMe_ZH_CN)

# Project Description

This project is based on the MVVM architecture, using WPF development technology, relying on the `.NET7 `framework, which you can download and install via the Releases section next to it, is a project that **predicts the corresponding tectonic environment based on the whole rock geochemical elements of magmatic rocks**.

Thanks to the following project dependency libraries, if you want to compile this project, please install the following library support first.

* [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm)
* [EPPlus](https://www.epplussoftware.com/)
* [Ookii.Dialogs](https://www.ookii.org/software/dialogs/)
* [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/)
* [Microsoft.Xaml.Behaviors.Wpf](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Wpf)
* [WPF-UI](https://wpfui.lepo.co/index.html)
* [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)

You can find the software installation package in Releases, with naming instructions:

* **DependencyFramework_FullPlatform** means that it can **run on Windows, Linux, MacOS**, but **dependent The `.NET7`framework** means that if you wish to run this program you need to download and install it first `.NET7` framework.
* **independence_win_x86** means that it runs only on the Windows platform and does not depend on the `.NET7` framework can be downloaded and unzipped directly to run.

If you have any bugs in the process of use, you can leave a message under **Issues** of the project.

> Project Agreement: [Apache-2.0 license] (https://github.com/MaxwellLei/GeoTectAI/tree/v1.0.0.0#)

# Machine learning models

## Datasets

The data of the machine learning model trained by the project are derived from two large geochemical databases [PetDB](http://www.earthchem.org/petdb) and [GEOROC](http://georoc.mpch-mainz.gwdg.de/georoc/), and the dataset trained in this project is stored in the `Python-->Data` folder of the project.

> If you have any other needs or questions, please contact me by private message.

## About the code to train the model

The code to train the model, neural network, convert to ONNX format, and package EXE will be stored in the `Python-->Code` folder.

## Batch prediction

1. Batch prediction import file only supports `Excel` type files (for example `.xlsx`). After importing the file, the **default first line is the title line** and reading the data will **try to automatically match attributes* *, the content in the title row will be retrieved, **not case sensitive to match the names corresponding to geochemical elements**, please check whether all columns are matched when making batch predictions.

2. If a column is incorrectly matched, for example: the value of the `La` element is incorrectly matched to a `String` type column, it will be converted to `0f` by default and the prediction will continue with a warning message.

3. After the batch prediction is successful, a column named `pre_name` will be added to the far right of the table. This column is the prediction results. These prediction results will be automatically mapped to the corresponding structural categories (not including the discrimination probability value).

    > ⚠️: Please make sure that the header row of your imported `Excel` type file does not have a column header named `pre_name`, otherwise the content of the column will be overwritten after prediction.

4. The export of batch prediction results only supports the `.xlsx` type. After export, the file will be in the `Data_OutPut` folder under the installation path of the software (if there is no such folder, the software will automatically create it). The export file name is The exported time is converted to `SHA` to name. You can directly locate the exported file by opening the file location.

# 💡Project Principle

1. first use `Python` to train machine learning models
2. then package the model as an `EXE`, pass in the parameters, and return the results.
3. use WPF to do the front-end of the software and some read verification, the prediction process calls `EXE` and returns the result.

# 🌹Project Screenshots

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/26ec2b65-765a-42c1-883a-d7cc79955460)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/27b6a083-63a2-4f85-8534-3d17fc2c08f2)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/68d9508f-c6a1-4047-89aa-7761a75c2908)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/116d1065-a64c-448b-a495-5246da3ecea1)
