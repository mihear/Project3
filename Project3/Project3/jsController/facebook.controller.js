

app.service('facebookService', facebookService);
function facebookService($http) {
    this.TopEffectingUser = function TopEffectingUser() {
        
        return $http.post('/api/FaceBook/TopEffectingUser')
    };
    this.TopResturnt = function TopResturnt() {
        
        return $http.post('/api/FaceBook/TopResturnt')
    };
    this.TopActivityPost = function TopActivityPost() {
        
        return $http.post('/api/FaceBook/TopActivityPost')
    };
    this.LessActivityPost = function LessActivityPost() {
        
        return $http.post('/api/FaceBook/LessActivityPost')
    };
    this.Positive = function Positive() {
        
        return $http.post('/api/FaceBook/Positive')
    };
    this.Negative = function Negative() {
        
        return $http.post('/api/FaceBook/Negative')
    };
}
app.controller('facebookController', facebookController);

function facebookController(facebookService) {

    var vm = this;
    vm.loader1 = true
    vm.loader2 = true

    facebookService.TopEffectingUser().then(function (response) {
        
        $('#myChart').css('display', 'block');
        vm.loader1 = false
        


        vm.TopEffectingUser = response.data;
        restName = []
        date = []
        for (var i = 0; i < vm.TopEffectingUser.length; i++) {
            restName[i] = vm.TopEffectingUser[i].Name;
            date[i] = vm.TopEffectingUser[i].Count;
        }
        console.log(vm.TopEffectingUser)
        
        var ctx = document.getElementById("myChart");
        vm.myChart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: restName,
                datasets: [{
                    label: 'Users',
                    data: date,
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


    })

    facebookService.TopResturnt().then(function (response) {
        
        $('#myChart1').css('display', 'block');
        vm.loader2 = false



        vm.TopResturnt = response.data;
        restName = []
        date = []
        for (var i = 0; i < vm.TopResturnt.length; i++) {
            restName[i] = vm.TopResturnt[i].Name;
            date[i] = vm.TopResturnt[i].Count;
        }
        console.log(vm.TopResturnt)
        
        var ctx = document.getElementById("myChart1");
        vm.myChart1 = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: restName,
                datasets: [{
                    label: 'Resturant',
                    data: date,
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


    })

    facebookService.TopActivityPost().then(function (response) {

        vm.TopActivityPost = response.data;

        console.log("r", vm.TopActivityPost)
    })

    facebookService.LessActivityPost().then(function (response) {

        vm.LessActivityPost = response.data;

        console.log("r", vm.LessActivityPost)
    })

    PN()
    function PN() {
        debugger
        vm.data = []
        facebookService.Positive().then(function (response) {
            vm.Positive = response.data;
            vm.data[0] = vm.Positive

            facebookService.Negative().then(function (response) {
                vm.Negative = response.data;
                vm.data[1] = vm.Negative


                vm.all = vm.Positive + vm.Negative




                var doughnutPieData = {
                    datasets: [{
                        data: vm.data,
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
                    labels: ['Positive Post','Negative Post']
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

        })




    }

}








