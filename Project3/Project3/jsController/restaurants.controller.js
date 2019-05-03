app.service('restaurantsService', restaurantsService);
function restaurantsService($http) {
    this.getOrderRateL = function getOrderRateL(filter) {
        debugger
        return $http.post('/api/DimRestaurants/OrderRateL', filter);
    };
    this.getAllRestaurant = function getAllRestaurant() {
        return $http.get('/api/DimRestaurants/getAllRestaurants');
    }

    
}

app.controller('restaurantsController', restaurantsController);
function restaurantsController(restaurantsService) {

    vm = this;
    vm.init = init
    vm.oRL = true
    vm.filter = {}
    vm.orderRate = orderRate
    function init() {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);


        vm.filter.from = firstDay;
        vm.filter.to = lastDay;
    }
    orderRate();
    function orderRate() {
        debugger
        restaurantsService.getOrderRateL(vm.filter).then(function (response) {
            debugger
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
                var myChart = new Chart(ctx, {
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
                $('#noResultOrder').css('display', 'none');
                $('#barCha').css('display', 'block');
                
            } else {
                vm.oRL = false
                $('#noResultOrder').css('display', 'block');
            }
        });
    }

    restaurantsService.getAllRestaurant().then(function (response) {

        vm.AllRestaurants = response.data;
    })



}