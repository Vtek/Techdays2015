(function (global) {
    $(global.document).ready(onDocumentReady);

    function onDocumentReady() {
        if (typeof $.connection === 'undefined') {
            console.error('$.connection does not exist.')
            return;
        }
        if (typeof $.connection.dashHub === 'undefined') {
            console.error('$.connection.dashHub does not exist.')
            return;
        }

        var hub = $.connection.dashHub;

        if (typeof hub.client === 'undefined') {
            console.error('hub.client does not exist.')
            return;
        }

        console.log('Attach to broadcast message.');
        // add callback to broadcasted messages
        hub.client.broadcastMessage = broadcastListener;

        console.log('Start hub connection.');
        $.connection.hub.start();

        function broadcastListener(message) {
            console.log(arguments);
        };
    };
})(window);