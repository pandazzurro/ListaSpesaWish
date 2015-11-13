"use strict";
app.webapi = "http://localhost/ListaSpesaWish";
app.factory('factories', ['$resource', function ($resource) {
    return {        
        user: $resource(app.webapi, {}, {
            login: { url: app.webapi + '/api/account/login', method: 'POST', isArray: false },
            logout: { url: app.webapi + '/api/account/logout', method: 'POST', isArray: false },            
        }),
        listaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + '/api/listaSpesa/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + '/api/listaSpesa', method: 'POST', isArray: false },
            update: { url: app.webapi + '/api/listaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + '/api/listaSpesa/:id', method: 'DELETE', isArray: false }
        }),
        utente: $resource(app.webapi, {}, {
            get: { url: app.webapi + '/api/utente/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + '/api/utente', method: 'POST', isArray: false },
            update: { url: app.webapi + '/api/utente/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + '/api/utente/:id', method: 'DELETE', isArray: false }
        }),
        voce: $resource(app.webapi, {}, {
            get: { url: app.webapi + '/api/voce/:id', method: 'GET', isArray: false },
            getAll: { url: app.webapi + '/api/voce/', method: 'GET', isArray: true },
            //add: { url: app.webapi + '/api/voce', method: 'POST', isArray: false },
            //update: { url: app.webapi + '/api/voce/:id', method: 'PUT', isArray: false },
            //remove: { url: app.webapi + '/api/voce/:id', method: 'DELETE', isArray: false }
        }),
        utenteListaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + '/api/utenteListaSpesa/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + '/api/utenteListaSpesa', method: 'POST', isArray: false },
            update: { url: app.webapi + '/api/utenteListaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + '/api/utenteListaSpesa/:id', method: 'DELETE', isArray: false }
        }),
        voceListaSpesa: $resource(app.webapi, {}, {
            get: { url: app.webapi + '/api/voceListaSpesa/:id', method: 'GET', isArray: false },
            add: { url: app.webapi + '/api/voceListaSpesa', method: 'POST', isArray: false },
            update: { url: app.webapi + '/api/voceListaSpesa/:id', method: 'PUT', isArray: false },
            remove: { url: app.webapi + '/api/voceListaSpesa/:id', method: 'DELETE', isArray: false }
        }),
    }
}]);