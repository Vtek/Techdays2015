'use strict';

var eventHub = require('event-hub-client').restClient(
    'serviceBusNamespace',
    'eventHubName',
    'sharedAccessKeyName',
    'sharedAccessKey');

function sendTestData2() {
    sendData('{"deviceid":"258651-3","datas":0,"timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"1806f82-4","datas":0,"timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"180bbc2-1","datas":"30","timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"180bbc2-2","datas":"20","timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    setTimeout(sendTestData, 2000);
}

function sendTestData() {
    sendData('{"deviceid":"258651-3","datas":1,"timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"1806f82-4","datas":1,"timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"180bbc2-1","datas":"25.4","timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    sendData('{"deviceid":"180bbc2-2","datas":"96.8","timestamp":"' + new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '') + '"}');
    setTimeout(sendTestData2, 2000);
}

function sendData(msg) {
    console.log(msg);
    eventHub.sendMessage(msg, function (err) {
        if (err != null) {
            console.log(err);
        }
    });
}

setTimeout(sendTestData, 1000);