(function () {
    'use strict';
    angular.module('juego').service('juegoService', ["$http", "$q", juegoService]);

    function juegoService($http, $q) {
        this.crearEdificioEnTablero = function (id, input_x, input_y) {
            return $http.post("Tablero/JugarUnidad", JSON.stringify({ "Id": id, "PosX": input_x, "PosY": input_y }));
        }
    }
})();