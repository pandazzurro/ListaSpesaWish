"use strict";
app.service('route-service', ['$routeProvider', function ($route) {
    this.route = $route;
}]);

app.service('user-service', ['factories', function (lswFct) {
    var _this = this;
    var user = {};

    this.login = function (username, password) {
        var queryParameters = {
            Username: username,
            Password: password
        };

        lswFct.user.login(queryParameters).$promise
          .then(function (userSuccess) {
              user = userSuccess;
          },
          function (error) {
              UIkit.notify('Username o password non validi', { status: 'warning', timeout: 5000 });
              user = {};
          });
    };

    this.logout = function (username, password) {
        var queryParameters = {
            Username: username,
            Password: password
        };

        lswFct.user.logout(queryParameters).$promise
          .then(function (userSuccess) {
              user = userSuccess;
              $location.path('/');
          },
          function (error) {
              UIkit.notify('Errore di logout', { status: 'warning', timeout: 5000 });
              user = {};
          });
    };

    this.isLoggedIn = function () {
        return user.userId !== undefined;
    }

    this.getCurrentUser = function () {
        return user;
    }
}]);

