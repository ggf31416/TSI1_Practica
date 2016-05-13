(function () {
    'use strict';
    angular.module('juego').controller("juegoCtrl", ["juegoService", "edificiosService", '$scope', '$rootScope',


    function (juegoService, edificiosService, $scope, $rootScope) {

        $rootScope.nombreJuego = "Atlas2";


        $scope.posicionarEdificio = function (nombre) {
            // llama a la funcion iniciarCustomDrag en el CrearCanvas.js
            iniciarCustomDrag(nombre);
        };

        $rootScope.listaEdificios = edificiosService.getAllTipoEdificios();
            /*[
                {
                    Nombre: "Cuartel",
                    Imagen: "/SPA/backOffice/cuartel.jpg",
                    Vida: 500,
                    Ataque: 12,
                    Defensa: 30,
                    TiempoConstruccion: "20/s"
                },
                {
                    Nombre: "Granja",
                    Imagen: "/SPA/backOffice/granja.jpg",
                    Vida: 450,
                    Ataque: 0,
                    Defensa: 10,
                    TiempoConstruccion: "100/s"
                }
        ];*/

        $rootScope.listaUnidades = [
                {
                    Nombre: "Soldado",
                    Imagen: "/SPA/backOffice/soldado.jpg",
                    Vida: 500,
                    Ataque: 12,
                    Defensa: 30,
                    TiempoConstruccion: "20/s"
                },
                {
                    Nombre: "Arquero",
                    Imagen: "/SPA/backOffice/arquero.jpg",
                    Vida: 450,
                    Ataque: 0,
                    Defensa: 10,
                    TiempoConstruccion: "100/s"
                }
        ];

        $rootScope.listaRecursos = [
                {
                    Nombre: "Recurso1",
                    Imagen: "/SPA/backOffice/soldado.jpg"
                },
                {
                    Nombre: "Recurso2",
                    Imagen: "/SPA/backOffice/arquero.jpg"
                }
        ];
    }
    ]);


    angular.module('juego').directive('gameCanvas', function ($injector) {
        var linkFn = function (scope, ele, attrs) {
            window.createGame(scope, $injector);
        };

        return {
            scope: true,
            templateUrl: "/SPA/frontOffice/directives/templateGameCanvas.html",
            link: linkFn
        }
    });
})();