namespace MCP2221ManagedWrapper;

public enum Mcp2221Status
{
    NoError = NativeMethods.E_NO_ERR,

    UnknownError = NativeMethods.E_ERR_UNKOWN_ERROR,
    CmdFailed = NativeMethods.E_ERR_CMD_FAILED,
    InvalidHandle = NativeMethods.E_ERR_INVALID_HANDLE,
    InvalidParameter = NativeMethods.E_ERR_INVALID_PARAMETER,
    InvalidPass = NativeMethods.E_ERR_INVALID_PASS,
    PasswordLimitReached = NativeMethods.E_ERR_PASSWORD_LIMIT_REACHED,
    FlashWriteProtected = NativeMethods.E_ERR_FLASH_WRITE_PROTECTED,

    Null = NativeMethods.E_ERR_NULL,
    DestinationTooSmall = NativeMethods.E_ERR_DESTINATION_TOO_SMALL,
    InputTooLarge = NativeMethods.E_ERR_INPUT_TOO_LARGE,
    FlashWriteFailed = NativeMethods.E_ERR_FLASH_WRITE_FAILED,
    Malloc = NativeMethods.E_ERR_MALLOC,

    NoSuchIndex = NativeMethods.E_ERR_NO_SUCH_INDEX,
    DeviceNotFound = NativeMethods.E_ERR_DEVICE_NOT_FOUND,
    InternalBufferTooSmall = NativeMethods.E_ERR_INTERNAL_BUFFER_TOO_SMALL,
    OpenDeviceError = NativeMethods.E_ERR_OPEN_DEVICE_ERROR,
    ConnectionAlreadyOpened = NativeMethods.E_ERR_CONNECTION_ALREADY_OPENED,
    CloseFailed = NativeMethods.E_ERR_CLOSE_FAILED,

    InvalidSpeed = NativeMethods.E_ERR_INVALID_SPEED,
    SpeedNotSet = NativeMethods.E_ERR_SPEED_NOT_SET,
    InvalidByteNumber = NativeMethods.E_ERR_INVALID_BYTE_NUMBER,
    InvalidAddress = NativeMethods.E_ERR_INVALID_ADDRESS,
    I2cBusy = NativeMethods.E_ERR_I2C_BUSY,
    I2cReadError = NativeMethods.E_ERR_I2C_READ_ERROR,
    AddressNack = NativeMethods.E_ERR_ADDRESS_NACK,
    Timeout = NativeMethods.E_ERR_TIMEOUT,
    TooManyRxBytes = NativeMethods.E_ERR_TOO_MANY_RX_BYTES,
    CopyRxDataFailed = NativeMethods.E_ERR_COPY_RX_DATA_FAILED,
    NoEffect = NativeMethods.E_ERR_NO_EFFECT,
    CopyTxDataFailed = NativeMethods.E_ERR_COPY_TX_DATA_FAILED,
    InvalidPec = NativeMethods.E_ERR_INVALID_PEC,
    BlockSizeMismatch = NativeMethods.E_ERR_BLOCK_SIZE_MISMATCH,

    RawTxTooLarge = NativeMethods.E_ERR_RAW_TX_TOO_LARGE,
    RawTxCopyFailed = NativeMethods.E_ERR_RAW_TX_COPYFAILED,
    RawRxCopyFailed = NativeMethods.E_ERR_RAW_RX_COPYFAILED
}
