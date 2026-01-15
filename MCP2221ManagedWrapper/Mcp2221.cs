namespace MCP2221ManagedWrapper;

using static MCP2221ManagedWrapper.NativeMethods;

public enum PinMode
{
    Output = MCP2221_GPDIR_OUTPUT,
    Input = MCP2221_GPDIR_INPUT
}

public enum PinValue
{
    Low = MCP2221_GPVAL_LOW,
    High = MCP2221_GPVAL_HIGH
}

// ReSharper disable InconsistentNaming
public sealed unsafe class Mcp2221 : IDisposable
{
    public const uint DefaultVid = 0x04D8;
    public const uint DefaultPid = 0x00DD;

    public IntPtr Handle { get; private set; }

    public bool IsOpen => Handle != IntPtr.Zero;

    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------

    private Mcp2221(IntPtr handle)
    {
        Handle = handle;
    }

    public void Dispose()
    {
        if (IsOpen)
        {
            Close();
        }
    }

    //------------------------------------------------------------------------
    // Open/Close
    //------------------------------------------------------------------------

    public static Mcp2221 OpenByIndex(uint index, uint vid = DefaultVid, uint pid = DefaultPid)
    {
        return new Mcp2221(Mcp2221_OpenByIndex(vid, pid, index));
    }

    public static Mcp2221 OpenBySerialNumber(string serialNumber, uint vid = DefaultVid, uint pid = DefaultPid)
    {
        return new Mcp2221(Mcp2221_OpenBySN(vid, pid, serialNumber));
    }

    public void Close()
    {
        if (IsOpen)
        {
            _ = Mcp2221_Close(Handle);
            Handle = IntPtr.Zero;
        }
    }

    public Mcp2221Status Reset()
        => (Mcp2221Status)Mcp2221_Reset(Handle);

    public static Mcp2221Status CloseAll()
        => (Mcp2221Status)Mcp2221_CloseAll();

    //------------------------------------------------------------------------
    // Library/Enumeration
    //------------------------------------------------------------------------

    public static Mcp2221Status GetConnectedDevices(uint vid, uint pid, out uint count)
        => (Mcp2221Status)Mcp2221_GetConnectedDevices(vid, pid, out count);

    public static Mcp2221Status GetConnectedDevices(out uint count)
        => (Mcp2221Status)Mcp2221_GetConnectedDevices(DefaultVid, DefaultPid, out count);

    public static Mcp2221Status GetLibraryVersion(out string version)
    {
        Span<char> buf = stackalloc char[16];
        buf.Clear();

        fixed (char* p = buf)
        {
            var status = (Mcp2221Status)Mcp2221_GetLibraryVersion(p);
            version = status == Mcp2221Status.NoError
                ? CreateStringFromNullTerminated(p, buf.Length)
                : string.Empty;
            return status;
        }
    }

    //------------------------------------------------------------------------
    // USB descriptors
    //------------------------------------------------------------------------

    public Mcp2221Status GetManufacturerDescriptor(out string manufacturer)
    {
        Span<char> buf = stackalloc char[32];
        buf.Clear();

        fixed (char* p = buf)
        {
            var status = (Mcp2221Status)Mcp2221_GetManufacturerDescriptor(Handle, p);
            manufacturer = status == Mcp2221Status.NoError
                ? CreateStringFromNullTerminated(p, buf.Length)
                : string.Empty;
            return status;
        }
    }

    public Mcp2221Status SetManufacturerDescriptor(string manufacturer)
        => (Mcp2221Status)Mcp2221_SetManufacturerDescriptor(Handle, manufacturer);

    public Mcp2221Status GetProductDescriptor(out string product)
    {
        Span<char> buf = stackalloc char[32];
        buf.Clear();

        fixed (char* p = buf)
        {
            var status = (Mcp2221Status)Mcp2221_GetProductDescriptor(Handle, p);
            product = status == Mcp2221Status.NoError
                ? CreateStringFromNullTerminated(p, buf.Length)
                : string.Empty;
            return status;
        }
    }

    public Mcp2221Status SetProductDescriptor(string product)
        => (Mcp2221Status)Mcp2221_SetProductDescriptor(Handle, product);

    public Mcp2221Status GetSerialNumberDescriptor(out string serial)
    {
        Span<char> buf = stackalloc char[32];
        buf.Clear();

        fixed (char* p = buf)
        {
            var status = (Mcp2221Status)Mcp2221_GetSerialNumberDescriptor(Handle, p);
            serial = status == Mcp2221Status.NoError
                ? CreateStringFromNullTerminated(p, buf.Length)
                : string.Empty;
            return status;
        }
    }

