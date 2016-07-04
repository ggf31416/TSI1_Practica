/// <reference path="~/SPA/frontOffice/models/unidad.js" />


(function () {
    'use strict';
    angular.module('juego').controller("juegoCtrl", ["$http", "$q","juegoService",'$scope', '$window','$rootScope',

   

    function ($http,$q,juegoService, $scope, $window,$rootScope) {


        var selectedUnit = null;

        $rootScope.nombreJuego = tenant;

        $scope.contador = 1;
        $scope.idJuego = 6;

        $scope.listaEnemigos = [];

        $scope.finMsg = "";

        var dataExtra = {};
        
        //juegoService.GetEstadoBatalla();

        $scope.posicionarUnidad= function (id) {
            // llama a la funcion iniciarCustomDrag en el CrearCanvas.js
            esUnidad = true;
            iniciarCustomDrag(id);
        };

        $scope.posicionarEdificio = function (id) {
            // llama a la funcion iniciarCustomDrag en el CrearCanvas.js
            esUnidad = false;
            iniciarCustomDrag(id);
        };

        
        $q.all([
            juegoService.GetEstadoBatalla()
        ]).then(function (data) {
            
            var data = data[0].data.ret;
            $scope.inicializar(data);
        });

        $scope.inicializar = function(data){
            
            $scope.batalla = data;//JSON.parse(data[0].data.ret);
            dataExtra = JSON.parse(data.Data);
            console.info("Datos batalla");
            console.info($scope.batalla);
            var batalla = $scope.batalla;
            if (!batalla) console.error("No hay datos de batalla");
            nombreJugador = batalla.IdJugador;
            shortId = batalla.ShortId;
            $rootScope.listaEdificios = batalla.tiposEdificio;
            //$rootScope.listaUnidades = [ {Ataque : 10, Defensa : 10, Id : 314159, Nombre : "Arquero", TiempoConstruccion : 10, Vida : 100, Imagen : "/SPA/backoffice/ImagenesSubidas/arquero.jpg" }]
            $rootScope.listaUnidades = batalla.tiposUnidad;
            //data[1];
            window.createGame();
        }
     

        $rootScope.listaRecursos = [
                {
                    Nombre: "Recurso1",
                    Imagen: "/SPA/backOffice/soldado.jpg"
                },
                {
                    Nombre: "Recurso2",
                    Imagen: "/SPA/backOffice/arquero.jpg"
                }
        ];

        var EstadoJuego = function() {
            this.edificios =  [];
            this.unidades_desplegadas = [];
            this.unidades = [];
            this.unidades_desplegables = {};
        };

        $scope.estadoJuego = new EstadoJuego();


        var altoJuego = 600;
        var anchoJuego = 800;


        $scope.game;

        var menuCuartel;
        var cursors;
        
        var unit_size = 20;
        var tile_size = unit_size * 4;
        var tablero_sizeX = 10 * tile_size;
        var tablero_sizeY = 10 * tile_size;
        var mouse_sprite;
        var buildings;
        var unidades_desplegadas;
        var unidadesPorId = {}

        var nombreJugador;
        var shortId;

        var estaEnBatalla = true;
       
        $scope.animaciones = {}

        window.createGame = function (scope, injector) {
            // Create our phaser $scope.game
            if ($scope.batalla){
                anchoJuego = $scope.batalla.SizeX * unit_size;
                altoJuego = $scope.batalla.SizeY * unit_size;
            }
            console.info("ancho: " + anchoJuego + ", alto: " + altoJuego);
            $scope.game = new Phaser.Game(anchoJuego, altoJuego, Phaser.CANVAS, 'divJuego', { preload: $scope.preload, create: create, update: update });
            //$scope.game.time.events.add(Phaser.Timer.SECOND * 1, pedirNombre, this);
        };


        function objetoUnidad(data,idSprite) {
           return $scope.game.add.sprite(data.PosX * unit_size, data.PosY * unit_size, idSprite);
        }
        

        function desplegable(sprite_id){
            return  $scope.estadoJuego.unidades_desplegables[sprite_id];
        }


        function mostrarUnidades(b) {
            $('#divEdificios').toggle(!b);
            $('#divUnidades').toggle(b);
        }

        function setInfoFromTipo(entidad,jugador,unitId){

            var tipo = $scope.batalla.jugadores[jugador].Tipos[entidad.id];
            var velocidad = 8;
            var defensa = 0;
            entidad.info = new Unidad_Info(unitId,jugador,tipo.hp,tipo.rango, velocidad,tipo.ataque,defensa,null);   
        }

        function setInfoFromData(entidad,data){
            
            var tipo = $scope.batalla.jugadores[data.jugador].Tipos[data.Id];

            entidad.info = new Unidad_Info(data.Unit_id,data.jugador,tipo.hp,tipo.rango,data.velocidad,tipo.ataque,data.defensa,data.target);   
        }

        function setInfoFromMensaje(entidad,data){
            
            var tipo = $scope.batalla.jugadores[data.J].Tipos[data.Id];
            entidad.info = new Unidad_Info(data.IdUn,data.J,tipo.hp,tipo.rango,data.velocidad,tipo.ataque,data.defensa,data.target);   
        }


        

        function crearEdificioInmediato(data) {
            if (data.hp <= 0) return;
            var idSprite =  data.Id;
            var edificio = $scope.game.add.sprite(data.PosX * unit_size, data.PosY * unit_size, idSprite);
            setInfoFromData(edificio, data);
            edificio.height = tile_size;
            edificio.width = tile_size;
            edificio.inputEnabled = true;
            //clickVisibleUnidades(edificio, true);
            unidadesPorId[edificio.info.unit_id] = edificio;
            buildings.add(edificio);
        }

        function crearUnidadFromMensaje(data){
            if (!unidadesPorId[data.IdUn]) {
                if (data.hp <= 0) return;
                var idSprite = data.Id;
                var unit = objetoUnidad(data,idSprite);
                unit.height = unit_size;
                unit.width = unit_size;
                unit.inputEnabled = true;
                if (unit.jugador == nombreJugador){
                    if (desplegable(sprite_id).cantidad){
                        desplegable(idSprite).cantidad -= 1;
                        $scope.contador++;
                    }
                }
                setInfoFromMensaje(unit, data)
                unidades_desplegadas.add(unit);
                unidadesPorId[unit.info.unit_id] = unit;
                agregarGraficos($scope.game, unit);
            }

        }

        function crearUnidadInmediato(data) {
            if (!unidadesPorId[data.Unit_id]) {
                if (data.hp < 0) return;
                var idSprite = data.Id;
                var unit = objetoUnidad(data,idSprite);
                unit.height = unit_size;
                unit.width = unit_size;
                unit.inputEnabled = true;
                
                setInfoFromData(unit, data)
                unidades_desplegadas.add(unit);
                unidadesPorId[unit.info.unit_id] = unit;
                agregarGraficos($scope.game, unit);
            }

        }

        $scope.cargarDesdeEstado = function () {
            var estado = $scope.estadoJuego;
            if (buildings){
                // quita todos los edificios
                buildings.removeAll(true);
            }
            if (unidades_desplegadas) {
                unidades_desplegadas.removeAll(true);
            }
            else{
                unidades_desplegadas =  $scope.game.add.physicsGroup();
            }
           estado.edificios.forEach (function (e) {
                crearEdificioInmediato(e);
            });
            if (estado.unidades_desplegadas){
            
               estado.unidades_desplegadas.forEach(function (e) {
                crearUnidadInmediato(e);
                });
            }
            //$scope.$apply();
        };

        function esEsteJugador(json){
            return json.Id == nombreJugador;
        }

        function cargarEstado( ){
            var batalla = $scope.batalla;
            $scope.estadoJuego = new EstadoJuego();
            var est = $scope.estadoJuego;
            estaEnBatalla = true;
            for(var idJug in batalla.jugadores){
                var jugador = batalla.jugadores[idJug];
                if (esEsteJugador(jugador)){
                    var jug_u = jugador.Unidades;
                    jug_u.forEach(function(c){
                        est.unidades_desplegables[c.UnidadId] = { cantidad : c.Cantidad};
                    });
                }
            };
            est.unidades_desplegadas = [];
             batalla.unidades.forEach(function(u){
                est.unidades_desplegadas.push(JSON.parse(u));
             });
            
            est.edificios = [];
             batalla.edificios.forEach(function(u){
                est.edificios.push(JSON.parse(u));
             });
            
            
            $scope.cargarDesdeEstado();
            console.log("Se termino de cargar batalla");
        }

       

        var MsgA = {
            AddEdificio: "AddEd",
            AddUnidad: "AddUn",
            MoveUnit: "MoveUnit",
            TargetUnit: "TargetUnit",
            UpdateHP: "UpdateHP",
            ListaAcciones: "ListaAcciones",
            IniciarAtaque: "IniciarAtaque",
            PosUnit: "PosUnit"
        };

        function posicionarAbs(sprite,x,y){
            sprite.x = x * unit_size;
            sprite.y = y * unit_size;
        }

        function ejecutarMensaje(msg){
            if (msg.A){
                if (msg.A == MsgA.IniciarAtaque){
                    $scope.inicializar(msg.Data);//cargarEstado(msg);
                }
                else if(msg.A == MsgA.PosUnit){
                     var unit = unidadesPorId[msg.IdUn];
                     if(unit){
                        posicionarAbs(unit,msg.PosX,msg.PosY);
                     }
                }
                else if (msg.A == MsgA.AddEdificio) {
                        crearEdificioInmediato(msg);
                }
                else if (msg.A == MsgA.AddUnidad) {
                    crearUnidadFromMensaje(msg);
                }
                else if (msg.A == MsgA.MoveUnit) {
                    var unit = unidadesPorId[msg.IdUn];
                    if (unit) {
                        unit.info.target = msg.T;
                        moverUnidad(unit, msg.Path);
                    }
                }
                else if (msg.A == MsgA.Target) {
                    var unit = unidadesPorId[msg.IdUn];
                    if (unit) {
                        unit.info.target = msg.T;
                    }
                }
                else if (msg.A == MsgA.UpdateHP) {
                    var unit = unidadesPorId[msg.IdUn];
                    // setear vida
                    if (unit){
                        if (unit.info.hp > msg.VN && unit.info.hp > 0){
                            unit.info.hp = msg.VN;
                        }
                        if (msg.VN < 0){
                            // matar

                            unit.kill();
                        }
                    }
                }
                else if (msg.A == MsgA.ListaAcciones){
                    msg.L.forEach(function(a){
                        ejecutarMensaje(a);
                    });
                }    
            }
            

        }

        $scope.VolverBase = function(){
            $window.location.href = "/" + tenant + "/Home/Aldea";
        }

        $scope.iniciarSignalR = function () {
            // Declare a proxy to reference the hub.
            $scope.tablero_signalR = $.connection.chatHub;
            // Create a function that the hub can call to broadcast messages.
            $scope.tablero_signalR.client.broadcastMessage = function (name, message) {
                if (message!=""){
                    console.debug(message);
                    var msg = JSON.parse(message);
                    if ( msg.A){
                        ejecutarMensaje(msg);    
                    }
                    else if (msg.Tipo && msg.Tipo == "FinBatalla"){
                        mostrarFin(msg);
                    }
                    
                }
                //$scope.estadoJuego.edificios = msjJSON.edificios;
                //$scope.estadoJuego.unidades_desplegadas = msjJSON.unidades;
                //$scope.estadoJuego.edificios.push(msjJSON);
                //$scope.cargarDesdeEstado();
            };
            // Get the user name and store it to prepend to messages.
           // $('#displayname').val(prompt('Enter your name:', ''));
            // Set initial focus to message input box.
           // $('#message').focus();
            // Start the connection.
            $.connection.hub.start().done(function () {
                
            });
        };

         function mostrarFin(mensaje){
            $scope.finMsg = mensaje.Msg;
            $scope.$apply();
            console.info($scope.finMsg);
            $('#dialogoFin').modal('show');
        }

        $scope.iniciarSignalR();

        $scope.preload = function() {

            //  You can fill the preloader with as many assets as your $scope.game requires

            //  Here we are loading an image. The first parameter is the unique
            //  string by which we'll identify the image later in our code.

            //  The second parameter is the URL of the image (relative)
            // por ahora harcodeado
            //$scope.game.load.json('jsonEdificios', '/Entidades/GetAllTipoEdificios');
            //console.log("preload");

            // activa la carga desde cualquier dominio (se puede restringir al dominio del servidor de imagenes)
            $scope.game.load.crossOrigin = 'anonymous';
            if (dataExtra.Pasto){
                $scope.game.load.image('grass', dataExtra.Pasto);
            }
            else{
                $scope.game.load.image('grass', '/Content/img/grass.jpg');
            }
            
            $scope.game.load.image('cuartel', '/Content/img/Barracks4.png');
            $scope.game.load.image('cuartelMenu', '/Content/img/cuartel.jpg');
            $scope.game.load.image('def_proy', '/Content/img/Crystal_Arrow.gif');
            if (!$scope.estadoJuego.unidades_desplegables){
                $scope.estadoJuego.unidades_desplegables = {};
            }
            
            $scope.listaEdificios.forEach(function (e) {
                if (e.Imagen != null) {
                    console.log(e);
                   // $scope.game.load.image(e.Id, 'http://localhost:56662' + e.Imagen);
                    $scope.game.load.image(e.Id,  e.Imagen);

                }
            });

            $scope.listaUnidades.forEach(function (e) {
                if (e.Imagen != null) {
                    console.log(e);
                    $scope.game.load.image(e.Id, e.Imagen);
                    /*if (!$scope.estadoJuego.unidades_desplegables[e.Id]){
                        $scope.estadoJuego.unidades_desplegables[e.Id] = { cantidad: 0};    
                    }*/
                    
                    //$scope.estadoJuego.unidades_desplegables[e.Id].cantidad = 10;
                }
            });

        }


        function clickVisibleUnidades(sprite, b) {
            sprite.events.onInputDown.add(function () { mostrarUnidades(b) }, this);
        }

        function create() {
            var game = $scope.game;
            game.physics.startSystem(Phaser.Physics.ARCADE)
            // setea el tamaño del tablero en pixeles en 2000x2000 
            tablero_sizeX = $scope.batalla.SizeX * unit_size;
            tablero_sizeY = $scope.batalla.SizeY * unit_size;
             game.world.resize(tablero_sizeX, tablero_sizeY);

            // disableVisibilityChange intenta que el juego siga corriendo aunque pierda el foco
            // no es posible en todos los casos (por ejemplo puede no funcionar al cambiar a otro tab dependiendo del navegador)
            // pero deberia permitir que siga funcionando al hacer click afuera, lo que reduce problemas de UI
             game.stage.disableVisibilityChange = true;

            //  The deadzone is a Rectangle that defines the limits at which the camera will start to scroll
            //  It does NOT keep the target sprite within the rectangle, all it does is control the boundary
            //  at which the Fcamera will start to move. So when the sprite hits the edge, the camera scrolls
            //  (until it reaches an edge of the world)



            mouse_sprite = game.add.sprite( game.world.centerX,  game.world.centerY, "grass");
            mouse_sprite.alpha = 0;
             game.physics.arcade.enable(mouse_sprite,this);

            //seguirMouse();
            var imagenPasto = game.cache.getImage("grass");
            var bitmap =   game.add.bitmapData(2 * tile_size,2 * tile_size);
            if (imagenPasto.width > 2 * tile_size && imagenPasto.height > 2 * tile_size){
                var r = new Phaser.Rectangle(0,0,2 * tile_size, 2 * tile_size);
                bitmap.copyRect(imagenPasto,r,0,0);
                var tile = $scope.game.add.tileSprite(0, 0,  game.world.width,  game.world.height, bitmap);
                tile.tileScale = new PIXI.Point(0.5,0.5);
            }
            else{
                var tile = $scope.game.add.tileSprite(0, 0,  game.world.width,  game.world.height, "grass");
                tile.tileScale = new PIXI.Point(1,1);
            }
            
           
            //tile.inputEnabled = true;
            //clickVisibleUnidades(tile, false);


            buildings = $scope.game.add.physicsGroup();
            unidades_desplegadas = $scope.game.add.physicsGroup();

            cursors = $scope.game.input.keyboard.createCursorKeys();

            var proyectiles = $scope.game.add.physicsGroup();
            //crearInternalMenu();



            cargarEstado();

             game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
 
            //have the game centered horizontally
             game.scale.pageAlignHorizontally = true;
            game.scale.pageAlignVertically = true;
         
            //screen size will be set automatically
         
            game.scale.updateLayout();
        }

        function iniciarCustomDrag(nombreEdificio) {
            nombreEntidadDragged = nombreEdificio;
            if (spriteDragged != null) {
                spriteDragged.destroy();
                spriteDragged = null;
            }
        }

        var nombreEntidadDragged = null;
        var spriteDragged = null;
        var esUnidad = false;

        $scope.accionMouseOver = function () {
            //console.log("Mouse Over: spriteDragged=" + spriteDragged);
            if (nombreEntidadDragged != null) {
                $scope.game.canvas.focus();
                if (esUnidad) {
                    crearDragUnidad(nombreEntidadDragged);
                }
                else {
                    crearDragEdificio(nombreEntidadDragged);
                }
                
                nombreEntidadDragged = null;
            }
        }

        $scope.puedoDesplegarUnidad = function(sprite_id){
            return !estaEnBatalla  || (desplegable(sprite_id) &&  desplegable(sprite_id).cantidad > 0);
        };

        $scope.cantUnidad = function(id){
            return  (desplegable(id)) ? desplegable(id).cantidad : "";
        }


        function activarSeguirMouse() {
            var borde = 50;
            $scope.game.camera.follow(mouse_sprite);
            $scope.game.camera.deadzone = new Phaser.Rectangle(borde, borde, anchoJuego - borde, altoJuego - borde);
        }


        function crearDrag(imagen, size, stepSize, stopFunc) {
            // worldX,Y obtienen la posicion del puntero en relacion al mundo
            // x,y solos obtienen en relacion a la camara
            var input_x = $scope.game.input.activePointer.worldX;
            var input_y = $scope.game.input.activePointer.worldY;

            var entidad = $scope.game.add.sprite(input_x, input_y, imagen);
            entidad.id = imagen;
            entidad.height = size;
            entidad.width = size;

            entidad.inputEnabled = true;
            // tam_grilla_x,tam_grilla_y, ajustar a grilla al: arrastrar, soltar
            entidad.input.draggable = true;
            entidad.input.enableSnap(stepSize, stepSize, true, true);

            entidad.events.onDragStop.add(stopFunc, this);

            $scope.game.physics.arcade.enable(entidad);
            entidad.anchor.x = 0;
            entidad.anchor.y = 0;

            spriteDragged = entidad;

        }

        function crearDragEdificio(imagen) {
            crearDrag(imagen, tile_size, tile_size, $scope.aceptarPosicionEdificio);
        }


        function crearDragUnidad(imagen) {
            crearDrag(imagen, unit_size, unit_size, $scope.aceptarPosicionUnidad);
        }

        var paths = [
         {x: 5, y: 5, s: 4},
         {x: 3, y : 3, s: 2},
         {x: 9, y : 9, s: 8}
        ]

        function hacerSeleccionableUnidad(sprite) {
            sprite.inputEnabled = true;
            sprite.events.onInputDown.add(function (sprite, pointer) {

            });
        }

        $scope.aceptarPosicionEdificio = function (sprite, pointer) {
            //console.log("aceptarPosicionEdificio");
            if (!$scope.game.physics.arcade.overlap(sprite, buildings) && !$scope.game.physics.arcade.overlap(sprite, unidades_desplegadas)) {
                var input_x = sprite.x / unit_size;
                var input_y = sprite.y / unit_size;
                console.debug(sprite);

                sprite.input.draggable = false;
                buildings.add(sprite);
                $scope.game.debug.reset();
                sprite.alpha = 0.8;
                //sprite.tint = 0xFFFF00;
                construccion(sprite);

                juegoService.crearEdificioEnTablero(sprite.id, input_x, input_y,nombreJugador).then(function () {
                    // hacer algo
                });
            }
            else {
                sprite.destroy();
                $scope.game.debug.reset();
            }
            spriteDragged = null;
        }


        function agregarGraficos(game,sprite){
            var graphics = game.add.graphics(sprite.x,sprite.y);
            crearGraficoUnidad(sprite, graphics);
            sprite.graficos = graphics;
            sprite.events.onKilled.add(function(f) {
                //console.info(f);
                if (sprite.graficos) sprite.graficos.destroy();
                console.info(sprite.info.unit_id + " was kiled");
            });
            sprite.events.onDestroy.add(function(f) {
                if (sprite.graficos) sprite.graficos.destroy();
                //console.info(sprite.info.unit_id + " was destroyed");
            });
        }

        // se podría permitir personalizar
        function crearGraficoUnidad(sprite,graphics){
            var colorCirculo = 0xffffff;
            if (sprite.info ){
                colorCirculo = sprite.info.jugador == nombreJugador ? 0x00ff00 : 0xff0000;
            }
            // dibuja un circulo del color apropiado
            graphics.lineStyle(1, colorCirculo, 1);
            graphics.drawCircle(sprite.width/2,sprite.height / 2, unit_size * 1.5);
            /*graphics.beginFill(0x00ff00,1);
            graphics.vida = graphics.drawRect(0, sprite.height * 0.9,sprite.width,20);
            graphics.endFill();
            graphics.beginFill(0xff0000,1);
            graphics.vidaFaltante = graphics.drawRect(0, sprite.height * 0.9,0,20);
            graphics.endFill();*/
        }



        $scope.aceptarPosicionUnidad = function (sprite, pointer) {
            //console.log("aceptarPosicionUnidad");
            var continuo = false;
            if (!$scope.game.physics.arcade.overlap(sprite, buildings) && !$scope.game.physics.arcade.overlap(sprite, unidades_desplegadas)) {
                var input_x = sprite.x / unit_size;
                var input_y = sprite.y / unit_size;
                sprite.input.draggable = false;
                unidades_desplegadas.add(sprite);
                $scope.game.debug.reset();
                sprite.alpha = 1.0;
                sprite.tint = 0xFFFFFF;
 
                while(unidadesPorId[shortId + "#" + $scope.contador]){
                    $scope.contador++;
                }
                setInfoFromTipo(sprite,nombreJugador,shortId + "#" + $scope.contador);
                unidadesPorId[sprite.info.unit_id] = sprite;
                agregarGraficos($scope.game, sprite);
                //sprite.id_logico = $scope.contador--; // asigno un id automatico que luego cambio}
                if (estaEnBatalla){
                    desplegable(sprite.id).cantidad -= 1;
                    juegoService.posicionarUnidad(sprite.id, sprite.info.unit_id, input_x, input_y,nombreJugador);
                    $scope.$apply();
                    if (desplegable(sprite.id).cantidad > 0){
                        continuo = true;
                        spriteDragged = sprite.id;
                        crearDragUnidad(sprite.id);
                    }
                    
                }
                else{
                    juegoService.construirUnidad(sprite.id,nombreJugador);
                }
                
            }
            else {
                continuo = true;
                //sprite.destroy();
                //$scope.game.debug.reset();
            }
            if (!continuo) spriteDragged = null;
        }

        function distancia(x1,y1,x2,y2){
            var dx = x2 - x1; 
            var dy = y2 - y1;
            return srqt(dx * dx + dy * dy);
        }

        function moverUnidad(sprite, paths) {
            detenerMovimiento(sprite);
            var tweenAnterior = null;
            var primerTween = null;
            var x_ant = sprite.x;
            var y_ant = sprite.y;
            for (var i in paths) {
                var x_new = paths[i].x * unit_size;
                var y_new = paths[i].y * unit_size;
                //var distancia = distancia(x_ant,y_ant,x_new,y_new);
                var tiempo = paths[i].t; 
                var tween = $scope.game.add.tween(sprite).to({ x: x_new , y: y_new }, tiempo , Phaser.Easing.Linear.None, false);
                if (tweenAnterior) {
                    tweenAnterior.chain(tween)
                }
                else {
                    primerTween = tween;
                }

                tweenAnterior = tween;
            }
            //spriteDragged.body.velocity.x = 1;
            //spriteDragged.body.velocity.y = 1;
            primerTween.start();
            $scope.animaciones[sprite.info.unit_id] = primerTween;


        }

        function onDragUpdateBuild(sprite, pointer) {
            //$scope.game.debug.body(sprite);
            //crearCuadrado(sprite.x, sprite.y);
            //sprite.children[0].moveTo(sprite.x, sprite.y);
            if ($scope.game.physics.arcade.overlap(sprite, buildings)) {
                sprite.tint = 0xFF0000;
                $scope.game.debug.body(sprite, 'rgba(255, 0, 0, 0.5)');
                //$scope.game.debug.geom(sprite.children[0], 'rgba(0,255,0,0.5)');
            }
            else {
                sprite.tint = 0x00FF00;
                //$scope.game.debug.geom(sprite.children[0], 'rgba(0,0,255,0.5)');
                $scope.game.debug.body(sprite);
            }

        }

        function sobreponerCuadrado(sprite, color, alpha) {
            alpha = alpha || 0.5; // 0.5 sino se pasa alpha
            var completionSprite = $scope.game.add.graphics(0, 0);
            completionSprite.beginFill(color, alpha);
            completionSprite.bounds = new PIXI.Rectangle(sprite.x, sprite.y, sprite.width, sprite.height);
            completionSprite.drawRect(sprite.x - sprite.width * sprite.anchor.x, sprite.y - sprite.height * sprite.anchor.y, sprite.width, sprite.height);
            return completionSprite;
        }

        function construccion(sprite) {
            var c = sobreponerCuadrado(sprite, 0xFFFF00, 0.5)
            construir(sprite, c, 5);
        }

        function construir(sprite, rec, segundos) {
            $scope.game.time.events.add(Phaser.Timer.SECOND * segundos, function () {
                rec.destroy();
                sprite.alpha = 1;
                sprite.tint = 0xFFFFFF;
                clickVisibleUnidades(sprite, true);
                //enviarSignalR();
            }, this);
        }


        function checkOverlap(spriteA, spriteB) {
            if (spriteA == null || spriteB == null) return false;
            var boundsA = spriteA.getBounds();
            var boundsB = spriteB.getBounds();

            return Phaser.Rectangle.intersects(boundsA, boundsB);
        }

        function dinamicoUnidad(sprite){
            var bmd = this.game.add.bitmapData(this.config.width, this.config.height);
            bmd.ctx.fillStyle = this.config.bar.color;
            bmd.ctx.beginPath();
            bmd.ctx.rect(0, 0, this.config.width, this.config.height);
            bmd.ctx.fill();

            this.barSprite = this.game.add.sprite(this.x - this.bgSprite.width/2, this.y, bmd);
            this.barSprite.anchor.y = 0.5;

            if(this.flipped){
                this.barSprite.scale.x = -1;
            }
        }

        var contFrames = 0;

        function update() {
            contFrames++;
            // actualizo los circulos de las unidades para que coincidan los centros
            // si lo hago con addChild me escala el circulo y no lo quiero 
            unidades_desplegadas.forEach(function(u){
                if (u.graficos){

                    //var porcentaje = u.info.hp / u.info.max_hp;
                    /*u.graficos.vida.width = u.width *  porcentaje;
                    u.graficos.vidaFaltante.x = u.graficos.vida.width;
                    u.graficos.vidaFaltante.width = u.width * (1 - porcentaje);*/
                    u.graficos.x  = u.x;
                    u.graficos.y = u.y;
                    //$scope.game.debug.text(u.info.hp,u.x,u.y + u.height * 1.4);
                }
                if (contFrames % 5 == 4) animacionAtaque(u);
            });

            if (cursors.up.isDown) {
                $scope.game.camera.y -= 4;
            }
            else if (cursors.down.isDown) {
                $scope.game.camera.y += 4;
            }

            if (cursors.left.isDown) {
                $scope.game.camera.x -= 4;
            }
            else if (cursors.right.isDown) {
                $scope.game.camera.x += 4;
            }



            if (spriteDragged != null) {
                var s = esUnidad ? unit_size : tile_size;
                spriteDragged.x = Math.floor($scope.game.input.activePointer.worldX / s) * s;
                spriteDragged.y = Math.floor($scope.game.input.activePointer.worldY / s) * s

                if ($scope.game.physics.arcade.overlap(spriteDragged, buildings)
                    || $scope.game.physics.arcade.overlap(spriteDragged, unidades_desplegadas)) {
                    spriteDragged.tint = 0xFF0000;
                    $scope.game.debug.body(spriteDragged, 'rgba(255, 0, 0, 0.5)');
                }
                else {
                    spriteDragged.tint = 0x00FF00;
                    $scope.game.debug.body(spriteDragged);
                }
            }
            //$scope.game.debug.text($scope.game.input.activePointer.x, 200, 32);
            //$scope.game.debug.text($scope.game.input.activePointer.y, 200, 64);
            /*if (spriteDragged != null) {
                $scope.game.debug.text("isDragged: " + spriteDragged.input.isDragged, 200, 96);
            }*/
            //$scope.game.physics.arcade.collide(unidades_desplegadas, buildings,function(){ });

        }



        function disparar(spriteAt, spriteDest) {
            var game = $scope.game;
            var proyectil = game.add.sprite( spriteAt.x+spriteAt.width/2, spriteAt.y+spriteAt.height/2,'def_proy');
            proyectil.targetSprite = spriteDest;
            var angulo = game.physics.arcade.angleToXY(spriteAt,spriteDest.x+spriteDest.width/2,spriteDest.y+spriteDest.height/2);
            proyectil.rotation = angulo + (45 * Math.PI /180);
            proyectil.width = unit_size * 1.0;
            proyectil.height = unit_size * 1.0;
            //proyectiles.add(proyectil);

            var animacionProyectil =  game.add.tween(proyectil);
            animacionProyectil.to({ x: spriteDest.x, y: spriteDest.y }, 500, 'Linear', true);

            // destruye el proyectil cuando alcanza el objetivo
            animacionProyectil.onComplete.add(function(sprite, tween) { sprite.destroy(); }, this);
            

        }

        function detenerMovimiento(sprite) {
            var tw = $scope.animaciones[sprite.info.unit_id];
            if (tw) {
                //$scope.game.tweens.remove(tw);
                tw.stop();
                if (tw.chainedTween) tw.chainedTween.stop();
                delete $scope.animaciones[sprite.info.unit_id];
            }
        }

        function animacionAtaque(spriteAtaq) {
            if (! spriteAtaq.info.target) return;
            if (spriteAtaq.info.hp <= 0) return;
            var def = unidadesPorId[spriteAtaq.info.target];
            if (!def) return;
            var game = $scope.game;
            var distancia = $scope.game.physics.arcade.distanceBetween(spriteAtaq, def);
            if (distancia <= (spriteAtaq.info.rango + 1) * unit_size) {
                //console.info("En rango");
                //detenerMovimiento(spriteAtaq);
                if (spriteAtaq.info && def.info.hp > 0){ //  && spriteAtaq.info.ataque > 0
                    if (!spriteAtaq.info.prox_ataque || game.time.now > spriteAtaq.info.prox_ataque){
                        disparar(spriteAtaq,def);
                        spriteAtaq.info.prox_ataque = game.time.now + 500;
                    }
                }
            }
        }



        function seguirMouse() {
            sprite = mouse_sprite;
            //  only move when you click

            //  400 is the speed it will move towards the mouse
            $scope.game.physics.arcade.moveToPointer(sprite, 100);

            //  if it's overlapping the mouse, don't move any more
            if (Phaser.Rectangle.contains(sprite.body, $scope.game.input.x, $scope.game.input.y)) {
                sprite.body.velocity.setTo(0, 0);
            }

            /*else {
                sprite.body.velocity.setTo(0, 0);
            }*/
        }


        $scope.mostrarEnemigos = function () {
            juegoService.getListaEnemigos(nombreJugador, $scope.nombreJuego).then(function (data) {
                $scope.listaEnemigos = data;
                //$scope.$apply();
                $("#listaEnemigos").toggle(true);
            });
        };

        $scope.iniciarAtaque = function(enemigo){
            var jsonAtaque = {
                Enemigo: enemigo
            };
            juegoService.iniciarAtaque(jsonAtaque);
        }

        




    }
    ]);

    function focusCanvas() {
        $("#divJuego canvas").focus();
    }

    angular.module('juego').directive('gameCanvas', function ($injector) {
        var linkFn = function (scope, ele, attrs) {
            
        };

        return {
            scope: true,
            templateUrl: "/SPA/frontOffice/directives/templateGameCanvas.html",
            link: linkFn
        }
    });
})();