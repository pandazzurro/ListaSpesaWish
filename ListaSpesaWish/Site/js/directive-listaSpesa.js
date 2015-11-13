/// <reference path="app.js" />
/// <reference path="factory.js" />
/// <reference path="services.js" />
/// <reference path="userController.js" />

app.directive('listaSpesa', ['factories', 'user-service', '$location', function (lswFct, userSrv, $location) {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/lista-spesa.html',
        controller: function () {
            var _this = this;
            _this.vociAdded = [];
            _this.utentiAdded = [];
            _this.lista = {};
            _this.voci = [];
            _this.utenti = [];            


            this.pulisciLista = function () {
                _this.vociAdded = [];
                _this.utentiAdded = [];
                _this.lista = {};
                _this.voci = [];
                _this.utenti = [];
            };
            this.addList = function () {
                var queryParameters = {
                    Nome: _this.lista.Nome,
                    UtentiListaSpesa: [],
                    VoceListaSpesa: []
                };

                lswFct.listaSpesa.add(queryParameters).$promise
                .then(function (success) {
                    _this.lista = success;

                });
            };
            this.addVoce = function (voce) {
                newVoce = jQuery.extend(true, {}, voce);
                newVoce.incremental = _this.duplicateVoceIncremental();
                _this.vociAdded.push(newVoce);
                UIkit.notify('Aggiunta voce: ' + newVoce.Name, { status: 'success', timeout: 5000 });
            }
            this.removeVoce = function (voce) {                
                _this.vociAdded.pop(voce);                
            }
            this.addUtente = function (utente) {
                if (_this.utentiAdded.indexOf(utente) < 0) {
                    _this.utentiAdded.push(utente);
                    UIkit.notify('Aggiunta utente: ' + utente.Username, { status: 'success', timeout: 5000 });
                }   
                else
                    UIkit.notify('Utente già presente', { status: 'danger', timeout: 5000 });
            }
            this.removeUtente = function (utente) {                
                _this.utentiAdded.pop(utente);                
            }

            this.loadVoci = function () {                
                lswFct.voce.getAll().$promise
                .then(function (success) {
                    _this.voci = success;
                });
            };

            this.loadUtenti = function () {
                lswFct.utente.getAll().$promise
                .then(function (success) {
                    _this.utenti = success;
                    _this.loadUtenteCorrenteInLista();
                });
            };

            // Carico l'utente corrente nella lista della spesa. 
            this.loadUtenteCorrenteInLista = function () {
                if (_this.utenti != undefined) {
                    _this.utentiAdded = [];
                    _this.utenti.forEach(function (u) {
                        if (u.IdUtente == userSrv.getCurrentUser().IdUtente) {
                            _this.utentiAdded.push(u);
                        }
                    })
                } else {
                    UIkit.notify('Nessun utente presente', { status: 'warning', timeout: 5000 });
                }
            }

            // Discrimino le voci doppie nella lista della spesa
            this.duplicateVoceIncremental = function () {
                if (_this.vociAdded.length > 1) {
                    _this.vociAdded.sort(function compareNumbers(a, b) {
                        return a.incremental - b.incremental;
                    })[_this.vociAdded.length - 1] + 1;
                }
                else
                    return _this.vociAdded.length;                
            }


            // OnLoad
            this.loadVoci();
            this.loadUtenti();
        },
        controllerAs: 'listaSpesa'
    };
}]);