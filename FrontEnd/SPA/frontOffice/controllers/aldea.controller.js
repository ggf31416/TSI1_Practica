(function () {
'use strict';
angular.module('aldeas').controller("aldeaCtrl", ["$http", "$q", "aldeasService", "juegoService",
        "edificiosService", "unidadesService", '$scope', '$rootScope', '$interval','$window',

    function ($http, $q, aldeasService, juegoService, edificiosService, unidadesService, $scope, $rootScope, $interval,$window) {

        //--------------Inicializacion de variables---------------------
        
        $scope.iniciarSignalR = function () {
            // Declare a proxy to reference the hub.
            $scope.tablero_signalR = $.connection.chatHub;
            // Create a function that the hub can call to broadcast messages.
            $scope.tablero_signalR.client.broadcastMessage = function (name, message) {
                var msg = JSON.parse(message);
                if (msg.Tipo && msg.Tipo == "NotificacionAtaque") {
                    //Mostrar mensaje 
                    if (msg.SoyAtacante && !msg.SoyAliado) {
                        alert("el ataque empezara en " + msg.TiempoAtaque + " segundos");
                        //el ataque empezara en msg.TiempoAtaque segundos
                    } else if (!msg.SoyAtacante && !msg.SoyAliado) {
                        alert("me van a atacar en " + msg.TiempoAtaque + " segundos!");
                        //me atacaran en msg.TiempoAtaque segundos
                    }
                } else if (msg.Tipo && msg.Tipo == "IniciarAtaque") {
                    alert("Inicia el ataque!");
                    $window.location.href = "/" + $rootScope.NombreJuego + "/Home/Index";
                }


            };

            $.connection.hub.start().done(function () {

            });
        };

        $scope.iniciarSignalR();
        

        /*$rootScope.listaRecursos = [
               {
                   Nombre: "Oro",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/oro.jpg",
                   Valor: 134,
                   Produccion: 1
               },
               {
                   NombreF: "Madera",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/madera.jpg",
                   Valor: 546,
                   Produccion: 2
               }
        ];

        $rootScope.listaUnidades = [
               {
                   Nombre: "Soldado",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/soldado.jpg",
                   Valor: 10
               },
               {
                   Nombre: "Arquero",
                   Imagen: "/SPA/backOffice/ImagenesSubidas/arquero.jpg",
                   Valor: 20
               }
        ];
        
        $rootScope.dataJugador = {"EstadoUnidades":{
                                    9:{
                                        "Estado":"A",
                                        "Faltante":0,
                                        "Cantidad":0
                                    },
                                    9:{
                                        "Estado":"C",
                                        "Faltante":20,
                                        "Cantidad":2
                                    }
                                },
                                "EstadoTecnologias":{
                                    "INT":{
                                        "Estado":"STRING", //A terminado C construyendo (Puedo/NoPuedo solo tecnologias)
                                        "Faltante":"INT",
                                        "Cantidad":"INT"
                                    },
                                    "INT":{
                                        "Estado":"STRING",
                                        "Faltante":"INT",
                                        "Cantidad":"INT"
                                    }
                                },
                                "EstadoRecursos":{
                                    1:{
                                        "Total":100,
                                        "Produccion":1
                                    },
                                    2: {
                                        "Total": 100,
                                        "Produccion": 2
                                    },
                                    3: {
                                        "Total": 100,
                                        "Produccion": 3
                                    }
                                }
        }
        */
        $scope.initVariables = function () {
                    $rootScope.NombreJuego = tenant;
                    console.debug($rootScope.NombreJuego);
                    aldeasService.getAllData()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.listaEdificios = data.TipoEdificios;
                                    $rootScope.listaUnidades = data.TipoUnidades;
                                    $rootScope.listaRecursos = data.TipoRecursos;
                                    $rootScope.tablero = data.Tablero;
                                    $rootScope.listaTecnologias = data.Tecnologias;
                                    $rootScope.dataJuego = data.DataJuego;
                                    $rootScope.dataJugador = data.DataJugador;
                                    console.debug($rootScope.dataJugador);

                                    $scope.style = { "background-image": "url('" + $rootScope.tablero.ImagenFondo + "')" };

                                    $rootScope.tablero.enConstruccion = "https://storagegabilo.blob.core.windows.net/imagenes/gente_en_obra.png";
                                    //timer para actualizar recursos
                                    $interval(function () {
                                        for (var i = 0; i < $rootScope.listaRecursos.length; i++) {
                                            $rootScope.dataJugador.EstadoRecursos[$rootScope.listaRecursos[i].Id].Total = $rootScope.dataJugador.EstadoRecursos[$rootScope.listaRecursos[i].Id].Total + $rootScope.dataJugador.EstadoRecursos[$rootScope.listaRecursos[i].Id].Produccion;
                                        }
                                    }, 1000);
                                    console.debug($rootScope.dataJugador.Clan);
                                    if ($rootScope.dataJugador.Clan != null) {
                                        $scope.soyAdmin = aldeasService.soyAdministrador();
                                    }

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

        $scope.getEstadoTecnologia = function (tecnologia) {
            var tec = $rootScope.dataJugador.EstadoTecnologias[tecnologia.Id];
            return tec.Estado;
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

        $scope.getImagenRec = function (id) {
            var edificio = findEdificioInArray($rootScope.listaRecursos, id)[0];
            return edificio.Imagen;
        }

        $scope.getImagenUnidad = function (id) {
            var edificio = findEdificioInArray($rootScope.listaUnidades, id)[0];
            return edificio.Imagen;
        }

        $scope.editCell = function (fila, columna, casilla) {
            if (casilla.Id == -1) {
                if ($scope.aux == undefined){
                    //Abro cuadro para contruir
                    $scope.editCasilla = casilla;
                    $scope.editCasilla.fila = fila;
                    $scope.editCasilla.columna = columna;
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

        $scope.abrirDonarAJugador = function (jugador) {
            $('#dialogoClanes').modal('hide');
            console.debug(jugador);
            $scope.donacion = {
                IdJugadorDestino: jugador.clienteId,
                Nombre: jugador.nombre
            }
            $scope.donacion.Valores = [];
            for (var i = 0; i < $rootScope.listaRecursos.length; i++) {
                var rec = $rootScope.listaRecursos[i];
                $scope.donacion.Valores.push({
                    IdRecurso: rec.Id,
                    Value: 0
                });
            }
            $('#dialogoDonarRecursos').modal('show');
        }

        function getCosto(aux, IdRecurso) {
            var rec = jQuery.grep(aux, function (value) {
                return value.IdRecurso === IdRecurso;
            })[0];
            return rec ? rec.Value : 0;
        }

        $scope.donar = function () {
            var ret = aldeasService.enviarRecursos($scope.donacion);
            if (ret.success) {
                for (var j = 0; j < $rootScope.dataJugador.EstadoRecursos.length; j++) {
                    $rootScope.dataJugador.EstadoRecursos[j].Total = $rootScope.dataJugador.EstadoRecursos[j].Total - getDonacion($scope.donacion.Valores, $rootScope.dataJugador.EstadoRecursos[j].Id);
                }
                $('#dialogoDonarRecursos').modal('hide');
            }
        }

        $scope.abrirClan = function () {
            aldeasService.getJugadoresSinClan()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.jugadoresSinClan = data;
                                })
                                .catch(function (err) {
                                    alert(err)
                                });
            aldeasService.getJugadoresEnElClan()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.jugadoresEnClan = data;
                                })
                                .catch(function (err) {
                                    alert(err)
                                });
            
            //Abro cuadro de clanes
            $('#dialogoClanes').modal('show');
        }

        $scope.abandonarClan = function () {
            var ret = aldeasService.abandonarClan();
            if (ret) {
                $rootScope.dataJugador.Clan = null;
                $scope.soyAdmin = false;
            }
            $('#dialogoClanes').modal('hide');
        }

        $scope.agregarAlClan = function (user) {
            var ret = aldeasService.agregarJugadorClan(user);
            if (ret.success) {
                aldeasService.getJugadoresSinClan()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.jugadoresSinClan = data;
                                })
                                .catch(function (err) {
                                    alert(err)
                                });
                aldeasService.getJugadoresEnElClan()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.jugadoresEnClan = data;
                                })
                                .catch(function (err) {
                                    alert(err)
                                });
            }
        }

        $scope.abrirCrearClan = function () {
            //Abro cuadro de tecnologias
            $('#dialogoCrearClan').modal('show');
        }

        $scope.crearClan = function () {
            var json = { NombreClan: $scope.NombreClan };
            var ret = aldeasService.crearClan(json);
            if(ret.success){
                $rootScope.dataJugador.Clan = $scope.NombreClan;
                $scope.soyAdmin = true;
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

        $scope.listarUsuarios = function () {
            aldeasService.getJugadoresAtacables()
                                .then(function (data) {
                                    console.debug(data);
                                    $rootScope.listaUsuario = data;
                                })
                                .catch(function (err) {
                                    alert(err)
                                });
            //Abro cuadro de usuarios para atacar
            $('#dialogoDatosUsuarios').modal('show');
        }

        $scope.actualizarTecnologia = function (idTecnologia) {
            //----LLAMAR AL SERVICIO DE ACTUALIZAR TECNOLOGIAS--------
            if (!$scope.auxTecnologia) {
                    var json = {
                        IdElemento: idTecnologia
                    };
                    var data = aldeasService.desarrollarTecnologia(json);
                    if (data.sucess) {
                        $scope.auxTecnologia = findEdificioInArray($rootScope.dataJugador.EstadoTecnologias[idTecnologia], idTecnologia)[0];
                        //de donde se saca el tiempo:
                        $scope.modeloTecnologia = findEdificioInArray($rootScope.listaTecnologias, idTecnologia)[0];

                        for (var j = 0; j < $rootScope.dataJugador.EstadoRecursos.length; j++) {
                            $rootScope.dataJugador.EstadoRecursos[j].Total = $rootScope.dataJugador.EstadoRecursos[j].Total - getCosto($scope.auxTecnologia, $rootScope.dataJugador.EstadoRecursos[j].Id);
                        }
                        $scope.timerTecnologia = $interval(function () {
                            $interval.cancel($scope.timerTecnologia);
                            aldeasService.getEntidadesActualizadas()
                                                    .then(function (data) {
                                                        console.debug(data);
                                                        $rootScope.listaEdificios = data.TipoEdificios;
                                                        console.debug($rootScope.listaEdificios);
                                                        $rootScope.listaUnidades = data.TipoUnidades;

                                                    })
                                                    .catch(function (err) {
                                                        alert(err)
                                                    });
                            $scope.auxTecnologia = undefined;
                        }, $scope.modeloTecnologia.TiempoConstruccion * 1000);
                    }
                    else
                        alert("No es posible desarrollar la tecnologia");
            } else
                alert("Ya hay otra tecnologia actualizando");
            $('#dialogoDatosTecnologias').modal('hide');
        }

        function getCosto(aux, IdRecurso){
            var rec = jQuery.grep(aux.Costos, function (value) {
                return value.IdRecurso === IdRecurso;
            })[0];
            return rec ? rec.Value : 0;
        }

        function getProduccion(aux, IdRecurso) {
            var rec = jQuery.grep(aux.RecursosAsociados, function (value) {
                return value.IdRecurso === parseInt(IdRecurso);
            })[0];
            return rec ? rec.Value : 0;
        }

        $scope.addEdificio = function (id) {
            var fila = $scope.editCasilla.fila;
            var columna = $scope.editCasilla.columna;
            var json = { PosFila: fila, PosColumna: columna, IdTipoEdificio: id }
            var data = aldeasService.construirEdificio(json);
            if (data.ret) {
                $scope.aux = findEdificioInArray($rootScope.listaEdificios, id)[0];
                for (var i = 0; i < $rootScope.dataJugador.EstadoRecursos.length; i++) {
                    $rootScope.dataJugador.EstadoRecursos[i].Total = $rootScope.dataJugador.EstadoRecursos[i].Total - getCosto($scope.aux, $rootScope.dataJugador.EstadoRecursos[i].Id);
                }
                $scope.editCasilla.Id = -5;
                $scope.timerConstruccion = $interval(function () {
                    var keys = Object.keys($rootScope.dataJugador.EstadoRecursos);
                    for (var j = 0; j < keys.length; j++) {
                        $rootScope.dataJugador.EstadoRecursos[keys[j]].Produccion = $rootScope.dataJugador.EstadoRecursos[keys[j]].Produccion + getProduccion($scope.aux, keys[j]);
                    }
                    $scope.editCasilla.Id = $scope.aux.Id;
                    $interval.cancel($scope.timerConstruccion);
                    $scope.aux = undefined;
                }, $scope.aux.TiempoConstruccion * 1000);
            }
            else
                alert("No es posible construir en ese lugar");
            $('#dialogoConstruir').modal('hide');
        }

        $scope.entrenarUnidades = function () {
            if (!$scope.auxUnidad) {
                for (var i = 0; i < $scope.unidadesAconstruir.length; i++) {
                    var json = {
                        IdTipoUnidad: parseInt($scope.unidadesAconstruir[i].Id),
                        Cantidad: parseInt( $scope.unidadesAconstruir[i].Cantidad)
                    };
                    var data = aldeasService.entrenarUnidades(json);
                    if (data.ret) {
                        $scope.auxUnidad = $rootScope.dataJugador.EstadoUnidades[$scope.unidadesAconstruir[i].Id];
                        
                        //de donde se saca el tiempo:
                        $scope.modeloUnidad = findEdificioInArray($rootScope.listaUnidades, $scope.unidadesAconstruir[i].Id)[0];

                        if ($scope.auxUnidad == undefined) {
                            $scope.auxUnidad ={
                                            Cantidad:0,
                                            Estado:0,
                                            Id:$scope.unidadesAconstruir[i].Id,
                                            Tiempo:$scope.modeloUnidad.TiempoConstruccion
                            }
                            $rootScope.dataJugador.EstadoUnidades[$scope.unidadesAconstruir[i].Id] = $scope.auxUnidad;
                        }
                        $scope.auxUnidad.CantAConstruir = parseInt($scope.unidadesAconstruir[i].Cantidad);
                        
                        for (var j = 0; j < $rootScope.dataJugador.EstadoRecursos.length; j++) {
                            $rootScope.dataJugador.EstadoRecursos[j].Total = $rootScope.dataJugador.EstadoRecursos[j].Total - getCosto($scope.auxUnidad, $rootScope.dataJugador.EstadoRecursos[j].Id);
                        }
                        $scope.timerUnidad = $interval(function () {
                            $interval.cancel($scope.timerUnidad);
                            console.debug($scope.auxUnidad);
                            $scope.auxUnidad.Cantidad = $scope.auxUnidad.Cantidad + $scope.auxUnidad.CantAConstruir;
                            $scope.auxUnidad.Estado = 0;
                            $scope.auxUnidad = undefined;
                        }, $scope.modeloUnidad.TiempoConstruccion * 100 * $scope.auxUnidad.CantAConstruir);
                    }
                    else
                        alert("No es posible construir la unidad");
                }
            } else
                alert("Ya hay otra unidad actualizando");
            $('#dialogoDatosEdificio').modal('hide');
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

        $scope.iniciarAtaque = function (id) {
            var jsonAtaque = {
                Enemigo: id
            };
            juegoService.iniciarAtaque(jsonAtaque);
        }
    }
]);
})();
