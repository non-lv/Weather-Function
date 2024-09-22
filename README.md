# Weather Function
A Functions app that automatically stores weather for a specific location and provides Functions API to retrieve past weather

## How It's Made:

**Tech used:** .Net Core 6, C#, Azure Functions, Azure Blobs Storage, Azure Table Storage

A timed function periodically runs to check for new Weather updates from OpenWeather and stores the data in Azure Storage.
Past logs of the retrieval attemps are stored in a tablea and can be retrieved.

## Function endpoints:
* /api/GetWeather/{city}/{blobName}
  * Retrieves a specific Weather Record as Json
* /api/GetWeatherLogs?from=yyMMdd-HH:mm&to=yyMMdd-HH:mm
  * Retrieves all stored Weather Logs from passed in time period as Json
