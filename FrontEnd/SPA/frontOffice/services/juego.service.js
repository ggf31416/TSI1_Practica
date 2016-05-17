(function () {
    'use strict';
    angular.module('juego').service('juegoService', ["$http", "$q", juegoService]);

    function juegoService($http, $q) {
        this.crearEdificioEnTablero = function (id, input_x, input_y) {
            var ret = false;
            $.ajax({
                url: "Tablero/JugarUnidad",
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ "Id": id, "PosX": input_x, "PosY": input_y }),
                async: false,
                success: function (response) {
                    //ret = response;
                    ret = true;
                },
                error: function (xhr, status, error) {
                    ret = false;
                }
            });
            return ret;
        }
    }
})();