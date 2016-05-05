(function () {
    'use strict';
    angular.module('unidades').service('unidadesService', ["$http", "$q", unidadService]);

    function unidadService($http, $q) {
        this.getAllTipoUnidades = function () {
            var ret = "";

            $.ajax({
                url: "Entidades/GetAllTipoUnidades",
                type: 'GET',
                //async: false,
                success: function (response) {
                    //ret = response;
                    return response;
                },
                error: function (xhr, status, error) {
                    alert("Error");
                }
            });
        }
    }
})();