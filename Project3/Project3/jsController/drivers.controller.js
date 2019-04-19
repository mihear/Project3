
var app = angular.module('myApp', [])
app.service('StudentDetailsService', StudentDetailsService);
function StudentDetailsService($http) {
    this.getStudentDetails = function getStudentDetails() {
        debugger
        return $http.get('/api/DimDrivers/GetDriver');
    };
}
app.controller('StudentController', StudentController);

function StudentController(StudentDetailsService) {

    var vm = this;

    StudentDetailsService.getStudentDetails().then(function (response) {
        debugger
        vm.drivers = response.data;
        vm.driverName = [];
        console.log(vm.drivers);
        for (var i = 0; i < vm.drivers.length; i++) {
            var test = {};
            test['id'] = vm.drivers[i].id;
            test['content'] = vm.drivers[i].Name;
            vm.driverName.push(test);
        }
        console.log(vm.driverName)

        this.test = "yesssssss"


        var groups = new vis.DataSet(vm.driverName);

        // create a dataset with items
        // note that months are zero-based in the JavaScript Date object, so month 3 is April
        var items = new vis.DataSet([
            { id: 0, group: 4, content: 'item 0', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T16:02:45") },
            { id: 1, group: 4, content: 'item 1', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T17:02:45") },
            { id: 2, group: 10, content: 'item 2', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
            { id: 3, group: 10, content: 'item 3', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
            { id: 4, group: 11, content: 'item 4', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
            { id: 5, group: 12, content: 'item 5', start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") }
        ]);

        // create visualization
        var container = document.getElementById('visualization');
        var options = {
            // option groupOrder can be a property name or a sort function
            // the sort function must compare two groups and return a value
            //     > 0 when a > b
            //     < 0 when a < b
            //       0 when a == b
            groupOrder: function (a, b) {
                return a.value - b.value;
            },
            editable: true
        };

        var timeline = new vis.Timeline(container);
        timeline.setOptions(options);
        timeline.setGroups(groups);
        timeline.setItems(items);









        // handle response  
    });


    console.log("controller")
}




    //.controller("driverCtrl", function ($scope) {
    //    var vm = $scope;

    //    vm.test = "yessssssssss";
    //});

