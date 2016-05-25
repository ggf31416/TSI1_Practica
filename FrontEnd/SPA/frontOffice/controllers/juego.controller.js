(function () {
    'use strict';
    angular.module('juego').controller("juegoCtrl", ["$http", "$q","juegoService", "edificiosService", "unidadesService",'$scope', '$rootScope',

   

    function ($http,$q,juegoService, edificiosService, unidadesService, $scope, $rootScope) {


        var selectedUnit = null;

        $rootScope.nombreJuego = "Atlas2";

        $scope.contador = 1;
        
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
            edificiosService.getAllTipoEdificios(),
            unidadesService.getAllTipoUnidades()
        ]).then(function (data) {
            $rootScope.listaEdificios = data[0];
            $rootScope.listaUnidades = data[1];
            window.createGame();
        });

        /*[
            {
                Nombre: "Cuartel",
                Imagen: "/SPA/backOffice/cuartel.jpg",
                Vida: 500,
                Ataque: 12,
                Defensa: 30,
                TiempoConstruccion: "20/s"
            },
            {
                Nombre: "Granja",
                Imagen: "/SPA/backOffice/granja.jpg",
                Vida: 450,
                Ataque: 0,
                Defensa: 10,
                TiempoConstruccion: "100/s"
            }
    ];*/

        /*unidadesService.getAllTipoUnidades().
            then(function (data) {
                $rootScope.listaUnidades = data;
                console.log("Lista unidades:")
                console.log(data)
            })
            .catch(function(err) {
                alert(err);
            })
        */

        

        
        /*[
                {
                    Nombre: "Soldado",
                    Imagen: "/SPA/backOffice/soldado.jpg",
                    Vida: 500,
                    Ataque: 12,
                    Defensa: 30,
                    TiempoConstruccion: "20/s"
                },
                {
                    Nombre: "Arquero",
                    Imagen: "/SPA/backOffice/arquero.jpg",
                    Vida: 450,
                    Ataque: 0,
                    Defensa: 10,
                    TiempoConstruccion: "100/s"
                }
        ];*/

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

        $scope.estadoJuego = {
            edificios: [
                {
                    id: 3,
                    posX: 12,
                    posY: 8
                },
                {
                    id: 22,
                    posX: 12,
                    posY: 20
                }
            ],
            unidades_desplegadas: [],
            unidades: []
        }


        var altoJuego = 600;
        var anchoJuego = 800;


        $scope.game;

        var menuCuartel;
        var cursors;
        
        var unit_size = 20;
        var tile_size = unit_size * 4;
        var tablero_size = 10 * tile_size;
        var mouse_sprite;
        var buildings;
        var unidades_desplegadas;
        var unidadesPorId = {}

        var nombreJugador;

        var estaEnBatalla = false;

        $scope.animaciones = {}

        window.createGame = function (scope, injector) {
            // Create our phaser $scope.game
            $scope.game = new Phaser.Game(anchoJuego, altoJuego, Phaser.AUTO, 'divJuego', { preload: $scope.preload, create: create, update: update });
            pedirNombre();
        };

        function pedirNombre() {
            nombreJugador = window.prompt("Jugador?", "");

        }

        


        /*function crearInternalMenu() {
            menuCuartel = $scope.game.add.sprite(0, 0, 'cuartelMenu');
            menuCuartel.width = 64;
            menuCuartel.height = 64;
            menuCuartel.inputEnabled = true;
            menuCuartel.events.onInputDown.add(function () { crearDragEdificio('cuartel'); }, this);
            menuCuartel.fixedToCamera = true;
        }*/

        function mostrarUnidades(b) {
            $('#divEdificios').toggle(!b);
            $('#divUnidades').toggle(b);
        }

        function crearEdificioInmediato(data) {
            var idSprite =  data.Id;
            var edificio = $scope.game.add.sprite(data.PosX * tile_size, data.PosY * tile_size, idSprite);
            edificio.height = tile_size;
            edificio.width = tile_size;
            edificio.inputEnabled = true;
            clickVisibleUnidades(edificio, true);
            buildings.add(edificio);
        }

        function crearUnidadInmediato(data) {
            if (!unidadesPorId[data.Unit_id]) {
                var idSprite = data.Id;
                var unit = $scope.game.add.sprite(data.PosX * unit_size, data.PosY * unit_size, idSprite);
                unit.height = tile_size;
                unit.width = tile_size;
                unit.inputEnabled = true;
                clickVisibleUnidades(unit, true);
                unit.Unit_id = data.Unit_id;
                unidades_desplegadas.add(unit);
                unidadesPorId[unit.Unit_id] = unit;
                console.log("Unidad agregada desde remoto json:" + data);
            }
            else {
                console.log("Unidad ya estaba json:" + data);
            }
        }

        $scope.cargarDesdeEstado = function () {
            if (buildings){
                // quita todos los edificios
                buildings.removeAll(true);
            }
            if (unidades_desplegadas) {
                unidades_desplegadas.removeAll(true);
            }
            $scope.estadoJuego.edificios.forEach (function (e) {
                crearEdificioInmediato(e);
            });
            $scope.estadoJuego.unidades_desplegadas.forEach(function (e) {
                crearUnidadInmediato(e);
            });
        };

        $scope.iniciarSignalR = function () {
            // Declare a proxy to reference the hub.
            $scope.tablero_signalR = $.connection.chatHub;
            // Create a function that the hub can call to broadcast messages.
            $scope.tablero_signalR.client.broadcastMessage = function (name, message) {
                var msjJSON = JSON.parse(message);
                if (msjJSON.A == "AddEd") {
                    crearEdificioInmediato(msjJSON);
                }
                if (msjJSON.A == "AddUn") {
                    crearUnidadInmediato(msjJSON);
                }
                if (msjJSON.A == "MoveUnit") {
                    var unit = unidadesPorId[msjJSON.Unit_id];
                    if (unit) {
                        moverUnidad(unit, msjJSON.Path);
                    }
                    
                }
                //$scope.estadoJuego.edificios = msjJSON.edificios;
                //$scope.estadoJuego.unidades_desplegadas = msjJSON.unidades;
                $scope.estadoJuego.edificios.push(msjJSON);
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

        $scope.iniciarSignalR();

        $scope.preload = function() {

            //  You can fill the preloader with as many assets as your $scope.game requires

            //  Here we are loading an image. The first parameter is the unique
            //  string by which we'll identify the image later in our code.

            //  The second parameter is the URL of the image (relative)
            // por ahora harcodeado
            //$scope.game.load.json('jsonEdificios', '/Entidades/GetAllTipoEdificios');
            console.log("preload");

            // activa la carga desde cualquier dominio (se puede restringir al dominio del servidor de imagenes)
            $scope.game.load.crossOrigin = 'anonymous';

            $scope.game.load.image('grass', '/Content/img/grass.jpg');
            $scope.game.load.image('cuartel', '/Content/img/Barracks4.png');
            $scope.game.load.image('cuartelMenu', '/Content/img/cuartel.jpg');

            
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
                }
            });
        }


        function clickVisibleUnidades(sprite, b) {
            sprite.events.onInputDown.add(function () { mostrarUnidades(b) }, this);
        }

        function create() {
            $scope.game.physics.startSystem(Phaser.Physics.ARCADE)
            // setea el tamaño del tablero en pixeles en 2000x2000 
            $scope.game.world.resize(tablero_size, tablero_size);

            // disableVisibilityChange intenta que el juego siga corriendo aunque pierda el foco
            // no es posible en todos los casos (por ejemplo puede no funcionar al cambiar a otro tab dependiendo del navegador)
            // pero deberia permitir que siga funcionando al hacer click afuera, lo que reduce problemas de UI
            $scope.game.stage.disableVisibilityChange = true;

            //  The deadzone is a Rectangle that defines the limits at which the camera will start to scroll
            //  It does NOT keep the target sprite within the rectangle, all it does is control the boundary
            //  at which the camera will start to move. So when the sprite hits the edge, the camera scrolls
            //  (until it reaches an edge of the world)



            mouse_sprite = $scope.game.add.sprite($scope.game.world.centerX, $scope.game.world.centerY, "grass");
            mouse_sprite.alpha = 0;
            $scope.game.physics.arcade.enable(mouse_sprite,this);

            //seguirMouse();

            var tile = $scope.game.add.tileSprite(0, 0, $scope.game.world.width, $scope.game.world.height, "grass");
            tile.tileScale = new PIXI.Point(0.25, 0.25);
            tile.inputEnabled = true;
            clickVisibleUnidades(tile, false);


            buildings = $scope.game.add.physicsGroup();
            unidades_desplegadas = $scope.game.add.physicsGroup();

            cursors = $scope.game.input.keyboard.createCursorKeys();
            //crearInternalMenu();

            var phaserJSON = $scope.game.cache.getJSON('jsonEdificios');
            console.log(phaserJSON);

            $scope.cargarDesdeEstado();
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
            console.log("Mouse Over: spriteDragged=" + spriteDragged);
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
            console.log("aceptarPosicionEdificio");
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


        $scope.aceptarPosicionUnidad = function (sprite, pointer) {
            console.log("aceptarPosicionUnidad");
            if (!$scope.game.physics.arcade.overlap(sprite, buildings) && !$scope.game.physics.arcade.overlap(sprite, unidades_desplegadas)) {
                var input_x = sprite.x / unit_size;
                var input_y = sprite.y / unit_size;
                sprite.input.draggable = false;
                unidades_desplegadas.add(sprite);
                $scope.game.debug.reset();
                sprite.alpha = 1.0;
                sprite.tint = 0xFFFFFF;
                hacerSeleccionableUnidad(sprite);
                sprite.unit_id = nombreJugador + "#" + $scope.contador++;
                unidadesPorId[sprite.unit_id] = sprite;
                //sprite.id_logico = $scope.contador--; // asigno un id automatico que luego cambio}
                juegoService.posicionarUnidad(sprite.id, sprite.unit_id, input_x, input_y,nombreJugador).then(function (data) {
                    //sprite.id_logico = data.data.ret;
                    //unidadesPorId[sprite.id_logico] = sprite;
                    
                });
            }
            else {
                sprite.destroy();
                $scope.game.debug.reset();
            }
            spriteDragged = null;
        }

        function moverUnidad(sprite, paths) {
            var tweenAnterior = null;
            var primerTween = null;
            for (var i in paths) {
                var tiempo = 1000; // paths[i].s * 1000;
                var tween = $scope.game.add.tween(sprite).to({ x: paths[i].x * unit_size, y: paths[i].y * unit_size }, tiempo , Phaser.Easing.Linear.None, false);
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
            $scope.animaciones[sprite.Unit_id] = primerTween;
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

        function update() {
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
            $scope.game.debug.text($scope.game.input.activePointer.x, 200, 32);
            $scope.game.debug.text($scope.game.input.activePointer.y, 200, 64);
            if (spriteDragged != null) {
                $scope.game.debug.text("isDragged: " + spriteDragged.input.isDragged, 200, 96);
            }
            $scope.game.physics.arcade.collide(unidades_desplegadas, buildings,function(){console.log("Colision")});

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