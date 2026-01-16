namespace MCP2221ManagedWrapper;

using static MCP2221ManagedWrapper.NativeMethods;

// ReSharper disable InconsistentNaming
#pragma warning disable CA1008
#pragma warning disable CA1028
#pragma warning disable CA1069
#pragma warning disable CA1707
public enum PinDirection : byte
{
    Output = MCP2221_GPDIR_OUTPUT,
    Input = MCP2221_GPDIR_INPUT
}

public enum PinValue : byte
{
    Low = MCP2221_GPVAL_LOW,
    High = MCP2221_GPVAL_HIGH
}

// GP0        GP1       GP2        GP3
// --------------------------------------
// Gpio       Gpio      Gpio       Gpio
// Sspnd      ClockOut  UsbConfig  LedI2c
// LedUartRx  Adc       Adc        Adc
// LedUartTx  Dac       Dac
//            InterruptDetection
public enum PinFunction : byte
{
    // 0
    Gpio = MCP2221_GPFUNC_IO,
    // 1
    Sspnd = MCP2221_GP_SSPND,
    ClockOut = MCP2221_GP_CLOCK_OUT,
    UsbConfig = MCP2221_GP_USBCFG,
    LedI2c = MCP2221_GP_LED_I2C,
    // 2
    LedUartRx = MCP2221_GP_LED_UART_RX,
    Adc = MCP2221_GP_ADC,
    // 3
    LedUartTx = MCP2221_GP_LED_UART_TX,
    Dac = MCP2221_GP_DAC,
    // 4
    InterruptDetection = MCP2221_GP_IOC
}

public enum ValueSource : byte
{
    Flash = FLASH_SETTINGS,
    Runtime = RUNTIME_SETTINGS
}

public enum InterruptEdge : byte
{
    None = INTERRUPT_NONE,
    Positive = INTERRUPT_POSITIVE_EDGE,
    Negative = INTERRUPT_NEGATIVE_EDGE,
    Both = INTERRUPT_BOTH_EDGES
}

public enum VoltageReference : byte
{
    Vdd = VREF_VDD,
    V1024 = VREF_1024V,
    V2048 = VREF_2048V,
    V4096 = VREF_4096V
}

[Flags]
public enum PowerAttributes : byte
{
    Bus = MCP2221_USB_BUS,
    Self = MCP2221_USB_SELF,
    Remote = MCP2221_USB_REMOTE
}

public enum SecuritySetting : byte
{
    Disable = MCP2221_PASS_DISABLE,
    Enable = MCP2221_PASS_ENABLE,
    Change = MCP2221_PASS_CHANGE
}

public enum DutyCycle : byte
{
    P0 = 0, // 0 %
    P25 = 1, // 25 %
    P50 = 2, // 50 %
    P75 = 3 // 75 %
}

public enum ClockDivider : byte
{
    MHz24 = 1, // 24 MHz
    MHz12 = 2, // 12 MHz
    MHz6 = 3, // 6 MHz
    MHz3 = 4, // 3 MHz
    MHz1_5 = 5, // 1.5 MHz
    KHz750 = 6, // 750 kHz
    KHz375 = 7 // 375 kHz
}
#pragma warning restore CA1707
#pragma warning restore CA1069
#pragma warning restore CA1028
#pragma warning restore CA1008
// ReSharper restore InconsistentNaming

#pragma warning disable CA1819
public sealed class GpioSettings
{
    public PinFunction[] Functions { get; } = new PinFunction[4];

    public PinDirection[] Directions { get; } = new PinDirection[4];

