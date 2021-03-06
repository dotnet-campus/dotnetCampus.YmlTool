# dotnetCampus.YmlTool

YAML格式化小工具

| Build | NuGet |
|--|--|
|![](https://github.com/dotnet-campus/dotnetCampus.YmlTool/workflows/.NET%20Core/badge.svg)|[![](https://img.shields.io/nuget/v/dotnetCampus.YmlTool.svg)](https://www.nuget.org/packages/dotnetCampus.YmlTool)|

针对开发人员编写 YAML 文件中可能出现的错误需要进行软件检验，并提供一键格式化功能

工具可实现对用来做多语言的 YAML 文件错误检验，包括：

- 0001:没有key
- 0002:没有value
- 0003:tab缩进
- 0004:重复键
- 0005:缺少`:`符号

提供一键格式化功能，包括：

- 错误修复

  - 删除没有 key 的项
  - 使用空格替代 tab （2个空格替代1个tab）
  - 对第二个重复key重命名为 duplicate_keyname （注意注意注意）

- 格式整理

  - 统一同一层级项目缩进
  - 统一 `key：value` 之间冒号后的空格数目（标准 YAML 要求冒号后有1个空格）


格式化后需要人工修复项目：

- 0002：没有value

此工具为多语言 YAML 文件辅助工具，团队内部使用 YAML 作为多语言文件，在构建的时候通过 [dotnet-campus/dotnetCampus.YamlToCsharp](https://github.com/dotnet-campus/dotnetCampus.YamlToCsharp ) 将 YAML 文件转 C# 代码

## 安装

这个工具通过 dotnet tool 发布，可以使用下面代码进行安装

```csharp
dotnet tool install -g dotnetCampus.YmlTool
```

## 使用方法

安装完成在命令行输入 `YmlTool` 即可打开

打开之后选择 yml 文件，选择完成自动测试文件是否存在错误

![](http://image.acmx.xyz/lindexi%2F20205161648467139.jpg)

## 如何贡献

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/dotnet-campus/dotnetCampus.YmlTool/pulls)

如果你希望参与贡献，欢迎 [Pull Request](https://github.com/dotnet-campus/dotnetCampus.YmlTool/pulls)，或给我们 [报告 Bug](https://github.com/dotnet-campus/dotnetCampus.YmlTool/issues/new) 