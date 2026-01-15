// TODO delete
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace MCP2221ManagedWrapper;

// TODO static using NativeMethods
public sealed unsafe class Mcp2221 : IDisposable
{
    private const int DescriptorMaxChars = 30;
    private const int DescriptorBufferChars = DescriptorMaxChars + 1; // + null terminator

    private const int LibraryVersionBufferChars = 64;
    private const int RevisionBufferChars = 64;

    public IntPtr Handle { get; private set; }

    public bool IsOpen => Handle != IntPtr.Zero;

    private Mcp2221(IntPtr handle)
    {
        Handle = handle;
    }

    // ------------------------------------------------------------
    // Open/Close
    // ------------------------------------------------------------

    // TODO default VID/PID constants, 1st index
    public static Mcp2221 OpenByIndex(uint vid, uint pid, uint index)
    {
        return new Mcp2221(NativeMethods.Mcp2221_OpenByIndex(vid, pid, index));
    }

    // TODO
    public static Mcp2221 OpenBySerialNumber(uint vid, uint pid, string serialNumber)
    {
        return new Mcp2221(NativeMethods.Mcp2221_OpenBySN(vid, pid, serialNumber));
    }

    public void Dispose()
    {
        if (IsOpen)
        {
            Close();
        }
    }

    public Mcp2221Status Close()
    {
        if (!IsOpen)
        {
            return Mcp2221Status.InvalidHandle;
        }
        var st = (Mcp2221Status)NativeMethods.Mcp2221_Close(Handle);
        Handle = IntPtr.Zero;
        return st;
    }

    public Mcp2221Status Reset()
        => (Mcp2221Status)NativeMethods.Mcp2221_Reset(Handle);

    public static Mcp2221Status CloseAll()
        => (Mcp2221Status)NativeMethods.Mcp2221_CloseAll();

    // ------------------------------------------------------------
    // Library / enumeration
    // ------------------------------------------------------------
    public static Mcp2221Status GetConnectedDevices(uint vid, uint pid, out uint count)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetConnectedDevices(vid, pid, out count);

