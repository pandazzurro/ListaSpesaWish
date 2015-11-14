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
            _this.listeAll = [];
            _this.voci = [];
            _this.utenti = [];
            _this.editMode = false;

            this.pulisciLista = function () {
                _this.vociAdded = [];
                _this.utentiAdded = [];
                _this.lista = {};
                _this.loadUtenteCorrenteInLista();
                _this.modalInvertStatus("pulisciLista");
            };

            this.loadLists = function () {
                lswFct.listaSpesa.getAll().$promise
                .then(function (success) {
                    _this.listeAll = success;
                    _this.editMode = true;
                });
            }

            this.importListElement = function (l) {
                var le = l;

                _this.modalInvertStatus("caricaLista");
            }

            this.inviaMail = function (lista) {
                UIkit.modal.confirm("Sei sicuro di volere inviare una mail a tutti gli utenti della lista corrente?", function () {
                    _this.modalInvertStatus("caricaLista");
                    lswFct.listaSpesa.mail(lista).$promise
                    .then(function (success) {
                        UIkit.notify('Lista [' + lista.Nome + '] inviata con successo: ', { status: 'success', timeout: 5000, pos: 'bottom-center' });                        
                    });
                });
            }

            this.removeLista = function (lista) {
                UIkit.modal.confirm("Sei sicuro di rimuovere la lista selezionata?", function () {
                    _this.modalInvertStatus("caricaLista");
                    lswFct.listaSpesa.remove({ id: lista.IdListaSpesa }).$promise
                    .then(function (success) {
                        UIkit.notify('Lista [' + lista.Nome + '] rimossa con successo: ', { status: 'success', timeout: 5000, pos: 'bottom-center' });                        
                    });
                });
            }

            this.addList = function () {
                var queryParameters = {                
                    Nome: _this.lista.Nome,
                    UtentiListaSpesa: [],
                    VoceListaSpesa: []
                };

                lswFct.listaSpesa.add(queryParameters).$promise
                .then(function (success) {
                    _this.lista = success;
                    _this.addListComponent(_this.lista);
                    _this.modalInvertStatus("salvaLista");
                });
            };

            this.addListComponent = function(listaSpesa){
                var idUtentiListaSpesa = 0, idVoceListaSpesa = 0;
                var listaSpesa = { IdListaSpesa: listaSpesa.IdListaSpesa };

                _this.utentiAdded.forEach(function(u){
                    var UtentiListaSpesaParameter = {
                        IdUtentiListaSpesa: idUtentiListaSpesa,
                        Utente: { IdUtente: u.IdUtente },
                        ListaSpesa: listaSpesa
                    };

                    lswFct.utenteListaSpesa.add(UtentiListaSpesaParameter).$promise
                    .then(function (success) {
                    });
                });
                
                var vociSelezionate = [];
                _this.vociAdded.forEach(function (v) {
                    var VoceListaSpesaParameter = {
                        IdVoceListaSpesa: idVoceListaSpesa,
                        Comprata : v.Comprata ? true : false,
                        Voce: {IdVoce : v.IdVoce},
                        ListaSpesa: listaSpesa
                    }
                    lswFct.voceListaSpesa.add(VoceListaSpesaParameter).$promise
                   .    then(function (success) {
                    });
                });
            }

            this.addVoce = function (voce) {
                newVoce = jQuery.extend(true, {}, voce);
                newVoce.incremental = _this.duplicateVoceIncremental();
                _this.vociAdded.push(newVoce);
                UIkit.notify('Aggiunta voce: ' + newVoce.Name, { status: 'success', timeout: 5000, pos: 'bottom-center' });
            }
            this.removeVoce = function (voce) {                
                _this.vociAdded.pop(voce);                
            }
            this.addUtente = function (utente) {
                if (_this.utentiAdded.indexOf(utente) < 0) {
                    _this.utentiAdded.push(utente);
                    UIkit.notify('Aggiunta utente: ' + utente.Username, { status: 'success', timeout: 5000, pos: 'bottom-center' });
                }   
                else
                UIkit.notify('Utente già presente', { status: 'danger', timeout: 5000, pos:'bottom-center' });
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
                    UIkit.notify('Nessun utente presente', { status: 'warning', timeout: 5000, pos:'bottom-center' });
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
            this.loadUtenti();
        },
        controllerAs: 'listaSpesa'
    };
}]);