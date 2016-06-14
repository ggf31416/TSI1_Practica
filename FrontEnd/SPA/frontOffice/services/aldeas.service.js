(function () {
    'use strict';
    angular.module('aldeas').service('aldeasService', ["$http", "$q", "$rootScope", aldeaService]);


    function aldeaService($http, $q, $rootScope) {
        this.getAllData = function (tenant) {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetJuegoUsuario/')
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

        this.desarrollarTecnologia = function (json) {
            var ret = "";
            console.debug(JSON.stringify(json));
            $.ajax({
                url: '/' + $rootScope.NombreJuego + '/Tecnologia/DesarrollarTecnologia/',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(json),
                async: false,
                success: function (response) {
                    ret = response;
                },
                error: function (xhr, status, error) {
                    alert("Error al desarrollar tecnologia");
                }
            });
            return ret;
        }
        
        this.getEntidadesActualizadas = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetEntidadesActualizadas/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error al traer datos del juego")
            });

            return promise;

        };
        
        this.getJugadoresAtacables = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetJugadoresAtacables/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error al abandonar clan")
            });

            return promise;

        };

        //CLANES
        this.crearClan = function (json) {
            var ret = "";
            console.debug(JSON.stringify(json));
            $.ajax({
                url: '/' + $rootScope.NombreJuego + '/Juego/CrearClan/',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(json),
                async: false,
                success: function (response) {
                    ret = response;
                },
                error: function (xhr, status, error) {
                    alert("Error al crear clan");
                }
            });
            return ret;
        }

        this.abandonarClan = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/AbandonarClan/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error al abandonar clan")
            });

            return promise;

        };

        this.getJugadoresSinClan = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetJugadoresSinClan/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error al abandonar clan")
            });

            return promise;

        };

        this.soyAdministrador = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/SoyAdministrador/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error")
            });

            return promise;

        };

        this.getJugadoresEnElClan = function () {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('/' + $rootScope.NombreJuego + '/Juego/GetJugadoresEnElClan/')
            .success(function (data) {
                defered.resolve(data.ret);
            })
            .error(function (err) {
                defered.reject("Error")
            });

            return promise;

        };
        
    }
})();