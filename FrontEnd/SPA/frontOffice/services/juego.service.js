(function () {
    'use strict';
    angular.module('juego').service('juegoService', ["$http", "$q", juegoService]);

    function juegoService($http, $q) {
        //var jugador = "jugador1";

        this.loginJuego = function(loginJuegoParamas) {
            return $http.post('Home/login', loginJuegoParamas)
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
            return $http.post('Home/register', registerJuegoParams)
            .then(function (response) {
                if (response.data.status)
                    return response.data;
                else
                    return $q.reject('Error al registrarse');
            }, function (response) {
                return $q.reject('Error al registrarse' + reponse);
            })
        }

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