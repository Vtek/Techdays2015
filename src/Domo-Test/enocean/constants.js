'use strict';

exports.telegramType =
{
    "RepeatedSwitch": 0xF6,
    "OneByteSensor": 0xD5,
    "FourByteSensor": 0xA5
};


exports.packetState =
{
    "GetSync": 1,
    "GetHeader": 2,
    "CheckHeaderCRC": 3,
    "GetData": 4,
    "CheckDataCRC": 5
};

exports.packetType =
{
    "Radio": 0x01,
    "Response": 0x02,
    "RadioSubTel": 0x03,
    "Event": 0x04,
    "CommonCommand": 0x05,
    "SmartAckCommand": 0x06,
    "RemoteManCommand": 0x07,
    "RadioMessage": 0x08,
    "RadioAdvanced": 0x0A
};

