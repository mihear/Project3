
var app = angular.module('myApp', [])
app.service('StudentDetailsService', StudentDetailsService);
function StudentDetailsService($http) {
    this.getStudentDetails = function getStudentDetails(filter) {
        
        return $http.post('/api/DimDrivers/GetDrivers',filter);
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
    this.getdriverOrder = function getdriverOrder() {
        return $http.get('/api/DimDrivers/driverOrde')
    };
    this.getAllDrivers = function getAllDrivers() {
        return $http.get('/api/DimDrivers/getAllDrivers')
    };
}
app.controller('StudentController', StudentController);

function StudentController(StudentDetailsService) {

    var vm = this;
    vm.filter = {}
    //= {
    //    id = 0 ,
    //    from: new Date().toISOString().substr(0, 10),
    //    to: new Date().toISOString().substr(0, 10)
    //};
    var today = new Date();
    var dd = today.getDate();
    var ddd = today.getDate()+1;
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var todayn =  yyyy + '/' + mm + '/' +dd
    var todayt = yyyy + '/' + mm + '/' + ddd
    

    vm.filter['id'] = 0;
    vm.filter['from'] = new Date(todayn);
    vm.filter['to'] = new Date(todayt);
    //vm.filter['from'] = "2017/12/7";
    //vm.filter['to'] = "2017/12/8";
    
    vm.dTimeline = dTimeline;
    dTimeline();

    function dTimeline() {
        
        StudentDetailsService.getStudentDetails(vm.filter).then(function (response) {
            
            vm.drivers = response.data;
            if (vm.drivers.length > 0) {
                $('#noResult').css('display', 'none');
                console.log("data")
                vm.driverName = [];
                vm.data = []
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


                var groups = new vis.DataSet(vm.driverName);

                var items = new vis.DataSet(vm.data);

                // create visualization
                var container = document.getElementById('visualization');
                var options = {
                    orientation: 'both',
                    groupOrder: function (a, b) {
                        return a.value - b.value;
                    },
                    editable: {
                        add: false,         // add new items by double tapping
                        updateTime: false,  // drag items horizontally
                        updateGroup: false, // drag items from one group to another
                        remove: false,       // delete an item by tapping the delete button top right
                        overrideItems: false
                    }

                };

                var timeline = new vis.Timeline(container);
                timeline.setOptions(options);
                timeline.setGroups(groups);
                timeline.setItems(items);
            }
            else {
                console.log("no date")
                $('#noResult').css('display', 'block');
            }
        });
    }

   




    StudentDetailsService.getTotal().then(function (response) {
        vm.Total = response.data;
    })
    StudentDetailsService.getDriverProv().then(function (response) {

        vm.DriverProv = response.data;
        vm.DriverProv = vm.DriverProv - 1;
    })
    StudentDetailsService.getDriverType().then(function (response) {

        vm.DriverType = response.data;
            vm.names = [];
            vm.y = [];
            for (var i = 0; i < vm.DriverType.length; i++) {
                vm.names[i] = vm.DriverType[i].name;
                vm.y[i] = vm.DriverType[i].y
            }
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
    StudentDetailsService.getdriverOrder().then(function (response) {

            vm.driverOrder = response.data;
            vm.labels = [];
            vm.data1 = [];
            vm.data2 = [];

            for (var i = 0; i < vm.driverOrder.length; i++) {
                vm.labels[i] = vm.driverOrder[i].Name;
                vm.data1[i] = vm.driverOrder[i].allOrder;
                vm.data2[i] = vm.driverOrder[i].cancelOrder;

            }


            var ctx = document.getElementById("barChart");
            if (ctx) {
                ctx.height = 200;
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    defaultFontFamily: 'Poppins',
                    data: {
                        labels: vm.labels,
                        datasets: [
                            {
                                label: "All Order",
                                data: vm.data1,
                                borderColor: "rgba(0, 123, 255, 0.9)",
                                borderWidth: "0",
                                backgroundColor: "rgba(0, 123, 255, 0.5)",
                                fontFamily: "Poppins"
                            },
                            {
                                label: "Cancel Order",
                                data: vm.data2,
                                borderColor: "rgba(0,0,0,0.09)",
                                borderWidth: "0",
                                backgroundColor: "rgba(0,0,0,0.07)",
                                fontFamily: "Poppins"
                            }
                        ]
                    },
                    options: {
                        legend: {
                            position: 'top',
                            labels: {
                                fontFamily: 'Poppins'
                            }

                        },
                        scales: {
                            xAxes: [{
                                ticks: {
                                    fontFamily: "Poppins"

                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true,
                                    fontFamily: "Poppins"
                                }
                            }]
                        }
                    }
                });
            }
       


    




        })
    StudentDetailsService.getAllDrivers().then(function (response) {

        vm.allDrivers = response.data;
    })

    function getDate() {
        let today1 = new Date().toISOString().substr(0, 10);
        document.querySelector("#from").value = today1;
        document.querySelector("#to").value = today1;
    }
    getDate();



    

    
    console.log("controller")
}



