app.controller("UserController", function UserController($scope, $http) {

    $scope.userList = null;
    $scope.message = '';
    $scope.result = "color-default";
    $scope.isViewLoading = false;

    $http.get("/Account/GetAllUsers").then(function (data) {
        $scope.userList = data;
    }, function (data) { });


    $scope.DeteleTable = function (userModel) {
        if (confirm("Are you sure you want to detete this user?")) {
            $http.delete("/Account/Delete/" + userModel.Id).then(function SuccessDeleteUser() {
                var index = $scope.userList.data.indexOf(userModel);
                $scope.userList.data.splice(index, 1);
            });
        }
    };


});