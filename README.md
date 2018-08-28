# [QQRobot](https://github.com/IllyaTheHath/QQRobot)

## 简介
[QQRobot](https://github.com/IllyaTheHath/QQRobot) 是一个基于 dotnet core 2.0 框架开发的 QQ 聊天机器人
目前阶段功能实现：
* 图灵聊天机器人：监听 QQ 群消息，发现有带有机器人名字的内容时通过图灵机器人进行智能回复
* 群消息复读机：监听 QQ 群消息，随机复读群成员说的话

## 截图
![截图1](https://github.com/IllyaTheHath/QQRobot/blob/master/pic/pic1.png)  
![截图2](https://github.com/IllyaTheHath/QQRobot/blob/master/pic/pic2.png)

## 启动
* 安装 dotnet core 运行环境
* 下载二进制文件 或 下载源码手动编译
* 对二进制文件加入执行权限(Linux/OSX)
* 修改配置文件
* 运行程序，然后根据输出提示进行扫码登录

## 配置
配置文件为 config.ini：  
**[GlobalConfig] - 全局设置**  

	BotName		机器人名字  
	BotType		机器人类型  
	WorkGroup	工作群组，以","分隔

**[BotTypes] - 机器人类型列表**  

**[Turing] - 图灵机器人设置**  

	Api	图灵机器人的 API 地址
	Key	图灵机器人的 API-Key

**[Repeat]- 复读机**  

	Probability	复读几率

## Issues
由于各种原因，并没能进行完善的测试  
目前在 Win10 x64, Centos 7 x64 
环境下通过  
如使用中发现问题欢迎提 Issue

## 鸣谢
QQRobot的开发离不开以下项目：
* [DumbQQ-Core](https://github.com/rmbadmin/DumbQQ-Core)：对 SmartQQ API 的 C# 封装(dotnet core版本)
* [xiaov](https://github.com/b3log/xiaov)：一个用 Java 写的 QQ 聊天机器人 Web 服务，QQRobot的开发参考了此项目
* [图灵机器人](http://www.tuling123.com)：实现聊天智能回复功能
