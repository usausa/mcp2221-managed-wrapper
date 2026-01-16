//namespace Example;

//using MCP2221ManagedWrapper;

//internal sealed class Sht30
//{
//    private const byte Address = 0x44;

//    private readonly Mcp2221 mcp2221;

//    public Sht30(Mcp2221 mcp2221)
//    {
//        this.mcp2221 = mcp2221;
//    }

//    public bool Read(out float temperature, out float humidity)
//    {
//        // write 0x2C 0x06 (high repeatability measurement), wait 20ms, read 6 bytes
//        temperature = default;
//        humidity = default;

//        Span<byte> cmd = [0x2C, 0x06];
//        var status = mcp2221.I2cWrite(Address, true, cmd);
//        if (status != Mcp2221Status.NoError)
//        {
//            return false;
//        }

//        Thread.Sleep(20); // wait for measurement

//        // 6 bytes read: T(2) + CRC + RH(2) + CRC
//        Span<byte> data = stackalloc byte[6];
//        status = mcp2221.I2cRead(Address, true, data);
//        if (status != Mcp2221Status.NoError)
//        {
//            return false;
//        }

//        // skip CRC check

//        var rawTemperature = (ushort)((data[0] << 8) | data[1]);
//        temperature = (float)(-45.0 + (175.0 * rawTemperature / 65535.0));

//        var rawHumidity = (ushort)((data[3] << 8) | data[4]);
//        humidity = (float)(100.0 * rawHumidity / 65535.0);

//        return true;
//    }
//}
