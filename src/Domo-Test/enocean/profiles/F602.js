'use strict';

exports.parse = function (packet, telegram) {
    var result = {};

    if (telegram.rpsT21) {
        var pressedButton = (packet.data[1] & 0xE0) >> 5;
        if (telegram.rpsNu) {
            result.action = 'pressed';
            if (pressedButton == 3) {
                result.pressed = 1;
            } else {
                result.pressed = 0;
            }
        } else {
            result.action = 'release';
        }
    }

    return result;
};