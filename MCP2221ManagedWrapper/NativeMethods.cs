namespace MCP2221ManagedWrapper;

using System.Runtime.InteropServices;

// TODO check all extern
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006
#pragma warning disable CA2101
#pragma warning disable CA5392
#pragma warning disable CS8981
internal static unsafe class NativeMethods
{
    internal const string DllName = "mcp2221_dll_um_x64.dll";

    // ---- Open/Close ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern IntPtr Mcp2221_OpenByIndex(
        uint VID,
        uint PID,
        uint index);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern IntPtr Mcp2221_OpenBySN(
        uint VID,
        uint PID,
        string serialNo);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_Close(
        IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_Reset(
        IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_CloseAll();

    // ---- Library / enumeration ----
    // wchar_t* output => char* (UTF-16)
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetLibraryVersion(
        char* version);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetConnectedDevices(
        uint vid,
        uint pid,
        out uint noOfDevs);

    // ---- USB descriptors ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetManufacturerDescriptor(
        IntPtr handle,
        char* manufacturerString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_SetManufacturerDescriptor(
        IntPtr handle,
        string manufacturerString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetProductDescriptor(
        IntPtr handle,
        char* productString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_SetProductDescriptor(
        IntPtr handle,
        string productString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetSerialNumberDescriptor(
        IntPtr handle,
        char* serialNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_SetSerialNumberDescriptor(
        IntPtr handle,
        string serialNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetFactorySerialNumber(
        IntPtr handle,
        char* serialNumber);

    // ---- USB ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetVidPid(
        IntPtr handle,
        out uint vid,
        out uint pid);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetVidPid(
        IntPtr handle,
        uint vid,
        uint pid);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetUsbPowerAttributes(
        IntPtr handle,
        out byte powerAttributes,
        out uint currentReq);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetUsbPowerAttributes(
        IntPtr handle,
        byte powerAttributes,
        uint currentReq);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetSerialNumberEnumerationEnable(
        IntPtr handle,
        out byte snEnumEnabled);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetSerialNumberEnumerationEnable(
        IntPtr handle,
        byte snEnumEnabled);

    // ---- I2C ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cCancelCurrentTransfer(
        IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cRead(
        IntPtr handle,
        uint bytesToRead,
        byte slaveAddress,
        byte use7bitAddress,
        byte* i2cRxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cWrite(
        IntPtr handle,
        uint bytesToWrite,
        byte slaveAddress,
        byte use7bitAddress,
        byte* i2cTxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetAdvancedCommParams(
        IntPtr handle,
        byte timeout,
        byte maxRetries);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetSpeed(
        IntPtr handle,
        uint speed);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cWriteNoStop(
        IntPtr handle,
        uint bytesToWrite,
        byte slaveAddress,
        byte use7bitAddress,
        byte* i2cTxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cReadRestart(
        IntPtr handle,
        uint bytesToRead,
        byte slaveAddress,
        byte use7bitAddress,
        byte* i2cRxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_I2cWriteRestart(
        IntPtr handle,
        uint bytesToWrite,
        byte slaveAddress,
        byte use7bitAddress,
        byte* i2cTxData);

    // ---- SMBus ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusWriteByte(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusReadByte(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte* readByte);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusWriteWord(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte* data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusReadWord(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte* readData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusBlockWrite(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte byteCount,
        byte* data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusBlockRead(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte byteCount,
        byte* readData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusBlockWriteBlockReadProcessCall(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte command,
        byte writeByteCount,
        byte* writeData,
        byte readByteCount,
        byte* readData);

    // ---- GPIO / misc ----
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetInitialPinValues(
        IntPtr handle,
        out byte ledUrxInitVal,
        out byte ledUtxInitVal,
        out byte ledI2cInitVal,
        out byte sspndInitVal,
        out byte usbCfgInitVal);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetInitialPinValues(
        IntPtr handle,
        byte ledUrxInitVal,
        byte ledUtxInitVal,
        byte ledI2cInitVal,
        byte sspndInitVal,
        byte usbCfgInitVal);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetInterruptEdgeSetting(
        IntPtr handle,
        byte whichToGet,
        out byte interruptPinMode);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetInterruptEdgeSetting(
        IntPtr handle,
        byte whichToSet,
        byte interruptPinMode);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_ClearInterruptPinFlag(
        IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetInterruptPinFlag(
        IntPtr handle,
        out byte flagValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetClockSettings(
        IntPtr handle,
        byte whichToGet,
        out byte dutyCycle,
        out byte clockDivider);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetClockSettings(
        IntPtr handle,
        byte whichToSet,
        byte dutyCycle,
        byte clockDivider);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetAdcData(
        IntPtr handle,
        uint* adcDataArray);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetAdcVref(
        IntPtr handle,
        byte whichToGet,
        out byte adcVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetAdcVref(
        IntPtr handle,
        byte whichToSet,
        byte adcVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetDacVref(
        IntPtr handle,
        byte whichToGet,
        out byte dacVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetDacVref(
        IntPtr handle,
        byte whichToSet,
        byte dacVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetDacValue(
        IntPtr handle,
        byte whichToGet,
        out byte dacValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetDacValue(
        IntPtr handle,
        byte whichToSet,
        byte dacValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetGpioSettings(
        IntPtr handle,
        byte whichToGet,
        byte* pinFunctions,
        byte* pinDirections,
        byte* outputValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetGpioSettings(
        IntPtr handle,
        byte whichToSet,
        byte* pinFunctions,
        byte* pinDirections,
        byte* outputValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetGpioValues(
        IntPtr handle,
        byte* gpioValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetGpioValues(
        IntPtr handle,
        byte* gpioValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetGpioDirection(
        IntPtr handle,
        byte* gpioDir);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetGpioDirection(
        IntPtr handle,
        byte* gpioDir);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetSecuritySetting(
        IntPtr handle,
        out byte securitySetting);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetSecuritySetting(
        IntPtr handle,
        byte securitySetting,
        [MarshalAs(UnmanagedType.LPStr)] string currentPassword,
        [MarshalAs(UnmanagedType.LPStr)] string newPassword);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SendPassword(
        IntPtr handle,
        [MarshalAs(UnmanagedType.LPStr)] string password);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SetPermanentLock(
        IntPtr handle);

    // wchar_t* output => char* (UTF-16)
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    internal static extern int Mcp2221_GetHwFwRevisions(
        IntPtr handle,
        char* hardwareRevision,
        char* firmwareRevision);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_GetLastError();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusSendByte(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    internal static extern int Mcp2221_SmbusReceiveByte(
        IntPtr handle,
        byte slaveAddress,
        byte use7bitAddress,
        byte usePec,
        byte* readByte);

    // ---- Error codes (C constants) ----
    internal const int E_NO_ERR = 0;
    internal const int E_ERR_UNKOWN_ERROR = -1;
    internal const int E_ERR_CMD_FAILED = -2;
    internal const int E_ERR_INVALID_HANDLE = -3;
    internal const int E_ERR_INVALID_PARAMETER = -4;
    internal const int E_ERR_INVALID_PASS = -5;
    internal const int E_ERR_PASSWORD_LIMIT_REACHED = -6;
    internal const int E_ERR_FLASH_WRITE_PROTECTED = -7;

    internal const int E_ERR_NULL = -10;
    internal const int E_ERR_DESTINATION_TOO_SMALL = -11;
    internal const int E_ERR_INPUT_TOO_LARGE = -12;
    internal const int E_ERR_FLASH_WRITE_FAILED = -13;
    internal const int E_ERR_MALLOC = -14;

    internal const int E_ERR_NO_SUCH_INDEX = -101;
    internal const int E_ERR_DEVICE_NOT_FOUND = -103;
    internal const int E_ERR_INTERNAL_BUFFER_TOO_SMALL = -104;
    internal const int E_ERR_OPEN_DEVICE_ERROR = -105;
    internal const int E_ERR_CONNECTION_ALREADY_OPENED = -106;
    internal const int E_ERR_CLOSE_FAILED = -107;

    internal const int E_ERR_INVALID_SPEED = -401;
    internal const int E_ERR_SPEED_NOT_SET = -402;
    internal const int E_ERR_INVALID_BYTE_NUMBER = -403;
    internal const int E_ERR_INVALID_ADDRESS = -404;
    internal const int E_ERR_I2C_BUSY = -405;
    internal const int E_ERR_I2C_READ_ERROR = -406;
    internal const int E_ERR_ADDRESS_NACK = -407;
    internal const int E_ERR_TIMEOUT = -408;
    internal const int E_ERR_TOO_MANY_RX_BYTES = -409;
    internal const int E_ERR_COPY_RX_DATA_FAILED = -410;
    internal const int E_ERR_NO_EFFECT = -411;
    internal const int E_ERR_COPY_TX_DATA_FAILED = -412;
    internal const int E_ERR_INVALID_PEC = -413;
    internal const int E_ERR_BLOCK_SIZE_MISMATCH = -414;

    internal const int E_ERR_RAW_TX_TOO_LARGE = -301;
    internal const int E_ERR_RAW_TX_COPYFAILED = -302;
    internal const int E_ERR_RAW_RX_COPYFAILED = -303;

    // ---- Constants ----
    // TODO check all const
    internal const int FLASH_SETTINGS = 0;
    internal const int RUNTIME_SETTINGS = 1;
    internal const byte NO_CHANGE = 0xFF;

    internal const byte MCP2221_GPFUNC_IO = 0;
    internal const byte MCP2221_GP_SSPND = 1;
    internal const byte MCP2221_GP_CLOCK_OUT = 1;
    internal const byte MCP2221_GP_USBCFG = 1;
    internal const byte MCP2221_GP_LED_I2C = 1;
    internal const byte MCP2221_GP_LED_UART_RX = 2;
    internal const byte MCP2221_GP_ADC = 2;
    internal const byte MCP2221_GP_LED_UART_TX = 3;
    internal const byte MCP2221_GP_DAC = 3;
    internal const byte MCP2221_GP_IOC = 4;

    internal const byte MCP2221_GPDIR_INPUT = 1;
    internal const byte MCP2221_GPDIR_OUTPUT = 0;
    internal const byte MCP2221_GPVAL_HIGH = 1;
    internal const byte MCP2221_GPVAL_LOW = 0;

    internal const byte INTERRUPT_NONE = 0;
    internal const byte INTERRUPT_POSITIVE_EDGE = 1;
    internal const byte INTERRUPT_NEGATIVE_EDGE = 2;
    internal const byte INTERRUPT_BOTH_EDGES = 3;

    internal const byte VREF_VDD = 0;
    internal const byte VREF_1024V = 1;
    internal const byte VREF_2048V = 2;
    internal const byte VREF_4096V = 3;

    internal const byte MCP2221_USB_BUS = 0x80;
    internal const byte MCP2221_USB_SELF = 0x40;
    internal const byte MCP2221_USB_REMOTE = 0x20;

    internal const byte MCP2221_PASS_ENABLE = 1;
    internal const byte MCP2221_PASS_DISABLE = 0;
    internal const byte MCP2221_PASS_CHANGE = 0xFF;
}