    public Mcp2221Status SetSerialNumberDescriptor(string serial)
        => (Mcp2221Status)Mcp2221_SetSerialNumberDescriptor(Handle, serial);

    public Mcp2221Status GetFactorySerialNumber(out string serial)
    {
        Span<char> buf = stackalloc char[32];
        buf.Clear();

        fixed (char* p = buf)
        {
            var status = (Mcp2221Status)Mcp2221_GetFactorySerialNumber(Handle, p);
            serial = status == Mcp2221Status.NoError
                ? CreateStringFromNullTerminated(p, buf.Length)
                : string.Empty;
            return status;
        }
    }

    public Mcp2221Status GetHwFwRevisions(out string hardwareRevision, out string firmwareRevision)
    {
        Span<char> hw = stackalloc char[64];
        Span<char> fw = stackalloc char[64];
        hw.Clear();
        fw.Clear();

        fixed (char* pHw = hw)
        fixed (char* pFw = fw)
        {
            var status = (Mcp2221Status)Mcp2221_GetHwFwRevisions(Handle, pHw, pFw);
            if (status == Mcp2221Status.NoError)
            {
                hardwareRevision = CreateStringFromNullTerminated(pHw, hw.Length);
                firmwareRevision = CreateStringFromNullTerminated(pFw, fw.Length);
            }
            else
            {
                hardwareRevision = string.Empty;
                firmwareRevision = string.Empty;
            }
            return status;
        }
    }

    //------------------------------------------------------------------------
    // USB attributes
    //------------------------------------------------------------------------

    public Mcp2221Status GetVidPid(out uint vid, out uint pid)
        => (Mcp2221Status)Mcp2221_GetVidPid(Handle, out vid, out pid);

    public Mcp2221Status SetVidPid(uint vid, uint pid)
        => (Mcp2221Status)Mcp2221_SetVidPid(Handle, vid, pid);

    public Mcp2221Status GetUsbPowerAttributes(out byte powerAttributes, out uint currentReq)
        => (Mcp2221Status)Mcp2221_GetUsbPowerAttributes(Handle, out powerAttributes, out currentReq);

    public Mcp2221Status SetUsbPowerAttributes(byte powerAttributes, uint currentReq)
        => (Mcp2221Status)Mcp2221_SetUsbPowerAttributes(Handle, powerAttributes, currentReq);

    public Mcp2221Status GetSerialNumberEnumerationEnable(out byte enabled)
        => (Mcp2221Status)Mcp2221_GetSerialNumberEnumerationEnable(Handle, out enabled);

    public Mcp2221Status SetSerialNumberEnumerationEnable(byte enabled)
        => (Mcp2221Status)Mcp2221_SetSerialNumberEnumerationEnable(Handle, enabled);

    //------------------------------------------------------------------------
    // I2C
    //------------------------------------------------------------------------

    public Mcp2221Status I2cCancelCurrentTransfer()
        => (Mcp2221Status)Mcp2221_I2cCancelCurrentTransfer(Handle);

    public Mcp2221Status SetAdvancedCommParams(byte timeout, byte maxRetries)
        => (Mcp2221Status)Mcp2221_SetAdvancedCommParams(Handle, timeout, maxRetries);

    public Mcp2221Status SetSpeed(uint speed)
        => (Mcp2221Status)Mcp2221_SetSpeed(Handle, speed);

