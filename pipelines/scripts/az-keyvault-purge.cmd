ECHO Dependent on https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli

ECHO *** Auth ****
az login & az account set --subscription 7ff496d5-8747-4a50-b5a9-ed99706a855c & ECHO Authenticated

ECHO *** Key Vaults soft-deletes ***
az keyvault list-deleted
az keyvault purge --name NAME

ECHO *** Cognitive Services soft-deletes ***
az cognitiveservices account list-deleted
az cognitiveservices account purge --name NAME --location LOCATION --resource-group RESORUCE_GROUP

ECHO *** App Configuration soft-deletes ***
az appconfig list -g RESORUCE_GROUP
