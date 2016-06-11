(function () {
    'use strict';
    angular.module('aldeas').service('aldeasService', ["$http", "$q", aldeaService]);


    function aldeaService($http, $q) {
        this.getAllData = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/Juego/GetAllDataJuego/' + 6)
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