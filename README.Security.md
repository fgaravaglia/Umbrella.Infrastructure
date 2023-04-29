
# Installation

To install it, use proper command:

```
dotnet add package Umbrella.Security 
```

[![Nuget](https://img.shields.io/nuget/v/Umbrella.Security.svg?style=plastic)](https://www.nuget.org/packages/Umbrella.Security/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Umbrella.Security.svg)](https://www.nuget.org/packages/Umbrella.Security/)

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/Umbrella.Security/)

# Usage of Decorators

to decorate a specific method of a class (example: API controller) to require a claim to be executed, follow the snippet below:

```c#
        /// <summary>
        /// Reders the index view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ClaimsAuthorize(ClaimTypes.RANKING, "R")]
        public IActionResult Index()
        {
          .....
        }
```
