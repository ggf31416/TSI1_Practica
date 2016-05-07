(function () {
    'use strict';
    angular.module('juego').controller("juegoCtrl", ["juegoService", "edificiosService", '$scope', '$rootScope',


    function (juegoService, edificiosService, $scope, $rootScope) {

        $rootScope.nombreJuego = "Atlas2";


        $scope.posicionarEdificio = function (nombre) {
            // llama a la funcion iniciarCustomDrag en el CrearCanvas.js
            iniciarCustomDrag(nombre);
        };

        $rootScope.listaEdificios = edificiosService.getAllTipoEdificios();
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

        $rootScope.listaUnidades = [
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
        ];

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

        var altoJuego = 600;
        var anchoJuego = 800;


        $scope.game;

        var menuCuartel;
        var cursors;
        var tile_size = 128;
        var mouse_sprite;
        var buildings;

        window.createGame = function (scope, injector) {
            // Create our phaser $scope.game
            $scope.game = new Phaser.Game(anchoJuego, altoJuego, Phaser.AUTO, 'divJuego', { preload: $scope.preload, create: create, update: update });
        };

        function cargarSprites() {

        }

        function crearInternalMenu() {
            menuCuartel = $scope.game.add.sprite(0, 0, 'cuartelMenu');
            menuCuartel.width = 64;
            menuCuartel.height = 64;
            menuCuartel.inputEnabled = true;
            menuCuartel.events.onInputDown.add(function () { crearDragEdificio('cuartel'); }, this);
            menuCuartel.fixedToCamera = true;
        }


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
                    $scope.game.load.image(e.Id, e.Imagen);
                }
            });
        }



        function create() {
            $scope.game.physics.startSystem(Phaser.Physics.ARCADE)
            // setea el tamaño del tablero en pixeles en 2000x2000 
            $scope.game.world.resize(2000, 2000);

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
            $scope.game.physics.arcade.enable(mouse_sprite);

            //seguirMouse();

            var tile = $scope.game.add.tileSprite(0, 0, $scope.game.world.width, $scope.game.world.height, "grass");
            tile.tileScale = new PIXI.Point(0.25, 0.25);


            buildings = $scope.game.add.physicsGroup();

            cursors = $scope.game.input.keyboard.createCursorKeys();
            //crearInternalMenu();

            var phaserJSON = $scope.game.cache.getJSON('jsonEdificios');
            console.log(phaserJSON);

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

        $scope.accionMouseOver = function () {
            console.log("Mouse Over: spriteDragged=" + spriteDragged);
            if (nombreEntidadDragged != null) {
                $scope.game.canvas.focus();
                crearDragEdificio(nombreEntidadDragged);
                nombreEntidadDragged = null;
            }
        }


        function activarSeguirMouse() {
            var borde = 50;
            $scope.game.camera.follow(mouse_sprite);
            $scope.game.camera.deadzone = new Phaser.Rectangle(borde, borde, anchoJuego - borde, altoJuego - borde);
        }

        function crearDragEdificio(imagen) {
            // worldX,Y obtienen la posicion del puntero en relacion al mundo
            // x,y solos obtienen en relacion a la camara

            var input_x = $scope.game.input.activePointer.worldX;
            var input_y = $scope.game.input.activePointer.worldY;

            var cuartel = $scope.game.add.sprite(input_x, input_y, imagen);

            cuartel.height = tile_size;
            cuartel.width = tile_size;

            cuartel.inputEnabled = true;
            cuartel.input.enableDrag();

            // al final lo hago manualmente en el update
            // tam_grilla_x,tam_grilla_y, ajustar a grilla al: arrastrar, soltar
            //cuartel.input.enableSnap(tile_size, tile_size, true, true);

            cuartel.events.onDragStop.add(aceptarPosicionEdificio, this);

            $scope.game.physics.arcade.enable(cuartel);
            cuartel.anchor.x = 0;
            cuartel.anchor.y = 0;

            spriteDragged = cuartel;


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
                //enviarSignalR();
            }, this);
        }


        function aceptarPosicionEdificio(sprite, pointer) {
            console.log("aceptarPosicionEdificio");
            if (!$scope.game.physics.arcade.overlap(sprite, buildings)) {
                if (!checkOverlap(sprite, menuCuartel)) {
                    sprite.input.draggable = false;
                    buildings.add(sprite);
                    $scope.game.debug.reset();
                    sprite.alpha = 0.8;
                    //sprite.tint = 0xFFFF00;
                    construccion(sprite);
                }
            }
            else {
                sprite.destroy();
                $scope.game.debug.reset();
            }
            spriteDragged = null;
        }

        function checkOverlap(spriteA, spriteB) {
            if (spriteA == null || spriteB == null) return false;
            var boundsA = spriteA.getBounds();
            var boundsB = spriteB.getBounds();

            return Phaser.Rectangle.intersects(boundsA, boundsB);
        }

        function update() {
            /*if ($scope.game.input.mousePointer.isDown) {
                activarSeguirMouse();
                seguirMouse();
            }
            else {
                $scope.game.camera.unfollow();
            }*/
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
                spriteDragged.x = Math.floor($scope.game.input.activePointer.worldX / tile_size) * tile_size;
                spriteDragged.y = Math.floor($scope.game.input.activePointer.worldY / tile_size) * tile_size

                if ($scope.game.physics.arcade.overlap(spriteDragged, buildings)) {
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

    function allowDrop(ev) {
        ev.preventDefault();
        //focusCanvas(); 
        return false;
    }

    function focusCanvas() {
        $("#divJuego canvas").focus();
    }

    function drop(ev) {
        ev.preventDefault();
        accionMouseOver();
        //var data = ev.dataTransfer.getData("text");
        //ev.target.appendChild(document.getElementById(data));
        //crearDragEdificio(data);
    }

    angular.module('juego').directive('gameCanvas', function ($injector) {
        var linkFn = function (scope, ele, attrs) {
            window.createGame(scope, $injector);
        };

        return {
            scope: true,
            templateUrl: "/SPA/frontOffice/directives/templateGameCanvas.html",
            link: linkFn
        }
    });
})();