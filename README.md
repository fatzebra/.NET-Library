# Fat Zebra .NET Library v1.0.0

It's been a while since I've written in C#, so please excuse my code if it seems lacking in quality.

Currently there is support for:

* Purchase with Card Details
* Purchase with Token
* Tokenize Card
* Refund Purchase

### Dependencies
* System.Json
* Visual Studio test tools for unit testing

## Basic Usage

1. Add a reference to the library.
2. Create a new instance of the Gateway
3. Purchase, Refund, Tokenize etc

```
var gw = new FatZebra.Gateway("username", "token");

// if you are using the sandbox, set sandbox = true
gw.SandboxMode = true;

// if you are using test mode in the live env, set live = true
gw.TestMode = true;

// Finally, create a purchase
var response = gw.Purchase(120, "M Smith", "5123456789012346", DateTime(2012, 05, 31), "123", invoice.record_number, Request.UserHostAddress);

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