    public Mcp2221Status I2cRead(byte slaveAddress, bool use7BitAddress, Span<byte> rx)
    {
        fixed (byte* p = rx)
        {
            return (Mcp2221Status)Mcp2221_I2cRead(
                Handle,
                (uint)rx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    public Mcp2221Status I2cWrite(byte slaveAddress, bool use7BitAddress, ReadOnlySpan<byte> tx)
    {
        fixed (byte* p = tx)
        {
            return (Mcp2221Status)Mcp2221_I2cWrite(
                Handle,
                (uint)tx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    public Mcp2221Status I2cWriteNoStop(byte slaveAddress, bool use7BitAddress, ReadOnlySpan<byte> tx)
    {
        fixed (byte* p = tx)
        {
            return (Mcp2221Status)Mcp2221_I2cWriteNoStop(
                Handle,
                (uint)tx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    public Mcp2221Status I2cReadRestart(byte slaveAddress, bool use7BitAddress, Span<byte> rx)
    {
        fixed (byte* p = rx)
        {
            return (Mcp2221Status)Mcp2221_I2cReadRestart(
                Handle,
                (uint)rx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    public Mcp2221Status I2cWriteRestart(byte slaveAddress, bool use7BitAddress, ReadOnlySpan<byte> tx)
    {
        fixed (byte* p = tx)
        {
            return (Mcp2221Status)Mcp2221_I2cWriteRestart(
                Handle,
                (uint)tx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    //------------------------------------------------------------------------
    // SMBus
    //------------------------------------------------------------------------

    public Mcp2221Status SmbusWriteByte(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, byte data)
    {
        return (Mcp2221Status)Mcp2221_SmbusWriteByte(
            Handle,
            slaveAddress,
            use7BitAddress ? (byte)1 : (byte)0,
            usePec ? (byte)1 : (byte)0,
            command,
            data);
    }

    public Mcp2221Status SmbusReadByte(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, out byte readByte)
    {
        fixed (byte* p = &readByte)
        {
            return (Mcp2221Status)Mcp2221_SmbusReadByte(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                p);
        }
    }

    public Mcp2221Status SmbusWriteWord(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, ReadOnlySpan<byte> data2Bytes)
    {
        fixed (byte* p = data2Bytes)
        {
            return (Mcp2221Status)Mcp2221_SmbusWriteWord(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                p);
        }
    }

    public Mcp2221Status SmbusReadWord(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, Span<byte> read2Bytes)
    {
        fixed (byte* p = read2Bytes)
        {
            return (Mcp2221Status)Mcp2221_SmbusReadWord(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                p);
        }
    }

    public Mcp2221Status SmbusBlockWrite(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, ReadOnlySpan<byte> data)
    {
        fixed (byte* p = data)
        {
            return (Mcp2221Status)Mcp2221_SmbusBlockWrite(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                (byte)data.Length,
                p);
        }
    }

    public Mcp2221Status SmbusBlockRead(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, Span<byte> readData)
    {
        fixed (byte* p = readData)
        {
            return (Mcp2221Status)Mcp2221_SmbusBlockRead(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                (byte)readData.Length,
                p);
        }
    }

    public Mcp2221Status SmbusBlockWriteBlockReadProcessCall(
        byte slaveAddress,
        bool use7BitAddress,
        bool usePec,
        byte command,
        ReadOnlySpan<byte> writeData,
        Span<byte> readData)
    {
        fixed (byte* pW = writeData)
        fixed (byte* pR = readData)
        {
            return (Mcp2221Status)Mcp2221_SmbusBlockWriteBlockReadProcessCall(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                command,
                (byte)writeData.Length,
                pW,
                (byte)readData.Length,
                pR);
        }
    }

    public Mcp2221Status SmbusSendByte(byte slaveAddress, bool use7BitAddress, bool usePec, byte data)
    {
        return (Mcp2221Status)Mcp2221_SmbusSendByte(
            Handle,
            slaveAddress,
            use7BitAddress ? (byte)1 : (byte)0,
            usePec ? (byte)1 : (byte)0,
            data);
    }

    public Mcp2221Status SmbusReceiveByte(byte slaveAddress, bool use7BitAddress, bool usePec, out byte readByte)
    {
        fixed (byte* p = &readByte)
        {
            return (Mcp2221Status)Mcp2221_SmbusReceiveByte(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                p);
        }
    }

    //------------------------------------------------------------------------
    // GPIO
    //------------------------------------------------------------------------

    public Mcp2221Status GetInitialPinValues(out byte ledUartRx, out byte ledUartTx, out byte ledI2c, out byte sspnd, out byte usbCfg)
        => (Mcp2221Status)Mcp2221_GetInitialPinValues(Handle, out ledUartRx, out ledUartTx, out ledI2c, out sspnd, out usbCfg);

    public Mcp2221Status SetInitialPinValues(byte ledUartRx, byte ledUartTx, byte ledI2c, byte sspnd, byte usbCfg)
        => (Mcp2221Status)Mcp2221_SetInitialPinValues(Handle, ledUartRx, ledUartTx, ledI2c, sspnd, usbCfg);

    // TODO Enum
    public Mcp2221Status GetInterruptEdgeSetting(byte whichToGet, out byte interruptPinMode)
        => (Mcp2221Status)Mcp2221_GetInterruptEdgeSetting(Handle, whichToGet, out interruptPinMode);

    public Mcp2221Status SetInterruptEdgeSetting(byte whichToSet, byte interruptPinMode)
        => (Mcp2221Status)Mcp2221_SetInterruptEdgeSetting(Handle, whichToSet, interruptPinMode);

    // TODO check
    public Mcp2221Status ClearInterruptPinFlag()
        => (Mcp2221Status)Mcp2221_ClearInterruptPinFlag(Handle);

    public Mcp2221Status GetInterruptPinFlag(out byte flagValue)
        => (Mcp2221Status)Mcp2221_GetInterruptPinFlag(Handle, out flagValue);

    // TODO Enum
    public Mcp2221Status GetClockSettings(byte whichToGet, out byte dutyCycle, out byte clockDivider)
        => (Mcp2221Status)Mcp2221_GetClockSettings(Handle, whichToGet, out dutyCycle, out clockDivider);

    public Mcp2221Status SetClockSettings(byte whichToSet, byte dutyCycle, byte clockDivider)
        => (Mcp2221Status)Mcp2221_SetClockSettings(Handle, whichToSet, dutyCycle, clockDivider);

    // TODO check
    public Mcp2221Status GetAdcData(Span<uint> adcDataArray)
    {
        fixed (uint* p = adcDataArray)
        {
            return (Mcp2221Status)Mcp2221_GetAdcData(Handle, p);
        }
    }

    // TODO check
    public Mcp2221Status GetAdcVref(byte whichToGet, out byte adcVref)
        => (Mcp2221Status)Mcp2221_GetAdcVref(Handle, whichToGet, out adcVref);

    public Mcp2221Status SetAdcVref(byte whichToSet, byte adcVref)
        => (Mcp2221Status)Mcp2221_SetAdcVref(Handle, whichToSet, adcVref);

    // TODO check
    public Mcp2221Status GetDacVref(byte whichToGet, out byte dacVref)
        => (Mcp2221Status)Mcp2221_GetDacVref(Handle, whichToGet, out dacVref);

    public Mcp2221Status SetDacVref(byte whichToSet, byte dacVref)
        => (Mcp2221Status)Mcp2221_SetDacVref(Handle, whichToSet, dacVref);

    // TODO check
    public Mcp2221Status GetDacValue(byte whichToGet, out byte dacValue)
        => (Mcp2221Status)Mcp2221_GetDacValue(Handle, whichToGet, out dacValue);

    public Mcp2221Status SetDacValue(byte whichToSet, byte dacValue)
        => (Mcp2221Status)Mcp2221_SetDacValue(Handle, whichToSet, dacValue);

    // TODO check
    public Mcp2221Status GetGpioSettings(byte whichToGet, Span<byte> pinFunctions, Span<byte> pinDirections, Span<byte> outputValues)
    {
        fixed (byte* pF = pinFunctions)
        fixed (byte* pD = pinDirections)
        fixed (byte* pO = outputValues)
        {
            return (Mcp2221Status)Mcp2221_GetGpioSettings(Handle, whichToGet, pF, pD, pO);
        }
    }

    // TODO check
    public Mcp2221Status SetGpioSettings(byte whichToSet, ReadOnlySpan<byte> pinFunctions, ReadOnlySpan<byte> pinDirections, ReadOnlySpan<byte> outputValues)
    {
        fixed (byte* pF = pinFunctions)
        fixed (byte* pD = pinDirections)
        fixed (byte* pO = outputValues)
        {
            return (Mcp2221Status)Mcp2221_SetGpioSettings(Handle, whichToSet, pF, pD, pO);
        }
    }

    // TODO Delete
    public Mcp2221Status GetGpioValues(Span<byte> gpioValues)
    {
        fixed (byte* p = gpioValues)
        {
            return (Mcp2221Status)Mcp2221_GetGpioValues(Handle, p);
        }
    }

    public Mcp2221Status GetGpioValues(out PinValue value0, out PinValue value1, out PinValue value2, out PinValue value3)
    {
        var buffer = stackalloc byte[4];
        var status = (Mcp2221Status)Mcp2221_GetGpioValues(Handle, buffer);
        value0 = (PinValue)buffer[0];
        value1 = (PinValue)buffer[1];
        value2 = (PinValue)buffer[2];
        value3 = (PinValue)buffer[3];
        return status;
    }

    // TODO Delete
    public Mcp2221Status SetGpioValues(ReadOnlySpan<byte> gpioValues)
    {
        fixed (byte* p = gpioValues)
        {
            return (Mcp2221Status)Mcp2221_SetGpioValues(Handle, p);
        }
    }

    public Mcp2221Status SetGpioValues(PinValue value0, PinValue value1, PinValue value2, PinValue value3)
    {
        var buffer = stackalloc byte[4];
        buffer[0] = (byte)value0;
        buffer[1] = (byte)value1;
        buffer[2] = (byte)value2;
        buffer[3] = (byte)value3;
        return (Mcp2221Status)Mcp2221_SetGpioValues(Handle, buffer);
    }

    public Mcp2221Status SetGpioValue(int pin, PinValue value)
    {
        var status = GetGpioValues(out var value0, out var value1, out var value2, out var value3);
        if (status != Mcp2221Status.NoError)
        {
            return status;
        }

        switch (pin)
        {
            case 0:
                value0 = value;
                break;
            case 1:
                value1 = value;
                break;
            case 2:
                value2 = value;
                break;
            case 3:
                value3 = value;
                break;
            default:
                return Mcp2221Status.InvalidParameter;
        }

        return SetGpioValues(value0, value1, value2, value3);
    }

    // TODO Delete
    public Mcp2221Status GetGpioDirection(Span<byte> gpioDir)
    {
        fixed (byte* p = gpioDir)
        {
            return (Mcp2221Status)Mcp2221_GetGpioDirection(Handle, p);
        }
    }

    public Mcp2221Status GetGpioDirection(out PinMode mode0, out PinMode mode1, out PinMode mode2, out PinMode mode3)
    {
        var buffer = stackalloc byte[4];
        var status = (Mcp2221Status)Mcp2221_SetGpioDirection(Handle, buffer);
        mode0 = (PinMode)buffer[0];
        mode1 = (PinMode)buffer[1];
        mode2 = (PinMode)buffer[2];
        mode3 = (PinMode)buffer[3];
        return status;
    }

    // TODO Delete
    public Mcp2221Status SetGpioDirection(ReadOnlySpan<byte> gpioDir)
    {
        fixed (byte* p = gpioDir)
        {
            return (Mcp2221Status)Mcp2221_SetGpioDirection(Handle, p);
        }
    }

    public Mcp2221Status SetGpioDirection(PinMode mode0, PinMode mode1, PinMode mode2, PinMode mode3)
    {
        var buffer = stackalloc byte[4];
        buffer[0] = (byte)mode0;
        buffer[1] = (byte)mode1;
        buffer[2] = (byte)mode2;
        buffer[3] = (byte)mode3;
        return (Mcp2221Status)Mcp2221_SetGpioDirection(Handle, buffer);
    }

    public Mcp2221Status SetGpioDirection(int pin, PinMode mode)
    {
        var status = GetGpioDirection(out var mode0, out var mode1, out var mode2, out var mode3);
        if (status != Mcp2221Status.NoError)
        {
            return status;
        }

        switch (pin)
        {
            case 0:
                mode0 = mode;
                break;
            case 1:
                mode1 = mode;
                break;
            case 2:
                mode2 = mode;
                break;
            case 3:
                mode3 = mode;
                break;
            default:
                return Mcp2221Status.InvalidParameter;
        }

        return SetGpioDirection(mode0, mode1, mode2, mode3);
    }

    //------------------------------------------------------------------------
    // Misc
    //------------------------------------------------------------------------

    public Mcp2221Status GetSecuritySetting(out byte securitySetting)
        => (Mcp2221Status)Mcp2221_GetSecuritySetting(Handle, out securitySetting);

    public Mcp2221Status SetSecuritySetting(byte securitySetting, string currentPassword, string newPassword)
        => (Mcp2221Status)Mcp2221_SetSecuritySetting(Handle, securitySetting, currentPassword, newPassword);

    public Mcp2221Status SendPassword(string password)
        => (Mcp2221Status)Mcp2221_SendPassword(Handle, password);

    //public Mcp2221Status SetPermanentLock()
    //    => (Mcp2221Status)Mcp2221_SetPermanentLock(Handle);

    public static Mcp2221Status GetLastError()
        => (Mcp2221Status)Mcp2221_GetLastError();

    //------------------------------------------------------------------------
    // Helpers
    //------------------------------------------------------------------------

    private static string CreateStringFromNullTerminated(char* p, int maxChars)
    {
        var len = 0;
        for (; len < maxChars; len++)
        {
            if (p[len] == '\0')
            {
                break;
            }
        }
        return new string(p, 0, len);
    }
}
// ReSharper restore InconsistentNaming
