'use strict';
var util = require('util');
var crc8 = require('../tools/crc8.js');
var radioTelegramParser = require('./radioTelegram.js');
var constants = require('./constants.js');
var fs = require('fs');
var path = require('path');
var EventEmitter = require('events').EventEmitter;

var currentPacketState = constants.packetState.GetSync;

var SerialPort = require('serialport').SerialPort;
var serialPort = null;
var isOpen = false;

var packet;
var headerBuffer = new Buffer(5); // Buffer de lecture

var packetByteReceived = 0;
var packetDataReceived = 0;
var packetHeaderCrc = 0;
var packetDataCrc = 0;

var profiles = [];
var profileParsers = [];

var Enocean = function() {
    var self = this;

    initParser();
}

util.inherits(Enocean, EventEmitter);


function Packet() {
    var packetType = 0;
    var dataLentgh = 0;
    var optionalDataLength = 0;
    var data = null;
}

function d2h(d) {
    return (d / 256 + 1 / 512).toString(16).substring(2, 4);
}

Enocean.prototype.registerProfile = function(id, rorg, func, type) {
    profiles[id] = {
        "id": id.toString(16),
        "rorg": d2h(rorg).toUpperCase(),
        "func": d2h(func).toUpperCase(),
        "type": d2h(type).toUpperCase()
    };
    console.log('* equipment ' + id.toString(16) + ' registered');
};

serialPort = new SerialPort('/dev/tty.usbserial-FTXGRHSV', {
    baudRate: 57600,
    dataBits: 8,
    parity: 'none',
    startBits: 1,
    stopBits: 1,
    flowControl: false
});

serialPort.on("open", function () {
    isOpen = true;
    console.log('EnOcean: USB0 port open');
});

serialPort.on('data', function(data) {
    if (!isOpen) {
        return;
    }
    //console.log('on data: ' + data.toString('hex')+' len:'+data.length);

    for (var i = 0; i < data.length; i++) {
        switch (currentPacketState) {
            case constants.packetState.GetSync:
                if (data[i] == 0x55) {
                    packetHeaderCrc = 0;
                    packetDataCrc = 0;
                    packetByteReceived = 0;
                    packetDataReceived = 0;
                    headerBuffer.fill(0x00);
                    currentPacketState = constants.packetState.GetHeader;
                    packet = new Packet();
                }
                break;
            case constants.packetState.GetHeader:
                data.copy(headerBuffer, packetByteReceived, i, i + 1);
                packetHeaderCrc = crc8.proccrc8(packetHeaderCrc, data[i]);
                packetByteReceived++;

                if (packetByteReceived >= 4) {
                    packet.dataLength = headerBuffer.readUInt16BE(0);
                    packet.optionalDataLength = headerBuffer.readUInt8(2);
                    packet.packetType = headerBuffer.readUInt8(3);

                    currentPacketState = constants.packetState.CheckHeaderCRC;
                }
                break;
            case constants.packetState.CheckHeaderCRC:
                packetByteReceived++;

                if (packetHeaderCrc == data[i]) {
                    packet.data = new Buffer(packet.dataLength + packet.optionalDataLength);
                    currentPacketState = constants.packetState.GetData;

                } else {
                    console.log('CRC KO');
                    currentPacketState = packetState.GetSync;
                }
                break;
            case constants.packetState.GetData:
                packetByteReceived++

                data.copy(packet.data, packetDataReceived, i, i + 1);
                packetDataCrc = crc8.proccrc8(packetDataCrc, data[i]);
                packetDataReceived++;
                if (packetDataReceived >= packet.dataLength + packet.optionalDataLength) {
                    currentPacketState = constants.packetState.CheckDataCRC;
                }
                break;
            case constants.packetState.CheckDataCRC:
                packetByteReceived++;
                if (packetDataCrc == data[i]) {
                    parsePacket(packet);
                } else {
                    console.log('Data CRC KO');
                }
                currentPacketState = constants.packetState.GetSync;
                break;
            default:
                currentPacketState = constants.packetState.GetSync;
        }
    }
});

function parsePacket(packet) {
    if (packet.packetType == constants.packetType.Radio) {
        // Parse radiotelegram
        var result = radioTelegramParser.parse(packet);
        // Test if is Teach request
        if (result.isTeach) {
            return;
        }

        var profile = null;

        if (profiles[result.senderId] == null) {
            return;
        }
        profile = profiles[result.senderId];

        var profileId = profile.rorg + profile.func;
        if (profileParsers[profileId] == null) {
            return;
        }
        var parserResult = profileParsers[profileId].parse(packet, result);

        module.exports.emit('data', { sender: result.senderId.toString(16), dbm: result.signalDbm, values: parserResult });
    }
}

function initParser() {
    var profilePath = path.resolve('./enocean/profiles');
    fs.readdirSync(profilePath).forEach(function (fileName) {
        var filePath = path.join(profilePath, fileName);
        if (fs.statSync(filePath).isFile()) {
            var profileId = fileName.substring(0, fileName.lastIndexOf('.')).toUpperCase();
            profileParsers[profileId] = require(filePath);
            console.log('* profile ' + profileId + ' loaded');
        }
    });
}

module.exports = new Enocean();