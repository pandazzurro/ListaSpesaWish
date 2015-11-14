"use strict";
// calcolo della route di base in caso di siti secondari
app.webapi = window.location.origin + window.location.pathname;
app.factory('factories', ['$resource', function ($resource) {
    return {        
        user: $resource(app.webapi, {}, {
            login: { url: app.webapi + 'api/account/login', method: 'POST', isArray: false },
            logout: { url: app.webapi + 'api/account/logout', method: 'POST', isArray: false },            
        }),
        listaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + 'api/ListaSpesa/:id', method: 'GET', isArray: false },
            getAll: { url: app.webapi + 'api/ListaSpesa/', method: 'GET', isArray: true },
            add: { url: app.webapi + 'api/ListaSpesa', method: 'POST', isArray: false },
            mail: { url: app.webapi + 'api/ListaSpesa/Mail', method: 'POST', isArray: false },
            update: { url: app.webapi + 'api/ListaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + 'api/ListaSpesa/:id', method: 'DELETE', isArray: false },
            clear: { url: app.webapi + 'api/ListaSpesa/Clear/:id', method: 'DELETE', isArray: false }
        }),
        utente: $resource(app.webapi, {}, {
            get: { url: app.webapi + 'api/utente/:id', method: 'GET', isArray: false },
            getAll: { url: app.webapi + 'api/utente/', method: 'GET', isArray: true },
            add: { url: app.webapi + 'api/utente', method: 'POST', isArray: false },
            update: { url: app.webapi + 'api/utente/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + 'api/utente/:id', method: 'DELETE', isArray: false }
        }),
        voce: $resource(app.webapi, {}, {
            get: { url: app.webapi + 'api/Voce/:id', method: 'GET', isArray: false },
            getAll: { url: app.webapi + 'api/Voce/', method: 'GET', isArray: true },
        }),
        utenteListaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + 'api/UtentiListaSpesa/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + 'api/UtentiListaSpesa', method: 'POST', isArray: false },
            update: { url: app.webapi + 'api/UtentiListaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + 'api/UtentiListaSpesa/:id', method: 'DELETE', isArray: false }
        }),
        voceListaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + 'api/VoceListaSpesa/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + 'api/VoceListaSpesa', method: 'POST', isArray: false },
            update: { url: app.webapi + 'api/VoceListaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + 'api/VoceListaSpesa/:id', method: 'DELETE', isArray: false }
        }),
    }
}]);