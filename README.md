
# Acme.Core.Extensions
Useful extensions for .net framework types. This is a pure utility package and should be kept as simple as possible but also as powerful as possible :)

[![NuGet Package](https://img.shields.io/nuget/v/Acme.UmbracoHelpers.svg)](https://www.nuget.org/packages/Acme.UmbracoHelpers/) [![NuGet Package](https://img.shields.io/nuget/dt/Acme.UmbracoHelpers.svg)](https://www.nuget.org/packages/Acme.UmbracoHelpers/)  [![License](https://img.shields.io/badge/license-LGPL--3.0-blue.svg)](LICENSE) 

## Getting started
Install [NuGet Package](https://www.nuget.org/packages/Acme.UmbracoHelpers/) Acme.UmbracoHelpers from Package Manager or from Package Manager Console:
```
PM> Install-Package Acme.UmbracoHelpers
```

## Changes
### 1.0.0
* Create the base package

## Roadmap & Ideas
* Keep library compatible with .Net Framework and lowest Umbraco possible
* Avoid too many references to external libraries 

## Related Projects
### Acme.Web.Security.Headers
*Secure your web site/application with a simple package.*
https://github.com/olibos/Acme.Web.Security.Headers

### Acme.Core.Extensions
*Useful extensions for .net framework types. This is a pure utility package and should be kept as simple as possible but also as powerful as possible :)*
https://github.com/simonbaudart/Acme.Core.Extensions

# Documentation
All the library is self documented with XmlDoc, but here you can find some code sample.

## HtmlHelperExtensions
### GetAbsoluteUrl
Convert the relative Uri into an absolute one.
## PublishedContentExtensions
### GetFallbackPropertyValue
Get a property value, and if null, fall back to next one.
### GetTitleOrName
Get a property called title, or, if not found, get the name of the node.
## UrlHelperExtensions
### GetPathWithCrc
Get the path in parameters with some kind of CRC, if the file exists, append a version.
## UmbracoManager
Class to access the umbraco context outside of the views.