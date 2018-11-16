# Payabbhi .NET Library

Make sure you have signed up for your [Payabbhi Account](https://payabbhi.com/docs/account) and downloaded the [API keys](https://payabbhi.com/docs/account/#api-keys) from the [Portal](https://payabbhi.com/portal).

## Requirements

.NET 4.0 and later.

## Installation
### Install via NuGet

From the command line:

```
$ nuget install Payabbhi

```

From Package Manager:

```
PM> Install-Package Payabbhi
```

From within Visual Studio:

  1. Open the Solution Explorer.
  2. Right-click on a project within your solution.
  3. Click on `Manage NuGet Packages`.
  4. Click on the `Browse` tab and search for `Payabbhi`.
  5. Click on the `Payabbhi` package and click `Install`.

## Documentation

Please refer to:
- [.NET API Documentation](https://payabbhi.com/docs/api/?csharp)
- [Integration Guide](https://payabbhi.com/docs/integration)


## Usage
A typical usage of the Payabbhi .NET Library is shown below:


```csharp
using Payabbhi;

// Set your credentials
Client client = new Client('<accessId>', '<secretKey>');

// Create an order
Order order = client.Order.Create(
                new Dictionary<string, object>() {
                  {"merchant_order_id", '<merchantOrderId>'},
                  {"amount", 100},
                  {"currency", "INR"},
                  {"payment_auto_capture", false}
                });


```
For more examples see the [.NET API documentation](https://payabbhi.com/docs/api/?csharp)


### Verifying payment signature
Payabbhi .NET library provides utility functions for verifying the payment signature received in the payment callback. The snippet below demonstrates a typical usage:

```csharp
client.Utility.VerifyPaymentSignature(
  new Dictionary<string, string>() {
    {"payment_signature", '<paymentSignature>'},
    {"order_id", '<orderId>'},
    {"payment_id", '<paymentId>'}
  });
```

### Verifying webhook signature
Payabbhi .NET library provides utility functions for verifying the webhook signature. The snippet below demonstrates a typical usage:

```csharp
client.Utility.VerifyWebhookSignature(payload,actualSignature,secret);

// replayInterval is optional
client.Utility.VerifyWebhookSignature(payload,actualSignature,secret,replayInterval);
```
