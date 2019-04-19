
var app = angular.module('myApp' , [])
app.service('StudentDetailsService', StudentDetailsService);
function StudentDetailsService($http) {
    this.getStudentDetails = function getStudentDetails()
    {
        debugger
        return $http.get('/api/DimDrivers/GetDriver');
    };
}
app.controller('StudentController', StudentController);

function StudentController(StudentDetailsService) {

    StudentDetailsService.getStudentDetails().then(function (response) {
        debugger
        console.log(response.data);
        this.test = "yesssssss"
    // handle response  
    });


    console.log("controller")
}




    //.controller("driverCtrl", function ($scope) {
    //    var vm = $scope;

    //    vm.test = "yessssssssss";
    //});

