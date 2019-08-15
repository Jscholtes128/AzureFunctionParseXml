# AzureFunctionParseXml
Samples Azure Function to Parse XMl files to JSON using Blob Trigger


### Add local.settings.json for local use:

`{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<Connection String to Blob Store>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "BLOB_STORAGE": "<Connection String to Blob Store>",
    "XmlNode":"//CallResults"
  }
}`
