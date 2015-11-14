"use strict";
var app = angular.module('lsw', ['ngRoute', 'ngResource', 'ngCookies', 'angular-loading-bar', 'ngImgCrop']);

    // configure loading bar
    app.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = true;
        cfpLoadingBarProvider.includeBar = true;
    }]);

    app.config(['$httpProvider', function ($httpProvider) {
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }

        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get.Pragma = 'no-cache';
    }]);

    app.config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider
            .when('/voci', {
                template: '<voci></voci>'
            })
            .when('/listaSpesa', {
                template: '<lista-spesa></lista-spesa>'
            })
            .when('/', {
                templateUrl: 'Site/templates/home.html'
            })
            .otherwise({
                redirectTo: '/'
            });
        }]);

   
    app.run(['$rootScope', '$location', 'user-service', function ($rootScope, $location, userSrv) {
        $rootScope.$on("$routeChangeStart", function (event, next, current) {
            if (next.$$route.originalPath != "/register" &&
                !userSrv.isLoggedIn()) {
                $location.path("/");
            }

            $('html, body').animate({ scrollTop: 0 }, 'fast');
        });
    }]);    