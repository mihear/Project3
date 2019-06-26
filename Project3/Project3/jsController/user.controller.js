app.service('userService', userService);
function userService($http) {
    this.getOrderRateL = function getOrderRateL(filter) {

        return $http.post('/api/DimUsers/UserRateL', filter);
    };
    this.getOrderRateH = function getOrderRateH(filter) {

        return $http.post('/api/DimUsers/UserRateH', filter);
    };
    this.getDishL = function getDishL(filter) {

        return $http.post('/api/DimUsers/AreaOrderL', filter);
    };
    this.getDishH = function getDishH(filter) {

        return $http.post('/api/DimUsers/AreaOrderH', filter);
    };

    this.getAreaL = function getAreaL(filter) {

        return $http.post('/api/DimUsers/AreaOrderL', filter);
    };
    this.getAreaH = function getAreaH(filter) {

        return $http.post('/api/DimUsers/AreaOrderH', filter);
    };

    this.DishForMining = function DishForMining() {
        return $http.get('/api/DimUsers/UserForMining');
    }
    this.DishDataMining = function DishDataMining(filter) {

        return $http.post('/api/DimUsers/UserDataMining?id=' + filter);
    };


}

app.controller('userController', userController);
function userController(userService) {

    vm = this;
    vm.loading = true;

    vm.init = init
    vm.oRL = true
    vm.DishRL = true
    vm.AreaRL = true
    vm.filter = {}
    vm.filterDish = {}
    vm.areaOrderFilter = {}
    vm.areaOrderFilter = {}

    vm.orderRateLowest = orderRateLowest
    vm.orderRateHighest = orderRateHighest
    vm.DishLowest = DishLowest
    vm.DishHighest = DishHighest
    vm.areaOrderLowest = areaOrderLowest
    vm.areaOrderHighest = areaOrderHighest

    function init() {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        vm.filter.from = firstDay;
        vm.filter.to = lastDay;

        vm.filterDish.from = firstDay;
        vm.filterDish.to = lastDay;

        vm.areaOrderFilter.from = firstDay;
        vm.areaOrderFilter.to = lastDay;

    }
    orderRateLowest();
    DishLowest(); 
    areaOrderLowest();
    vm.DishDataMining = DishDataMining

    function orderRateLowest() {
        vm.oRL = true
        $("#H").css('display', 'none');
        $("#L").css('display', 'block');
        $("#arrowH").css('display', 'none');
        $("#arrowL").css('display', 'contents');
        $('#noResultOrder').css('display', 'none');

        userService.getOrderRateL(vm.filter).then(function (response) {


            vm.orderRateL = response.data
            console.log(vm.orderRateL)
            $("#myChart").empty();
            if (vm.orderRateL.length > 0) {
                $("#myChart").empty();
                vm.oRL = false
                restName = []
                date = []
                for (var i = 0; i < vm.orderRateL.length; i++) {
                    restName[i] = vm.orderRateL[i].Name;
                    date[i] = vm.orderRateL[i].Count;
                }


                var ctx = document.getElementById("myChart");
                vm.myChart = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Orders',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barCha').css('display', 'block');

            } else {
                vm.oRL = false
                $('#noResultOrder').css('display', 'block');
            }
        });
    }

    function orderRateHighest() {
        vm.oRL = true
        $("#L").css('display', 'none');
        $("#H").css('display', 'block');
        $("#arrowL").css('display', 'none');
        $("#arrowH").css('display', 'contents');

        $('#noResultOrdeH').css('display', 'none');

        userService.getOrderRateH(vm.filter).then(function (response) {

            $('#noResultOrder').css('display', 'none');

            vm.orderRateH = response.data
            $("#myChart").empty();
            vm.myChart.destroy();

            if (vm.orderRateH.length > 0) {
                vm.oRL = false
                $("#myChart").empty();
                restName = []
                date = []
                for (var i = 0; i < vm.orderRateH.length; i++) {
                    restName[i] = vm.orderRateH[i].Name;
                    date[i] = vm.orderRateH[i].Count;
                }


                var ctx = document.getElementById("myChart");
                vm.myChart = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Orders',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barCha').css('display', 'block');

            } else {
                $("#barCha").css('display', 'none');
                vm.oRL = false
                $('#noResultOrder').css('display', 'block');
            }
        });
    }

    function DishLowest() {
        vm.oRL = true
        $("#DishH").css('display', 'none');
        $("#DishL").css('display', 'block');
        $("#arrowDishH").css('display', 'none');
        $("#arrowDishL").css('display', 'contents');
        $('#noResultDish').css('display', 'none');

        userService.getDishL(vm.filterDish).then(function (response) {

            vm.DishRL = false
            vm.DishL = response.data
            console.log(vm.DishL)
            $("#myChartD").empty();
            if (vm.DishL.length > 0) {
                $("#myChartD").empty();
                restName = []
                date = []
                for (var i = 0; i < vm.DishL.length; i++) {
                    restName[i] = vm.DishL[i].Name;
                    date[i] = vm.DishL[i].Count;
                }


                var ctx = document.getElementById("myChartD");
                vm.myChartD = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Users',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barChaDish').css('display', 'block');

            } else {
                vm.DishRL = false
                $('#noResultDish').css('display', 'block');
            }
        });
    }

    function DishHighest() {
        vm.oRL = true
        $("#DishL").css('display', 'none');
        $("#DishH").css('display', 'block');
        $("#arrowDishL").css('display', 'none');
        $("#arrowDishH").css('display', 'contents');
        $('#noResultDish').css('display', 'none');

        userService.getDishH(vm.filterDish).then(function (response) {

            vm.DishRL = false
            vm.DishH = response.data
            console.log(vm.DishH)
            $("#myChartD").empty();

            if (vm.DishH.length > 0) {
                $("#myChartD").empty();
                restName = []
                date = []
                for (var i = 0; i < vm.DishH.length; i++) {
                    restName[i] = vm.DishH[i].Name;
                    date[i] = vm.DishH[i].Count;
                }


                var ctx = document.getElementById("myChartD");
                vm.myChartD = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Users',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barChaDish').css('display', 'block');

            } else {
                vm.DishRL = false
                $('#noResultDish').css('display', 'block');
            }
        });
    }

    function areaOrderLowest() {
        vm.AreaRL = true
        $("#areaH").css('display', 'none');
        $("#areaL").css('display', 'block');
        $("#arrowAreaH").css('display', 'none');
        $("#arrowAreaL").css('display', 'contents');
        $('#noResultArea').css('display', 'none');

        userService.getAreaL(vm.areaOrderFilter).then(function (response) {

            vm.AreaRL = false
            vm.AreaL = response.data
            console.log(vm.AreaL)
            $("#myChartA").empty();
            if (vm.AreaL.length > 0) {
                $("#myChartA").empty();
                restName = []
                date = []
                for (var i = 0; i < vm.AreaL.length; i++) {
                    restName[i] = vm.AreaL[i].Name;
                    date[i] = vm.AreaL[i].Count;
                }


                var ctx = document.getElementById("myChartA");
                vm.myChartAO = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Orders',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barChaArea').css('display', 'block');

            } else {
                vm.AreaRL = false
                $('#noResultArea').css('display', 'block');
            }
        });
    }

    function areaOrderHighest() {
        vm.AreaRL = true
        $("#areaL").css('display', 'none');
        $("#areaH").css('display', 'block');
        $("#arrowAreaL").css('display', 'none');
        $("#arrowAreaH").css('display', 'contents');
        $('#noResultArea').css('display', 'none');

        userService.getAreaH(vm.areaOrderFilter).then(function (response) {
            debugger
            vm.AreaRL = false
            vm.AreaH = response.data
            console.log(vm.AreaH)
            $("#myChartA").empty();

            if (vm.AreaH.length > 0) {

                $("#myChartA").empty();
                restName = []
                date = []
                for (var i = 0; i < vm.AreaH.length; i++) {
                    restName[i] = vm.AreaH[i].Name;
                    date[i] = vm.AreaH[i].Count;
                }


                var ctx = document.getElementById("myChartA");
                vm.myChartAO = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        labels: restName,
                        //labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10", "2015-11", "2015-12"],
                        datasets: [{
                            label: 'Orders',
                            data: date,
                            //data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6, 2, 1],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
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
                                'rgba(255, 159, 64, 1)',
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    maxRotation: 90,
                                    minRotation: 80
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

                $('#barChaArea').css('display', 'block');

            } else {
                vm.AreaRL = false
                $('#noResultArea').css('display', 'block');
            }
        });
    }


    userService.DishForMining().then(function (response) {

        vm.Alldishmining = response.data;
        console.log(vm.Alldishmining)
    })

    function DishDataMining() {

        debugger
        userService.DishDataMining(vm.filterF).then(function (response) {
            debugger
            vm.Mining = response.data;
            if (vm.Mining.length > 0) {
                $("#noResultMin").css('display', 'none');
                $("#areaChart").css('display', 'block');

                count = []
                date = []
                for (var i = 0; i < vm.Mining.length; i++) {
                    date[i] = vm.Mining[i].Date;
                    count[i] = vm.Mining[i].Count;
                }


                var areaData = {
                    labels: date,
                    datasets: [{
                        label: '#',
                        data: count,
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
                        borderWidth: 1,
                        fill: 'origin', // 0: fill to 'origin'
                        fill: '+2', // 1: fill to dataset 3
                        fill: 1, // 2: fill to dataset 1
                        fill: false, // 3: no fill
                        fill: '-2' // 4: fill to dataset 2
                    }]
                };

                var areaOptions = {
                    plugins: {
                        filler: {
                            propagate: true
                        }
                    }
                }

                if ($("#areaChart").length) {
                    var areaChartCanvas = $("#areaChart").get(0).getContext("2d");
                    var areaChart = new Chart(areaChartCanvas, {
                        type: 'line',
                        data: areaData,
                        options: areaOptions
                    });
                }
            }


        })
    }



}