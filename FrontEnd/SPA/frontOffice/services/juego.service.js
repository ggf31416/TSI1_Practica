(function () {
    'use strict';
    angular.module('juego').service('juegoService', ["$http", "$q", juegoService]);

    function juegoService($http, $q) {
        //var jugador = "jugador1";

        function postAccion(json) {
            return $http({
                method: 'POST',
                dataType: 'text',
                url: "Tablero/Accion",
                data: { data: JSON.stringify(json) }
            });
        }

        this.crearEdificioEnTablero = function (id, input_x, input_y,jugador) {
            //return $http.post("Tablero/JugarUnidad", JSON.stringify({ "Id": id, "PosX": input_x, "PosY": input_y }));
            return postAccion({ "A": "AddEd", "J": jugador, "Id": id, "PosX": input_x, "PosY": input_y });
        }



        this.posicionarUnidad = function (idTipo, idUnidad,input_x, input_y,jugador){
            return postAccion({ "A": "AddUnidad", "J": jugador, "Id": idTipo, "IdUn": idUnidad, "PosX": input_x, "PosY": input_y });
        }

        this.moverUnidad = function (id, input_x, input_y,jugador){
            return $http.post("Tablero/Accion", JSON.stringify({ "A": "MoveUnidad", "J": jugador , "Id": id, "PosX": input_x, "PosY": input_y}));
        }

        this.registrarJugador = function (jugador,juego) {
            return $http.post("Tablero/Accion", JSON.stringify({ "A": "RegistrarJugador", "J": jugador }));
        }

        
    }

})();