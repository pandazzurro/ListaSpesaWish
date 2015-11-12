"use strict";
app.directive('navbarTop', function () {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/navbar-top.html'
    };
});

app.directive('navbarFooter', function () {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/navbar-footer.html'
    };
});

app.directive('formLogin', function () {
    return {
        restrict: 'E',
        templateUrl: 'Site/templates/form-login.html'
    };
});
