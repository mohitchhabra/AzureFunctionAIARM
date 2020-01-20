# Prerequisites

This document outlines the steps necessary to have a development environment capable of running the demos and `Azure Functions` & `ARM Templates` in general.

## .Net Core SDK

*Important: This step is only required if your are planing to only use `Visual Studio Code` as your development environnement and not yet have `Visual Studio` installed*

1. Download the current version of the .Net Core SDK [here](https://dotnet.microsoft.com/download)
   - Make sure to choose `Download .Net Core SDK`
2. Run the installer and follow the instructions

## Azure Functions Core Tools

*Important: This step is only required if your are planing to only use `Visual Studio Code` as your development environnement and not yet have `Visual Studio` installed*

1. Download the current version of Node.JS from [here](https://nodejs.org/en/)
   - If you are unsure whether to choose `current` or `LTS`, choose `LTS` as its supported for a longer period of time and will work the same.
2. Run the installer and follow the instructions
3. Reboot your machine (optional)
4. Open up a PowerShell window
5. Run `npm i -g azure-functions-core-tools --unsafe-perm true` to install the `Azure Functions Core Tools` globally
6. Re-open PowerShell and try the `func --version` command
   - If it prints a version number to the console, you are done. Otherwise retry the installation process, since there must have been an issue. Also check your PATH variables!

## Visual Studio Code

1. Download `Visual Studio Code` from [here](https://visualstudio.microsoft.com/de/?rr=https%3A%2F%2Fduckduckgo.com%2F)
2. Once installed head to the `Extensions` tab (ctrl+shift+x)
3. Search for the following extensions and install them
   - [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account) (required)
   - [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) (required)
   - [Azure Storage](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurestorage) (optional)
   - [Azure Application Insights](https://marketplace.visualstudio.com/items?itemName=visualstudioonlineapplicationinsights.application-insights) (optional)
   - [ARM Tools](https://marketplace.visualstudio.com/items?itemName=msazurermtools.azurerm-vscode-tools) (required)
   - [ARM Template Viewer](https://marketplace.visualstudio.com/items?itemName=bencoleman.armview) (optional)

## Visual Studio 2019

1. Download `Visual Studio 2019` from [here](https://visualstudio.microsoft.com/de/?rr=https%3A%2F%2Fduckduckgo.com%2F)
   - If you do not have a Visual Studio subscription choose `Community Edition` - It comes with all the necessary tools for developing Azure Functions.
2. In the installer make sure to check `ASP.NET and web development`, as well as `Azure development`
3. Hit `Install` to start the installation

![Visual Studio 2019 installer](./images/vs-installer.jpg)
