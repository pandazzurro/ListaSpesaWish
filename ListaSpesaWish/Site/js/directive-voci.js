/// <reference path="app.js" />
/// <reference path="factory.js" />
/// <reference path="services.js" />
/// <reference path="userController.js" />

app.directive('voci', ['factories', 'user-service', '$location', function (lswFct, userSrv, $location) {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/voci.html',
        controller: function ($scope) {
            var _this = this;
            _this.vociList = [];
            _this.voceSelected = {};
            _this.timeout = 3000;
            
            this.loadVoci = function () {                
                lswFct.voce.getAll().$promise
                .then(function (success) {
                    _this.vociList = success;
                });
            };

            // gestore delle immagini caricate.
            $scope.currentImage = '';
            $scope.currentCroppedImage = '';
            var handleFileSelect = function (evt) {
                var file = evt.currentTarget.files[0];
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.currentImage = evt.target.result;
                        $('img-crop>canvas').css({ 'margin-top': 0, 'margin-left': 0 });
                    });
                };
                reader.readAsDataURL(file);
            };
            angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

            this.removeVoce = function (voce) {
                UIkit.modal.confirm("Sei sicuro di rimuovere la voce selezionata?", function () {
                    UIkit.notify('Voce ' + voce.Name + ' è stata rimossa', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                    lswFct.voce.remove({ id: voce.IdVoce });
                    for (i = 0; i < _this.vociList.length; i++) {
                        if (_this.vociList[i].IdVoce == voce.IdVoce)
                            _this.vociList.splice(i, 1);
                    }
                });
            }
            
            this.addVoce = function (voce) {
                var queryParameters ={
                        Image: $scope.currentCroppedImage.replace(/^data:image\/(png|jpg);base64,/, ""),
                        Name: _this.voceSelected.Name
                }

                lswFct.voce.add(queryParameters).$promise
                .then(function (success) {
                    _this.vociList.push(success);
                    UIkit.notify('Voce ' + success.Name + ' è stata aggiunta', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                    _this.modalInvertStatus("addVoce");
                });
                $scope.currentImage = '';
                $scope.currentCroppedImage = '';
                _this.voceSelected = {};
            };

            this.canSave = function () {
                if ($scope.currentImage == '' && (_this.voceSelected.Name == '' || _this.voceSelected.Name == undefined))
                    return false;
                return true;
            }

            this.modalInvertStatus = function (modalId) {
                var modal = UIkit.modal("#" + modalId);
                if (modal.isActive()) {
                    modal.hide();
                } else {
                    modal.show();
                }
            }

            // OnLoad
            this.loadVoci();
        },
        controllerAs: 'voci'
    };
}]);