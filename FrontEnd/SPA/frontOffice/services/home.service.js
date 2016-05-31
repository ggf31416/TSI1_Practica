(function () {
    'use strict';
    angular.module('home').factory('homeService', ["$http", "$q", homeService, function ($http, $q) {
        return {
            loginJuego : function(loginJuegoJson) {
                return $http.post('Login/login', loginJuegoJson)
                .then(function(response) {
                    return response.data;
                }, function(response) {
                    return $q.reject('Error de login');
                })
            }
        };
    }
    ]);
})