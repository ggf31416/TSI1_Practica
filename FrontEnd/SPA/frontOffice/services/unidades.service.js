(function () {
    'use strict';
    angular.module('unidades').service('unidadesService', ["$http", "$q", unidadService]);



    function unidadService($http, $q) {


        this.getAllTipoUnidadesSync = function () {
            var ret = "";

            $.ajax({
                url: "/Entidades/GetAllTipoUnidades",
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

        this.getAllTipoUnidades = function () {
            var promise = $http.get("Entidades/GetAllTipoUnidades").then(
                function (data) {
                    if (data.data.success == false) {
                        throw new Error(data.data.responseText);
                    }
                    return data.data.ret;
                }).catch(
                    function (err) {
                        console.log(err);
                        alert(err);
                });
             return promise;
        }

    }
})();