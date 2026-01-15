namespace MCP2221ManagedWrapper;

using System.Runtime.InteropServices;

// TODO check all extern
// ReSharper disable CommentTypo
// ReSharper disable GrammarMistakeInComment
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006
#pragma warning disable CA2101
#pragma warning disable CA5392
#pragma warning disable CS8981
internal static unsafe class NativeMethods
{
    private const string DllName = "mcp2221_dll_um_x64.dll";

    //------------------------------------------------------------------------
    // Const
    //------------------------------------------------------------------------

    // Error codes

    public const int E_NO_ERR = 0;
    public const int E_ERR_UNKOWN_ERROR = -1;
    public const int E_ERR_CMD_FAILED = -2;
    public const int E_ERR_INVALID_HANDLE = -3;
    public const int E_ERR_INVALID_PARAMETER = -4;
    public const int E_ERR_INVALID_PASS = -5;
    public const int E_ERR_PASSWORD_LIMIT_REACHED = -6;
    public const int E_ERR_FLASH_WRITE_PROTECTED = -7;

    // null pointer received
    public const int E_ERR_NULL = -10;
    // destination string too small
    public const int E_ERR_DESTINATION_TOO_SMALL = -11;
    public const int E_ERR_INPUT_TOO_LARGE = -12;
    public const int E_ERR_FLASH_WRITE_FAILED = -13;
    public const int E_ERR_MALLOC = -14;

    //we tried to connect to a device with a non existent index
    public const int E_ERR_NO_SUCH_INDEX = -101;
    // no device matching the provided criteria was found
    public const int E_ERR_DEVICE_NOT_FOUND = -103;
    // one of the internal buffers of the function was too small
    public const int E_ERR_INTERNAL_BUFFER_TOO_SMALL = -104;
    // an error occurred when trying to get the device handle
    public const int E_ERR_OPEN_DEVICE_ERROR = -105;
    // connection already opened
    public const int E_ERR_CONNECTION_ALREADY_OPENED = -106;
    public const int E_ERR_CLOSE_FAILED = -107;

    // I2C errors

    public const int E_ERR_INVALID_SPEED = -401;
    public const int E_ERR_SPEED_NOT_SET = -402;
    public const int E_ERR_INVALID_BYTE_NUMBER = -403;
    public const int E_ERR_INVALID_ADDRESS = -404;
    public const int E_ERR_I2C_BUSY = -405;
    //mcp2221 signaled an error during the i2c read operation
    public const int E_ERR_I2C_READ_ERROR = -406;
    public const int E_ERR_ADDRESS_NACK = -407;
    public const int E_ERR_TIMEOUT = -408;
    public const int E_ERR_TOO_MANY_RX_BYTES = -409;
    //could not copy the data received from the slave into the provided buffer;
    public const int E_ERR_COPY_RX_DATA_FAILED = -410;
    // The i2c engine (inside mcp2221) was already idle. The cancellation command had no effect.
    public const int E_ERR_NO_EFFECT = -411;
    // failed to copy the data into the HID buffer
    public const int E_ERR_COPY_TX_DATA_FAILED = -412;
    public const int E_ERR_INVALID_PEC = -413;
    // The slave sent a different value for the block size(byte count) than we expected
    public const int E_ERR_BLOCK_SIZE_MISMATCH = -414;

    public const int E_ERR_RAW_TX_TOO_LARGE = -301;
    public const int E_ERR_RAW_TX_COPYFAILED = -302;
    public const int E_ERR_RAW_RX_COPYFAILED = -303;

    // TODO check all const

    // Constants

    public const int FLASH_SETTINGS = 0;
    public const int RUNTIME_SETTINGS = 1;
    public const byte NO_CHANGE = 0xFF;

    // GPIO settings

    public const byte MCP2221_GPFUNC_IO = 0;
    public const byte MCP2221_GP_SSPND = 1;
    public const byte MCP2221_GP_CLOCK_OUT = 1;
    public const byte MCP2221_GP_USBCFG = 1;
    public const byte MCP2221_GP_LED_I2C = 1;
    public const byte MCP2221_GP_LED_UART_RX = 2;
    public const byte MCP2221_GP_ADC = 2;
    public const byte MCP2221_GP_LED_UART_TX = 3;
    public const byte MCP2221_GP_DAC = 3;
    public const byte MCP2221_GP_IOC = 4;

    public const byte MCP2221_GPDIR_INPUT = 1;
    public const byte MCP2221_GPDIR_OUTPUT = 0;
    public const byte MCP2221_GPVAL_HIGH = 1;
    public const byte MCP2221_GPVAL_LOW = 0;

    public const byte INTERRUPT_NONE = 0;
    public const byte INTERRUPT_POSITIVE_EDGE = 1;
    public const byte INTERRUPT_NEGATIVE_EDGE = 2;
    public const byte INTERRUPT_BOTH_EDGES = 3;

    public const byte VREF_VDD = 0;
    public const byte VREF_1024V = 1;
    public const byte VREF_2048V = 2;
    public const byte VREF_4096V = 3;

    public const byte MCP2221_USB_BUS = 0x80;
    public const byte MCP2221_USB_SELF = 0x40;
    public const byte MCP2221_USB_REMOTE = 0x20;

    public const byte MCP2221_PASS_ENABLE = 1;
    public const byte MCP2221_PASS_DISABLE = 0;
    public const byte MCP2221_PASS_CHANGE = 0xFF;

