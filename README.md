# MiunskeBoardProject
![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![image](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)


## Table of Contents
* [Overview](#Overview)
* [How it works](#How-it-works)
* [Customize it](#Customize-it)
* [Comments and adnotations](#Comments-and-adnotations)
* [Technologies used](#Technologies-used)
* [License](#License)



## Overview

This is a program done for electricians in B.A.U.S AT company to help them debug and verify software downloaded onto CAN control boards. Often times when some error occured on CAN boards from first look you could't tell if it's a software error or that hardware has some physical flaws. <br/><br/>
This program simulates CAN boards as user specifies and in live time reacts to sent CAN messages as it was a real device. That way you can very easily tell source of problem and\or validate your code against these boards

## How it works

![GUI](https://github.com/R3VANEK/MiunskeBoardProject/blob/main/example.png)


Every board has its own pins and connectors configuration. This data is stored in appriopriate JSON files. Such files are connected to c# class that renders visual view of board. Clicking on drawing of connector you will open smaller window that have visual data of pins in it. When you connect your CAN module and click top-right button in program, every connector will begin to receive CAN signals and update its UI based on configuration file

## Customize it

You can add completely new board to better suit your needs. [Here](https://github.com/R3VANEK/MiunskeBoardProject/blob/main/instruction.odt) is documentation to do that in polish

Summary
* Create new json file in format of example file ```./connector-configurations/MiunskeG2.json```
* Copy and rename example class ```./boards/MiunskeG2.xaml``` and its ```.xaml.cs``` file
* Add in ```.xaml.cs``` path to your newly created json config file
* Head to newly created ```.xaml.cs``` file and adjust visuals using my images or create new ones
* If you don't see any error prompt from program, your board is added to menu and it is now online!


## Comments and adnotations
* Program has custom written validation and error checking for correct json config files format. It prompts you about specific lines and parts of file that are wrong
* It uses interface CANFOX and library SIECA132.DLL to communicate with USB connected CAN modules to laptop or PC with this program
* C# implementation of CAN communciation thanks to Uwe Baus B.A.U.S AT
* Although I have implemented CAN errors such as "No device connected" it would require further handling
* Due to usage of .NET program is only runnable on machines with Windows

## Technologies used
* C#
* WinForms

## License
GNU GPL
