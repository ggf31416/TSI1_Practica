(function () {
'use strict';
angular.module('aldeas').controller("aldeaCtrl", ["$http", "$q", "aldeasService",
        "edificiosService", "unidadesService", '$scope', '$rootScope', '$interval',

    function ($http, $q, aldeasService, edificiosService, unidadesService, $scope, $rootScope, $interval) {

        //--------------Inicializacion de variables---------------------
        $rootScope.listaRecursos = [
               {
                   Nombre: "Oro",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/oro.jpg",
                   Valor: 134,
                   Produccion: 1
               },
               {
                   Nombre: "Madera",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/madera.jpg",
                   Valor: 546,
                   Produccion: 2
               }
        ];
        
        //timer para actualizar recursos
        $interval(function () {
            for (var i = 0; i < $rootScope.listaRecursos.length; i++) {
                $rootScope.listaRecursos[i].Valor = $rootScope.listaRecursos[i].Valor + $rootScope.listaRecursos[i].Produccion;
            }
        },1000);
        
        $scope.initVariables = function () {
                    $rootScope.NombreJuego = tenant;
                    console.debug($rootScope.NombreJuego);
                    aldeasService.getAllData()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.listaEdificios = data.TipoEdificios;
                                    console.debug($rootScope.listaEdificios);
                                    $rootScope.listaUnidades = data.TipoUnidades;
                                    //$rootScope.listaRecursos = data.TipoRecursos;
                                    $rootScope.tablero = data.Tablero;
                                    $rootScope.listaTecnologias = data.Tecnologias;
                                    $rootScope.dataJuego = data.DataJuego;

                                    $scope.style = { "background-image": "url('" + $rootScope.tablero.ImagenFondo + "')" };

                                    $rootScope.tablero.enConstruccion = "https://storagegabilo.blob.core.windows.net/imagenes/gente_en_obra.png";
                                    })
                                .catch(function (err) {
                                    alert(err)
                                });
        }

        
        //--------------Fin inicializacion de variables---------------------

        $scope.getImgCasilla = function (id) {
            if (id == -1) {
                return $rootScope.tablero.ImagenTerreno;
            } if(id == -5){
                return $rootScope.tablero.enConstruccion;
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

        function getUnidades() {
            $scope.unidadesDisponibles = [];
            for (var i = 0; i < $scope.entidad.UnidadesAsociadas.length; i++) {
                $scope.unidadesDisponibles.push(jQuery.grep($rootScope.listaUnidades, function (value) {
                    return value.Id == $scope.entidad.UnidadesAsociadas[i];
                })[0]);
            }
        }

        $scope.entidad = null;

        $scope.editCell = function (casilla) {
            if (casilla.Id == -1) {
                if ($scope.aux == undefined){
                    //Abro cuadro para contruir
                    $scope.editCasilla = casilla;
                    var id = casilla.Id;
                    $('#dialogoConstruir').modal('show');
                } else {
                    alert("Ya hay un edificio en construccion");
                }
            } else {
                $scope.editCasilla = casilla;
                var id = casilla.Id;
                //abro cuadro del edificio
                $scope.entidad = findEdificioInArray($rootScope.listaEdificios, id)[0];
                console.debug($scope.entidad);
                getUnidades();
                
                $scope.unidadesAconstruir = [];
                $('#dialogoDatosEdificio').modal('show');
            }
        }

        $scope.listaTecnologias = function () {
            //Abro cuadro de tecnologias
            $('#dialogoDatosTecnologias').modal('show');
        }

        $scope.listaUnidades = function () {
            //Abro cuadro de unidades
            $('#dialogoDatosUnidades').modal('show');
        }
        $scope.actualizarTecnologia = function (idTecnologia) {
            //----LLAMAR AL SERVICIO DE ACTUALIZAR TECNOLOGIAS--------
            $('#dialogoDatosTecnologias').modal('hide');
        }

        function getCosto(IdRecurso){
            console.debug($scope.aux);
        }

        $scope.addEdificio = function (id) {
            
            $scope.aux = findEdificioInArray($rootScope.listaEdificios, id)[0];
            for (var i = 0; i < $rootScope.listaRecursos.length; i++) {
                $rootScope.listaRecursos[i].Valor = $rootScope.listaRecursos[i].Valor - getCosto($scope.aux, $rootScope.listaRecursos[i].Id);
            }
            $scope.editCasilla.Id = -5;
            $scope.timerConstruccion = $interval(function () {
                $scope.editCasilla.Id = $scope.aux.Id;
                $interval.cancel($scope.timerConstruccion);
                $scope.aux = undefined;
            }, $scope.aux.TiempoConstruccion * 1000);
            
            $('#dialogoConstruir').modal('hide');
        }

        $scope.crearUnidad = function (e) {
            $scope.unidadesAconstruir.push({
                Id: e.Id,
                Nombre: e.Nombre,
                Cantidad: 0
            })
        }

        $scope.removerUnidad = function (Id) {
            $scope.unidadesAconstruir = jQuery.grep($scope.unidadesAconstruir, function (value) {
                return value.Id != Id;
            });
        }
    }
]);
})();
