'use strict';

exports.parse = function (packet, telegram) {
    var isOpen = (packet.data[1] & 0x01);

    return { open: (isOpen == 1 ? 0 : 1) };
};