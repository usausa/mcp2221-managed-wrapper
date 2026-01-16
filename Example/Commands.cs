namespace Example;

using MCP2221ManagedWrapper;

using Smart.CommandLine.Hosting;

public static class CommandBuilderExtensions
{
    public static void AddCommands(this ICommandBuilder commands)
    {
        commands.AddCommand<GpioCommand>();
        commands.AddCommand<I2cCommand>();
    }
}

//--------------------------------------------------------------------------------
// GPIO
//--------------------------------------------------------------------------------
[Command("gpio", "GPIO test")]
public sealed class GpioCommand : ICommandHandler
{
    [Option<uint>("--index", "-i", Description = "Index")]
    public uint Index { get; set; }

    [Option<int>("--loop", "-l", Description = "Loop", DefaultValue = 10)]
    public int Loop { get; set; }

    [Option<int>("--wait", "-w", Description = "Wait", DefaultValue = 1000)]
    public int Wait { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var device = Mcp2221.OpenByIndex(Index);

        var status = device.SetGpioDirection(PinDirection.Output, PinDirection.Output, PinDirection.Output, PinDirection.Output);
        if (status != Mcp2221Status.NoError)
        {
            Console.WriteLine($"SetGpioDirection failed. status=[{status}]");
            return;
        }

        for (var i = 0; i < Loop; i++)
        {
            device.SetGpioValues(PinValue.High, PinValue.Low, PinValue.High, PinValue.Low);
            await Task.Delay(Wait);
            device.SetGpioValues(PinValue.Low, PinValue.High, PinValue.Low, PinValue.High);
            await Task.Delay(Wait);
        }
    }
}

//--------------------------------------------------------------------------------
// I2C
//--------------------------------------------------------------------------------
[Command("i2c", "I2C test")]
// ReSharper disable once InconsistentNaming
public sealed class I2cCommand : ICommandHandler
{
    private const byte Address = 0x44;

    [Option<uint>("--index", "-i", Description = "Index")]
    public uint Index { get; set; }

    [Option<int>("--loop", "-l", Description = "Loop", DefaultValue = 10)]
    public int Loop { get; set; }

    [Option<int>("--wait", "-w", Description = "Wait", DefaultValue = 1000)]
    public int Wait { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        // SHT30 sensor test
        using var device = Mcp2221.OpenByIndex(Index);

        var status = device.SetSpeed(100000);
        if (status != Mcp2221Status.NoError)
        {
            Console.WriteLine($"SetSpeed failed. status=[{status}]");
            return;
        }

        status = device.SetAdvancedCommParams(timeout: 20, maxRetries: 3);
        if (status != Mcp2221Status.NoError)
        {
            Console.WriteLine($"SetAdvancedCommParams failed. status=[{status}]");
            return;
        }

        for (var i = 0; i < Loop; i++)
        {
            status = ReadValues(device, out var temperature, out var humidity);
            Console.WriteLine(status == Mcp2221Status.NoError
                ? $"Temperature={temperature}C, humidity={humidity}%"
                : $"ReadValues failed. status=[{status}]");
            await Task.Delay(Wait);
        }
    }

    private static Mcp2221Status ReadValues(Mcp2221 device, out float temperature, out float humidity)
    {
        temperature = default;
        humidity = default;

        Span<byte> cmd = [0x2C, 0x06];
        var status = device.I2cWrite(Address, true, cmd);
        if (status != Mcp2221Status.NoError)
        {
            return status;
        }

        Thread.Sleep(20); // wait for measurement

        // 6 bytes read: T(2) + CRC + RH(2) + CRC
        Span<byte> data = stackalloc byte[6];
        status = device.I2cRead(Address, true, data);
        if (status != Mcp2221Status.NoError)
        {
            return status;
        }

        // skip CRC check

        var rawTemperature = (ushort)((data[0] << 8) | data[1]);
        temperature = (float)(-45.0 + (175.0 * rawTemperature / 65535.0));

        var rawHumidity = (ushort)((data[3] << 8) | data[4]);
        humidity = (float)(100.0 * rawHumidity / 65535.0);

        return Mcp2221Status.NoError;
    }
}
