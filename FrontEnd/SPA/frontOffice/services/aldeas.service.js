(function () {
    'use strict';
    angular.module('aldeas').service('aldeasService', ["$http", "$q", "$rootScope", aldeaService]);


    function aldeaService($http, $q, $rootScope) {
        this.getAllData = function (tenant) {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetAllDataJuego/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error al traer datos del juego")
            });

            return promise;

        };

        this.construirEdificio = function (json) {
            var ret = "";
            console.debug(JSON.stringify(json));
            $.ajax({
                url: '/' + $rootScope.NombreJuego + '/Tablero/ConstruirEdificio/',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(json),
                async: false,
                success: function (response) {
                    ret = response;
                },
                error: function (xhr, status, error) {
                    alert("Error al crear edificio");
                }
            });
            return ret;
        }

        this.entrenarUnidades = function (json) {
            var ret = "";
            console.debug(JSON.stringify(json));
            $.ajax({
                url: '/' + $rootScope.NombreJuego + '/Tablero/EntrenarUnidad/',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(json),
                async: false,
                success: function (response) {
                    ret = response;
                },
                error: function (xhr, status, error) {
                    alert("Error al crear edificio");
                }
            });
            return ret;
        }
        
    }
})();