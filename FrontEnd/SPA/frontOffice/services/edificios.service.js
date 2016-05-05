(function () {
    'use strict';
    angular.module('edificios').service('edificiosService', ["$http", "$q", edificioService]);



    function edificioService($http, $q) {
        this.getAllTipoEdificios = function () {
            var ret = "";

            $.ajax({
                url: "/Entidades/GetAllTipoEdificios",
                type: 'GET',
                async: false,
                success: function (response) {
                    //ret = response;
                    console.debug(response.ret);
                    ret = response.ret;
                },
                error: function (xhr, status, error) {
                    alert("Error al traer resultados");
                }
            });
            return ret;
        };

        this.altaTipoEntidad = function (entidad) {
            var ret = "";
            console.debug(JSON.stringify(entidad));
            $.ajax({
                url: "Entidades/altaTipoEntidad",
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(entidad),
                async: false,
                success: function (response) {
                    //ret = response;
                    ret= response;
                },
                error: function (xhr, status, error) {
                    alert("Error al crear edificio");
                }
            });
            return ret;
        }
    }
})();