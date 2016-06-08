(function () {

    var app = angular.module('tsi', [
        'ngRoute',
        // Custom modules 
        'edificios',
        'juego',
        'recursos',
        'unidades',
        'aldeas'
    ]);

    angular.module('tsi').config(['$routeProvider', '$locationProvider', configFunction]);


    angular.module('juego').directive("w3TestDirective", function () {
        return {
            restrict : "E",
            template : "<h1>Made by a directive!</h1>"
        };
    });


   


    /*@ngInject*/
    function configFunction($routeProvider, $locationProvider) {
        $locationProvider.html5Mode({ enabled: true, requireBase: false}).hashPrefix('!');  

        // Routes
        $routeProvider.otherwise({ redirectTo: '/' });
    }
})();