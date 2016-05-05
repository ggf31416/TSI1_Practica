(function () {
    'use strict';
    angular.module('recursos').service('recursosService', ["$http", "$q", recursoService]);

    function recursoService($http, $q) {
        this.getAllTipoRecursos = function () {
            var ret = "";

            $.ajax({
                url: "Entidades/GetAllTipoRecursos",
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