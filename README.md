# Azure Functions & ARM

This project demonstrates Azure Function (v2) and automatic deployment using Azure ARM Templates. The demonstrated functionality is a port of lbugnion's [sample-azure-coinvalue](https://github.com/lbugnion/sample-azure-coinvalue) to the current Azure Functions runtime.

## Run the demo

<!-- TODO: Add the url to the azuredeploy.json -->
[![Deploy to Azure](https://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/?repository=)

You can press the button above to deploy the ARM template in azure. Alternatively make sure that you have the Azure Functions runtime installed, as well as the Azure storage emulator. Due to the fact that Functions rely heavily on Azure Storage Accounts. *The button above only deploys the infrastructure on Azure. You will need to push the code yourself!*

<!-- TODO: Complete documentation -->

## Usefull resources

- [ARM Template Viewer](https://marketplace.visualstudio.com/items?itemName=bencoleman.armview) (for Visual Studio Code)
- [ARMVIZ visual designer](http://armviz.io/designer) (Web App)


- [ARM templates documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/)
- [ARM resources reference](https://docs.microsoft.com/en-us/azure/templates/)
- [ARM functions reference](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions)
