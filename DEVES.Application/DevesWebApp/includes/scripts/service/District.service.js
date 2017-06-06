(function() {
    'use strict';
    angular
        .module('app.core')
        .service('DistrictService', DistrictService);

    DistrictService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function DistrictService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_District', pk: 'Code',
            "prop": {
                Code: {name: 'DistrictCode'},
                Name: {name: 'DistrictName'}
            }
        };
        var procName  = 'sp_Query_District';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/