﻿app.controller("GameListController", function GameListController($scope, $http) {
    
    $scope.gameList = null;
    $scope.opponent_1 = null;
    $scope.opponent_2 = null;
    $scope.message = '';
    $scope.result = "color-default";
    $scope.isViewLoading = false;

    $http.get(actionAddress).then(function (data) {
        $scope.gameList = data;
        $scope.opponent_1 = data.data[0].Opponents;
        $scope.opponent_2 = data.data[0].Opponents;
    }, function () {
            alert("Unexpected error occurred.");
        });
       

    $scope.DeteleTable = function (gameModel) {
        if (confirm("Are you sure you want to detete this table?"))
        {
            $http.delete("/Game/Delete/" + gameModel.Id).then(function SuccessDeleteGame() {
                var index = $scope.gameList.data.indexOf(gameModel);
                $scope.gameList.data.splice(index, 1);
            }, function FailedDeleteGame() {
                alert("Unexpected error occurred.");
            });
        }
    };
    
    
});