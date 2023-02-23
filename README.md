# Sample APIs for Azure API Management

This repository is a template for the [Azure Developer CLI](https://aka.ms/azure-dev/overview) (azd) that includes several different APIs:

- Star Wars API - a read-only API about the Star Wars films
  - REST (includes caching demonstration)
  - Synthetic GraphQL

- Todo List API - a read/write API for storing todo lists.
  - REST
  - GraphQL (pass-through)

Others will be added in the future.  For each backend service, the appropriate configuration is added to the Azure API Management instance to handle that API, and logging for the API is handled via Azure App Insights.

## Prerequisites

The following prerequisites are required to use this application. Please ensure that you have them all installed locally.

- [Azure Developer CLI](https://aka.ms/azd-install)
- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) - for the API backend

## Quick start

The fastest way for you to get this application up and running on Azure is to use the `azd up` command. This single command will create and configure all necessary Azure resources - including access policies and roles for your account and service-to-service communication with Managed Identities.

1. Open a terminal, create a new empty folder, and change into it.
1. Run the following command to initialize the project, provision Azure resources, and deploy the application code.

```bash
azd up --template adrianhall/api-management-sample-apis
```

You will be prompted for the following information:

- `Environment Name`: This will be used as a prefix for the resource group that will be created to hold all Azure resources. This name should be unique within your Azure subscription.
- `Azure Location`: The Azure location where your resources will be deployed.
- `Azure Subscription`: The Azure Subscription where your resources will be deployed.

> NOTE: This may take a while to complete as it executes three commands: `azd init` (initializes environment), `azd provision` (provisions Azure resources), and `azd deploy` (deploys application code). You will see a progress indicator as it provisions and deploys your application.

When `azd up` is complete it will output the URLs for each backend service. You can also access the APIs through a centralized API Management instance.

> NOTE:
>
> - The `azd up` command will create Azure resources that will incur costs to your Azure subscription. You can clean up those resources manually via the Azure portal or with the `azd down` command.
> - You can call `azd up` as many times as you like to both provision and deploy your solution, but you only need to provide the `--template` parameter the first time you call it to get the code locally. Subsequent `azd up` calls do not require the template parameter. If you do provide the parameter, all your local source code will be overwritten if you agree to overwrite when prompted.
> - You can always create a new environment with `azd env new`.

### Overriding Service Locations

While most services are available in all regions, you may need to override the location for certain services.  The following are supported:

* `appInsightsLocationName` for Application Insights.
* `staticSitesLocationName` for Static Web Apps.

To override
## Application architecture

This application utilizes the following Azure resources:

- [**Azure API Management**](https://learn.microsoft.com/azure/api-management/).
- [**Azure App Services**](https://learn.microsoft.com/azure/app-service/).
- [**Azure SQL**]https://learn.microsoft.com/azure/azure-sql/).
- [**Azure Monitor**](https://learn.microsoft.com/azure/azure-monitor/).
- [**Azure Cache for Redis**](https://learn.microsoft.com/azure/azure-cache-for-redis/).
- [**Static Web Apps**](https://learn.microsoft.com/azure/static-web-apps/).

> This template provisions resources to an Azure subscription that you will select upon provisioning them. Please refer to the [Pricing calculator for Microsoft Azure](https://azure.microsoft.com/pricing/calculator/) and, if needed, update the included Azure resource definitions found in `infra/main.bicep` to suit your needs.

### Application code

The repo is structured to follow the [Azure Developer CLI](https://aka.ms/azure-dev/overview) conventions including:

- **Source Code**: All application source code is located in the `src` folder.
- **Infrastructure as Code**: All application "infrastructure as code" files are located in the `infra` folder.
- **Azure Developer Configuration**: An `azure.yaml` file located in the root that ties the application source code to the Azure services defined in your "infrastructure as code" files.
- **VS Code Configuration**: All VS Code configuration to run and debug the application is located in the `.vscode` folder.

### Azure Subscription

This template will create infrastructure and deploy code to Azure. If you don't have an Azure Subscription, you can sign up for a [free account here](https://azure.microsoft.com/free/). Make sure you have contributor role to the Azure subscription.

### Azure Developer CLI - VS Code Extension

The Azure Developer experience includes an Azure Developer CLI VS Code Extension that mirrors all of the Azure Developer CLI commands into the `azure.yaml` context menu and command palette options. If you are a VS Code user, then we highly recommend installing this extension for the best experience.

Here's how to install it:

#### VS Code

1. Click on the "Extensions" tab in VS Code
1. Search for "Azure Developer CLI" - authored by Microsoft
1. Click "Install"

#### Marketplace

1. Go to the [Azure Developer CLI - VS Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.azure-dev) page
1. Click "Install"

Once the extension is installed, you can press `F1`, and type "Azure Developer CLI" to see all of your available options. You can also right click on your project's `azure.yaml` file for a list of commands.

### Next Steps

At this point, you have a complete application deployed on Azure. But there is much more that the Azure Developer CLI can do. These next steps will introduce you to additional commands that will make creating applications on Azure much easier. Using the Azure Developer CLI, you can setup your pipelines, monitor your application, test and debug locally.

#### Monitor the application using `azd monitor`

To help with monitoring applications, the Azure Dev CLI provides a `monitor` command to help you get to the various Application Insights dashboards.

- Run the following command to open the "Overview" dashboard:

  ```bash
  azd monitor --overview
  ```

- Live Metrics Dashboard

  Run the following command to open the "Live Metrics" dashboard:

  ```bash
  azd monitor --live
  ```

- Logs Dashboard

  Run the following command to open the "Logs" dashboard:

  ```bash
  azd monitor --logs
  ```

#### Run and Debug Locally

The easiest way to run and debug is to leverage the Azure Developer CLI Visual Studio Code Extension. Refer to this [walk-through](https://aka.ms/azure-dev/vscode) for more details.

#### Clean up resources

When you are done, you can delete all the Azure resources created with this template by running the following command:

```bash
azd down
```
