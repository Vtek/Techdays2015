'use strict';

var serialport = require("serialport").SerialPort;
var enocean = require("./enocean/enocean.js");

var eventHub = require('event-hub-client').restClient(
    'serviceBusNamespace',
    'eventHubName',
    'sharedAccessKeyName',
    'sharedAccessKey');

enocean.registerProfile(0x0180BBC2, 0xA5, 0x04, 0x01);
enocean.registerProfile(0x1806F82, 0xD5, 0x00, 0x01);
enocean.registerProfile(0x258651, 0xF6, 0x02, 0x01);

enocean.on('data', function(data) {
    for (var key in data.values) {
        var msg = { deviceid: 0, datas: null, timestamp: new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') };

        switch (key) {
            case 'pressed':
                msg.deviceid = data.sender + '-3';
                msg.datas = data.values.pressed;
                break;
            case 'humi':
                msg.deviceid = data.sender + '-2';
                msg.datas = data.values.humi.toFixed(1);
                break;
            case 'temp':
                msg.deviceid = data.sender + '-1';
                msg.datas = data.values.temp.toFixed(1);
                break;
            case 'open':
                msg.deviceid = data.sender + '-4';
                msg.datas = data.values.open;
                break;
            default:
                continue;
        }

        var msgJson = JSON.stringify(msg);
        console.log(msgJson);
        eventHub.sendMessage(msgJson, function (err) {
            if (err != null) {
                console.log(err);
            }
        });
    }
});
