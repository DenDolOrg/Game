app.controller("TableListController", function TableListController($scope, $http) {
    
    $scope.tableList = null;
    $scope.message = '';
    $scope.result = "color-default";
    $scope.isViewLoading = false;

    $http.get(actionAddress).then(function (data) {
        $scope.tableList = data;
    }, function (data) { });
       

    $scope.DeteleTable = function (tableModel) {
        if (confirm("Are you sure you want to detete this table?"))
        {
            $http.delete("/Table/Delete/" + tableModel.Id).then(function SuccessDeleteTable() {
                var index = $scope.tableList.data.indexOf(tableModel);
                $scope.tableList.data.splice(index, 1);
            }, function FailedDeleteTetable() {
                alert("Unexpected error occurred.");
            });
        }
    };
    
    
});