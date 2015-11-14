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
            _this.timeout = 3000;
            _this.editMode = false;

            this.pulisciLista = function (ExceptCurrentUser, ModalChange) {
                _this.vociAdded = [];
                _this.utentiAdded = [];
                _this.lista = {};
                if (ExceptCurrentUser) { }
                else {
                    _this.loadUtenteCorrenteInLista();
                    if (ModalChange === undefined)
                        _this.modalInvertStatus("pulisciLista");
                    _this.editMode = false;
                }
            };

            this.loadLists = function () {
                lswFct.listaSpesa.get({ id: userSrv.getCurrentUser().IdUtente }).$promise
                .then(function (success) {
                    _this.listeAll = success;                   
                });
            }

            this.importListElement = function (l) {
                _this.pulisciLista(true);

                _this.lista = l;
                l.VociListaSpesa.forEach(function (vls) {                    
                    lswFct.voceListaSpesa.get({ id: vls.IdVoceListaSpesa }).$promise
                    .then(function (voceToAdd) {
                        voceToAdd.Voce.Comprata = voceToAdd.Comprata;
                        voceToAdd.Voce.IdVoceListaSpesa = voceToAdd.IdVoceListaSpesa;
                        _this.vociAdded.push(voceToAdd.Voce);
                    });
                });

                l.UtentiListaSpesa.forEach(function (uls) {                    
                    lswFct.utenteListaSpesa.get({ id: uls.IdUtentiListaSpesa }).$promise
                    .then(function (utenteToAdd) {
                        utenteToAdd.Utente.IdUtentiListaSpesa = utenteToAdd.IdUtentiListaSpesa;
                        _this.utentiAdded.push(utenteToAdd.Utente);
                    });
                });
               
                _this.editMode = true;
                UIkit.notify('Lista [' + l.Nome + '] caricata con successo: ', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                _this.modalInvertStatus("caricaLista");
            }

            this.inviaMail = function (lista) {
                UIkit.modal.confirm("Sei sicuro di volere inviare una mail a tutti gli utenti della lista corrente?", function () {
                    _this.modalInvertStatus("caricaLista");
                    lswFct.listaSpesa.mail(lista).$promise
                    .then(function (success) {
                        UIkit.notify('Lista [' + lista.Nome + '] inviata con successo: ', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });                        
                    });
                });
            }

            this.removeLista = function (lista) {
                UIkit.modal.confirm("Sei sicuro di rimuovere la lista selezionata?", function () {
                    _this.modalInvertStatus("caricaLista");
                    lswFct.listaSpesa.remove({ id: lista.IdListaSpesa }).$promise
                    .then(function (success) {
                        UIkit.notify('Lista [' + lista.Nome + '] rimossa con successo: ', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });                        
                    });
                });
            }

            this.saveList = function () {
                if (_this.editMode) {
                    _this.updateListComponent();
                }
                else {
                    _this.addList();
                }
            }

            this.addList = function () {
                var queryParameters = {                
                    Nome: _this.lista.Nome,
                    UtentiListaSpesa: [],
                    VociListaSpesa: []
                };

                lswFct.listaSpesa.add(queryParameters).$promise
                .then(function (success) {
                    _this.lista = success;
                    _this.addListComponent(_this.lista);
                    _this.modalInvertStatus("salvaLista");
                });
            };

            this.addListComponent = function (listaSpesa) {
                var idUtentiListaSpesa = 0, idVoceListaSpesa = 0;
                var listaSpesa = { IdListaSpesa: listaSpesa.IdListaSpesa };

                _this.utentiAdded.forEach(function (u) {
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
                        Comprata: v.Comprata ? true : false,
                        Voce: { IdVoce: v.IdVoce },
                        ListaSpesa: listaSpesa
                    }
                    lswFct.voceListaSpesa.add(VoceListaSpesaParameter).$promise
                   .then(function (success) {
                   });
                });
            }

            this.updateList = function () {
                _this.lista.UtentiListaSpesa = [];
                _this.lista.VociListaSpesa = [];
                
                var idSpesa = _this.lista.IdListaSpesa;
                var nomeLista = _this.lista.Nome;

                lswFct.listaSpesa.update({ id: _this.lista.IdListaSpesa }, _this.lista,
                function (success) {
                    lswFct.listaSpesa.clear({ id: idSpesa }).$promise
                    .then(function (esito) {
                        if(esito.Response)
                            UIkit.notify('Lista ' + nomeLista + ' è stata rimossa poichè completa', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                        else
                            UIkit.notify('Lista ' + nomeLista + ' aggiornata', { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                    });                    
                    _this.pulisciLista(false, true);
                });
                _this.modalInvertStatus("salvaLista");
            }

            this.updateListComponent = function () {
                // UTENTI
                _this.utentiAdded.forEach(function (u) {
                    var trovato = false;
                    _this.lista.UtentiListaSpesa.forEach(function (uls) {
                        // aggiunta
                        if (u.IdUtentiListaSpesa === undefined && !trovato) {
                            trovato = true;
                            var UtentiListaSpesaParameter = {
                                IdUtentiListaSpesa: 0,
                                Utente: u,
                                ListaSpesa: _this.lista
                            };

                            lswFct.utenteListaSpesa.add(UtentiListaSpesaParameter).$promise
                            .then(function (success) {
                            });
                        }                        
                    });
                });
                
                _this.lista.UtentiListaSpesa.forEach(function (uls) {
                    var trovato = false;
                    _this.utentiAdded.forEach(function (u) {
                        if (uls.IdUtentiListaSpesa === u.IdUtentiListaSpesa || uls.IdUtentiListaSpesa === undefined) {
                            trovato = true;
                        }
                    });
                    if (!trovato) {
                        // rimozione
                        lswFct.utenteListaSpesa.remove({ id: uls.IdUtentiListaSpesa });
                    }
                });

                // VOCI
                _this.vociAdded.forEach(function (v) {
                    var trovato = false;
                    _this.lista.VociListaSpesa.forEach(function (uls) {
                        // aggiunta
                        if (v.IdVoceListaSpesa === undefined && !trovato) {
                            trovato = true;
                            var VoceListaSpesaParameter = {
                                IdVoceListaSpesa: 0,
                                Comprata: v.Comprata ? true : false,
                                Voce: v,
                                ListaSpesa: _this.lista
                            };

                            lswFct.voceListaSpesa.add(VoceListaSpesaParameter).$promise
                            .then(function (success) {
                            });
                        }
                        //Update
                        if (v.IdVoceListaSpesa === uls.IdVoceListaSpesa && uls.Comprata != v.Comprata && !trovato) {
                            trovato = true;
                            var VoceListaSpesaParameter = {
                                IdVoceListaSpesa: v.IdVoceListaSpesa,
                                Comprata: v.Comprata ? true : false,
                                Voce: v,
                                ListaSpesa: _this.lista
                            };
                            lswFct.voceListaSpesa.update({ id: v.IdVoceListaSpesa }, VoceListaSpesaParameter, function (success) {});
                        }
                    });

                });

                _this.lista.VociListaSpesa.forEach(function (uls) {
                    var trovato = false;
                    _this.vociAdded.forEach(function (v) {
                        if (uls.IdVoceListaSpesa === v.IdVoceListaSpesa || uls.IdVoceListaSpesa === undefined) {
                            trovato = true;
                        }
                    });
                    if (!trovato) {
                        // rimozione
                        lswFct.voceListaSpesa.remove({ id: uls.IdVoceListaSpesa });
                    }
                });

                // LISTA
                _this.updateList();
            }

            this.addVoce = function (voce) {
                newVoce = jQuery.extend(true, {}, voce);
                newVoce.incremental = _this.duplicateVoceIncremental();
                _this.vociAdded.push(newVoce);
                UIkit.notify('Aggiunta voce: ' + newVoce.Name, { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
            }
            this.removeVoce = function (voce) {                
                _this.vociAdded.pop(voce);                
            }
            this.addUtente = function (utente) {
                var findInArray = false;
                _this.utentiAdded.forEach(function (element) {
                    if (element.IdUtente == utente.IdUtente)
                        findInArray = true;
                });

                if (!findInArray) {
                    _this.utentiAdded.push(utente);
                    UIkit.notify('Aggiunta utente: ' + utente.Username, { status: 'success', timeout: _this.timeout, pos: 'bottom-center' });
                }   
                else
                    UIkit.notify('Utente già presente', { status: 'danger', timeout: _this.timeout, pos:'bottom-center' });
            }
            this.removeUtente = function (utente) {
                for (i = 0; i < _this.utentiAdded.length; i++) {
                    if (_this.utentiAdded[i].IdUtente == utente.IdUtente) {
                        _this.utentiAdded.splice(i, 1);
                    }
                };
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
                    UIkit.notify('Nessun utente presente', { status: 'warning', timeout: _this.timeout, pos: 'bottom-center' });
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

            this.modalSetStatus = function (modalId, active) {
                var modal = UIkit.modal("#" + modalId);
                if (modal.isActive() && !active) {
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