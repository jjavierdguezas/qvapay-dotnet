# qvapay-dotnet

NuGet package with a QvaPay client for dotnet developers.

## Configuring

Having a `JSON` configuration file like:

```json
{
  [...]

  "QvaPayConfiguration": 
  {
    "AppId": "<APP_ID>",
    "AppSecret": "<APP_SECRET>"
  }

  [...]
}
```

You can add `QvaPayClient` to the DI just using

```csharp
services.AddQvaPayClient();
```

You can change the root key in your config file like:

```json
{
  [...]

  "QvaPay": // <--------------- this changed
  { 
    "AppId": "<APP_ID>",
    "AppSecret": "<APP_SECRET>"
  }

  [...]
}
```

Then you can do:

```csharp
services.AddQvaPayClient((config) => {
    // This will search for "QvaPay:AppId" and "QvaPay:AppSecret" in the config
    config.AppConfigJsonPrefix = "QvaPay";   
});
```

Also you can manually set the info without a config file:

```csharp
services.AddQvaPayClient((config) => {
    config.AppId = "<APP_ID>";
    config.AppSecret = "<APP_SECRET>";
});
```

## aspnet core

Just add your app's configuration from QvaPay to the `appsettings.json`

In the `Startup.cs` use the `AddQvaPayClient(...)` method in the `ConfigureServices` section

```csharp
[...]
using QvaPay.Sdk;

namespace MyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) [...]

        public void ConfigureServices(IServiceCollection services)
        {
            [...]
            services.AddQvaPayClient();
            [...]
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) [...]
    }
}
```

### Define QvaPay callback handler

To define a handler for the request made from QvaPay when an invoice is processed, you need to follow two steps. First, you must implement and register in the DI an `IQvaPayCallbackHander`. Then you can use the shorthand:

```csharp
[...]
using QvaPay.Sdk;
using QvaPay.Sdk.Callback;

namespace MyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) [...]

        public void ConfigureServices(IServiceCollection services)
        {
            [...]
            services.AddQvaPayClient();

            // QvaPayCallbackHander.HandleCallback(invoiceId, remoteId) will be called on QvaPay callback request
            services.AddSingleton<IQvaPayCallbackHander, QvaPayCallbackHander>();
            [...]
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          [...]
          app.UseEndpoints(endpoints =>
          {
              // The webhook defined in QvaPay website looks like: `https://YOURHOST:PORT/qvapay-callback`
              endpoints.MapQvaPayCallback("qvapay-callback");
              endpoints.MapControllers();
          });
          [...]
        }
    }
}
```

handler:

```csharp
[...]
using QvaPay.Sdk.Callback;

namespace MyWebApp
{
    public class QvaPayCallbackHander : IQvaPayCallbackHander
    {
        public Task<QvaPayCallbackResponse> HandleCallback(Guid invoiceId, string remoteId = null)
        {
          // check info validity
          // if (!IsValid(invoiceId, remoteId))
          //     return Task.FromResult(new QvaPayCallbackResponse
          //     {
          //         StatusCode = HttpStatusCode.BadRequest,
          //         Message = "Invalid Info"
          //     });

          // process invoice
          [...]
          
          return Task.FromResult(new QvaPayCallbackResponse
          {
              StatusCode = HttpStatusCode.OK,
              Message = "Received"
          });
        }
    }
}
```

## Using the client

After adding the client to your DI just pass `IQvaPayClient` in your constructor

```csharp
using QvaPay.Sdk;

namespace MyWebApp
{
    public class QvaPayPaymentService 
    {
        private readonly IQvaPayClient _qvaPayClient;

        public QvaPayPaymentService(IQvaPayClient qvaPayClient) {
            _qvaPayClient = qvaPayClient;
        }

        public async Task<double> GetBalance() {
            var qvaPayResponse = await _qvaPayClient.GetBalance();

            // if (!qvaPayResponse.Success)
            //     throw new Exception(qvaPayResponse.Message)

            return qvaPayResponse.Data;
        }
    }
}
```
