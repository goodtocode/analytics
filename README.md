# GoodToCode Analytics Library for Azure Cognitive Services
<sup>GoodToCode Analytics supports file infrastructure (Excel), AI services (Azure Cognitive Services, Text Analytics) and persistence (Azure Storage Tables, CosmosDb) for Data Lake analytics workflows.</sup> <br>

This is a simple, low-dependency library for managing Azure Cognitive Services and Text Analytics, and persisting the results to Azure Storage Tables and CosmosDb. These services rely on Azure Machine Learning and Artificial Intelligence in the [Azure Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/) suite. The APIs supported are text analytics and cognitive services, expanding to others such as computer vision, facial recognition, video indexing, etc.

#### /src Contents
Path | Item | Contents
--- | --- | ---
src | - | Contains the C# solution, project files and source code.
src | Analytics.Activities | Workflow activities to be the steps of an Durable Function Orchestration
src | Analytics.Domain | Domain Entities for this solutions services.
src | Analytics.Unit.Tests | Unit tests against fakes for cognitive services and text analytics.

#### /infrastructure ARM Templates
Path | Contents
--- | --- | ---
infrastructure | - | Contains Azure DevOps YML files, Windows PowerShell scripts, and variables to support Azure DevOps YML Pipelines.
infrastructure | *.json | ARM template for that Azure resource.
infrastructure | *.parameters.json | Parameter definition for the ARM template for that Azure resource.

#### /pipeline YML Files
Path | Item | Contents
--- | --- | ---
pipelines | - | Contains Azure DevOps YML files, Windows PowerShell scripts, and variables to support Azure DevOps YML Pipelines.
pipelines | gtc-rg-analytics-src.yml | Azure DevOps Pipeline main file.
pipelines | scripts | Command Line Interface files (.cmd) for windows/bash commands. Windows PowerShell scripts Set-Version.ps1.
pipelines | steps | Azure DevOps Pipeline step templates.
pipelines | variables | Variables (non-secret only) for the Azure landing zone, Azure infrastructure and NuGet packages.

#### Azure Cognitive Services
Cognitive Service | Purpose
:---------------------:| --- 
[Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)|Inspects each image associated with an incoming article to (1) scrape out written words from the image and (2) determine what types of objects are present in the image. 
[Face API](https://azure.microsoft.com/en-us/services/cognitive-services/face/)|Inspects each image associated with an incoming article to find faces and determine whether the face represents a male or female and associates an estimated age to those faces.
[Text Analytics](https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/) | Used to find <i>key word phrases</i> and <i>entities</i> in title and body text after it has been translated.
[Translation API](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/) | Determines the language of the incoming title and body, when present, then translates them to English. However, the target language is just another input and can be changed from English to any [supported language](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-languages) of your choice.

#### Azure Services used in GoodToCode repositories
Azure Service | Purpose
:---------------------:| --- 
[Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/)| NoSQL database where original content as well as processing results are stored.
[Azure Functions](https://azure.microsoft.com/en-us/try/app-service/)|Code blocks that analyze the documents stored in the Azure Cosmos DB.
[Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/)|Service bus queues are used as triggers for durable Azure Functions.
[Azure Storage](https://azure.microsoft.com/en-us/services/storage/)|Holds images from articles and hosts the code for the Azure Functions.

> <b> Note </b> This design uses the service collection extensions, dependency inversion, queue notification, and serverless patterns for simplicity. While these are useful patterns, this is not the only pattern that can be used to accomplish this data flow.
>
> [Azure Service Bus Topics](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions) could be used which would allow processing different parts of the article in a parallel as opposed to the serial processing done in this example. Topics would be useful if article inspection processing time is critical.  A comparison between Azure Service Bus Queues and Azure Service Bus Topics can be found [here](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions).
>
>Azure functions could also be implemented in an [Azure Logic App](https://azure.microsoft.com/en-us/services/logic-apps/).  However, with parallel processing the user would have to implement record-level locking such as [Redlock](https://redis.io/topics/distlock) until Cosmos DB supports [partial document updates](https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/6693091-be-able-to-do-partial-updates-on-document). 
>
>A comparison between durable functions and Logic apps can be found [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-compare-logic-apps-ms-flow-webjobs).

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.