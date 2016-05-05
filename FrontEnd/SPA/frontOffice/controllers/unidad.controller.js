(function () {
'use strict';
angular.module('unidades').controller("unidadesCtrl", ["unidadesService", '$scope', '$rootScope',

    function (unidadesService, $scope, $rootScope) {

        //--------------Inicializacion de variables---------------------
        $scope.title = "Unidades";

        $scope.lista = $rootScope.listaUnidades;

        $scope.showAsociationDialog = false;

        $scope.entidad = {};

        //--------------Fin inicializacion de variables---------------------
        $scope.darDeAlta = function () {
            var newEntity = {
                Nombre: $scope.entidad.nombre,
                Imagen: $scope.entidad.imagen,
                Vida: $scope.entidad.vida,
                Ataque: $scope.entidad.ataque,
                Defensa: $scope.entidad.defensa,
                TiempoConstruccion: $scope.entidad.tiempoContruccion
            }
            $rootScope.listaUnidades.push(newEntity);

            //console.debug(edificiosService.altaTipoEntidad(newEntity));
        }

        $scope.edit = function (index) {
            $scope.editForm = true;
            $scope.indexSelected = index;
            var e = $scope.lista[$scope.indexSelected];

            $scope.entidad.nombre = e.Nombre;
            $scope.entidad.imagen = e.Imagen;
            $scope.entidad.vida = e.Vida;
            $scope.entidad.ataque = e.Ataque;
            $scope.entidad.defensa = e.Defensa;
            $scope.entidad.tiempoContruccion = e.TiempoConstruccion;
        }

        $scope.modificar = function () {
            $scope.lista[$scope.indexSelected].Nombre = $scope.entidad.nombre;
            $scope.lista[$scope.indexSelected].Imagen = $scope.entidad.imagen;
            $scope.lista[$scope.indexSelected].Vida = $scope.entidad.vida;
            $scope.lista[$scope.indexSelected].Ataque = $scope.entidad.ataque;
            $scope.lista[$scope.indexSelected].Defensa = $scope.entidad.defensa;
            $scope.lista[$scope.indexSelected].TiempoConstruccion = $scope.entidad.tiempoContruccion;


            //console.debug(edificiosService.modificarTipoEntidad($scope.lista[index]));
        }

        $scope.uploadFile = function (input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onloadend = function (e) {

                    //Sets the Old Image to new New Image
                    $('#photo-id').attr('src', e.target.result);

                    //Create a canvas and draw image on Client Side to get the byte[] equivalent
                    var canvas = document.createElement("canvas");
                    var imageElement = document.createElement("img");

                    imageElement.setAttribute('src', e.target.result);
                    canvas.width = imageElement.width;
                    canvas.height = imageElement.height;
                    var context = canvas.getContext("2d");
                    context.drawImage(imageElement, 0, 0);
                    var base64Image = canvas.toDataURL("image/jpeg");

                    //Removes the Data Type Prefix 
                    //And set the view model to the new value
                    $scope.entidad.imagen = base64Image.replace(/data:image\/jpeg;base64,/g, '');
                    console.debug($scope.entidad.imagen);
                    console.debug(base64Image);
                }

                //Renders Image on Page
                reader.readAsDataURL(input.files[0]);
            }

        };

    }
]);
   
})();