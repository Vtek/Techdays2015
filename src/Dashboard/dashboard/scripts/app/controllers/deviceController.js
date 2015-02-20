(function (angular) {
    'use strict';

    angular.module('app.controllers')
        .controller('DeviceController', ['Hub', '$scope', DeviceController]);

    function DeviceController(Hub, $scope) {
        $scope.selectedDevice = null;
        $scope.deviceRecordModel = {};
        $scope.doorAlert = false;
        $scope.switchAlert = false;

        $scope.getLastTemperature = getLastTemperature;

        var targetHub = 'dashHub',
            temperatureDevice = 'Température';

        new Hub(targetHub, {
            listeners: {
                broadcastDeviceData: broadcastDeviceData,
                broadcastAlert: broadcastAlert,
                broadcastAverageJson: broadcastAverageJson,
                broadcastDeviceDataJson: broadcastDeviceDataJson,
                broadcastAlertJson: broadcastAlertJson,
                broadcastInformation: broadcastInformation
            },
            errorHandler: function (error) {
                console.error(error);
            }
        });

        function broadcastDeviceDataJson(message) {
            console.log('Storm DeviceData : ' + message);
        }

        function broadcastAverageJson(message) {
            console.log('Storm AverageData : ' + message);
        }
        
        function broadcastAlertJson(message) {
            console.log('Storm AlertData : ' + message);
        }

        function broadcastInformation(message) {
            console.log('Storm Bolt information : ' + message);
        }

        function broadcastDeviceData(message) {
            var data = JSON.parse(message);
            if (typeof data === 'undefined' || data === null)
                return;

            var deviceName = data.deviceName,
                deviceId = data.deviceId,
                deviceUnit = data.deviceUnit,
                chartData = {
                    date: new Date(data.date),
                    value: data.data
                };

            $scope.$apply(function () {
                if (typeof $scope.deviceRecordModel[deviceId] === 'undefined') {
                    $scope.deviceRecordModel[deviceId] = {
                        deviceId: deviceId,
                        deviceName: deviceName,
                        deviceUnit: deviceUnit,
                        data: []
                    };
                }

                var length = $scope.deviceRecordModel[deviceId].data.length;
                if (length > 0 && $scope.deviceRecordModel[deviceId].data[length - 1].date > chartData.date)
                    return;
                $scope.deviceRecordModel[deviceId].data.push(chartData);
            });
        }

        function broadcastAlert(message) {
            var data = JSON.parse(message);
            if (typeof data === 'undefined' || data === null)
                return;

            if (isBoolean(data.isActive)) {
                if (data.deviceName === 'Porte') {
                    $scope.doorAlert = data.isActive;
                }
                else if (data.deviceName === 'Interrupteur') {
                    $scope.switchAlert = data.isActive;
                }
            }
        }

        function getLastTemperature() {
            for (var property in $scope.deviceRecordModel) {
                if ($scope.deviceRecordModel[property] &&
                    $scope.deviceRecordModel[property].deviceName === temperatureDevice &&
                    $scope.deviceRecordModel[property].data.length > 0) {
                    return $scope.deviceRecordModel[property].data[$scope.deviceRecordModel[property].data.length - 1].value;
                }
            }
            return null;
        }

        function isBoolean(obj) {
            return obj === true || obj === false || toString.call(obj) === '[object Boolean]';
        }
    }
})(angular);