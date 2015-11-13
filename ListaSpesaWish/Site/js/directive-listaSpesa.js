app.directive('listaSpesa', ['factories', 'user-service', '$location', function (lswFct, userSrv, $location) {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/lista-spesa.html',
        controller: function () {
            var _this = this;

            this.addList = function () {
                var queryParameters = {
                    Nome: "Lista",
                    UtentiListaSpesa: [],
                    VoceListaSpesa: []
                };

                lswFct.listaSpesa.add(queryParameters).$promise
                .then(function (success) {
                     _this.lista = success;
                });
            };


            this.loadVoci = function () {                
                lswFct.voce.getAll().$promise
                .then(function (success) {
                    _this.voci = success;
                });
            };

            //this.sendProposal = function (details) {
            //    var newLocation = '/mail/compose/' + details.user.id + '//' + details.libraryComponent.libraryComponentId;
            //    $location.path(newLocation);
            //}

            this.loadVoci();
        },
        controllerAs: 'listaSpesa'
    };
}]);