

app.service('facebookService', facebookService);
function facebookService($http) {
    this.getData = function getData() {

        return $http.get('/R.json')
    };
}
app.controller('facebookController', facebookController);

function facebookController(facebookService) {

    var vm = this;
    facebookService.getData().then(function (response) {

        vm.fData = response.data;
        vm.PosNeg = []
        console.log(vm.fData)
        var x = 0;
        var x1 = 0;
        var y = 0;
        var y1 = 0;
        debugger
        for (var i = 0; i < vm.fData.length; i++) {
            if (vm.fData[i].isPositive == "1") {
                x++
                var c = vm.fData[i].comments;
                var c1 = c.split(" ");
                x1 += parseInt(c1[0]);

            }
            else {
                y++
                var c = vm.fData[i].comments;
                var c1 = c.split(" ");
                y1 += parseInt(c1[0]);
            }
                
        }
        vm.x1 = x1;
        vm.y1 = y1;

        vm.PosNeg.push(x);
        vm.PosNeg.push(y);
        console.log(vm.PosNeg)



        var data = {
            labels: ["Pos", "Neg"],
            datasets: [{
                label: '# of Votes',
                data: vm.PosNeg,
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

        // Get context with jQuery - using jQuery's .get() method.
        if ($("#barChart").length) {
            var barChartCanvas = $("#barChart").get(0).getContext("2d");
            // This will get the first returned node in the jQuery collection.
            var barChart = new Chart(barChartCanvas, {
                type: 'bar',
                data: data,
                options: options
            });
        }


        var doughnutPieData = {
            datasets: [{
                data: vm.PosNeg,
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
            labels: ["Pos Comment :" + vm.x1 +"  " , "Neg Comment :" + vm.y1  + "  "]
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




}








