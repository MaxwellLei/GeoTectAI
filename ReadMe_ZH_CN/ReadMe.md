# 项目说明

本项目基于 MVVM 架构，使用 WPF 开发技术，依赖于 `.NET7` 框架，你可以通过旁边的 Releases 下载安装使用，项目是**基于岩浆岩的全岩地球化学元素来预测其对应的大地构造环境**。

感谢如下项目依赖库，如果你希望编译本项目请先安装如下库支持。

* [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm)
* [EPPlus](https://www.epplussoftware.com/)
* [Ookii.Dialogs](https://www.ookii.org/software/dialogs/)
* [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/)
* [Microsoft.Xaml.Behaviors.Wpf](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Wpf)
* [WPF-UI](https://wpfui.lepo.co/index.html)
* [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)

你可以在 Releases 中找到软件安装包，关于命名说明：

* **DependencyFramework_FullPlatform** 表示可以**运行在 Windows,Linux,MacOS上**，但是**依赖 `.NET7` 框架**，意味着你如果希望运行这个程序，你需要先下载安装 `.NET7` 框架后才可运行。
* **independence_win_x86** 表示**仅运行在 Windows 平台上，不依赖于 .NET7 框架**，你可以直接下载解压运行使用。

如果你在使用过程中，出现任何 BUG，可以在项目的 **Issues** 下留言。

> 项目协议：[Apache-2.0 license](https://github.com/MaxwellLei/GeoTectAI/tree/v1.0.0.0#)

# 机器学习模型

## 数据集

项目训练机器学习模型数据来源于两个大型地球化学数据库 [PetDB](http://www.earthchem.org/petdb) 和 [GEOROC](http://georoc.mpch-mainz.gwdg.de/georoc/) ，本项目训练的数据集存放在项目的`Python-->Data`文件夹中。

> 如果有其他需要或者问题请私信联系我。

## 关于训练模型的代码

训练模型，神经网络，转换ONNX格式，打包EXE的代码会存放在`Python-->Code`文件夹中。

# 使用说明

项目训练好的模型会存放在安装目录下的`Resources-->ML_Model`文件夹中，对应的模型已经有了对应的名称。

> ⚠️请注意，不要重命名模型文件的名称，因为打包 EXE 文件传递参数返回值的问题，我简单的通过区别特定标识符来区别模型来返回不同的参数，重命名模型会出现不可预知的错误。

# 单项预测

单项预测会拒绝全 0 值和非数值型数值的预测，所以在预测前请检查自己的数据是否输入和数据类型是否正确。

> ⚠️：在某些情况下，输入数值的时候会出现系统精度自动补齐的情况，例如`0.23`，输入后变成了`0.230001`这种情况，请再次尝试修改为`0.23`即可。

## 批量预测

1. 批量预测导入文件仅支持`Excel`类型的文件（例`.xlsx`），在导入文件后，**默认第一行是标题行**，读取数据会**尝试自动匹配属性**，会检索标题行中的内容，**不区分大小写来匹配地球化学元素对应的名称**，在进行批量预测的时候请检查是否全部匹配上了列。

2. 如果错误的匹配了列，如：`La`元素的数值错误的匹配到了某个`String`类型的列，会默认将其转换为 `0f` 继续预测并伴随有警告消息通知。

3. 批量预测成功后会在表格的最右侧添加一列名称为`pre_name`，在这一列是预测结果，这些预测结果会自动映射成对应的构造类别（并不包含判别概率值）。

   > ⚠️：请确保自己的导入的`Excel`类型的文件标题行没有名称为`pre_name`的列标题，否则在预测后会覆盖该列的内容。

4. 批量预测结果的导出仅支持`.xlsx`类型，导出后文件会在软件的安装路径下的`Data_OutPut`文件夹中（如果没有该文件夹，软件会自动创建），导出文件名称是由导出的时间转换为`SHA`来命名的。你可以通过**打开文件所在位置**来直接定位导出的文件。

# 项目原理

1. 首先使用 `Python` 来训练机器学习模型
2. 然后打包模型为 `EXE`，传入参数，返回结果
3. 使用 WPF 做软件前端和一些读取验证，预测过程调用 `EXE`，返回结果

# 项目截图

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/26ec2b65-765a-42c1-883a-d7cc79955460)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/27b6a083-63a2-4f85-8534-3d17fc2c08f2)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/68d9508f-c6a1-4047-89aa-7761a75c2908)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/116d1065-a64c-448b-a495-5246da3ecea1)

