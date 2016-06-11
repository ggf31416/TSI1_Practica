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
    }
})();