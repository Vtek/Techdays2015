(function (angular) {
    'use strict';

    angular.module('app.directives')
        .directive('dashChart', dashChart);

    function dashChart() {
        return {
            restrict: 'E',
            scope: {
                model: '=',
                maxItem: '@',
                width: '@',
                height: '@'
            },
            template: '<div style="display: inline-block;"></div>',
            replace: true,
            link: link
        };

        function link(scope, element) {
            var lineChart = new google.visualization.AreaChart(element[0]),
                maxItem = parseInt(scope.maxItem) || 10,
                width = parseInt(scope.width),
                height = parseInt(scope.height);

            var chartOptions = {
                legend: 'none',
                width: width === +width ? width : 1500, // Check if width is a Number
                height: height === +height ? height : 600, // Check if height is a Number
                lineWidth: 1,
                colors: ['#15A0C8']
            };

            scope.$watch('model.data', chartDataListener, true);

            function draw(data) {
                var table = new google.visualization.DataTable();
                table.addColumn('datetime');
                table.addColumn('number');
                table.addRows(data.length);

                var view = new google.visualization.DataView(table);
                var axisCoord = {
                    h: {
                        min: undefined,
                        max: undefined
                    },
                    v: {
                        min: undefined,
                        max: undefined
                    }
                };

                var value;
                for (var i = 0; i < data.length; i++) {
                    value = data[i].value;
                    table.setCell(i, 0, data[i].date);
                    table.setCell(i, 1, value);

                    if (axisCoord.v.min > value) {
                        axisCoord.v.min = value;
                    }
                    if (axisCoord.v.max < value) {
                        axisCoord.v.max = value;
                    }
                }

                if (data.length > 0) {
                    var last = data[data.length - 1];
                    axisCoord.h.max = last.date;
                    axisCoord.h.min = new Date(last.date.getTime() - maxItem * 1000);
                }
                else {
                    axisCoord.h.max = axisCoord.h.min = new Date();
                }

                axisCoord.v.min = (axisCoord.v.min >= 5) ? axisCoord.v.min - 5 : 0;
                axisCoord.v.max += 5;

                chartOptions.vAxis = {
                    title: scope.model.deviceName + ' (' + scope.model.deviceUnit + ')',
                    minValue: axisCoord.v.min,
                    maxValue: axisCoord.v.max
                };

                chartOptions.hAxis = {
                    title: 'Time',
                    viewWindow: {
                        min: axisCoord.h.min,
                        max: axisCoord.h.max
                    }
                };

                lineChart.draw(view, chartOptions);
            }

            function chartDataListener(data) {
                if (data) {
                    draw(data);
                }
            }
        }
    }
})(angular);