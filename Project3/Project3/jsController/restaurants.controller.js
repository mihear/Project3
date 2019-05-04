﻿app.service('restaurantsService', restaurantsService);
function restaurantsService($http) {
    this.getOrderRateL = function getOrderRateL(filter) {

        return $http.post('/api/DimRestaurants/OrderRateL', filter);
    };
    this.getOrderRateH = function getOrderRateH(filter) {

        return $http.post('/api/DimRestaurants/OrderRateH', filter);
    };
    this.getAllRestaurant = function getAllRestaurant() {
        return $http.get('/api/DimRestaurants/getAllRestaurants');
    }
    this.getDishL = function getDishL(filter) {

        return $http.post('/api/DimDishes/OrderRateL', filter);
    };
    this.getDishH = function getDishH(filter) {
        debugger
        return $http.post('/api/DimDishes/OrderRateH', filter);
    };
    this.getAllDish = function getAllDish() {
        return $http.get('/api/DimDishes/getAllDish');
    }



}

app.controller('restaurantsController', restaurantsController);
function restaurantsController(restaurantsService) {

    vm = this;
    vm.init = init
    vm.oRL = true
    vm.DishRL = true
    vm.filter = {}
    vm.filterDish = {}

    vm.orderRateLowest = orderRateLowest
    vm.orderRateHighest = orderRateHighest

    vm.DishLowest = DishLowest
    vm.DishHighest = DishHighest

    function init() {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        vm.filter.from = firstDay;
        vm.filter.to = lastDay;

        vm.filterDish.from = firstDay;
        vm.filterDish.to = lastDay;
    }
    orderRateLowest();
    DishLowest();


    function orderRateLowest() {
        vm.oRL = true
        $("#H").css('display', 'none');
        $("#L").css('display', 'block');
        $("#arrowH").css('display', 'none');
        $("#arrowL").css('display', 'contents');
        $('#noResultOrder').css('display', 'none');

        restaurantsService.getOrderRateL(vm.filter).then(function (response) {

            vm.oRL = false
            vm.orderRateL = response.data
            console.log(vm.orderRateL)
            $("#myChart").empty();
            if (vm.orderRateL.length > 0) {

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
                            label: 'Restaurant',
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

        restaurantsService.getOrderRateH(vm.filter).then(function (response) {

            $('#noResultOrder').css('display', 'none');
            vm.oRL = false
            vm.orderRateH = response.data
            $("#myChart").empty();
            vm.myChart.destroy();

            if (vm.orderRateH.length > 0) {

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
                            label: 'Restaurant',
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

        restaurantsService.getDishL(vm.filterDish).then(function (response) {

            vm.DishRL = false
            vm.DishL = response.data
            console.log(vm.DishL)
            $("#myChartD").empty();
            if (vm.DishL.length > 0) {

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
                            label: 'Dishs',
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
        debugger
        restaurantsService.getDishH(vm.filterDish).then(function (response) {
            debugger
            vm.DishRL = false
            vm.DishH = response.data
            console.log(vm.DishH)
            $("#myChartD").empty();
            if (vm.DishH.length > 0) {

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
                            label: 'Dishs',
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






    restaurantsService.getAllRestaurant().then(function (response) {

        vm.AllRestaurants = response.data;
    })



}