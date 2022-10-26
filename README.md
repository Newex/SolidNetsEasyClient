# What is it?
This is a client for making type safe requests to the [nets easy API endpoint](https://developers.nets.eu/nets-easy/en-EU/api/payment-v1/).

### Status
Current development status is: under development, non functional library.

# Quickstart (TBD)

Add the package to your project:  
```
$ `dotnet add package [PACKAGE_NAME]`
```

Register the service in the startup process:

```csharp
// Register TUS services
builder.Services.AddNetsEasyClient();
```

# Features (TBD)
Use the client to make and manage payments, from the backend. Remember that the you still need a frontend for a customer to input payment details.


# Roadmap
* Handle payments, subscriptions and webhooks in a type safe and easy to use way.
* Create nuget package
* Add unit tests
* Add easy to use configuration for handling API keys and other client settings
* Add example site