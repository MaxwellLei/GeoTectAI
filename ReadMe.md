[⭐简体中文](https://github.com/MaxwellLei/GeoTectAI/tree/main/ReadMe_ZH_CN)

# Project Description

This project is based on the MVVM architecture, using WPF development technology, and relies on the `.NET7` framework. You can download and use it through the Releases section. The project is **based on the whole-rock geochemistry of igneous rocks to predict their corresponding tectonic settings**.

Thanks to the following project dependency libraries. If you want to compile or develop this project further, please install the following libraries first.

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

> Project Agreement: [Apache-2.0 license](https://github.com/MaxwellLei/GeoTectAI/tree/v1.0.0.0#)

# Machine learning models

## Datasets

The project training machine learning model data comes from two large geochemical databases [PetDB](http://www.earthchem.org/petdb) and [GEOROC](http://georoc.mpch-mainz.gwdg.de/georoc/) (🌹**Thanks to the database and many researchers for providing data support**🌹), the training data set of this project is stored in the `DataSet` file of the project.

> If you have any other needs or questions, please contact me by private message.

## About the code to train the model

The code to train the model, neural network, convert to ONNX format, and package EXE will be stored in the `Python` folder.

> ⚠️Please note: When calling the code, please make sure you have installed the relevant `Python` package dependencies

## Batch prediction

1. Batch prediction import file **only supports `Excel` type files** (for example `.xlsx`). After importing the file, the **default first line is the title line**, and the read data will **try to automatically match Attribute **, the content in the title row will be retrieved, **is not case-sensitive to match the names corresponding to geochemical elements (only relevant element strings are included) **, please check whether all match when making batch predictions On the list. The default is fuzzy matching, which will determine whether the list name contains a string corresponding to the geochemical element for matching. Compared with strict matching, it is more flexible. You can modify the matching mode through settings.

2. If a column is incorrectly matched, for example: the value of the `La` element is incorrectly matched to a `String` type column, it will be converted to `0f` by default and the prediction will continue with a warning message.

3. After the batch prediction is successful, a column named `pre_name` will be added to the far right of the table. This column is the prediction results. These prediction results will be automatically mapped to the corresponding structural categories (not including the discrimination probability value).

    > ⚠️: Please make sure that the header row of your imported `Excel` type file does not have a column header named `pre_name`, otherwise the content of the column will be overwritten after prediction.

4. The export of batch prediction results only supports the `.xlsx` type. After export, the file will be in the `Data_OutPut` folder under the installation path of the software (if there is no such folder, the software will automatically create it). The export file name is The exported time is converted to `SHA` to name. You can directly locate the exported file by opening the file location.

# 💡Project Principle

1. First use `Python` to train machine learning models.
2. Then package and read the `EXE` of the machine learning model, pass in the parameters, and return the results.
3. Use WPF to do the front-end of the software and some read verification, the prediction process calls `EXE` and returns the result.

> 🤔: Regarding why I chose to use this method, I initially tried to use Microsoft’s onnx library to directly call the trained model file, so that I didn’t need to go through Python to fiddle with it. After trying it, I found that this method would make my software The size is doubled. Compared with directly using Python to write an intermediate EXE and then calling it, a lot of space can be left.
>
> 🧐: If you ask me why I don’t consider using `PyQT` to write directly, this is very good in terms of efficiency and development speed. On the one hand, I have been using `C#` for a longer time and am more familiar with it. On the other hand, I think the interface written in `WPF` is more beautiful and flexible than `PyQT`.

# 🌹Project Screenshots

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/26ec2b65-765a-42c1-883a-d7cc79955460)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/27b6a083-63a2-4f85-8534-3d17fc2c08f2)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/68d9508f-c6a1-4047-89aa-7761a75c2908)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/116d1065-a64c-448b-a495-5246da3ecea1)
