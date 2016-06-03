
var Unidad_Info = function (unit_id, jugador,hp, rango,velocidad) {
    hp = hp || 100;
    rango = rango || 8;
    this.unit_id = unit_id;
    this.jugador = jugador;
    //this.ataque = ataque;
    //this.defensa = defensa;
    this.hp = hp;
    this.max_hp = hp;
    this.velocidad = velocidad;
    this.path = [];
    this.target = null;
    this.rango = rango;
}


