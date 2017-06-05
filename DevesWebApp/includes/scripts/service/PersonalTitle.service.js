(function() {
    'use strict';
    angular
        .module('app.core')
        .service('PersonalTitleService', PersonalTitleService);

    PersonalTitleService.$inject = ['$baseResource','resourceManager','$q'];
    // memory service
    function PersonalTitleService($resource,resourceManager,$q){

        var CFG = {
            procname: 'sp_Query_PersonalTitle', pk: 'Code',
            "prop": {
                Code: {name: 'Code'},
                Name: {name: 'Name'}
            }
        };
        var procName  = 'sp_Query_PersonalTitle';
        var _resource = $resource(CFG);
        var manager = new resourceManager({resource:_resource,proc:CFG});
        return manager;

    }

})(); /*END*/