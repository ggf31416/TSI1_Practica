(function () {
    'use strict';
    angular.module('home').controller("homeCtrl", ["homeService", '$scope', '$rootScope',

        function (homeService, $scope, $rootScope) {

            //--------------Inicializacion de variables---------------------
            $scope.title = "Recursos";

            $scope.lista = $rootScope.listaRecursos;

            $scope.showAsociationDialog = false;

            $scope.entidad = {};

            //--------------Fin inicializacion de variables---------------------
            $scope.loginJuego = function () {
                homeService.loginJuego(loginJuegoParams)
                .then(function (response) {

                }).catch(function (msjError) {

                });
            }

        }
    ]);

})();