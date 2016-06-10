(function () {
    'use strict';
    angular.module('juego').service('juegoService', ["$http", "$q", juegoService]);

    function juegoService($http, $q) {
        //var jugador = "jugador1";

        this.loginJuego = function(loginJuegoJson) {
            return $http.post('Home/login', loginJuegoJson)
            .then(function(response) {
                return response.data;
            }, function(response) {
                return $q.reject('Error de login');
            })
        }

        function postAccion(json) {
            return $http({
                method: 'POST',
                dataType: 'text',
                url: "/Tablero/Accion",
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

        this.construirUnidad = function (idTipo,jugador){
            return postAccion({ "A": "BU", "J": jugador, "Id": idTipo});
        }

        this.moverUnidad = function (id, input_x, input_y,jugador){
            return $http.post("/Tablero/Accion", JSON.stringify({ "A": "MoveUnidad", "J": jugador , "Id": id, "PosX": input_x, "PosY": input_y}));
        }

        this.registrarJugador = function (jugador,juego) {
            return $http({
                method: 'POST',
                dataType: 'text',
                url: "/Tablero/RegistrarJugador",
                data: { "jugador": jugador }
            });
        }


        this.getListaEnemigos = function (jugador,juego) {
            var promise = $http.get("/Tablero/GetListaDeJugadoresAtacables",{params: {"jugador": jugador}}).then(
                function (data) {
                    if (data.data.success == false) {
                        throw new Error(data.data.responseText);
                        console.log("Error al cargar enemigos atacables: " + data.data.responseText + " msg" + data.data.msg);
                    }
                    return data.data.ret;
                }).catch(
                    function (err) {
                        console.log("Error al cargar enemigos atacables: " + err);
                });
             return promise;
        }

        this.iniciarAtaque = function(ataqueJson){
             return $http.post("/Tablero/iniciarAtaque", JSON.stringify(ataqueJson));


        }

        
    }

})();