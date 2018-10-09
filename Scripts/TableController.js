
var app = angular.module("app", []);

app.controller("TableListController", function TableListController($scope, $http) {

    $scope.tableList = null;
    $scope.message = '';
    $scope.result = "color-default";
    $scope.isViewLoading = false;

    $http.get("/Table/TableList").then(function (data) {
        $scope.tableList = data;
    }, function (data) { });
       

    $scope.DeteleTable = function (tableModel) {
        $http.delete("/Table/Delete/" + tableModel.Id).then(function SuccessDeleteTable(data) {
            var index = $scope.tableList.data.indexOf(tableModel);
            $scope.tableList.data.splice(index, 1);
        }, function FailedDeteTetable(data) { });      
    };
    
    
});