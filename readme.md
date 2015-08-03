# OpsGenie Dotnet API Client
  
Simple C# dotnet client for OpsGenie Alert Api.
  
Including a cli client to raise, ack and resolve alerts. 
  
See OpsGenie alert api documentation:  
https://www.opsgenie.com/docs/web-api/alert-api
  
 
## 
  
 Exampl Raise, Acknowledge and Close:
 ```csharp
 
    var opsClient = new OpsGenieClient(new OpsGenieClientConfig
	{
	    ApiKey = ".. your api key",
	    ApiUrl = "https://api.opsgenie.com/v1/json/alert"
	});
	
	var response = opsClient.Raise(new Alert 
	{
	    Alias = "alert2", 
	    Source = "Test", 
	    Message = "All systems down"
	});
	
	if (response.Ok)
	{
	    var respAck =  opsClient.Acknowledge(response.AlertId, null, "Working on it!");
	
	    var respClose = opsClient.Close(response.AlertId, null, "Fixed by ..");
	}
 
 ```
 
As of v2 a custom JsonSerializer needs to be provided, this to provide flexibility in which version of which jsonserializer is used. See the cli project for usage of ServiceStack.Text v4. If you project is using any other Json serializer, Jil, NewtonSoft just hookup your version in those two methods:  
```csharp
using OpsGenieApi.Helpers;
using ServiceStack.Text;

namespace MyProject.OpsGenieImplementation
{
    public class OpsGenieSerializer : IJsonSerializer
    {
        public T DeserializeFromString<T>(string json)
        {
            //provide you deserializer
            return JsonSerializer.DeserializeFromString<T>(json);
        }

        public string SerializeToString<T>(T data)
        {
            //provide your serializer
            return JsonSerializer.SerializeToString(data);
        }
    }
    
    
}
```
Then provide it using the constructor:
```csharp
	var opsClient = new OpsGenieClient(opsConfig, new OpsGenieSerializer());
```


 
 Published the library to nuget for easy reuse:
```
Install-Package Simple.OpsGenieApi
``` 
  
## Cli interface

For integration with a monitoring tool with limited options to call external APIs I added a CLI tool. 
With this you can resolve, acknowledge and open incidents using command line. 
See OpsGeniecli project. Commandline options:  
 - c: config, config file, see format below
 - s: source, source of alert
 - m: message, message/details of incident
 - a: action, Raise, Acknowledge, Resolve
 - i: alias, important value, keeps alerts unique (so raising same alert use same value. same value can be used to close alert
 - d: description, optional description of alert
 - n: note, optional note of alert
  
Example - create new alert:  
`opsgeniecli -c opsgenie.config -a raise -s cli-test -m "Testing cli" -d "Long description" -i alert1 `
  
Example - acknowledge:  
`opsgeniecli -c opsgenie.config -a acknowledge -n " new ack note with more words" -i alert1 `
  
Example - close/resolve:  
`opsgeniecli -c opsgenie.config -a resolve -n "new resolve note with more words" -i alert1`
  
Example config file:
```
<?xml version="1.0" encoding="utf-8"?>
<OpsGenieClientConfig>
	 <ApiUrl>https://api.opsgenie.com/v1/json/alert</ApiUrl>
	 <ApiKey> .. your api key .. </ApiKey>	
</OpsGenieClientConfig>

```


## Dependencies

It has the following nuget dependencies:

- servicestack.client version v4.0.42 
- CommandLineParser  version 1.9.71
  

## TODO

- improve code, error handling, more tests
- cleanup interface 

Requests/Comments? Let me know..
