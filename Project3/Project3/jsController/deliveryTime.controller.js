app.service('deliveryTimeService', deliveryTimeService);
function deliveryTimeService($http) {
    this.getOnOffOrder = function getOnOffOrder(filter) {
        return $http.post('/api/DimRestaurants/OnOffOrder', filter);
    };
}

app.controller('deliveryTimeController', deliveryTimeController);
function deliveryTimeController(deliveryTimeService) {

    vm = this;
    vm.init = init
    vm.OnOffFilter = {}
    vm.OnOffOrder = OnOffOrder

    function init() {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        vm.OnOffFilter.from = firstDay;
        vm.OnOffFilter.to = lastDay;
    }

    OnOffOrder()
    function OnOffOrder() {
        debugger
        deliveryTimeService.getOnOffOrder(vm.OnOffFilter).then(function (response) {
            debugger
            vm.OnOff = response.data;
        })

    }
}
