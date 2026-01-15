namespace MCP2221ManagedWrapper;

using static MCP2221ManagedWrapper.NativeMethods;

// ReSharper disable InconsistentNaming
public enum Mcp2221Status
{
    NoError = E_NO_ERR,

    UnknownError = E_ERR_UNKOWN_ERROR,
    CmdFailed = E_ERR_CMD_FAILED,
    InvalidHandle = E_ERR_INVALID_HANDLE,
    InvalidParameter = E_ERR_INVALID_PARAMETER,
    InvalidPass = E_ERR_INVALID_PASS,
    PasswordLimitReached = E_ERR_PASSWORD_LIMIT_REACHED,
    FlashWriteProtected = E_ERR_FLASH_WRITE_PROTECTED,

    Null = E_ERR_NULL,
    DestinationTooSmall = E_ERR_DESTINATION_TOO_SMALL,
    InputTooLarge = E_ERR_INPUT_TOO_LARGE,
    FlashWriteFailed = E_ERR_FLASH_WRITE_FAILED,
    Malloc = E_ERR_MALLOC,

    NoSuchIndex = E_ERR_NO_SUCH_INDEX,
    DeviceNotFound = E_ERR_DEVICE_NOT_FOUND,
    InternalBufferTooSmall = E_ERR_INTERNAL_BUFFER_TOO_SMALL,
    OpenDeviceError = E_ERR_OPEN_DEVICE_ERROR,
    ConnectionAlreadyOpened = E_ERR_CONNECTION_ALREADY_OPENED,
    CloseFailed = E_ERR_CLOSE_FAILED,

    InvalidSpeed = E_ERR_INVALID_SPEED,
    SpeedNotSet = E_ERR_SPEED_NOT_SET,
    InvalidByteNumber = E_ERR_INVALID_BYTE_NUMBER,
    InvalidAddress = E_ERR_INVALID_ADDRESS,
    I2cBusy = E_ERR_I2C_BUSY,
    I2cReadError = E_ERR_I2C_READ_ERROR,
    AddressNack = E_ERR_ADDRESS_NACK,
    Timeout = E_ERR_TIMEOUT,
    TooManyRxBytes = E_ERR_TOO_MANY_RX_BYTES,
    CopyRxDataFailed = E_ERR_COPY_RX_DATA_FAILED,
    NoEffect = E_ERR_NO_EFFECT,
    CopyTxDataFailed = E_ERR_COPY_TX_DATA_FAILED,
    InvalidPec = E_ERR_INVALID_PEC,
    BlockSizeMismatch = E_ERR_BLOCK_SIZE_MISMATCH,

    RawTxTooLarge = E_ERR_RAW_TX_TOO_LARGE,
    RawTxCopyFailed = E_ERR_RAW_TX_COPYFAILED,
    RawRxCopyFailed = E_ERR_RAW_RX_COPYFAILED
}
// ReSharper restore InconsistentNaming