    //------------------------------------------------------------------------
    // Method
    //------------------------------------------------------------------------

    // Open/Close
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr Mcp2221_OpenByIndex(uint VID, uint PID, uint index);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern IntPtr Mcp2221_OpenBySN(uint VID, uint PID, string serialNo);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_Close(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_Reset(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_CloseAll();

    // Library/Enumeration

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetLibraryVersion(char* version);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetConnectedDevices(uint vid, uint pid, out uint noOfDevs);

    // USB descriptors

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetManufacturerDescriptor(IntPtr handle, char* manufacturerString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_SetManufacturerDescriptor(IntPtr handle, string manufacturerString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetProductDescriptor(IntPtr handle, char* productString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_SetProductDescriptor(IntPtr handle, string productString);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetSerialNumberDescriptor(IntPtr handle, char* serialNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_SetSerialNumberDescriptor(IntPtr handle, string serialNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetFactorySerialNumber(IntPtr handle, char* serialNumber);

    // USB

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetVidPid(IntPtr handle, out uint vid, out uint pid);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetVidPid(IntPtr handle, uint vid, uint pid);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetUsbPowerAttributes(IntPtr handle, out byte powerAttributes, out uint currentReq);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetUsbPowerAttributes(IntPtr handle, byte powerAttributes, uint currentReq);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetSerialNumberEnumerationEnable(IntPtr handle, out byte snEnumEnabled);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetSerialNumberEnumerationEnable(IntPtr handle, byte snEnumEnabled);

    // I2C

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cCancelCurrentTransfer(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cRead(IntPtr handle, uint bytesToRead, byte slaveAddress, byte use7bitAddress, byte* i2cRxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cWrite(IntPtr handle, uint bytesToWrite, byte slaveAddress, byte use7bitAddress, byte* i2cTxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetAdvancedCommParams(IntPtr handle, byte timeout, byte maxRetries);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetSpeed(IntPtr handle, uint speed);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cWriteNoStop(IntPtr handle, uint bytesToWrite, byte slaveAddress, byte use7bitAddress, byte* i2cTxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cReadRestart(IntPtr handle, uint bytesToRead, byte slaveAddress, byte use7bitAddress, byte* i2cRxData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_I2cWriteRestart(IntPtr handle, uint bytesToWrite, byte slaveAddress, byte use7bitAddress, byte* i2cTxData);

    // SMBus

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusWriteByte(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusReadByte(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte* readByte);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusWriteWord(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte* data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusReadWord(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte* readData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusBlockWrite(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte byteCount, byte* data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusBlockRead(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte byteCount, byte* readData);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusBlockWriteBlockReadProcessCall(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte command, byte writeByteCount, byte* writeData, byte readByteCount, byte* readData);

    // GPIO

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetInitialPinValues(IntPtr handle, out byte ledUrxInitVal, out byte ledUtxInitVal, out byte ledI2cInitVal, out byte sspndInitVal, out byte usbCfgInitVal);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetInitialPinValues(IntPtr handle, byte ledUrxInitVal, byte ledUtxInitVal, byte ledI2cInitVal, byte sspndInitVal, byte usbCfgInitVal);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetInterruptEdgeSetting(IntPtr handle, byte whichToGet, out byte interruptPinMode);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetInterruptEdgeSetting(IntPtr handle, byte whichToSet, byte interruptPinMode);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_ClearInterruptPinFlag(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetInterruptPinFlag(IntPtr handle, out byte flagValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetClockSettings(IntPtr handle, byte whichToGet, out byte dutyCycle, out byte clockDivider);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetClockSettings(IntPtr handle, byte whichToSet, byte dutyCycle, byte clockDivider);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetAdcData(IntPtr handle, uint* adcDataArray);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetAdcVref(IntPtr handle, byte whichToGet, out byte adcVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetAdcVref(IntPtr handle, byte whichToSet, byte adcVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetDacVref(IntPtr handle, byte whichToGet, out byte dacVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetDacVref(IntPtr handle, byte whichToSet, byte dacVref);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetDacValue(IntPtr handle, byte whichToGet, out byte dacValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetDacValue(IntPtr handle, byte whichToSet, byte dacValue);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetGpioSettings(IntPtr handle, byte whichToGet, byte* pinFunctions, byte* pinDirections, byte* outputValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetGpioSettings(IntPtr handle, byte whichToSet, byte* pinFunctions, byte* pinDirections, byte* outputValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetGpioValues(IntPtr handle, byte* gpioValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetGpioValues(IntPtr handle, byte* gpioValues);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetGpioDirection(IntPtr handle, byte* gpioDir);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetGpioDirection(IntPtr handle, byte* gpioDir);

    // Misc

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetSecuritySetting(IntPtr handle, out byte securitySetting);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetSecuritySetting(IntPtr handle, byte securitySetting, [MarshalAs(UnmanagedType.LPStr)] string currentPassword, [MarshalAs(UnmanagedType.LPStr)] string newPassword);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SendPassword(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string password);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SetPermanentLock(IntPtr handle);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public static extern int Mcp2221_GetHwFwRevisions(IntPtr handle, char* hardwareRevision, char* firmwareRevision);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_GetLastError();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusSendByte(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte data);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern int Mcp2221_SmbusReceiveByte(IntPtr handle, byte slaveAddress, byte use7bitAddress, byte usePec, byte* readByte);
}
