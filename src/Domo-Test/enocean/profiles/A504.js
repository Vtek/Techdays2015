'use strict';

exports.parse = function (packet, telegram) {
    var humidity = 0.4 * packet.data[2];
    var temperature = 0.16 * packet.data[3];

    return { temp: temperature, humi: humidity };
};