    public static Mcp2221Status GetLibraryVersion(out string version)
    {
        Span<char> buf = stackalloc char[LibraryVersionBufferChars];
        buf.Clear();

        fixed (char* p = buf)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetLibraryVersion(p);
            if (st != Mcp2221Status.NoError)
            {
                version = string.Empty;
                return st;
            }

            version = CreateStringFromNullTerminated(p, buf.Length);
            return Mcp2221Status.NoError;
        }
    }

    // ------------------------------------------------------------
    // USB descriptors (out string, stackalloc)
    // ------------------------------------------------------------
    public Mcp2221Status GetManufacturerDescriptor(out string manufacturer)
    {
        Span<char> buf = stackalloc char[DescriptorBufferChars];
        buf.Clear();

        fixed (char* p = buf)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetManufacturerDescriptor(Handle, p);
            if (st != Mcp2221Status.NoError)
            {
                manufacturer = string.Empty;
                return st;
            }

            manufacturer = CreateStringFromNullTerminated(p, buf.Length);
            return Mcp2221Status.NoError;
        }
    }

    public Mcp2221Status SetManufacturerDescriptor(string manufacturer)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetManufacturerDescriptor(Handle, manufacturer);

    public Mcp2221Status GetProductDescriptor(out string product)
    {
        Span<char> buf = stackalloc char[DescriptorBufferChars];
        buf.Clear();

        fixed (char* p = buf)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetProductDescriptor(Handle, p);
            if (st != Mcp2221Status.NoError)
            {
                product = string.Empty;
                return st;
            }

            product = CreateStringFromNullTerminated(p, buf.Length);
            return Mcp2221Status.NoError;
        }
    }

    public Mcp2221Status SetProductDescriptor(string product)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetProductDescriptor(Handle, product);

    public Mcp2221Status GetSerialNumberDescriptor(out string serial)
    {
        Span<char> buf = stackalloc char[DescriptorBufferChars];
        buf.Clear();

        fixed (char* p = buf)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetSerialNumberDescriptor(Handle, p);
            if (st != Mcp2221Status.NoError)
            {
                serial = string.Empty;
                return st;
            }

            serial = CreateStringFromNullTerminated(p, buf.Length);
            return Mcp2221Status.NoError;
        }
    }

    public Mcp2221Status SetSerialNumberDescriptor(string serial)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetSerialNumberDescriptor(Handle, serial);

    public Mcp2221Status GetFactorySerialNumber(out string serial)
    {
        Span<char> buf = stackalloc char[DescriptorBufferChars];
        buf.Clear();

        fixed (char* p = buf)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetFactorySerialNumber(Handle, p);
            if (st != Mcp2221Status.NoError)
            {
                serial = string.Empty;
                return st;
            }

            serial = CreateStringFromNullTerminated(p, buf.Length);
            return Mcp2221Status.NoError;
        }
    }

    public Mcp2221Status GetHwFwRevisions(out string hardwareRevision, out string firmwareRevision)
    {
        Span<char> hw = stackalloc char[RevisionBufferChars];
        Span<char> fw = stackalloc char[RevisionBufferChars];
        hw.Clear();
        fw.Clear();

        fixed (char* pHw = hw)
        fixed (char* pFw = fw)
        {
            var st = (Mcp2221Status)NativeMethods.Mcp2221_GetHwFwRevisions(Handle, pHw, pFw);
            if (st != Mcp2221Status.NoError)
            {
                hardwareRevision = string.Empty;
                firmwareRevision = string.Empty;
                return st;
            }

            hardwareRevision = CreateStringFromNullTerminated(pHw, hw.Length);
            firmwareRevision = CreateStringFromNullTerminated(pFw, fw.Length);
            return Mcp2221Status.NoError;
        }
    }

    // ------------------------------------------------------------
    // USB attributes
    // ------------------------------------------------------------
    public Mcp2221Status GetVidPid(out uint vid, out uint pid)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetVidPid(Handle, out vid, out pid);

    public Mcp2221Status SetVidPid(uint vid, uint pid)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetVidPid(Handle, vid, pid);

    public Mcp2221Status GetUsbPowerAttributes(out byte powerAttributes, out uint currentReq)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetUsbPowerAttributes(Handle, out powerAttributes, out currentReq);

    public Mcp2221Status SetUsbPowerAttributes(byte powerAttributes, uint currentReq)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetUsbPowerAttributes(Handle, powerAttributes, currentReq);

    public Mcp2221Status GetSerialNumberEnumerationEnable(out byte enabled)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetSerialNumberEnumerationEnable(Handle, out enabled);

    public Mcp2221Status SetSerialNumberEnumerationEnable(byte enabled)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetSerialNumberEnumerationEnable(Handle, enabled);

    // ------------------------------------------------------------
    // I2C
    // ------------------------------------------------------------
    public Mcp2221Status I2cCancelCurrentTransfer()
        => (Mcp2221Status)NativeMethods.Mcp2221_I2cCancelCurrentTransfer(Handle);

    public Mcp2221Status SetAdvancedCommParams(byte timeout, byte maxRetries)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetAdvancedCommParams(Handle, timeout, maxRetries);

    public Mcp2221Status SetSpeed(uint speed)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetSpeed(Handle, speed);

    public Mcp2221Status I2cRead(byte slaveAddress, bool use7BitAddress, Span<byte> rx)
    {
        fixed (byte* p = rx)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_I2cRead(
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
            return (Mcp2221Status)NativeMethods.Mcp2221_I2cWrite(
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
            return (Mcp2221Status)NativeMethods.Mcp2221_I2cWriteNoStop(
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
            return (Mcp2221Status)NativeMethods.Mcp2221_I2cReadRestart(
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
            return (Mcp2221Status)NativeMethods.Mcp2221_I2cWriteRestart(
                Handle,
                (uint)tx.Length,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                p);
        }
    }

    // ------------------------------------------------------------
    // SMBus
    // ------------------------------------------------------------
    public Mcp2221Status SmbusWriteByte(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, byte data)
        => (Mcp2221Status)NativeMethods.Mcp2221_SmbusWriteByte(
            Handle,
            slaveAddress,
            use7BitAddress ? (byte)1 : (byte)0,
            usePec ? (byte)1 : (byte)0,
            command,
            data);

    public Mcp2221Status SmbusReadByte(byte slaveAddress, bool use7BitAddress, bool usePec, byte command, out byte readByte)
    {
        readByte = 0;
        fixed (byte* p = &readByte)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusReadByte(
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
        if (data2Bytes.Length < 2)
        {
            return Mcp2221Status.InvalidParameter;
        }

        fixed (byte* p = data2Bytes)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusWriteWord(
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
        if (read2Bytes.Length < 2)
        {
            return Mcp2221Status.InvalidParameter;
        }

        fixed (byte* p = read2Bytes)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusReadWord(
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
        if (data.Length > byte.MaxValue)
        {
            return Mcp2221Status.InputTooLarge;
        }

        fixed (byte* p = data)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusBlockWrite(
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
        // “byteCountはSpan.Length基準”
        if (readData.Length > byte.MaxValue)
        {
            return Mcp2221Status.InputTooLarge;
        }

        fixed (byte* p = readData)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusBlockRead(
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
        // “byteCountはSpan.Length基準”
        if (writeData.Length > byte.MaxValue || readData.Length > byte.MaxValue)
        {
            return Mcp2221Status.InputTooLarge;
        }

        fixed (byte* pW = writeData)
        fixed (byte* pR = readData)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusBlockWriteBlockReadProcessCall(
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
        => (Mcp2221Status)NativeMethods.Mcp2221_SmbusSendByte(
            Handle,
            slaveAddress,
            use7BitAddress ? (byte)1 : (byte)0,
            usePec ? (byte)1 : (byte)0,
            data);

    public Mcp2221Status SmbusReceiveByte(byte slaveAddress, bool use7BitAddress, bool usePec, out byte readByte)
    {
        readByte = 0;
        fixed (byte* p = &readByte)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SmbusReceiveByte(
                Handle,
                slaveAddress,
                use7BitAddress ? (byte)1 : (byte)0,
                usePec ? (byte)1 : (byte)0,
                p);
        }
    }

    // ------------------------------------------------------------
    // GPIO / ADC / DAC
    // ------------------------------------------------------------
    public Mcp2221Status GetInitialPinValues(out byte ledUrxInitVal, out byte ledUtxInitVal, out byte ledI2cInitVal, out byte sspndInitVal, out byte usbCfgInitVal)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetInitialPinValues(Handle, out ledUrxInitVal, out ledUtxInitVal, out ledI2cInitVal, out sspndInitVal, out usbCfgInitVal);

    public Mcp2221Status SetInitialPinValues(byte ledUrxInitVal, byte ledUtxInitVal, byte ledI2cInitVal, byte sspndInitVal, byte usbCfgInitVal)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetInitialPinValues(Handle, ledUrxInitVal, ledUtxInitVal, ledI2cInitVal, sspndInitVal, usbCfgInitVal);

    public Mcp2221Status GetInterruptEdgeSetting(byte whichToGet, out byte interruptPinMode)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetInterruptEdgeSetting(Handle, whichToGet, out interruptPinMode);

    public Mcp2221Status SetInterruptEdgeSetting(byte whichToSet, byte interruptPinMode)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetInterruptEdgeSetting(Handle, whichToSet, interruptPinMode);

    public Mcp2221Status ClearInterruptPinFlag()
        => (Mcp2221Status)NativeMethods.Mcp2221_ClearInterruptPinFlag(Handle);

    public Mcp2221Status GetInterruptPinFlag(out byte flagValue)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetInterruptPinFlag(Handle, out flagValue);

    public Mcp2221Status GetClockSettings(byte whichToGet, out byte dutyCycle, out byte clockDivider)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetClockSettings(Handle, whichToGet, out dutyCycle, out clockDivider);

    public Mcp2221Status SetClockSettings(byte whichToSet, byte dutyCycle, byte clockDivider)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetClockSettings(Handle, whichToSet, dutyCycle, clockDivider);

    public Mcp2221Status GetAdcData(Span<uint> adcDataArray)
    {
        fixed (uint* p = adcDataArray)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_GetAdcData(Handle, p);
        }
    }

    public Mcp2221Status GetAdcVref(byte whichToGet, out byte adcVref)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetAdcVref(Handle, whichToGet, out adcVref);

    public Mcp2221Status SetAdcVref(byte whichToSet, byte adcVref)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetAdcVref(Handle, whichToSet, adcVref);

    public Mcp2221Status GetDacVref(byte whichToGet, out byte dacVref)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetDacVref(Handle, whichToGet, out dacVref);

    public Mcp2221Status SetDacVref(byte whichToSet, byte dacVref)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetDacVref(Handle, whichToSet, dacVref);

    public Mcp2221Status GetDacValue(byte whichToGet, out byte dacValue)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetDacValue(Handle, whichToGet, out dacValue);

    public Mcp2221Status SetDacValue(byte whichToSet, byte dacValue)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetDacValue(Handle, whichToSet, dacValue);

    public Mcp2221Status GetGpioSettings(byte whichToGet, Span<byte> pinFunctions, Span<byte> pinDirections, Span<byte> outputValues)
    {
        fixed (byte* pF = pinFunctions)
        fixed (byte* pD = pinDirections)
        fixed (byte* pO = outputValues)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_GetGpioSettings(Handle, whichToGet, pF, pD, pO);
        }
    }

    public Mcp2221Status SetGpioSettings(byte whichToSet, ReadOnlySpan<byte> pinFunctions, ReadOnlySpan<byte> pinDirections, ReadOnlySpan<byte> outputValues)
    {
        fixed (byte* pF = pinFunctions)
        fixed (byte* pD = pinDirections)
        fixed (byte* pO = outputValues)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SetGpioSettings(Handle, whichToSet, pF, pD, pO);
        }
    }

    public Mcp2221Status GetGpioValues(Span<byte> gpioValues)
    {
        fixed (byte* p = gpioValues)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_GetGpioValues(Handle, p);
        }
    }

    public Mcp2221Status SetGpioValues(ReadOnlySpan<byte> gpioValues)
    {
        fixed (byte* p = gpioValues)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SetGpioValues(Handle, p);
        }
    }

    public Mcp2221Status GetGpioDirection(Span<byte> gpioDir)
    {
        fixed (byte* p = gpioDir)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_GetGpioDirection(Handle, p);
        }
    }

    public Mcp2221Status SetGpioDirection(ReadOnlySpan<byte> gpioDir)
    {
        fixed (byte* p = gpioDir)
        {
            return (Mcp2221Status)NativeMethods.Mcp2221_SetGpioDirection(Handle, p);
        }
    }

    // ------------------------------------------------------------
    // Security / misc
    // ------------------------------------------------------------
    public Mcp2221Status GetSecuritySetting(out byte securitySetting)
        => (Mcp2221Status)NativeMethods.Mcp2221_GetSecuritySetting(Handle, out securitySetting);

    public Mcp2221Status SetSecuritySetting(byte securitySetting, string currentPassword, string newPassword)
        => (Mcp2221Status)NativeMethods.Mcp2221_SetSecuritySetting(Handle, securitySetting, currentPassword, newPassword);

    public Mcp2221Status SendPassword(string password)
        => (Mcp2221Status)NativeMethods.Mcp2221_SendPassword(Handle, password);

    public Mcp2221Status SetPermanentLock()
        => (Mcp2221Status)NativeMethods.Mcp2221_SetPermanentLock(Handle);

    public static Mcp2221Status GetLastError()
        => (Mcp2221Status)NativeMethods.Mcp2221_GetLastError();

    // ------------------------------------------------------------
    // helpers
    // ------------------------------------------------------------
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
