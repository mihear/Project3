
var app = angular.module('myApp', [])
app.service('StudentDetailsService', StudentDetailsService);
function StudentDetailsService($http) {
    this.getStudentDetails = function getStudentDetails(filter) {
        
        return $http.post('/api/DimDrivers/GetDrivers',filter);
    };
    this.getTotal = function getTotal() {
        return $http.get('/api/DimDrivers/getTotalDrivers')
    };
    this.getAvailable = function getAvailable() {
        return $http.get('/api/DimDrivers/getAvailable')
    };
    this.getActive = function getActive() {
        return $http.get('/api/DimDrivers/getActive')
    };
    this.getDriverProv = function getDriverProv() {
        return $http.get('/api/DimDrivers/getTotalProviderDrivers')
    };
    this.getDriverType = function getDriverType(filter) {
        return $http.post('/api/DimDrivers/DriverType', filter)
    };
    this.getdriverOrder = function getdriverOrder(orderFilter) {
          
        return $http.post('/api/DimDrivers/driverOrder', orderFilter)
    };
    this.getAllDrivers = function getAllDrivers() {
        return $http.get('/api/DimDrivers/getAllDrivers')
    };
    this.SuccefulOrderDriver = function SuccefulOrderDriver(filter) {
        return $http.post('/api/DimDrivers/SuccefulOrderDriver', filter)
    };
    this.AcceptReject = function AcceptReject(filter) {
        return $http.post('/api/DimDrivers/AcceptReject', filter)
    };
}
app.controller('StudentController', StudentController);

