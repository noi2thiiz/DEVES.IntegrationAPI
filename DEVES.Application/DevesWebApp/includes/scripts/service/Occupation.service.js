(function() {
    'use strict';
    angular
        .module('app.core')
        .service('OccupationService', OccupationService);

    OccupationService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function OccupationService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_Occupation', pk: 'Code',
            "prop": {
                Code: {name: 'Code'},
                Name: {name: 'Name'}
            }
        };
        var procName  = 'sp_Query_Occupation';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/