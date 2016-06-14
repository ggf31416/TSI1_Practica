
var Unidad_Info = function (unit_id, jugador,hp, rango,velocidad,ataque,defensa,target) {
    hp = hp || 100;
    rango = rango || 8;
    target = target || null;
    ataque = ataque || 0;
    this.unit_id = unit_id;
    this.jugador = jugador;
    this.ataque = ataque;
    this.defensa = defensa;
    this.hp = hp;
    this.max_hp = hp;
    this.velocidad = velocidad;
    this.path = [];
    this.target = target;
    this.rango = rango;
}


