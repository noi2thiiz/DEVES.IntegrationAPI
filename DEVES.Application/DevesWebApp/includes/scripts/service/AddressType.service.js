(function() {
    'use strict';
    angular
        .module('app.core')
        .service('AddressTypeService', AddressTypeService);

    AddressTypeService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function AddressTypeService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_AddressType', pk: 'Code',
            "prop": {
                Code: {name: 'Code'},
                Name: {name: 'Name'}
            }
        };
        var procName  = 'sp_Query_AddressType';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/