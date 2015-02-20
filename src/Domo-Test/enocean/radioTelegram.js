'use strict';

var constants = require("./constants.js");

exports.parse = function(packet) {
    var result = {
        isTeach: false,
        type: null,
        senderId: 0,
        signalDbm: 0,
        destinationId: 0,
        repeaterCount: 0,
        rpsT21: 0,
        rpsNu: 0
    };

    parseCommon(packet, result);

    switch (packet.data[0]) {
        case constants.telegramType.RepeatedSwitch:
            result.senderId = parseSenderId(packet.data, 2);
            parseStatus(packet.data, 6, result);
            break;
        case constants.telegramType.FourByteSensor:
            result.senderId = parseSenderId(packet.data, 5);
            result.isTeach = (packet.data[4] & 0x8) == 0;
            parseStatus(packet.data, 6, result);
            break;
        case constants.telegramType.OneByteSensor:
            result.senderId = parseSenderId(packet.data, 2);
            result.isTeach = (packet.data[1] & 0x8) == 0;
            parseStatus(packet.data, 9, result);
            break;
    }

    return result;
}

function parseCommon(packet, result) {
    if (packet.optionalDataLength <= 0) {
        return;
    }

    var offset = packet.dataLength;
    if (packet.optionalDataLength < 5) {
        return;
    }
    result.destinationId = packet.data.readUInt32BE(offset + 1);

    if (packet.optionalDataLength < 6) {
        return;
    }
    result.signalDbm = packet.data.readUInt8(offset + 5);
}

function parseSenderId(data, offset) {
    return data.readUInt32BE(offset);
}

function parseStatus(data, offset, result) {
    result.rpsT21 = (data[offset] & 0x20) != 0;
    result.rpsNu = (data[offset] & 0x10) != 0;
    result.repeaterCount = data[offset] & (0x07);
}