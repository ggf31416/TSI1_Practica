(function () {
'use strict';
angular.module('aldeas').controller("aldeaCtrl", ["$http", "$q", "aldeasService",
        "edificiosService", "unidadesService", '$scope', '$rootScope',

    function ($http, $q, aldeasService, edificiosService, unidadesService, $scope, $rootScope) {

        //--------------Inicializacion de variables---------------------
        /*$rootScope.tablero = [
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
        */
        $scope.initVariables = function () {
            $rootScope.NombreJuego = tenant;
            console.debug($rootScope.NombreJuego);
            aldeasService.getAllData($rootScope.NombreJuego)
                                    .then(function (data) {
                                        console.debug(data);
                                        //$rootScope.tablero = data;
                                        $rootScope.listaEdificios = data.TipoEdificios;
                                        console.debug($rootScope.listaEdificios);
                                        $rootScope.listaUnidades = data.TipoUnidades;
                                        $rootScope.listaRecursos = data.TipoRecursos;
                                        $rootScope.tablero = data.Tablero;
                                        $rootScope.listaTecnologias = data.Tecnologias;
                                        $rootScope.dataJuego = data.DataJuego;

                                        $scope.style = { "background-image": "url('" + $rootScope.tablero.ImagenFondo + "')" };
                                        })
                                    .catch(function (err) {
                                        alert(err)
                                    });
        }

        
        //--------------Fin inicializacion de variables---------------------

        $scope.getImgCasilla = function (id) {
            if (id == -1) {
                return $rootScope.tablero.ImagenTerreno;
            } else {
                var edificio = findEdificioInArray($rootScope.listaEdificios, id)[0];
                return edificio.Imagen;
            }
        }

        $scope.getCostoRecurso = function (edificio, idRecurso) {
            var recurso = jQuery.grep(edificio.Costos, function (value) {
                return value.IdRecurso === idRecurso;
            })[0];
            if(recurso)
                return recurso.Value;
            else
                return 0;
        }

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
            if(id == -1){
                //Abro cuadro para contruir
                $('#dialogoConstruir').modal('show');
            }else{
                //abro cuadro del edificio
                $scope.entidad = findEdificioInArray($rootScope.listaEdificios, id)[0];
                console.debug($scope.entidad);
                $('#dialogoDatosEdificio').modal('show');
            }
        }

        $scope.listaTecnologias = function () {
            //Abro cuadro de tecnologias
            $('#dialogoDatosTecnologias').modal('show');
        }

        $scope.actualizarTecnologia = function (idTecnologia) {
            //----LLAMAR AL SERVICIO DE ACTUALIZAR TECNOLOGIAS--------
            $('#dialogoDatosTecnologias').modal('hide');
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