(function () {

    var app = angular.module('tsi', [
        'ngRoute',
        // Custom modules 
        'edificios',
        'juego',
        'recursos',
        'unidades'
    ]);

    angular.module('tsi').config(['$routeProvider', '$locationProvider', configFunction]);

    angular.module('juego').directive("w3TestDirective", function () {
        return {
            restrict : "E",
            template : "<h1>Made by a directive!</h1>"
        };
    });


    angular.module('juego').directive('gameCanvas', function ($injector) {
        var linkFn = function (scope, ele, attrs) {
            // link Function
        };


        return {
            scope: {
                players: '=',
                mapId: '='
            },
            template: '<div id="divJuego"  ondrop = "drop(event)" ondragover = "allowDrop(event)"><script>window.createGame()</script></div>',
            link: linkFn
        }
    });

    app.controller('StudentController', function ($scope) {
        $scope.Mahesh = {};
        $scope.Mahesh.name = "Mahesh Parashar";
        $scope.Mahesh.rollno = 1;

        $scope.Piyush = {};
        $scope.Piyush.name = "Piyush Parashar";
        $scope.Piyush.rollno = 2;
    });

    app.directive('student', function () {
        //define the directive object
        var directive = {};

        //restrict = E, signifies that directive is Element directive
        directive.restrict = 'E';

        //template replaces the complete element with its text. 
        directive.template = "Student: <b>{{student.name}}</b> , Roll No: <b>{{student.rollno}}</b>";

        //scope is used to distinguish each student element based on criteria.
        directive.scope = {
            student: "=name"
        }

        //compile is called during application initialization. AngularJS calls it once when html page is loaded.

        directive.compile = function (element, attributes) {
            element.css("border", "1px solid #cccccc");

            //linkFunction is linked with each element with scope to get the element specific data.
            var linkFunction = function ($scope, element, attributes) {
                element.html("Student: <b>" + $scope.student.name + "</b> , Roll No: <b>" + $scope.student.rollno + "</b><br/>");
                element.css("background-color", "#ff00ff");
            }
            return linkFunction;
        }
        return directive;
    });

    /*@ngInject*/
    function configFunction($routeProvider, $locationProvider) {
        $locationProvider.html5Mode({ enabled: true, requireBase: false}).hashPrefix('!');  

        // Routes
        $routeProvider.otherwise({ redirectTo: '/' });
    }
})();