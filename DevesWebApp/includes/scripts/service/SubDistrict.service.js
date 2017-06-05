(function() {
    'use strict';
    angular
        .module('app.core')
        .service('SubDistrictService', SubDistrictService);

    SubDistrictService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function SubDistrictService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_SubDistrict', pk: 'Code',
            "prop": {
                Code: {name: 'SubDistrictCode'},
                Name: {name: 'SubDistrictName'}
            }
        };
        var procName  = 'sp_Query_SubDistrict';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/