    public PinValue[] Values { get; } = new PinValue[4];
}
#pragma warning restore CA1819

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

    public Mcp2221Status GetUsbPowerAttributes(out PowerAttributes powerAttributes, out uint currentReq)
    {
        var status = (Mcp2221Status)Mcp2221_GetUsbPowerAttributes(Handle, out var powerAttributesByte, out currentReq);
        powerAttributes = (PowerAttributes)powerAttributesByte;
        return status;
    }

    public Mcp2221Status SetUsbPowerAttributes(PowerAttributes powerAttributes, uint currentReq)
        => (Mcp2221Status)Mcp2221_SetUsbPowerAttributes(Handle, (byte)powerAttributes, currentReq);

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

    public Mcp2221Status GetInterruptEdgeSetting(ValueSource source, out InterruptEdge interruptEdge)
    {
        var status = (Mcp2221Status)Mcp2221_GetInterruptEdgeSetting(Handle, (byte)source, out var interruptPinDirection);
        interruptEdge = (InterruptEdge)interruptPinDirection;
        return status;
    }

    public Mcp2221Status SetInterruptEdgeSetting(ValueSource source, InterruptEdge interruptEdge)
        => (Mcp2221Status)Mcp2221_SetInterruptEdgeSetting(Handle, (byte)source, (byte)interruptEdge);

    public Mcp2221Status ClearInterruptPinFlag()
        => (Mcp2221Status)Mcp2221_ClearInterruptPinFlag(Handle);

    public Mcp2221Status GetInterruptPinFlag(out byte flagValue)
        => (Mcp2221Status)Mcp2221_GetInterruptPinFlag(Handle, out flagValue);

    public Mcp2221Status GetClockSettings(ValueSource source, out DutyCycle dutyCycle, out ClockDivider clockDivider)
    {
        var status = (Mcp2221Status)Mcp2221_GetClockSettings(Handle, (byte)source, out var dutyCycleByte, out var clockDividerByte);
        dutyCycle = (DutyCycle)dutyCycleByte;
        clockDivider = (ClockDivider)clockDividerByte;
        return status;
    }

    public Mcp2221Status SetClockSettings(ValueSource source, DutyCycle dutyCycle, ClockDivider clockDivider)
        => (Mcp2221Status)Mcp2221_SetClockSettings(Handle, (byte)source, (byte)dutyCycle, (byte)clockDivider);

    public Mcp2221Status GetAdcData(out uint value1, out uint value2, out uint value3)
    {
        var buffer = stackalloc uint[3];
        var status = (Mcp2221Status)Mcp2221_GetAdcData(Handle, buffer);
        value1 = buffer[0];
        value2 = buffer[1];
        value3 = buffer[2];
        return status;
    }

    public Mcp2221Status GetAdcVref(ValueSource source, out VoltageReference adcVref)
    {
        var status = (Mcp2221Status)Mcp2221_GetAdcVref(Handle, (byte)source, out var adcVrefByte);
        adcVref = (VoltageReference)adcVrefByte;
        return status;
    }

    public Mcp2221Status SetAdcVref(ValueSource source, VoltageReference adcVref)
        => (Mcp2221Status)Mcp2221_SetAdcVref(Handle, (byte)source, (byte)adcVref);

    public Mcp2221Status GetDacVref(ValueSource source, out VoltageReference dacVref)
    {
        var status = (Mcp2221Status)Mcp2221_GetDacVref(Handle, (byte)source, out var dacVrefByte);
        dacVref = (VoltageReference)dacVrefByte;
        return status;
    }

    public Mcp2221Status SetDacVref(ValueSource source, VoltageReference dacVref)
        => (Mcp2221Status)Mcp2221_SetDacVref(Handle, (byte)source, (byte)dacVref);

    public Mcp2221Status GetDacValue(ValueSource source, out byte dacValue)
        => (Mcp2221Status)Mcp2221_GetDacValue(Handle, (byte)source, out dacValue);

    public Mcp2221Status SetDacValue(ValueSource source, byte dacValue)
        => (Mcp2221Status)Mcp2221_SetDacValue(Handle, (byte)source, dacValue);

    public Mcp2221Status GetGpioSettings(ValueSource source, GpioSettings settings)
    {
        var functions = stackalloc byte[4];
        var directions = stackalloc byte[4];
        var modes = stackalloc byte[4];
        var status = (Mcp2221Status)Mcp2221_GetGpioSettings(Handle, (byte)source, functions, directions, modes);
        for (var i = 0; i < 4; i++)
        {
            settings.Functions[i] = (PinFunction)functions[i];
            settings.Directions[i] = (PinDirection)directions[i];
            settings.Values[i] = (PinValue)modes[i];
        }
        return status;
    }

    public Mcp2221Status SetGpioSettings(ValueSource source, GpioSettings settings)
    {
        var functions = stackalloc byte[4];
        var directions = stackalloc byte[4];
        var modes = stackalloc byte[4];
        for (var i = 0; i < 4; i++)
        {
            functions[i] = (byte)settings.Functions[i];
            directions[i] = (byte)settings.Directions[i];
            modes[i] = (byte)settings.Values[i];
        }
        return (Mcp2221Status)Mcp2221_SetGpioSettings(Handle, (byte)source, functions, directions, modes);
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

    public Mcp2221Status GetGpioDirection(out PinDirection mode0, out PinDirection mode1, out PinDirection mode2, out PinDirection mode3)
    {
        var buffer = stackalloc byte[4];
        var status = (Mcp2221Status)Mcp2221_SetGpioDirection(Handle, buffer);
        mode0 = (PinDirection)buffer[0];
        mode1 = (PinDirection)buffer[1];
        mode2 = (PinDirection)buffer[2];
        mode3 = (PinDirection)buffer[3];
        return status;
    }

    public Mcp2221Status SetGpioDirection(PinDirection mode0, PinDirection mode1, PinDirection mode2, PinDirection mode3)
    {
        var buffer = stackalloc byte[4];
        buffer[0] = (byte)mode0;
        buffer[1] = (byte)mode1;
        buffer[2] = (byte)mode2;
        buffer[3] = (byte)mode3;
        return (Mcp2221Status)Mcp2221_SetGpioDirection(Handle, buffer);
    }

    public Mcp2221Status SetGpioDirection(int pin, PinDirection mode)
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

    public Mcp2221Status GetSecuritySetting(out SecuritySetting securitySetting)
    {
        var status = (Mcp2221Status)Mcp2221_GetSecuritySetting(Handle, out var securitySettingByte);
        securitySetting = (SecuritySetting)securitySettingByte;
        return status;
    }

    public Mcp2221Status SetSecuritySetting(SecuritySetting securitySetting, string currentPassword, string newPassword)
        => (Mcp2221Status)Mcp2221_SetSecuritySetting(Handle, (byte)securitySetting, currentPassword, newPassword);

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