function StudentController(StudentDetailsService) {

    var vm = this;
    vm.init = init;
   

    vm.sloading = true;
    vm.loading = true;
    vm.loading1 = true;
    vm.loading2 = true;
    vm.loading4 = true;


    vm.filter = {}
    vm.filter1 = {}
    vm.filter3 = {}
    vm.filter4 = {}

    function init() {

        var today = new Date();
        var dd = today.getDate();
        var ddd = today.getDate() + 1;
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        var todayn = yyyy + '/' + mm + '/' + dd
        var todayt = yyyy + '/' + mm + '/' + ddd


        vm.filter['id'] = 0;
        vm.filter['from'] = new Date(todayn);
        vm.filter['to'] = new Date(todayt);

        vm.filter1['id'] = 0;
        vm.filter1['from'] = new Date(todayn);
        vm.filter1['to'] = new Date(todayt);

        vm.filter3['id'] = 1;
        vm.filter3['to'] = new Date(todayn);
        vm.filter3['from'] = new Date(todayn);
    //vm.filter['from'] = "2017/12/7";
        //vm.filter['to'] = "2017/12/8";
        vm.filter4['from'] = new Date(todayn);



        var today123 = new Date();
        var dd123 = today.getDate();
        var ddd123 = today.getDate() + 1;
        var mm123 = today.getMonth() + 1; //January is 0!

        var yyyy123 = today123.getFullYear();
        if (dd123 < 10) {
            dd123 = '0' + dd123;
        }
        if (mm123 < 10) {
            mm123 = '0' + mm123;
        }
        var todayn123 = yyyy123 + '/' + mm123 + '/' + dd123
        var todayt123 = yyyy123 + '/' + mm123 + '/' + ddd123

        vm.orderFilter['from'] = new Date(todayn123);
        vm.orderFilter['to'] = new Date(todayt123);


    }
    
    vm.dTimeline = dTimeline;
    vm.dOrder = dOrder;
    vm.sOrder = sOrder

    vm.AcceptReject = AcceptReject

    sOrder()
    function sOrder() {
        
        $('#sOrder').css('display', 'none');
        $('#sResult').css('display', 'none');
        vm.sloading = true


        StudentDetailsService.SuccefulOrderDriver(vm.filter3).then(function (response) {
            
            vm.listSucOrder = response.data;
            console.log(vm.listSucOrder)
            vm.listSucOrder.WorkHours = vm.listSucOrder.WorkHours/60
            vm.sloading = false
            if (vm.listSucOrder.AV != 0) {
                $('#sOrder').css('display', 'block');
            } else {
                $('#sResult').css('display', 'block');

            }
        })
    }

    AcceptReject()
    function AcceptReject() {
        vm.loading4 = true;
        $('#arNoResult').css('display', 'none');
        StudentDetailsService.AcceptReject(vm.filter4).then(function (response) {
            vm.arOrderList = response.data;
            vm.loading4 = false;
            debugger
            if (vm.arOrderList.length > 0) {
                vm.arOrderL = {}
                for (var i = 0; i < 5; i++) {
                    vm.arOrderL[i] = vm.arOrderList[i]
                }
                $('#arTable').css('display', 'block');
            } else {
                $('#arNoResult').css('display', 'block');
            }


        })
    }


    dTimeline();

    function dTimeline() {
        
        StudentDetailsService.getStudentDetails(vm.filter).then(function (response) {
           // console.log(response.data)
            vm.drivers = {}
           // 
            $("#visualization").empty();
            

            vm.drivers = response.data;
            if (vm.drivers.length > 0) {
                
              //  console.log("data")
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
                        updateTime: true,  // drag items horizontally
                        updateGroup: true, // drag items from one group to another
                        remove: true,       // delete an item by tapping the delete button top right
                        overrideItems: false
                    }

                };

                var timeline = new vis.Timeline(container);
                timeline.setOptions(options);
                timeline.setGroups(groups);
                timeline.setItems(items);
                vm.loading2 = false;

                $('#noResult').css('display', 'none');
                $("#visualization").css('display', 'block');
                
            }
            else {
                vm.loading2 = false;

              //  console.log("no date")
                $('#noResult').css('display', 'block');
            }
        });
    }

    StudentDetailsService.getTotal().then(function (response) {
        vm.Total = response.data;
    })
    StudentDetailsService.getAvailable().then(function (response) {
        
        vm.Available = response.data;
    })
    StudentDetailsService.getActive().then(function (response) {
        
        vm.Active = response.data;
    })
    StudentDetailsService.getDriverProv().then(function (response) {

        vm.DriverProv = response.data;
        vm.DriverProv = vm.DriverProv - 1;
    })

    vm.driversType = driversType
    driversType()
    function driversType() {
        $('#pieResultOrder').css('display', 'none');
        $('#pieCha').css('display', 'none');

        vm.loading = true;

        StudentDetailsService.getDriverType(vm.filter1).then(function (response) {


            vm.DriverType = response.data;
            console.log("DriverType", vm.DriverType)
            if (vm.DriverType.length > 0) {
                vm.names = [];
                vm.y = [];
                for (var i = 0; i < vm.DriverType.length; i++) {
                    vm.names[i] = vm.DriverType[i].Type;
                    vm.y[i] = vm.DriverType[i].Size
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

                $('#pieResultOrder').css('display', 'none');
                vm.loading = false;
                $("#pieCha").css('display', 'block');
            } else {
                vm.loading = false;
                $('#pieResultOrder').css('display', 'block');
            }


        })
    }

    vm.orderFilter = {}

    dOrder();
    function dOrder() {
        StudentDetailsService.getdriverOrder(vm.orderFilter).then(function (response) {
            $("#barChart").empty();

            vm.driverOrder = response.data;
            vm.labels = [];
            vm.data1 = [];
            vm.data2 = [];
            if (vm.driverOrder.length > 0) {
                
               
               // $('#barCha').css('display', 'block');

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

                vm.loading1 = false;
                $('#noResultOrder').css('display', 'none');
                $("#barCha").css('display', 'block');



            }
            else {
                vm.loading1 = false;
                $('#noResultOrder').css('display', 'block');
               // $('#barCha').css('display', 'none');
            }



        })
    }

    StudentDetailsService.getAllDrivers().then(function (response) {

        vm.allDrivers = response.data;
        console.log("All Driver", vm.allDrivers)
    })


    function getDate() {
        let today1 = new Date().toISOString().substr(0, 10);
        document.querySelector("#from").value = today1;
        document.querySelector("#to").value = today1;
    }
    getDate();

    console.log("controller")
}


app.controller('AcReController', AcReController);

function AcReController(StudentDetailsService) {
    debugger
    var vm = this;
    vm.init = init;
    vm.loading5 = true;
    vm.filter5 = {}
    function init() {

        var today = new Date();
        var dd = today.getDate();
        var ddd = today.getDate() + 1;
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        var todayn = yyyy + '/' + mm + '/' + dd
        var todayt = yyyy + '/' + mm + '/' + ddd


        vm.filter5['from'] = new Date(todayn);

    }
    vm.fAcceptReject = fAcceptReject
    fAcceptReject()
    function fAcceptReject() {
        vm.loading5 = true;
        $('#farNoResult').css('display', 'none');
        StudentDetailsService.AcceptReject(vm.filter5).then(function (response) {
            vm.farOrderList = response.data;
            vm.loading5 = false;
            debugger
            if (vm.farOrderList.length > 0) {
                $('#farTable').css('display', 'block');
            } else {
                $('#farNoResult').css('display', 'block');
            }


        })
    }
}

