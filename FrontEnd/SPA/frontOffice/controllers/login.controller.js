
(function () {
    'use strict';
    angular.module('juego').controller("loginCtrl", ["$http", "$q", "loginService", '$scope', '$rootScope', '$window',

   

    function ($http, $q, juegoService, $scope, $rootScope, $window) {

        $rootScope.nombreJuego = tenant;
        
        $scope.loginJuego = function (response) {
            if (response.status === 'connected') {
                $scope.clienteId = response.authResponse.userID;
                $scope.token = response.authResponse.accessToken;
                var loginJuegoParams = {
                    "clienteId": $scope.clienteId,
                    "token": $scope.token
                };
                juegoService.loginJuego(loginJuegoParams)
                .then(function (response) {
                    console.debug("http://" + $window.location.host + "/" + $rootScope.nombreJuego + "/Home/Aldea/");
                    $window.location.href = "http://" + $window.location.host + "/" + $rootScope.nombreJuego + "/Home/Aldea/";
                }).catch(function (msjError) {
                    if (msjError === "registrarse") {
                        $('#modalRegistro').modal('show');
                    }
                    
                });
            } else if (response.status === 'not_authorized') {

            } else {

            }
        }

        $scope.submitRegister = function () {
            var registerJuegoParams = {
                "clienteId": $scope.clienteId,
                "token": $scope.token,
                "username": username_register.value,
                "nombre": nombre_register.value,
                "apellido": apellido_register.value
            };
            juegoService.registerJuego(registerJuegoParams)
                .then(function (response) {
                    $window.location.href = "http://" + $window.location.host + "/" + $rootScope.nombreJuego + "/Home/Aldea/";
                }).catch(function (msjError) {
                    console.log(msjError);
                });
        }
    }])
})();