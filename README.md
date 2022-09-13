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



## Overivew

This is a program done for electricians in B.A.U.S AT company to help them debug and verify software downloaded onto CAN control boards. It simulates any of these boards 
and provide nice graphical way of viewing how CAN message is affecting it

## How it works

Every board has its own pins and connectors configuration. This data is stored in appriopriate json files. Such files are connected to c# class that renders visual view of board.
Clicking on drawing of connector you will open smaller window that have visual data of pins in it. When you connect your CAN module and click top-right button in program, every connector
will begin to receive CAN signals and update its UI based on configuration file

## Customize it

You can add completely new board to better suit your needs. Here are steps to do that: <br/>

Open this project in Visual Studio 2019 and copy paste ```./connector-configurations/MiunskeG2.json```. This is example file, rename it as you like.

