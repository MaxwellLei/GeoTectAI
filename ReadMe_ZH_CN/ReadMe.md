# 项目说明

本项目基于 MVVM 架构，使用 WPF 开发技术，依赖于 `.NET7` 框架，你可以通过旁边的 Releases 下载安装使用，项目是**基于岩浆岩的全岩地球化学元素来预测其对应的大地构造环境**。

> 项目协议：[Apache-2.0 license](https://github.com/MaxwellLei/GeoTectAI/tree/v1.0.0.0#)

# 项目原理

1. 首先使用 `Python` 来训练机器学习模型
2. 然后打包模型为 `EXE`，传入参数，返回结果
3. 使用 WPF 做软件前端和一些读取验证，预测过程调用 `EXE`，返回结果

# 项目截图

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/26ec2b65-765a-42c1-883a-d7cc79955460)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/faa2022a-8648-479c-bb56-eec1cbfddca9)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/68d9508f-c6a1-4047-89aa-7761a75c2908)

![image](https://github.com/MaxwellLei/GeoTectAI/assets/57181782/116d1065-a64c-448b-a495-5246da3ecea1)

