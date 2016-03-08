# Fat Zebra .NET Library v2.0.5

## Support for TLS 1.1 and TLS 1.2
In order to support the updated gateway endpoint configuration for the retirement of TLS 1.0 the following changes may
be required for your application:

* Update to .NET 4.5 or above
* Add the following code prior to making a call into the Gateway:

```
System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
```

___
Currently there is support for:

* Purchase with Card Details
* Purchase with Token
* Tokenize Card
* Refund Purchase
* Subscriptions
* Customers

### Dependencies
* System.Json
* Visual Studio test tools for unit testing

## Basic Usage

1. Add a reference to the library.
2. Configure the Gateway:
```c#
Gateway.Username = "TEST";
Gateway.Token = "TEST";
Gateway.SandboxMode = true;
Gateway.TestMode = true;
```
3. Purchase, Refund, Tokenize etc

```c#
// Create a purchase
var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime(2012, 05, 31), "123", invoice.record_number, Request.UserHostAddress);

if (response.Successful && response.Result.Successful) 
{
	// Your logic here
}
else
{
	// Do something with the errors
	Response.Write(response.Errors[0]);
}

```

## Notes

Contributors, code reviewers and more are welcome - as I mentioned, this isn't my best code, but it is
functional and passes the basic tests. Please feel free to contact me for more info at matthew.savage@fatzebra.com.au

Eventually once the library is of better quality this will be added to NuGet.
