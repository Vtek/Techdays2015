(function (angular) {
    'use strict';

    angular.module('app.directives')
        .directive('dashLevel', dashLevel);

    function dashLevel() {
        return {
            restrict: 'E',
            scope: {
                model: '=',
                minLevel: '=',
                maxLevel: '=',
                label: '@',
            },
            replace: true,
            link: link
        };

        function link(scope, element) {
            var config = {
                size: 250,
                label: scope.label,
                min: typeof scope.minLevel !== 'undefined' ? scope.minLevel : 0,
                max: typeof scope.maxLevel !== 'undefined' ? scope.maxLevel : 100,
                minorTicks: 5
            };

            var range = config.max - config.min;
            if (range <= 0)
                return;

            config.warningArea = [
                {
                    from: config.min + range * 0.75,
                    to: config.min + range * 0.9
                }
            ];
            config.alertArea = [
                {
                    from: config.min + range * 0.9,
                    to: config.max
                }
            ];

            var level = new Level(element[0], config);
            level.render();

            scope.$watch('model', function (newValue) {
                if (newValue && level) {
                    level.redraw(newValue);
                }
            });
        }
    }
})(angular);