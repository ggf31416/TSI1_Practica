
var Unidad_Info = function (unit_id, jugador, ataque, defensa, hp, velocidad) {
    this.unit_id = unit_id;
    this.jugador = jugador;
    this.ataque = ataque;
    this.defensa = defensa;
    this.hp = hp;
    this.max_hp = hp;
    this.velocidad = velocidad;
    this.path = [];
}
