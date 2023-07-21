
# Installation

To install it, use proper command:

```
dotnet add package Umbrella.Infrastructure.EmailHelper 
```

[![Nuget](https://img.shields.io/nuget/v/Umbrella.Infrastructure.EmailHelper .svg?style=plastic)](https://www.nuget.org/packages/Umbrella.Infrastructure.EmailHelper /)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Umbrella.Infrastructure.EmailHelper .svg)](https://www.nuget.org/packages/Umbrella.Infrastructure.EmailHelper /)

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/Umbrella.Infrastructure/)

# usage of email sender component

to enable of email sender library, you have to follow these steps:

## Add section on AppSettings.json 
create a new section on your appSettings.json file for your application:

```json
  "Email": {
    "DefaultSenderAddress": "info@email.it",
    "DefaultSenderName": "",
    "SmtpServer": "smtp.server.com",
    "SmtpServerPort": 43,
    "SmtpUsername": "xxxx",
    "SmtpPassword": "yyyyyy",
    "TemplateFolderPath": "c:\Temp\Templates",
  }
```

## Register services into DI
register the services into DI:
```c#
builder.Services.AddEmailServices(builder.Configuration);
```