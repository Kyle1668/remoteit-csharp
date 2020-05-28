# remoteit-csharp

[![NuGet](https://img.shields.io/nuget/v/Remoteit.svg?style=flat)](https://www.nuget.org/packages/Remoteit/1.0.2)
[![Build Status](https://dev.azure.com/kyledevinobrien/remoteit-csharp/_apis/build/status/Kyle1668.remoteit-csharp?branchName=master)](https://dev.azure.com/kyledevinobrien/remoteit-csharp/_build/latest?definitionId=8&branchName=master)

remoteit-csharp is an open-source C# client for the [remote.it REST API](https://docs.remote.it/api-reference/overview). With this package, you can programmatically list and securely connect to your remote.it devices. This client is distributed as a NuGet package targeting the .NET Core framework.

Anyone is welcome to contribute to this project. Feel free to make any issues and pull-requests.

## Installation

### Using the .NET Core Command Line Tools

If you are building with the .NET Core command-line tools, you can run the following command from within your project directory:

`dotnet add package Remoteit`

### Using Visual Studio IDE

From within Visual Studio, you can use the NuGet GUI to search for and install the Remoteit NuGet package. Alternatively, you can use the built-in package manager console and write the following command.

`Install-Package Remoteit`

## Usage

### Authentication

```csharp
var remoteitUsername = Environment.GetEnvironmentVariable("REMOTEIT_USERNAME");
var remoteitPassword = Environment.GetEnvironmentVariable("REMOTEIT_PASSWORD");
var remoteitDevKey = Environment.GetEnvironmentVariable("REMOTEIT_DEVELOPER_KEY");

var remoteitClient = new RemoteitClient(remoteitUsername, remoteitPassword, remoteitDevKey);
```

After authenticating, you can list your devices, connect to them, and terminate device connections.

### Listing Your Devices

```csharp
var remoteitClient = new RemoteitClient(remoteitUsername, remoteitPassword, remoteitDevKey);

List<RemoteitDevice> devices = await remoteitClient.GetDevices();
```

### Connecting to a Device

```csharp
var remoteitClient = new RemoteitClient(remoteitUsername, remoteitPassword, remoteitDevKey);

var deviceAddress = "80:00:00:00:01:XX:XX:XX";

// Create a device connection
ServiceConnection connectionData = await remoteitClient.ConnectToService(deviceAddress);

// Example: https://XXXasnap.p18.rt3.io/
string connectionUrl = connectionData.Proxy;
```

### Terminating a Device Connection

```csharp
var remoteitClient = new RemoteitClient(remoteitUsername, remoteitPassword, remoteitDevKey);

string deviceAddress = "80:00:00:00:01:XX:XX:XX";

// Create a device connection
ServiceConnection connectionData = await remoteitClient.ConnectToService(deviceAddress);

// Get the connection's ID from the service connection object.
string connectionId = connectionData.ConnectionId;

// Terminate the connection. An exception is thrown if the termination is unsucesful.
await remoteitClient.TerminateDeviceConnection(deviceAddress, connectionId);
```

## Development

### Dependencies

You can install the project's dependencies by running `dotnet restore` in the project root.

### Testing

All tests are located in the `Remoteit.Test` project. The testing libraries used for this project are [xUnit](https://xunit.net/) and [Moq](https://www.nuget.org/packages/Moq).

Continues integration is done through Azure Pipelines. The `azure-pipelines.yml` file configures this pipeline. [Pipeline Link](https://dev.azure.com/kyledevinobrien/remoteit-csharp/_build?definitionId=8&_a=summary)

## Authors

* **Kyle O'Brien** - *Initial work* - [LinkedIn](https://www.linkedin.com/in/kyle1668/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
