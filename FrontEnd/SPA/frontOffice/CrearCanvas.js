var altoJuego = 600;
var anchoJuego = 800;


var game;

var menuCuartel;
var cursors;
var tile_size = 128;
var mouse_sprite;
var buildings;

window.createGame = function (scope, injector) {
    // Create our phaser game
   game = new Phaser.Game(anchoJuego, altoJuego, Phaser.AUTO, 'divJuego', { preload: preload, create: create, update: update });
};

function cargarSprites() {
    $scope.listaEdificios.foreach(function (e) {
        game.load.image(e.Nombre, e.Imagen);
    });
}

function crearInternalMenu() {
    menuCuartel = game.add.sprite(0, 0, 'cuartelMenu');
    menuCuartel.width = 64;
    menuCuartel.height = 64;
    menuCuartel.inputEnabled = true;
    menuCuartel.events.onInputDown.add(function () { crearDragEdificio('cuartel'); }, this);
    menuCuartel.fixedToCamera = true;
}


function preload() {

    //  You can fill the preloader with as many assets as your game requires

    //  Here we are loading an image. The first parameter is the unique
    //  string by which we'll identify the image later in our code.

    //  The second parameter is the URL of the image (relative)
    // por ahora harcodeado
    game.load.image('grass', '/Content/img/grass.jpg');
    game.load.image('cuartel', '/Content/img/Barracks4.png');
    game.load.image('cuartelMenu', '/Content/img/cuartel.jpg');
    //cargarSprites();
}



function create() {
    game.physics.startSystem(Phaser.Physics.ARCADE)
    game.world.resize(2000, 2000);

    //  The deadzone is a Rectangle that defines the limits at which the camera will start to scroll
    //  It does NOT keep the target sprite within the rectangle, all it does is control the boundary
    //  at which the camera will start to move. So when the sprite hits the edge, the camera scrolls
    //  (until it reaches an edge of the world)



    mouse_sprite = game.add.sprite(game.world.centerX, game.world.centerY, "grass");
    mouse_sprite.alpha = 0;
    game.physics.arcade.enable(mouse_sprite);

    //seguirMouse();

    var tile = game.add.tileSprite(0, 0, game.world.width, game.world.height, "grass");
    tile.tileScale = new PIXI.Point(0.25, 0.25);


    buildings = game.add.physicsGroup();

    cursors = game.input.keyboard.createCursorKeys();
    crearInternalMenu();
   


}




function activarSeguirMouse() {
    var borde = 50;
    game.camera.follow(mouse_sprite);
    game.camera.deadzone = new Phaser.Rectangle(borde, borde, anchoJuego - borde, altoJuego - borde);
}

function crearDragEdificio(imagen) {
    // worldX,Y obtienen la posicion del puntero en relacion al mundo
    // x,y solos obtienen en relacion a la camara

    var input_x = game.input.activePointer.worldX;
    var input_y = game.input.activePointer.worldY;

    var cuartel = game.add.sprite(input_x, input_y, imagen);

    cuartel.height = tile_size;
    cuartel.width = tile_size;
    cuartel.anchor.x = 0.5;
    cuartel.anchor.y = 0.5;
    cuartel.inputEnabled = true;
    cuartel.input.enableDrag();
    // tam_grilla_x,tam_grilla_y, ajustar a grilla al: arrastrar, soltar
    cuartel.input.enableSnap(tile_size, tile_size, true, true);

    cuartel.events.onDragStart.add(onDragStartBuild, this);
    cuartel.events.onDragStop.add(onDragStopBuild, this);
    cuartel.events.onDragUpdate.add(onDragUpdateBuild);

    game.physics.arcade.enable(cuartel);
    cuartel.input.startDrag(this.game.input.activePointer);
    //var cuadrado = crearCuadrado(cuartel);
    //cuadrado.bringToTop();
    //cuartel.addChild(cuadrado);

}

function crearCuadrado(sprite) {
    var r = new Phaser.Rectangle(sprite.x, sprite.y, tile_size, tile_size);
    return r;
    /*var drawnObject;
    var width = 2 * sprite.body.width; // example;
    var height = 2 * sprite.body.height; // example;
    var bmd = game.add.bitmapData(width, height);

    bmd.ctx.beginPath();
    bmd.ctx.rect(0, 0, width, height);
    bmd.ctx.fillStyle = '#00ff00';
    bmd.ctx.fill();
    drawnObject = game.add.sprite(sprite,sprite.camera.y, bmd);
    drawnObject.anchor.setTo(0, 0);
    return drawnObject;*/


}


function onDragStartBuild(sprite, pointer) {

}


function onDragUpdateBuild(sprite, pointer) {
    //game.debug.body(sprite);
    //crearCuadrado(sprite.x, sprite.y);
    //sprite.children[0].moveTo(sprite.x, sprite.y);
    if (game.physics.arcade.overlap(sprite, buildings)) {
        sprite.tint = 0xFF0000;
        game.debug.body(sprite, 'rgba(255, 0, 0, 0.5)');
        //game.debug.geom(sprite.children[0], 'rgba(0,255,0,0.5)');
    }
    else {
        sprite.tint = 0x00FF00;
        //game.debug.geom(sprite.children[0], 'rgba(0,0,255,0.5)');
        game.debug.body(sprite);
    }

}

function construccion(sprite) {
    var completionSprite = game.add.graphics(0, 0);
    completionSprite.beginFill(0xFFFF00, 0.5);
    completionSprite.bounds = new PIXI.Rectangle(sprite.x, sprite.y, sprite.width, sprite.height);
    completionSprite.drawRect(sprite.x - sprite.width / 2, sprite.y - sprite.height / 2, sprite.width, sprite.height);
    construir(sprite, completionSprite, 5);
}

function construir(sprite, rec, segundos) {
    game.time.events.add(Phaser.Timer.SECOND * segundos, function () {
        rec.destroy();
        sprite.alpha = 1;
        sprite.tint = 0xFFFFFF;
        //enviarSignalR();
    }, this);
}


function onDragStopBuild(sprite, pointer) {
    if (!game.physics.arcade.overlap(sprite, buildings)) {
        if (!checkOverlap(sprite, menuCuartel)) {
            sprite.input.draggable = false;
            buildings.add(sprite);


            game.debug.reset();
            sprite.alpha = 0.8;
            //sprite.tint = 0xFFFF00;
            construccion(sprite);
        }

    }
    else {
        sprite.destroy();
        game.debug.reset();
    }

}

function checkOverlap(spriteA, spriteB) {

    var boundsA = spriteA.getBounds();
    var boundsB = spriteB.getBounds();

    return Phaser.Rectangle.intersects(boundsA, boundsB);
}

function update() {
    /*if (game.input.mousePointer.isDown) {
        activarSeguirMouse();
        seguirMouse();
    }
    else {
        game.camera.unfollow();
    }*/
    if (cursors.up.isDown) {
        game.camera.y -= 4;
    }
    else if (cursors.down.isDown) {
        game.camera.y += 4;
    }

    if (cursors.left.isDown) {
        game.camera.x -= 4;
    }
    else if (cursors.right.isDown) {
        game.camera.x += 4;
    }

}

function seguirMouse() {
    sprite = mouse_sprite;
    //  only move when you click

    //  400 is the speed it will move towards the mouse
    game.physics.arcade.moveToPointer(sprite, 100);

    //  if it's overlapping the mouse, don't move any more
    if (Phaser.Rectangle.contains(sprite.body, game.input.x, game.input.y)) {
        sprite.body.velocity.setTo(0, 0);
    }

    /*else {
        sprite.body.velocity.setTo(0, 0);
    }*/
}
