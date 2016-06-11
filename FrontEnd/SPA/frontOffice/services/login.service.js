(function () {
    'use strict';
    angular.module('juego').service('loginService', ["$http", "$q", "$rootScope", loginService]);

    function loginService($http, $q, $rootScope) {
        //var jugador = "jugador1";

        this.loginJuego = function(loginJuegoParamas) {
            return $http.post('/'+$rootScope.nombreJuego + '/Home/login', loginJuegoParamas)
            .then(function (response) {
                if (response.data.status)
                    return response.data;
                else
                    return $q.reject('registrarse');
            }, function(response) {
                return $q.reject('Error de login');
            })
        }

        this.registerJuego = function (registerJuegoParams) {
            return $http.post('/' + $rootScope.nombreJuego + '/Home/register', registerJuegoParams)
            .then(function (response) {
                if (response.data.status)
                    return response.data;
                else
                    return $q.reject('Error al registrarse');
            }, function (response) {
                return $q.reject('Error al registrarse' + reponse);
            })
        }
    }
})();