app.controller('UserController', ['$scope', '$cookies', 'user-service', function ($scope, $cookies, userSrv) {
    $scope.username = undefined;
    $scope.password = undefined;

    var _this = this;

    this.login = function () {
        userSrv.login(username, password);
    };

    this.logout = function () {
        userSrv.logout($scope.username, $scope.password);

        $scope.username = '';
        $scope.password = '';
    };

    this.isLoggedIn = function () {
        return userSrv.isLoggedIn();
    };

    this.getCurrentUser = function () {
        return userSrv.getCurrentUser();
    };

    this.register = function () {
        window.location = '#/register';
    };
}]);