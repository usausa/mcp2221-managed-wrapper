# MCP2221ManagedWrapper

[![NuGet](https://img.shields.io/nuget/v/MCP2221ManagedWrapper.svg)](https://www.nuget.org/packages/MCP2221ManagedWrapper/)

## What is this?

MCP2221 unmaged wrapper for .NET applications.  
This library provides a managed interface to interact with the MCP2221 USB-to-UART/I2C converter chip from Microchip Technology.

## Usage example

### GPIO

```csharp
using MCP2221ManagedWrapper;

using var device = Mcp2221.OpenByIndex(0);

device.SetGpioDirection(PinDirection.Output, PinDirection.Output, PinDirection.Output, PinDirection.Output);

while (true)
{
    // LED blink
    device.SetGpioValues(PinValue.High, PinValue.Low, PinValue.High, PinValue.Low);
    Thread.Sleep(1000);
    device.SetGpioValues(PinValue.Low, PinValue.High, PinValue.Low, PinValue.High);
    Thread.Sleep(1000);
}
```

### I2C

```csharp
using MCP2221ManagedWrapper;

using var device = Mcp2221.OpenByIndex(0);

// Initialize
device.SetSpeed(100000);
device.SetAdvancedCommParams(timeout: 20, maxRetries: 3);

// SHT30 temperature and humidity sensor
var data = new byte[6];
while (true)
{
    device.I2cWrite(0x44, true, [0x2C, 0x06]);

    Thread.Sleep(20); // wait for measurement

    // Read: T(2) + CRC + RH(2) + CRC
    device.I2cRead(0x44, true, data);

    // Parse
    var rawTemperature = (ushort)((data[0] << 8) | data[1]);
    var temperature = (float)(-45.0 + (175.0 * rawTemperature / 65535.0));

    var rawHumidity = (ushort)((data[3] << 8) | data[4]);
    var humidity = (float)(100.0 * rawHumidity / 65535.0);

    Console.WriteLine($"Temperature={temperature}C, humidity={humidity}%");

    Thread.Sleep(1000);
}
```

## Link

https://www.microchip.com/en-us/product/mcp2221
