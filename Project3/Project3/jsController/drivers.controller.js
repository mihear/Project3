
var app = angular.module('myApp', [])
app.service('StudentDetailsService', StudentDetailsService);
function StudentDetailsService($http) {
    this.getStudentDetails = function getStudentDetails() {
        return $http.get('/api/DimDrivers/GetDriver');
    };
    this.getTotal = function getTotal() {
        return $http.get('/api/DimDrivers/getTotalDrivers')
    };
    this.getDriverProv = function getDriverProv() {
        return $http.get('/api/DimDrivers/getTotalProviderDrivers')
    };
    this.getDriverType = function getDriverType() {
        return $http.get('/api/DimDrivers/DriverType')
    };
}
app.controller('StudentController', StudentController);

function StudentController(StudentDetailsService) {

    var vm = this;

    StudentDetailsService.getStudentDetails().then(function (response) {
        vm.drivers = response.data;
        vm.driverName = [];
        vm.data = []
        console.log(vm.drivers);
        var k = 0;
        for (var i = 0; i < vm.drivers.length; i++) {
            var test = {};
            test['id'] = vm.drivers[i].id;
            test['content'] = vm.drivers[i].Name;
            vm.driverName.push(test);
            for (var j = 0; j < vm.drivers[i].Times.length; j++) {
                var object = {};
                object['id'] = k;
                object['group'] = vm.drivers[i].id;
                object['start'] = vm.drivers[i].Times[j].PickedupTime;
                object['end'] = vm.drivers[i].Times[j].DeliveredTime;
                vm.data.push(object)
                k++;
            }   
        }
        
        console.log(vm.data)

        this.test = "yesssssss"


        var groups = new vis.DataSet(vm.driverName);

        // create a dataset with items
        // note that months are zero-based in the JavaScript Date object, so month 3 is April
        //var items = new vis.DataSet([
        //    { id: 0, group: 4, start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T16:02:45") },
        //    { id: 1, group: 4,  start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T17:02:45") },
        //    { id: 2, group: 10,  start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
        //    { id: 3, group: 10, start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
        //    { id: 4, group: 11, start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") },
        //    { id: 5, group: 12, start: new Date("2017-12-02T15:02:45"), end: new Date("2017-12-02T15:02:45") }
        //]);






        var items = new vis.DataSet(vm.data);

        // create visualization
        var container = document.getElementById('visualization');
        var options = {
            orientation: 'both',
            // option groupOrder can be a property name or a sort function
            // the sort function must compare two groups and return a value
            //     > 0 when a > b
            //     < 0 when a < b
            //       0 when a == b
            groupOrder: function (a, b) {
                return a.value - b.value;
            },
            editable: {
                add: false,         // add new items by double tapping
                updateTime: false,  // drag items horizontally
                updateGroup: false, // drag items from one group to another
                remove: false,       // delete an item by tapping the delete button top right
                overrideItems: false }
            
        };

        var timeline = new vis.Timeline(container);
        timeline.setOptions(options);
        timeline.setGroups(groups);
        timeline.setItems(items);









        // handle response  
    });
    StudentDetailsService.getTotal().then(function (response) {

        vm.Total = response.data;
        console.log(vm.Total)
    })
    StudentDetailsService.getDriverProv().then(function (response) {

        vm.DriverProv = response.data;
        vm.DriverProv = vm.DriverProv - 1;
        console.log(vm.Total)
    })
    StudentDetailsService.getDriverType().then(function (response) {

        vm.DriverType = response.data;
        vm.names = [];
        vm.y = [];
        console.log(vm.DriverType)
        for (var i = 0; i < vm.DriverType.length; i++) {
            vm.names[i] = vm.DriverType[i].name;
            vm.y[i] = vm.DriverType[i].y
        }
        console.log(vm.names)
        console.log(vm.y)
        var doughnutPieData = {
            datasets: [{
                data: vm.y,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.5)',
                    'rgba(54, 162, 235, 0.5)',
                    'rgba(255, 206, 86, 0.5)',
                    'rgba(153, 102, 255, 0.5)',
                    'rgba(255, 159, 64, 0.5)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
            }],

            // These labels appear in the legend and in the tooltips when hovering different arcs
            labels: vm.names
        };
        var doughnutPieOptions = {
            responsive: true,
            animation: {
                animateScale: true,
                animateRotate: true
            }
        };


        if ($("#pieChart").length) {
            var pieChartCanvas = $("#pieChart").get(0).getContext("2d");
            var pieChart = new Chart(pieChartCanvas, {
                type: 'pie',
                data: doughnutPieData,
                options: doughnutPieOptions
            });
        }


    })
   





    var options = {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        legend: {
            display: false
        },
        elements: {
            point: {
                radius: 0
            }
        }

    };


    var data = {
        labels: ["2013", "2014", "2014", "2015", "2016", "2017"],
        datasets: [{
            label: '# of Votes',
            data: [10, 19, 3, 5, 2, 3],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255,99,132,1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
        }]
    };





    if ($("#barChart").length) {
        var barChartCanvas = $("#barChart").get(0).getContext("2d");
        // This will get the first returned node in the jQuery collection.
        var barChart = new Chart(barChartCanvas, {
            type: 'bar',
            data: data,
            options: options
        });
    }
    console.log("controller")
}



