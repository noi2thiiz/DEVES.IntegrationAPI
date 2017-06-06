/**
 * ใช้สำหรับ Selectbox ที่เป็นตัวเลือกแบบปกติ
 * ใช้คู่กับ ui-lookup-select.pug  และ ui-lookup-select-col-12.pug
 * ใช้คู่กับ ui-lookup-select.pug  และ ui-lookup-select-col-12.pug
 * ตัวอย่าง
 * - var cfg = {"service":"LutAttTypeService","param":"PI_CURR_ATT_ID","label":"ระดับการยอมรับและความเข้าใจ","vm":"lvm","placeholder":"เลือกระดับการยอมรับและความเข้าใจ" ,"defaultText":"ทั้งหมด"}
 * include ../../templates/ui-lookup-select-col-12.pug
 */
(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('LookupSelectController', LookupSelectController);

    LookupSelectController.$inject = ['$scope','Memory', '$injector', '$rootScope', '$resource', '$controller', '$window', '$q'];
    function LookupSelectController($scope,Memory, $injector, $rootScope, $resource, $controller, $window, $q) {

        var vm = this;
        vm.selected = {};
        vm.selectedId = null;
        vm.items = [];
        vm.paramName = "";
        vm.model = {};
        vm.placeholder = "กรุณาเลือก";
        vm.placeholderTmp = "กรุณาเลือก";
        vm.defaultSelected = [];
        vm.dependToParam =[];
        vm.disabled = true;
        vm.disbledOnShow = false;
        vm.required = false;


        function _unselect() {
            vm.selectedId = null;
            vm.selected = {};
            vm.disabled = true;

        }

        function _load(dataService, item) {
            if(!item){
                item = {CODE:""};
                var valid = true;
            }else {
                var valid = false;
            }

            _unselect();
            //alert(JSON.stringify(item));
            var selectedId = $.trim(item.CODE);
            vm.selected = {};
            vm.disabled = true;
            vm.placeholder = "กำลังโหลด....";
            var items = [];
            var options = [];
            dataService.fetchAll().then(function (response) {
                items = response.data;
                console.log(items);

                angular.forEach(items, function (val, key) {
                    //console.log("forEach items = "+JSON.stringify(val));
                    options.push({CODE:$.trim(val.Code),DESCR:$.trim(val.Name)});
                    if (selectedId == $.trim(val.Code)) {
                        vm.selected = {CODE:$.trim(val.Code),DESCR:$.trim(val.Name)};
                        valid = true;
                       // console.log("set selectedId = "+vm.selectedId);
                    }
                });

                if(!valid){
                    // alert("ไม่พบ : "+ selectedId);
                }else{
                    vm.selectedId = angular.copy(selectedId);
                    // alert("พบ : "+ selectedId );
                }

                if (items.length <= 0) {

                    vm.placeholder = "ไม่พบข้อมูล";
                    items.push({CODE:"",DESCR:"ไม่พบข้อมูล"});

                } else {
                    vm.placeholder = angular.copy(vm.placeholderTmp);

                    if( vm.required != true ){
                        items.push({CODE:"",DESCR:"เลือกทั้งหมด"});
                    }

                }

                if(vm.disbledOnShow==true){
                    vm.disabled = true;
                }else{
                    vm.disabled = false;
                }



                //console.log(options);
                vm.items = options;
                vm.deferred.resolve({});

            });
        };





        vm.init = function (deferred, dataServiceName, paramName, placeholder, defaultSelected,disbledOnShow,dependOnParam,model,filter,dependToParam,required) {

            vm.deferred = $q.defer();
            _unselect();

            if(dependOnParam=='null'){
                dependOnParam = undefined;
            }
            if(dependOnParam==null){
                dependOnParam = undefined;
            }

            vm.placeholderTmp = angular.copy(placeholder);
            vm.placeholder = angular.copy(placeholder);
            vm.defaultSelected = angular.copy(defaultSelected);
            vm.disbledOnShow = disbledOnShow ;
            vm.dependToParam = dependToParam ;
            vm.dependOnParam = dependOnParam;
            vm.required = required;

            console.log('init '+dataServiceName);
            if(true){
                deferred.promise.then(function (item) {
                    //console.warn("===========  deferred.promise===========")
                    //console.warn($rootScope.dataItem)




                    $injector.invoke([dataServiceName, function (dataService) {
                        console.log('$injector '+dataServiceName);

                        vm.paramName = paramName;
                        vm.dataService = dataService;

                        if($rootScope.dataItem){
                            if($rootScope.dataItem[paramName]){
                                var  item =  {CODE: $rootScope.dataItem[paramName] } ;

                            }

                        }

                        _load(dataService, item);
                    }]);



                });
            }


        };





    };

})();