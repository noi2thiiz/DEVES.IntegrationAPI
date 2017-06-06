(function() {
    'use strict';
    angular
        .module('app.core')
        .service('CountryService', CountryService);

    CountryService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function CountryService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_Nationalities', pk: 'Code',
            "prop": {
                Code: {name: 'Code'},
                Name: {name: 'Name'}
            }
        };
        var procName  = 'sp_Query_Country';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/