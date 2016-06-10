(function () {
'use strict';
angular.module('aldeas').controller("aldeaCtrl", ["$http", "$q", "aldeasService",
        "edificiosService", "unidadesService", '$scope', '$rootScope',

    function ($http, $q, aldeasService, edificiosService, unidadesService, $scope, $rootScope) {

        //--------------Inicializacion de variables---------------------
        $rootScope.tablero = [
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 1, Imagen: "/SPA/backOffice/ImagenesSubidas/cuartel.jpg" }, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] },
            { cols: [{ Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}, { Id: 0, Imagen: "/SPA/backOffice/ImagenesSubidas/pasto.jpg"}] }
        ]

        $rootScope.listaRecursos = [
               {
                   Nombre: "Oro",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/oro.jpg",
                   Valor: 134
               },
               {
                   Nombre: "Madera",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/madera.jpg",
                   Valor: 546
               }
        ];

        $q.all([
            edificiosService.getAllTipoEdificios(),
            unidadesService.getAllTipoUnidades()
        ]).then(function (data) {
            $rootScope.listaEdificios = data[0];
            $rootScope.listaUnidades = data[1];
           // window.createGame();
        });

        //--------------Fin inicializacion de variables---------------------

        function findEdificioInArray(lista, id) {
            if (lista != null)
                return jQuery.grep(lista, function (value) {
                    return value.Id == id;
                });
            else return [];
        }
        $scope.entidad = null;
        $scope.editCell = function (casilla) {
            $scope.editCasilla = casilla;
            var id = casilla.Id;
            if(id == 0){
                //Abro cuadro para contruir
                $('#dialogoConstruir').modal('show');
            }else{
                //abro cuadro del edificio
                $scope.entidad = findEdificioInArray($rootScope.listaEdificios, id)[0];
                $('#dialogoDatosEdificio').modal('show');
            }
        }

        $scope.addEdificio = function (id) {
            var edificio = findEdificioInArray($rootScope.listaEdificios, id)[0];
            $scope.editCasilla.Id = edificio.Id;
            $scope.editCasilla.Imagen = edificio.Imagen;
            $('#dialogoConstruir').modal('hide');
        }


    }
]);
})();