# SocketServer
[![Build status](https://ci.appveyor.com/api/projects/status/h5d2cwjswi81iogh/branch/master?svg=true)](https://ci.appveyor.com/project/nwestfall/socketserver/branch/master)

Simple .NET Core console application to test receiving data via a socket connection

## Running the application

To run the application, either download the solution and build on your machine or click the build icon above and in the "artifacts" are a Windows and Mac executable.

#### To Run on Windows
 - Open Command Prompt
 - Navigate to folder containing executable
 - type ./SocketClient.exe

#### To Run on Mac
 - Open Terminal
 - Navigate to folder containing executable
 - type ./SocketClient

#### Once running, you will have to enter the following information
 - IP Address to run Socket Server on
 - Port to run Socket Server on

You can stop sending data any time by clicking any key
NOTE: Socket Server will only work with 1 Connected Client at a time
