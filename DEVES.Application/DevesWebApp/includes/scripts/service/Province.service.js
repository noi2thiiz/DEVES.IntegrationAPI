(function() {
    'use strict';
    angular
        .module('app.core')
        .service('ProvinceService', ProvinceService);

    ProvinceService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function ProvinceService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_Province', pk: 'Code',
            "prop": {
                Code: {name: 'ProvinceCode'},
                Name: {name: 'ProvinceName'}
            }
        };
        var procName  = 'sp_Query_Province';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/